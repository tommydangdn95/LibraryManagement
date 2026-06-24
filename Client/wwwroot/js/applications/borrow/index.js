$(function () {
    onTabClick();
    $("#nav-all-request-tab").click();
})


function onTabClick() {
    $("#nav-all-request-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(null);
    });

    $("#nav-new-request-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.submitRequest)
    });

    $("#nav-approved-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.approved)
    });

    $("#nav-borrowing-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.borrowing);
    });

    $("#nav-overdue-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.overdue);
    });

    $("#nav-returned-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.returned);
    });
    $("#nav-canceled-tab").click(async function () {
        $(this).tab('show');
        await getListBorrowRequest(BORROW_STATUS.canceled);
    });
}



async function getListBorrowRequest(borrowStatus, page = 1) {
    const table = getTableLoad(borrowStatus);
    addLoadingSpin(table);

    const url = "/borrow/getlistborrow";
    const request = {
        title: $("#searchBorrowRequest").val(),
        BorrowStatus: borrowStatus,
        Page: page
    };
    const result = await sendApiRequest(url, request, 'html');
    table.html(result);
}

function getTableLoad(borrowStatus) {
    switch (borrowStatus) {
        case BORROW_STATUS.submitRequest:
            return $("#new-request");
        case BORROW_STATUS.approved:
            return $("#approved-request");
        case BORROW_STATUS.borrowing:
            return $("#borrowing-request");
        case BORROW_STATUS.overdue:
            return $("#overdue-request");
        case BORROW_STATUS.returned:
            return $("#returned-request");
        case BORROW_STATUS.canceled:
            return $("#canceled-request");
        default:
            return $("#all-request");
    }
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
