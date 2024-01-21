import React, { Component } from 'react'

export class ImportData extends Component {
    static displayName = ImportData.name;

    constructor(props) {
        super(props);
        this.state = {
            config: { apiUrl: "https://localhost:44338/" },
            imported: false,
            importing: false,
            errorMessage: ""
        };
    }

    componentDidMount() {
    }

    handleSubmit(e) {
        e.preventDefault();

        const form = e.target;
        const formData = new FormData(form);

        this.importAccounts(formData.get("requestBody"), () => {
            this.importTransactions(formData.get("requestBody"));
        });
    }

    importAccounts(requestBody, callback) {
        this.setState({ importing: true, imported: false, errorMessage: "" })

        fetch(this.state.config.apiUrl + 'Transactions/ImportRBS', {
            method: "POST",
            body: requestBody
        }).then((response) => {
            if (response.status === 200) {
                callback();
            } else {
                this.setState({ importing: false, imported: true, errorMessage: "Failed to import" });
                console.log(response.status, response.statusText);
            }
        })
        .catch((error) => {
            this.setState({ importing: false, imported: true, errorMessage: "Failed to import" });
            console.log(error.status, error.statusText);
        });
    }

    importTransactions(requestBody) {
        fetch(this.state.config.apiUrl + 'Accounts/ImportRBS', {
            method: "POST",
            body: requestBody
        }).then((response) => {
            if (response.status === 200) {
                this.setState({ importing: false, imported: true, errorMessage: "" });
            } else {
                this.setState({ importing: false, imported: true, errorMessage: "Failed to import" });
                console.log(response.status, response.statusText);
            }

        })
        .catch((error) => {
            this.setState({ importing: false, imported: true, errorMessage: "Failed to import" });
            console.log(error.status, error.statusText);
            // 3. get error messages, if any
            error.json().then((json) => {
                console.log(json);
            });
        });
    }

    getLoadingMessage() {
        if (!this.state.importing && this.state.imported && this.state.errorMessage === "") {
            return (
                <div className="alert alert-success">Data Imported</div>
            );
        } else if (this.state.importing) {
            return (
                <div className="alert alert-info">Importing Data</div>
            );
        } else if (this.state.imported && this.state.errorMessage !== "") {
            return (
                <div className="alert alert-error">{this.state.errorMessage}</div>
            );
        } else {
            return null;
        }
    }

    getSubmitButton() {
        if (!this.state.importing) {
            return (
                <button type="submit" className="btn btn-primary">Import Data</button>
            )
        } else {
            return null;
        }
    }

    render() {
        return (
            <div>
                <h1>Import Data</h1>
                {this.getLoadingMessage()}
                <form method="POST" onSubmit={(e) => this.handleSubmit(e)}>
                    <textarea rows="40" cols="100" name="requestBody" />
                    <br />
                    {this.getSubmitButton()}
                </form>
            </div>
        );
    }

}
