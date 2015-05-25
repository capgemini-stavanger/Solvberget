'use strict';

angular.module('solvbergetinfoScreenwebApp').controller('ImageCtrl', function ($scope, $rootScope) {

    console.log("ImageCtrl");

    $scope.imageSrc = $$config.apiPrefix + "/infoscreen/image/" + $scope.template.slideOptions.image;
    
    $rootScope.title = "Sølvberget";
  });

