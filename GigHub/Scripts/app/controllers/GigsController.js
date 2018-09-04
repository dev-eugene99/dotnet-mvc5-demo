var GigsController = function (attendanceService, followArtistService) {
    var button;

    var init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
        $(container).on("click", ".js-toggle-follow", toggleFollowArtist);
    };


    var toggleAttendance = function (e) {
        button = $(e.target);

        var gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default"))
            attendanceService.createAttendance(gigId, onAttendDone, fail);
        else
            attendanceService.deleteAttendance(gigId, onAttendDone, fail);
    };

    var toggleFollowArtist = function (e) {
        button = $(e.target);

        var artistId = button.attr("data-artist-id");

        if (button.text().trim() === "Following")
            followArtistService.unFollowArtist(artistId, onFollowDone, fail);
        else
            followArtistService.followArtist(artistId, onFollowDone, fail);
    };

    var onAttendDone = function () {
        var text = (button.text().trim() === "Going") ? "Going ?" : "Going";
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var onFollowDone = function () {
        var text = (button.text().trim() === "Following") ? "Follow ?" : "Following";
        button.text(text);
    };

    var fail = function () {
        alert("something failed!");
    };

    return {
        init: init
    }

}(AttendanceService, FollowArtistService);