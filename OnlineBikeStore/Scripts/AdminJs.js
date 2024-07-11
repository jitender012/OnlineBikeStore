$(document).ready(function () {
    getStores();
    getStockDataByStore();
    UpdateQuantity();
})

//admin layout stores dropdown
function getStores() {
    $.ajax({
        url: "/Store/GetStores",
        type: "GET",
        success: function (data) {
            console.log(data)
            var store_dd = $('#stores-dropd');
            $.each(data, function (index, item) {
                store_dd.append('<a href="/Dashboard/StoreDashboard?store_id=' + item.store_id + '">' + item.store_name + '</a>');
            });
        },
        error: function (xhr, status, error) {            
            console.error(error);
        }
    })
}

//store dashboard stock list
function getStockDataByStore() {
    var storeId = $('#store-id').val();

    $.ajax({
        url: '/Stock/GetStockInStore',
        type: 'GET',
        dataType: "json",
        data: { storeId: storeId },
        success: function (data) {
            displayStockLevel(data);
            loadProducts(data);
        },
        error: function () {
            console.error('Error occurred while loading products.');
        }
    });
}

//calculate stock level and display
function displayStockLevel(data) {
    var allinventory = 0;
    var lowinventory = 0;
    var finishedinventory = 0;

    data.forEach(function (item) {
        allinventory++;

        if (item.quantity === 0) {
            finishedinventory++;
        } else if (item.quantity <= 10) {
            lowinventory++;
        }
    });

    $('#allInventory').text(allinventory);
    $('#lowInventory').text(lowinventory);
    $('#finishedInventory').text(finishedinventory);
}

//load stock list that are in store
function loadProducts(data) {
    var tbody = $("#inventory-table-body");
    tbody.empty();  
    data.forEach(function (product, index) {
        var row = $("<tr>");
       
        row.append($("<td>").text(product.product_name));
        row.append($("<td>").text(product.model_year));
        row.append($("<td>").text(product.quantity));

        //upddate stock quantity button for each stock
        var updateQuantityBtn = $('<button>', {
            class: 'btn',
            text: 'Update',
            type: 'button',
            'data-bs-toggle': 'modal',
            'data-bs-target': '#updateStockModal'

        }).on('click', function () {
            $('#productId').val(product.product_id);
            $('#storeId').val(product.store_id);
            $('#product-name').val(product.product_name);
            $('#current-quantity').val(product.quantity);
        });;
        row.append(updateQuantityBtn);
        tbody.append(row);

    });
}

//load products that are not in store and diplay in modal
function getStockNotInStore() {
    var storeId = $('#store-id').val();

    $.ajax({
        url: '/Stock/AddStock',
        type: 'GET',
        dataType: "json",
        data: { storeId: storeId },
        success: function (data) {          
            var tbody = $("#addItemsTblBody");
            tbody.empty();
            data.forEach(function (product, index) {
                var row = $("<tr>");

                row.append($("<td>").text(product.product_name));
                row.append($("<td>").text(product.model_year));

                var quantityInput = $('<input>', {
                    type: 'number',
                    id: product.product_id,
                    name: 'quantity',                 
                    class:"qty-field"
                });
                row.append(quantityInput);
             
                tbody.append(row);

            });
        },
        error: function () {
            console.error('Error occurred while loading products.');
        }
    });
}
function UpdateQuantity() {
    $('#updateQuantityButton').click(function () {
        var productId = $("#productId").val();
        var storeId = $("#storeId").val();
        var quantity = $("#quantity").val();

        $.ajax({
            url: '@Url.Action("AddStock", "Stock")',
            type: "POST",
            dataType: "json",
            data: {
                store_id: storeId,
                product_id: productId,
                quantity: quantity
            },
            success: function (result) {
                $('#updateStockModal').modal('hide');
                getTableData();
            },
            error: function (xhr, status, error) {
                console.error('Error occurred:', error);
            }
        });
    });
};