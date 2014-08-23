GlycReSoftMSMSGlycopeptideResultsViewApp.service("csvService", [
  "$window", function($window) {
    this.serializedFields = ["Oxonium_ions", "Stub_ions", "bare_b_ions", "bare_y_ions", "b_ion_coverage", "y_ion_coverage", "b_ions_with_HexNAc", "y_ions_with_HexNAc", "startAA", "endAA", "vol", "numOxIons", "numStubs", "bestCoverage", "meanCoverage", "percentUncovered", "peptideLens", "MS1_Score", "MS2_Score", "Obs_Mass", "Calc_mass", 'ppm_error', 'abs_ppm_error'];
    this.parse = function(stringData) {
      var instantiatedData, rowData;
      rowData = d3.csv.parse(stringData);
      instantiatedData = this.deserializeAfterParse(rowData);
      return instantiatedData;
    };
    this.format = function(rowData) {
      var serializedData, stringData;
      serializedData = this.serializeBeforeFormat(rowData);
      stringData = d3.csv.format(serializedData);
      return stringData;
    };
    this.deserializeAfterParse = function(predictions) {
      var self;
      self = this;
      _.forEach(predictions, function(obj) {
        _.forEach(self.serializedFields, function(field) {
          return obj[field] = angular.fromJson(obj[field]);
        });
        obj.call = obj.call === "Yes" ? true : false;
        obj.ambiguity = obj.ambiguity === "True" ? true : false;
        obj.groupBy = 0;
        return obj;
      });
      return predictions;
    };
    this.serializeBeforeFormat = function(predictions) {
      var self;
      self = this;
      predictions = _.cloneDeep(predictions);
      _.forEach(predictions, function(obj) {
        _.forEach(self.serializedFields, function(field) {
          return obj[field] = angular.toJson(obj[field]);
        });
        obj.call = obj.call ? "Yes" : "No";
        obj.ambiguity = obj.ambiguity ? "True" : "False";
        obj.groupBy = 0;
        return obj;
      });
      return predictions;
    };
    return $window.csvService = this;
  }
]);
