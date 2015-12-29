(function () {
    'use strict';
    app.controller('editViewController', ["$scope", "apiService", "$location", EditViewController]);

    function EditViewController(scope, service, $location) {

        

        scope.addAction = function() {
            service.getNewInputAction().then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to generate new action", result.errorMessage, 5000);
                    return;
                }
                scope.inputConfiguration.actions.push(result.content);
                var l = scope.inputConfiguration.actions.length;
                scope.setCurrentAction(scope.inputConfiguration.actions[l - 1]);
            });
        }

        scope.deleteAction = function(index) {
            scope.inputConfiguration.actions.removeAt(index);
        }

        

        scope.deleteCurrentAction = function () {
            service.getNewInputAction().then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to generate new action", result.errorMessage, 5000);
                    return;
                }
                var i = scope.inputConfiguration.actions.map(function (e) { return e.name; }).indexOf(scope.currentAction.name);
                if (i > -1) {
                    scope.inputConfiguration.actions[i] = result.content;
                    scope.wipeCurrentAction();
                }
            });
        }

        scope.init(1);
    }
})();