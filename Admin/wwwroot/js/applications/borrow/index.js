$(function () {
    (async () => {
        await getListBorrowRequest();
    })()

})


async function getListBorrowRequest(page = 1) {
    const url = "/borrow/getlist";
    const request = {

    };
    const result = await sendApiRequest(url, request, 'html', 'GET');
    $("#listDocument").html(result);
}


function getSelectedDocumentTypes() {
    return $('#listDocumentType .form-check-input:checked')
        .map(function () { return $(this).val(); })
        .get();
}