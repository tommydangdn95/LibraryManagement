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

                    await getListBorrowRequest(activeBorrowStatus);
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

            $("#borrowConfirmModal").modal('hide');
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

    $("#nav-borrowing-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.borrowing)
    });

    $("#nav-overdue-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.overdue)
    });

    $("#nav-returned-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.returned);
    });

    $("#nav-canceled-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.canceled);
    });

    $("#nav-all-request-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(null);
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
        case BORROW_STATUS.borrowing:
            return $("#nav-borrowing");
        case BORROW_STATUS.overdue:
            return $("#nav-overdue");
        case BORROW_STATUS.returned:
            return $("#nav-returned");
        case BORROW_STATUS.canceled:
            return $("#nav-canceled");
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

async function onCancel(borrowId, activeBorrowStatus) {
    var result = confirm("Are you sure want to cancel this request?");
    if (result) {
        const url = "/borrow/updateStatus";
        const request = {
            BorrowRequestId: borrowId,
            BorrowStatus: BORROW_STATUS.canceled
        }

        await onUpdateBorrowRequestStatus(url, request, activeBorrowStatus, "Cancel borrow request successfully", "Could not cancel borrow request document");
    }
}

async function onUpdateBorrowRequestStatus(url, request, activeBorrowStatus, successMessage, errorMessage) {
    try {
        const result = await sendApiRequest(url, request);
        if (result.isSuccess) {
            Swal.fire({
                title: successMessage,
                icon: "success"
            });

            await getListBorrowRequest(activeBorrowStatus);
            return;
        }

        Swal.fire({
            title: errorMessage,
            text: result.message,
            icon: "error"
        });
    }
    catch (e) {
        Swal.fire({
            title: errorMessage,
            text: e,
            icon: "error"
        });
    }
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