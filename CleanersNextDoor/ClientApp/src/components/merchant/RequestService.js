import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom';
import { AuthConsumer } from './../../context/AuthContext';
import { authenticationService } from './../../services/authentication.service';

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
            customer: authenticationService.currentUserValue
        };

        this.addToCart = this.addToCart.bind(this);
        this.removeCartItem = this.removeCartItem.bind(this);
    }

    componentDidMount() {
        this.populateCustomerCart()
        this.populateMerchantItems()
    }

    populateMerchantItems() {
        fetch(`merchants/${this.state.merchantId}/items`)
            .then(response => response.json())
            .then(data => this.setState({ items: data, itemsLoading: false }));
    }

    populateCustomerCart() {
        const token = this.state.customer.token;
        let headers = {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        }
        fetch(`customers/cart/${this.state.merchantId}`, headers)
            .then(response => response.json())
            .then(data => {
                this.setState({
                    cart: data,
                    cartLoading: false,
                    orderId: data && data.cartItems.length > 0
                        ? data.cartItems[0].orderID
                        : 0
                });
            })
    }

    addToCart = (id) => {
        var cartItemTransaction = {
            itemID: Number(id),
            orderId: this.state.orderId,
            newQty: null
        };
        this.tryCartTransaction(cartItemTransaction)
    }

    removeCartItem = (id) => {
        var removeCartItem = {
            itemID: Number(id),
            orderId: this.state.orderId,
        };

        const headers = {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${this.state.customer.token}`
        }

        const request = {
            method: 'post',
            headers: headers,
            body: JSON.stringify(removeCartItem)
        }

        fetch(`customers/removeCartItem`, request)
            .then(reponse => reponse.json())
            .then(data => data ? this.populateCustomerCart() : console.log(data))
    }

    handleQtyChange = (event) => {
        var newQty = event.target.value;
        var itemId = event.target.name;

        const cartItemTransaction = {
            itemID: Number(itemId),
            orderId: this.state.orderId,
            newQty: Number(newQty)
        };

        this.tryCartTransaction(cartItemTransaction);
    }

    tryCartTransaction(cartItemTransaction) {
        const headers = {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${this.state.customer.token}`
        }
        const request = {
            method: 'post',
            headers: headers,
            body: JSON.stringify(cartItemTransaction)
        }
        fetch(`customers/addtocart`, request)
            .then(response => response.json())
            .catch(ex => console.log(ex))
            .then(data => {
                this.populateCustomerCart()
            })
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ isAuth }) => (
                        <div>
                            {!isAuth
                                ? <Redirect to='/customer/sign-in' />
                                : this.renderContent()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }

    renderContent() {
        let contents = this.state.itemsLoading
            ? <p><em>Loading Items...</em></p>
            : this.renderMerchantItems(this.state.items);

        let currentOrderContents = this.state.cartLoading
            ? <p><em>Loading Cart...</em></p>
            : this.renderCart(this.state.cart);

        return (
            <div className="container">
                <h1>Start Pick up Request</h1>
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
