app.factory('apiService', ["$http","serverAddressService", function ($http, serverAddressService) {
    var rootUrl = serverAddressService.serverHost + "api/";
    return {
        getAllConfigurations: function () {
            return $http.get(rootUrl + "action/getallconfigurations");
        },

        getLegacyConfigurations: function() {
            return $http.get(rootUrl + "action/getlegacyconfigurations");
        },

        getViewConfigurations: function() {
            return $http.get(rootUrl + "action/getviewconfigurations");
        },

        getNewConfiguration: function(actionCount) {
            return $http.get(rootUrl + "action/getnewconfiguration?actionCount=" + actionCount);
        },

        getNewScript: function() {
            return $http.get(rootUrl + "scripts/getnewscript");
        },

        getNewInputAction: function() {
            return $http.get(rootUrl + "action/getnewinputaction");
        },

        getConfiguration: function (name) {
            return $http.get(rootUrl + "action/getconfiguration?name="+ name);
        },

        executeAction: function(container) {
            return $http.post(rootUrl + "action/execute", container);
        },

        removeConfiguration: function(name) {
            return $http.delete(rootUrl + "action/removeconfiguration?name=" + name);
        },

        saveConfiguration: function(conf) {
            return $http.post(rootUrl + "action/save", conf);
        },

        saveScript: function (script) {
            return $http.post(rootUrl + "scripts/save", script);
        },

        removeScript: function(script) {
            return $http.post(rootUrl + "scripts/remove", script);
        },

        validateScript: function(script) {
            return $http.post(rootUrl + "scripts/validate", script);
        },

        getSettings: function() {
            return $http.get(rootUrl + "settings/getappsettings");
        },

        saveSettings: function(settings) {
            return $http.post(rootUrl + "settings/storeappsettings", settings);
        },

        getAllScripts: function() {
            return $http.get(rootUrl + "scripts/getallscripts");
        },
        
        getAllScriptObjects: function() {
            return $http.get(rootUrl + "scripts/getallscriptobjects");
        },

        getProcessNames: function() {
            return $http.get(rootUrl + "system/getprocessnames");
        },

        getExportedConfig: function(configName) {
            return $http.get(rootUrl + "export/download?configName=" + configName, {
                 responseType: "arraybuffer"
            });
        },

        signalReload: function() {
            return $http.get(rootUrl + "system/reload");
        }
    }
}]);
