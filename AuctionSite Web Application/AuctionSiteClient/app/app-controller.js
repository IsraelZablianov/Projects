var AppController = (function () {
    function AppController(userService, $http) {
        this.userService = userService;
        this.$http = $http;
        this.userService.checkIfLogedIn();
    }
    AppController.$inject = ['userService', '$http'];
    return AppController;
}());
app.controller("AppController", AppController);
//# sourceMappingURL=app-controller.js.map