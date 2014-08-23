var resizeable;

resizeable = GlycReSoftMSMSGlycopeptideResultsViewApp.directive('resizable', function($window) {
  return {
    restrict: "A",
    scope: {
      percent: "=windowPercent"
    },
    link: function(scope, element) {
      var windowPercent;
      windowPercent = scope.percent;
      scope.initializeWindowSize = function() {
        scope.windowHeight = $window.innerHeight * windowPercent;
        scope.windowWidth = $window.innerWidth;
        return element.css("height", scope.windowHeight + "px");
      };
      scope.initializeWindowSize();
      return angular.element($window).bind('resize', function() {
        scope.initializeWindowSize();
        return scope.$apply();
      });
    }
  };
});
