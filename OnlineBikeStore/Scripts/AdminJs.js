$(document).ready(function () {
    getStores();
    getStockDataByStore();  
})

//admin layout stores dropdown
function getStores() {
    $.ajax({
        url: "/Store/GetStores",
        type: "GET",
        success: function (data) {
            var store_dd = $('#stores-dropd');
            $.each(data, function (index, item) {
                store_dd.append('<a href="/Dashboard/StoreDashboard?storeId=' + item.store_id + '">' + item.store_name + '</a>');
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
        url: '/Dashboard/GetStockInStore',
        type: 'GET',
        data: { storeId: storeId },
        success: function (data) {
            $('#_StoreDashboardContainer').html(data);
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

//GET Modal Data: load products that are not in store and diplay in modal
function getStockNotInAnyStore() {
    $.ajax({
        url: '/Stock/GetProductsNotInAnyStore',
        type: 'GET',
        dataType: "json",
        success: function (data) {

            var tbody = $("#addItemsTblBody");

            // Check if DataTable is already initialized and destroy it
            if ($.fn.DataTable.isDataTable('#addItemsTable')) {
                $('#addItemsTable').DataTable().clear().destroy();
            }
            tbody.empty();

            if (data.length === 0 || data.length < 1) {
                var noDataRow = $("<tr>").append($("<td>").attr("colspan", 3).text("All products are added!"));
                tbody.append(noDataRow);
            }
            else {
                data.forEach(function (product, index) {

                    var row =
                        `<tr>
                            <td>${product.product_name}</td>                           
                            <td>
                                <input type="number" id="${product.product_id}" name="quantity" class="form-control w-50" min="1" />
                            </td>
                            <td>
                                <button class="btn" type="button" id="add-btn-${product.product_id}"><i class="bi bi-plus-square-fill"></i></button>
                            </td>
                        </tr>`;

                    tbody.append(row);

                    $(`#add-btn-${product.product_id}`).on('click', function () {

                        var qty = $(`#${product.product_id}`).val();
                        var storeId = $("#store-id").val();

                        if (!qty || qty <= 0) {
                            alert("Please enter a valid quantity.");
                            return;
                        }

                        $.ajax({
                            url: '/Stock/AddOrUpdateStock/',
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                store_id: storeId,
                                product_id: product.product_id,
                                quantity: qty
                            },
                            success: function () {
                                $(`#add-btn-${product.product_id}`).text('Added').prop('disabled', true);
                                getStockDataByStore();
                            },
                            error: function (xhr, status, error) {
                                console.error('Error occurred:', error);
                            }
                        });
                    });

                });
            }

            $('#addItemsTable').DataTable({
                "paging": true,
                "searching": true,
                "ordering": true,
                "info": true
            });
        },
        error: function () {
            console.error('Error occurred while loading products.');
        }
    });
}
function UpdateQuantity(productId, storeId) {
   
    var quantity = $("#newQuantity").val();

    $.ajax({       
        url: '/Stock/AddOrUpdateStock',
        type: "POST",
        dataType: "json",
        data: {
            store_id: storeId,
            product_id: productId,
            quantity: quantity
        },
        success: function (result) {
            $('#updateStockModal').modal('hide');
            getStockDataByStore();
            alert("Quantity updated")
        },
        error: function (xhr, status, error) {
            console.error('Error occurred:', error);
        }
    });

};