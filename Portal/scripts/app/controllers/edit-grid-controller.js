(function () {
    'use strict';
    app.controller('editGridController', ["$scope", "apiService", "$location", EditGridController]);

    function EditGridController(scope, service, location) {
        scope.inputConfiguration = null;
        scope.configuration = null;
        scope.currentAction = null;

        scope.hasErrors = false;
        scope.message = null;
        scope.errorMessage = null;

        scope.setForEdit = function(a) {
            scope.currentAction = a;
        }

        scope.$watch("inputConfiguration.name", function (n, o) {
            if (scope.inputConfiguration === null)
                return;
            scope.inputConfiguration.fileName = n.replace(/\s+/g, '') + ".json";
        });

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
        }


        init();
    }

})();