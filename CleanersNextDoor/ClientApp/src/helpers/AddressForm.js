import React from 'react';
import TextInput from './TextInput'

const AddressForm = props => {

    return (
        <div hidden={props.hidden !== undefined ? props.hidden : false}>
            <TextInput name="street1"
                placeholder={props.street1.placeholder}
                label={props.street1.label}
                value={props.street1.value}
                onChange={props.changeHandler}
                touched={props.street1.touched ? 1 : 0}
                valid={props.street1.valid ? 1 : 0}
                errors={props.street1.errors} />
            <TextInput name="street2"
                placeholder={props.street2.placeholder}
                label={props.street2.label}
                value={props.street2.value}
                onChange={props.changeHandler}
                touched={props.street2.touched ? 1 : 0}
                valid={props.street2.valid ? 1 : 0}
                errors={props.street2.errors} />
            <div className="form-row">
                <div className="col-md-4">
                    <TextInput name="city"
                        placeholder={props.city.placeholder}
                        label={props.city.label}
                        value={props.city.value}
                        onChange={props.changeHandler}
                        touched={props.city.touched ? 1 : 0}
                        valid={props.city.valid ? 1 : 0}
                        errors={props.city.errors} />
                </div>
                <div className="col-md-4">
                    <div className="form-group">
                        <label className="font-weight-bold">State</label>
                        <select className="form-control" name="stateAbbreviation" value={props.stateAbbreviation.value} onChange={props.changeHandler}>
                            <option value="">Select One</option>
                            <option value="FL">Florida</option>
                            <option value="NY">New York</option>
                            <option value="CA">California</option>
                        </select>
                    </div>
                </div>
                <div className="col-md-4">
                    <TextInput name="zip"
                        placeholder={props.zip.placeholder}
                        label={props.zip.label}
                        value={props.zip.value}
                        onChange={props.changeHandler}
                        touched={props.zip.touched ? 1 : 0}
                        valid={props.zip.valid ? 1 : 0}
                        errors={props.zip.errors} />
                </div>
            </div>
            <TextInput name="note"
                placeholder={props.note.placeholder}
                label={props.note.label}
                value={props.note.value}
                onChange={props.changeHandler}
                touched={props.note.touched ? 1 : 0}
                valid={props.note.valid ? 1 : 0}
                errors={props.note.errors} />
            <TextInput name="scheduledAt" type="date"
                placeholder={props.scheduledAt.placeholder}
                label={props.scheduledAt.label}
                value={props.scheduledAt.value}
                onChange={props.changeHandler}
                touched={props.scheduledAt.touched ? 1 : 0}
                valid={props.scheduledAt.valid ? 1 : 0}
                errors={props.scheduledAt.errors} />
        </div>
    )
}

export default AddressForm
