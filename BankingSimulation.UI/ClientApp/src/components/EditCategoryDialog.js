import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { InputGroup } from 'react-bootstrap';

export default class EditCategoryDialog extends Component {
    constructor(props) {
        super(props);
        this.apiService = new ApiService();
        this.state = {
            category: null,
            keyword: "",
            editing: false
        };
    }

    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.getCategoryData();
    }

    async getCategoryData() {
        this.setState({ category: null });

        var categoryId = this.props.categoryId;

        var categoryEntry = await this.apiService.get("Categories?$filter=Id eq " + categoryId + "&$expand=Keywords");

        if (categoryEntry.length === 1) {
            this.setState({ category: categoryEntry[0] });
        }
    }

    render() {
        if (this.state.category === null) {
            return;
        }

        return (
              <Modal show={true}>
                <Modal.Header>
                  <Modal.Title>{this.state.category.name}</Modal.Title>
                </Modal.Header>
                {this.renderEditCategoryDialogBody()}
                {this.renderEditCategoryDialogFooter()}
                
              </Modal>
          );
    }


    renderEditCategoryDialogBody() {
        if (this.state.editing) {
            return;
        }

        return (
            <Modal.Body>
                <InputGroup>
                    <Form.Control 
                        type="text" 
                        placeholder="Category name here" 
                        value={this.state.category.name} 
                        onChange={(e) => this.changeCategoryName(e.target.value)} 
                        />
                    <Button variant="primary" onClick={(_) => this.updateCategoryHeader()} className="float-end">Update Name</Button>
                </InputGroup>

                <InputGroup>
                    <Form.Control 
                        type="text" 
                        placeholder="Category name here" 
                        value={this.state.category.description} 
                        onChange={(e) => this.changeCategoryDescription(e.target.value)} 
                        />
                    <Button variant="primary" onClick={(_) => this.updateCategoryHeader()} className="float-end">Update Description</Button>
                </InputGroup>

                <InputGroup style={{ marginTop: "5px" }} >
                    <Form.Control 
                        type="text" 
                        placeholder="Keyword" 
                        value={this.state.keyword} 
                        onChange={(e) => this.setState({ keyword: e.target.value })} 
                    />
                    <Button variant="secondary" onClick={(_) => this.addKeyword()} className="float-end">Add Keyword</Button>
                </InputGroup>

                {this.renderEditCategoryDialogKeywords(this.state.category.keywords)}
            </Modal.Body>
        );
    }


    changeCategoryName(name) {
        let categoryCopy = JSON.parse(JSON.stringify(this.state.category));

        categoryCopy.name = name;

        this.setState({ category: categoryCopy });
    }

    async updateCategoryHeader() {
        this.setState({ editing: true });

        await this.apiService.putJson("Categories", this.state.category);
        
        this.setState({ editing: false });
        this.props.onSubmit();
    }

    changeCategoryDescription(description) {
        let categoryCopy = JSON.parse(JSON.stringify(this.state.category));

        categoryCopy.description = description;

        this.setState({ category: categoryCopy });
    }

    renderEditCategoryDialogKeywords(keywords) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
              <thead>
                <tr>
                  <th>Name</th>
                  <th style={{ textAlign: "right" }}>Actions</th>
                </tr>
              </thead>
              <tbody>
                {keywords.map(k =>
                  <tr key={k.id}>
                    <td>{k.keyword}</td>
                    <td style={{ textAlign: "right" }}>{k.deleting ? <p>Deleting</p> : <Button variant="danger" onClick={(_) => this.deleteKeyword(k.id)}>Delete</Button>}</td>
                  </tr>
                )}
              </tbody>
            </table>
        );
    }

    async deleteKeyword(keywordId) {
        this.setState({ editing: true });

        await this.apiService.deleteJson("CategoryKeywords", { id: keywordId });
        
        await this.getCategoryData();
        
        this.setState({ editing: false });
    }

    renderEditCategoryDialogFooter() {
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

    async addKeyword() {
        this.setState({ editing: true });

        await this.apiService.postJson("CategoryKeywords", { keyword: this.state.keyword, categoryId: this.state.category.id });
        await this.getCategoryData();

        this.setState({ editing: false, userId: "" });
    }
}