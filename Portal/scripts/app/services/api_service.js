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

        getConfiguration: function (name) {
            return $http.get(rootUrl + "action/getconfiguration?name="+ name);
        },

        executeAction: function(action) {
            return $http.post(rootUrl + "action/execute", action);
        },

        removeConfiguration: function(name) {
            return $http.delete(rootUrl + "action/removeconfiguration?name=" + name);
        },

        getSettings: function() {
            return $http.get(rootUrl + "settings/getappsettings");
        },

        saveSettings: function(settings) {
            return $http.post(rootUrl + "settings/storeappsettings", settings);
        }
    }
}]);
