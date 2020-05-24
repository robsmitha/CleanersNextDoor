import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AuthConsumer } from './../../context/AuthContext'
import { merchantService } from '../../services/merchant.service'
import Loading from '../../helpers/Loading';
import { FaStar, FaHeart, FaMapMarker, FaShoppingCart, FaClock } from 'react-icons/fa';
import './Merchant.css'
export class Merchant extends Component {
    constructor(props) {
        super(props)
        this.state = {
            merchantId: this.props.match.params.id,
            merchant: null,
            items: null,
            itemTypes: null
        }
        this.servicesRef = React.createRef()  
    }

    componentDidMount() {
        this.populateMerchantInformation()
        this.populateMerchantItems()
    }

    populateMerchantInformation() {
        merchantService.getMerchant(this.state.merchantId)
            .then(data => {
                this.setState({
                    merchant: data
                })
            })
    }

    scrollToServices = () => window.scrollTo(0, this.servicesRef.current.offsetTop)

    populateMerchantItems(itemTypeId) {
        merchantService.getItems(this.state.merchantId, itemTypeId)
            .then(data => {
                this.setState({
                    items: data.items,
                    itemTypes: data.itemTypes
                })
            })
    }

    itemTypeFilter = (event, itemTypeId) => {
        event.preventDefault()
        this.populateMerchantItems(itemTypeId)
    }

