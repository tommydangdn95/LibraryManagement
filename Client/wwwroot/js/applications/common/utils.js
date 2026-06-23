var BORROW_STATUS = {
    canceled: -1,
    submitRequest: 0,
    approved: 1,
    borrowing: 2,
    returned: 3,
    overdue: 4,
    lost: 5,
}


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


function addLoadingSpin(element) {
    const loading = `
        <div id="loadingSpinner" class="text-center">
                <div class="spinner-border text-primary" role="status" style="width:2.5rem;height:2.5rem;">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p class="text-muted small mt-2 mb-0">Loading...</p>
            </div>
    `;
    element.html(loading);
}