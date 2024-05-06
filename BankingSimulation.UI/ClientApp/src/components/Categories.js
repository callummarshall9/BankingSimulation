import React, { Component } from 'react';
import ApiService from '../services/ApiService';

import Button from 'react-bootstrap/Button';
import AddCategoryDialog from './AddCategoryDialog';
import EditCategoryDialog from './EditCategoryDialog';

export default class Categories extends Component {
    constructor(props) {
        super(props);

        this.apiService = new ApiService();
        this.state = { categories: [], loading: true, showAddCategoryDialog: false, editCategoryDialogId: null };
    }


    componentDidMount() {
        this.apiService.getAuthorisationToken();
        this.populateCategoriesData();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderCategoriesTable(this.state.categories);

        return (
            <div>
                <h1 id="tableLabel">Categories</h1>
                <p><Button variant="primary" onClick={(_) => this.showAddCategoryDialog()}>Add Category</Button>Showing your categories</p>
                {contents}
                {this.state.showAddCategoryDialog ? <AddCategoryDialog 
                    onClose={(_) => this.setState({ showAddCategoryDialog: false })} 
                    onSubmit={(_) => this.populateCategoriesData()}
                    /> : null}
                {this.state.editCategoryDialogId ? <EditCategoryDialog
                    categoryId={this.state.editCategoryDialogId}
                    onClose={(_) => this.setState({ editCategoryDialogId: null })}
                    onSubmit={(_) => this.populateCategoriesData()} 
                    /> : null}
            </div>
        );
    }

    renderCategoriesTable(categories) {
        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Description</th>
                  <th>Keywords</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {categories.map(category =>
                  <tr key={category.id}>
                    <td>{category.name}</td>
                    <td>{category.description}</td>
                    <td>{Categories.getCategoryKeywords(category.keywords)}</td>
                    <td>
                        <Button 
                          variant="secondary" 
                          onClick={(_) => this.editCategoryId(category.id)}
                        >Edit</Button>
                        {
                        category.Deleting 
                          ? <p>Deleting</p> 
                          : <Button 
                              variant="danger" 
                              onClick={(_) => this.deleteCategoryId(category.id)}
                            >Delete</Button>
                          }
                      </td>

                  </tr>
                )}
              </tbody>
            </table>
          );
    }

    static getCategoryKeywords(keywords) {
        return keywords.map(k => (<div key={k.id} className="badge badge-primary">{k.Keyword}</div>));
    }

    async editCategoryId(categoryId) {
        this.setState({ editCategoryDialogId: categoryId });
    }

    async deleteCategoryId(categoryId) {
        var categories = this.state.categories;

        var categoryEntry = categories.filter(r => r.id === categoryId)[0];

        categoryEntry.Deleting = true;

        this.setState({ categories: categories });

        await this.apiService.deleteJson("Categories", { id: categoryId });

        this.populateCategoriesData();
    }

    async populateCategoriesData() {
        this.setState({ categories: [], loading: true });

        var categoryData = await this.apiService.get("Categories?$expand=Keywords($orderby=Keyword)");

        this.setState({ categories: categoryData, loading: false });
    }

    showAddCategoryDialog() {
        this.setState({ showAddCategoryDialog: true });
    }

}