# save-csv.coffee
# Handles the indirection to create a client-side file save

angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").directive "saveCsv", ["csvService", (csvService) ->

    saveCsv = (predictions, element, fileName="results.csv") ->
        if not (Blob? and saveAs?)
            alert("File Saving is not supported with this browser")
            return
        output = csvService.format(predictions)
        blob = new Blob([output], {type: "text/csv;charset=utf-8"})
        saveAs(blob, fileName)

    return {
        restrict: "EA"
        scope:{
            predictions:'='
            predictionsUnfiltered:'='
            mayOpenFile:'='
        }
        templateUrl: "templates/save-csv-menu.html"
        link: (scope, element, attrs) ->
            console.log("Save-Csv!!!", arguments)
            scope.status = {isopen: false}
            window.TESTCSVBTN = scope
            element.find(".save-filter-results-anchor").click (e) ->
                saveCsv(scope.predictions, element, "filtered-results.csv")
            element.find(".save-all-results-anchor").click (e) ->
                saveCsv(scope.predictionsUnfiltered, element, "all-results.csv")
            element.find(".open-file-anchor").click (e) ->
                element.find("#file-opener").click()
            element.find("#file-opener").change (e) ->
                fileReader = new FileReader()
                fileReader.onload = (e) ->
                    fileContents = e.target.result
                    parsedData = d3.csv.parse(fileContents)
                    registerDataChange(parsedData)
                fileReader.readAsText(@files[0], 'UTF-8');
                console.log "reading file"

    }


]