import React from 'react';
import { Route } from 'react-router-dom';  
import { NavMenu } from './NavMenu';
import { Footer } from './Footer';



const Layout = ({ children }) => (
    <div>
        <NavMenu />
        {children}
        <Footer />
    </div>
);

const LayoutRoute = ({ component: Component, ...rest }) => {
    return (
        <Route {...rest} render={matchProps => (
            <Layout>
                <Component {...matchProps} />
            </Layout>
        )} />
    )
}

export default LayoutRoute;