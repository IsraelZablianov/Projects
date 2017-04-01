var UserLoginController = (function () {
    function UserLoginController(userService) {
        this.userService = userService;
        this.userLogin = new userLogin();
        this.userRegistration = new UserRegistrationDTO();
        this.invalidUser = false;
        this.userExsits = false;
        this.invalidRegistration = false;
        this.invalidCaptcha = false;
        this.invalidPassword = false;
    }
    UserLoginController.prototype.login = function () {
        var _this = this;
        this.invalidUser = true;
        if (this.userLogin.userName !== undefined && this.userLogin.password !== undefined) {
            this.userService.login(this.userLogin).then(function (response) {
                _this.userService.isLogedIn = response;
                if (_this.userService.isLogedIn) {
                    _this.invalidUser = false;
                }
            });
        }
        else {
            this.invalidUser = true;
        }
    };
    UserLoginController.prototype.signIn = function () {
        var _this = this;
        if (this.userRegistration.name !== undefined &&
            !this.invalidPassword &&
            this.userRegistration.email !== undefined &&
            this.userRegistration.id !== undefined) {
            this.userService.addUser(this.userRegistration, this.captcha)
                .then(function (response) {
                switch (response) {
                    case AddUserStatuse.ALREADY_EXIST:
                        {
                            _this.userExsits = true;
                            break;
                        }
                    case AddUserStatuse.CAPTCHA_ERR:
                        {
                            _this.invalidCaptcha = true;
                            break;
                        }
                    case AddUserStatuse.SUCCESS:
                        {
                            _this.userService.isLogedIn = true;
                            break;
                        }
                }
            });
        }
        else {
            this.invalidRegistration = true;
        }
    };
    UserLoginController.prototype.checkPasswordValid = function () {
        if (this.userRegistration.password !== undefined && this.userRegistration.password.length == 8
            && /[A-Z]/.test(this.userRegistration.password[0])
            && /^[A-Za-z0-9]*/.test(this.userRegistration.password.substring(1, this.userRegistration.password.length - 1))) {
            this.invalidPassword = false;
        }
        else {
            this.invalidPassword = true;
        }
    };
    UserLoginController.$inject = ['userService'];
    return UserLoginController;
}());
//# sourceMappingURL=user-login-controller.js.map