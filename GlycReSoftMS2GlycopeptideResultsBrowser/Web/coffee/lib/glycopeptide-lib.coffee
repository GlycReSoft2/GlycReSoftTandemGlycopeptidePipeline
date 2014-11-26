class GlycopeptideLib
    @buildBackboneStack: (glycopeptide) ->
        stack = ({
            bIon: [0,0],
            yIon: [0,0],
            bHexNAc: [0,0],
            yHexNAc: [0,0]
            } for i in [0..glycopeptide.peptideLens])
        len = glycopeptide.peptideLens

        for bIon in glycopeptide.b_ion_coverage
            key = bIon.key
            index = parseInt(key.replace(/B/, ''))
            stack[index].bIon = [0, index]

        for yIon in glycopeptide.y_ion_coverage
            key = yIon.key
            index = parseInt(key.replace(/Y/, ''))
            stack[len - index].yIon = [len - index , len]

        for bHexNAc in glycopeptide.b_ions_with_HexNAc
            key = bHexNAc.key
            index = parseInt(/B([0-9]+)\+/.exec(key)[1])
            stack[index].bHexNAc = [0, index]

        for yHexNAc in glycopeptide.y_ions_with_HexNAc
            key = yHexNAc.key
            index = parseInt(/Y([0-9]+)\+/.exec(key)[1])
            stack[len - index].yHexNAc = [len - index, len]

        return stack

    @parseModificationSites: (glycopeptide) ->
            sequence = glycopeptide.Glycopeptide_identifier
            regex = /(\(.+?\)|\[.+?\])/
            index = 0
            fragments = sequence.split(regex)
            modifications = []
            for frag in fragments
                if frag.charAt(0) is "["
                    # Ignore the Glycan Identifier Chunk
                else if frag.charAt(0) is "("
                    # This is a modification site
                    label = frag.replace(/\(|\)/g, "")
                    feature = name: label, position: index
                    modifications.push feature
                else
                    index += frag.length


            return modifications


class ProteinBackboneSpace
    constructor: (@predictions, @options={}) ->
        @stacks = _.groupBy @predictions, (p) -> [p.startAA, p.endAA]
        for pileup, matches of @stacks
            matches = matches.sort (a, b) ->
                if a.MS2_Score < b.MS2_Score
                    return -1
                else if a.MS2_Score > b.MS2_Score
                    return 1
                return 0

class GlycopeptideLib.Glycopeptide
    constructor: (glycopeptide) ->
        @data = glycopeptide
        console.log(5)


if(module?)
    if not module.exports?
        module.exports = {}

    module.exports.GlycopeptideLib = GlycopeptideLib
    module.exports.ProteinBackboneSpace = ProteinBackboneSpace
