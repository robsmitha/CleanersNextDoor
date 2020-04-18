
import validate from './Validate'

const handleChange = (name, value, formControls) => {

    const updatedControls = {
        ...formControls
    };

    const updatedFormElement = {
        ...updatedControls[name]
    };

    updatedFormElement.value = value;
    updatedFormElement.touched = true;
    updatedFormElement.errors = validate(value, updatedFormElement.validationRules, updatedFormElement.label);
    updatedFormElement.valid = updatedFormElement.errors === undefined || updatedFormElement.errors === null || updatedFormElement.errors.length === 0;
    updatedControls[name] = updatedFormElement;
    let formIsValid = true;
    for (let inputIdentifier in updatedControls) {
        formIsValid = updatedControls[inputIdentifier].valid && formIsValid;
    }

    return {
        formControls: updatedControls,
        formIsValid: formIsValid
    };
}

export default handleChange