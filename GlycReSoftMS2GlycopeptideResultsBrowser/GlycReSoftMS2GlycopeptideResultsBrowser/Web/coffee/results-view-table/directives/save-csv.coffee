# save-csv.coffee
# Handles the indirection to create a client-side file save

angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").directive "saveCsv", ["csvService", (csvService) ->

    saveCsv = (predictions, element) ->
        if not (Blob? and saveAs?)
            alert("File Saving is not supported with this browser")
            return
        output = csvService.format(predictions)
        #encoded = encodeURIComponent(output)
        #uriContent = "data:text/csv;charset=utf-8," + encoded;
        #console.log(uriContent.length)
        #tag = (angular.element("<a></a>").html("Download"))
        #console.log tag
        #tag.attr("href", uriContent)
        #console.log 'href'
        #tag.attr('download', 'results.csv')
        #console.log 'download'
        #element.parent().append(tag)
        blob = new Blob([output], {type: "text/csv;charset=utf-8"})
        saveAs(blob, "results.csv")



    return {
        link: (scope, element, attrs) ->
            console.log("Save-Csv", arguments)
            element.click -> saveCsv(scope._predictionsReceiver, element)
    }


]