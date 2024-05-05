import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';

export default class Roles extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();
        this.state = { roles: [], loading: true, addRole: { name: "", showDialog: false } };
    }


    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.populateRolesData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderRolesTable(this.state.roles);

        return (
            <div>
                <h1 id="tableLabel">Roles</h1>
                <p><Button variant="primary" onClick={(_) => this.showAddRoleDialog()}>Add Role</Button>Showing your roles</p>
                {contents}
                {this.renderAddRoleDialog()}
            </div>
        );
    }

    renderRolesTable(roles) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Users</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {roles.map(role =>
                  <tr key={role.Id}>
                    <td>{role.Name}</td>
                    <td>{Roles.getRoleUsers(role.UserRoles)}</td>
                    <td>{role.Deleting ? <p>Deleting</p> : <Button variant="danger" onClick={(_) => this.deleteRoleId(role.Id)}>Delete</Button>}</td>
                  </tr>
                )}
              </tbody>
            </table>
          );
    }

    static getRoleUsers(userRoles) {
        return userRoles.map(ur => (<div className="badge badge-primary">{ur.UserId}</div>));
    }

    async deleteRoleId(roleId) {
        var roles = this.state.roles;

        var roleEntry = roles.filter(r => r.Id === roleId)[0];

        roleEntry.Deleting = true;

        this.setState({ roles: roles });

        await this.apiService.deleteJson("Roles", { Id: roleId });

        this.populateRolesData();
    }

    async populateRolesData() {
        this.setState({ roles: [], loading: true });

        var roleData = await this.apiService.get("Roles?$expand=UserRoles($orderby=UserId)");

        this.setState({ roles: roleData, loading: false });
    }

    renderAddRoleDialog() {
        if (!this.state.addRole.showDialog) {
            return;
        }

        return (
              <Modal show={this.state.addRole.showDialog}>
                <Modal.Header closeButton>
                  <Modal.Title>Add role</Modal.Title>
                </Modal.Header>
                {this.renderAddRoleDialogBody()}
                {this.renderAddRoleDialogFooter()}
                
              </Modal>
          );
    }

    renderAddRoleDialogBody() {
        if (this.state.addRole.adding) {
            return;
        }

        return (
            <Modal.Body>
                <Form.Group className="mb-3">
                    <Form.Label>Role Name</Form.Label>
                    <Form.Control type="text" placeholder="Role name here" value={this.state.addRole.name} onChange={(e) => this.setState({ addRole: { name: e.target.value, showDialog: true } })} />
                </Form.Group>
            </Modal.Body>
        );
    }

    renderAddRoleDialogFooter() {
        if (!this.state.addRole.adding) {
            return (
                <Modal.Footer>
                  <Button variant="secondary">Close</Button>
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

    showAddRoleDialog() {
        this.setState({ addRole: { name: "", showDialog: true, adding: false } });
    }

    async addRole() {
        this.setState({ addRole: { name: this.state.addRole.name, showDialog: true, adding: true } });

        await this.apiService.postJson("Roles", { Name: this.state.addRole.name });
        
        this.setState({ addRole: { name: "", showDialog: false, adding: false } });
        this.populateRolesData();
    }
}