import React from 'react';

const TextInput = props => {
    let formControl = "form-control";

    if (props.touched && !props.valid) {
        formControl = 'form-control is-invalid';
    }

    let errors = props.errors !== undefined ? props.errors : []
   
    return (
        <div className="form-group">
            <label htmlFor={props.name}>{props.label === undefined ? props.name : props.label}</label>
            <input type="text" className={formControl} {...props} />
            {errors.map(m =>
                <div key={m} className="text-danger">
                    {m}
                </div>
            )}
        </div>
        )
}

export default TextInput
