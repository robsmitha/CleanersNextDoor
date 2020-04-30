
import validate from './Validate'

const handleFormIsValid = (formControls) => {
    let formIsValid = true;
    for (let inputIdentifier in formControls) {
        if (formControls[inputIdentifier].valid !== undefined) {
            formIsValid = formControls[inputIdentifier].valid && formIsValid
        } else if (Array.isArray(formControls[inputIdentifier])) {
            for (let i = 0; i < formControls[inputIdentifier].length; i++) {
                for (let key in formControls[inputIdentifier][i]) {
                    if (formControls[inputIdentifier][i][key].valid !== undefined)
                        formIsValid = formControls[inputIdentifier][i][key].valid && formIsValid
                }
            }
        }
    }
    return formIsValid
}

const getFormElementByName = (name, formControls, index) => {
    if (index === undefined && formControls[name] !== undefined)
        return formControls[name]

    for (let inputIdentifier in formControls) {
        if (Array.isArray(formControls[inputIdentifier])
            && formControls[inputIdentifier][index][name] !== undefined) {
            return formControls[inputIdentifier][index][name]
        }
    }

    return null
}

const handleChange = (name, value, formControls, index) => {
    const updatedControls = {
        ...formControls
    };
    const updatedFormElement = getFormElementByName(name, updatedControls, index)
    if (updatedFormElement === null) return;
    updatedFormElement.value = value;
    updatedFormElement.touched = true;
    updatedFormElement.errors = validate(value, updatedFormElement.validationRules, updatedFormElement.label);
    updatedFormElement.valid = updatedFormElement.errors === undefined || updatedFormElement.errors === null || updatedFormElement.errors.length === 0;
    updatedControls[name] = updatedFormElement;
    let formIsValid = handleFormIsValid(updatedControls)
    return {
        formControls: updatedControls,
        formIsValid: formIsValid
    };
}

export default handleChange