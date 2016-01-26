(function () {
    'use strict';
    app.controller('frontPageController', ["$scope", "apiService", FrontPageController]);

    function FrontPageController(scope, service) {
        scope.availableViewConfigurations = null;
        scope.availableLegacyConfigurations = null;
        scope.newGridRows = 1;
        scope.newGridColumns = 1;

        scope.getLegacyConfigurations = function() {
            service.getLegacyConfigurations().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to load legacy configurations", result.errorMessage, 5000);
                    return;
                }
                scope.availableLegacyConfigurations = result.content;
            });
        }

        scope.getViewConfigurations = function() {
            service.getViewConfigurations().then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to load view configurations", result.errorMessage, 5000);
                    return;
                }
                scope.availableViewConfigurations = result.content;
            });
        }

        scope.removeGrid = function (index) {
            service.removeConfiguration(scope.availableLegacyConfigurations[index]).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to remove grid", result.errorMessage, 5000);
                    return;
                }
                scope.availableLegacyConfigurations.splice(index, 1);
            });
        }

        scope.createGrid = function() {
            window.location.href = "edit-grid.html?rows="+ scope.newGridRows + "&columns=" + scope.newGridColumns;
        }

        scope.editGrid = function (index) {
            var name = scope.availableLegacyConfigurations[index];
            window.location.href = "edit-grid.html?name=" + name;
        }

        scope.openGrid = function (index) {
            var name = scope.availableLegacyConfigurations[index];
            window.location.href = "play-grid.html?name=" + name;
        }

        scope.exportGrid = function (index) {
            service.getExportedConfig(scope.availableLegacyConfigurations[index]).then(function(response) {
                var result = response.data;
                saveAs(
                    new Blob([result], { type: 'application/octet-stream' }),
                    "download.zip"
                );
            });
        }

        scope.dataFromFile = function(file) {
            service.upload(file).then(function(response) {
                scope.getLegacyConfigurations();
                scope.getViewConfigurations();
            });
        }

        scope.exportView = function (index) {
            service.getExportedConfig(scope.availableViewConfigurations[index]).then(function (response) {
                var result = response.data;
                saveAs(
                    new Blob([result], { type: 'application/octet-stream' }),
                    "download.zip"
                );
            });
        }

        scope.createView = function() {
            window.location.href = "edit-view.html";
        }

        scope.removeView = function (index) {
            service.removeConfiguration(scope.availableViewConfigurations[index]).then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to remove view", result.errorMessage, 5000);
                    return;
                }
                scope.availableViewConfigurations.splice(index, 1);
            });
        }

        scope.editView = function (index) {
            var name = scope.availableViewConfigurations[index];
            window.location.href = "edit-view.html?name=" + name;
        }

        scope.openView = function (index) {
            var name = scope.availableViewConfigurations[index];
            service.getConfiguration(name).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    return;
                }
                var url = result.content.view;
                window.location.href = "/Views/" + url + "/index.html";
            });
        }

        scope.init = function() {
            service.getSettings().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to get app settings", result.errorMessage, 5000);
                    return;
                }

                var appSettings = result.content;
                if (appSettings.firstTimeRunning) {
                    window.location.href = "setup.html";
                    return;
                }
                scope.getLegacyConfigurations();
                scope.getViewConfigurations();
            });
        }

        scope.init();
    }

})();