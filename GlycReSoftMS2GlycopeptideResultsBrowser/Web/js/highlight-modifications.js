angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").filter("highlightModifications", function() {
  return function(input, colorMap) {
    var frag, fragments, modName, out, regex, _i, _len;
    if (input == null) {
      input = '';
    }
    if (colorMap == null) {
      colorMap = {};
    }
    out = "";
    regex = /(\(.+?\)|\[.+?\])/;
    fragments = input.split(regex);
    for (_i = 0, _len = fragments.length; _i < _len; _i++) {
      frag = fragments[_i];
      if (frag.charAt(0) === "(") {
        modName = frag.replace(/\(|\)/g, "");
        if (colorMap[modName] != null) {
          out += "<span class='mod-string' style='color:" + colorMap[modName] + "'>" + frag + "</span>";
        } else {
          out += "<span class='mod-string css-" + modName + "'>" + frag + "</span>";
        }
      } else if (frag.charAt(0) === "[") {
        out += " <b>" + frag + "</b>";
      } else {
        out += frag;
      }
    }
    return out;
  };
});
