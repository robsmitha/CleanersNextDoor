import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom';
import { AuthConsumer } from './../../context/AuthContext'
import './OrderDetails.css'
import { Container, Row, Col, Card } from 'reactstrap';
import Loading from '../../helpers/Loading';
import { customerService } from '../../services/customer.service';

export class OrderDetails extends Component {
    constructor(props) {
        super(props)
        this.state = {
            id: this.props.match.params.id,
            orderDetails: null
        }
    }

    componentDidMount() {
        this.getOrder()
    }

    getOrder = async() => {
        const data = await customerService.getOrder(this.state.id)
        if (data && data.order.id > 0) {
            this.setState({ orderDetails: data })
        }
        else {
            this.props.history.push('/')
        }
    }

    render() {
        return (
            <AuthConsumer>
                {({ authenticated }) => (
                    <div>
                        {!authenticated
                            ? <Redirect to='/sign-up' />
                            : OrderDetails.renderOrder(this.state)}
                    </div>
                )}
            </AuthConsumer>
        )
    }

    static renderOrder(state) {
        const { orderDetails, id } = state
        return (
            <Container className="mt-3 mb-5">
                <Link to={'/'}>Home</Link> &minus;
                <Link to={'/account'}> Account</Link> &minus;&nbsp;
                <Link to={'/order-history'}> Order history</Link> &minus; Order #{id}

                <div className="my-md-5 my-4">
                    <p className="subtitle">
                        Order #{id}
                    </p>
                    {orderDetails === null
                        ? <Loading message="Loading order, please wait.." />
                        : <div>
                            <h1 className="h3 mb-0">
                                {orderDetails.order.merchantName}
                            </h1>
                            <small className="d-block mb-2">Updated on {orderDetails.order.displayUpdated}</small>
                            <hr />
                            <h6>
                                {orderDetails.order.serviceRequest.workflowName}
                            </h6>
                            <p className="text-muted">
                                {orderDetails.order.serviceRequest.correspondences
                                    .filter(c => c.correspondenceTypeCustomerConfigures === true).length} correspondence steps
                            </p>
                            <Row>
                                {orderDetails.order.serviceRequest.correspondences
                                    .filter(c => c.correspondenceTypeCustomerConfigures === true)
                                    .map(c =>
                                        <Col md="6" key={c.id} className="d-flex align-items-center mb-3 mb-md-0">
                                            <div className="date-tile mr-3">
                                                <div className="text-uppercase">
                                                    <span className="text-sm">
                                                        {c.monthAbbrevation}
                                                    </span>
                                                    <br />
                                                    <strong className="text-lg">
                                                        {c.dayOfMonth}
                                                    </strong>
                                                </div>
                                            </div>
                                            <p className="text-sm mb-0">
                                                <span className="d-block">{c.correspondenceTypeName}</span>
                                                <small className="text-muted d-block">{c.correspondenceTypeDescription}</small>
                                            </p>
                                        </Col>
                                    )}
                            </Row>
                            <hr />
                            <Row>
                                <Col sm="4">
                                    <h6>
                                        Name
                                    </h6>
                                    <p className="text-muted">
                                        {orderDetails.order.serviceRequest.name}
                                    </p>
                                </Col>
                                <Col sm="4">
                                    <h6>
                                        Phone
                                    </h6>
                                    <p className="text-muted">
                                        {orderDetails.order.serviceRequest.phone}
                                    </p>
                                </Col>
                                <Col sm="4">
                                    <h6>
                                        Email
                                    </h6>
                                    <p className="text-muted">
                                        {orderDetails.order.serviceRequest.email}
                                    </p>
                                </Col>
                            </Row>
                            <hr />
                            <Row>
                                <Col sm>
                                    <h6>
                                        {orderDetails.order.serviceRequest.serviceRequestStatusTypeName}
                                    </h6>
                                    <p className="text-muted">{orderDetails.order.serviceRequest.serviceRequestStatusTypeDescription}</p>
                                </Col>
                                <Col sm="auto">
                                    <img className="avatar avatar-lg avatar-border-white ml-4" src={orderDetails.order.merchantDefaultImageUrl} />
                                </Col>
                            </Row>
                            <hr />
                            <Row>
                                <Col sm="6" className="border-sm-right">
                                    <Card className="border-0 h-100 bg-light">
                                        <h6>
                                            {orderDetails.order.lineItems.length} item{orderDetails.order.lineItems.length == 1 ? '' : 's'}
                                        </h6>
                                        <div className="pb-3 text-muted small">
                                            {orderDetails.order.lineItems.map(li =>
                                                <Row key={li.id}>
                                                    <Col xs>
                                                        <span>{li.itemName}</span>
                                                    </Col>
                                                    <Col xs="2">
                                                        x1
                                                    </Col>
                                                    <Col xs="2" className="text-right">
                                                        ${li.itemAmount}
                                                    </Col>
                                                </Row>
                                            )}
                                        </div>
                                        <div className="mt-auto d-flex w-100 justify-content-between text-muted">
                                            <p className="mb-0">
                                                Order total
                                            </p>
                                            <p className="mb-0">
                                                {orderDetails.order.displayOrderTotal}
                                            </p>
                                        </div>
                                    </Card>
                                </Col>
                                <Col sm="6">
                                    <Card className="border-0 h-100 bg-light">
                                        <h6>
                                            {orderDetails.order.payments.length} payment{orderDetails.order.payments.length == 1 ? '' : 's'}
                                        </h6>
                                        <div className="pb-3 text-muted small">
                                            {orderDetails.order.payments.map(p =>
                                                <Row key={p.id}>
                                                    <Col xs>
                                                        <span>{p.paymentTypeName}</span>
                                                    </Col>
                                                    <Col xs="auto">
                                                        {p.paymentStatusTypeName}
                                                    </Col>
                                                    <Col xs="2" className="text-right">
                                                        ${p.amount}
                                                    </Col>
                                                </Row>
                                            )}
                                        </div>
                                        <div className="mt-auto d-flex w-100 justify-content-between text-muted">
                                            <p className="mb-0">
                                                Payment total
                                            </p>
                                            <p className="mb-0">
                                                {orderDetails.order.displayPaymentTotal}
                                            </p>
                                        </div>
                                    </Card>
                                </Col>
                            </Row>
                            <hr />
                            <h6>
                                {orderDetails.order.orderStatusTypeName}
                            </h6>
                            <p className="text-muted">
                                {orderDetails.order.orderStatusTypeDescription}
                            </p>
                        </div>}
                </div>
            </Container>
        )
    }
}