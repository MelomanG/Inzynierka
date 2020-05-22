export interface RegisterModel {
    Username: string,
	Email: string,
	Password: string,
	ConfirmationPassword: string
}

export interface LoginModel {
    Email: string,
	Password: string,
}

export interface RefreshTokenModel {
    refreshToken : string;
}

export interface LoginResponseModel {
	accessToken: AccessToken,
	refreshToken: string
}

export interface AccessToken {
	token: string,
	expiresIn: number
}