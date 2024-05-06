import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import AddRoleDialog from './AddRoleDialog';
import EditRoleDialog from './EditRoleDialog';

export default class Roles extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();
        this.state = { roles: [], loading: true, showAddRoleDialog: false, editRoleDialogId: null };
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
                {this.state.showAddRoleDialog ? <AddRoleDialog 
                    onClose={(_) => this.setState({ showAddRoleDialog: false })} 
                    onSubmit={(_) => this.populateRolesData()}
                    /> : null}
                {this.state.editRoleDialogId ? <EditRoleDialog
                    roleId={this.state.editRoleDialogId}
                    onClose={(_) => this.setState({ editRoleDialogId: null })}
                    onSubmit={(_) => this.populateRolesData()} 
                    /> : null}
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
                    <td>
                        <Button 
                          variant="secondary" 
                          onClick={(_) => this.editRoleId(role.Id)}
                        >Edit</Button>
                        {
                        role.Deleting 
                          ? <p>Deleting</p> 
                          : <Button 
                              variant="danger" 
                              onClick={(_) => this.deleteRoleId(role.Id)}
                            >Delete</Button>
                          }
                      </td>

                  </tr>
                )}
              </tbody>
            </table>
          );
    }

    static getRoleUsers(userRoles) {
        return userRoles.map(ur => (<div key={ur.UserId} className="badge badge-primary">{ur.UserId}</div>));
    }

    async editRoleId(roleId) {
        this.setState({ editRoleDialogId: roleId });
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

    showAddRoleDialog() {
        this.setState({ showAddRoleDialog: true });
    }

}