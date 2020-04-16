import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from '../context/AuthContext'
import { Row, Col, Container, Badge, Card, CardBody, CardFooter, CardHeader } from 'reactstrap'


export class SavedAddresses extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: true,
            customer: null,
            missingAddress: true,
        }
    }

    componentDidMount() {
        this.populateAddressInformation()
    }

    populateAddressInformation() {
        fetch(`customers/profile`)
            .then(response => response.json())
            .then(data => {
                if (data.id > 0) {
                    this.setState({
                        customer: data,
                        loading: false,
                        missingAddress: data.addresses && data.addresses.length === 0
                    })
                }
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
            : this.renderAddresses(this.state.customer.addresses);
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
                                    Set up your default saved addresses for Speedy pick up and delivery.
                                </p>
                                <Link to="/new-address" className="btn btn-success btn-lg mr-2">
                                    New Address
                                </Link>
                                <Link to="/profile" className="btn btn-secondary btn-lg">
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

                    <h3 className="border-bottom">
                        My Addresses
                    </h3>
                    <p>
                        Address information is used for scheduling pick up and delivery services.
                    </p>

                    <Row hidden={addresses.length === 0}>
                        {addresses.map(a =>
                            <Col md="4" key={a.id} className="mb-4">
                                <Card className={'h-100 ' + (a.isDefault ? 'border-primary' : '')}>
                                    <CardHeader className="bg-primary text-white card-header text-center py-0 lead" hidden={!a.isDefault}>
                                        DEFAULT
                                    </CardHeader>
                                    <CardBody>
                                        <p className="mb-1 font-weight-bold">
                                            {a.street1} {a.street2}
                                            <Badge className="float-right" hidden={a.name.length === 0} color="dark" pill>{a.name.toUpperCase()}</Badge>
                                        </p>
                                        <small className="d-block text-muted">{a.city}, {a.stateAbbreviation}. {a.zip}</small>                                    </CardBody>
                                    <CardFooter>
                                        <Link className="btn btn-link btn-sm" to={'/edit-address/:id'.replace(':id', a.id)}>
                                            Edit
                                        </Link>
                                        <button type="button" className="btn btn-link btn-sm text-danger" value={a.id} onClick={this.removeAddress}>
                                            Remove
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