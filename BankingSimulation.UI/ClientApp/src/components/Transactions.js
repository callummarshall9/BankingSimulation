import React, { Component } from 'react';
import ApiService from '../services/ApiService';
import Select from 'react-select'
import Button from 'react-bootstrap/Button';

export default class Transactions extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();
        this.state = { transactions: [], loading: true, calendarEvents: [], activeCalendarEvent: null };
    }

    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.populateTransactionsData();
        this.populateCalendarEventsData();
    }

    async populateTransactionsData() {
        if (this.state.activeCalendarEvent) {
            await this.loadForPeriod(this.state.activeCalendarEvent);
            return;
        }

        this.setState({ transactions: [], loading: true });

        var transactionData = await this.apiService.get("Transactions?$select=Id,Date,Description,Value,CategoryId&$expand=Category($select=Name)&$orderby=Date,Description");

        this.setState({ transactions: transactionData, loading: false });
    }


    async populateCalendarEventsData() {
        this.setState({ calendarEvents: [], activeCalendarEvent: null });

        var calendarEvents = await this.apiService.get("CalendarEvents?$orderby=Start&$expand=Calendar");
        var selectOptions = calendarEvents.map(ce => ({ label: ce.calendar.name + " - " + ce.name + " (" + ce.start + " - " + ce.end + ")", value: ce }));
        
        this.setState({ calendarEvents: calendarEvents, activeCalendarEvent: null, selectOptions: selectOptions });
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderTransactionsTable(this.state.transactions);

        return (
            <div>
                <h1 id="tableLabel">Transactions</h1>
                <p>Showing your transactions</p>
                {this.state.activeCalendarEvent != null ? <p> between {this.state.activeCalendarEvent.start} and {this.state.activeCalendarEvent.end}</p> : null }
                <div style={{ width: "400px", marginTop: "5px" }}>
                    <Select options={this.state.selectOptions} onChange={(newValue) => this.changeCalendarEvent(newValue.value)}  />
                </div>
                {contents}
            </div>
        );
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
                  <th>Value</th>
                </tr>
              </thead>
              <tbody>
                {transactions.map(transaction =>
                  <tr key={transaction.id}>
                    <td>{new Date(transaction.date).toLocaleDateString("en-GB")}</td>
                    <td>{transaction.description}  {Transactions.getTransactionCategory(transaction)}</td>
                    <td>{transaction.value}</td>
                  </tr>
                )}
              </tbody>
            </table>
          );
    }

    changeCalendarEvent(newValue) {
        this.setState({ activeCalendarEvent: newValue });
        this.loadForPeriod(newValue);
    }

    async loadForPeriod(calendarEvent) {
        this.setState({ transactions: [], loading: true });

        var forPeriodResults = await this.apiService.get("Transactions?$expand=Category($select=Name)&$select=Id,Date,Description,Value,CategoryId&$filter=Date ge " + calendarEvent.start + " and Date le " + calendarEvent.end + "&$orderby=Date,Description");

        this.setState({ transactions: forPeriodResults, loading: false });
    }
}