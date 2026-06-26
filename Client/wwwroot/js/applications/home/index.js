$(function () {

    (async () => {
        await getListDocument();
    })()

    $("#borrowModal").on('show.bs.modal', async function (e) {
        
        const documentId = $(this).data('documentId');
        const result = await onGetModalBorrowRequest(documentId);
        $('#borrowModalContent').html(result);

        const today = new Date().toISOString().split('T')[0];
        $('#modalBorrowReturnDate').attr('min', today);
    })


    $("#borrowModal").on('click', '.submitBorrowBtn', async function (e) {
        e.preventDefault();

        const result = confirm("Are you sure borrow this document");
        if (!result) {
            return;
        }

        const url = "/Home/CreateBorrowRequest";
        const request = {
            ReturnDate: $("#modalBorrowReturnDate").val(),
            Note: $("#modalBorrowNote").val(),
            DocumentId: $("#modalBorrowDocumentId").val(),
            BranchId: $("#modalBorrowBranchId").val()
        }

        try {
            addLoadingSpin($("#borrowModalContent"));
            const result = await sendApiRequest(url, request);
            if (result.isSuccess) {
                Swal.fire({
                    title: "Create new borrow request successfully",
                    icon: "success"
                });

                $("#borrowModal").modal('hide');

                await getListDocument();
                return;
            }

            Swal.fire({
                title: "Could not create submit borrow request document",
                text: result.message,
                icon: "error"
            });

        }

        catch (e) {
            Swal.fire({
                title: "Could not create submit borrow request document",
                text: e,
                icon: "error"
            });
        }

        $("#borrowModal").modal('hide');
    })

    $("#filterBtn").on('click', async function (e) {
        e.preventDefault();
        await getListDocument();
    })
})


async function getListDocument(page = 1) {
    const table = $('#listDocument');
    addLoadingSpin(table);
    const searchText = $('#SearchDocumentName').val();
    const branchId = $('#BranchId').val();
    const listDocumentType = getSelectedDocumentTypes();
    const borrowStatuses = $('input[name="BorrowStatus"]:checked').val();
    const documentTypes = $('#listDocumentType input[type="checkbox"]:checked').map(function () {
        return $(this).val();
    }).get();
    const startDate = $('#StartDate').val();
    const endDate = $('#EndDate').val();

    const url = "/Home/GetList";
    const request = {
        BranchId: branchId,
        SearchDocumentName: searchText,
        DocumentTypes: documentTypes,
        StartDate: startDate,
        EndDate: endDate,
        Page: page
    };
    const result = await sendApiRequest(url, request, 'html');
    table.html(result);
}


function onShowBorrowRequest(documentId) {
    const borrowModal = $("#borrowModal");
    const content = $('#borrowModalContent');
    addLoadingSpin(content);

    borrowModal.data('documentId', documentId);
    borrowModal.modal({
        backdrop: 'static',
        keyboard: false
    });

    borrowModal.modal('show');
}

async function onGetModalBorrowRequest(documentId) {
    const url = "/Home/GetModalBorrowRequest";
    const request = {
        documentId
    };
    const result = await sendApiRequest(url, request, 'html', 'GET');
    return result;
}


function getSelectedDocumentTypes() {
    return $('#listDocumentType .form-check-input:checked')
        .map(function () { return $(this).val(); })
        .get();
}