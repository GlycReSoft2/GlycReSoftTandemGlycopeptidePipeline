angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").directive("ambiguityPlot", [
  "$window", function($window) {
    var ambiguityPlotTemplater, ms1MassGroupingFn, positionGroupingFn, scalingDownFn, scalingUpFn, updatePlot;
    scalingDownFn = function(value) {
      return Math.log(value);
    };
    scalingUpFn = function(value) {
      return Math.exp(value);
    };
    ambiguityPlotTemplater = function(scope, seriesData, xAxisTitle, yAxisTitle) {
      var ambiguityPlotTemplateImpl, infitesimal;
      infitesimal = 1 / (Math.pow(1000, 1000));
      return ambiguityPlotTemplateImpl = {
        chart: {
          height: $window.innerHeight * 0.6,
          type: "bubble",
          zoomType: 'xy'
        },
        plotOptions: {
          series: {
            point: {
              events: {
                click: function(evt) {
                  var chart, point, xs, ys;
                  point = this;
                  chart = this.series.chart;
                  xs = _.pluck(this.series.points, "x");
                  ys = _.pluck(this.series.points, "y");
                  scope.$apply(function() {
                    return scope.describedPredictions = _.pluck(point.series.points, "data");
                  });
                  console.log(this);
                  chart.xAxis[0].setExtremes(Math.min.apply(null, xs) * (1 - infitesimal), Math.max.apply(null, xs) * (1 + infitesimal));
                  chart.yAxis[0].setExtremes(Math.min.apply(null, ys) * (1 - infitesimal), Math.max.apply(null, ys) * (1 + infitesimal));
                  return chart.showResetZoom();
                }
              }
            },
            states: {
              hover: false
            }
          }
        },
        legend: {
          title: {
            text: "<b>Legend</b> <small>(click series to hide)</small>"
          },
          align: 'right',
          verticalAlign: 'top',
          y: 60,
          width: 200,
          height: $window.innerHeight * .8
        },
        tooltip: {
          formatter: function() {
            var contents, point;
            point = this.point;
            contents = " MS1 Score: <b>" + point.x + "</b><br/> Mass: <b>" + (scalingUpFn(point.z)) + "</b><br/> MS2 Score: <b>" + point.y + "</b>(ME: <i>" + point.MS2_ScoreMeanError + "</i>)<br/> Number of Matches: <b>" + point.series.data.length + "</b><br/>";
            return contents;
          },
          headerFormat: "<span style=\"color:{series.color}\">●</span> {series.name}</span><br/>",
          pointFormat: " MS1 Score: <b>{point.x}</b>" + "<br/>Mass: <b>{point.z}</b><br/>MS2 Score: <b>{point.y}</b>(ME: <i>{point.MS2_ScoreMeanError}</i>)<br/> Number of Matches: <b>{series.data.length}</b><br/>",
          positioner: function(boxWidth, boxHeight, point) {
            var ttAnchor;
            ttAnchor = {
              x: point.plotX,
              y: point.plotY
            };
            ttAnchor.x -= boxWidth * 1;
            if (ttAnchor.x <= 0) {
              ttAnchor.x += 2 * (boxWidth * 1);
            }
            return ttAnchor;
          }
        },
        title: {
          text: "Ambiguous Groups Plot"
        },
        xAxis: {
          title: {
            text: xAxisTitle
          },
          events: {}
        },
        yAxis: {
          title: {
            text: yAxisTitle
          }
        },
        series: seriesData
      };
    };
    ms1MassGroupingFn = function(predictions) {
      var ionMassMS1Groups, ionMassMS1Series, ionPoints, notAmbiguous, p, perfectAmbiguous;
      ionPoints = (function() {
        var _i, _len, _results;
        _results = [];
        for (_i = 0, _len = predictions.length; _i < _len; _i++) {
          p = predictions[_i];
          _results.push({
            x: p.MS1_Score,
            y: p.MS2_Score,
            z: scalingDownFn(p.Obs_Mass),
            data: p
          });
        }
        return _results;
      })();
      ionMassMS1Groups = _.groupBy(ionPoints, function(pred) {
        return pred.x.toFixed(3) + "-" + pred.z.toFixed(3);
      });
      ionMassMS1Series = [];
      notAmbiguous = [];
      perfectAmbiguous = [];
      _.forEach(ionMassMS1Groups, function(group, id) {
        var i, mean, meanError, s, scoreRange, _i, _j, _len, _ref;
        if (group.length === 1) {
          _.forEach(group, function(pred) {
            return pred.MS2_ScoreMeanError = 0;
          });
          return notAmbiguous.push({
            data: group,
            name: "MS1/Mass " + id
          });
        } else {
          scoreRange = _.pluck(group, "y");
          mean = 0;
          for (_i = 0, _len = scoreRange.length; _i < _len; _i++) {
            s = scoreRange[_i];
            mean += s;
          }
          mean /= scoreRange.length;
          meanError = (function() {
            var _j, _len1, _results;
            _results = [];
            for (_j = 0, _len1 = scoreRange.length; _j < _len1; _j++) {
              s = scoreRange[_j];
              _results.push(s - mean);
            }
            return _results;
          })();
          for (i = _j = 0, _ref = group.length; 0 <= _ref ? _j < _ref : _j > _ref; i = 0 <= _ref ? ++_j : --_j) {
            group[i].MS2_ScoreMeanError = meanError[i] === 0 ? 0 : meanError[i].toFixed(4);
          }
          return ionMassMS1Series.push({
            data: group,
            name: "MS1/Mass " + id
          });
        }
      });
      return {
        ionSeries: ionMassMS1Series,
        notAmbiguous: notAmbiguous
      };
    };
    positionGroupingFn = function(predictions) {
      var ionPoints, ionStartLengthGroups, ionStartLengthSeries, notAmbiguous, p, perfectAmbiguous;
      ionPoints = (function() {
        var _i, _len, _results;
        _results = [];
        for (_i = 0, _len = predictions.length; _i < _len; _i++) {
          p = predictions[_i];
          _results.push({
            x: p.startAA,
            y: p.MS2_Score,
            z: p.endAA - p.startAA,
            data: p
          });
        }
        return _results;
      })();
      ionStartLengthGroups = _.groupBy(ionPoints, function(pred) {
        return pred.x.toFixed(3) + "-" + pred.z.toFixed(3);
      });
      ionStartLengthSeries = [];
      notAmbiguous = [];
      perfectAmbiguous = [];
      _.forEach(ionStartLengthGroups, function(group, id) {
        var i, mean, meanError, s, scoreRange, _i, _j, _len, _ref;
        if (group.length === 1) {
          _.forEach(group, function(pred) {
            return pred.MS2_ScoreMeanError = 0;
          });
          return ionStartLengthSeries.push({
            data: group,
            name: "Start AA/Length  " + id
          });
        } else {
          scoreRange = _.pluck(group, "y");
          mean = 0;
          for (_i = 0, _len = scoreRange.length; _i < _len; _i++) {
            s = scoreRange[_i];
            mean += s;
          }
          mean /= scoreRange.length;
          meanError = (function() {
            var _j, _len1, _results;
            _results = [];
            for (_j = 0, _len1 = scoreRange.length; _j < _len1; _j++) {
              s = scoreRange[_j];
              _results.push(s - mean);
            }
            return _results;
          })();
          for (i = _j = 0, _ref = group.length; 0 <= _ref ? _j < _ref : _j > _ref; i = 0 <= _ref ? ++_j : --_j) {
            group[i].MS2_ScoreMeanError = meanError[i] === 0 ? 0 : meanError[i].toFixed(4);
          }
          return ionStartLengthSeries.push({
            data: group,
            name: "Start AA/Length " + id
          });
        }
      });
      return {
        ionSeries: ionStartLengthSeries,
        notAmbiguous: notAmbiguous
      };
    };
    updatePlot = function(predictions, scope, element) {
      var chart, groupParams, ionSeries, notAmbiguous, plotOptions, xAxisTitle, yAxisTitle, _ref;
      groupParams = scope.grouping.groupingFnKey;
      console.log("Grouping Parameters: ", groupParams);
      scope.seriesData = groupParams.groupingFn(predictions);
      console.log("Series Data: ", scope.seriesData);
      scope.describedPredictions = [];
      _ref = scope.seriesData, ionSeries = _ref.ionSeries, notAmbiguous = _ref.notAmbiguous;
      plotOptions = ambiguityPlotTemplater(scope, ionSeries, xAxisTitle = groupParams.xAxisTitle, yAxisTitle = groupParams.yAxisTitle);
      console.log(plotOptions);
      chart = element.find(".ambiguity-plot-container");
      return chart.highcharts(plotOptions);
    };
    return {
      restrict: "AE",
      template: "<div class='amiguity-container'> <div class='plot-grouping-fn-selector-container'> <select class='plot-grouping-fn-selector-box' ng-model='grouping.groupingFnKey' ng-options='key for (key, value) in grouping.groupingsOptions' ng-change='requestPredictionsUpdate()'> </select> </div> <div class='ambiguity-plot-container'></div> <div class='ambiguity-peptide-sequences-container' ng-if='describedPredictions.length > 0'> <div class='ambiguity-peptide-attributes-container clearfix'> <div class='pull-left ambiguity-peptide-attributes'> <p>MS2 Score Range: {{describedMS2Min}} - {{describedMS2Max}}</p> <p>Peptide Region: {{describedPredictions[0].startAA}} - {{describedPredictions[0].endAA}}</p> </div> <div class='pull-left ambiguity-peptide-attributes'> <p>Peptide Sequence: {{describedPredictions[0].Peptide}}</p> <p>Distinct Glycan Count: {{keys(_.groupBy(describedPredictions, 'Glycan')).length}} </div> </div> <table class='table table-striped table-compact ambiguity-peptide-sequences-table'> <tr> <th>Glycopeptide Identifier</th> <th>Peptide Coverage</th> <th># Stub Ions</th> <th>B | Y Ions Coverage (+HexNAc)</th> <th>MS2 Score</th> </tr> <tr ng-repeat='match in describedPredictions | orderBy:[\"MS2_Score\",\"Glycan\",\"numStubs\"]:true'> <td ng-bind-html='match.Glycopeptide_identifier | highlightModifications'></td> <td>{{match.meanCoverage | number:4}}</td> <td>{{match.Stub_ions.length}}</td> <td>{{match.percent_b_ion_coverage * 100|number:1}}%({{match.percent_b_ion_with_HexNAc_coverage * 100|number:1}}%) | {{match.percent_y_ion_coverage * 100|number:1}}%({{match.percent_y_ion_with_HexNAc_coverage * 100|number:1}}%) </td> <td>{{match.MS2_Score}}</td> </tr> </table> </div> </div>",
      scope: {
        active: "="
      },
      link: function(scope, element, attr) {
        scope._predictions = [];
        scope.describedPredictions = [];
        scope.describedMS2Min = 0;
        scope.describedMS2Max = 0;
        scope.grouping = {};
        scope.grouping.groupingsOptions = {
          "MS1 Score + Mass": {
            groupingFn: ms1MassGroupingFn,
            xAxisTitle: "MS1 Score",
            yAxisTitle: "MS2 Score"
          },
          "Start AA + Length": {
            groupingFn: positionGroupingFn,
            xAxisTitle: "Peptide Start Position",
            yAxisTitle: "MS2 Score"
          }
        };
        scope._ = _;
        scope.keys = Object.keys;
        scope.grouping.groupingFnKey = scope.grouping.groupingsOptions["MS1 Score + Mass"];
        angular.element($window).bind('resize', function() {
          var chart;
          try {
            chart = element.find(".ambiguity-plot-container").highcharts();
            return chart.setSize($window.innerWidth, $window.innerHeight * 0.6);
          } catch (_error) {}
        });
        scope.$watch("describedPredictions", function(newVal) {
          var scoreRange;
          scoreRange = _.pluck(scope.describedPredictions, 'MS2_Score');
          scope.describedMS2Min = Math.min.apply(null, scoreRange);
          scope.describedMS2Max = Math.max.apply(null, scoreRange);
          return scope.$emit("selectedPredictions", {
            selectedPredictions: scope.describedPredictions
          });
        });
        scope.$on("ambiguityPlot.renderPlot", function(evt, params) {
          console.log("Received", arguments);
          scope._predictions = params.predictions;
          return updatePlot(params.predictions, scope, element);
        });
        return scope.requestPredictionsUpdate = function(opts) {
          if (opts == null) {
            opts = {};
          }
          console.log("Requesting Updates");
          return scope.$emit("ambiguityPlot.requestPredictionsUpdate", opts);
        };
      }
    };
  }
]);
