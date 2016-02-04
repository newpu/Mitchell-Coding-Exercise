(function () {
    "use strict";

    angular.module("claimManagement")
           .controller("claimEditCtrl", claimEditCtrl);

    function claimEditCtrl($http, $routeParams) {
        var vm = this;

        vm.claimNumber = $routeParams.claimNumber;

        vm.vehicles = [];
        vm.updatedClaim = {};
        vm.newVehicle = {};
        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/claims/detail/" + vm.claimNumber)
            .then(function (response) {
                vm.updatedClaim = response.data;
            }, function (error) {
                vm.errorMessage = "Fail in getting claim by claim number";
            })
           .finally(function () {
               vm.isBusy = false;
           });
     
        vm.getVehicle = function () {
           vm.isBusy = true;
           vm.errorMessage = "";

           $http.get("/api/claims/" + vm.claimNumber + "/vehicles")
             .then(function (response) {
                 angular.copy(response.data, vm.vehicles);

             }, function (error) {
                 vm.errorMessage = "Fail in getting vehicles";
             })
            .finally(function () {
                vm.isBusy = false;
            });
       }

        vm.showImage = false;

        vm.toggle = function () {
            vm.showImage = !vm.showImage;
        }

        vm.addVehicle = function () {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/claims" + vm.claimNumber + "/vehicles", vm.newVehicle)
                 .then(function (response) {
                     vm.claims.Vehicles.push(response.data);
                     vm.newVehicle = {};
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

            $http.put("/api/claims", vm.updatedClaim)
                 .then(function (response) {
                     //success
                     angular.copy(response.data, vm.updatedClaim);
                     vm.errorMessage = "claim updated";
                 }, function () {
                     vm.errorMessage = "Fail in update claim";
                 })
                 .finally(function () {
                     vm.isBusy = false;
                 });

            
            //$http.patch("/api/claims/" + vm.claimNumber, vm.updatedClaim)
            //     .then(function (response) {
            //         //success
            //         angular.copy(response.data, vm.updatedClaim);
            //         vm.errorMessage = "claim updated";
            //     }, function () {
            //         vm.errorMessage = "Fail in update claim";
            //     })
            //     .finally(function () {
            //         vm.isBusy = false;
            //     });
        }


    }


}());