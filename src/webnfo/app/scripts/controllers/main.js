'use strict';

angular.module('solvbergetinfoScreenwebApp').factory('$exceptionHandler', function($injector, $log) {
  return function(exception, cause) {
    
	var screenstatus = $injector.get('screenstatus');
	
	var msg = exception.message += ' (caused by "' + cause + '")';	
    var screenId = ($routeParams.id) ? $routeParams.id : "default";
	
	console.log("Sending error info to server: " + msg);
	console.error(exception);
		
	screenstatus(screenId).save({error : msg});		
	
	return true;
  };
});


angular.module('solvbergetinfoScreenwebApp').controller('MainCtrl', function ($scope, $rootScope, $timeout, $routeParams, slides, screenstatus) {
    // Called after first slide retrieval. Starts a loop where slides are rotated every timeOut second.
    // Each slide defines its own timeout interval.
    $scope.onSlidesReceived = function () {

        console.log("onSlidesReceived", $scope.slides);
		
		window.onerror = function(message, url, linenumber) {
		
			screenstatus(screenId).save({error : message });
		  
			return true;
		}

        $scope.template=$scope.slides[0];
        $scope.count=0;

        $scope.nextSlide = function (timeOut) {
            

            console.log("next slide in " + timeOut + "ms.");

            $timeout(function () {

                console.log("NEXT SLIDE. $scope", $scope);

                $scope.template = $scope.slides[$scope.count];
                $scope.templateName = "views/slides/" + $scope.template.template + ".html?" + new Date().getTime();
                
				screenstatus(screenId).save({template : $scope.template.template });
				
				$scope.count+=1;
                if($scope.count>=$scope.slides.length) {
                    $scope.count=0;
                }
                
                $scope.nextSlide($scope.template.duration * 1000);
            }, timeOut);
        };

        $scope.nextSlide(0);
    };

    // Reloads slides every timeOut milliseconds
    $scope.reloadSlides = function (timeOut, screenId) {
        $timeout(function() {
            slides(screenId).query(
                function (data) {

                    console.log("update. ", data);

                    $scope.slides = data;
                    
                    $scope.reloadSlides(timeOut, screenId);
                }
            );
        }, timeOut);
    };

    // Mapper between slide names and

    var screenId = ($routeParams.id) ? $routeParams.id : "default";
    // Load slides and start slideshow

    console.log("screenId = " + screenId);
    
    $scope.slides = slides(screenId).query($scope.onSlidesReceived);

    // Start reload rotation of slides
    $scope.reloadSlides(2 * 60 * 1000, screenId);
    $rootScope.title = "Sølvberget";
});
