# scientific-notation.coffee
# A filter to intelligently apply Scientific Notation to a number

angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").filter "scientificNotation",
() ->
    (input, options = {}) ->
        decimals = options.decimals || 5
        # Account for lead integer and decimal point by adding 2
        fractionSize = (options.fraction || 5) + 2
        input = parseFloat(input) if (typeof input) is not "number"
        stringForm = input.toString()
        [integer, mantissa] = stringForm.split(".")
        if mantissa.length > decimals
            sciNot = input.toExponential()
            [fractional, exponent] = sciNot.split(/e/)
            fractional = fractional.slice(0,fractionSize) if fractional.length > fractionSize
            return fractional + "e" + exponent
        else
            return input