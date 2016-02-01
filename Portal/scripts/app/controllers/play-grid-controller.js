(function () {
    'use strict';
    app.requires.push('akoenig.deckgrid');
    app.controller('playGridController', ["$scope", "apiService", "serverAddressService", "$location", PlayGridController]);

    function PlayGridController(scope, service, addressService, location) {
        scope.inputConfiguration = null;
        scope.configuration = null;
        scope.useDesktopWindow = false;
        scope.useForegroundWindow = true;
        scope.processName = "";
        scope.columns = 0;
        scope.rows = 0;

        scope.cellStyle = function (action) {
            var s = {};
            s["background-color"] = action.color;
            s["color"] = action.textColor;
            s["width"] = 100 / scope.inputConfiguration.gridSettings.columns + '%';
            s["height"] = 100 / scope.inputConfiguration.gridSettings.rows + '%';
            if (action.mediaItem.fileName.length > 0) {
                s["background-image"] = "url("+ addressService.serverHost + "media/" + scope.inputConfiguration.trimmedName + "/" + action.mediaItem.fileName + ")";
                s["background-size"] = action.mediaItem.size;
                s["background-repeat"] = action.mediaItem.repeat;
                s["background-position"] = action.mediaItem.positionLeft + " " + action.mediaItem.positionTop;
            }
            return s;
        }

        scope.execute = function (action) {
            if (action.sequences.length === 0
                && action.mouseSequences.length === 0
                && action.scriptSequences.length === 0
                )
                return;

            if (!action.isEnabled)
                return;

            var executionContainer = {
                useForegroundWindow: scope.useForegroundWindow,
                useDesktop: scope.useDesktopWindow,
                processName: scope.processName,
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
                    scope.inputConfiguration = result.content;
                    scope.inputConfiguration.trimmedName = scope.inputConfiguration.trimmedName = scope.inputConfiguration.name.replace(/\s+/g, '');
                    var actions = result.content.actions;
                    scope.columns = result.content.gridSettings.columns;
                    scope.rows = result.content.gridSettings.rows;
                    scope.processName = result.content.processName;
                    scope.useDesktopWindow = result.content.useDesktopWindow;
                    scope.useForegroundWindow = result.content.useForegroundWindow;
                    scope.configuration = listToMatrix(scope.rows, scope.columns, actions);
                });
            }
        }

        
        init();
    }

})();