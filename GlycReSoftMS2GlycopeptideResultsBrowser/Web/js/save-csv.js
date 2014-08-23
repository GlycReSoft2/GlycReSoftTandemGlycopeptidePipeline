angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").directive("saveCsv", [
  "csvService", function(csvService) {
    var saveCsv;
    saveCsv = function(predictions, element) {
      var blob, output;
      if (!((typeof Blob !== "undefined" && Blob !== null) && (typeof saveAs !== "undefined" && saveAs !== null))) {
        alert("File Saving is not supported with this browser");
        return;
      }
      output = csvService.format(predictions);
      blob = new Blob([output], {
        type: "text/csv;charset=utf-8"
      });
      return saveAs(blob, "results.csv");
    };
    return {
      link: function(scope, element, attrs) {
        console.log("Save-Csv", arguments);
        return element.click(function() {
          return saveCsv(scope._predictionsReceiver, element);
        });
      }
    };
  }
]);
