#cluster-color.coffee
# This directive was previously used to apply the c0-c5 coloring classes, but was found
# to have issues with the ngGrid row binding. It was replaced by direct textual interpolation
# in the "class" attribute of the template

(GlycReSoftMSMSGlycopeptideResultsViewApp.directive "clusterColor",
    () -> return {
        restrict: "A"
        scope: {
            match:"="
        }
        link: (scope, element, attrs) ->
            console.log(scope.match.groupBy)
            caliper = scope.match.groupBy % 6
            element.addClass("c" + caliper)
    })
