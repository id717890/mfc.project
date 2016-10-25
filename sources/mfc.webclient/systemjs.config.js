/**
 * System configuration for Angular samples
 * Adjust as necessary for your application needs.
 */
(function (global) {
    System.config({
        paths: {
            // paths serve as alias
            'npm:': 'node_modules/'
        },
        // map tells the System loader where to look for things
        map: {
            // our app is within the app folder
            app: 'app',
            // angular bundles
            '@angular/core': 'npm:@angular/core/bundles/core.umd.js',
            '@angular/common': 'npm:@angular/common/bundles/common.umd.js',
            '@angular/compiler': 'npm:@angular/compiler/bundles/compiler.umd.js',
            '@angular/platform-browser': 'npm:@angular/platform-browser/bundles/platform-browser.umd.js',
            '@angular/platform-browser-dynamic': 'npm:@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
            '@angular/http': 'npm:@angular/http/bundles/http.umd.js',
            '@angular/router': 'npm:@angular/router/bundles/router.umd.js',
            '@angular/forms': 'npm:@angular/forms/bundles/forms.umd.js',
            // other libraries
            'rxjs': 'npm:rxjs',
            'core-js': 'npm:core-js',
            'ts-metadata-helper': 'npm:ts-metadata-helper',
            'angular-in-memory-web-api': 'npm:angular-in-memory-web-api',
            'angular2-modal': 'npm:angular2-modal/bundles/angular2-modal.umd.js',
            'angular2-modal/plugins/bootstrap': 'npm:angular2-modal/bundles/angular2-modal.bootstrap.umd.js',
            'angular2-dynamic-component': 'npm:angular2-dynamic-component',
            'angular2-busy': 'node_modules/angular2-busy',
            'mydatepicker': 'npm:mydatepicker',
            'ng2-pagination': 'npm:ng2-pagination'
        },
        // packages tells the System loader how to load when no filename and/or no extension
        packages: {
            app: {
                main: './main.js',
                defaultExtension: 'js'
            },
            rxjs: {
                defaultExtension: 'js'
            },
            'angular-in-memory-web-api': {
                main: './index.js',
                defaultExtension: 'js'
            },
            'core-js': {
                defaultExtension: 'js'
            },
            'ts-metadata-helper': {
                defaultExtension: 'js'
            },
            'angular2-dynamic-component': {
                defaultExtension: 'js'
            },
            'angular2-busy': { main: 'index.js', defaultExtension: 'js' },
            mydatepicker: {
                main: './index.js',
                defaultExtension: 'js'
            },
            'ng2-pagination': { main: 'index.js', defaultExtension: 'js' },
        }
    });
})(this);