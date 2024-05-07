import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import AddCalendarEventDialog from './AddCalendarEventDialog';

export default class CalendarEvents extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();

        var params = new URLSearchParams(window.location.search);
        var calendarId = params.get("calendarId");

        this.state = { calendarEvents: [], loading: true, showAddCalendarEventDialog: false, editCalendarEventDialogId: null, calendarId: calendarId };
    }


    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.populateCalendarEventsData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderCalendarEventsTable(this.state.calendarEvents);

        return (
            <div>
                <h1 id="tableLabel">Calendar Events</h1>
                <p><Button variant="primary" onClick={(_) => this.showAddCalendarEventDialog()}>Add Calendar Event</Button>Showing your calendar events</p>
                {contents}
                {this.state.showAddCalendarEventDialog ? <AddCalendarEventDialog 
                    calendarId={this.state.calendarId}
                    onClose={(_) => this.setState({ showAddCalendarEventDialog: false })} 
                    onSubmit={(_) => this.populateCalendarEventsData()}
                    /> : null}
            </div>
        );
    }

    renderCalendarEventsTable(calendarEvents) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Start</th>
                  <th>End</th>
                  <th style={{ textAlign: "right" }}>Actions</th>
                </tr>
              </thead>
              <tbody>
                {calendarEvents.map(calendarEvent =>
                  <tr key={calendarEvent.id}>
                    <td>{calendarEvent.name}</td>
                    <td>{new Date(calendarEvent.start).toLocaleDateString("en-GB")}</td>
                    <td>{new Date(calendarEvent.end).toLocaleDateString("en-GB")}</td>
                    <td style={{ textAlign: "right" }}>
                        {
                        calendarEvent.Deleting 
                          ? <p>Deleting</p> 
                          : <Button 
                              variant="danger" 
                              onClick={(_) => this.deleteCalendarEventId(calendarEvent.id)}
                            >Delete</Button>
                          }
                      </td>

                  </tr>
                )}
              </tbody>
            </table>
          );
    }

    async editCalendarEventId(calendarEventId) {
        this.setState({ editCalendarEventDialogId: calendarEventId });
    }

    async deleteCalendarEventId(calendarEventId) {
        var calendarEvents = this.state.calendarEvents;

        var calendarEventEntry = calendarEvents.filter(r => r.id === calendarEventId)[0];

        calendarEventEntry.Deleting = true;

        this.setState({ calendarEvents: calendarEvents });

        await this.apiService.deleteJson("CalendarEvents", { id: calendarEventId });

        this.populateCalendarEventsData();
    }

    async populateCalendarEventsData() {
        this.setState({ calendarEvents: [], loading: true });

        var calendarEventData = await this.apiService.get("CalendarEvents?$filter=CalendarId eq " + this.state.calendarId + "&$expand=Calendar");

        this.setState({ calendarEvents: calendarEventData, loading: false });
    }

    showAddCalendarEventDialog() {
        this.setState({ showAddCalendarEventDialog: true });
    }

}