    render() {
        const { merchant, items, itemTypes } = this.state
        return (
            <div>
                {merchant !== null
                    ? Merchant.renderMerchant(merchant, this.scrollToServices)
                    : <div>
                        <section className="pt-7 pb-5 d-flex align-items-end dark-overlay bg-cover">
                            <div className="container overlay-content">
                                <div className="d-flex justify-content-between align-items-start flex-column flex-lg-row align-items-lg-end">
                                    <div className="text-white mb-4 mb-lg-0">
                                        <h1 className="text-shadow verified">
                                            <Loading message="Loading merchant, please wait.." />
                                        </h1>
                                    </div>
                                </div>
                            </div>
                        </section>
                        <section className="pb-6">
                            <div className="container">
                                <div className="row">
                                    <div className="col-md-8">
                                        <h2 className="mb-2">About</h2>
                                        <Loading />
                                    </div>
                                    <div className="col-md-4">
                                        <h2 className="mb-2">Locations</h2>
                                        <Loading />
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>}
                


                <section className="pb-6" ref={this.servicesRef}>
                    <div className="container">
                        <h2 className="mb-2">Services</h2>
                        {items === null
                            ? <Loading />
                            : Merchant.renderMerchantItems(items, itemTypes, merchant, this.itemTypeFilter)}
                    </div>
                </section>
                
            </div>
        )
    }
    static renderMerchant(merchant, scrollToServices) {
        const defaultLocation = merchant.locations.filter(i => i.isDefault === true);
        const defaultImageUrl = merchant.images.filter(i => i.isDefault === true);
        return (
            <div>
                <section className="pt-7 pb-5 d-flex align-items-end dark-overlay bg-cover" style={{
                    backgroundImage: 'url(:url)'.replace(':url', defaultImageUrl[0].imageUrl)
                }}>
                    <div className="container overlay-content">
                        <div className="d-flex justify-content-between align-items-start flex-column flex-lg-row align-items-lg-end">
                            <div className="text-white mb-4 mb-lg-0">
                                <div className="badge badge-pill badge-transparent px-3 py-2 mb-4">{merchant.merchantTypeName}</div>
                                <h1 className="text-shadow verified">{merchant.name}</h1>
                                <p><FaMapMarker />&nbsp;{defaultLocation[0].location}</p>
                                <span className="d-block">
                                    <span className="text-white">
                                        <FaClock />&nbsp;
                                        {defaultLocation[0].operatingHours}
                                    </span>
                                </span>
                            </div>
                            <button type="button" className="btn btn-primary btn-lg mb-3" onClick={scrollToServices}>
                                See our services
                            </button>
                        </div>
                    </div>
                </section>
                <section className="py-5">
                    <div className="container">
                        <div className="row">
                            <div className="col-md-8">
                                <h2 className="mb-2">About</h2>
                                <p className="lead">{merchant.shortDescription}</p>
                                <p className="text-muted">
                                    {merchant.description}
                                </p>
                                <hr className="mb-0" />
                            </div>
                            <div className="col-md-4">
                                <h2 className="mb-2">Locations</h2>
                                <div className="list-group border-0 shadow mb-5">
                                    {merchant.locations.map(l =>
                                        <div className="list-group-item" key={l.id}>
                                            <address className="mb-1">
                                                {l.location}
                                            </address>
                                            <address className="small text-muted">
                                                <abbr title="Hours">H:</abbr>
                                                {l.operatingHours}
                                                <br />
                                                <abbr title="Phone">P:</abbr>
                                                {l.phone}
                                                <br />
                                                <abbr title="Email">E:</abbr>
                                                <a href={'mailto:{0}'.replace('{0}', l.contactEmail)}>{l.contactEmail}</a>
                                            </address>
                                        </div>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        )
    }

    static renderAuthCTA(merchantId) {
        return (
            <AuthConsumer>
                {({ authenticated }) => (
                    <span>
                        {authenticated
                            ? <Link to={'/request-service/:id'.replace(':id', merchantId)} className="btn btn-primary btn-lg mb-3" hidden={!authenticated}>
                                Start pickup request
                            </Link>
                            : <Link to="/sign-in" className="btn btn-primary btn-lg mb-3" hidden={authenticated}>
                                Sign in to order
                            </Link>}
                    </span>
                )}
            </AuthConsumer>
        )
    }

    static renderMerchantItems(items, itemTypes, merchant, itemTypeFilter) {
        return (
            <div>
                <p className="lead">
                    {merchant !== null ? merchant.callToAction : ''}
                </p>
                <ul className="nav nav-pills-custom mb-3">
                    {itemTypes !== null
                        ? itemTypes.map(it =>
                        <li key={it.id} className="nav-item">
                                <a href="#" className="nav-link px-2 py-1" value={it.id} onClick={(event) => itemTypeFilter(event, it.id)}>
                                {it.name}
                            </a>
                        </li>
                        )
                    : ''}
                </ul>
                <div className="row">
                    {items.map(i =>
                        <div key={i.id} className="col-md-4 col-sm-6 mb-4 hover-animate">
                            <div className="card h-100 border-0 shadow">
                                <div style={{
                                    backgroundImage: 'url(:url)'.replace(':url', i.defaultImageUrl),
                                    minHeight: 200 + 'px'
                                }} className="card-img-top overflow-hidden dark-overlay bg-cover">
                                    <Link className="text-decoration-none tile-link" to={'/item/:id'.replace(':id', i.id)}>
                                        <div className="card-img-overlay-bottom z-index-20">
                                            <h4 className="text-white text-shadow">{i.name}</h4>
                                            <p className="mb-2 text-xs">
                                                <FaStar color="gold" />&nbsp;
                                                        <FaStar color="gold" />&nbsp;
                                                    </p>
                                        </div>
                                        <div className="card-img-overlay-top d-flex justify-content-between align-items-center">
                                            <div className="badge badge-transparent badge-pill px-3 py-2">
                                                {i.itemTypeName}
                                            </div>
                                            <button type="button" to={''} className="btn btn-link card-fav-icon position-relative z-index-40">
                                                <FaHeart />
                                            </button>
                                        </div>
                                    </Link>
                                </div>
                                <div className="card-body d-flex flex-column">
                                    <p className="mb-1">{i.shortDescription}</p>
                                    <div className="row mt-auto">
                                        <div className="col-auto" hidden={i.displayPrice.length === 0}>
                                            <small className="text-muted d-block">
                                                {i.displayPrice}
                                            </small>
                                        </div>
                                        <div className="col-auto" hidden={i.maxAllowed === null || true}>
                                            <small className="text-muted d-block">
                                                Max allowed: {i.displayMaxAllowed}
                                            </small>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        )
    }

}