(function () {
    'use strict';
    app.controller('editGridController', ["$scope", "apiService", "$location", EditGridController]);

    function EditGridController(scope, service, location) {
        scope.configuration = null;
        
        scope.deleteCurrentAction = function() {
            service.getNewInputAction().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.hasError = true;
                    scope.message = "Failed to generate new action";
                    scope.errorMessage = result.errorMessage;
                    return;
                }
                var i = scope.inputConfiguration.actions.map(function (e) { return e.name; }).indexOf(scope.currentAction.name);
                if (i > -1) {
                    scope.currentAction = null;
                    scope.inputConfiguration.actions[i] = result.content;
                }
                scope.configuration = listToMatrix(scope.inputConfiguration.actions, 5);
            }); 
        }

        scope.$watch("inputConfiguration.actions", function (n, o) {
            if (scope.inputConfiguration === null)
                return;
            scope.configuration = listToMatrix(scope.inputConfiguration.actions, 5);
        });
        /*function init() {
            var s = location.search();
            if (s !== null && s !== undefined && s.hasOwnProperty("name")) {
                service.getConfiguration(s.name).then(function(response) {
                    var result = response.data;
                    if (!result.success) {
                        scope.hasError = true;
                        scope.errorMessage = "Failed to get actions. Is the server running?";
                        return;
                    }
                    var actions = result.content.actions;
                    scope.inputConfiguration = result.content;
                    scope.configuration = listToMatrix(actions, 5);
                });
            } else {
                service.getNewConfiguration(30).then(function (response) {
                    var result = response.data;
                    if (!result.success) {
                        scope.hasError = true;
                        scope.errorMessage = "Failed to get actions. Is the server running?";
                        return;
                    }
                    var actions = result.content.actions;
                    scope.inputConfiguration = result.content;
                    scope.configuration = listToMatrix(actions, 5);
                });
            }

            service.getAllScripts().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.hasError = true;
                    scope.errorMessage = "Failed to get scripts. " + result.errorMessage;
                    return;
                }
                scope.availableScripts = result.content;
            });
        }


        init();*/
    }

})();