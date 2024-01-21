import React, { Component } from 'react';

export class Accounts extends Component {
  static displayName = Accounts.name;

  constructor(props) {
    super(props);
    this.state = { accounts: [], loading: true, config: { apiUrl: "https://localhost:7050/" } };
  }

  componentDidMount() {
    this.populateAccountData();
  }

  static getAccountName(account) {
    var name = account.name;

    if (account.friendlyName && account.friendlyName !== "") {
        name = account.friendlyName + " (" + account.name + ")";
    }

    return name;
  }

  static getAccountSystems(accountSystems) {
    return accountSystems.map(acs => (<span key={acs.bankingSystemId} className="badge badge-primary">{acs.bankingSystemId}</span>));
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
            <tr key={account.id}>
              <td>{Accounts.getAccountName(account)}</td>
              <td>{account.number}</td>
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
    const accountRequest = await fetch(this.state.config.apiUrl + 'Accounts');
    const accountData = await accountRequest.json();

    const accountBankingSystemReferenceRequest = await fetch(this.state.config.apiUrl + 'AccountBankingSystemReferences');
    const accountBankingSystemReferenceData = await accountBankingSystemReferenceRequest.json();

    console.log(accountData);
    console.log(accountBankingSystemReferenceData);

    var finalAccountData = accountData.map(ad => {
        ad.AccountSystemReferences = accountBankingSystemReferenceData.filter(absrd => absrd.accountId === ad.id);
        return ad;
    });

    this.setState({ accounts: finalAccountData, loading: false });
  }
}
