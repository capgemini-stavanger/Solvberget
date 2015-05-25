'use strict';

var $$config =  {
    apiPrefix: 'http://localhost:8080/',
    appUrlPrefix : 'http://solvbergetapp.cloudapp.net/infoscreen/',
    apiPrefixEscaped : function(){
        return this.apiPrefix.replace(/:(\d+)/,'\\:$1'); // workaround to escape port number : so it doesn't get interpreted as a variable by $resource
      }
  };

angular.module('solvbergetinfoScreenwebApp', [
  'ngCookies',
  'ngResource',
  'ngSanitize',
  'ngRoute'
])
  .config(function ($routeProvider) {
    $routeProvider
      .when('/', {
        templateUrl: 'views/main.html',
        controller: 'MainCtrl'
      })
      .when('/:id', {
        templateUrl: 'views/main.html',
        controller: 'MainCtrl'
      })
      .otherwise({
        redirectTo: '/'
      });
  }).run(function ($locale) {
    console.log("Current locale: " + $locale.id);
  });
    
