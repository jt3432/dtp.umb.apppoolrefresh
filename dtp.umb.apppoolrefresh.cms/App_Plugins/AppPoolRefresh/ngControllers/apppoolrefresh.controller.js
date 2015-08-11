angular.module('umbraco').controller('AppPoolRefresh.Controller',
	function ($scope, $route, $log, $timeout, appPoolRefreshResource) {

	    $scope.refresh = function () {
	        $scope.isProcessing = true;
	        appPoolRefreshResource.refresh().then(function (response) {
	            $timeout(function () {
	                $scope.isProcessing = false;
	                //window.location.reload(true);
	            }, 1000);
		    });
		};
	});
