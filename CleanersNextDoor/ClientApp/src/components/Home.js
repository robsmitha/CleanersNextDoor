import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AuthConsumer } from './../context/AuthContext';
import { merchantService } from '../services/merchant.service';
import { Badge } from 'reactstrap';
import Loading from '../helpers/Loading';
import TextInput from '../helpers/TextInput';
import { FaStar, FaMapPin, FaHeart, FaFilter, FaLocationArrow, FaSearch } from 'react-icons/fa';
import './Home.css'

export class Home extends Component {
    constructor(props) {
        super(props)
        this.state = {
            merchants: null,
            formIsValid: false,
            formControls: {
                keyword: {
                    value: '',
                    placeholder: 'Search by merchant or service',
                    label: 'Keyword',
                    valid: true,
                    touched: false,
                    errors: []
                },
                location: {
                    value: '',
                    placeholder: 'Enter location',
                    label: 'Location',
                    valid: true,
                    touched: false,
                    errors: []
                }
            },
            location: '',
            displayLocation: ''
        }
    }

    componentDidMount() {
        this.fetchMerchants({})
    }

    fetchMerchants(args) {
        merchantService.searchMerchants(args)
            .then(data => {
                const updatedControls = {
                    ...this.state.formControls
                }
                if (data.displayLocation && data.displayLocation.length > 0)
                    updatedControls.location.value = data.displayLocation;

                this.setState({
                    merchants: data.merchants,
                    displayLocation: data.displayLocation,
                    formControls: updatedControls
                })
            })
    }

    useMyLocation = event => {
        this.getLocation()
    }

    getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition((position) => {
                let args = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                }
                this.fetchMerchants(args)
            });
        } else {
            console.log("Geolocation is not supported by this browser.")
            //this.getClient()
        }
    }

    changeHandler = event => {
        const name = event.target.name;
        const value = event.target instanceof HTMLInputElement && event.target.getAttribute('type') == 'checkbox'
            ? event.target.checked
            : event.target.value;
        this.handleChange(name, value)
    }

    handleChange = (name, value) => {
        const updatedControls = {
            ...this.state.formControls
        };

        const updatedFormElement = {
            ...updatedControls[name]
        };

        updatedFormElement.value = value;
        updatedFormElement.touched = true;
        updatedFormElement.errors = []//validate(value, updatedFormElement.validationRules, updatedFormElement.placeholder);
        updatedFormElement.valid = updatedFormElement.errors === undefined || updatedFormElement.errors === null || updatedFormElement.errors.length === 0;
        updatedControls[name] = updatedFormElement;
        let formIsValid = true;
        for (let inputIdentifier in updatedControls) {
            formIsValid = updatedControls[inputIdentifier].valid && formIsValid;
        }
        this.setState({
            formControls: updatedControls,
            formIsValid: formIsValid
        })
    }

    submitHandler = event => {
        event.preventDefault()
        const { keyword, location } = this.state.formControls;
        let args = {
            keyword: keyword.value,
            location: location.value
        }
        this.fetchMerchants(args)
    }

    render() {
        const { merchants, formControls, displayLocation } = this.state;
        return (
            <div>
                <div className="container-fluid mt-3">
                    <h1 className="mb-1 h5 text-primary text-uppercase">Nearby Laundry Services</h1>
                    <h2 className="h3 mb-1">
                        {displayLocation == null || displayLocation.length == 0
                            ? <span>All locations</span>
                            : <span>{displayLocation}</span>}
                    </h2>
                    <small className="text-muted">
                        Choose service.&nbsp;
                        Enter pick up / drop off address.&nbsp;
                                <strong>It's that easy.</strong>
                    </small>
                    <hr />
                    <div className="row no-gutter">
                        <div className="col-md-3">
                            <form onSubmit={this.submitHandler}>

                                <div className="row">
                                    <div className="col-sm-12">
                                        <TextInput name="keyword"
                                            placeholder={formControls.keyword.placeholder}
                                            label={formControls.keyword.label}
                                            value={formControls.keyword.value}
                                            onChange={this.changeHandler}
                                            touched={formControls.keyword.touched ? 1 : 0}
                                            valid={formControls.keyword.valid ? 1 : 0}
                                            errors={formControls.keyword.errors} />
                                    </div>
                                    <div className="col-sm-12">
                                        <button type="button" className="btn btn-link btn-sm p-0 float-right" onClick={this.useMyLocation}>
                                            Use my location
                                        </button>
                                        <TextInput name="location"
                                            placeholder={formControls.location.placeholder}
                                            label={formControls.location.label}
                                            value={formControls.location.value}
                                            onChange={this.changeHandler}
                                            touched={formControls.location.touched ? 1 : 0}
                                            valid={formControls.location.valid ? 1 : 0}
                                            errors={formControls.location.errors} />
                                    </div>
                                </div>
                                <div className="my-2">
                                    <button type="submit" className="btn btn-primary mb-2">
                                        <FaFilter />&nbsp;
                                        Filter
                                    </button>
                                </div>
                            </form>
                        </div>
                        <div className="col-md-9">
                            {merchants === null
                                ? <Loading message="Loading merchants" />
                                : Home.renderMerchantList(merchants)}
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    static renderMerchantList(merchants) {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <p>
                            <strong>{merchants.length}</strong>&nbsp;results found
                        </p>
                    </div>
                    <div className="col-auto">

                    </div>
                </div>
                <div className="row">
                    {merchants.map(m =>
                        <div key={m.id} className="col-md-4 col-sm-6 mb-4 hover-animate">
                            <div className="card h-100 border-0 shadow">
                                <div style={{
                                    backgroundImage: 'url(:url)'.replace(':url', m.defaultImageUrl),
                                    minHeight: 200 + 'px'
                                }} className="card-img-top overflow-hidden dark-overlay bg-cover">
                                    <Link className="text-decoration-none tile-link" to={'merchant/:id'.replace(':id', m.id)}>
                                        <div className="card-img-overlay-bottom z-index-20">
                                            <h4 className="text-white text-shadow">{m.name}</h4>
                                            <p className="mb-2 text-xs">
                                                <FaStar color="gold" />&nbsp;
                                                <FaStar color="gold" />&nbsp;
                                            </p>
                                        </div>
                                        <div className="card-img-overlay-top d-flex justify-content-between align-items-center">
                                            <div className="badge badge-transparent badge-pill px-3 py-2">
                                                {m.merchantTypeName}
                                            </div>
                                            <button type="button" to={''} className="btn btn-link card-fav-icon position-relative z-index-40">
                                                <FaHeart />
                                            </button>
                                        </div>
                                    </Link>
                                </div>
                                <div className="card-body text-dark">
                                    <p className="text-muted mb-1">{m.shortDescription}</p>
                                    <small className="text-muted d-block mb-3">
                                        {m.itemTypes.map((it, index) =>
                                            <span key={it.id}>
                                                <Link to={'/category/' + it.id}>
                                                    {it.name}
                                                </Link>
                                                {index < m.itemTypes.length - 1 ? ', ' : ''}
                                            </span>
                                        )}
                                    </small>
                                    <small hidden={m.locations.length === 0}>
                                        <FaMapPin />&nbsp;{m.locations[0].distanceAway.length === 0
                                            ? m.locations[0].location
                                            : <span>{m.locations[0].distanceAway}</span>}
                                    </small>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        )
    }
}