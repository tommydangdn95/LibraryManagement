async function sendApiRequest(url, request, datatype = 'json', method = 'POST') {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            type: method,
            contentType: 'application/json',
            dataType: datatype,
            data: method === 'GET' ? request : JSON.stringify(request),
            success: function (data) {
                resolve(data);
            },
            error: function (xhr, status, error) {
                reject({ xhr, status, error });
            }
        });
    });
}