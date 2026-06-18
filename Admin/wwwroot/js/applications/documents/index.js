$(function () {
    (async () => {
        await GetListDocument();
    })()

})


async function GetListDocument(page = 1) {
    const url = "/document/getlist";
    // const searchText = $('#SearchDocumentName').val();
    // const branchId = $('#BranchId').val();
    // const listDocumentType = getSelectedDocumentTypes();
    // const borrowStatus = $('input[name="BorrowStatus"]:checked').val();


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