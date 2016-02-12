'use strict';

var GulpConfig = (function () {
    function gulpConfig() {
        this.paths = {
            root: './',
            webroot: './wwwroot/'
        };

        this.paths.nodeModulesRoot = this.paths.root + 'node_modules/';

        this.paths.tsRoot = './typescript/';
        this.paths.tsOutputPath = this.paths.webroot + 'js/';

        this.paths.tsApp = this.paths.tsRoot + 'app/';
        this.paths.tsAppSelector = this.paths.tsApp + '**/*.ts';
        this.paths.tsTypings = this.paths.tsRoot + 'typings/';
        this.paths.tsTypingsSelector = this.paths.tsTypings + '**/*.ts';

        this.paths.jsLibPath = this.paths.tsOutputPath + 'lib/';

        this.paths.sassRoot = './styles/';
        this.paths.sassApp = this.paths.sassRoot;
        this.paths.sassAppSelector = this.paths.sassApp + '**/*.scss';
        this.paths.sassOutputPath = this.paths.webroot + 'css/';

        this.paths.fontsOutputPath = this.paths.webroot + 'fonts/';

        this.paths.swaggerSourcePath = this.paths.root + '../../artifacts/bin/Zen.Massage.Site/Debug/dnx451/Zen.Massage.Site.xml';
        this.paths.swaggerDestinationPath = this.paths.webroot + 'schemas/api';
    }
    return gulpConfig;
})();
module.exports = GulpConfig;