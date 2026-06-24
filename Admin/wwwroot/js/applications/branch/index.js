$(function () {
    (async () => {
        await getlistbranch();
    })()

})


async function getlistbranch(page = 1) {
    const url = "/branch/getlist";
    const request = {
        Page: page
    };
    const result = await sendApiRequest(url, request, 'html', 'GET');
    $("#listBranch").html(result);
}


function getSelectedDocumentTypes() {
    return $('#listDocumentType .form-check-input:checked')
        .map(function () { return $(this).val(); })
        .get();
}