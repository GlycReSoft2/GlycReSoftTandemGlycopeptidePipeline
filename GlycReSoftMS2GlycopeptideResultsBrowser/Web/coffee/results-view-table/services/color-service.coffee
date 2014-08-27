# color-service.coffee
# Extracts the logic of associating a name with a color

angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").service "colorService", [()->
    @colors = ["blue", "rgb(228, 211, 84)", "red", "purple", "grey", "black", "green", "orange", "brown"]
    @_colorIter = 0

    @colorMap = {
        Peptide: "seagreen"
        HexNAc: "#CC99FF"
    }

    @_nextColor = ->
        color = @colors[@_colorIter++]
        if @_colorIter >= @colors.length
            @_colorIter = 0
        return color

    @getColor = (label) ->
        if label not of @colorMap
            @colorMap[label] = @_nextColor()
        return @colorMap[label]

    console.log(@)

]