var url = 'https://localhost:5001/api'

$(document).ready(function () {
    getCategories();
    getProducts();
});

function getCategories() {
    $.getJSON(url + '/categories')
        .done(function (data) {
            $.each(data, function (key, item) {
                $('<dt>', { text: item.categoryName }).appendTo($('#categories'));
                $('<dd>', { text: '• ' + item.description }).appendTo($('#categories'));
            });
        })
        .fail(function () {
            $('<dt>', { text: 'ERROR: cannot call api' }).appendTo($('#categories'));
        });
}

function getProducts() {
    $.getJSON(url + '/products')
        .done(function (data) {
            $.each(data, function (key, item) {
                $('<dt>', { text: item.productName }).appendTo($('#products'));
                $('<dd>', { text: 'Quantity per unit: ' + item.quantityPerUnit + '; Price: ' + item.price })
                    .appendTo($('#products'));
                $('<dd>', { text: 'Supplier: ' + item.supplier.companyName + '; Category: ' + item.category.categoryName })
                    .appendTo($('#products'));
            });
        })
        .fail(function () {
            $('<dt>', { text: 'ERROR: cannot call api' }).appendTo($('#products'));
        });
}
