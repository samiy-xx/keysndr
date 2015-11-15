(function () {
    'use strict';
    app.controller('scriptsController', ["$scope", "apiService", ScriptsController]);

    function ScriptsController(scope, service) {
        scope.scripts = [];
        scope.currentScript = null;

        scope.setSelected = function(script) {
            scope.currentScript = script;
        }

        scope.getAllScripts = function() {
            service.getAllScriptObjects().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.hasError = true;
                    scope.message = "Failed to get scripts";
                    scope.errorMessage = result.errorMessage;
                    return;
                }
                scope.scripts = result.content;
            });
        }

        scope.getAllScripts();
    }

})();