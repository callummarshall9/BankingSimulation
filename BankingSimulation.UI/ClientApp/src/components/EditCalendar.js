import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import { Button, InputGroup, Form } from 'react-bootstrap';
import AddCalendarEventDialog from './AddCalendarEventDialog';
import TransactionsNetChart from './TransactionsNetChart';

export default class EditCalendar extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();

        var params = new URLSearchParams(window.location.search);
        var calendarId = params.get("calendarId");

        this.state = { 
          calendarEvents: [], 
          loading: true, 
          showAddCalendarEventDialog: false, 
          editCalendarEventDialogId: null, 
          calendarId: calendarId, 
          calendar: null,
          chartData: []
        };
    }


    async componentDidMount() {
        this.apiService.getAuthorisationToken();
        await this.populateCalendarData();
        await this.populateChartData();
    }

    async populateChartData() {
      var data = await this.apiService.get("Calendars/ComputeNetCalendarStats?calendarId=" + this.state.calendarId);

      this.setState({ chartData: data });
    }

    async changeCalendarName(name) {
      let copy = JSON.parse(JSON.stringify(this.state.calendar));

      copy.name = name;

      this.setState({ calendar: copy });
    }

    async updateCalendarName() {
      await this.apiService.putJson("Calendars", this.state.calendar);
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderCalendarEventsTable(this.state.calendarEvents);

        return (
            <div>
                {this.state.chartData.length > 0 ? <TransactionsNetChart chartData={this.state.chartData} /> : null }

                {this.state.calendar != null ? <InputGroup>
                    <Form.Control 
                        type="text" 
                        placeholder="Name" 
                        value={this.state.calendar.name} 
                        onChange={(e) => this.changeCalendarName(e.target.value)} 
                        />
                    <Button variant="primary" onClick={(_) => this.updateCalendarName()} className="float-end">Update Name</Button>
                </InputGroup> : null}

                <h1 id="tableLabel">Calendar Events</h1>
                <p><Button variant="primary" onClick={(_) => this.showAddCalendarEventDialog()}>Add Calendar Event</Button>Showing your calendar events</p>
                {contents}
                {this.state.showAddCalendarEventDialog ? <AddCalendarEventDialog 
                    calendarId={this.state.calendarId}
                    onClose={(_) => this.setState({ showAddCalendarEventDialog: false })} 
                    onSubmit={(_) => this.populateCalendarData()}
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

        await this.populateCalendarData();
    }

    async populateCalendarData() {
      this.setState({ calendarEvents: [], loading: true });

      var calendarData = await this.apiService.get("Calendars?$filter=Id eq " + this.state.calendarId + "&$expand=CalendarEvents");

      this.setState({ calendarEvents: calendarData[0].calendarEvents, calendar: calendarData[0], loading: false });
    }

    showAddCalendarEventDialog() {
        this.setState({ showAddCalendarEventDialog: true });
    }

}