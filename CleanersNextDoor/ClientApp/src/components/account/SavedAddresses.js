import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from './../../context/AuthContext'
import { Row, Col, Container, Badge, Card, CardBody, CardFooter } from 'reactstrap'


export class SavedAddresses extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            customer: null
        }
    }

    componentDidMount() {
        this.populateAddressInformation()
    }

    populateAddressInformation() {
        fetch(`customers/getAddresses`)
            .then(response => response.json())
            .then(data => {
                this.setState({
                    addresses: data,
                    loading: false
                })
            })
    }

    removeAddress = event => {
        if (window.confirm(`Are you sure you want to delete address ${event.target.value}?`)) {

            const request = {
                method: 'post',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ id: Number(event.target.value) })
            }

            fetch(`customers/removeAddress`, request)
                .then(reponse => reponse.json())
                .then(data => data ? this.populateAddressInformation() : console.log(data))
        }
    }

    checkHandler = event => {
        const request = {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: Number(event.target.value) })
        }

        fetch(`customers/setDefaultAddress`, request)
            .then(reponse => reponse.json())
            .then(data => data ? this.populateAddressInformation() : console.log(data))
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated }) => (
                        <div>
                            {!authenticated
                                ? <Redirect to='/' />
                                : this.renderLayout()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }

    renderLayout() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderAddresses(this.state.addresses);
        return (
            <div>
                <header className="bg-primary py-3 mb-5">
                    <Container className="h-100">
                        <Row className="h-100 align-items-center">
                            <Col>
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    Saved Addresses
                                </h1>
                                <p className="lead text-white-50">
                                    Set up your default saved addresses for speedy pick up and delivery.
                                </p>
                                <Link to="/new-address" className="btn btn-success btn-lg mr-2">
                                    New Address
                                </Link>
                                <Link to="/account" className="btn btn-secondary btn-lg">
                                    My Account
                                </Link>
                            </Col>
                        </Row>
                    </Container>
                </header>
                {contents}
            </div>
        )
    }

    renderAddresses(addresses) {
        return (
            <div>
                <Container>
                    <h3>
                        My Addresses
                    </h3>
                    <p>
                        Address information is used for scheduling pick up and delivery services.
                    </p>

                    <Row hidden={addresses.length === 0}>
                        {addresses.map(a =>
                            <Col md="4" key={a.id} className="mb-4">
                                <Card className={'h-100 ' + (a.isDefault ? 'border-primary' : '')}>
                                    <CardBody className="pb-0 border-bottom-0">
                                        <Row>
                                            <Col>
                                                <p className="mb-1 font-weight-bold">
                                                    {a.street1} {a.street2}
                                                </p>
                                                <small className="d-block text-muted">{a.city}, {a.stateAbbreviation}. {a.zip}</small>
                                                <Badge hidden={a.name.length === 0} color="light" className="border" pill>{a.name.toUpperCase()}</Badge>
                                            </Col>
                                            <Col xs="auto">
                                                <div className="custom-control custom-radio">
                                                    <input type="radio" id={'isDefault' + a.id} name="isDefault" className="custom-control-input" value={a.id} id={'isDefault' + a.id} checked={a.isDefault} onChange={this.checkHandler} />
                                                    <label className={'custom-control-label ' + (a.isDefault ? 'font-weight-bold' : '')} htmlFor={'isDefault' + a.id}>DEFAULT</label>
                                                </div>
                                            </Col>
                                        </Row>
                                    </CardBody>
                                    <CardFooter className="bg-white pt-0 border-top-0">
                                        <button type="button" className="btn btn-link btn-sm text-danger pl-0" value={a.id} onClick={this.removeAddress}>
                                            REMOVE
                                        </button>
                                    </CardFooter>
                                </Card>
                            </Col>
                        )}
                    </Row>
                    <div hidden={addresses.length > 0} className="mb-4">
                        <p className="lead mb-1">
                            You have no saved addresses.
                        </p>
                        <small className="text-muted">
                            <Link to='/new-address' className="text-decoration-none">
                                Add a new address&nbsp;
                            </Link>
                            to speed up the checkout process.
                        </small>
                    </div>
                </Container>
            </div>
            )
    }
}