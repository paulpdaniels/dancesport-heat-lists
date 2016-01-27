/**
 * Created by Paul on 1/31/2015.
 */

;
(function (root) {
    'use strict';
    var module = angular.module('dancesportHeatLists', ['ui.bootstrap', 'ngRoute']);

    module.config(function ($routeProvider) {
        $routeProvider
            .when('/about', {templateUrl: 'partials/about.html', controller: 'AboutController'})
            .when('/search', {templateUrl: 'partials/search.html', controller: 'CompetitionController'})
            .otherwise('/search');
    });

    module.controller('AboutController', function ($scope) {

    });

    module.controller('NavigationController', function ($scope, $rootScope, $location) {

        $scope.activeItem = '/search';

        $rootScope.$on('$routeChangeSuccess', function () {
            $scope.activeItem = $location.url();
        });

    });


    module.controller('CompetitionController', function ($scope, $http, $route, $routeParams) {

        $scope.competitions = [];
        $scope.events = [];
        $scope.competitors = [];

        $scope.selectedCompetition = null;
        $scope.selectedEvent = null;
        $scope.selectedCompetitor = null;

        function reload() {
            if ($scope.selectedCompetition) {
                $http.get('competitions/' + $scope.selectedCompetition.key + '.json')
                    .success(function (data, status, headers, config) {
                        if (status === 200) {
                            var jsonData = angular.fromJson(data);

                            //Can't read the old data now...whoops
                            if (jsonData.version < 2)
                                return;

                            $scope.events = jsonData.events;
                            $scope.competitors = jsonData.dancers;
                            $scope.selectedEvent = $scope.events[0];

                            $scope.changeEvent();
                        }
                    });
            }
        }


        $http.get('competitions/manifest.json')
            .then(function (resp) {
                var manifest = angular.fromJson(resp.data);
                $scope.competitions = manifest.competitions;
                $scope.selectedCompetition = $scope.competitions[0];
                reload();
            });


        $scope.eventLookup = "";
        $scope.competitorLookup = "";

        $scope.changeCompetition = function () {
            reload();
        };

        $scope.changeEvent = function () {

            var competitors = _.filter($scope.competitors, function (competitor) {
                return _.includes(competitor.events, $scope.selectedEvent.$id);
            });


            var partners = createPartnerships(competitors);

            $scope.selectedCompetitors = partners;

        };

        $scope.searchEvent = function (item, model, label) {

            for (var i = 0, len = $scope.events.length; i < len; ++i) {
                if (model == $scope.events[i]) {
                    $scope.selectedEvent = $scope.events[i];
                    break;
                }
            }

            $scope.changeEvent();
        };

        $scope.searchCompetitor = function (item, model, label) {
            $scope.selectedCompetitor = _.find($scope.competitors, {$id: item.$id});
        };

        $scope.getEventName = function (eventId) {
            var event = _.find($scope.events, {$id: eventId});

            if (event)
                return event.name;
        };

        function createPartnerships(competitors) {

            var partnerships = [];

            while (competitors.length > 0) {

                var head = competitors.pop();

                for (var i = 0, ii = competitors.length; i < ii; ++i) {

                    if (!_.includes(competitors[i].partners, head.name) ||
                        !_.includes(head.partners, competitors[i].name)) {
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

    });

})(window);


