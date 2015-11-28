app.factory('apiService', ["$http", function ($http) {
    var rootUrl = "https://api.github.com";
    
	return {
        getReleases: function () {
            return $http.get(rootUrl + "/repos/samiy-xx/keysndr/releases");
        },
		getLatestRelease: function() {
			return $http.get(rootUrl + "/repos/samiy-xx/keysndr/releases/latest");
		}
    }
}]);
