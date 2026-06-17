$(function () {
    (async () => {
        await GetListDocument();
    })

})


async function GetListDocument(page = 1) {
    const url = "/Home/GetList";
    const searchText = $('#SearchDocumentName').val();
    const branchId = $('#BranchId').val();
    const listDocumentType = getSelectedDocumentTypes();
    const borrowStatus = $('input[name="BorrowStatus"]:checked').val();


    const payload = {
        
    };
    const result = await sendApiRequest()
}


function getSelectedDocumentTypes() {
    return $('#listDocumentType .form-check-input:checked')
        .map(function () { return $(this).val(); })
        .get();
}