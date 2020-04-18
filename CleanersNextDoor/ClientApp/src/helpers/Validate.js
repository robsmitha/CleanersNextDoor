const RULES = {
    MIN_LENGTH: 'minLength',
    IS_REQUIRED: 'isRequired',
    IS_EMAIL: 'isEmail'
}

const validate = (value, rules, label) => {
    let errorMessages = []
    for (let rule in rules) {

        switch (rule) {
            case RULES.IS_REQUIRED:
                if (!requiredValidator(value)) {
                    errorMessages.push(`${label} is required.`);
                }
                break;

            case RULES.MIN_LENGTH:
                if (!minLengthValidator(value, rules[rule])) {
                    if (value.length > 0) {
                        errorMessages.push(`The minimum length for ${label} is ${rules[rule]} characters.`);
                    }
                }
                break;

            case RULES.IS_EMAIL:
                if (!emailValidator(value)) {
                    if (value.length > 0) {
                        errorMessages.push(`"${value}" is not a valid email address.`);
                    }
                }
                break;
        }

    }

    return errorMessages;
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