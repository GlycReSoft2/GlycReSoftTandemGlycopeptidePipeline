var fs = require("fs");
module.exports = function(grunt){
    grunt.initConfig({
        pkg: grunt.file.readJSON("package.json"),
        coffee: {
            compile: {
                options: {
                    sourceMap: false,
                    bare: true,
                },
                expand: true,
                flatten: true,
                cwd: "coffee",
                src: ["**/*.coffee"],
                dest: "js",
                ext: '.js',
            }
        },

        less: {
            compile: {
                options: {
                  paths: [
                    "css/",
                  ]
                },
                files: {
                    "css/style.css": "css/style.less",
                }
            }
        },

        concat: {
            options: {
                separator: ";\n",
                banner: "",
            },
            jsVendorGlobals: {
                src: [
                      'js/vendor/addEventListenerPolyfill.js',
                      'js/vendor/es5-sham.min.js',
                      'js/vendor/es5-shim.min.js',
                      'js/vendor/jquery.min.js',
                      'js/vendor/angular.min.js',
                      //'bower_components/angular/angular.js',

                      'js/vendor/lodash.min.js',
                      'js/vendor/d3.min.js',

                      'js/vendor/highcharts.js',
                      'js/vendor/highcharts-more.js',
                      'js/vendor/highcharts-exporting.js',
                      'js/vendor/highcharts-data.js',

                      'js/vendor/filtrex.js',
                      'js/vendor/FileSaver.js',


                      'js/vendor/biojs/jquery-migrate.min.js',
                      'js/vendor/biojs/jquery-ui-1.8.2.custom.min.js',
                      'js/vendor/biojs/Biojs.js',
                      'js/vendor/biojs/Biojs.FeatureViewer.js',
                      'js/vendor/biojs/jquery.tooltip.js',
                      'js/vendor/biojs/raphael.js',
                      'js/vendor/biojs/canvg.js',
                      'js/vendor/biojs/rgbcolor.js',


                      'js/vendor/angular-ui.min.js',
                      'js/vendor/angular-ui-ieshiv.min.js',
                      'js/vendor/ng-grid-2.0.12.min.js',
                      'js/vendor/ui-bootstrap-0.11.0.min.js',
                      'js/vendor/ui-bootstrap-tpls-0.11.0.min.js',
                      'js/vendor/angular-sanitize.min.js',

                      ],
              dest: "js/vendor/vendor.concat.js",
            },
            jsApp: {
                src: [
                      //Controllers and Applications
                      'js/app.js',
                      'js/results-representation.js',
                      'js/modal.js',

                      //Services
                      'js/csv-service.js',
                      'js/color-service.js',

                      //View Directives
                      'js/protein-sequence-view.js',
                      'js/ambiguity-plot.js',

                      //Component Directives
                      'js/fragment-ion.js',
                      'js/resizeable.js',
                      'js/save-csv.js',
                      'js/html-popover.js',
                      'js/help-menu.js',

                      //Filters
                      'js/highlight-modifications.js',
                      'js/scientific-notation.js',

                     ],
                dest: "js/app.concat.js"
            },
            css: {
              src: ["css/vendor/*.css", "css/vendor/biojs/*.css",  "css/style.css"],
              dest: "css/main.css"
            }
        },

        watch: {
            coffee: {
                files: [
                    "**/*.coffee",
                ],
                tasks: ["coffee", "concat:jsApp"]
            },
            less: {
                files: [
                    "css/style.less",
                ],
                tasks: ["less", 'concat:css'],
            }
        },
    })

    grunt.loadNpmTasks('grunt-contrib-coffee');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-ng-annotate');

    grunt.registerTask('default', ['coffee', "less", "concat", "watch"]);
}