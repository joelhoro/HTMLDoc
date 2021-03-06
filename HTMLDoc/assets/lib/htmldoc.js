angular.module('htmlDoc', [])
	   .controller('htmlDocJsonLoader', function ($scope, $http, $attrs, $location) {
	        var source = $attrs.source || $location.url();
    		$http.get(source, { cache: true }).success(function (data) {
				$scope.data = data;
			})
	   })
	  .directive('htmlDocTable', function(){
		// usage:
		//
		//		<div html-doc-table data='[{a:1,b:2}, ...]'></div>

        return {
          scope: {
            data	: '=',
          },
          //transclude: true,
          restrict: 'A',	
          controller: function ($scope, $filter, $attrs) {
				var cachedKeys = null;
				$scope.keys = function (d) {
					if (d == undefined) return [];
					if (cachedKeys != null) return cachedKeys;
					var keys = Object.keys(d[0]);
					cachedKeys = keys;
					console.log(keys);
					return keys;
				}

		  		$scope.sortColumnFn = function(column) { 
		  			console.log("Current sort: ", $scope.sortColumn);
		  			if($scope.sortColumn == column)
		  				$scope.reverse = !$scope.reverse;
		  			else
		  				$scope.reverse = false;
		  			$scope.sortColumn = column;
		          	$scope.data = $filter('orderBy')($scope.data,column,$scope.reverse);
		          	console.log("Sorting by ", $scope.sortColumn, "scope=", $scope);
		  		}
		  },
          templateUrl: 'templates\\htmltableTemplate.html' 
          	} 
      	} ); 
