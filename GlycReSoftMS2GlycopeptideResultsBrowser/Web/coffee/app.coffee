# Angular Application Definition
GlycReSoftMSMSGlycopeptideResultsViewApp = angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp", [
    "ui.bootstrap",
    "ngGrid",
    "ngSanitize"
])

# Simple mathematics
Array::sum = ->
    total = 0
    for i in @
        total += i
    total

Array::mean = ->
    total = @sum()
    total / @length

# Number.isInteger is not implemented in IE
if not Number.isInteger?
    Number.isInteger = (nVal) -> 
        typeof nVal is "number" and isFinite(nVal) and nVal > -9007199254740992 and 
        nVal < 9007199254740992 and Math.floor(nVal) == nVal