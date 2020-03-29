import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Authentication } from '../../services/authentication';

export class RequestService extends Component {

    constructor(props) {
        super(props);
        this.state = {
            items: [],
            itemsLoading: true,
            cart: [],
            cartLoading: true,
            merchantId: Number(this.props.match.params.id),
            orderId: 0,
            customerId: Authentication.getCustomerId()
        };

        this.addToCart = this.addToCart.bind(this);
        this.removeCartItem = this.removeCartItem.bind(this);
    }

    componentDidMount() {
        this.populateMerchantItems()
        this.populateCustomerCart()
    }

    async populateMerchantItems() {
        const merchantId = this.state.merchantId
        const response = await fetch(`merchants/${merchantId}/items`);
        const data = await response.json();
        this.setState({ items: data, itemsLoading: false });
    }

    async populateCustomerCart() {
        const customerId = Authentication.getCustomerId();
        const merchantId = this.state.merchantId
        const response = await fetch(`customers/${customerId}/cart/${merchantId}`);
        const data = await response.json();
        if (this.state.orderId === 0 && data && data.cartItems.length > 0) {
            this.setState({
                orderId: data.cartItems[0].orderID
            })
        }
        this.setState({ cart: data, cartLoading: false });
    }

    addToCart = (id) => {
        var cartItemTransaction = {
            itemID: Number(id),
            customerId: this.state.customerId,
            orderId: this.state.orderId,
            newQty: null
        };
        this.tryCartTransaction(cartItemTransaction)
    }

    removeCartItem = (id) => {
        this.tryRemoveCartItem(id)
    }

    handleQtyChange = (event) => {
        var newQty = event.target.value;
        var itemId = event.target.name;

        var cartItemTransaction = {
            itemID: Number(itemId),
            customerId: this.state.customerId,
            orderId: this.state.orderId,
            newQty: Number(newQty)
        };

        this.tryCartTransaction(cartItemTransaction);
    }

    async tryCartTransaction(cartItemTransaction) {

        const response = await fetch(`customers/${this.state.customerId}/addtocart`, {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(cartItemTransaction)
        });

        const data = await response.json();

        if (response.status === 500) {
            alert(data)
        }
        else {
            this.populateCustomerCart()
        }
    }
    async tryRemoveCartItem(id) {

        var removeCartItem = {
            itemID: Number(id),
            orderId: this.state.orderId,
        };

        const response = await fetch(`customers/${this.state.customerId}/removeCartItem`, {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(removeCartItem)
        });

        const success = await response.json();
        if (success) {
            this.populateCustomerCart()
        }
    }
    render() {
        let contents = this.state.itemsLoading
            ? <p><em>Loading Items...</em></p>
            : this.renderMerchantItems(this.state.items);

        let currentOrderContents = this.state.cartLoading
            ? <p><em>Loading Cart...</em></p>
            : this.renderCart(this.state.cart);

        return (
            <div className="container">
                <h1>Start Service Request</h1>
                <h3>Select Services</h3>
                <hr />
                <div className="row">
                    <div className="col-md-4">
                        {currentOrderContents}
                    </div>
                    <div className="col-md-8">
                        {contents}
                    </div>
                </div>
            </div>
        )
    }

    renderMerchantItems(items) {
        return (
            <div>
                <div className="list-group">
                    {items.map(i =>
                        <button key={i.id} value={i.id} type="button" className="list-group-item list-group-item-action" onClick={() => this.addToCart(i.id)}>
                            <div className="d-flex w-100 justify-content-between">
                                <h5 className="mb-1"> {i.name}</h5>
                                <small>{i.displayPrice}</small>
                            </div>
                            <p className="mb-1">{i.description}</p>
                            <small>Max Allowed: {i.maxAllowed}</small>
                        </button>
                    )}
                </div>
            </div>
        )
    }


    renderCart(cart) {
        let cartItems = cart.cartItems;
        return (
            <div>
                <div className="list-group list-group-flush">
                    {cartItems.map(c =>
                        <div className="list-group-item" key={c.id}>
                            <div className="d-flex w-100 justify-content-between">
                                <h5 className="mb-1"> {c.itemName}</h5>
                                <small>{c.displayPrice}</small>
                            </div>
                            <div className="form-row">
                                <div className="col">
                                    <label htmlFor="qty">QTY: </label>
                                    <select value={c.currentQuantity} name={c.itemID} className="form-control  form-control-sm" onChange={this.handleQtyChange}>
                                        {[...Array(c.itemMaxAllowed + 1)].map((x, i) =>
                                            <option key={i} value={i}>{i}</option>
                                        )}
                                    </select>
                                </div>
                                <div className="col-auto">
                                    <br />
                                    <button type="button" className="btn btn-danger btn-sm mt-2" onClick={() => this.removeCartItem(c.itemID)}>
                                        Remove
                                    </button>
                                </div>
                            </div>
                        </div>
                    )}
                    <div className="list-group-item">
                        <strong>Total (USD):</strong>
                        <span className="float-right">
                            {cart.displayPrice}
                        </span>
                    </div>
                </div>
                <Link to={'/payment/:id'.replace(':id', this.state.merchantId)} className="btn btn-primary btn-block">
                    Pay Now
                </Link>
                <Link to={'/merchant/:id'.replace(':id', this.state.merchantId)} className="btn btn-secondary btn-block">
                    Go Back
                </Link>
            </div>
        )
    }
}
