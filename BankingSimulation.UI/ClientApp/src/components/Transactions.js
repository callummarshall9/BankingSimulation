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
    }

    async populateTransactionsData() {
        if (this.state.activeCalendarEvent) {
            await this.loadForPeriod(this.state.activeCalendarEvent);
            return;
        }

        this.setState({ transactions: [], loading: true });

        var transactionData = await this.apiService.get("Transactions?$select=Id,Date,Description,Value,CategoryId&$expand=Category($select=Name),Account&$orderby=Date,Description");

        this.setState({ transactions: transactionData, searchableTransactions: transactionData, loading: false });
    }


    async populateCalendarEventsData() {
        this.setState({ calendarEvents: [], activeCalendarEvent: null, chartData: [] });

        var calendarEvents = await this.apiService.get("CalendarEvents?$orderby=Calendar/Name,Start&$expand=Calendar");
        var selectOptions = calendarEvents.map(ce => ({ label: ce.calendar.name + " - " + ce.name + " (" + ce.start + " - " + ce.end + ")", value: ce }));
        
        this.setState({ calendarEvents: calendarEvents, activeCalendarEvent: null, selectOptions: selectOptions });
    }

    matchTransactionDescription(source, st) {
        return source.split(";").map(text => st.description.toLowerCase().indexOf(text.toLowerCase()) !== -1).filter(r => r).length > 0
    }



    render() {
        console.log(this.state.showUnfiltered);

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderTransactionsTable(this.state.transactions.filter(r => this.state.showUnfiltered ? (!r.categoryId) : true));

        return (
            <div>
                <h1 id="tableLabel">Transactions</h1>
                
                <p>Showing your transactions {this.state.activeCalendarEvent != null ? <> between {this.state.activeCalendarEvent.start} and {this.state.activeCalendarEvent.end}</> : null }</p>
                <div style={{ width: "400px", marginTop: "5px" }}>
                    <Select options={this.state.selectOptions} onChange={(newValue) => this.changeCalendarEvent(newValue.value)}  />
                </div>
                {this.state.activeCalendarEvent != null ? <TransactionsChart chartData={this.state.chartData} /> : null}
                
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
                    <td>{transactions.map(t => t.value).reduce((a,b) => a + b).toFixed(2)}</td>
                </tr>
              </tbody>
            </table>
          );
    }

    changeCalendarEvent(newValue) {
        this.setState({ activeCalendarEvent: newValue });
        this.loadForPeriod(newValue);
    }

    async loadForPeriod(calendarEvent) {
        this.setState({ transactions: [], loading: true, chartData: [] });

        var forPeriodResults = await this.apiService.get("Transactions?$expand=Category($select=Name),Account&$select=Id,Date,Description,Value,CategoryId&$filter=Date ge " + calendarEvent.start + " and Date le " + calendarEvent.end + "&$orderby=Date,Description");
        let data = await this.apiService.get("Calendars/ComputeCalendarStats?calendarId=" + this.state.activeCalendarEvent.calendarId);

        this.setState({ transactions: forPeriodResults, searchableTransactions: forPeriodResults, loading: false, chartData: data });
        this.searchTransactions(this.state.search, forPeriodResults);
    }

    searchTransactions(text, transactions) {
        let newTransactions = transactions.filter(st => st.description !== null && this.matchTransactionDescription(text, st));

        this.setState({ search: text, transactions: newTransactions });
    }
}