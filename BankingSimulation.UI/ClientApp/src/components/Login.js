import React, { Component } from 'react';
import ApiConfig from '../config/ApiConfig'

export default class Login extends Component {
  static displayName = Login.name;

  constructor(props) {
    super(props);
    this.state = {
        username: "",
        password: "",
        error: "",
        loading: false
    };
  }

  componentDidMount() {
    var jwtToken = document.cookie.split(";").map(r => r.split("=")).filter(r => r[0].trim() === "token");

    if (jwtToken.length === 0) {
        return;
    }

    var jwtTokenValue = jwtToken[0][1].trim();

    fetch(ApiConfig.LoginAuthority + "Security/SSOUser/Me()", {
        method: "GET",
        headers: {
            "Content-Type": "application/json; charset=utf-8;",
            "Authorization": "bearer " + jwtTokenValue
        }
    }).then(async (response) => {
        var json = await response.json();

        if (json.Id !== "Guest") {
            this.setState({ error: "", loading: false });
            this.props.onLoggedIn();
        }
    })
  }

  login() {
    this.setState({ loading: true });

    fetch(ApiConfig.LoginAuthority + "Account/Login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json; charset=utf-8;"
        },
        body: JSON.stringify({ User: this.state.username, "Pass": this.state.password })
    }).then(async (response) => {
        this.setState({ error: "", loading: false });
        var json = await response.json();

        if (response.status === 500) {
            this.setState({ error: json.error, loading: false });
        } else if (response.status === 401) {
            this.setState( {error: "Invalid login", loading: false })
        } else if (response.status === 200) {
            this.setState({ error: "", loading: false });
            document.cookie = "token=" + json.id + "; expires=" + json.expires + "; path=/;";
            this.props.onLoggedIn();
        } else {
            this.setState({ error: "Unhandled error occured", loading: false });
        }
    }).catch((_) => {
        this.setState({ error: "Unhandled error occured", loading: false });
    });
  }

  renderError() {
    if (this.state.error !== "") {
        return (<div className="alert alert-warning mb-md-5 mt-md-4">{this.state.error}</div>);
    }
  }

  renderLoginButton() {
    if (!this.state.loading) {
        return (
            <button  data-mdb-button-init
            data-mdb-ripple-init
            className="btn btn-outline-light btn-lg px-5"
            type="submit"
            onClick={(_) => this.login()}>Login</button>
        );
    } else {
        return (
            <button  data-mdb-button-init
            data-mdb-ripple-init
            className="btn btn-outline-light btn-lg px-5"
            type="submit"
            disabled="true"
            >Loading</button>
        );
    }
  }

  render() {
    return (
        <section className="vh-100 gradient-custom">
            <div className="container py-5 h-100">
                <div className="row d-flex justify-content-center align-items-center h-100">
                    <div className="col-12 col-md-8 col-lg-6 col-xl-5">
                        <div className="card bg-dark text-white" style={{ borderRadius: "1rem" }} >
                            <div className="card-body p-5 text-center">

                                <div className="mb-md-5 mt-md-4 pb-5">

                                    <h2 className="fw-bold mb-2 text-uppercase">Login</h2>
                                    <p className="text-white-50 mb-5">Please enter your login and password!</p>

                                    <div data-mdb-input-init className="form-outline form-white mb-4">
                                        <input type="email" 
                                                id="typeEmailX" 
                                                placeholder="Email" 
                                                className="form-control form-control-lg"
                                                value={this.state.username}
                                                onChange={(e) => this.setState({username: e.target.value})} />
                                    </div>

                                    <div data-mdb-input-init className="form-outline form-white mb-4">
                                        <input type="password" 
                                                placeholder="Password" 
                                                id="typePasswordX" 
                                                className="form-control form-control-lg"
                                                value={this.state.password}
                                                onChange={(e) => this.setState({password: e.target.value})} />
                                    </div>

                                    {this.renderLoginButton()}
                                    {this.renderError()}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
  }
}