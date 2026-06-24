$(function () {
    (async () => {
        await getListUser();
    })()

})


async function getListUser(page = 1) {
    const url = "/user/getlist";
    const request = {
        Page: page
    };
    const result = await sendApiRequest(url, request, 'html', 'GET');
    $("#listUser").html(result);
}