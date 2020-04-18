import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { Container, Row, Col, FormGroup } from 'reactstrap';
import { FaLock } from 'react-icons/fa';
import { AuthConsumer } from './../../context/AuthContext'
import TextInput from './../../helpers/TextInput';
import handleChange from './../../helpers/HandleChange';
import { customerService } from '../../services/customer.service'


export class NewAddress extends Component {

    constructor(props) {
        super(props);
        this.state = {
            formIsValid: false,
            isDefault: false,
            formControls: {
                street1: {
                    value: '',
                    placeholder: 'Street 1',
                    label: 'Street address',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                street2: {
                    value: '',
                    placeholder: 'Street 2',
                    label: 'Apt / suite (optional)',
                    valid: true,
                    touched: false,
                    errors: []
                },
                city: {
                    value: '',
                    placeholder: 'City',
                    label: 'City',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                stateAbbreviation: {
                    value: '',
                    placeholder: 'State',
                    label: 'State',
                    valid: true,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 2
                    },
                    errors: []
                },
                zip: {
                    value: '',
                    placeholder: 'Zip',
                    label: 'Zip',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 5
                    },
                    errors: []
                },
                name: {
                    value: '',
                    placeholder: 'Address name',
                    label: 'Address name (optional)',
                    valid: true
                }
            }
        };
    }
    changeHandler = event => {
        const name = event.target.name;
        const value = event.target.value;
        this.setState(handleChange(name, value, this.state.formControls));
    }

    checkHandler = event => {
        if (event.target instanceof HTMLInputElement && event.target.getAttribute('type') == 'checkbox') {

        }
        const name = event.target.name;
        const value = event.target.checked;
        this.setState({
            [name]: value
        });
    }

    submitHandler = (event) => {
        event.preventDefault();
        this.setState({
            formIsValid: false
        });
        const {
            street1,
            street2,
            city,
            stateAbbreviation,
            zip,
            name
        } = this.state.formControls;

        let address = {
            street1: street1.value,
            street2: street2.value,
            city: city.value,
            stateAbbreviation: stateAbbreviation.value,
            zip: zip.value,
            name: name.value,
            isDefault: this.state.isDefault
        }

        customerService.addAddress(address)
            .then(data => {
                if (data !== null) {
                    if (data.id > 0) {
                        this.props.history.push(`/saved-addresses`)
                    } else {
                        alert(data)
                    }
                }
                else {
                    //request failed
                }

                this.setState({
                    formIsValid: true
                });
            })
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ authenticated }) => (
                        <div>
                            {!authenticated
                                ? <Redirect to='/sign-in' />
                                : this.renderContent()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }
    renderContent() {
        return (
            <div>
                <header className="bg-primary py-3 mb-5 shadow">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-lg-12">
                                <h1 className="display-4 text-white mt-5 mb-2">
                                    Create New Address
                                </h1>
                                <p className="lead text-white-50">
                                    Create a new address by entering your address information below.
                                </p>
                                <Link to="/account" className="btn btn-success btn-lg mr-2">My Account</Link>
                                <Link to="/saved-addresses" className="btn btn-secondary btn-lg">Saved Addresses</Link>
                            </div>
                        </div>
                    </div>
                </header>
                <Container>

                    <h3>
                        Address Details
                    </h3>
                    <p className="mb-1">
                        Please enter an address you would like saved on your profile.
                    </p>

                    <Row>
                        <Col sm="9" md="7" lg="5">
                            <form method="post" onSubmit={this.submitHandler}>

                                <TextInput name="street1"
                                    placeholder={this.state.formControls.street1.placeholder}
                                    label={this.state.formControls.street1.label}
                                    value={this.state.formControls.street1.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.street1.touched ? 1 : 0}
                                    valid={this.state.formControls.street1.valid ? 1 : 0}
                                    errors={this.state.formControls.street1.errors} />
                                <TextInput name="street2"
                                    placeholder={this.state.formControls.street2.placeholder}
                                    label={this.state.formControls.street2.label}
                                    value={this.state.formControls.street2.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.street2.touched ? 1 : 0}
                                    valid={this.state.formControls.street2.valid ? 1 : 0}
                                    errors={this.state.formControls.street2.errors} />
                                <TextInput name="city"
                                    placeholder={this.state.formControls.city.placeholder}
                                    label={this.state.formControls.city.label}
                                    value={this.state.formControls.city.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.city.touched ? 1 : 0}
                                    valid={this.state.formControls.city.valid ? 1 : 0}
                                    errors={this.state.formControls.city.errors} />
                                <Row>
                                    <Col md="8">
                                        <FormGroup>
                                            <label className="font-weight-bold">State</label>
                                            <select className="form-control" name="stateAbbreviation" value={this.state.formControls.stateAbbreviation.value} onChange={this.changeHandler}>
                                                <option value="">Select One</option>
                                                <option value="FL">Florida</option>
                                                <option value="NY">New York</option>
                                                <option value="CA">California</option>
                                            </select>
                                        </FormGroup>
                                    </Col>
                                    <Col md="4">
                                        <TextInput name="zip"
                                            placeholder={this.state.formControls.zip.placeholder}
                                            label={this.state.formControls.zip.label}
                                            value={this.state.formControls.zip.value}
                                            onChange={this.changeHandler}
                                            touched={this.state.formControls.zip.touched ? 1 : 0}
                                            valid={this.state.formControls.zip.valid ? 1 : 0}
                                            errors={this.state.formControls.zip.errors} />
                                    </Col>
                                </Row>

                                <div className="form-group">
                                    <div className="custom-control custom-checkbox">
                                        <input type="checkbox" className="custom-control-input" id="isDefault" name="isDefault" onChange={this.checkHandler} checked={this.state.isDefault} />
                                        <label className="custom-control-label" htmlFor="isDefault">
                                            Make this my default address
                                        </label>
                                    </div>
                                </div>

                                <TextInput name="name"
                                    placeholder={this.state.formControls.name.placeholder}
                                    label={this.state.formControls.name.label}
                                    value={this.state.formControls.name.value}
                                    onChange={this.changeHandler}
                                    touched={this.state.formControls.name.touched ? 1 : 0}
                                    valid={this.state.formControls.name.valid ? 1 : 0}
                                    errors={this.state.formControls.name.errors} />


                                <button className="btn btn-primary btn-block mb-3" type="submit" disabled={!this.state.formIsValid}>
                                    Save new address
                                </button>
                            </form>
                        </Col>
                    </Row>
                    <small className="text-muted d-block mb-4">
                        <FaLock /> Your data is never shared with third parties.
                    </small>
                </Container>
            </div>
        )
    }
}