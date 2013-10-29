'use strict';

var mediaDetaljerCtrl = function ($scope, $rootScope, $routeParams, documents, documentRating) {

    $scope.document = documents.get({id : $routeParams.id}, function(){

        console.log("doc", $scope.document);

        $rootScope.breadcrumb.push($scope.document.Title);
        $scope.imageUrl = 'http://localhost:39465/v2/documents/' + $scope.document.Id + '/thumbnail';
    });

    $scope.rating = documentRating.get({id : $routeParams.id});
};

angular.module('Solvberget.WebApp')
    .controller('BookCtrl', mediaDetaljerCtrl)
    .controller('FilmCtrl', mediaDetaljerCtrl)
    .controller('LydbokCtrl', mediaDetaljerCtrl)
    .controller('CdCtrl', mediaDetaljerCtrl)
    .controller('NotehefteCtrl', mediaDetaljerCtrl);
