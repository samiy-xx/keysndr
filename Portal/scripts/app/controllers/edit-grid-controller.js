(function () {
    'use strict';
    app.controller('editGridController', ["$scope", "apiService", "$location", EditGridController]);

    function EditGridController(scope, service, $location) {
        scope.configuration = null;
        scope.mediaFileNames = [];

        scope.loadMediaFilenames = function() {
            service.loadMediaFilenames(scope.inputConfiguration).then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to load media files", result.errorMessage, 5000);
                    return;
                }
                scope.mediaFileNames = result.content;
            });
        }

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
                    
                    scope.inputConfiguration.actions[i] = result.content;
                    scope.wipeCurrentAction();
                }
                scope.configuration = listToMatrix(scope.inputConfiguration.actions, scope.rows);
            }); 
        }

        scope.$watch("inputConfiguration.actions", function (n, o) {
            if (scope.inputConfiguration === null)
                return;
            scope.configuration = listToMatrix(
                scope.inputConfiguration.gridSettings.rows,
                scope.inputConfiguration.gridSettings.columns,
                scope.inputConfiguration.actions);
        });
        
        scope.loaded = function() {
            scope.loadMediaFilenames();
        }
        scope.initGrid = function() {
            var c = 30;
            var s = $location.search();
            if (s !== null && s !== undefined && s.hasOwnProperty("rows") && s.hasOwnProperty("columns")) {
                scope.init(s.rows * s.columns, function () {
                    scope.inputConfiguration.gridSettings.columns = s.columns;
                    scope.inputConfiguration.gridSettings.rows = s.rows;
                    scope.loaded();
                });
            } else {
                scope.init(c, function() {
                    scope.loaded();
                });  
            }
            
        }
        scope.initGrid();
        
    }

})();