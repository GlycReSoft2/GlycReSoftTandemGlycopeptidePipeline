angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").filter("highlightModifications", [
  "colorService", "$sce", function(colorService, $sce) {
    return function(input, sce) {
      var frag, fragments, modName, out, regex, _i, _len;
      if (input == null) {
        input = '';
      }
      if (sce == null) {
        sce = true;
      }
      out = "";
      regex = /(\(.+?\)|\[.+?\])/;
      fragments = input.split(regex);
      for (_i = 0, _len = fragments.length; _i < _len; _i++) {
        frag = fragments[_i];
        if (frag.charAt(0) === "(") {
          modName = frag.replace(/\(|\)/g, "");
          out += "<span class='mod-string' style='color:" + (colorService.getColor(modName)) + "'>" + frag + "</span>";
        } else if (frag.charAt(0) === "[") {
          out += " <b>" + frag + "</b>";
        } else {
          out += frag;
        }
      }
      if (sce) {
        out = $sce.trustAsHtml(out);
      }
      return out;
    };
  }
]);
