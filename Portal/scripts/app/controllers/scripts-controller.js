(function () {
    'use strict';
    app.requires.push('ui.ace');
    app.controller('scriptsController', ["$scope", "apiService", ScriptsController]);

    function ScriptsController(scope, service) {
        scope.scripts = [];
        scope.currentScript = null;
        scope.currentSourceName = null;

        scope.codeModel = "code here";
        scope.editorOptions = {
            lineWrapping: true,
            lineNumbers: true,
            mode: 'javascript'
        };
        
        scope.$watch("currentScript.name", function (n, o) {
            if (scope.currentScript === null)
                return;
            scope.currentScript.fileName = n.replace(/\s+/g, '') + ".script";
        });

        scope.exitConfig = function() {
            window.location.href = "index.html";
        }

        scope.addInput = function() {
            scope.currentScript.inputs.push({ "key": "", "value": "" });
        }

        scope.removeInput = function(index) {
            scope.currentScript.inputs.splice(index, 1);
        }

        scope.addScript = function() {
            service.getNewScript().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to generate new script", result.errorMessage, 5000);
                    return;
                }
                var script = result.content;
                script.isStored = false;
                scope.scripts.push(script);
                scope.currentScript = script;
            });
        }

        scope.addSourceFile = function() {
            scope.currentScript.sourceFileNames.push("");
        }

        scope.saveScript = function() {
            service.saveScript(scope.currentScript).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to save script", scope.errorMessage, 5000);
                    return;
                }
                scope.displaySuccessMessage("Script saved", "Ok", 5000);
                scope.currentScript.isStored = true;
            });
        }

        scope.removeScript = function (index) {
            var script = scope.scripts[index];
            if (!script.isStored) {
                scope.scripts.removeAt(index);
                return;
            }
            service.removeScript(script).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to remove script from the server", result.errorMessage, 5000);
                    return;
                }
                scope.scripts.removeAt(index);
            });
        }

        scope.removeSource = function(index) {
            scope.currentScript.sourceFileNames.removeAt(index);
        }

        scope.moveSource = function (index, dir) {
            var nextIndex = index + dir;
            if (!scope.canMoveSource)
                return;

            var o = scope.currentScript.sourceFileNames[index];
            scope.currentScript.sourceFileNames.removeAt(index);
            scope.currentScript.sourceFileNames.insertAt(nextIndex, o);
        }

        scope.canMoveSource = function (index, dir) {
            var nextIndex = index + dir;
            if (nextIndex < 0)
                return false;
            if (nextIndex > scope.currentScript.sourceFileNames.length - 1)
                return false;
            return true;
        }

        scope.validateScript = function() {
            service.validateScript(scope.currentScript).then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Script did not validate", scope.errorMessage, 5000);
                    return;
                }
                scope.displaySuccessMessage("Script validated", "Ok", 5000);
            });
        }

        scope.setSelected = function(script) {
            scope.currentScript = script;
        }

        scope.getAllScripts = function() {
            service.getAllScriptObjects().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.setErrorMessage("Failed to get scripts", scope.errorMessage, 5000);
                    return;
                }
                scope.scripts = result.content;
                for (var i = 0; i < scope.scripts.length; i++) {
                    var script = scope.scripts[i];
                    script.isStored = true;
                }
            });
        }
        scope.aceLoaded = function(editor) {
            
        }

        scope.aceChanged = function(e) {
            
        }

        scope.saveSource = function() {
            service.saveScriptSource(scope.currentScript, scope.currentSourceName, scope.codeModel).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.setErrorMessage("Failed to get scripts", scope.errorMessage, 5000);
                    return;
                }
                scope.displaySuccessMessage("Script source saved", "Ok", 5000);
            });
        }

        scope.setSourceForEdit = function(index) {
            var sourceFileName = scope.currentScript.sourceFileNames[index];
            scope.currentSourceName = sourceFileName;
            service.loadScriptSource(scope.currentScript, sourceFileName).then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.setErrorMessage("Failed to get scripts", scope.errorMessage, 5000);
                    return;
                }

                scope.codeModel = result.content;
            });
        }
        
        scope.getAllScripts();
    }

})();