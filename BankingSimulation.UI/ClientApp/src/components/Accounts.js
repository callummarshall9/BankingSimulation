import React, { Component } from 'react';
import ApiService from '../services/ApiService';

export default class Accounts extends Component {
  static displayName = Accounts.name;

  constructor(props) {
    super(props);
    this.apiService = new ApiService();

    this.state = { accounts: [], loading: true };
  }

  componentDidMount() {
    this.apiService.getAuthorisationToken();
    this.populateAccountData();
  }

  static getAccountName(account) {
    var name = account.Name;

      if (account.FriendlyName && account.FriendlyName !== "") {
          name = account.FriendlyName + " (" + account.Name + ")";
    }

    return name;
  }

  static getAccountSystems(accountSystems) {
    return accountSystems.map(acs => (<span key={acs.BankingSystemId} className="badge badge-primary">{acs.BankingSystemId}</span>));
  }

  static renderAccountsTable(accounts) {
    return (
      <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Number</th>
            <th>Systems</th>
          </tr>
        </thead>
        <tbody>
          {accounts.map(account =>
            <tr key={account.Id}>
              <td>{Accounts.getAccountName(account)}</td>
              <td>{account.Number}</td>
              <td>{Accounts.getAccountSystems(account.AccountSystemReferences)}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Accounts.renderAccountsTable(this.state.accounts);

    return (
      <div>
        <h1 id="tableLabel">Accounts</h1>
        <p>Showing your accounts</p>
        {contents}
      </div>
    );
  }

    async populateAccountData() {
        var accountData = await this.apiService.get("Accounts?$expand=AccountSystemReferences");

      this.setState({ accounts: accountData, loading: false });
  }
}
