import ApiConfig from '../config/ApiConfig'

export default class ApiService {

    baseUrl = "";
    loginAuthority = "";
    authorisationToken = "";

    constructor() {
        this.baseUrl = ApiConfig.BaseUrl;
        this.loginAuthority = ApiConfig.LoginAuthority;
    }

    mapToErrorMessage(statusCode, json) {
        if (statusCode === 401) {
            return "Invalid login";
        } else if (statusCode === 500) {
            return json.error;
        }

        return "";
    }

    async getAuthorisationToken() {
        try {
            var jwtToken = document.cookie.split(";").map(r => r.split("=")).filter(r => r[0].trim() === "token");

            if (jwtToken.length === 0) {
                return;
            }

            var jwtTokenValue = jwtToken[0][1].trim();

            this.authorisationToken = jwtTokenValue;
        } catch (_) {
            return;
        }

    }

    async isLoggedIn() {
        try {
            var response = await fetch(ApiConfig.LoginAuthority + "Security/SSOUser/Me()", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json; charset=utf-8;",
                    "Authorization": "bearer " + this.authorisationToken
                }
            });

            var json = await response.json();

            return !(json.Id === "Guest");
        } catch (_) {
            return false;
        }
    }

    async get(url) {
        var response = await fetch(this.baseUrl + url, {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8;",
                "Authorization": "bearer " + this.authorisationToken
            }
        });

        var json = await response.json();

        return json;
    }

    async post(url, body) {
        var response = await fetch(this.baseUrl + url, {
            method: "POST",
            headers: {
                "Content-Type": "text/plain",
                "Authorization": "bearer " + this.authorisationToken
            },
            body: body
        });

        var json = await response.text();

        return json;
    }

    async login(username, password) {
        try {
            var response = await fetch(this.loginAuthority + "Account/Login", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json; charset=utf-8;"
                    },
                    body: JSON.stringify({ User: username, "Pass": password })
                });

            var json = "";

            try {
                json = await response.json();
            }
            catch (_) {
                return { status: response.status, error: "Unhandled error occured" };
            }

            if (response.status === 200) {
                document.cookie = "token=" + json.id + "; expires=" + json.expires + "; path=/;";
            }

            return { status: response.status, error: this.mapToErrorMessage(response.status, json) };
        } catch (_) {
            return { status: -1, error: "Unhandled error occured" };
        }
    }
}