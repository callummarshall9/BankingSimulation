import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';

export default class AddRoleDialog extends Component {
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
                  <Modal.Title>Add role</Modal.Title>
                </Modal.Header>
                {this.renderAddRoleDialogBody()}
                {this.renderAddRoleDialogFooter()}
                
              </Modal>
          );
    }

    renderAddRoleDialogBody() {
        if (this.state.adding) {
            return;
        }

        return (
            <Modal.Body>
                <Form.Group className="mb-3">
                    <Form.Label>Role Name</Form.Label>
                    <Form.Control type="text" placeholder="Role name here" value={this.state.name} onChange={(e) => this.setState({ name: e.target.value })} />
                </Form.Group>
            </Modal.Body>
        );
    }

    renderAddRoleDialogFooter() {
        if (!this.state.adding) {
            return (
                <Modal.Footer>
                  <Button variant="secondary" onClick={(_) => this.props.onClose()}>Close</Button>
                  <Button variant="primary" onClick={(_) => this.addRole()}>Add</Button>
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


    async addRole() {
        this.setState({ adding: true });

        await this.apiService.postJson("Roles", { name: this.state.name });
        
        this.setState({ name: "", adding: false });
        this.props.onSubmit();
        this.props.onClose();
    }
}