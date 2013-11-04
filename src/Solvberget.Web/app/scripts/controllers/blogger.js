'use strict';

angular.module('Solvberget.WebApp')
  .controller('BloggerCtrl', function ($scope, $rootScope, blogs) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Blogger');

        $scope.items = blogs.query();

        $scope.pathFor = function(item){
            return $rootScope.path('BlogCtrl', {id: item.id});
        };
  })
    .controller('BlogCtrl', function ($scope, $routeParams, $rootScope, blogs) {

        $rootScope.breadcrumb.clear();
        $rootScope.breadcrumb.push('Blogger');

        $scope.blog = blogs.get({id : $routeParams.id}, function(){

            $rootScope.breadcrumb.push($scope.blog.title)

        });
    });

