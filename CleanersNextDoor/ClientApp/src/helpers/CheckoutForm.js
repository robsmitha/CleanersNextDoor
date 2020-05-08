import React, { useState } from 'react';
import {
    CardElement,
    useElements,
    useStripe
} from '@stripe/react-stripe-js';

// Custom styling can be passed to options when creating an Element.
const CARD_ELEMENT_OPTIONS = {
    style: {
        base: {
            color: '#32325d',
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSmoothing: 'antialiased',
            fontSize: '16px',
            '::placeholder': {
                color: '#aab7c4'
            }
        },
        invalid: {
            color: '#fa755a',
            iconColor: '#fa755a'
        }
    }
};

const CheckoutForm = (props) => {
    const [error, setError] = useState(null);
    const stripe = useStripe();
    const elements = useElements();
    console.log(props)
    // Handle real-time validation errors from the card Element.
    const handleChange = (event) => {
        if (event.error) {
            setError(event.error.message);
        } else {
            setError(null);
        }
    }

    // Handle form submission.
    const handleSubmit = async (event) => {
        event.preventDefault();
        if (!stripe || !elements) {
            // Stripe.js has not yet loaded.
            // Make sure to disable form submission until Stripe.js has loaded.
            return;
        }

        const result = await stripe.confirmCardPayment(props.clientSecret, {
            payment_method: {
                card: elements.getElement(CardElement),
                billing_details: {
                    name: props.name,
                },
            }
        });

        if (result.error) {
            // Inform the user if there was an error.
            setError(result.error.message);
        } else {
            setError(null);

            // The payment has been processed!
            if (result.paymentIntent.status === 'succeeded') {
                // Show a success message to your customer
                // There's a risk of the customer closing the window before callback
                // execution. Set up a webhook or plugin to listen for the
                // payment_intent.succeeded event that handles any business critical
                // post-payment actions.
                props.stripeTokenHandler(result.paymentIntent)
            }
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div className="form-group">
                <label htmlFor="card-element" className="font-weight-bold">
                    Credit or debit card
                </label>
                <CardElement
                    className="form-control"
                    id="card-element"
                    options={CARD_ELEMENT_OPTIONS}
                    onChange={handleChange}
                />
                <div className="card-errors" role="alert">{error}</div>
            </div>
            <button type="submit" className="btn btn-dark btn-block" disabled={props.disabled}>
                Complete Checkout
            </button>
        </form>
    );
}

export default CheckoutForm