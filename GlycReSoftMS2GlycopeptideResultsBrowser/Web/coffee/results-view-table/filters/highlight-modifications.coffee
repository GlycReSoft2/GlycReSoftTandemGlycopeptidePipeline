# highlight-modifications.coffee
# A filter to parse the Glycopeptide_identifier string and mark up the modification sites
# and visually separate the glycan composition tuple.

angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").filter "highlightModifications",
() ->
    return (input = '', colorMap = {}) ->
        out = ""
        regex = /(\(.+?\)|\[.+?\])/
        fragments = input.split(regex)
        for frag in fragments
            if frag.charAt(0) is "(" # Then we are dealing with a modification
                modName = frag.replace(/\(|\)/g,"")
                if colorMap[modName]?
                    out += "<span class='mod-string' style='color:#{colorMap[modName]}'>#{frag}</span>"
                else
                    out += "<span class='mod-string css-#{modName}'>#{frag}</span>"
            else if frag.charAt(0) is "["
                out += " <b>#{frag}</b>"
            else
                out += frag
        return out


