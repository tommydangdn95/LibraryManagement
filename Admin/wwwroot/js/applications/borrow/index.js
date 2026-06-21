let BORROW_STATUS = {
    cancel: -1,
    submitRequest: 0,
    approved: 1,
    borrowing: 2,
    returned: 3,
    overdue: 4,
    lost: 5,
}


$(function () {
    onTabClick();
    $("#nav-new-request-tab").click();

    $("#borrowConfirmModal").on('show.bs.modal', async function (e) {

        const borrowDetailId = $(this).data('borrowDetailId');
        const borrowStatus = $(this).data('borrowStatus');
        const activeBorrowStatus = $(this).data('activeBorrowStatusTab');
        const result = await getBorrowRequestDetailItem(borrowDetailId);
        $('#borrowConfirmContent').html(result);

        $("#borrowConfirmModal .saveChangebtn").off().on('click', async function (e) {
            const url = "/borrow/updateStatus";
            const request = {
                BorrowRequestId: borrowDetailId,
                BorrowStatus: borrowStatus
            }

            try {
                addLoadingSpin($("#borrowConfirmContent"));
                const result = await sendApiRequest(url, request);
                if (result.isSuccess) {
                    Swal.fire({
                        title: "Update borrow request successfully",
                        icon: "success"
                    });

                    $("#borrowConfirmModal").modal('hide');

                    await getListDocument(activeBorrowStatus);
                    return;
                }

                Swal.fire({
                    title: "Could not update borrow request document",
                    text: result.message,
                    icon: "error"
                });
            }
            catch (e) {
                Swal.fire({
                    title: "Could not update borrow request document",
                    text: e,
                    icon: "error"
                });
            }
        })
    })
})


function onTabClick() {
    $("#nav-new-request-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.submitRequest);
    });

    $("#nav-approved-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.approved)
    });

    $("#nav-cancel-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.cancel);
    });

    $("#nav-returned-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.returned);
    });

    $("#nav-all-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(null, $("#nav-all-request"));
    });
}


async function getListBorrowRequest(borrowStatus, page = 1) {
    const table = getTableLoad(borrowStatus);
    
    addLoadingSpin(table);
    const url = "/borrow/getlist";
    const request = {
        BorrowStatus: borrowStatus
    };
    const result = await sendApiRequest(url, request, 'html', 'GET');
    table.html(result);
}

function getTableLoad(borrowStatus) {
    switch (borrowStatus) {
        case BORROW_STATUS.submitRequest:
            return $("#new-request");
        case BORROW_STATUS.approved:
            return $("#nav-approved");
        case BORROW_STATUS.cancel:
            return $("#nav-cancel");
        case BORROW_STATUS.returned:
            return $("#nav-returned");
        default:
            return $("#nav-all-request");
    }
}

function onUpdateBorrowStatus(borrowId, activeStatusTab, borrowStatus) {
    const borrowModal = $("#borrowConfirmModal");
    const content = $('#borrowConfirmContent');
    addLoadingSpin(content);

    borrowModal.data('borrowDetailId', borrowId);
    borrowModal.data('activeBorrowStatusTab', activeStatusTab);
    borrowModal.data('borrowStatus', borrowStatus);
    borrowModal.modal({
        backdrop: 'static',
        keyboard: false
    });

    borrowModal.modal('show');
}

async function getBorrowRequestDetailItem(borrowRequestId) {
    const url = "/borrow/details";
    const request = {
        borrowDetailId: borrowRequestId
    };
    const result = await sendApiRequest(url, request, 'html', 'GET');
    return result;
}


function getSelectedDocumentTypes() {
    return $('#listDocumentType .form-check-input:checked')
        .map(function () { return $(this).val(); })
        .get();
}