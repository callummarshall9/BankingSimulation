import React, { Component } from 'react'
import ApiService from '../services/ApiService'

export default class ImportData extends Component {
    static displayName = ImportData.name;

    constructor(props) {
        super(props);
        this.apiService = new ApiService();
        this.state = {
            imported: false,
            importing: false,
            errorMessage: ""
        };
    }

    componentDidMount() {
        this.apiService.getAuthorisationToken();
    }

    async handleSubmit(e) {
        e.preventDefault();

        const form = e.target;
        const formData = new FormData(form);

        await this.importAccounts(formData.get("requestBody"), () => {
            this.importTransactions(formData.get("requestBody"));
        });
    }

    async importAccounts(requestBody, callback) {
        this.setState({ importing: true, imported: false, errorMessage: "" })

        try {
            await this.apiService.post("Accounts/ImportRBS", requestBody);
            callback();

        } catch (_) {
            this.setState({ importing: false, imported: true, errorMessage: "Failed to import" });
        }
    }

    async importTransactions(requestBody) {

        try {
            await this.apiService.post("Transactions/ImportRBS", requestBody);
            this.setState({ importing: false, imported: true, errorMessage: "" });
        } catch (_) {
            this.setState({ importing: false, imported: true, errorMessage: "Failed to import" });
        }
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
