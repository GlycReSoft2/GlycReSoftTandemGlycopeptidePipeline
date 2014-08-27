var GlycReSoftMSMSGlycopeptideResultsViewApp;

GlycReSoftMSMSGlycopeptideResultsViewApp = angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp", ["ui.bootstrap", "ngGrid", "ngSanitize"]);

Array.prototype.sum = function() {
  var i, total, _i, _len;
  total = 0;
  for (_i = 0, _len = this.length; _i < _len; _i++) {
    i = this[_i];
    total += i;
  }
  return total;
};

Array.prototype.mean = function() {
  var total;
  total = this.sum();
  return total / this.length;
};
