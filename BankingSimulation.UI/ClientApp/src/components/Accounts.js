import React, { Component } from 'react';
import ApiService from '../services/ApiService';
import AccountSummaryChart from './AccountSummaryChart';

import Select from 'react-select';

export default class Accounts extends Component {
  static displayName = Accounts.name;

  constructor(props) {
    super(props);
    this.apiService = new ApiService();

    this.state = { 
      accounts: [], 
      chartData: [],
      loading: true 
    };
  }

  componentDidMount() {
    this.apiService.getAuthorisationToken();
    this.populateAccountData();
    this.populateCalendarData();
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
            <tr key={account.Id}>
              <td>{Accounts.getAccountName(account)}</td>
              <td>{account.number}</td>
              <td>{Accounts.getAccountSystems(account.accountSystemReferences)}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  async changeCalendar(calendarId) {
    var chartData = await this.apiService.get("Transactions/GetCalendarEventAccountSummaries?calendarId=" + calendarId);

    this.setState({ chartData: chartData });
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Accounts.renderAccountsTable(this.state.accounts);

    return (
      <div>
        <h1 id="tableLabel">Accounts</h1>
        <div style={{ width: "400px", marginTop: "5px" }}>
            <Select options={this.state.calendars} onChange={(newValue) => this.changeCalendar(newValue.value.id)}  />
        </div>
        {this.state.chartData.length > 0 ? <AccountSummaryChart chartData={this.state.chartData} /> : null }

        <p>Showing your accounts</p>
        {contents}
      </div>
    );
  }

  async populateAccountData() {
    var accountData = await this.apiService.get("Accounts?$expand=AccountSystemReferences");

    this.setState({ accounts: accountData, loading: false });
  }

  async populateCalendarData() {
    var calendarData = await this.apiService.get("Calendars?$orderby=Name");

    var selectOptions = calendarData.map(c => ({ label: c.name, value: c }));

    this.setState({ calendars: selectOptions });
  }
}
