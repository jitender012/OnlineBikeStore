var userId = 0;

$(document).ready(function () {
    if (userId < 1) {
        getData();
    }
});

//Get logged in user id
function getData() {
    $.ajax({
        url: '/Account/GetUserId',
        method: 'GET',
        success: function (data) {
            userId = data;            
        },
        error: function (error) {
            console.error("Error fetching data:", error);
        }
    });
}

