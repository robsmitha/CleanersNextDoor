
export async function get(url) {
    return fetch(url)
        .then(handleResponse)
}

export async function post(url, data) {
    const request = {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return fetch(url, request)
        .then(handleResponse)
}

async function handleResponse(response) {
    let data = await response.json()
    if (response.ok) return data;
    if (!response.ok && response.status === 400) {
        let errors = ''
        for (let d in data) errors += data[d] + '\n'
        return errors;
    }
    return null;
}