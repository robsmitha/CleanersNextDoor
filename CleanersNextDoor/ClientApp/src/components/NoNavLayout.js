import React from 'react';
import { Route } from 'react-router-dom';
import { Footer } from './Footer';



const NoNavLayout = ({ children }) => (
    <div>
        {children}
        <Footer />
    </div>
);

const NoNavLayoutRoute = ({ component: Component, ...rest }) => {
    return (
        <Route {...rest} render={matchProps => (
            <NoNavLayout>
                <Component {...matchProps} />
            </NoNavLayout>
        )} />
    )
}

export default NoNavLayoutRoute;