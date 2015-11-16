(function () {
    'use strict';
    app.controller('scriptsController', ["$scope", "apiService", ScriptsController]);

    function ScriptsController(scope, service) {
        scope.scripts = [];
        scope.currentScript = null;

        scope.$watch("currentScript.name", function (n, o) {
            if (scope.currentScript === null)
                return;
            scope.currentScript.fileName = n.replace(/\s+/g, '') + ".script";
        });

        scope.exitConfig = function() {
            window.location.href = "index.html";
        }

        scope.saveScript = function() {
            service.saveScript(scope.currentScript).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to save script", scope.errorMessage, 5000);
                    return;
                }
                scope.displaySuccessMessage("Script saved", "Ok", 5000);
            });
        }

        scope.validateScript = function() {
            service.validateScript(scope.currentScript).then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Script did not validate", scope.errorMessage, 5000);
                    return;
                }
                scope.displaySuccessMessage("Script validated", "Ok", 5000);
            });
        }

        scope.setSelected = function(script) {
            scope.currentScript = script;
        }

        scope.getAllScripts = function() {
            service.getAllScriptObjects().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.setErrorMessage("Failed to get scripts", scope.errorMessage, 5000);
                    return;
                }
                scope.scripts = result.content;
            });
        }

        scope.getAllScripts();
    }

})();