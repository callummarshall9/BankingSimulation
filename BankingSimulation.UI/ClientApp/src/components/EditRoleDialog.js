import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { InputGroup } from 'react-bootstrap';

export default class EditRoleDialog extends Component {
    constructor(props) {
        super(props);
        this.apiService = new ApiService();
        this.state = {
            role: { name: "", userRoles: [] },
            editing: false
        };
    }

    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.getRoleData();
    }

    async getRoleData() {
        this.setState({ role: { name: "", userRoles: [] } });

        var roleId = this.props.roleId;

        var roleEntry = await this.apiService.get("Roles?$filter=Id eq " + roleId + "&$expand=UserRoles");

        if (roleEntry.length === 1) {
            this.setState({ role: roleEntry[0] });
        }
    }

    render() {
        if (this.props.roleId === null) {
            return;
        }

        return (
              <Modal show={true}>
                <Modal.Header>
                  <Modal.Title>{this.state.role.name}</Modal.Title>
                </Modal.Header>
                {this.renderEditRoleDialogBody()}
                {this.renderEditRoleDialogFooter()}
                
              </Modal>
          );
    }

    changeRole(name) {
        let roleCopy = JSON.parse(JSON.stringify(this.state.role));

        roleCopy.name = name;

        this.setState({ role: roleCopy });
    }

    renderEditRoleDialogBody() {
        if (this.state.editing) {
            return;
        }

        return (
            <Modal.Body>
                <InputGroup>
                    <Form.Control 
                        type="text" 
                        placeholder="Role name here" 
                        value={this.state.role.name} 
                        onChange={(e) => this.changeRole(e.target.value)} 
                        />
                    <Button variant="primary" onClick={(_) => this.updateName()} className="float-end">Update Name</Button>
                </InputGroup>

                <InputGroup style={{ marginTop: "5px" }} >
                    <Form.Control 
                        type="text" 
                        placeholder="User Id" 
                        value={this.state.userId} 
                        onChange={(e) => this.setState({ userId: e.target.value })} 
                    />
                    <Button variant="secondary" onClick={(_) => this.addUser()} className="float-end">Add User</Button>
                </InputGroup>

                {this.renderEditRoleDialogUsers(this.state.role.userRoles)}
            </Modal.Body>
        );
    }

    renderEditRoleDialogUsers(users) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
              <thead>
                <tr>
                  <th>Name</th>
                  <th style={{ textAlign: "right" }}>Actions</th>
                </tr>
              </thead>
              <tbody>
                {users.map(user =>
                  <tr key={user.userId}>
                    <td>{user.userId}</td>
                    <td style={{ textAlign: "right" }}>{user.Deleting ? <p>Deleting</p> : <Button variant="danger" onClick={(_) => this.deleteUserRole(user.userId)}>Delete</Button>}</td>
                  </tr>
                )}
              </tbody>
            </table>
        );
    }

    async deleteUserRole(userId) {
        this.setState({ editing: true });

        await this.apiService.deleteJson("UserRoles", { userId: userId, roleId: this.state.role.id });
        
        await this.getRoleData();
        
        this.setState({ editing: false });
    }

    renderEditRoleDialogFooter() {
        if (!this.state.editing) {
            return (
                <Modal.Footer>
                  <Button variant="secondary" onClick={(_) => this.props.onClose()}>Close</Button>
                </Modal.Footer>
            );
        } else {
            return (
                <Modal.Footer>
                  <p>Saving</p>
                </Modal.Footer>
            )
        }
    }


    async updateName() {
        this.setState({ editing: true });

        await this.apiService.putJson("Roles", this.state.role);
        
        this.setState({ editing: false });
        this.props.onSubmit();
    }

    async addUser() {
        this.setState({ editing: true });

        await this.apiService.postJson("UserRoles", { userId: this.state.userId, roleId: this.state.role.id });
        await this.getRoleData();

        this.setState({ editing: false, userId: "" });
    }
}