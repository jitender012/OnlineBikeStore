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
            console.log("Fetched Value:", userId);
        },
        error: function (error) {
            console.error("Error fetching data:", error);
        }
    });
}

//add or remoe item from wishlist
function AddRemoveWishlist(productId) {
    $.ajax({
        type: "POST",
        url: '/Wishlist/AddRemoveWishlist',
        data: { pId: productId },
        success: function (response) {
            if (response.success) {
                var heartIcon = $("#wishlist_" + productId);
                if (response.inWishlist) {
                    heartIcon.removeClass("far fa-heart").addClass("fa fa-heart red-heart");
                } else {
                    heartIcon.removeClass("fa fa-heart red-heart").addClass("far fa-heart");
                    $("#wishlistItemDiv_" + productId).remove();
                }
            } else {
                alert("Failed to update wishlist.");
            }
        },
        error: function () {
            alert("An error occurred while updating the wishlist.");
        }
    });
}

//Load cutomer reviews in product details page
function loadFeedbacks() {
    $('#feedbacks').empty();
    var avgRating = $("#overallRating");
    avgRating.empty();
    var customerCounter = 0;
    var starCounter = 0;

    var productId = $("#product_id").val();
    $.ajax({
        url: '/Feedback/ProductReviews',
        type: 'GET',
        data: { pId: productId },
        success: function (feedbacks) {

            $.each(feedbacks, function (index, feedback) {
                customerCounter += 1;
                starCounter += feedback.ratingValue;
                var feedbackHtml = '<div>';

                feedbackHtml += '<input type="hidden" name="feedback_id" value="' + feedback.feedback_id + '" />';

                // Add stars based on rating
                for (var i = 1; i <= 5; i++) {
                    if (i <= feedback.ratingValue) {
                        feedbackHtml += '<i class="fa fa-star" style="color:yellow"></i>';
                    } else {
                        feedbackHtml += '<i class="fa fa-star" style="color:grey"></i>';
                    }
                }

                feedbackHtml += '<br />';
                if (feedback.feedback_text != null) {
                    feedbackHtml += '<span>' + feedback.feedback_text + '</span>';
                    feedbackHtml += '<br/>';
                }

                if (feedback.image_url !== null) {
                    feedbackHtml += '<img src="' + feedback.image_url + '" style="max-width:120px;max-height:100px;" /><br />';
                }
                var dateString = feedback.date;
                var timestamp = parseInt(dateString.replace(/[^0-9]/g, ''));
                var date = new Date(timestamp);
                var formattedDate = date.toLocaleDateString();

                feedbackHtml += '<span style="font-size:13px; color: grey">' + feedback.customer_name + ', ' + formattedDate + '</span>';
                feedbackHtml += '<hr/>';
                feedbackHtml += '</div>';

                // Append the feedback to the div
                $('#feedbacks').append(feedbackHtml);

                //For edit review

                if (feedback.customer_id === userId) {
                    $("#feedbackId").val(feedback.feedback_id);
                    $('#ratingValue').val(feedback.ratingValue);
                    $('#stars').attr('value', feedback.ratingValue);
                    $('textarea[name="feedback_text"]').val(feedback.feedback_text);
                    if (feedback.image_url) {
                        $('#image_url').val(feedback.image_url);
                    }
                }

            });
            var star = '<i class="fa fa-star me-1" style="color:green"></i>'
            var avg = starCounter / customerCounter;
            var span = $('<span>').text(avg + ' ' + '(' + customerCounter + ' ratings)');

            avgRating.append(star).append(span);
        },
        error: function () {
            $('#message').html('<p style="color: red;">An unexpected error occurred.</p>');
        }
    });
}
