$(function () {
    (async () => {
        await getlistDocument();
    })()

})


async function getlistDocument(page = 1) {
    addLoadingSpin($("#listDocument"));
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

async function onDeleteDocument(documentId) {
    const result = confirm("Are you sure want to delete this document ? ");
    if (!result) {
        return;
    }


    const url = "/document/delete";
    const request = {
        DocumentId: documentId
    }

    try {
        const result = await sendApiRequest(url, request);
        if (result.isSuccess) {
            Swal.fire({
                title: "Delete document successfully",
                icon: "success"
            });

            $("#borrowConfirmModal").modal('hide');

            await getlistDocument();
            return;
        }

        Swal.fire({
            title: "Could not delete document",
            text: result.message,
            icon: "error"
        });
    }
    catch (e) {
        Swal.fire({
            title: "Could not delete document",
            text: e.error,
            icon: "error"
        });
    }
}

function getSelectedDocumentTypes() {
    return $('#listDocumentType .form-check-input:checked')
        .map(function () { return $(this).val(); })
        .get();
}