import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Container, Row, Col } from 'reactstrap';

export class HowItWorks extends Component {
    constructor(props) {
        super(props)
    }

    componentDidMount() {

    }

    render() {
        return (
            <section className="pb-6">
                <Container className="mt-3 mb-5">
                    <Link to={'/'}>Home</Link> &minus; How it Works

                <div className="my-md-5 my-4">
                        <h1 className="h3">
                            How it Works
                    </h1>
                        <p className="text-muted">
                            Select from many merchant services such as laundry, alterations and more. <br />
                                    Enter pick up and delivery information. <strong>It's that easy.</strong>
                        </p>
                    </div>
                    <div className="py-4">
                        <h2 className="mb-3 text-primary h4">Service</h2>
                        <Row>
                            <Col md="6">
                                <h5>You Need a Laundry Service</h5>
                                <p className="text-muted mb-4">Laundry errands can be a hassle.
                                    Our pick up and delivery services make it easy.</p>
                            </Col>
                            <Col md="6">
                                <h5>Merchants Provide Services</h5>
                                <p className="text-muted mb-4">
                                    Dry cleaning, alterations, suit cleanings and more.
                                    </p>
                            </Col>
                            <Col md="6">
                                <h5>We Bring the Service to You</h5>
                                <p className="text-muted mb-4">
                                    Schedule a pick up and delivery that works for your schedule. We will make it happen.
                                    </p>
                            </Col>
                        </Row>
                    </div>
                </Container>
            </section>
            )
    }
}