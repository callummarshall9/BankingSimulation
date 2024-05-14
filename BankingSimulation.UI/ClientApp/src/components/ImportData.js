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

        this.transactionEndpoint = {
            "rbs": "Transactions/ImportRBS",
            "barclays": "Transactions/ImportBarclays",
            "barclaysCard": "Transactions/ImportBarclaysCard",
            "mbna": "Transactions/ImportMBNA"
        };

        this.accountEndpoint = {
            "rbs": "Accounts/ImportRBS",
            "barclays": "Accounts/ImportBarclays",
            "barclaysCard": "Accounts/ImportBarclaysCard",
            "mbna": "Accounts/ImportMBNA"
        };
    }

    componentDidMount() {
        this.apiService.getAuthorisationToken();
    }

    async handleSubmit(e) {
        e.preventDefault();

        const form = e.target;
        const formData = new FormData(form);

        var selectedBank = formData.get("bank");

        await this.importAccounts(formData.get("requestBody"), this.accountEndpoint[selectedBank], () => {
            this.importTransactions(formData.get("requestBody"), this.transactionEndpoint[selectedBank]);
        });
    }

    async importAccounts(requestBody, endpoint, callback) {
        this.setState({ importing: true, imported: false, errorMessage: "" })

        try {
            await this.apiService.post(endpoint, requestBody);
            callback();

        } catch (_) {
            this.setState({ importing: false, imported: true, errorMessage: "Failed to import" });
        }
    }

    async importTransactions(requestBody, endpoint) {

        try {
            await this.apiService.post(endpoint, requestBody);
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
                <div class="mb-3">
                    <label for="exampleFormControlInput1" class="form-label">Bank</label>
                    <select name="bank" class="form-select">
                        <option value="rbs" selected>RBS</option>
                        <option value="barclays">Barclays</option>
                        <option value="barclaysCard">Barclays Card</option>
                        <option value="mbna">MBNA</option>
                    </select>                    
                </div>
    
                    <textarea rows="40" cols="100" name="requestBody" />
                    <br />
                    {this.getSubmitButton()}
                </form>
            </div>
        );
    }

}
