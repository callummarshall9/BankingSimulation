import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';

export default class AddCalendarDialog extends Component {
    constructor(props) {
        super(props);
        this.apiService = new ApiService();
        this.state = {
            name: "",
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
                  <Modal.Title>Add calendar</Modal.Title>
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
                    <Form.Label>Calendar Name</Form.Label>
                    <Form.Control type="text" placeholder="Calendar name here" value={this.state.name} onChange={(e) => this.setState({ name: e.target.value })} />
                </Form.Group>
            </Modal.Body>
        );
    }

    renderAddCalendarDialogFooter() {
        if (!this.state.adding) {
            return (
                <Modal.Footer>
                  <Button variant="secondary" onClick={(_) => this.props.onClose()}>Close</Button>
                  <Button variant="primary" onClick={(_) => this.addCalendar()}>Add</Button>
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

    async createCalendarRoleIfNotExists() {
        await this.apiService.isLoggedIn();

        let userCalendarRole = await this.apiService.get("Roles?$filter=Name eq '" + this.apiService.userId + " Calendars'");

        if (userCalendarRole.length === 0) {
            let roleName = this.apiService.userId + " Calendars";

            var role = await this.apiService.postJson("Roles", { name: roleName });
            return role.id;
        } else {
            return userCalendarRole[0].id;
        }

    }


    async addCalendar() {
        this.setState({ adding: true });

        let roleId = await this.createCalendarRoleIfNotExists();

        await this.apiService.postJson("Calendars", { name: this.state.name, roleId: roleId });
        
        this.setState({ name: "", description: "", adding: false });
        this.props.onSubmit();
        this.props.onClose();
    }
}