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
        
        scope.init(30);
    }

})();