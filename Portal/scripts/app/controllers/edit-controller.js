﻿(function () {
    'use strict';
    app.controller('editController', ["$scope", "apiService", "$location", EditController]);

    function EditController(scope, service, $location) {
        scope.currentAction = null;
        scope.inputConfiguration = null;
        scope.currentAction = null;
        scope.keys = AppKeys;
        scope.keyModifiers = AppModifiers;
        scope.winModifiers = WinModifiers;
        scope.availableScripts = [];
        scope.processSelector = {
            'useDesktopWindow': false,
            'useForegroundWindow': false,
            'useProcessName': false
        }
        scope.processNames = ["loading..."];

        scope.$watch("inputConfiguration.name", function (n, o) {
            if (scope.inputConfiguration === null)
                return;
            scope.inputConfiguration.fileName = n.replace(/\s+/g, '') + ".json";
        });

        scope.exitConfig = function() {
            window.location.href = "index.html";
        }
        scope.exportConfiguration = function() {
            var confFileName = scope.inputConfiguration.fileName;
            saveAs(
                new Blob([angular.toJson(scope.inputConfiguration, true)], { type: 'text/json' }),
                confFileName
            );
        }
        scope.saveConfiguration = function () {
            service.saveConfiguration(scope.inputConfiguration).then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to save configuration", result.errorMessage, 5000);
                    return;
                }
                scope.displaySuccessMessage("Configuration saved", "OK", 5000);
            });
        }

        scope.setForEdit = function (a) {
            scope.currentAction = a;
        }

        scope.getProcessNames = function () {
            service.getProcessNames().then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.setErrorMessage("Failed to load processes", scope.errorMessage, 5000);
                    return;
                }

                scope.processNames = result.content;
            });
        }

        scope.setProcessName = function (p) {
            if (!scope.processSelector.useProcessName)
                return;
            scope.inputConfiguration.processName = p;
        }

        scope.setProcessTarget = function (d, f, p) {
            scope.processSelector.useDesktopWindow = d;
            scope.processSelector.useForegroundWindow = f;
            scope.processSelector.useProcessName = p;
            if (!p)
                scope.inputConfiguration.processName = "";
            scope.inputConfiguration.useDesktopWindow = d;
            scope.inputConfiguration.useForegroundWindow = f;
        }

        scope.hasWinModifier = function (index, keyName) {
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
        scope.addSequence = function () {
            if (scope.currentAction === null)
                return;

            scope.currentAction.sequences.push({
                "keepdown": 0,
                "modifiers": [],
                "winmodifiers": [],
                "entry": {}
            });
        }
        scope.deleteScript = function (index) {
            scope.currentAction.scriptSequences.splice(index, 1);
        }

        scope.deleteSequence = function (index) {
            scope.currentAction.sequences.removeAt(index);
        }

        scope.moveSequence = function (index, dir) {
            var nextIndex = index + dir;
            if (!scope.canMoveSequence)
                return;

            var o = scope.currentAction.sequences[index];
            scope.currentAction.sequences.removeAt(index);
            scope.currentAction.sequences.insertAt(nextIndex, o);
        }

        scope.canMoveSequence = function (index, dir) {
            var nextIndex = index + dir;
            if (nextIndex < 0)
                return false;
            if (nextIndex > scope.currentAction.sequences.length - 1)
                return false;
            return true;
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

        scope.init = function (count) {
            var s = $location.search();
            if (s !== null && s !== undefined && s.hasOwnProperty("name")) {
                service.getConfiguration(s.name).then(function (response) {
                    var result = response.data;
                    if (!result.success) {
                        scope.displayErrorMessage("Failed to get actions. Is the server running?", result.errorMessage, 5000);
                        return;
                    }
                    scope.inputConfiguration = result.content;
                    scope.setProcessTarget(
                        scope.inputConfiguration.useDesktopWindow,
                        scope.inputConfiguration.useForegroundWindow,
                        scope.inputConfiguration.processName.length > 0
                    );
                });
            } else {
                service.getNewConfiguration(count).then(function (response) {
                    var result = response.data;
                    if (!result.success) {
                        scope.displayErrorMessage("Failed to get actions. Is the server running?", result.errorMessage, 5000);
                        return;
                    }
                    scope.inputConfiguration = result.content;
                    scope.setProcessTarget(
                        scope.inputConfiguration.useDesktopWindow,
                        scope.inputConfiguration.useForegroundWindow,
                        scope.inputConfiguration.processName.length > 0
                    );
                });
            }

            service.getAllScripts().then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to get scripts", result.errorMessage, 5000);
                    return;
                }
                scope.availableScripts = result.content;
            });
        } 
    }

})();