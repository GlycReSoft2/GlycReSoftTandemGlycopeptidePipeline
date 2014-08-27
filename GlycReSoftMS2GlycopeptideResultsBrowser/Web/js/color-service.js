angular.module("GlycReSoftMSMSGlycopeptideResultsViewApp").service("colorService", [
  function() {
    this.colors = ["blue", "rgb(228, 211, 84)", "red", "purple", "grey", "black", "green", "orange", "brown"];
    this._colorIter = 0;
    this.colorMap = {
      Peptide: "seagreen",
      HexNAc: "#CC99FF"
    };
    this._nextColor = function() {
      var color;
      color = this.colors[this._colorIter++];
      if (this._colorIter >= this.colors.length) {
        this._colorIter = 0;
      }
      return color;
    };
    this.getColor = function(label) {
      if (!(label in this.colorMap)) {
        this.colorMap[label] = this._nextColor();
      }
      return this.colorMap[label];
    };
    return console.log(this);
  }
]);
