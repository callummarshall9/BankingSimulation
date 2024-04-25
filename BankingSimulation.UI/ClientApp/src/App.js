import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import Login from './components/Login'
import './custom.css';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = {
            loggedIn: false
        };
    }

    render() {
        if (this.state.loggedIn) {
            return (
                <Layout>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} />;
                        })};
                    </Routes>
                </Layout>
            )
        } else {
            return <Login onLoggedIn={() => this.setState({ loggedIn: true })} />
        }
    }
}
