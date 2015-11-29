app.factory('apiService', ["$http", function ($http) {
    var rootUrl = "https://api.github.com";
    
	return {
        getReleases: function () {
            return $http.get(rootUrl + "/repos/samiy-xx/keysndr/releases");
        },
		getLatestRelease: function() {
			return $http.get(rootUrl + "/repos/samiy-xx/keysndr/releases/latest");
		},
        getFeed: function() {
            return $http.get("https://www.googleapis.com/plus/v1/people/+KeysndrWinplus/activities/public?key=AIzaSyAiXJtrrvRGp0O7khIYstDE44nkQhkBI-E");
        }
    }
}]);
