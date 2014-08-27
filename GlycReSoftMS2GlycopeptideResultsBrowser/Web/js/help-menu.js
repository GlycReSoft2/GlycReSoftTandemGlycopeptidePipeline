angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").directive("helpMenu", [
  "$modal", function($modal) {
    return {
      link: function(scope, element, attrs) {
        console.log("Help", arguments);
        return element.click(function() {
          var modalInstance;
          return modalInstance = $modal.open({
            templateUrl: 'templates/help-text.html',
            size: 'lg'
          });
        });
      }
    };
  }
]);
