var fragmentIon;

fragmentIon = GlycReSoftMSMSGlycopeptideResultsViewApp.directive("fragmentIon", function() {
  return {
    restrict: "AE",
    template: "<p class='fragment-ion-tag'><b>PPM Error</b>: {{fragment_ion.ppm_error|scientificNotation}} &nbsp; <b>Key</b>: {{fragment_ion.key}}</p>"
  };
});