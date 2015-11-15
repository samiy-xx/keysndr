(function () {
    'use strict';

    app.controller('viewController', ["$scope", "$location", "apiService", ViewController]);

    function ViewController(scope, $location, service) {
        scope.configuration = null;
        scope.isValid = false;


        scope.loadConfiguration = function (name) {
            if (name === undefined)
                return;
            service.getConfiguration(name).then(function (response) {
                var result = response.data;
                if (!result.success)
                    return;

                scope.configuration = result.content;
                scope.isValid = true;
            });
        }

        scope.execute = function (actionName) {
            if (!scope.isValid)
                return;

            var action = scope.find(actionName);
            if (action === null) {
                return;
            }
            var executionContainer = {
                useForegroundWindow: scope.configuration.useForegroundWindow,
                useDesktop: scope.configuration.useDesktop,
                processName: scope.configuration.processName,
                inputAction: action
            };
            service.executeAction(executionContainer).then(function (result) {

            });
        }

        scope.find = function (actionName) {
            for (var i = 0; i < scope.configuration.actions.length; i++) {
                var action = scope.configuration.actions[i];
                if (action.name === actionName)
                    return action;
            }
            return null;
        }
    }
})(app);