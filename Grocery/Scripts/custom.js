$(function () {
    //$("#tblProducts").DataTable();
    $("#tblProducts").on("click", ".btnDeleteProduct", function () {
        var btn = $(this);
        bootbox.confirm("Are you sure you want to delete this product?", function (result) {
            if (result) {
                var id = btn.data("id");
                $.ajax({
                    type: "POST",
                    url: "/Product/Delete/" + id,
                    succes: function () {
                        btn.closest("tr").remove();
                    }
                });
                location.reload();
            }
        })
    });
});
//$(function () {
//    $("#tblCart").on("click", ".RemoveLink", function () {
//        var btn = $(this);
//        bootbox.confirm("Are you sure you want to delete this product?", function (result) {
//            if (result) {
//                var id = btn.attr("data-id");
//                $.ajax({
//                    type: "POST",
//                    url: "/ShoppingCart/RemoveFromCart" + id,
//                    succes: function () {
//                        btn.closest("tr").remove();
//                    }
//                });
//                location.reload();
//            }
//        })
//    });
//});
$(function () {
    // Document.ready -> link up remove event handler
    $(".RemoveLink").click(function () {
        // Get the id from the link
        var recordToDelete = $(this).attr("data-id");
        if (recordToDelete != '') {
            // Perform the ajax post
            $.post("/ShoppingCart/RemoveFromCart", { "id": recordToDelete },
                function (data) {
                    // Successful requests get here
                    // Update the page elements
                    if (data.ItemCount == 0) {
                        $('#row-' + data.DeleteId).fadeOut('slow');
                    } else {
                        $('#item-count-' + data.DeleteId).text(data.ItemCount);
                    }
                    $('#cart-total').text(data.CartTotal);
                    $('#update-message').text(data.Message);
                    $('#cart-status').text('Cart (' + data.CartCount + ')');
                });
        }
    });
});
