import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from './../../context/AuthContext'
import { Row, Col, Container, Badge, Card, CardBody } from 'reactstrap'
import { customerService } from '../../services/customer.service'
import Loading from '../../helpers/Loading';
import { FaPlusCircle, FaTimesCircle } from 'react-icons/fa';


export class SavedAddresses extends Component {

    constructor(props) {
        super(props);
        this.state = {
            addresses: null
        }
    }

    componentDidMount() {
        this.populateAddressInformation()
    }

    async populateAddressInformation() {
        const data = await customerService.getAddresses()
        this.setState({
            addresses: data
        })
    }

    removeAddress = event => {
        if (window.confirm(`Are you sure you want to delete address ${event.target.value}?`)) {
            customerService.removeAddress({ id: Number(event.target.value) })
                .then(data => data ? this.populateAddressInformation() : console.log(data))
        }
    }

    checkHandler = event => {
        customerService.setDefaultAddress({ id: Number(event.target.value) })
            .then(data => data ? this.populateAddressInformation() : console.log(data))
    }

    render() {
        return (
            <AuthConsumer>
                {({ authenticated }) => (
                    <div>
                        {!authenticated
                            ? <Redirect to='/' />
                            : SavedAddresses.renderAddresses(this.state.addresses, this.checkHandler, this.removeAddress)}
                    </div>
                )}
            </AuthConsumer>
        )
    }

    static renderAddresses(addresses, checkHandler, removeAddress) {
        return (
            <Container className="mt-3 mb-5">
                <Link to={'/'}>Home</Link>&nbsp;&minus;&nbsp;
                <Link to={'/account'}>Account</Link>&nbsp;&minus;&nbsp;Your saved addresses
                <div className="my-md-5 my-4">
                    <h1 className="h3">
                        Saved addresses
                            </h1>
                    <p className="text-muted">Address information is used for scheduling pick up and delivery services.</p>
                </div>
                {addresses === null
                    ? <Loading message="Loading addresses, please wait" />
                    : <div>
                        <Row>
                            {addresses.map(a =>
                                <Col md="4" key={a.id} className="mb-4">
                                    <Card className="h-100">
                                        <CardBody>
                                            <div className="custom-control custom-radio">
                                                <input type="radio" id={'isDefault' + a.id} name="isDefault" className="custom-control-input" value={a.id} id={'isDefault' + a.id} checked={a.isDefault} onChange={checkHandler} />
                                                <label className={'custom-control-label'} htmlFor={'isDefault' + a.id}>
                                                    <span className="sr-only">Default Address</span>
                                                    <h5 className={'mb-0 '.concat(a.isDefault ? 'text-primary' : '')}>
                                                        {a.street1} {a.street2}
                                                    </h5>
                                                    <small className="d-block text-muted">{a.city}, {a.stateAbbreviation}. {a.zip}</small>
                                                </label>
                                            </div>
                                            <div className="my-2" hidden={true}>
                                                <Badge hidden={a.name.length === 0} color="light" className="border" pill>{a.name.toUpperCase()}</Badge>
                                            </div>
                                            <div className="mt-2">
                                                <button type="button" className="btn btn-link btn-sm pl-0 text-danger" value={a.id} onClick={removeAddress}>
                                                    <FaTimesCircle /> REMOVE
                                                </button>
                                            </div>
                                        </CardBody>
                                    </Card>
                                </Col>
                            )}
                            <Col md="4" className="mb-4">
                                <Link to='/new-address' className="text-decoration-none">
                                    <Card className="h-100">
                                        <CardBody>
                                            <span className="sr-only" hidden={addresses.length > 0}>
                                                You have no saved addresses.
                                            </span>
                                            <h5 className={'mb-1 '}>
                                                <FaPlusCircle /> New address
                                            </h5>
                                            <small className={'d-block '.concat(addresses.length === 0 ? 'text-danger' : 'text-muted')}>Add a new address</small>
                                        </CardBody>
                                    </Card>
                                </Link>
                            </Col>
                        </Row>
                    </div>
                }
            </Container>
        )
    }
}