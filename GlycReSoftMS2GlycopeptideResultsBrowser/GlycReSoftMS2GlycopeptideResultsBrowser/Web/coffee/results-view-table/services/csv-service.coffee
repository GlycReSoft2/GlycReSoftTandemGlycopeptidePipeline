# csv-service.coffee
# Depends upon d3 to provide the Csv parsing features

GlycReSoftMSMSGlycopeptideResultsViewApp.service "csvService", ["$window", ($window) ->


    @serializedFields = ["Oxonium_ions", "Stub_ions", "bare_b_ions", "bare_y_ions", "b_ion_coverage", "y_ion_coverage",
                        "b_ions_with_HexNAc", "y_ions_with_HexNAc", "startAA", "endAA", "vol", "numOxIons", "numStubs",
                        "bestCoverage", "meanCoverage", "percentUncovered", "peptideLens", "MS1_Score", "MS2_Score",
                        "Obs_Mass", "Calc_mass", 'ppm_error', 'abs_ppm_error']


    @parse = (stringData) ->
        rowData = d3.csv.parse(stringData)
        instantiatedData = @deserializeAfterParse(rowData)
        return instantiatedData

    @format = (rowData) ->
        serializedData = @serializeBeforeFormat(rowData)
        stringData = d3.csv.format(serializedData)
        return stringData

    # Translating from CSV leaves many fields as strings or JSON trees that need to be parsed into
    # JS Numbers and Objects
    @deserializeAfterParse = (predictions) ->
        self = this
        _.forEach(predictions, (obj) ->
            _.forEach(self.serializedFields, (field) ->
                obj[field] = angular.fromJson(obj[field]))
            obj.call = if obj.call == "Yes" then true else false
            obj.ambiguity = if obj.ambiguity == "True" then true else false
            obj.groupBy = 0
            #console.log(obj)
            return obj
        )
        return predictions

    @serializeBeforeFormat = (predictions) ->
        self = this
        predictions = _.cloneDeep(predictions)
        _.forEach(predictions, (obj) ->
            _.forEach(self.serializedFields, (field) ->
                obj[field] = angular.toJson(obj[field]))
            obj.call = if obj.call then "Yes" else "No"
            obj.ambiguity = if obj.ambiguity then "True" else "False"
            obj.groupBy = 0
            #console.log(obj)
            return obj
        )
        return predictions

    $window.csvService = this
]

