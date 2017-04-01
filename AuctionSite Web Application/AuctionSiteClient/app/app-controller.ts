class AppController {
    public static $inject: string[] = ['userService', '$http'];
    constructor(public userService: UserService, private $http: ng.IHttpService) {
        this.userService.checkIfLogedIn();
    }
}

app.controller("AppController", AppController); 