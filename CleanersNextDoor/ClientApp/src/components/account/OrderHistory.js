import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom';
import { customerService } from '../../services/customer.service';
import { AuthConsumer } from './../../context/AuthContext'
import { Container, ListGroup, Row, Col, Badge, ListGroupItem } from 'reactstrap';
import './OrderHistory.css'
import Loading from '../../helpers/Loading';

export class OrderHistory extends Component {
    constructor(props) {
        super(props)
        this.state = {
            orders: null,
            selectedSort: 'isUpcoming'
        }
    }

    componentDidMount() {
        this.populateOrderHistory();
    }

    async populateOrderHistory() {
        let data = await customerService.getOrderHistory()
        if (data) {
            let orders = []
            if (data !== null) {
                orders = data.orders.sort((a, b) => {
                    var ad = a.modifiedTime === null ? new Date(a.createdAt).getTime() : new Date(a.modifiedTime)
                    var bd = b.modifiedTime === null ? new Date(b.createdAt).getTime() : new Date(b.modifiedTime)
                    return bd - ad;
                })
                //orders = orders.concat(data.orders.filter(o => o.isOpenOrder))
                //    .concat(data.orders.filter(o => o.isUpcomingOrder))
                //    .concat(data.orders.filter(o => !o.isOpenOrder && !o.isUpcomingOrder))
            }
            this.setState({
                orders: orders
            })
        }
        else {
            //error occurred, load error component
        }
    }

    handleSortChange = event => {
        var sortBy = event.target.value;
        console.log(sortBy)
        this.setState({
            selectedSort: sortBy
        })
    }

    render() {
        return (
            <AuthConsumer>
                {({ authenticated }) => (
                    <div>
                        {!authenticated
                            ? <Redirect to='/sign-up' />
                            : OrderHistory.renderOrders(this.state, this.handleSortChange)}
                    </div>
                )}
            </AuthConsumer>
        )
    }

    static renderOrders(state, handleSortChange) {
        const { orders, selectedSort } = state
        return (
            <Container className="mt-3 mb-5">

                <Link to={'/'}>Home</Link> &minus;
                <Link to={'/account'}> Account</Link> &minus; Your order history

                <div className="my-md-5 my-4">
                    <h1 className="h3">
                        Order history
                    </h1>
                    <p className="text-muted">
                        You can always access your past orders on your account.
                    </p>
                </div>
                <Row>
                    <Col md>
                        You have <strong>{orders === null ? 0 : orders.length} orders</strong>.
                    </Col>
                    <Col md="3">
                        <div className="input-group mb-3">
                            <span className="input-group-prepend">
                                <span className="input-group-text bg-light text-dark border-0">
                                    SORT BY
                                </span>
                            </span>
                            <select value={selectedSort} className="custom-select rounded" onChange={handleSortChange}>
                                <option key='isUpcomingOrder' value='isUpcomingOrder' defaultValue>Upcoming</option>
                                <option key='isOpenOrder' value='isOpenOrder'>Open</option>
                                <option key='isComplete' value='isComplete'>Past</option>
                            </select>
                        </div>

                    </Col>
                </Row>
                <ListGroup className="list-group shadow">
                    {orders === null
                        ? <ListGroupItem><Loading message="Loading orders, please wait.." /></ListGroupItem>
                        : orders.map((o, index) =>
                            <Link className="list-group-item list-group-item-action p-4" key={o.id} to={o.isOpenOrder
                                ? '/cart/:id'.replace(':id', o.merchantID)
                                : '/order-details/:id'.replace(':id', o.id)}>
                                <Row>
                                    <Col lg="4" className="align-self-center mb-4 mb-lg-0">
                                        <div className="mb-3">
                                            <h2 className="h5 mb-0">
                                                {o.merchantName}
                                            </h2>
                                        </div>
                                        <p className="text-sm text-muted">
                                            {o.merchantMerchantTypeName}
                                        </p>

                                        <Badge pill color={o.isUpcomingOrder ? 'success' : o.isOpenOrder ? 'warning' : 'secondary'} className="p-2 font-weight-bold">
                                            {o.displayUpdated}
                                        </Badge>
                                    </Col>
                                    <Col lg="8">
                                        <Row>
                                            <Col xs="6" md="4" className="mb-3 mb-lg-0">
                                                <h6 className="label-heading">
                                                    Total
                                                </h6>
                                                <p className="text-sm font-weight-bold">
                                                    {o.displayOrderTotal}
                                                </p>
                                                <h6 className="label-heading">
                                                    # of items
                                                </h6>
                                                <p className="text-sm font-weight-bold">
                                                    {o.lineItems.length}
                                                </p>
                                            </Col>
                                            <Col xs="6" md="4" className="mb-3 mb-lg-0" hidden={o.serviceRequest.name === null || o.serviceRequest.name.length === 0}>
                                                <h6 className="label-heading">
                                                    Name
                                                    </h6>
                                                <p className="text-sm font-weight-bold">
                                                    {o.serviceRequest.name}
                                                </p>
                                                <h6 className="label-heading">
                                                    Phone
                                                    </h6>
                                                <p className="text-sm font-weight-bold">
                                                    {o.serviceRequest.phone}
                                                </p>
                                            </Col>
                                            <Col xs="6" md="4" className="align-self-center" hidden={o.serviceRequest.name !== null && o.serviceRequest.name.length > 0}>
                                                <h6 className="label-heading text-secondary">
                                                    Please checkout
                                                </h6>
                                            </Col>
                                            <Col xs="12" md="4" className="align-self-center">
                                                <span className={'text-sm text-uppercase mr-4 mr-lg-0 '.concat(o.isOpenOrder ? 'text-warning' : 'text-primary')}>
                                                    {o.orderStatusTypeName}
                                                </span>
                                                <br className="d-none d-lg-block" />
                                                <span className="text-success text-sm text-uppercase mr-4 mr-lg-0">
                                                    {o.serviceRequest.serviceRequestStatusTypeName}
                                                </span>
                                            </Col>
                                        </Row>
                                    </Col>
                                </Row>
                            </Link>
                        )}
                </ListGroup>
            </Container>
            )
    }
}