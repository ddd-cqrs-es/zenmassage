// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397705
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
var ZenMassageApp;
(function (ZenMassageApp) {
    "use strict";
    var Application;
    (function (Application) {
        function initialize() {
            document.addEventListener('deviceready', onDeviceReady, false);
        }
        Application.initialize = initialize;
        function onDeviceReady() {
            // Handle the Cordova pause and resume events
            document.addEventListener('pause', onPause, false);
            document.addEventListener('resume', onResume, false);
            // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
            window.plugins.Pebble.setAppUUID('29207e29-1f35-4f89-9871-0a579e84d105', function (info) {
                navigator.notification.alert('watch app linked', function () { });
            }, function (error) {
                navigator.notification.alert('watch app not linked', function () { });
            });
        }
        function onPause() {
            // TODO: This application has been suspended. Save application state here.
        }
        function onResume() {
            // TODO: This application has been reactivated. Restore application state here.
        }
    })(Application = ZenMassageApp.Application || (ZenMassageApp.Application = {}));
    window.onload = function () {
        Application.initialize();
    };
})(ZenMassageApp || (ZenMassageApp = {}));
//# sourceMappingURL=index.js.map