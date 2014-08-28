# Creates an IIFE to define the closure of functions for the controller
do (()->

    # Groups prediction objects by a grouping predicate function.
    # It uses LoDash's groupBy function.
    # It mutates the input by setting a groupBy key to the cluster it
    # belongs to.
    setGroupBy = (grouping, predictions) -> (
        clustered = _.groupBy(predictions, grouping)
        id = 0
        _.forEach(clustered, (matches, key) ->
                for match in matches
                    match['groupBy'] = id
                id++
            )
        return predictions
        );

    applyFiltrex = (predictions, filtrexExpr) ->
        filt = compileExpression(filtrexExpr)
        console.log(filt, filt.js)
        filterResults = _.map(predictions, filt)
        passed = []
        for i in [0...predictions.length]
            if filterResults[i] is 1
                passed.push(predictions[i])
        return passed

    filterByFiltrex = ($scope, orderBy) ->
        expr = $scope.params.filtrexExpr.toLowerCase()
        for column, key of $scope.headerSubstituitionDictionary
                expr = expr.replace(new RegExp(column, "g"), key)
        try
            console.log(expr)
            filteredPredictions = applyFiltrex($scope._predictions, expr)
            throw new Error "Incomplete Expression" if filteredPredictions.length is 0 and $scope._predictions.length is not 0
            orderedResults = orderBy(filteredPredictions, ["MS1_Score", "Obs_Mass", "MS2_Score"])
            groupedResults = if $scope.groupByKey? then setGroupBy($scope.groupByKey, orderedResults) else orderedResults
            $scope.predictions = groupedResults
            $scope.filtrexError = false
            return groupedResults
            #console.log("Complete")
        catch ex
            console.log "in catch"
            console.log(ex, $scope.filtrexError)
            $scope.filtrexError = true if expr.length > 0
            if expr.length is 0
                $scope.predictions = $scope._predictions


    updateFiltrexDebounce = _.debounce ($scope, orderBy) ->
            $scope.$apply -> filterByFiltrex($scope, orderBy)
        10000

    # This controller's main data may be injected by external sources at any time.
    # Watch the injection site for changes and perform the most up-to-date filtering
    # and processing on this data when it arrives
    watchExternalDataChanges = ($scope, $window, orderBy) ->
        $scope.$watch("_predictionsReceiver", (newVal, oldVald) ->
            console.log(arguments)
            $scope._predictions = orderBy(newVal, ["MS1_Score", "Obs_Mass", "-MS2_Score"])
            filteredPredictions = filterByFiltrex($scope, orderBy)
            filteredPredictions = $scope._predictions if not filteredPredictions?
            groupedPredictions = $scope.setGroupBy($scope.params.currentGroupingRule.groupByKey, filteredPredictions)
            $scope.predictions = groupedPredictions
            return true
        false)

    # Scrolls the ngGrid instance to a given row index
    focusRow = ($scope, targetRowIndex) ->
        grid = $scope.gridOptions.ngGrid
        position = (grid.rowMap[targetRowIndex] * grid.config.rowHeight)
        console.log(grid.$viewport)
        grid.$viewport.scrollTop(position)
        console.log(grid.$viewport)

    # Start-up logic for the Controller
    activateFn = ($scope, $window, $filter) ->
        orderBy = $filter("orderBy")
        $scope.deregisterWatcher = watchExternalDataChanges($scope, $window, orderBy)
        $scope.$watch("params.filtrexExpr", -> updateFiltrexDebounce($scope, orderBy))
        $scope.$on("selectedPredictions", (evt, params) ->
            try
                $scope.gridOptions.selectAll(false)
                for glycopeptide in params.selectedPredictions
                    index = (_.findIndex($scope.predictions, {"Glycopeptide_identifier": glycopeptide.Glycopeptide_identifier}))
                    $scope.gridOptions.selectRow(index, true)
            )
        $scope.$on("ambiguityPlot.requestPredictionsUpdate", (evt, params) -> $scope.sendRenderPlotEvt())
        $scope.headerSubstituitionDictionary = $scope.buildHeaderSubstituitionDictionary()
        console.log("Activation Complete")

    helpText = {

        filtrex: '<article class="help-article"><h3>Filtrex</h3><h4>Expressions</h4><p>There are only 2 types: numbers and strings.</p><table><tbody><table class="table"><thead><tr><th>Numeric arithmetic</th><th>Description</th></tr></thead><tbody><tr><td>x + y</td><td>Add</td></tr><tr><td>x - y</td><td>Subtract</td></tr><tr><td>x * y</td><td>Multiply</td></tr><tr><td>x / y</td><td>Divide</td></tr><tr><td>x % y</td><td>Modulo</td></tr><tr><td>x ^ y</td><td>Power</td></tr></tbody></table><table class="table"><thead><tr><th>Comparisons</th><th>Description</th></tr></thead><tbody><tr><td>x == y</td><td>Equals</td></tr><tr><td>x &lt; y</td><td>Less than</td></tr><tr><td>x &lt;= y</td><td>Less than or equal to</td></tr><tr><td>x &gt; y</td><td>Greater than</td></tr><tr><td>x &gt;= y</td><td>Greater than or equal to</td></tr><tr><td>x in (a, b, c)</td><td>Equivalent to (x == a or x == b or x == c)</td></tr><tr><td>x not in (a, b, c)</td><td>Equivalent to (x != a and x != b and x != c)</td></tr></tbody></table><table class="table"><thead><tr><th>Boolean logic</th><th>Description</th></tr></thead><tbody><tr><td>x or y</td><td>Boolean or</td></tr><tr><td>x and y</td><td>Boolean and</td></tr><tr><td>not x</td><td>Boolean not</td></tr><tr><td>x ? y : z</td><td>If boolean x, value y, else z</td></tr></tbody></table><p>Created by Joe Walnes, <a href="https://github.com/joewalnes/filtrex"><br/>(See https://github.com/joewalnes/filtrex for more usage information.)</a></p></article>'

    }

    filterRules = {
        requirePeptideBackboneCoverage: {
            label: "Require Peptide Backbone Fragment Ions Matches"
            filtrex: "Mean Peptide Coverage > 0"
        }
        requireStubIons: {
            label: "Require Stub Ions Matches"
            filtrex: "Stub Ion Count > 0"
        }
        requireIonsWithHexNAc: {
            label: "Require Peptide Backbone Ion Fragmentss with HexNAc Matches"
            filtrex: "(% Y Ion With HexNAc Coverage + % B Ion With HexNAc Coverage) > 0"
        }
    }

    groupingRules = {
        ms1ScoreObsMass: {
            label: "Group ion matches by MS1 Score and Observed Mass (Ambiguous Matches)"
            groupByKey: (x) -> [x.MS1_Score, x.Obs_Mass]
        }
        startAALength: {
            label: "Group ion matches by the starting amino acid index and the peptide length (Heterogeneity)"
            groupByKey: (x) -> [x.startAA, x.peptideLens]
        }

    }

    angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").controller(
        "ClassifierResultsTableCtrl", [ "$scope", "$window", '$filter', 'csvService',
        ($scope, $window, $filter, csvService) ->
            orderBy = $filter("orderBy")
            $scope.helpText = helpText
            $scope.filterRules = filterRules
            $scope.groupingRules = groupingRules

            $scope.predictions = []
            $scope._predictions = []
            $scope._predictionsReceiver = []

            $scope.params = {}
            $scope.headerSubstituitionDictionary = {}

            $scope.params.filtrexExpr = "MS2 Score > 0.2"
            $scope.params.currentGroupingRule = $scope.groupingRules.ms1ScoreObsMass

            $scope.groupByKey = null
            $scope.deregisterWatcher = null

            $scope.ping = (args) -> console.log("ping", arguments, $scope)

            $scope.extendFiltrex = (expr) ->
                console.log "Extending Filtrex with #{expr}"
                if $scope.params.filtrexExpr.length > 0
                    $scope.params.filtrexExpr += " and " + expr
                else
                    $scope.params.filtrexExpr += expr

            $scope.filterByFiltrex = ->
                dictionary = $scope.substituteHeaders()
                expr = $scope.params.filtrexExpr.toLowerCase()
                for column, key of dictionary
                    expr = expr.replace(column, key)
                console.log expr
                results = applyFiltrex($scope._predictions, expr)
                # Sort without negating MS2_Score because the data is pre-sorted and does not need to be
                # reversed.
                orderedResults = orderBy(results, ["MS1_Score", "Obs_Mass", "MS2_Score"])
                groupedResults = setGroupBy($scope.groupByKey, orderedResults)
                return groupedResults

            $scope.sendRenderPlotEvt = () -> $scope.$broadcast("ambiguityPlot.renderPlot", {predictions: $scope.predictions})
            $scope.sendUpdateProteinViewEvt = () -> $scope.$broadcast("proteinSequenceView.updateProteinView", {predictions: $scope.predictions})
            $scope.setGroupBy = (grouping, predictions = null) ->
                $scope.groupByKey = grouping
                setGroupBy(grouping, predictions)

            $scope.scrollToSelection = ->
                if $scope.gridOptions.$gridScope? and $scope.gridOptions.$gridScope.selectedItems?
                    selectedItems = $scope.gridOptions.$gridScope.selectedItems
                    topIndex = Infinity # The index at the top of the selection (nearest to 0)
                    for glycopeptide in selectedItems
                        index = (_.findIndex($scope.predictions, {"Glycopeptide_identifier": glycopeptide.Glycopeptide_identifier}))
                        topIndex = index if index < topIndex
                    if topIndex is Infinity
                        return false
                    focusRow($scope, topIndex)

            $scope.buildHeaderSubstituitionDictionary = ->
                dictionary = {}
                dictionary.NAME_MAP = []
                BLACK_LIST = ["Peptide Span"]
                for column in $scope.gridOptions.columnDefs
                    if not (column.displayName in BLACK_LIST)
                        dictionary.NAME_MAP.push column.displayName
                        dictionary[column.displayName.toLowerCase()] = column.field
                dictionary["Start AA".toLowerCase()] = "startAA"
                dictionary.NAME_MAP.push "Start AA"
                dictionary["End AA".toLowerCase()] = "endAA"
                dictionary.NAME_MAP.push "End AA"
                dictionary["AA Length".toLowerCase()] = "peptideLens"
                dictionary.NAME_MAP.push "AA Length"
                dictionary["Oxonium Ion Count".toLowerCase()] = "numOxIons"
                dictionary.NAME_MAP.push "Oxonium Ion Count"
                dictionary["Stub Ion Count".toLowerCase()] = "numStubs"
                dictionary.NAME_MAP.push "Stub Ion Count"
                dictionary["% Y Ion Coverage".toLowerCase()] = "percent_y_ion_coverage"
                dictionary.NAME_MAP.push "% Y Ion Coverage"
                dictionary["% B Ion Coverage".toLowerCase()] = "percent_b_ion_coverage"
                dictionary.NAME_MAP.push "% B Ion Coverage"
                dictionary["% Y Ion With HexNAc Coverage".toLowerCase()] = "percent_y_ion_with_HexNAc_coverage"
                dictionary.NAME_MAP.push "% Y Ion With HexNAc Coverage"
                dictionary["% B Ion With HexNAc Coverage".toLowerCase()] = "percent_b_ion_with_HexNAc_coverage"
                dictionary.NAME_MAP.push "% B Ion With HexNAc Coverage"

                return dictionary


            $scope.gridOptions = {
                data: "predictions"
                showColumnMenu: true
                showFilter: false
                enableSorting: false
                enableHighlighting: true
                enablePinning: true
                rowHeight: 90

                columnDefs:[
                    {
                        field:'MS2_Score'
                        width:90
                        pinned: true
                        displayName:"MS2 Score"
                        cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
                    }
                    {
                        field:'MS1_Score'
                        width:90
                        pinned: true
                        displayName:"MS1 Score"
                        cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
                    }
                    {
                        field:'Obs_Mass'
                        width:130
                        pinned: true
                        displayName:"Observed Mass"
                        cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
                    }
                    {
                        field:'vol'
                        width:100
                        pinned: true
                        displayName:"Volume"
                        cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
                    }
                    {
                        field:'ppm_error'
                        width:90
                        displayName:"PPM Error"
                        cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|scientificNotation|number:4}}</div></div>'
                    }
                    {
                        field:'Glycopeptide_identifier'
                        width: 240
                        displayName:"Glycopeptide Sequence"
                        cellClass: "matched-ions-cell glycopeptide-identifier"
                        cellTemplate: '<div><div class="ngCellText" ng-bind-html="row.getProperty(col.field)|highlightModifications"></div></div>'
                    }
                    {
                        field:'meanCoverage'
                        width:180
                        displayName:"Mean Peptide Coverage"
                        cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
                    }
                    {
                        field:'percentUncovered'
                        width:165
                        displayName:"% Peptide Uncovered"
                        cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field) * 100|number:2}}</div></div>'
                    }
                    {
                        field: "startAA"
                        width: 180
                        displayName: "Peptide Span"
                        cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)}}-{{row.entity.endAA}}&nbsp;({{row.entity.peptideLens}})</div></div>'
                    }
                    {
                        field:'Oxonium_ions',
                        width: 200
                        headerClass: null
                        displayName:"Oxonium Ions"
                        cellClass: "stacked-ions-cell-grid"
                        cellTemplate:
                                    '<div>
                                        <div class="ngCellText">
                                            <div class="coverage-text">{{row.entity.numOxIons}} Ions Matched</div>
                                            <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion>
                                        </div>
                                    </div>'
                    }
                    {
                        field:'Stub_ions',
                        width: 340

                        displayName:"Stub Ions"
                        headerClass: null
                        cellClass: "stacked-ions-cell-grid"
                        cellTemplate:
                                    '<div>
                                        <div class="ngCellText">
                                            <div class="coverage-text">{{row.entity.numStubs}} Ions Matched</div>
                                            <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion>
                                        </div>
                                    </div>'
                    }
                    {
                        field:'b_ion_coverage',
                        width: 340
                        displayName:"B Ions"
                        headerClass: null
                        cellClass: "stacked-ions-cell-grid"
                        cellTemplate:
                                    '<div>
                                        <div class="ngCellText">
                                            <div class="coverage-text">{{row.entity.percent_b_ion_coverage * 100|number:1}}% Coverage</div>
                                            <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion>
                                        </div>
                                    </div>'
                    }
                    {
                        field:'y_ion_coverage',
                        width: 340
                        displayName:"Y Ions"
                        headerClass: null
                        cellClass: "stacked-ions-cell-grid"
                        cellTemplate:
                                    '<div>
                                        <div class="ngCellText">
                                            <div class="coverage-text">{{row.entity.percent_y_ion_coverage * 100|number:1}}% Coverage</div>
                                            <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion>
                                        </div>
                                    </div>'
                    }
                    {
                        field:'b_ions_with_HexNAc',
                        width: 340
                        displayName:"B Ions with HexNAc"
                        headerClass: null
                        cellClass: "stacked-ions-cell-grid"
                        cellTemplate:
                                    '<div>
                                        <div class="ngCellText">
                                            <div class="coverage-text">{{row.entity.percent_b_ion_with_HexNAc_coverage * 100 |number:1}}% Coverage</div>
                                            <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion>
                                        </div>
                                    </div>'
                    }
                    {
                        field:'y_ions_with_HexNAc',
                        width: 340
                        displayName:"Y Ions with HexNAc"
                        headerClass: null
                        cellClass: "stacked-ions-cell-grid"
                        cellTemplate:
                                    '<div>
                                        <div class="ngCellText">
                                            <div class="coverage-text">{{row.entity.percent_y_ion_with_HexNAc_coverage * 100|number:1}}% Coverage</div>
                                            <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion>
                                        </div>
                                    </div>'
                    }
                ]
                # Class setting in outer-most div interpolates color class from the prediction's groupBy
                rowTemplate: '<div style="height: 100%" class="c{{row.entity.groupBy % 6}}">
                                <div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell matched-ions-cell">
                                    <div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div>
                                        <div ng-cell>
                                    </div>
                                </div>
                            </div>'
            }



            activateFn($scope, $window, $filter)
            $window.ClassifierResultsTableCtrlInstance = $scope])
)