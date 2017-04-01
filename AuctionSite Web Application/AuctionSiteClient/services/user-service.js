var UserService = (function () {
    function UserService($http, $window) {
        this.$http = $http;
        this.$window = $window;
        this.isLogedIn = false;
    }
    UserService.prototype.addUser = function (user, captcha) {
        var _this = this;
        return this.verifyCaptcha(captcha)
            .then(function (response) {
            if (response) {
                _this.updateAutentication({ userName: user.id, password: user.password });
                return _this.$http.post("api/users", user)
                    .then(function (data) {
                    if (data.data) {
                        return AddUserStatuse.SUCCESS;
                    }
                    else {
                        return AddUserStatuse.ALREADY_EXIST;
                    }
                });
            }
            else {
                return AddUserStatuse.CAPTCHA_ERR;
            }
        });
    };
    UserService.prototype.verifyCaptcha = function (captcha) {
        return this.$http.post("api/captcha", { captcha: captcha }).then(function (data) {
            return data.data;
        });
    };
    UserService.prototype.checkIfLogedIn = function () {
        var _this = this;
        return this.checkAuthorization().then(function (response) { return _this.isLogedIn = response; });
    };
    UserService.prototype.login = function (userlogin) {
        this.updateAutentication(userlogin);
        return this.checkAuthorization();
    };
    UserService.prototype.checkAuthorization = function () {
        return this.$http.get("api/users").
            then(function (data) { return data.status === 200; });
    };
    UserService.prototype.updateAutentication = function (userlogin) {
        this.updateLocalStorage(userlogin);
        this.$http.defaults.headers.common['Authorization'] = 'Basic ' + window.btoa(userlogin.userName + ':' + userlogin.password);
    };
    UserService.prototype.updateLocalStorage = function (userlogin) {
        this.$window.localStorage.setItem('Authorization', 'Basic ' + window.btoa(userlogin.userName + ':' + userlogin.password));
        this.$window.localStorage.setItem('UserName', userlogin.userName);
    };
    UserService.$inject = ['$http', '$window'];
    return UserService;
}());
var AddUserStatuse;
(function (AddUserStatuse) {
    AddUserStatuse[AddUserStatuse["SUCCESS"] = 0] = "SUCCESS";
    AddUserStatuse[AddUserStatuse["CAPTCHA_ERR"] = 1] = "CAPTCHA_ERR";
    AddUserStatuse[AddUserStatuse["ALREADY_EXIST"] = 2] = "ALREADY_EXIST";
})(AddUserStatuse || (AddUserStatuse = {}));
app.service('userService', UserService);
//# sourceMappingURL=user-service.js.map