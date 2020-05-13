import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { Container, Row, Col, FormGroup, Card, CardHeader, CardBody } from 'reactstrap';
import { FaLock, FaAddressCard } from 'react-icons/fa';
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
        if (event.target instanceof HTMLInputElement && event.target.getAttribute('type') == 'checkbox') {
            const value = event.target.checked;
            this.setState({
                [name]: value
            });
        }
        else {
            const value = event.target.value;
            this.setState(handleChange(name, value, this.state.formControls));
        }
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
                    if (data === true) {
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
            <AuthConsumer>
                {({ authenticated }) => (
                    <div>
                        {!authenticated
                            ? <Redirect to='/sign-in' />
                            : NewAddress.renderCreateAddressForm(this.state, this.changeHandler, this.submitHandler)}
                    </div>
                )}
            </AuthConsumer>
        )
    }

    static renderCreateAddressForm(state, changeHandler, submitHandler) {
        const { formControls, isDefault, formIsValid } = state

        return (
            <Container className="mt-3 mb-5">
                <Link to={'/'}>Home</Link>&nbsp;&minus;&nbsp;
                <Link to={'/account'}>Account</Link>&nbsp;&minus;&nbsp;
                <Link to={'/saved-addresses'}>Saved addresses</Link>&nbsp;&minus;&nbsp;New address
                <div className="my-md-5 my-4">
                    <h1 className="h3">
                        New address
                    </h1>
                    <p className="text-muted">
                        Please enter an address you would like saved on your profile for scheduling pick up and delivery services.
                    </p>
                </div>

                <Row>
                    <Col sm="7" md="6" lg="6">
                        <form method="post" onSubmit={submitHandler}>

                            <TextInput name="street1"
                                placeholder={formControls.street1.placeholder}
                                label={formControls.street1.label}
                                value={formControls.street1.value}
                                onChange={changeHandler}
                                touched={formControls.street1.touched ? 1 : 0}
                                valid={formControls.street1.valid ? 1 : 0}
                                errors={formControls.street1.errors} />
                            <TextInput name="street2"
                                placeholder={formControls.street2.placeholder}
                                label={formControls.street2.label}
                                value={formControls.street2.value}
                                onChange={changeHandler}
                                touched={formControls.street2.touched ? 1 : 0}
                                valid={formControls.street2.valid ? 1 : 0}
                                errors={formControls.street2.errors} />
                            <TextInput name="city"
                                placeholder={formControls.city.placeholder}
                                label={formControls.city.label}
                                value={formControls.city.value}
                                onChange={changeHandler}
                                touched={formControls.city.touched ? 1 : 0}
                                valid={formControls.city.valid ? 1 : 0}
                                errors={formControls.city.errors} />
                            <Row>
                                <Col md="8">
                                    <FormGroup>
                                        <label className="font-weight-bold">State</label>
                                        <select className="form-control" name="stateAbbreviation" value={formControls.stateAbbreviation.value} onChange={changeHandler}>
                                            <option value="">Select One</option>
                                            <option value="FL">Florida</option>
                                            <option value="NY">New York</option>
                                            <option value="CA">California</option>
                                        </select>
                                    </FormGroup>
                                </Col>
                                <Col md="4">
                                    <TextInput name="zip"
                                        placeholder={formControls.zip.placeholder}
                                        label={formControls.zip.label}
                                        value={formControls.zip.value}
                                        onChange={changeHandler}
                                        touched={formControls.zip.touched ? 1 : 0}
                                        valid={formControls.zip.valid ? 1 : 0}
                                        errors={formControls.zip.errors} />
                                </Col>
                            </Row>

                            <div className="form-group">
                                <div className="custom-control custom-checkbox">
                                    <input type="checkbox" className="custom-control-input" id="isDefault" name="isDefault" onChange={changeHandler} checked={isDefault} />
                                    <label className="custom-control-label" htmlFor="isDefault">
                                        Make this my default address
                                        </label>
                                </div>
                            </div>

                            <TextInput name="name"
                                placeholder={formControls.name.placeholder}
                                label={formControls.name.label}
                                value={formControls.name.value}
                                onChange={changeHandler}
                                touched={formControls.name.touched ? 1 : 0}
                                valid={formControls.name.valid ? 1 : 0}
                                errors={formControls.name.errors} />


                            <button className="btn btn-primary btn-block mb-4" type="submit" disabled={!formIsValid}>
                                Save new address
                            </button>
                        </form>
                    </Col>
                    <Col sm="5" md="4" lg="4" className="ml-lg-auto">
                        <Card className="h-100 border-0 shadow">
                            <CardHeader className="bg-primary text-white border-bottom-0">
                                <Row>
                                    <Col><h5>Why do you need this info?</h5></Col>
                                    <Col xs="auto"><FaAddressCard /></Col>
                                </Row>
                            </CardHeader>
                            <CardBody>
                                <p>
                                    Saving default addresses is a completely optional feature of our system. 
                                     
                                </p>
                                <p className="font-weight-bold">
                                    Your data is never shared with third parties.
                                </p>
                                <p>
                                    The goal with default saved addresses is to make checkout as few fields as possible.
                                </p>
                                <p>
                                    Another great benefit of saving your default address is to make it easier to setup automatic subscriptions.
                                </p>
                            </CardBody>
                        </Card>
                    </Col>
                </Row>
            </Container>
            )
    }

}