var GigsController = function (attendanceService) {

    var button;

    var fail = function () {
        alert("Something failed!");

    };

    var done = function () {
        button.toggleClass("btn-default").toggleClass("btn-info")
            .text(button.hasClass("btn-default") ? "Going?" : "Going");
    };





    var toggleAttendance = function (e) {
        button = $(e.target);
        var gigId = button.attr("data-gig-id");
        if (button.hasClass("btn-default")) {
            attendanceService.createAttendance(gigId, done, fail);
        } else {
            attendanceService.deleteAttendance(gigId, done, fail);
        }
    };




    var init = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
    };



    return {
        init: init
    }

}(AttendanceService);