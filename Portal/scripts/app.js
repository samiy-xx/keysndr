var app = angular.module("keysndr", []);

//For testing
var node_or_local_filesystem_url_override = true;
var override_url = "http://192.168.0.112:45889/";

app.config(['$locationProvider',
    function ($locationProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }
]);

app.factory('serverAddressService', ["$location", function ($location) {
    var url = "";
    var protocol = $location.protocol();
    if (protocol !== "http" && protocol !== "https") {
        url += override_url;
    } else {
        var host = $location.host();
        var port = $location.port();

        url += protocol + "://" + host + ":" + port + "/";
    }
    if (node_or_local_filesystem_url_override)
        url = override_url;
    return {
        serverHost: url
    };
}]);


function listToMatrix(list, elementsPerSubArray) {
    var matrix = [], i, k;
    for (i = 0, k = -1; i < list.length; i++) {
        if (i % elementsPerSubArray === 0) {
            k++;
            matrix[k] = [];
        }
        matrix[k].push(list[i]);
    }
    return matrix;
}

// Prototypes
Array.prototype.insertAt = function (index, item) {
    this.splice(index, 0, item);
};

Array.prototype.removeAt = function (index) {
    this.splice(index, 1);
};

/*Object.prototype.getKeyByValue = function (value) {
    for (var prop in this) {
        if (this.hasOwnProperty(prop)) {
            if (this[prop] === value)
                return prop;
        }
    }
}*/
