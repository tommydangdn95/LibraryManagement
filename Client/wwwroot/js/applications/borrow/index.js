$(function () {

    (async () => {
        await getListDocumentBorrow();
    })()

})


async function getListDocumentBorrow(page = 1) {
    const table = $('#borrowList');
    addLoadingSpin(table);

    const url = "/Borrow/GetList";
    const request = {

    };
    const result = await sendApiRequest(url, request, 'html', 'GET');
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
