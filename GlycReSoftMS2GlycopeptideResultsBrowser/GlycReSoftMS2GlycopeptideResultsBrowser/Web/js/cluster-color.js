GlycReSoftMSMSGlycopeptideResultsViewApp.directive("clusterColor", function() {
  return {
    restrict: "A",
    scope: {
      match: "="
    },
    link: function(scope, element, attrs) {
      var caliper;
      console.log(scope.match.groupBy);
      caliper = scope.match.groupBy % 6;
      return element.addClass("c" + caliper);
    }
  };
});
