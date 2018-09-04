var FollowArtistService = function () {
    var followArtist = function (artistId, done, fail) {
        $.post("/api/followings", { Id: artistId })
            .done(done)
            .fail(fail);
    };

    var unFollowArtist = function (artistId, done, fail) {
        $.ajax({
            url: "/api/followings",
            data: { Id: artistId },
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        followArtist: followArtist,
        unFollowArtist: unFollowArtist
    }
}();