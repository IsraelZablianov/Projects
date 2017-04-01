var app = angular.module("app", ['ui.bootstrap', 'vcRecaptcha', 'infinite-scroll']);
app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common['Authorization'] = window.localStorage.getItem('Authorization');
    }]);
//# sourceMappingURL=app.js.map