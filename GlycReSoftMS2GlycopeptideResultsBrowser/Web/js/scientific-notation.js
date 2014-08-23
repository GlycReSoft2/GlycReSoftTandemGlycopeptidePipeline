angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").filter("scientificNotation", function() {
  return function(input, options) {
    var decimals, exponent, fractionSize, fractional, integer, mantissa, sciNot, stringForm, _ref, _ref1;
    if (options == null) {
      options = {};
    }
    decimals = options.decimals || 5;
    fractionSize = (options.fraction || 5) + 2;
    if ((typeof input) === !"number") {
      input = parseFloat(input);
    }
    stringForm = input.toString();
    _ref = stringForm.split("."), integer = _ref[0], mantissa = _ref[1];
    if (mantissa.length > decimals) {
      sciNot = input.toExponential();
      _ref1 = sciNot.split(/e/), fractional = _ref1[0], exponent = _ref1[1];
      if (fractional.length > fractionSize) {
        fractional = fractional.slice(0, fractionSize);
      }
      return fractional + "e" + exponent;
    } else {
      return input;
    }
  };
});
