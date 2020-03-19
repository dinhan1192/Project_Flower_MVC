$(document).ready(function () {
    var arrStars = document.querySelectorAll(".stars-outer");

    $.each(arrStars, function () {
        var type = $(this).attr("data-type");
        if (type == "AtDetailView") {
            var getRating = $("#rating").val();
            if (getRating == null) {
                getRating = 0;
            }
            const starTotal = 5;

            // 2
            const starPercentage = (getRating / starTotal) * 100;
            // 3
            const starPercentageRounded = `${(Math.round(starPercentage / 10) * 10)}%`;
            // 4
            //if ($(".stars-inner")[0]) {
            //    document.querySelector(`.${getRating} .stars-inner`).style.width = starPercentageRounded;
            //}
            $(this).find(".stars-innerDisplay").css("width", starPercentageRounded);
            $(this).find(".stars-inner").css("width", starPercentageRounded);
            //document.querySelector(`.stars-innerDisplay`).style.width = starPercentageRounded;
            previousPercentage = starPercentage;

            $(".ratingFlower").click(function (e) {
                var thisOffset = $(this).offset().left;
                var relX = e.pageX - thisOffset;
                var perc = relX / $(this).width() * 100;
                //var perc = (e.pageX - this.offsetLeft) / $(this).outerWidth() * 100;
                getRating = starTotal / 100 * perc;

                // 2
                const starPercentage = (getRating / starTotal) * 100;
                //const ratingStarPercentage = (starPercentage / previousPercentage) * 100;
                // 3
                const starPercentageRounded = `${(Math.round(starPercentage / 10) * 10)}%`;
                // 4

                //document.querySelector(`.${rating} .stars-finalinner`).style.width = starPercentageRounded;
                $(this).find(".stars-finalinner").css("width", starPercentageRounded);

                $('#rating').val(getRating);
            })
        }
    })
});  