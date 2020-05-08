import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { AuthContext } from './../../context/AuthContext'
import { merchantService } from '../../services/merchant.service'
import Loading from '../../helpers/Loading';
import './Item.css'
import { FaShoppingCart } from 'react-icons/fa';
import { customerService } from '../../services/customer.service'

export class Item extends Component {
    constructor(props) {
        super(props)
        this.state = {
            id: this.props.match.params.id,
            item: null,
            cart: null,
            loggedIn: false,
            orderId: 0,
            selectedQuantity: 1,
            totalDisplay: null,
            currentQuantity: 0
        }
    }

    componentDidMount() {
        this.setState({
            loggedIn: this.context.authenticated
        })
        this.populateItem(this.state.id)
    }

    handleSubmit = (e) => {
        e.preventDefault()
        const { selectedQuantity, currentQuantity, orderId, item } = this.state;
        if (selectedQuantity === 0 && currentQuantity === 0) return;
        const data = {
            itemID: item.id,
            orderId: orderId,
            newQty: selectedQuantity
        };
        this.updateCart(data)
    }

    handleQtyChange = (event) => {
        var newQty = event.target.value;
        this.setState({
            selectedQuantity: Number(newQty)
        })
    }

    updateCart = (data) => {
        customerService.cartTransaction(data)
            .then(data => {
                if (data !== null) {
                    if (data.orderID > 0) {
                        this.populateCustomerCart(this.state.item.merchantID)
                    } else {
                        alert(data)
                    }
                }
                else {
                    //request failed
                }
            })
    }

    loadItem = (event, id) => {
        this.setState({
            id: id,
            item: null,
            cart: null,
            selectedQuantity: 1,
            totalDisplay: null,
            currentQuantity: 0
        })
        this.populateItem(id)
    }

    populateItem = async (id) => {
        const data = await merchantService.getItem(id)
        if (data) {
            this.setState({
                item: data.item
            })

            if (data.item.merchantID && this.state.loggedIn) {
                this.populateCustomerCart(data.item.merchantID)
            }
        }
        else {
            //could not load item, show "loading error, try again" helper message
        }
    }

    populateCustomerCart = async (merchantId) => {
        const data = await customerService.getCart(merchantId)
        if (data) {
            const lineItemsInCart = data.cartItems.filter(i => i.itemID === Number(this.state.id))
            const qty = lineItemsInCart.length === 0 ? 0 : lineItemsInCart[0].currentQuantity
            this.setState({
                cart: data,
                orderId: data.orderID,
                selectedQuantity: 1,
                currentQuantity: qty
            });
        }
    }


    render() {
        const {
            item,
            cart,
            loggedIn,
            selectedQuantity,
            currentQuantity
        } = this.state;
        return (
            <div>
                <section className="py-5">
                    <div className="container">
                        <div className="row">
                            <div className="col-md-8">
                                {item !== undefined && item !== null
                                    ? Item.renderItem(item, selectedQuantity, currentQuantity, this.handleSubmit, this.handleQtyChange, this.state.loggedIn)
                                    : <Loading message="Loading item, please wait..." />}
                            </div>
                            <div className="col-md-4">
                                <h2 className="mb-2">Your cart</h2>
                                {!loggedIn
                                    ? <div className="card">
                                        <div className="list-group list-group-flush">
                                            <div className="list-group-item">
                                                <small className="d-block">
                                                    Merchant
                                                </small>
                                            </div>
                                            <div className="list-group-item">
                                                <p>
                                                    Please&nbsp;<Link to={'/sign-up'}>create an account</Link>&nbsp;or&nbsp;
                                                    <Link to={'/sign-in'}>sign in</Link>&nbsp;to start an order with.
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                    : cart !== null && item !== null
                                        ? Item.renderCart(cart, item, this.loadItem)
                                        : <Loading />}
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        )
    }
    static renderItem(item, selectedQuantity, currentQuantity, handleSubmit, handleQtyChange, loggedIn) {
        const defaultImage = item.images.filter(i => i.isDefault === true);
        return (
            <div>
                <h2 className="mb-0">{item.name}</h2>
                <small className="text-muted d-block mb-2">
                    {item.displayPrice}
                </small>
                <p className="lead">{item.shortDescription}</p>
                <p className="text-muted">
                    {item.description}
                </p>
                <small className="text-muted d-block">
                    Max allowed: {item.maxAllowed}
                </small>
                <div className="my-3">
                    {item.maxAllowed <= currentQuantity
                        ? <small className="text-muted d-block">Max reached: You have {currentQuantity} {item.name}(s) in your cart.</small>
                        : <form onSubmit={loggedIn === true ? handleSubmit : (event) => event.preventDefault()}>
                            <div className="form-row">
                                <div className="col-md-auto col-12">
                                    <div className="input-group mb-3">
                                        <span className="input-group-prepend">
                                            <span className="input-group-text bg-light text-dark border-0">
                                                QTY
                                        </span>
                                        </span>
                                        <select disabled={loggedIn === false} value={selectedQuantity} className="custom-select rounded" onChange={handleQtyChange}>
                                            {[...Array(item.maxAllowed)].map((x, i) =>
                                                <option key={i + 1} value={i + 1}>{i + 1}</option>
                                            )}
                                        </select>
                                        <span className="input-group-append" hidden={true}>
                                            <span className="input-group-text bg-light text-dark border-0" hidden={item.displayPrice.length === 0}>{item.displayPrice}</span>
                                        </span>
                                    </div>
                                </div>
                                <div className="col-md-auto col-12">
                                    <button type="submit" className="btn btn-outline-dark btn-block"
                                        disabled={loggedIn === false || selectedQuantity === 0 && currentQuantity === 0}>
                                        Add {selectedQuantity} to cart ${(selectedQuantity * item.price).toFixed(2)}
                                    </button>
                                </div>
                            </div>
                        </form>}
                </div>
                <hr />
                <div className="mb-3">
                    <h2 className="mb-2">Gallery</h2>
                    <div className="row">
                        {item.images.map((ii, index) =>
                            <div className="col-md-4" key={index}>
                                <img src={ii.imageUrl} className="img-thumbnail img-fluid" />
                            </div>
                        )}
                    </div>
                </div>
                <hr />
                <div className="mb-3">
                    <h2 className="mb-2">Tags</h2>
                    <div className="badge badge-pill badge-dark px-3 py-2">{item.itemTypeName}</div>
                </div>
            </div>
        )
    }

    static renderCart(cart, item, loadItem) {
        const { cartItems } = cart;
        return (
            <div>
                <div className="card border-0 shadow mb-3">
                    <div className="list-group list-group-flush">
                        <div className="list-group-item">
                            <div className="d-flex w-100 justify-content-between">
                                <p className="mb-0">
                                    {item.merchantName}
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
                            <button type="button" className="list-group-item list-group-item-action" key={c.id} onClick={(event) => loadItem(event, c.itemID)} disabled={item.id === c.itemID}>
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
                            </button>
                        )}
                        <div className="list-group-item list-group-item-light">
                            <span>Total (USD):</span>
                            <span className="float-right">
                                {cart.displayPrice}
                            </span>
                        </div>

                    </div>
                </div>
                <Link className="btn btn-dark btn-block mb-2" to={'/cart/:id'.replace(':id', item.merchantID)}>
                    View your cart
                </Link>
                <Link className="btn btn-light btn-block mb-2" to={'/merchant/:id'.replace(':id', item.merchantID)}>
                    Continue shopping
                </Link>
            </div>
        )
    }
}

Item.contextType = AuthContext