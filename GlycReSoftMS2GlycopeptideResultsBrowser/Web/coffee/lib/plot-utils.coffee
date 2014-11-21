PlotUtils = (() ->
    ns = {}
    addAlphaToRGB = (rgb, opacity) ->
        return "rgba(#{rgb.r}, #{rgb.g}, #{rgb.b}, #{opacity}"

    ns.BackboneStackChart =
    class BackboneStackChart
        @template = {
            chart:
                type: "columnrange"
                inverted: true
            title:
                text: "Peptide Backbone Fragment Coverage"
            xAxis:[
                {
                    title:
                        text: "Sequence Position"
                    allowDecimals: false
                }
            ]
            yAxis:
                {
                    title:
                        text: "Backbone Fragmentation Site"
                    allowDecimals: false
                }
            plotOptions:
                columnrange:
                    borderWidth: 4


            legend:
                enabled: true
            series: []
        }
        constructor: (@glycopeptide, @container) ->
            @backboneStack = GlycopeptideLib.buildBackboneStack(@glycopeptide)
            @config = _.cloneDeep(BackboneStackChart.template)
            @config.series.push {name: "b Ion", data: _.pluck(@backboneStack, "bIon")}
            @config.series.push {name: "y Ion", data: _.pluck(@backboneStack, "yIon")}
            @config.series.push {name: "b Ion + HexNAc", data: _.pluck(@backboneStack, "bHexNAc")}
            @config.series.push {name: "y Ion + HexNAc", data: _.pluck(@backboneStack, "yHexNAc")}
            @addModificationBars()

        addModificationBars: ->
            modificationSites = GlycopeptideLib.parseModificationSites(@glycopeptide)
            for mod in modificationSites
                @config.series.push {
                    name: "#{mod.name}-#{mod.position}"
                    data:([i, mod.position] for i in [0..@glycopeptide.peptideLens])
                    type: "scatter"
                    color: addAlphaToRGB(new RGBColor(ColorSource.getColor(mod.name)), 0.8)
                    marker:
                        radius: 6
                }


        render: ->
            @chart = $(@container).highcharts(@config)
            @


    ns.ModificationDistributionChart =
    class ModificationDistributionChart

        constructor: (@predictions, @container) ->

        render: ->
            @chart = $(@container).highcharts(@config)
            @

    return ns)()
