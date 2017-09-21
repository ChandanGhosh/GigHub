var AttendanceService = function () {

    var createAttendance = function (gigId, done, fail) {
        $.ajax({
                url: "/api/attendances",
                type: "POST",
                data:
                {
                    "id": gigId
                }
            })
            .done(done)
            .fail(fail);
    };

    var deleteAttendance = function (gigId, done, fail) {
        $.ajax({
                url: "/api/attendances",
                type: "DELETE",
                data:
                {
                    "id": gigId
                }
            })
            .done(done)
            .fail(fail);
    };

    return {
        createAttendance: createAttendance,
        deleteAttendance: deleteAttendance
    };
}();
