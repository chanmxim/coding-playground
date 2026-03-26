$(document).ready(function () {
    
    $("#productSearch").on('keyup', function () {
        let searchString = $(this).val().trim();
        
        $.ajax({
            url: "Products/LiveProductsSearch",
            type: "GET",
            data: {searchString: searchString},
            beforeSend: function () {
                $("#productList").html("<i class=\"fa-solid fa-spinner fa-spin-pulse\"></i>");
            },
            success: function (result) {
                setTimeout(() => {
                    $("#productList").html(result);
                }, 1000);
            }
        })
    })
})
