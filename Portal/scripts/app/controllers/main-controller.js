(function () {
    'use strict';
    app.controller('mainController', ["$scope", "$timeout", "apiService", MainController]);

    function MainController(scope, $timeout, service) {
        scope.hasError = false;
        scope.header = "";
        scope.message = "";
        scope.errorMessage = "";


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
    }
})();