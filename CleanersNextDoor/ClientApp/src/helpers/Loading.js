import React from 'react';

const Loading = props => {
    let message = props.message !== undefined ? props.message : ''
    return (
        <span>
            <span className="spinner-grow" role="status">
                <span className="sr-only">Loading...</span>
            </span>
            {message}
        </span>
    )
}

export default Loading