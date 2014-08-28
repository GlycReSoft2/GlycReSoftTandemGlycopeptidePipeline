var __indexOf = [].indexOf || function(item) { for (var i = 0, l = this.length; i < l; i++) { if (i in this && this[i] === item) return i; } return -1; };

(function() {
  var activateFn, applyFiltrex, filterByFiltrex, filterRules, focusRow, groupingRules, helpText, setGroupBy, updateFiltrexDebounce, watchExternalDataChanges;
  setGroupBy = function(grouping, predictions) {
    var clustered, id;
    clustered = _.groupBy(predictions, grouping);
    id = 0;
    _.forEach(clustered, function(matches, key) {
      var match, _i, _len;
      for (_i = 0, _len = matches.length; _i < _len; _i++) {
        match = matches[_i];
        match['groupBy'] = id;
      }
      return id++;
    });
    return predictions;
  };
  applyFiltrex = function(predictions, filtrexExpr) {
    var filt, filterResults, i, passed, _i, _ref;
    filt = compileExpression(filtrexExpr);
    console.log(filt, filt.js);
    filterResults = _.map(predictions, filt);
    passed = [];
    for (i = _i = 0, _ref = predictions.length; 0 <= _ref ? _i < _ref : _i > _ref; i = 0 <= _ref ? ++_i : --_i) {
      if (filterResults[i] === 1) {
        passed.push(predictions[i]);
      }
    }
    return passed;
  };
  filterByFiltrex = function($scope, orderBy) {
    var column, ex, expr, filteredPredictions, groupedResults, key, orderedResults, _ref;
    expr = $scope.params.filtrexExpr.toLowerCase();
    _ref = $scope.headerSubstituitionDictionary;
    for (column in _ref) {
      key = _ref[column];
      expr = expr.replace(new RegExp(column, "g"), key);
    }
    try {
      console.log(expr);
      filteredPredictions = applyFiltrex($scope._predictions, expr);
      if (filteredPredictions.length === 0 && $scope._predictions.length === !0) {
        throw new Error("Incomplete Expression");
      }
      orderedResults = orderBy(filteredPredictions, ["MS1_Score", "Obs_Mass", "MS2_Score"]);
      groupedResults = $scope.groupByKey != null ? setGroupBy($scope.groupByKey, orderedResults) : orderedResults;
      $scope.predictions = groupedResults;
      $scope.filtrexError = false;
      return groupedResults;
    } catch (_error) {
      ex = _error;
      console.log("in catch");
      console.log(ex, $scope.filtrexError);
      if (expr.length > 0) {
        $scope.filtrexError = true;
      }
      if (expr.length === 0) {
        return $scope.predictions = $scope._predictions;
      }
    }
  };
  updateFiltrexDebounce = _.debounce(function($scope, orderBy) {
    return $scope.$apply(function() {
      return filterByFiltrex($scope, orderBy);
    });
  });
  10000;
  watchExternalDataChanges = function($scope, $window, orderBy) {
    return $scope.$watch("_predictionsReceiver", function(newVal, oldVald) {
      var filteredPredictions, groupedPredictions;
      console.log(arguments);
      $scope._predictions = orderBy(newVal, ["MS1_Score", "Obs_Mass", "-MS2_Score"]);
      filteredPredictions = filterByFiltrex($scope, orderBy);
      if (filteredPredictions == null) {
        filteredPredictions = $scope._predictions;
      }
      groupedPredictions = $scope.setGroupBy($scope.params.currentGroupingRule.groupByKey, filteredPredictions);
      $scope.predictions = groupedPredictions;
      return true;
    }, false);
  };
  focusRow = function($scope, targetRowIndex) {
    var grid, position;
    grid = $scope.gridOptions.ngGrid;
    position = grid.rowMap[targetRowIndex] * grid.config.rowHeight;
    console.log(grid.$viewport);
    grid.$viewport.scrollTop(position);
    return console.log(grid.$viewport);
  };
  activateFn = function($scope, $window, $filter) {
    var orderBy;
    orderBy = $filter("orderBy");
    $scope.deregisterWatcher = watchExternalDataChanges($scope, $window, orderBy);
    $scope.$watch("params.filtrexExpr", function() {
      return updateFiltrexDebounce($scope, orderBy);
    });
    $scope.$on("selectedPredictions", function(evt, params) {
      var glycopeptide, index, _i, _len, _ref, _results;
      try {
        $scope.gridOptions.selectAll(false);
        _ref = params.selectedPredictions;
        _results = [];
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
          glycopeptide = _ref[_i];
          index = _.findIndex($scope.predictions, {
            "Glycopeptide_identifier": glycopeptide.Glycopeptide_identifier
          });
          _results.push($scope.gridOptions.selectRow(index, true));
        }
        return _results;
      } catch (_error) {}
    });
    $scope.$on("ambiguityPlot.requestPredictionsUpdate", function(evt, params) {
      return $scope.sendRenderPlotEvt();
    });
    $scope.headerSubstituitionDictionary = $scope.buildHeaderSubstituitionDictionary();
    return console.log("Activation Complete");
  };
  helpText = {
    filtrex: '<article class="help-article"><h3>Filtrex</h3><h4>Expressions</h4><p>There are only 2 types: numbers and strings.</p><table><tbody><table class="table"><thead><tr><th>Numeric arithmetic</th><th>Description</th></tr></thead><tbody><tr><td>x + y</td><td>Add</td></tr><tr><td>x - y</td><td>Subtract</td></tr><tr><td>x * y</td><td>Multiply</td></tr><tr><td>x / y</td><td>Divide</td></tr><tr><td>x % y</td><td>Modulo</td></tr><tr><td>x ^ y</td><td>Power</td></tr></tbody></table><table class="table"><thead><tr><th>Comparisons</th><th>Description</th></tr></thead><tbody><tr><td>x == y</td><td>Equals</td></tr><tr><td>x &lt; y</td><td>Less than</td></tr><tr><td>x &lt;= y</td><td>Less than or equal to</td></tr><tr><td>x &gt; y</td><td>Greater than</td></tr><tr><td>x &gt;= y</td><td>Greater than or equal to</td></tr><tr><td>x in (a, b, c)</td><td>Equivalent to (x == a or x == b or x == c)</td></tr><tr><td>x not in (a, b, c)</td><td>Equivalent to (x != a and x != b and x != c)</td></tr></tbody></table><table class="table"><thead><tr><th>Boolean logic</th><th>Description</th></tr></thead><tbody><tr><td>x or y</td><td>Boolean or</td></tr><tr><td>x and y</td><td>Boolean and</td></tr><tr><td>not x</td><td>Boolean not</td></tr><tr><td>x ? y : z</td><td>If boolean x, value y, else z</td></tr></tbody></table><p>Created by Joe Walnes, <a href="https://github.com/joewalnes/filtrex"><br/>(See https://github.com/joewalnes/filtrex for more usage information.)</a></p></article>'
  };
  filterRules = {
    requirePeptideBackboneCoverage: {
      label: "Require Peptide Backbone Fragment Ions Matches",
      filtrex: "Mean Peptide Coverage > 0"
    },
    requireStubIons: {
      label: "Require Stub Ions Matches",
      filtrex: "Stub Ion Count > 0"
    },
    requireIonsWithHexNAc: {
      label: "Require Peptide Backbone Ion Fragmentss with HexNAc Matches",
      filtrex: "(% Y Ion With HexNAc Coverage + % B Ion With HexNAc Coverage) > 0"
    }
  };
  groupingRules = {
    ms1ScoreObsMass: {
      label: "Group ion matches by MS1 Score and Observed Mass (Ambiguous Matches)",
      groupByKey: function(x) {
        return [x.MS1_Score, x.Obs_Mass];
      }
    },
    startAALength: {
      label: "Group ion matches by the starting amino acid index and the peptide length (Heterogeneity)",
      groupByKey: function(x) {
        return [x.startAA, x.peptideLens];
      }
    }
  };
  return angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").controller("ClassifierResultsTableCtrl", [
    "$scope", "$window", '$filter', 'csvService', function($scope, $window, $filter, csvService) {
      var orderBy;
      orderBy = $filter("orderBy");
      $scope.helpText = helpText;
      $scope.filterRules = filterRules;
      $scope.groupingRules = groupingRules;
      $scope.predictions = [];
      $scope._predictions = [];
      $scope._predictionsReceiver = [];
      $scope.params = {};
      $scope.headerSubstituitionDictionary = {};
      $scope.params.filtrexExpr = "MS2 Score > 0.2";
      $scope.params.currentGroupingRule = $scope.groupingRules.ms1ScoreObsMass;
      $scope.groupByKey = null;
      $scope.deregisterWatcher = null;
      $scope.ping = function(args) {
        return console.log("ping", arguments, $scope);
      };
      $scope.extendFiltrex = function(expr) {
        console.log("Extending Filtrex with " + expr);
        if ($scope.params.filtrexExpr.length > 0) {
          return $scope.params.filtrexExpr += " and " + expr;
        } else {
          return $scope.params.filtrexExpr += expr;
        }
      };
      $scope.filterByFiltrex = function() {
        var column, dictionary, expr, groupedResults, key, orderedResults, results;
        dictionary = $scope.substituteHeaders();
        expr = $scope.params.filtrexExpr.toLowerCase();
        for (column in dictionary) {
          key = dictionary[column];
          expr = expr.replace(column, key);
        }
        console.log(expr);
        results = applyFiltrex($scope._predictions, expr);
        orderedResults = orderBy(results, ["MS1_Score", "Obs_Mass", "MS2_Score"]);
        groupedResults = setGroupBy($scope.groupByKey, orderedResults);
        return groupedResults;
      };
      $scope.sendRenderPlotEvt = function() {
        return $scope.$broadcast("ambiguityPlot.renderPlot", {
          predictions: $scope.predictions
        });
      };
      $scope.sendUpdateProteinViewEvt = function() {
        return $scope.$broadcast("proteinSequenceView.updateProteinView", {
          predictions: $scope.predictions
        });
      };
      $scope.setGroupBy = function(grouping, predictions) {
        if (predictions == null) {
          predictions = null;
        }
        $scope.groupByKey = grouping;
        return setGroupBy(grouping, predictions);
      };
      $scope.scrollToSelection = function() {
        var glycopeptide, index, selectedItems, topIndex, _i, _len;
        if (($scope.gridOptions.$gridScope != null) && ($scope.gridOptions.$gridScope.selectedItems != null)) {
          selectedItems = $scope.gridOptions.$gridScope.selectedItems;
          topIndex = Infinity;
          for (_i = 0, _len = selectedItems.length; _i < _len; _i++) {
            glycopeptide = selectedItems[_i];
            index = _.findIndex($scope.predictions, {
              "Glycopeptide_identifier": glycopeptide.Glycopeptide_identifier
            });
            if (index < topIndex) {
              topIndex = index;
            }
          }
          if (topIndex === Infinity) {
            return false;
          }
          return focusRow($scope, topIndex);
        }
      };
      $scope.buildHeaderSubstituitionDictionary = function() {
        var BLACK_LIST, column, dictionary, _i, _len, _ref, _ref1;
        dictionary = {};
        dictionary.NAME_MAP = [];
        BLACK_LIST = ["Peptide Span"];
        _ref = $scope.gridOptions.columnDefs;
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
          column = _ref[_i];
          if (!(_ref1 = column.displayName, __indexOf.call(BLACK_LIST, _ref1) >= 0)) {
            dictionary.NAME_MAP.push(column.displayName);
            dictionary[column.displayName.toLowerCase()] = column.field;
          }
        }
        dictionary["Start AA".toLowerCase()] = "startAA";
        dictionary.NAME_MAP.push("Start AA");
        dictionary["End AA".toLowerCase()] = "endAA";
        dictionary.NAME_MAP.push("End AA");
        dictionary["AA Length".toLowerCase()] = "peptideLens";
        dictionary.NAME_MAP.push("AA Length");
        dictionary["Oxonium Ion Count".toLowerCase()] = "numOxIons";
        dictionary.NAME_MAP.push("Oxonium Ion Count");
        dictionary["Stub Ion Count".toLowerCase()] = "numStubs";
        dictionary.NAME_MAP.push("Stub Ion Count");
        dictionary["% Y Ion Coverage".toLowerCase()] = "percent_y_ion_coverage";
        dictionary.NAME_MAP.push("% Y Ion Coverage");
        dictionary["% B Ion Coverage".toLowerCase()] = "percent_b_ion_coverage";
        dictionary.NAME_MAP.push("% B Ion Coverage");
        dictionary["% Y Ion With HexNAc Coverage".toLowerCase()] = "percent_y_ion_with_HexNAc_coverage";
        dictionary.NAME_MAP.push("% Y Ion With HexNAc Coverage");
        dictionary["% B Ion With HexNAc Coverage".toLowerCase()] = "percent_b_ion_with_HexNAc_coverage";
        dictionary.NAME_MAP.push("% B Ion With HexNAc Coverage");
        return dictionary;
      };
      $scope.gridOptions = {
        data: "predictions",
        showColumnMenu: true,
        showFilter: false,
        enableSorting: false,
        enableHighlighting: true,
        enablePinning: true,
        rowHeight: 90,
        columnDefs: [
          {
            field: 'MS2_Score',
            width: 90,
            pinned: true,
            displayName: "MS2 Score",
            cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
          }, {
            field: 'MS1_Score',
            width: 90,
            pinned: true,
            displayName: "MS1 Score",
            cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
          }, {
            field: 'Obs_Mass',
            width: 130,
            pinned: true,
            displayName: "Observed Mass",
            cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
          }, {
            field: 'vol',
            width: 100,
            pinned: true,
            displayName: "Volume",
            cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
          }, {
            field: 'ppm_error',
            width: 90,
            displayName: "PPM Error",
            cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|scientificNotation|number:4}}</div></div>'
          }, {
            field: 'Glycopeptide_identifier',
            width: 240,
            displayName: "Glycopeptide Sequence",
            cellClass: "matched-ions-cell glycopeptide-identifier",
            cellTemplate: '<div><div class="ngCellText" ng-bind-html="row.getProperty(col.field)|highlightModifications"></div></div>'
          }, {
            field: 'meanCoverage',
            width: 180,
            displayName: "Mean Peptide Coverage",
            cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)|number:4}}</div></div>'
          }, {
            field: 'percentUncovered',
            width: 165,
            displayName: "% Peptide Uncovered",
            cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field) * 100|number:2}}</div></div>'
          }, {
            field: "startAA",
            width: 180,
            displayName: "Peptide Span",
            cellTemplate: '<div><div class="ngCellText matched-ions-cell">{{row.getProperty(col.field)}}-{{row.entity.endAA}}&nbsp;({{row.entity.peptideLens}})</div></div>'
          }, {
            field: 'Oxonium_ions',
            width: 200,
            headerClass: null,
            displayName: "Oxonium Ions",
            cellClass: "stacked-ions-cell-grid",
            cellTemplate: '<div> <div class="ngCellText"> <div class="coverage-text">{{row.entity.numOxIons}} Ions Matched</div> <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion> </div> </div>'
          }, {
            field: 'Stub_ions',
            width: 340,
            displayName: "Stub Ions",
            headerClass: null,
            cellClass: "stacked-ions-cell-grid",
            cellTemplate: '<div> <div class="ngCellText"> <div class="coverage-text">{{row.entity.numStubs}} Ions Matched</div> <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion> </div> </div>'
          }, {
            field: 'b_ion_coverage',
            width: 340,
            displayName: "B Ions",
            headerClass: null,
            cellClass: "stacked-ions-cell-grid",
            cellTemplate: '<div> <div class="ngCellText"> <div class="coverage-text">{{row.entity.percent_b_ion_coverage * 100|number:1}}% Coverage</div> <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion> </div> </div>'
          }, {
            field: 'y_ion_coverage',
            width: 340,
            displayName: "Y Ions",
            headerClass: null,
            cellClass: "stacked-ions-cell-grid",
            cellTemplate: '<div> <div class="ngCellText"> <div class="coverage-text">{{row.entity.percent_y_ion_coverage * 100|number:1}}% Coverage</div> <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion> </div> </div>'
          }, {
            field: 'b_ions_with_HexNAc',
            width: 340,
            displayName: "B Ions with HexNAc",
            headerClass: null,
            cellClass: "stacked-ions-cell-grid",
            cellTemplate: '<div> <div class="ngCellText"> <div class="coverage-text">{{row.entity.percent_b_ion_with_HexNAc_coverage * 100 |number:1}}% Coverage</div> <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion> </div> </div>'
          }, {
            field: 'y_ions_with_HexNAc',
            width: 340,
            displayName: "Y Ions with HexNAc",
            headerClass: null,
            cellClass: "stacked-ions-cell-grid",
            cellTemplate: '<div> <div class="ngCellText"> <div class="coverage-text">{{row.entity.percent_y_ion_with_HexNAc_coverage * 100|number:1}}% Coverage</div> <fragment-ion ng-repeat="fragment_ion in row.getProperty(col.field)"></fragment-ion> </div> </div>'
          }
        ],
        rowTemplate: '<div style="height: 100%" class="c{{row.entity.groupBy % 6}}"> <div ng-style="{ \'cursor\': row.cursor }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell matched-ions-cell"> <div class="ngVerticalBar" ng-style="{height: rowHeight}" ng-class="{ ngVerticalBarVisible: !$last }"> </div> <div ng-cell> </div> </div> </div>'
      };
      activateFn($scope, $window, $filter);
      return $window.ClassifierResultsTableCtrlInstance = $scope;
    }
  ]);
})();
