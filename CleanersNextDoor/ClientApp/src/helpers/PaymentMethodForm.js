import React, { useState } from 'react';
import { customerService } from './../services/customer.service'
import {
    CardElementProps,
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

const PaymentMethodForm = (props) => {
    const [error, setError] = useState(null);
    const stripe = useStripe();
    const elements = useElements();

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
        const card = elements.getElement(CardElement);

        //TODO: set default_source to isDefault value
        //https://stripe.com/docs/saving-cards#multiple-payment-methods
        let stripeClientSecret = await customerService.stripeClientSecret();
        const result = await stripe.confirmCardSetup(stripeClientSecret.secret, {
            payment_method: {
                card: card,
                billing_details: {
                    name: props.cardHolderName,
                },
            }
        });

        if (result.error) {
            // Inform the user if there was an error.
            setError(result.error.message);
        } else {
            setError(null);
            // The setup has succeeded. Display a success message and send
            // result.setupIntent.payment_method to your server to save the
            // card to a Customer
            props.stripePaymentMethodHandler(result.setupIntent.payment_method);
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
            <button type="submit" className="btn btn-primary btn-block" disabled={props.disabled}>
                Save payment method
            </button>
        </form>
    );
}

export default PaymentMethodForm