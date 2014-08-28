angular.module('GlycReSoftMSMSGlycopeptideResultsViewApp').directive("popoverHtmlUnsafePopup", function() {
  return {
    restrict: "EA",
    replace: true,
    scope: {
      title: "@",
      content: "@",
      placement: "@",
      animation: "&",
      isOpen: "&"
    },
    template: '<div class="popover {{placement}}" ng-class="{ in: isOpen(), fade: animation() }"> <div class="arrow"></div> <div class="popover-inner"> <h3 class="popover-title" ng-bind="title" ng-show="title"></h3> <div class="popover-content" bind-html-unsafe="content"></div> </div> </div>'
  };
}).directive("popoverHtmlUnsafe", [
  "$tooltip", function($tooltip) {
    console.log(arguments);
    return $tooltip("popoverHtmlUnsafe", "popover", "click");
  }
]);
