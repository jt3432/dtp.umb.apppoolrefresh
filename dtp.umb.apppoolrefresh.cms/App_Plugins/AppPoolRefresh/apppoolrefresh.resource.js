angular.module('umbraco.resources')
	.factory('appPoolRefreshResource', function ($http) {
	    return {
	        refresh: function () {
	            return $http.get('backoffice/apppoolrefresh/apppoolrefreshapi/refresh');
	        }
	    };
	});