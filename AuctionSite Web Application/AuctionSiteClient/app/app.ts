const app = angular.module("app", ['ui.bootstrap', 'vcRecaptcha', 'infinite-scroll']);

app.config(['$httpProvider', function ($httpProvider: ng.IHttpService) {
    $httpProvider.defaults.headers.common['Authorization'] = window.localStorage.getItem('Authorization');
}])