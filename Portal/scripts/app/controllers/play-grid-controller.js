(function () {
    'use strict';
    app.requires.push('akoenig.deckgrid');
    app.controller('playGridController', ["$scope", "apiService", "$location", PlayGridController]);

    function PlayGridController(scope, service, location) {
        scope.configuration = null;

        scope.execute = function (action) {
            if (action.sequences.length === 0
                && action.mouseSequences.length === 0
                && action.scriptSequences.length === 0)
                return;
            var executionContainer = {
                useForegroundWindow: true,
                useDesktop: false,
                processName: "",
                inputAction: action
            };
            service.executeAction(executionContainer).then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.hasErrors = true;
                    scope.errorMessage = result.errorMessage;
                }
            });
        } 

        function listToMatrix(list, elementsPerSubArray) {
            var matrix = [], i, k;
            for (i = 0, k = -1; i < list.length; i++) {
                if (i % elementsPerSubArray === 0) {
                    k++;
                    matrix[k] = [];
                }
                matrix[k].push(list[i]);
            }
            return matrix;
        }

        function init() {
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

                    scope.configuration = listToMatrix(actions, 5);
                });
            }
        }

        
        init();
    }

})();