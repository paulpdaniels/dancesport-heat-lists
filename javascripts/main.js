/**
 * Created by Paul on 1/31/2015.
 */

;
(function (root) {
    'use strict';
    var module = angular.module('dancesportHeatLists', ['ui.bootstrap', 'ngRoute']);

    module.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/about', {templateUrl: 'partials/about.html', controller: 'AboutController'})
            .when('/search', {templateUrl: 'partials/search.html', controller: 'CompetitionController'})
            .otherwise('/search');
    }]);

    module.factory('CompetitionService', ['$http', function($http) {


        return {
            getCompetition : function(competitionKey) {
                return $http.get('dist/competitions/' + competitionKey  + '.json')
                    .then(function(resp) {
                        return angular.fromJson(resp.data);
                    })
            },
            getCompetitionList : function() {
                return $http.get('dist/competitions/manifest.json')
                    .then(function(resp) {
                        return angular.fromJson(resp.data);
                    });
            }
        }
    }]);

    module.controller('AboutController', ['$scope', function ($scope) {

    }]);

    module.controller('NavigationController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

        $scope.activeItem = '/search';

        $rootScope.$on('$routeChangeSuccess', function () {
            $scope.activeItem = $location.url();
        });

    }]);


    module.controller('CompetitionController', ['$scope', '$http', 'CompetitionService',
        function ($scope, $http, CompetitionService) {

        $scope.competitions = [];
        $scope.events = [];
        $scope.competitors = [];

        $scope.selectedCompetition = null;
        $scope.selectedEvent = null;
        $scope.selectedCompetitor = null;

        function reload() {
            if ($scope.selectedCompetition) {
                CompetitionService.getCompetition($scope.selectedCompetition.key)
                    .then(function(competition) {
                        if (competition.version < 4) {
                            return;
                        }

                        $scope.events = competition.events;
                        $scope.competitors = competition.dancers;
                        $scope.selectedEvent = $scope.events[0];

                        $scope.changeEvent();
                    });
            }
        }

        CompetitionService.getCompetitionList()
            .then(function (manifest) {
                $scope.competitions = manifest.competitions;
                $scope.selectedCompetition = $scope.competitions[0];
                reload();
            });

        $scope.changeCompetition = function () {
            reload();
        };

        $scope.changeEvent = function () {

            var competitors = _.filter($scope.competitors, function (competitor) {
                return _.includes(competitor.events, $scope.selectedEvent.$id);
            });

            $scope.selectedCompetitors = createPartnerships(competitors);

        };

        $scope.searchEvent = function (item, model, label) {
            $scope.selectedEvent = _.find($scope.events, {$id: item.$id});
            $scope.lookup = $scope.selectedEvent.name;
            $scope.changeEvent();
        };

        $scope.searchCompetitor = function (item, model, label) {
            $scope.selectedCompetitor = _.find($scope.competitors, {$id: item.$id});
            $scope.competitorLookup = $scope.selectedCompetitor.name;
        };

        var resolutionCache = {};

        $scope.getEvent = function (eventId) {
            var event = resolutionCache[eventId] || _.find($scope.events, {$id: eventId});

            if (event) {
                resolutionCache[eventId] = event;
                return event;
            }
        };

        function createPartnerships(competitors) {

            var partnerships = [];

            while (competitors.length > 0) {

                var head = competitors.pop();

                for (var i = 0, ii = competitors.length; i < ii; ++i) {

                    if (!_.includes(competitors[i].partners, head.$id) ||
                        !_.includes(head.partners, competitors[i].$id)) {
                        continue;
                    }

                    swapArray(competitors, i, competitors.length - 1);
                    partnerships.push([head, competitors.pop()]);

                    break;
                }

            }

            return partnerships;
        }

        function swapArray(arr, i, j) {
            var temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

    }]);

})(window);


