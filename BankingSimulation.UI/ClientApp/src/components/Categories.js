import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import AddCategoryDialog from './AddCategoryDialog';
import EditCategoryDialog from './EditCategoryDialog';

import Select from 'react-select'

export default class Categories extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();
        this.state = { 
            categories: [], 
            accounts: [],
            activeAccounts: [],
            loading: true, 
            showAddCategoryDialog: false, 
            editCategoryDialogId: null, 
            calendarEvents: [], 
            selectOptions: [],
            activeCalendarEvent: null 
        };
    }


    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.populateCategoriesData();
        this.populateCalendarEventsData();
        this.populateAccountData();
    }

    async populateCategoriesData() {
        this.setState({ categories: [], loading: true, showAddCategoryDialog: false, editCategoryDialogId: null });

        var categoryData = await this.apiService.get("Categories?$expand=Keywords($orderby=Keyword)");

        this.setState({ categories: categoryData, loading: false });
    }

    async populateCalendarEventsData() {
        this.setState({ calendarEvents: [], activeCalendarEvent: null });

        var calendarEvents = await this.apiService.get("CalendarEvents?$orderby=Calendar/Name,Start&$expand=Calendar");
        var selectOptions = calendarEvents.map(ce => ({ label: ce.calendar.name + " - " + ce.name + " (" + ce.start + " - " + ce.end + ")", value: ce }));
        
        this.setState({ calendarEvents: calendarEvents, activeCalendarEvent: null, selectOptions: selectOptions });
    }

    async populateAccountData() {
        this.setState({ accounts: [] });

        var accounts = await this.apiService.get("Accounts?$select=Name,Number,Id&$orderby=Name");
        var accountSelectOptions = accounts.map(a => ({ label: a.name + " (" + a.number + ")", value: a.id }));

        accountSelectOptions.splice(0, 0, { label: "All", value: null });

        this.setState({ accounts: accountSelectOptions });
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderCategoriesTable(this.state.categories);

        return (
            <div>
                <h1 id="tableLabel">Categories</h1>
                <p>
                    <Button variant="primary" onClick={(_) => this.showAddCategoryDialog()}>Add Category</Button>
                </p>

                <div className="row">
                    <div className="col-sm-4">
                        Calendar Events: 
                        <div style={{ width: "400px", marginTop: "5px" }}>
                            <Select options={this.state.selectOptions} onChange={(newValue) => this.changeCalendarEvent(newValue.value)}  />
                        </div>
                    </div>
                    <div className="col-sm-4">
                        Accounts:
                        <div style={{ width: "400px", marginTop: "5px" }}>
                            <Select isMulti options={this.state.accounts} onChange={(newValue) => this.changeAccount(newValue)}  />
                        </div>
                    </div>
                </div>


                {contents}
                {this.state.showAddCategoryDialog ? <AddCategoryDialog 
                    onClose={(_) => this.populateCategoriesData()} 
                    onSubmit={(_) => this.populateCategoriesData()}
                    /> : null}
                {this.state.editCategoryDialogId ? <EditCategoryDialog
                    categoryId={this.state.editCategoryDialogId}
                    onClose={(_) => this.populateCategoriesData()}
                    onSubmit={(_) => this.populateCategoriesData()} 
                    /> : null}
            </div>
        );
    }

    changeCalendarEvent(newValue) {
        this.setState({ activeCalendarEvent: newValue });
        this.loadForPeriod(newValue);
    }

    changeAccount(activeAccountOptions) {
        this.setState({ activeAccounts: activeAccountOptions.map(a => a.value) });

        if (this.state.activeCalendarEvent !== null) {
            this.loadForPeriod(this.state.activeCalendarEvent);
        }
    }

    async loadForPeriod(calendarEvent) {
        this.setState({ categories: [], loading: true });

        let forPeriodResults = null;

        if (this.state.activeAccounts.length === 0) {
            forPeriodResults = await this.apiService.get("Categories/ForPeriod?fromPeriod=" + calendarEvent.start + "&toPeriod=" + calendarEvent.end);
        } else {
            forPeriodResults = await this.apiService.get("Categories/AccountsForPeriod?fromPeriod=" + calendarEvent.start + "&toPeriod=" + calendarEvent.end + "&accountIds=" + this.state.activeAccounts.join());
        }

        this.setState({ categories: forPeriodResults.results, loading: false });
    }

    getButtons(category) {
        if (category.id === null) {
            return <td></td>;
        }

        return (
        <td>
            <Button 
              variant="secondary" 
              onClick={(_) => this.editCategoryId(category.id)}
            >Edit</Button>
            {
            category.Deleting 
              ? <p>Deleting</p> 
              : <Button 
                  variant="danger" 
                  onClick={(_) => this.deleteCategoryId(category.id)}
                >Delete</Button>
              }
          </td>
        );
    }

    renderCategoriesTable(categories) {
        if (this.state.activeAccounts.length > 0 && this.state.activeCalendarEvent === null) {
            return <p><em>Please select a calendar event or remove the account selection to view category analysis for.</em></p>
        }

        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Description</th>
                  <th>Keywords</th>
                  {this.state.activeCalendarEvent != null ? <th>Sum of category between {this.state.activeCalendarEvent.start} and {this.state.activeCalendarEvent.end}</th> : null }
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {categories.map(category =>
                  <tr key={category.id}>
                    <td>{category.name}</td>
                    <td>{category.description}</td>
                    <td>{Categories.getCategoryKeywords(category.keywords)}</td>
                    {this.state.activeCalendarEvent != null ? <td>{category.sum}</td> : null }
                    {this.getButtons(category)}
                  </tr>
                )}
                {this.state.activeCalendarEvent != null ? <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>{Math.round(categories.map(c => c.sum).reduce((a,b) => a + b) * 100.0) / 100.0}</td>
                    <td></td>
                </tr> : null}
              </tbody>
            </table>
          );
    }

    static getCategoryKeywords(keywords) {
        if (keywords === null)
            return;

        return keywords.map(k => (<div key={k.id} className="badge badge-primary">{k.keyword}</div>));
    }

    async editCategoryId(categoryId) {
        this.setState({ editCategoryDialogId: categoryId });
    }

    async deleteCategoryId(categoryId) {
        var categories = this.state.categories;

        var categoryEntry = categories.filter(r => r.id === categoryId)[0];

        categoryEntry.Deleting = true;

        this.setState({ categories: categories });

        await this.apiService.deleteJson("Categories", { id: categoryId });

        this.populateCategoriesData();
    }

    showAddCategoryDialog() {
        this.setState({ showAddCategoryDialog: true });
    }

}