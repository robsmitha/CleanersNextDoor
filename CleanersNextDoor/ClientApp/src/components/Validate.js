const validate = (value, rules, label) => {
    let isValid = true;
    let errorMessages = []
    for (let rule in rules) {

        switch (rule) {
            case 'minLength':
                if (!minLengthValidator(value, rules[rule])) {
                    isValid = false;
                    if (value.length > 0) {
                        errorMessages.push(`The minimum length for ${label} is ${rules[rule]} characters.`);
                    }
                }
                break;

            case 'isRequired':
                if (!requiredValidator(value)) {
                    isValid = false;
                    errorMessages.push(`${label} is required.`);
                }
                break;

            case 'isEmail':
                if (!emailValidator(value)) {
                    isValid = false;
                    if (value.length > 0) {
                        errorMessages.push(`"${value}" is not a valid email address.`);
                    }
                }
                break;

            default: isValid = true;
        }

    }

    return { isValid: isValid, errorMessages: errorMessages };
}


/**
 * minLength Val
 * @param  value 
 * @param  minLength
 * @return          
 */
const minLengthValidator = (value, minLength) => {
    return value.length >= minLength;
}

/**
 * Check to confirm that feild is required
 * 
 * @param  value 
 * @return       
 */
const requiredValidator = value => {
    return value.trim() !== '';
}

/**
 * Email validation
 * 
 * @param value
 * @return 
 */
const emailValidator = value => {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(value).toLowerCase());
}


export default validate;