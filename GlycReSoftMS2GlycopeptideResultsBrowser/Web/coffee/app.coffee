# Angular Application Definition
GlycReSoftMSMSGlycopeptideResultsViewApp = angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp", [
    "ui.bootstrap",
    "ngGrid",
    "ngSanitize"
])

Array::sum = ->
    total = 0
    for i in @
        total += i
    total

Array::mean = ->
    total = @sum()
    total / @length