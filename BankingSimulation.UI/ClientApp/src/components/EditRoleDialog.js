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
            role: null,
            editing: false
        };
    }

    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.getRoleData();
    }

    async getRoleData() {
        this.setState({ role: null });

        var roleId = this.props.roleId;

        var roleEntry = await this.apiService.get("Roles?$filter=Id eq " + roleId + "&$expand=UserRoles");

        if (roleEntry.length === 1) {
            this.setState({ role: roleEntry[0] });
        }
    }

    render() {
        if (this.state.role === null) {
            return;
        }

        return (
              <Modal show={true}>
                <Modal.Header>
                  <Modal.Title>{this.state.role.Name}</Modal.Title>
                </Modal.Header>
                {this.renderEditRoleDialogBody()}
                {this.renderEditRoleDialogFooter()}
                
              </Modal>
          );
    }

    changeRole(name) {
        let roleCopy = JSON.parse(JSON.stringify(this.state.role));

        roleCopy.Name = name;

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
                        value={this.state.role.Name} 
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

                {this.renderEditRoleDialogUsers(this.state.role.UserRoles)}
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
                  <tr key={user.UserId}>
                    <td>{user.UserId}</td>
                    <td style={{ textAlign: "right" }}>{user.Deleting ? <p>Deleting</p> : <Button variant="danger" onClick={(_) => this.deleteUserRole(user.UserId)}>Delete</Button>}</td>
                  </tr>
                )}
              </tbody>
            </table>
        );
    }

    async deleteUserRole(userId) {
        this.setState({ editing: true });

        await this.apiService.deleteJson("UserRoles", { UserId: userId, RoleId: this.state.role.Id });
        
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

        await this.apiService.postJson("UserRoles", { UserId: this.state.userId, RoleId: this.state.role.Id });
        await this.getRoleData();

        this.setState({ editing: false, userId: "" });
    }
}