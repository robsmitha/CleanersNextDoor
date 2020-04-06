
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
    var validationResult = validate(value, updatedFormElement.validationRules, updatedFormElement.label);
    updatedFormElement.valid = validationResult.isValid;
    updatedFormElement.errors = validationResult.errorMessages;

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