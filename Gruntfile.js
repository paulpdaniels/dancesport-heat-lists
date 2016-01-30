module.exports = function(grunt) {

    grunt.loadNpmTasks('grunt-minjson');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-concat');

    grunt.initConfig({
        minjson: {
            compile: {
                files: grunt.file.expandMapping(['competitions/*.json'], 'dist/', {
                    ext: '.min.json'
                })
            }
        },
        concat : {
            main: {
                files: [{
                    'dist/main.min.js' : [
                        'javascripts/libs/lodash.min.js',
                        'javascripts/libs/ui-bootstrap-tpls-0.12.0.min.js',
                        'javascripts/main.min.js'
                    ]
                }]
            }
        },
        uglify: {
            main : {
                files: {
                    'javascripts/main.min.js' : ['javascripts/main.js']
                }
            }
        }
    });

    grunt.registerTask('default', [
        'uglify:main',
        'concat:main',
        //'copy:main',
        'minjson:compile'
    ])


};

