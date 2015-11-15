(function () {
    'use strict';
    app.controller('editGridController', ["$scope", "apiService", "$location", EditGridController]);

    function EditGridController(scope, service, location) {
        scope.inputConfiguration = null;
        scope.configuration = null;
        scope.currentAction = null;
        scope.keys = AppKeys;
        scope.keyModifiers = AppModifiers;
        scope.winModifiers = WinModifiers;
        scope.availableScripts = [];

        scope.setForEdit = function(a) {
            scope.currentAction = a;
        }

        scope.$watch("inputConfiguration.name", function (n, o) {
            if (scope.inputConfiguration === null)
                return;
            scope.inputConfiguration.fileName = n.replace(/\s+/g, '') + ".json";
        });

        scope.saveConfiguration = function() {
            service.saveConfiguration(scope.inputConfiguration).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.hasError = true;
                    scope.message = "Failed to save configuration";
                    scope.errorMessage = result.errorMessage;
                    return;
                }
                scope.message = "Configuration saved";
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
                    scope.currentAction = null;
                    scope.inputConfiguration.actions[i] = result.content;
                }
                scope.configuration = listToMatrix(scope.inputConfiguration.actions, 5);
            }); 
        }

        scope.deleteScript = function(index) {
            scope.currentAction.scriptSequences.splice(index, 1);
        }

        scope.deleteSequence = function(index) {
            scope.currentAction.sequences.removeAt(index);
        }

        scope.moveSequence = function(index, dir) {
            var nextIndex = index + dir;
            if (nextIndex < 0)
                return;
            if (nextIndex > scope.currentAction.sequences.length - 1)
                return;

            var o = scope.currentAction.sequences[index];
            scope.currentAction.sequences.removeAt(index);
            scope.currentAction.sequences.insertAt(nextIndex, o);
        }

        scope.addScript = function (name) {
            var found = false;
            for (var i = 0; i < scope.currentAction.scriptSequences.length; i++) {
                var s = scope.currentAction.scriptSequences[i];
                if (s === name)
                    found = true;
            }
            if (!found)
                scope.currentAction.scriptSequences.push(name);
        }

        scope.addSequence = function() {
            if (scope.currentAction === null)
                return;

            scope.currentAction.sequences.push({
                "keepdown": 0,
                "modifiers": [],
                "winmodifiers" : [],
                "entry": {}
            });
        }

        scope.hasWinModifier = function(index, keyName) {
            var sequence = scope.currentAction.sequences[index];
            return sequence.winmodifiers.some(function (e) { return e.key === keyName });
        }

        scope.hasModifier = function (index, keyName) {
            var sequence = scope.currentAction.sequences[index];
            return sequence.modifiers.some(function (e) { return e.key === keyName });
        }

        scope.isModifierAdvanced = function (modName) {
            return (
                   modName === "LMenu"
                || modName === "RMenu"
                || modName === "RControlKey"
                || modName === "LControlKey"
                || modName === "LShiftKey"
                || modName === "RShiftKey"
                );
        }

        scope.setModifierForSequence = function (index, keyName) {

            var value = scope.keyModifiers[keyName];
            var sequence = scope.currentAction.sequences[index];
            var i = sequence.modifiers.map(function (e) { return e.key; }).indexOf(keyName);
            sequence.winmodifiers = [];
            if (i < 0) {
                sequence.modifiers.push({
                    "key": keyName,
                    "value": value
                });
                return;
            }
            sequence.modifiers.splice(i, i + 1);

        }

        scope.setWinModifierForSequence = function (index, keyName) {
            var value = scope.winModifiers[keyName];
            var sequence = scope.currentAction.sequences[index];
            var i = sequence.winmodifiers.map(function (e) { return e.key; }).indexOf(keyName);
            sequence.modifiers = [];
            if (i < 0) {
                sequence.winmodifiers.push({
                    "key": keyName,
                    "value": value
                });
                return;
            }
            sequence.winmodifiers.splice(i, i + 1);
            

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


        init();
    }

})();