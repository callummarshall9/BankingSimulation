import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';

export default class AddCalendarEventDialog extends Component {
    constructor(props) {
        super(props);
        this.apiService = new ApiService();
        this.state = {
            name: "", 
            start: new Date().toDateString(), 
            end: new Date().toDateString(),
            adding: false
        };
    }

    componentDidMount() {
        this.apiService.getAuthorisationToken();
    }

    render() {
        return (
              <Modal show={true}>
                <Modal.Header>
                  <Modal.Title>Add calendar events</Modal.Title>
                </Modal.Header>
                {this.renderAddCalendarDialogBody()}
                {this.renderAddCalendarDialogFooter()}
              </Modal>
          );
    }

    renderAddCalendarDialogBody() {
        if (this.state.adding) {
            return;
        }

        return (
            <Modal.Body>
                <Form.Group className="mb-3">
                    <Form.Label>Name</Form.Label>
                    <Form.Control type="text" placeholder="Calendar name here" value={this.state.name} onChange={(e) => this.setState({ name: e.target.value })} />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Start</Form.Label>
                    <Form.Control type="date" placeholder="Start date here" value={this.state.start} onChange={(e) => this.setState({ start: e.target.value })} />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>End</Form.Label>
                    <Form.Control type="date" placeholder="End date here" value={this.state.end} onChange={(e) => this.setState({ end: e.target.value })} />
                </Form.Group>
            </Modal.Body>
        );
    }

    renderAddCalendarDialogFooter() {
        if (!this.state.adding) {
            return (
                <Modal.Footer>
                  <Button variant="secondary" onClick={(_) => this.props.onClose()}>Close</Button>
                  <Button variant="primary" onClick={(_) => this.addCalendarEvent()}>Add</Button>
                </Modal.Footer>
            );
        } else {
            return (
                <Modal.Footer>
                  <p>Adding</p>
                </Modal.Footer>
            )
        }
    }

    async addCalendarEvent() {
        this.setState({ adding: true });

        await this.apiService.postJson("CalendarEvents", { name: this.state.name, start: this.state.start, end: this.state.end, calendarId: this.props.calendarId });
        
        this.setState({ name: "", start: new Date().toDateString(), end: new Date().toDateString(), adding: false });
        this.props.onSubmit();
        this.props.onClose();
    }
}