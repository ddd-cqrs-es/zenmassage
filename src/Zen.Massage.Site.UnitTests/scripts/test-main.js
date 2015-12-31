var tests = [];
for (var file in window.__karma__.files) {
    if (window.__karma__.files.hasOwnProperty(file)) {
        if (/.*Test\.js$/.test(file)) {
            //By default Karma test files are treated as absolute path dependencies
            //which means that all relative dependencies are resolved also as path dependencies without extension:
            //(../Services/ColorCalculator) is resolved as (/base/FrontEndTools.WebUI/Services/ColorCalculator)
            //To fix it we should make test files "relative" but this will cause karma's "no timestamp error"
            tests.push('.' + file);
        }
    }
}

// Register all-tests alias using the tests as dependencies
System.register('all-tests', tests);

// Import the unit tests and execute once promise has been fulfilled
System.import('all-tests')
    .then(function() {
        window.__karma__.start();
    });
