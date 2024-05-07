import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import AddCalendarDialog from './AddCalendarDialog';
import { Link } from 'react-router-dom';

export default class Calendars extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();
        this.state = { calendars: [], loading: true, showAddCalendarDialog: false, editCalendarDialogId: null };
    }


    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.populateCalendarsData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderCalendarsTable(this.state.calendars);

        return (
            <div>
                <h1 id="tableLabel">Calendars</h1>
                <p><Button variant="primary" onClick={(_) => this.showAddCalendarDialog()}>Add Calendar</Button>Showing your calendars</p>
                {contents}
                {this.state.showAddCalendarDialog ? <AddCalendarDialog 
                    onClose={(_) => this.setState({ showAddCalendarDialog: false })} 
                    onSubmit={(_) => this.populateCalendarsData()}
                    /> : null}
            </div>
        );
    }

    static getCalendarEventUrl(calendarId) {
        return "/calendar?calendarId=" + calendarId;
    }

    renderCalendarsTable(calendars) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
              <thead>
                <tr>
                  <th>Name</th>
                  <th style={{ textAlign: "right" }}>Actions</th>
                </tr>
              </thead>
              <tbody>
                {calendars.map(calendar =>
                  <tr key={calendar.id}>
                    <td>{calendar.name}</td>
                    <td style={{ textAlign: "right" }}>
                        <Link to={Calendars.getCalendarEventUrl(calendar.id)}><Button 
                          variant="secondary" 
                          onClick={(_) => this.editCalendarId(calendar.id)}
                        >Edit</Button></Link>
                        {
                        calendar.Deleting 
                          ? <p>Deleting</p> 
                          : <Button 
                              variant="danger" 
                              onClick={(_) => this.deleteCalendarId(calendar.id)}
                            >Delete</Button>
                          }
                      </td>

                  </tr>
                )}
              </tbody>
            </table>
          );
    }

    async editCalendarId(calendarId) {
        this.setState({ editCalendarDialogId: calendarId });
    }

    async deleteCalendarId(calendarId) {
        var calendars = this.state.calendars;

        var calendarEntry = calendars.filter(r => r.id === calendarId)[0];

        calendarEntry.Deleting = true;

        this.setState({ calendars: calendars });

        await this.apiService.deleteJson("Calendars", { id: calendarId });

        this.populateCalendarsData();
    }

    async populateCalendarsData() {
        this.setState({ calendars: [], loading: true });

        var calendarData = await this.apiService.get("Calendars");

        this.setState({ calendars: calendarData, loading: false });
    }

    showAddCalendarDialog() {
        this.setState({ showAddCalendarDialog: true });
    }

}