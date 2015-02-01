/**
 * Created by Paul on 1/31/2015.
 */

;(function (root) {
    'use strict';
    var module = angular.module('dancesportHeatLists', ['ui.bootstrap']);

    module.controller('CompetitionController', function($scope, $http, $q){

        $scope.competitions = [];
        $scope.selectedCompetition = null;
        $scope.events = [];
        $scope.selectedEvent = null;

        $http.get('competitions/manifest.json')
            .success(function(data, status, headers, config){
                if (status === 200) {
                    var manifest = angular.fromJson(data);
                    $scope.competitions = manifest.competitions;
                    $scope.selectedCompetition = $scope.competitions[0];
                }

                if ($scope.selectedCompetition) {
                    $http.get('competitions/' + $scope.selectedCompetition.key + '.json')
                        .success(function(data, status, headers, config){
                            if (status === 200) {
                                $scope.events = angular.fromJson(data).events;
                                $scope.selectedEvent = $scope.events[0];
                            }
                        });
                }
            });

        $scope.lookup = "";
        $scope.getMatchingEvents = function(val){
            return $q(function(resolve, reject) {

                var results = $scope.events.filter(function(event) {
                    return event.name.indexOf(val) != -1;
                });

                resolve(results);
            });

        };

        $scope.select = function(item, model, label) {

            var i = 0;
            for (var len = $scope.events.length; i < len; ++i) {
                if (model == $scope.events[i]) {
                    $scope.selectedEvent = $scope.events[i];
                    break;
                }
            }
        };

    });

})(window);


