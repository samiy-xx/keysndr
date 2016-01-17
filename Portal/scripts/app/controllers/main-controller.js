(function () {
    'use strict';
    app.controller('mainController', ["$scope", "$timeout", "apiService", MainController]);

    function MainController(scope, $timeout, service) {
        scope.hasError = false;
        scope.header = "";
        scope.message = "";
        scope.errorMessage = "";
        scope.appSettings = null;

        scope.displaySuccessMessage = function(messageHeader, messageBody, milliseconds) {
            scope.header = messageHeader;
            scope.message = messageBody;
            $timeout(function () { scope.reset(); }, milliseconds);
        }

        scope.displayErrorMessage = function (messageHeader, messageBody, milliseconds) {
            scope.header = messageHeader;
            scope.errorMessage = messageBody;
            scope.hasError = true;
            $timeout(function () { scope.reset(); }, milliseconds);
        }

        scope.reset = function() {
            scope.hasError = false;
            scope.message = "";
            scope.header = "";
            scope.errorMessage = "";
        }

        scope.getAppSettings = function () {
            service.getSettings().then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to load settings", result.errorMessage, 5000);
                    return;
                }
                scope.appSettings = result.content;
            });
        };
        scope.getAppSettings();
    }
})();