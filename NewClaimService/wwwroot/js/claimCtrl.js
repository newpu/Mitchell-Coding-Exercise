(function () {
    "use strict";

    angular.module("claimManagement")
           .controller("ClaimCtrl",  ClaimCtrl);

    function ClaimCtrl($http) {
        var vm = this;

        vm.claims = [];
        vm.updateClaim = {};
        vm.newClaim = {};
        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/claims")
             .then(function (response) {
                 angular.copy(response.data, vm.claims);

             }, function (error) {
                 vm.errorMessage = "Fail in getting claims";
             })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.showImage = false;

        vm.toggle = function () {
            vm.showImage = !vm.showImage;
        }

        vm.addClaim = function () {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/claims", vm.newClaim)
                 .then(function (response) {
                     vm.claims.push(response.data);
                     vm.newClaim = {};
                 }, function () {
                     vm.errorMessage = "Fail in add claim";
                 })
                 .finally(function () {
                     vm.isBusy = false;
                 });
        }

        vm.updateClaim = function () {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.put("/api/claims", vm.updateClaim)
                 .then(function (response) {
                     //success
                 }, function () {
                     vm.errorMessage = "Fail in update claim";
                 })
                 .finally(function () {
                     vm.isBusy = false;
                 });
        }


    }


}());