(function () {
    "use strict";
    var app = angular.module("claimClient", ["ngRoute"])
                     .config(function ($routeProvider) {

                         $routeProvider.when("/", {
                             controller: "ClaimCtrl",
                             controllerAS: "vm",
                             templateUrl: "/views/claims.html"
                         });

                         $routeProvider.when("/edit/:claimNumber", {
                             controller: "claimEditCtrl",
                             controllerAS: "vm",
                             templateUrl: "/views/claimEdit.html"
                         });

                         $routeProvider.otherwise({ redirecTo: "/"})
                     });

    app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.patch = {
            'Content-Type': 'application/json-patch + json;charset=utf-8'
        }
    }])

    

    
}());