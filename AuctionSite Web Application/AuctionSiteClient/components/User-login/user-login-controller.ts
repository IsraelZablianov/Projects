class UserLoginController {

    public static $inject: string[] = ['userService'];
    public userLogin: userLogin = new userLogin();
    public userRegistration: UserRegistrationDTO = new UserRegistrationDTO();
    public captcha;
    public invalidUser: boolean = false;
    public userExsits: boolean = false;
    public invalidRegistration: boolean = false;
    public invalidCaptcha: boolean = false;
    public invalidPassword: boolean = false;


    public constructor(public userService: UserService) {}

    public login() {
        this.invalidUser = true;
        if (this.userLogin.userName !== undefined && this.userLogin.password !== undefined) {
            this.userService.login(this.userLogin).then((response) => {
                this.userService.isLogedIn = response;
                if (this.userService.isLogedIn) {
                    this.invalidUser = false;
                }
            });
        }
        else {
            this.invalidUser = true;
        }
    }

    public signIn() {
        if (this.userRegistration.name !== undefined &&
            !this.invalidPassword &&
            this.userRegistration.email !== undefined &&
            this.userRegistration.id !== undefined) {
            this.userService.addUser(this.userRegistration, this.captcha)
                .then((response) => {
                    switch (response) {
                    case AddUserStatuse.ALREADY_EXIST:
                    {
                        this.userExsits = true;
                        break;
                    }
                    case AddUserStatuse.CAPTCHA_ERR:
                    {
                        this.invalidCaptcha = true;
                        break;
                    }
                    case AddUserStatuse.SUCCESS:
                    {
                        this.userService.isLogedIn = true;
                        break;
                    }

                    }
                });
        } else {
            this.invalidRegistration = true;
        }
    }

    public checkPasswordValid() {
        if (this.userRegistration.password !== undefined && this.userRegistration.password.length == 8
            && /[A-Z]/.test(this.userRegistration.password[0])
            && /^[A-Za-z0-9]*/.test( this.userRegistration.password.substring(1, this.userRegistration.password.length-1)))
        {
            this.invalidPassword = false;
        }
        else {
            this.invalidPassword = true;
        }
    }
}
