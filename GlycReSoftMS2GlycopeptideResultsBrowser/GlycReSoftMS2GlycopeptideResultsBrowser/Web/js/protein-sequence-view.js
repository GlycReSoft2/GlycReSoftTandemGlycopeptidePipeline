angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").directive("proteinSequenceView", [
  "$window", "$filter", function($window, $filter) {
    var colorMap, colors, featureTemplate, generateConfig, getBestScoresForModification, heightLayerMap, highlightModifications, legendKeyTemplate, nextColor, orderBy, parseGlycopeptideIdentifierToModificationsArray, shapeMap, shapes, transformFeatuersToLegend, transformPredictionGroupsToFeatures, typeCategoryMap, updateView, _colorIter, _layerCounter, _layerIncrement, _shapeIter;
    orderBy = $filter("orderBy");
    highlightModifications = $filter("highlightModifications");
    _colorIter = 0;
    colors = ["blue", "yellow", "red", "purple", "grey"];
    _shapeIter = 0;
    shapes = ["diamond", "triangle", "hexagon", "wave", "circle"];
    featureTemplate = {
      nonOverlappingStyle: {
        heightOrRadius: 10,
        y: 140
      },
      centeredStyle: {
        heightOrRadius: 48,
        y: 71
      },
      rowStyle: {
        heightOrRadius: 10,
        y: 181
      },
      text: "",
      type: "rect",
      fill: "#CDBCF6",
      stroke: "#CDBCF6",
      fillOpacity: 0.5,
      height: 10,
      evidenceText: ":",
      evidenceCode: "MS2 Score",
      typeCategory: "typeCategory",
      typeCode: "typeCode",
      path: "",
      typeLabel: "",
      featureLabel: "",
      featureStart: null,
      featureEnd: null,
      strokeWidth: 1,
      r: 10,
      featureTypeLabel: ""
    };
    legendKeyTemplate = {
      label: {
        total: 1,
        yPosCentered: 210,
        text: "Domain",
        yPos: 234,
        xPos: 50,
        yPosNonOverlapping: 234,
        yPosRows: 310
      },
      shape: {
        centeredStyle: {
          heightOrRadius: 5,
          y: 208
        },
        text: "",
        nonOverlappingStyle: {
          heightOrRadius: 5,
          y: 229
        },
        width: 30,
        fill: "#033158",
        cy: 229,
        cx: 15,
        type: "rect",
        fillOpacity: 0.5,
        stroke: "#033158",
        height: 5,
        r: 10,
        path: "",
        rowsStyle: {
          heightOrRadius: 5,
          y: 305
        },
        typeLabel: "",
        y: 229,
        strokeWidth: 1,
        x: 15
      }
    };
    shapeMap = {
      Peptide: "rect",
      HexNAc: "circle",
      PTM: "triangle"
    };
    colorMap = {
      Peptide: "seagreen",
      HexNAc: "#CC99FF"
    };
    typeCategoryMap = {};
    _layerCounter = 0;
    _layerIncrement = 15;
    heightLayerMap = {};
    nextColor = function() {
      var color;
      color = colors[_colorIter++];
      if (_colorIter >= colors.length) {
        _colorIter = 0;
      }
      return color;
    };
    generateConfig = function($window) {
      var configuration;
      configuration = {
        aboveRuler: 10,
        belowRuler: 30,
        requestedStop: 770,
        horizontalGridNumLines: 11,
        sequenceLineYCentered: 95,
        requestedStart: 1,
        gridLineHeight: 12,
        rightMargin: 20,
        sequenceLength: 770,
        horizontalGridNumLinesNonOverlapping: 11,
        horizontalGridNumLinesCentered: 6,
        verticalGridLineLengthRows: 284,
        unitSize: 0.8571428571428571,
        sizeYNonOverlapping: 184,
        style: "nonOverlapping",
        sequenceLineYRows: 155,
        sequenceLineY: 138,
        verticalGrid: false,
        rulerY: 20,
        dasSources: null,
        horizontalGrid: false,
        pixelsDivision: 50,
        sizeY: $window.innerHeight,
        sizeX: $window.innerWidth * .95,
        dasReference: null,
        sizeYRows: 260,
        rulerLength: $window.innerWidth * .8,
        verticalGridLineLengthNonOverlapping: 174,
        sizeYKey: 210,
        sizeYCentered: 160,
        sequenceLineYNonOverlapping: 138,
        verticalGridLineLength: 174,
        horizontalGridNumLinesRows: 8,
        leftMargin: 20,
        nonOverlapping: true,
        verticalGridLineLengthCentered: 172
      };
      return configuration;
    };
    transformFeatuersToLegend = function(featuresArray) {
      return [];
    };
    getBestScoresForModification = function(modifications) {
      var foldedMods, mod, modId, topMods;
      foldedMods = _.groupBy(modifications, "featureId");
      topMods = [];
      for (modId in foldedMods) {
        mod = foldedMods[modId];
        topMods.push(orderBy(mod, "evidenceCode", true)[0]);
      }
      return topMods;
    };
    parseGlycopeptideIdentifierToModificationsArray = function(glycoform, startSite) {
      var feature, frag, fragments, glycopeptide, index, label, modifications, regex, _i, _len;
      glycopeptide = glycoform.Glycopeptide_identifier;
      regex = /(\(.+?\)|\[.+?\])/;
      index = 0;
      fragments = glycopeptide.split(regex);
      modifications = [];
      for (_i = 0, _len = fragments.length; _i < _len; _i++) {
        frag = fragments[_i];
        if (frag.charAt(0) === "[") {

        } else if (frag.charAt(0) === "(") {
          label = frag.replace(/\(|\)/g, "");
          feature = _.cloneDeep(featureTemplate);
          feature.type = label in shapeMap ? shapeMap[label] : shapeMap.PTM;
          if (feature.type === "circle") {
            feature.r /= 2;
          }
          if (!(label in colorMap)) {
            colorMap[label] = nextColor();
            console.log(label, colorMap[label]);
          }
          feature.fill = colorMap[label];
          feature.featureStart = index + startSite;
          feature.featureEnd = index + startSite;
          feature.typeLabel = "";
          feature.typeCode = "";
          feature.typeCategory = "";
          feature.evidenceText = glycoform.MS2_Score;
          feature.featureLabel = label;
          feature.featureTypeLabel = label;
          feature.featureId = label + "-" + (index + startSite);
          if (!(label in heightLayerMap)) {
            _layerCounter += _layerIncrement;
            heightLayerMap[label] = _layerCounter;
          }
          feature.cy = 140 - (feature.r + heightLayerMap[label]);
          modifications.push(feature);
        } else {
          index += frag.length;
        }
      }
      return modifications;
    };
    transformPredictionGroupsToFeatures = function(predictions) {
      var arrange, depth, feature, featuresArray, foldedMods, frag, fragRange, fragments, glycoform, glycoformModifications, modifications, topMods, _i, _j, _len, _len1;
      fragments = _.groupBy(predictions, function(p) {
        return [p.startAA, p.endAA];
      });
      featuresArray = [];
      modifications = [];
      arrange = orderBy(Object.keys(fragments, function(range) {
        var end, start, _ref;
        _ref = range.split(","), start = _ref[0], end = _ref[1];
        return end - start;
      })).reverse();
      for (_i = 0, _len = arrange.length; _i < _len; _i++) {
        fragRange = arrange[_i];
        frag = fragments[fragRange];
        depth = 1;
        frag = orderBy(frag, "MS2_Score").reverse();
        for (_j = 0, _len1 = frag.length; _j < _len1; _j++) {
          glycoform = frag[_j];
          feature = _.cloneDeep(featureTemplate);
          feature.type = shapeMap.Peptide;
          feature.fill = colorMap.Peptide;
          feature.stroke = colorMap.Peptide;
          feature.featureStart = glycoform.startAA;
          feature.featureEnd = glycoform.endAA;
          feature.text = glycoform.Glycopeptide_identifier;
          feature.typeLabel = "Peptide";
          feature.typeCode = "";
          feature.typeCategory = "";
          feature.featureTypeLabel = "glycopeptide_match";
          feature.evidenceText = glycoform.MS2_Score;
          feature.featureId = glycoform.Glycopeptide_identifier.replace(/\[|\]|;|\(|\)/g, "-");
          feature.y = depth * (feature.height + 2 * feature.strokeWidth) + 125;
          glycoformModifications = parseGlycopeptideIdentifierToModificationsArray(glycoform, glycoform.startAA);
          modifications = modifications.concat(glycoformModifications);
          feature.modifications = _.pluck(glycoformModifications, "featureId");
          feature._obj = glycoform;
          feature.featureLabel = highlightModifications(glycoform.Glycopeptide_identifier, colorMap);
          featuresArray.push(feature);
          depth++;
        }
      }
      foldedMods = _.pluck(_.groupBy(modifications, "featureId"), function(obj) {
        return obj[0];
      });
      topMods = getBestScoresForModification(modifications);
      featuresArray = featuresArray.concat(topMods);
      return featuresArray;
    };
    updateView = function(scope, element) {
      var biojsId, conf;
      scope.start = Math.min.apply(null, _.pluck(scope.predictions, "startAA"));
      scope.end = Math.max.apply(null, _.pluck(scope.predictions, "endAA"));
      scope.featureViewerConfig.featuresArray = transformPredictionGroupsToFeatures(scope.predictions);
      scope.featureViewerConfig.legend.keys = [];
      conf = scope.featureViewerConfig.configuration = generateConfig($window);
      conf.requestedStart = scope.start;
      conf.requestedStop = scope.end;
      conf.sequenceLength = scope.end;
      if (scope.featureViewerInstance != null) {
        try {
          scope.featureViewerInstance.clear();
          biojsId = scope.featureViewerInstance;
          delete Biojs_FeatureViewer_array[biojsId - 1];
          delete scope.featureViewerInstance;
        } catch (_error) {}
      }
      scope.featureViewerInstance = new Biojs.FeatureViewer({
        target: "protein-sequence-view-container-div",
        json: _.cloneDeep(scope.featureViewerConfig)
      });
      scope.featureViewerInstance.onFeatureClick(function(featureShape) {
        var feature, id;
        id = featureShape.featureId;
        feature = _.find(scope.featureViewerConfig.featuresArray, {
          featureId: id
        });
        return console.log(id, feature);
      });
      scope.featureViewerInstance.onFeatureOn(function(featureShape) {
        var feature, id, mod, modId, _i, _len, _ref, _results;
        id = featureShape.featureId;
        feature = _.find(scope.featureViewerConfig.featuresArray, {
          featureId: id
        });
        if (feature.modifications != null) {
          _ref = feature.modifications;
          _results = [];
          for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            mod = _ref[_i];
            modId = "uniprotFeaturePainter_" + mod;
            _results.push(scope.featureViewerInstance.raphael.getById(modId).transform("s2").attr("fill-opacity", 1));
          }
          return _results;
        }
      });
      return scope.featureViewerInstance.onFeatureOff(function(featureShape) {
        var feature, id, mod, modId, _i, _len, _ref, _results;
        id = featureShape.featureId;
        feature = _.find(scope.featureViewerConfig.featuresArray, {
          featureId: id
        });
        if (feature.modifications != null) {
          _ref = feature.modifications;
          _results = [];
          for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            mod = _ref[_i];
            modId = "uniprotFeaturePainter_" + mod;
            _results.push(scope.featureViewerInstance.raphael.getById(modId).transform("s1").attr("fill-opacity", 0.5));
          }
          return _results;
        }
      });
    };
    return {
      restrict: "E",
      link: function(scope, element, attrs) {
        scope.getColorMap = function() {
          return colorMap;
        };
        scope.featureViewerConfig = {
          featuresArray: [],
          segment: "",
          configuration: {},
          legend: {
            segment: {
              yPosCentered: 190,
              text: "",
              yPos: 234,
              xPos: 15,
              yPosNonOverlapping: 214,
              yposRows: 290
            },
            key: []
          }
        };
        console.log("proteinSequenceView", arguments);
        window.TEST = scope;
        return scope.$on("proteinSequenceView.updateProteinView", function(evt, params) {
          return updateView(scope, element);
        });
      },
      template: "<div class='protein-sequence-view-container' id='protein-sequence-view-container-div'>!!</div>"
    };
  }
]);
