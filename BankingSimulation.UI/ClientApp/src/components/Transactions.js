import React, { Component } from 'react';
import ApiService from '../services/ApiService';
import TransactionsChart from '../components/TransactionsChart'
import Select from 'react-select'
import Form from 'react-bootstrap/Form';
import { Button, InputGroup } from 'react-bootstrap';


export default class Transactions extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();
        this.state = { 
            accounts: [],
            activeAccounts : [],
            categories: [],
            activeCategories: [],
            transactions: [], 
            loading: true, 
            calendarEvents: [], 
            activeCalendarEvent: null, 
            chartData: [], 
            search: "",
            showUnfiltered: false
        };
    }

    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.populateTransactionsData();
        this.populateCalendarEventsData();
        this.populateAccountData();
        this.populateCategories();
    }

    async populateTransactionsData() {
        if (this.state.activeCalendarEvent) {
            await this.loadForPeriod(this.state.activeCalendarEvent);
            return;
        }

        this.setState({ transactions: [], loading: true });

        var transactionData = await this.apiService.get("Transactions?$select=Id,Date,Description,Value,CategoryId,AccountId&$expand=Category($select=Name),Account&$orderby=Date,Description");

        this.setState({ transactions: transactionData, searchableTransactions: transactionData, loading: false });
    }


    async populateCalendarEventsData() {
        this.setState({ calendarEvents: [], activeCalendarEvent: null, chartData: [] });

        var calendarEvents = await this.apiService.get("CalendarEvents?$orderby=Calendar/Name,Start&$expand=Calendar");
        var calendarEventSelectOptions = calendarEvents.map(ce => ({ label: ce.calendar.name + " - " + ce.name + " (" + ce.start + " - " + ce.end + ")", value: ce }));
        
        calendarEventSelectOptions.splice(0, 0, { label: "All", value: null });

        this.setState({ calendarEvents: calendarEvents, activeCalendarEvent: null, calendarEventSelectOptions: calendarEventSelectOptions });
    }

    async populateAccountData() {
        this.setState({ accounts: [] });

        var accounts = await this.apiService.get("Accounts?$select=Name,Number,Id&$orderby=Name");
        var accountSelectOptions = accounts.map(a => ({ label: a.name + " (" + a.number + ")", value: a.id }));

        accountSelectOptions.splice(0, 0, { label: "All", value: null });

        this.setState({ accounts: accountSelectOptions });
    }

    changeAccount(activeAccountOptions) {
        this.setState({ activeAccounts: activeAccountOptions.map(a => a.value) });
        this.loadForPeriod(this.state.activeCalendarEvent);
    }

    async populateCategories() {
        this.setState({ categories: [] });

        var categories = await this.apiService.get("Categories?$select=Id,Name&$orderby=Name");

        var categorySelectOptions = categories.map(c => ({ label: c.name, value: c.id }));

        categorySelectOptions.splice(0, 0, { label: "All", value: null });

        this.setState({ categories: categorySelectOptions });
    }

    changeCategory(activeCategoryOptions) {
        console.log(activeCategoryOptions);
        this.setState({ activeCategories: activeCategoryOptions.map(a => a.value) });
    }

    matchTransactionDescription(source, st) {
        return source.split(";").map(text => st.description.toLowerCase().indexOf(text.toLowerCase()) !== -1).filter(r => r).length > 0
    }

    render() {
        console.log(this.state.showUnfiltered);

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderTransactionsTable(this.state.transactions
                .filter(r => this.state.showUnfiltered ? (!r.categoryId) : true)
                .filter(r => this.state.activeAccounts.length > 0 ? (this.state.activeAccounts.indexOf(r.accountId) !== -1) : true)
                .filter(r => this.state.activeCategories.length > 0 ? (this.state.activeCategories.indexOf(r.categoryId) !== -1) : true)
            );

        return (
            <div>
                <h1 id="tableLabel">Transactions</h1>
                
                <p>Showing your transactions {this.state.activeCalendarEvent != null ? <> between {this.state.activeCalendarEvent.start} and {this.state.activeCalendarEvent.end}</> : null }</p>
                <div className="row">
                    <div className="col-sm-4">
                        Calendar Events: 
                        <div style={{ width: "400px", marginTop: "5px" }}>
                            <Select options={this.state.calendarEventSelectOptions} onChange={(newValue) => this.changeCalendarEvent(newValue.value)}  />
                        </div>
                    </div>
                    <div className="col-sm-4">
                        Accounts:
                        <div style={{ width: "400px", marginTop: "5px" }}>
                            <Select isMulti options={this.state.accounts} onChange={(newValue) => this.changeAccount(newValue)}  />
                        </div>
                    </div>
                    <div className="col-sm-4">
                        Categories:
                        <div style={{ width: "400px", marginTop: "5px" }}>
                            <Select isMulti options={this.state.categories} onChange={(newValue) => this.changeCategory(newValue)}  />
                        </div>
                    </div>
                </div>

                {this.state.activeCalendarEvent != null ? <TransactionsChart chartData={this.getChartData(this.state.chartData)} /> : null}
                
                <Form.Label htmlFor="searchTransactions">Search</Form.Label>
                <InputGroup>
                    <Form.Control
                        type="text"
                        id="searchTransactions"
                        onChange={(e) => this.searchTransactions(e.target.value, this.state.searchableTransactions)}
                        value={this.state.search}
                    />
                    <Button variant="primary" onClick={() => this.populateTransactionsData()}>
                        Refresh
                    </Button>
                </InputGroup>
                <Form.Check // prettier-ignore
                    type="switch"
                    id="custom-switch"
                    label="Show only uncategorised"
                    checked={this.state.showUnfiltered}
                    onChange={(e) => this.setState({ showUnfiltered: e.target.checked })}
                />

                {contents}
            </div>
        );
    }

    getChartData(data) {
        return data
            .filter(d =>
                this.state.activeCategories.length > 0 ? (this.state.activeCategories.indexOf(d.categoryId) !== -1) : true
            )
            .filter(d => this.state.showUnfiltered ? (d.categoryId === null) : true)
    }

    static getTransactionCategory(transaction) {
        if (transaction.category) {
            return <span className="badge badge-primary">{transaction.category.name}</span>;
        }

        return null;
    }

    renderTransactionsTable(transactions) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
              <thead>
                <tr>
                  <th>Date</th>
                  <th>Description</th>
                  <th>Account Number</th>
                  <th>Value</th>
                </tr>
              </thead>
              <tbody>
                {transactions.map(transaction =>
                  <tr key={transaction.id}>
                    <td>{new Date(transaction.date).toLocaleDateString("en-GB")}</td>
                    <td>{transaction.description}  {Transactions.getTransactionCategory(transaction)}</td>
                    <td>{transaction.account.number}</td>
                    <td>{transaction.value}</td>
                  </tr>
                )}
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>{Transactions.calculateTransactionsSum(transactions)}</td>
                </tr>
              </tbody>
            </table>
          );
    }

    static calculateTransactionsSum(transactions) {
        if (transactions.length === 0)
            return 0.00;

        return transactions.map(t => t.value).reduce((a,b) => a + b).toFixed(2);
    }

    changeCalendarEvent(newValue) {
        this.setState({ activeCalendarEvent: newValue });
        this.loadForPeriod(newValue);
    }

    async loadForPeriod(calendarEvent) {
        this.setState({ 
            transactions: [], 
            loading: true, 
            chartData: []
        });

        if (calendarEvent === null) {
            await this.populateTransactionsData();
            return;
        }

        var forPeriodResults = await this.apiService.get("Transactions?$expand=Category($select=Name),Account&$select=Id,AccountId,Date,Description,Value,CategoryId&$filter=Date ge " + calendarEvent.start + " and Date le " + calendarEvent.end + "&$orderby=Date,Description");
        let data = null;

        if (this.state.activeAccounts.length === 0) {
            data = await this.apiService.get("Calendars/ComputeCalendarStats?calendarId=" 
                + this.state.activeCalendarEvent.calendarId);
        } else {
            data = await this.apiService.get("Calendars/ComputeCalendarAccountStats?calendarId=" 
                + this.state.activeCalendarEvent.calendarId + "&accountIds=" + this.state.activeAccounts.join());
        }

        this.setState({ 
            transactions: forPeriodResults, 
            searchableTransactions: forPeriodResults, 
            loading: false, 
            chartData: data, 
        });

        this.searchTransactions(this.state.search, forPeriodResults);
    }

    searchTransactions(text, transactions) {
        let newTransactions = transactions.filter(st => st.description !== null && this.matchTransactionDescription(text, st));

        this.setState({ 
            search: text, 
            transactions: newTransactions 
        });
    }
}