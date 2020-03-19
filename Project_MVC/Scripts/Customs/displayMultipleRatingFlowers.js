var arrStars = document.querySelectorAll(".stars-outer");

$.each(arrStars, function () {
    var type = $(this).data("type");
    if (type != "AtDetailView") {
        var getRating = $(this).attr("data-rating");
        if (getRating == null) {
            getRating = 0;
        }
        const starTotal = 5;

        const starPercentage = (getRating / starTotal) * 100;
        // 3
        const starPercentageRounded = `${(Math.round(starPercentage / 10) * 10)}%`;
        // 4
        $(this).find(".stars-innerDisplay").css("width", starPercentageRounded);
        previousPercentage = starPercentage;
    } 
})