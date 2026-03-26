$(document).ready(function () {
    $('.changeCartItemQuantityForm').submit(event => {
        event.preventDefault();

        let form = $(event.target);
        let productId = form.find('input[name="ProductId"]').val();
        let action = $(event.originalEvent.submitter).val();

        let formData = {
            ProductId: productId,
            Action: action,
        }

        let csrfToken = form.find('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: "Cart/UpdateItemQuantity",
            method: "POST",
            headers: {
                "RequestVerificationToken": csrfToken
            },
            contentType: "application/json",
            data: JSON.stringify(formData),
            success: result => {
                if (result.success) {
                    const item = $(`input[name="ProductId"][value="${productId}"]`).closest('tr');

                    if (result.quantity > 0) {
                        form.find('span').text(result.quantity);
                        item.find('.cartItemTotal').text(`$${result.total.toFixed(2)}`);

                    }
                    else if (item){
                        item.remove();

                        const cartItems = $('#cartItems tbody tr').length;

                        if (cartItems === 1) {
                            $('#cart').remove();
                            $('#emptyCartMessage').show();
                        }
                    }

                    const grandTotal = $('#cartItem_grandTotal');
                    
                    if (grandTotal.length) {
                        grandTotal.html(`<strong>$${result.grandTotal.toFixed(2)}</strong>`);
                    }

                } else{
                    console.log(result.message);
                }
            },
            error: function (xhr, status, error) {
                console.log("Error: " + xhr.responseText);
            }
        })
    })
})

$(document).ready(function () {
    $('.removeCartItemForm').submit(event => {
        event.preventDefault();

        let form = $(event.target);
        let productId = form.find('input[name="ProductId"]').val();

        let formData = {
            ProductId: productId,
        }

        let csrfToken = form.find('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: "Cart/RemoveItemFromCart",
            method: "POST",
            headers: {
                "RequestVerificationToken": csrfToken
            },
            contentType: "application/json",
            data: JSON.stringify(formData),
            success: result => {
                if (result.success) {
                    const item = $(`input[name="ProductId"][value="${productId}"]`).closest('tr');

                    item.remove();

                    const cartItems = $('#cartItems tbody tr').length;

                    if (cartItems === 1) {
                        $('#cart').remove();
                        $('#emptyCartMessage').show();
                    }

                    const grandTotal = $('#cartItem_grandTotal');
                    
                    if (grandTotal.length) {
                        grandTotal.html(`<strong>$${result.grandTotal.toFixed(2)}</strong>`);
                    }


                } else{
                    console.log(result.message);
                }
            },
            error: function (xhr, status, error) {
                console.log("Error: " + xhr.responseText);
            }
        })
    })
})