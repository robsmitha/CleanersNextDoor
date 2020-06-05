import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom';
import { AuthConsumer } from './../../context/AuthContext';
import { customerService } from '../../services/customer.service'
import Loading from '../../helpers/Loading';
import { FaShoppingCart } from 'react-icons/fa'

export class Cart extends Component {

    constructor(props) {
        super(props);
        this.state = {
            cart: null,
            merchantId: Number(this.props.match.params.id),
            orderId: 0
        };
    }

    componentDidMount() {
        this.populateCustomerCart()
    }

    async populateCustomerCart() {
        let data = await customerService.getCart(this.state.merchantId)
        if (data) {
            this.setState({
                cart: data,
                orderId: data.orderID,
                merchantName: data.merchantName
            });
        }
        else {

        }
    }

    handleQtyChange = (event, currentQty) => {
        var newQty = event.target.value;
        var itemId = event.target.name;

        const data = {
            itemID: Number(itemId),
            orderId: this.state.orderId,
            newQty: Number(newQty) - Number(currentQty)
        };

        customerService.cartTransaction(data)
            .then(data => {
                if (data !== null) {
                    if (data.orderID > 0) {
                        this.populateCustomerCart()
                    } else {
                        alert(data)
                    }
                }
                else {
                    //request failed
                }
            })
    }

    removeCartItem = (id) => {
        const data = {
            itemID: Number(id),
            orderId: this.state.orderId,
        };
        customerService.removeCartItem(data)
            .then(data => data ? this.populateCustomerCart() : console.log(data))
    }

    render() {
        const { cart, merchantId, merchantName } = this.state
        return (
            <AuthConsumer>
                {({ authenticated }) => (
                    <div>
                        {!authenticated
                            ? <Redirect to='/sign-in' />
                            : <section>
                                <div className="progress rounded-0 sticky-top">
                                    <div className="progress-bar" role="progressbar" style={{ width: 50 + '%' }} aria-valuenow={50} aria-valuemin={50} aria-valuemax="100"></div>
                                </div>
                                <div className="container my-3">
                                    <div className="row">
                                        <div className="col-md-8">
                                            <h1 className="h2">Shopping cart</h1>
                                            {cart === null
                                                ? <Loading message="Loading cart, please wait.." />
                                                : Cart.renderCart(cart, merchantId, this.removeCartItem, this.handleQtyChange)
                                            }
                                        </div>
                                        <div className="col-md-4">
                                            <h2>Your Order</h2>
                                            {cart === null
                                                ? <Loading />
                                                : Cart.renderTotal(cart, merchantId, merchantName)
                                            }
                                        </div>
                                    </div>
                                </div>
                            </section>}
                    </div>
                )}
            </AuthConsumer>
        )
    }

    static renderTotal(cart, merchantId, merchantName) {
        const { cartItems } = cart
        return (
            <div>
                <div className="card border-0 shadow mb-3">
                    <div className="list-group list-group-flush">
                        <div className="list-group-item">
                            <div className="d-flex w-100 justify-content-between">
                                <p className="mb-0">
                                    {merchantName}
                                </p>
                                <span className="d-block">
                                    <span className="badge badge-dark badge-pill">
                                        <FaShoppingCart />&nbsp;
                                        {cartItems.reduce((a, b) => a + b.currentQuantity, 0)}
                                    </span>
                                </span>
                            </div>
                        </div>
                        {cartItems.map(c =>
                            <div className="list-group-item" key={c.id}>
                                <div className="form-row">
                                    <div className="col-md-7">
                                        <small className="text-muted"> {c.itemName}</small>
                                    </div>
                                    <div className="col-2 text-right">
                                        <small className="text-muted">X {c.currentQuantity}</small>
                                    </div>
                                    <div className="col-3 text-right">
                                        <small className="text-muted">{c.displayPrice}</small>
                                    </div>
                                </div>
                            </div>
                        )}
                        <div className="list-group-item list-group-item-light">
                            <span>Total (USD):</span>
                            <span className="float-right">
                                {cart.displayPrice}
                            </span>
                        </div>
                    </div>
                </div>
                <Link to={'/payment/:id'.replace(':id', merchantId)} className="btn btn-dark btn-block">
                    Checkout now
                </Link>
                <Link to={'/merchant/:id'.replace(':id', merchantId)} className="btn btn-light btn-block">
                    Continue shopping
                </Link>
            </div>
            )
    }

    static renderCart(cart, merchantId, removeCartItem, handleQtyChange) {
        let cartItems = cart.cartItems;
        return (
            <div>
                {cartItems.map(c =>
                    <div className="border-bottom mb-3" key={c.id}>
                        <div className="d-flex w-100 justify-content-between">
                            <h5 className="mb-1"> {c.itemName}</h5>
                            <small>{c.displayPrice}</small>
                        </div>
                        <div className="form-row">
                            <div className="col-auto">
                                <div className="input-group input-group-sm  mb-3">
                                    <span className="input-group-prepend">
                                        <span className="input-group-text bg-light text-dark border-0">
                                            <label htmlFor="qty">QTY</label>
                                        </span>
                                    </span>
                                    <select value={c.currentQuantity} name={c.itemID} className="custom-select rounded" onChange={(event) => handleQtyChange(event, c.currentQuantity)}>
                                        {[...Array(c.itemMaxAllowed)].map((x, i) =>
                                            <option key={i + 1} value={i + 1}>{i + 1}</option>
                                        )}
                                    </select>
                                </div>
                            </div>
                            <div className="col-auto">
                                <button type="button" className="btn btn-link btn-sm text-danger" onClick={() => removeCartItem(c.itemID)}>
                                    Remove
                                    </button>
                            </div>
                        </div>
                    </div>
                )}
                <div hidden={cartItems.length > 0}>
                    You have no items in your cart. <Link to={'/merchant/:id'.replace(':id', merchantId)}>Click here</Link> to browse items.
                </div>
            </div>
        )
    }
}
