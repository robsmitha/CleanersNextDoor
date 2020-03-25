import React, { Component } from 'react';
import { Authentication } from '../../services/authentication'

export class Profile extends Component {
    constructor(props) {
        super(props)
        this.state = {
            user: null,
            loading: true
        }
    }

    componentDidMount() {
        this.populateProfileInformation()
    }
    static renderProfile(user) {
        return (
            <div>
                {user.username}
            </div>
            )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Profile.renderProfile(this.state.user);
        return (
            <div className="container">
                Profile
                {contents}
            </div>
            )
    }

    async populateProfileInformation() {
        const userId = Authentication.getUserId()
        const response = await fetch(`users/${userId}`);
        const data = await response.json();
        this.setState({ user: data, loading: false });
    }
}