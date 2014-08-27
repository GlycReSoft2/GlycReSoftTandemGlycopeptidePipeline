# cluster-plot.coffee
# A directive using HighCharts to render clustered bubble plots about
# (MS1 Score, Mass) Observation Ambiguity
#
# Depends upon LoDash.js and Highcharts.js
#
angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").directive "ambiguityPlot", ["$window",  ($window) ->

    scalingDownFn = (value) -> Math.log(value)
    scalingUpFn = (value) -> Math.exp(value)

    # Injects the directive scope into the Highcharts.Chart instance's callbacks
    ambiguityPlotTemplater = (scope, seriesData, xAxisTitle, yAxisTitle) ->
        # Very small number for zooming to
        infitesimal = 1/(Math.pow(1000, 1000))
        ambiguityPlotTemplateImpl = {
            chart: {
                height: $window.innerHeight * 0.6
                type: "bubble"
                zoomType: 'xy'
            }
            plotOptions: {
                series: {
                    point: {
                        events: {
                            click: (evt) ->
                                point = this
                                chart = @series.chart
                                xs = _.pluck(@series.points, "x")
                                ys = _.pluck(@series.points, "y")
                                # Publish this data to the directive scope for use elsewhere
                                scope.$apply(() ->
                                    scope.describedPredictions = _.pluck(point.series.points, "data")
                                    )
                                console.log(this)
                                chart.xAxis[0].setExtremes(Math.min.apply(null, xs) * (1 - infitesimal), Math.max.apply(null, xs) * (1 + infitesimal))
                                chart.yAxis[0].setExtremes(Math.min.apply(null, ys) * (1 - infitesimal), Math.max.apply(null, ys) * (1 + infitesimal))
                                chart.showResetZoom()
                        } # Close events
                    } # Close point
                states: {
                        hover: false
                    }
                } # Close series
            } # Close plotOptions
            legend: {
                title: {
                    text: "<b>Legend</b> <small>(click series to hide)</small>"
                }
                align: 'right',
                verticalAlign: 'top',
                y: 60
                width: 200
                height: $window.innerHeight * .8
            },
            tooltip: {
                formatter: () ->
                    point = this.point
                    contents = " MS1 Score: <b>#{point.x}</b><br/>
                Mass: <b>#{scalingUpFn(point.z)}</b><br/>
                MS2 Score: <b>#{point.y}</b>(ME: <i>#{point.MS2_ScoreMeanError}</i>)<br/>
                Number of Matches: <b>#{point.series.data.length}</b><br/>
                "
                    return contents
                headerFormat: "<span style=\"color:{series.color}\">●</span> {series.name}</span><br/>"
                #Leading space is required to align the first character of each row.
                pointFormat: " MS1 Score: <b>{point.x}</b>" +
                "<br/>Mass: <b>{point.z}</b><br/>MS2 Score: <b>{point.y}</b>(ME: <i>{point.MS2_ScoreMeanError}</i>)<br/>
                Number of Matches: <b>{series.data.length}</b><br/>
                "
                positioner: (boxWidth, boxHeight, point) ->
                    ttAnchor = {x: point.plotX, y: point.plotY}
                    ttAnchor.x -= (boxWidth * 1)
                    if ttAnchor.x <= 0
                        ttAnchor.x += 2 * (boxWidth * 1)
                    return ttAnchor

            }
            title: {
                text: "Ambiguous Groups Plot"
            }
            xAxis: {
                title: {
                    text: xAxisTitle
                }
                events: {

                }
            }
            yAxis: {
                title: {
                    text: yAxisTitle
                }
            }
            series: seriesData
        } # Close template

    ms1MassGroupingFn = (predictions) ->
        ionPoints = ({x: p.MS1_Score, y: p.MS2_Score, z: scalingDownFn(p.Obs_Mass), data: p} for p in predictions)
        ionMassMS1Groups = _.groupBy ionPoints, (pred) -> pred.x.toFixed(3) + "-" + pred.z.toFixed(3)
        ionMassMS1Series = []
        notAmbiguous = []
        perfectAmbiguous = []
        _.forEach ionMassMS1Groups,
            (group, id) ->
                if(group.length == 1)
                    _.forEach(group, (pred) -> pred.MS2_ScoreMeanError = 0)
                    notAmbiguous.push {data: group, name: "MS1/Mass " + id}
                else # Calculate the distance from the mean of the MS2 Scores in this series
                    scoreRange = _.pluck(group, "y")
                    mean = 0
                    for s in scoreRange
                        mean += s
                    mean /= scoreRange.length

                    meanError = ((s - mean) for s in scoreRange)
                    for i in [0...group.length]
                        group[i].MS2_ScoreMeanError = if meanError[i] == 0 then 0 else meanError[i].toFixed(4)

                    ionMassMS1Series.push {data: group, name: "MS1/Mass " + id}
        return {ionSeries: ionMassMS1Series, notAmbiguous: notAmbiguous}

    positionGroupingFn = (predictions) ->
        ionPoints = ({x: p.startAA, y: p.MS2_Score, z: p.endAA - p.startAA, data: p} for p in predictions)
        ionStartLengthGroups = _.groupBy ionPoints, (pred) -> pred.x.toFixed(3) + "-" + pred.z.toFixed(3)
        ionStartLengthSeries = []
        notAmbiguous = []
        perfectAmbiguous = []
        _.forEach ionStartLengthGroups,
            (group, id) ->
                if(group.length == 1)
                    _.forEach(group, (pred) -> pred.MS2_ScoreMeanError = 0)
                    ionStartLengthSeries.push {data: group, name: "Start AA/Length  " + id}
                else # Calculate the distance from the mean of the MS2 Scores in this series
                    scoreRange = _.pluck(group, "y")
                    mean = 0
                    for s in scoreRange
                        mean += s
                    mean /= scoreRange.length

                    meanError = ((s - mean) for s in scoreRange)
                    for i in [0...group.length]
                        group[i].MS2_ScoreMeanError = if meanError[i] == 0 then 0 else meanError[i].toFixed(4)

                    ionStartLengthSeries.push {data: group, name: "Start AA/Length " + id}
        return {ionSeries: ionStartLengthSeries, notAmbiguous: notAmbiguous}


    updatePlot = (predictions, scope, element) ->
        # Get grouping configuration bound from UI
        groupParams = scope.grouping.groupingFnKey
        console.log "Grouping Parameters: ", groupParams
        # Generate grouped series data
        scope.seriesData = groupParams.groupingFn(predictions)
        console.log("Series Data: ", scope.seriesData)
        scope.describedPredictions = []
        {ionSeries, notAmbiguous} = scope.seriesData
        # Initialize the plot template object, passing grouping labels
        plotOptions = ambiguityPlotTemplater(scope, ionSeries, xAxisTitle=groupParams.xAxisTitle,
            yAxisTitle=groupParams.yAxisTitle)
        # Render the plot using HighCharts
        console.log(plotOptions)
        chart = element.find(".ambiguity-plot-container")
        chart.highcharts(plotOptions)

    return {
            restrict: "AE"
            template: "
            <div class='amiguity-container'>
                <div class='plot-grouping-fn-selector-container'>
                    <select class='plot-grouping-fn-selector-box' ng-model='grouping.groupingFnKey'
                        ng-options='key for (key, value) in grouping.groupingsOptions' ng-change='requestPredictionsUpdate()'>
                    </select>
                </div>
                <div class='ambiguity-plot-container'></div>
                <div class='ambiguity-peptide-sequences-container' ng-if='describedPredictions.length > 0'>
                    <div class='ambiguity-peptide-attributes-container clearfix'>
                    <div class='pull-left ambiguity-peptide-attributes'>
                        <p>MS2 Score Range: {{describedMS2Min}} - {{describedMS2Max}}</p>
                        <p>Peptide Region: {{describedPredictions[0].startAA}} - {{describedPredictions[0].endAA}}</p>
                    </div>
                    <div class='pull-left ambiguity-peptide-attributes'>
                        <p>Peptide Sequence: {{describedPredictions[0].Peptide}}</p>
                        <p>Distinct Glycan Count: {{keys(_.groupBy(describedPredictions, 'Glycan')).length}}
                    </div>
                    </div>
                    <table class='table table-striped table-compact ambiguity-peptide-sequences-table'>
                        <tr>
                            <th>Glycopeptide Identifier</th>
                            <th>Peptide Coverage</th>
                            <th># Stub Ions</th>
                            <th>B | Y Ions Coverage (+HexNAc)</th>
                            <th>MS2 Score</th>
                        </tr>
                        <tr ng-repeat='match in describedPredictions | orderBy:[\"MS2_Score\",\"Glycan\",\"numStubs\"]:true'>
                            <td ng-bind-html='match.Glycopeptide_identifier | highlightModifications'></td>
                            <td>{{match.meanCoverage | number:4}}</td>
                            <td>{{match.Stub_ions.length}}</td>
                            <td>{{match.percent_b_ion_coverage * 100|number:1}}%({{match.percent_b_ion_with_HexNAc_coverage * 100|number:1}}%) |
                                {{match.percent_y_ion_coverage * 100|number:1}}%({{match.percent_y_ion_with_HexNAc_coverage * 100|number:1}}%)
                                </td>
                            <td>{{match.MS2_Score}}</td>
                        </tr>
                    </table>
                </div>
            </div>
                "
            link: (scope, element, attr) ->
                scope.describedPredictions = []
                scope.describedMS2Min = 0
                scope.describedMS2Max = 0
                scope.grouping = {}
                scope.grouping.groupingsOptions = {
                    "MS1 Score + Mass": {
                        groupingFn: ms1MassGroupingFn
                        xAxisTitle: "MS1 Score"
                        yAxisTitle: "MS2 Score"
                    }
                    "Start AA + Length": {
                        groupingFn: positionGroupingFn
                        xAxisTitle: "Peptide Start Position"
                        yAxisTitle: "MS2 Score"
                    }
                }
                scope._ = _ # let lodash be used in expressions
                scope.keys = Object.keys
                scope.grouping.groupingFnKey = scope.grouping.groupingsOptions["MS1 Score + Mass"]

                angular.element($window).bind 'resize', ->
                    try
                        chart = element.find(".ambiguity-plot-container").highcharts()
                        chart.setSize($window.innerWidth, $window.innerHeight * 0.6)

                scope.$watch("describedPredictions", (newVal) ->
                        scoreRange = _.pluck(scope.describedPredictions, 'MS2_Score')
                        scope.describedMS2Min = Math.min.apply(null, scoreRange)
                        scope.describedMS2Max = Math.max.apply(null, scoreRange)
                        scope.$emit("selectedPredictions", {selectedPredictions: scope.describedPredictions})
                    )
                scope.$on "ambiguityPlot.renderPlot", (evt, params)->
                    console.log("Received", arguments)
                    #scope.predictions = params.predictions
                    updatePlot(scope.predictions, scope, element)

                scope.requestPredictionsUpdate = (opts = {})->
                    console.log "Requesting Updates"
                    scope.$emit("ambiguityPlot.requestPredictionsUpdate", opts)

            }
]