angular.module('htmlDoc', [])
	   .controller('htmlDocJsonLoader', function($scope,$http,$attrs) {
			var source = $attrs.source;
			$http.get(source, { cache: true }).success(function(data) {
				$scope.data = data;
			})			
	   } )
	  .directive('htmlDocTable', function(){
	      debugger;
		// usage:
		//
		//		<div html-doc-table headers='["XXX","YYY"]' data='[[1,2],[3,4]]'></div>

        return {
          scope: {
            data	: '=',
            headers	: '='
          },
          restrict: 'A',	
          template: '<table class="table bootstrap-table table-striped table-hover">' +
			'<thead><tr><th ng-repeat="header in headers">{{header}}</th></tr></thead>' +
			'<tbody>' +
			'	<tr ng-repeat="row in data">' +
			'		<td ng-repeat="cell in row">{{cell}}</td>' +
			'	</tr>' +
			'</tbody>' +
			'</table>',
          	} 
      	} ); 
