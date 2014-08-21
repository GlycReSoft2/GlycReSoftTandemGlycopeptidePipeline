# protein-sequence-view.coffee
# Not an Angular View
#
# Render a Biojs.FeatureViewer that is decorated with peptide fragment matches
# Depends upon
# lodash
# Biojs
# Biojs.FeatureViewer - Modified to add Raphael.Element.node.id to Raphael.Element.id to make lookup faster
# raphael - The version of Raphael that Biojs bundles is old, and doesn't have Paper.getById.
# canvg
# rgbcolor
# jquery
# jquery.tooltip
# jquery.ui

# jQuery UI is attached to angular.element, and instantiating jQuery again in noConflictMode
# is missing the extensions
angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").directive "proteinSequenceView", ["$window", "$filter",
    ($window, $filter) ->
        orderBy = $filter("orderBy")
        highlightModifications = $filter("highlightModifications")

        _colorIter = 0
        colors = ["blue", "yellow", "red", "purple", "grey"]
        _shapeIter = 0
        shapes = ["diamond", "triangle", "hexagon","wave", "circle"]

        featureTemplate = {
            nonOverlappingStyle: {
                heightOrRadius: 10
                y: 140
            }

            centeredStyle: {
                heightOrRadius: 48
                y: 71
            }

            rowStyle: {
                heightOrRadius: 10
                y: 181
            }

            text: ""
            type: "rect" # shape string
            fill: "#CDBCF6" # color string
            stroke: "#CDBCF6" # color string
            fillOpacity: 0.5
            height: 10
            evidenceText: ":"
            evidenceCode: "MS2 Score"
            typeCategory: "typeCategory"
            typeCode: "typeCode"
            path: "" # ?
            typeLabel: "" # May connect with featureLabel
            featureLabel: ""
            featureStart: null # number
            featureEnd: null # number
            strokeWidth: 1 # number
            r: 10 # number

            featureTypeLabel: "" # string related to featureLabel and typeLabel
        }

        legendKeyTemplate ={
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
        }

        shapeMap = {
            Peptide: "rect"
            HexNAc: "circle"
            PTM: "triangle"
        }

        # This map persists within closure, but may differ
        # with different orders of examination of the same dataset
        colorMap = {
            Peptide: "seagreen"
            HexNAc: "#CC99FF"
        }

        typeCategoryMap = {
        }

        _layerCounter = 0
        _layerIncrement = 15
        heightLayerMap = {
        }

        nextColor = ->
            color = colors[_colorIter++]
            if _colorIter >= colors.length
                _colorIter = 0
            return color

        generateConfig = ($window) ->
            configuration = {
                aboveRuler: 10
                belowRuler: 30
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
            }
            return configuration

        transformFeatuersToLegend = (featuresArray) ->
            return []

        # Given a set of modifications at the same site, return the best score
        # for that site.
        getBestScoresForModification = (modifications) ->
            foldedMods = _.groupBy(modifications, "featureId")
            topMods = []
            for modId, mod of foldedMods
                topMods.push(orderBy(mod, "evidenceCode", true)[0])
            return topMods


        parseGlycopeptideIdentifierToModificationsArray = (glycoform, startSite) ->
            glycopeptide = glycoform.Glycopeptide_identifier
            regex = /(\(.+?\)|\[.+?\])/
            index = 0
            fragments = glycopeptide.split(regex)
            modifications = []
            for frag in fragments
                if frag.charAt(0) is "["
                    # Ignore the Glycan Identifier Chunk
                else if frag.charAt(0) is "("
                    # This is a modification site
                    label = frag.replace(/\(|\)/g, "")
                    feature = _.cloneDeep(featureTemplate)
                    feature.type = if label of shapeMap then shapeMap[label] else shapeMap.PTM

                    if feature.type is "circle"
                        feature.r /= 2

                    if label not of colorMap
                        colorMap[label] = nextColor()
                        console.log(label, colorMap[label])
                    feature.fill = colorMap[label]

                    feature.featureStart = index + startSite
                    feature.featureEnd = index + startSite
                    feature.typeLabel = ""
                    feature.typeCode = ""
                    feature.typeCategory  = ""
                    feature.evidenceText = glycoform.MS2_Score

                    feature.featureLabel = label
                    feature.featureTypeLabel = label
                    feature.featureId = label + "-" + (index + startSite) # Parens prevent string addition



                    if label not of heightLayerMap
                        _layerCounter += _layerIncrement
                        heightLayerMap[label] = _layerCounter

                    feature.cy = 140 - ((feature.r) + heightLayerMap[label])

                    modifications.push feature
                else
                    index += frag.length


            return modifications




        transformPredictionGroupsToFeatures = (predictions) ->
            fragments = _.groupBy(predictions, (p) -> [p.startAA, p.endAA])

            featuresArray = []
            modifications = []


            arrange = orderBy(Object.keys fragments, (range) ->
                [start, end] = range.split(",")
                return end-start).reverse()

            for fragRange in arrange
                frag = fragments[fragRange]
                depth = 1

                frag = orderBy(frag, "MS2_Score").reverse()

                for glycoform in frag
                    feature = _.cloneDeep featureTemplate
                    feature.type = shapeMap.Peptide
                    feature.fill = colorMap.Peptide
                    feature.stroke = colorMap.Peptide
                    feature.featureStart = glycoform.startAA
                    feature.featureEnd = glycoform.endAA
                    feature.text = glycoform.Glycopeptide_identifier

                    feature.typeLabel = "Peptide"
                    feature.typeCode = ""
                    feature.typeCategory = ""
                    feature.featureTypeLabel = "glycopeptide_match"
                    feature.evidenceText = glycoform.MS2_Score
                    feature.featureId = glycoform.Glycopeptide_identifier.replace(/\[|\]|;|\(|\)/g, "-")

                    feature.y = depth * (feature.height + 2 * feature.strokeWidth) + 125

                    glycoformModifications = parseGlycopeptideIdentifierToModificationsArray(glycoform, glycoform.startAA)
                    modifications = modifications.concat(glycoformModifications)
                    feature.modifications = _.pluck(glycoformModifications, "featureId")
                    feature._obj = glycoform

                    # Wait until after the modifications have been processed and registered in colorMap
                    feature.featureLabel = highlightModifications(glycoform.Glycopeptide_identifier, colorMap)

                    featuresArray.push feature

                    depth++

            foldedMods = _.pluck(_.groupBy(modifications, "featureId"), (obj) -> obj[0])
            topMods = getBestScoresForModification(modifications)
            featuresArray = featuresArray.concat(topMods)

            return featuresArray

        updateView = (scope, element) ->
            scope.start = Math.min.apply(null, _.pluck(scope.predictions, "startAA"))
            scope.end = Math.max.apply(null, _.pluck(scope.predictions, "endAA"))
            # Produce feature array
            scope.featureViewerConfig.featuresArray = transformPredictionGroupsToFeatures(scope.predictions)
            # Produce legend
            scope.featureViewerConfig.legend.keys = [] # TODO
            conf = scope.featureViewerConfig.configuration = generateConfig($window)
            conf.requestedStart = scope.start
            conf.requestedStop = scope.end
            conf.sequenceLength = scope.end
            if scope.featureViewerInstance?
                # BioJs caches all instances of a given widget. Have to remove all contents and
                # references to avoid memory leaks.
                try
                    scope.featureViewerInstance.clear()
                    biojsId = scope.featureViewerInstance
                    delete Biojs_FeatureViewer_array[biojsId - 1]
                    delete scope.featureViewerInstance


            scope.featureViewerInstance = new Biojs.FeatureViewer({
                    target: "protein-sequence-view-container-div"
                    json: _.cloneDeep(scope.featureViewerConfig)
                })
            scope.featureViewerInstance.onFeatureClick (featureShape) ->
                id = featureShape.featureId
                feature =  _.find(scope.featureViewerConfig.featuresArray, {featureId: id})
                console.log(id, feature)
            scope.featureViewerInstance.onFeatureOn (featureShape) ->
                id = featureShape.featureId
                feature =  _.find(scope.featureViewerConfig.featuresArray, {featureId: id})
                if feature.modifications?
                    for mod in feature.modifications
                        modId = "uniprotFeaturePainter_" + mod
                        scope.featureViewerInstance.raphael.getById(modId).transform("s2").attr("fill-opacity", 1)
            scope.featureViewerInstance.onFeatureOff (featureShape) ->
                id = featureShape.featureId
                feature =  _.find(scope.featureViewerConfig.featuresArray, {featureId: id})
                if feature.modifications?
                    for mod in feature.modifications
                        modId = "uniprotFeaturePainter_" + mod
                        scope.featureViewerInstance.raphael.getById(modId).transform("s1").attr("fill-opacity", 0.5)



        return {
            restrict: "E"
            link: (scope, element, attrs) ->
                scope.getColorMap = -> colorMap
                scope.featureViewerConfig = {
                    featuresArray: []
                    segment: ""
                    configuration: {}
                    legend: {
                        segment: {
                            yPosCentered: 190
                            text: ""
                            yPos: 234
                            xPos: 15
                            yPosNonOverlapping: 214
                            yposRows: 290
                        }
                        key:[

                        ]
                    }

                }
                console.log("proteinSequenceView", arguments)
                window.TEST = scope
                scope.$on "proteinSequenceView.updateProteinView", (evt, params) ->
                    updateView(scope, element)



            template: "<div class='protein-sequence-view-container' id='protein-sequence-view-container-div'>!!</div>"
        }
    ]

