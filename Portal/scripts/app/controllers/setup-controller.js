(function () {
    'use strict';
    app.controller('setupController', ["$scope", "apiService", "$timeout" ,SetupController]);

    function SetupController(scope, service, $timeout) {
        scope.stage = 1;
        scope.config = null;
        scope.displayLoading = false;
        scope.loadCount = 0;

        var stop = null;

        scope.canAdvance = function () {
            if (scope.stage === 1) {
                if (scope.config === null)
                    return false;
                return scope.config.dataFolder.length > 0;
            }

            if (scope.stage === 2) {
                return true;
            }

            return false;
        }
        
        scope.advance = function() {
            if (!scope.canAdvance())
                return;
            scope.stage++;
        }

        scope.goBack = function() {
            if (scope.stage <= 1)
                return;

            scope.stage--;
        }

        scope.reload = function () {
            service.saveSettings(scope.config).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to save app config", result.errorMessage, 5000);
                    return;
                }
                scope.displayLoading = true;
                scope.requestReload();
                $timeout(function () { scope.checkIfUp(); }, 3000);
            });
            
        }

        scope.checkIfUp = function() {
            service.getSettings().then(function (response) {
                var result = response.data;
                $timeout.cancel(stop);
                window.location.href = "index.html?" + (new Date()).getTime();
            }, function (r) {
                scope.loadCount++;
                $timeout(function () { scope.checkIfUp(); }, 2000);
            });
        }

        scope.requestReload = function() {
            service.signalReload().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Error requesting systemm reload", result.errorMessage, 5000);
                    return;
                }
            });
        }

        scope.init = function() {
            service.getSettings().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to load app settings", result.errorMessage, 5000);
                    return;
                }
                scope.config = result.content;
            });
        }

        scope.init();
    }

})();