import React from 'react';

const TextInput = props => {
    let formControl = "form-control";

    if (props.touched && !props.valid) {
        formControl = 'form-control is-invalid';
    }

    let errors = props.errors !== undefined ? props.errors : []
    let hidden = props.hidden !== undefined ? props.hidden : false;
   
    return (
        <div className="form-group" hidden={hidden}>
            <label htmlFor={props.name} className="font-weight-bold">{props.label === undefined ? props.name : props.label}</label>
            <input className={formControl} {...props} />
            {errors.map(m =>
                <div key={m} className="text-danger">
                    {m}
                </div>
            )}
        </div>
        )
}

export default TextInput
