import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';

export default class AddCategoryDialog extends Component {
    constructor(props) {
        super(props);
        this.apiService = new ApiService();
        this.state = {
            name: "",
            description: "",
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
                  <Modal.Title>Add category</Modal.Title>
                </Modal.Header>
                {this.renderAddCategoryDialogBody()}
                {this.renderAddCategoryDialogFooter()}
                
              </Modal>
          );
    }

    renderAddCategoryDialogBody() {
        if (this.state.adding) {
            return;
        }

        return (
            <Modal.Body>
                <Form.Group className="mb-3">
                    <Form.Label>Category Name</Form.Label>
                    <Form.Control type="text" placeholder="Category name here" value={this.state.name} onChange={(e) => this.setState({ name: e.target.value })} />
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Category Description</Form.Label>
                    <Form.Control type="text" placeholder="Category name here" value={this.state.description} onChange={(e) => this.setState({ description: e.target.value })} />
                </Form.Group>
            </Modal.Body>
        );
    }

    renderAddCategoryDialogFooter() {
        if (!this.state.adding) {
            return (
                <Modal.Footer>
                  <Button variant="secondary" onClick={(_) => this.props.onClose()}>Close</Button>
                  <Button variant="primary" onClick={(_) => this.addCategory()}>Add</Button>
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

    async createCategoryRoleIfNotExists() {
        await this.apiService.isLoggedIn();

        let userCategoryRole = await this.apiService.get("Roles?$filter=Name eq '" + this.apiService.userId + " Categories'");

        if (userCategoryRole.length === 0) {
            let roleName = this.apiService.userId + " Categories";

            var role = await this.apiService.postJson("Roles", { Name: roleName });
            return role.id;
        } else {
            return userCategoryRole[0].id;
        }

    }


    async addCategory() {
        this.setState({ adding: true });

        let roleId = await this.createCategoryRoleIfNotExists();

        await this.apiService.postJson("Categories", { Name: this.state.name, Description: this.state.description, RoleId: roleId });
        
        this.setState({ name: "", description: "", adding: false });
        this.props.onSubmit();
        this.props.onClose();
    }
}