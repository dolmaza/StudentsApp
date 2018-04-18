$(function () {
    $("#students-table-body").on("click", ".delete-btn", (function (e) {
        e.preventDefault();
        var _this = $(this);
        var url = _this.attr("href");

        bootbox.confirm(globals.textConfirmDelete,
            function (result) {
                if (result) {
                    $.ajax({
                        type: 'POST',
                        url: url,
                        data: {},
                        beforeSend: function () {
                            AjaxLoader.ShowLoader();
                        },
                        dataType: 'json',
                        success: function (response) {
                            if (response.IsSuccess) {
                                _this.closest('tr').remove();
                                toastrNotification.init(globals.textSuccess).showMessage();
                            } else {
                                toastrNotification.init(globals.textAbort, { notificationType: toastrNotification.types.error }).showMessage();
                            }
                        },
                        error: function (response) {
                            toastrNotification.init(globals.textAbort, { notificationType: toastrNotification.types.error }).showMessage();
                        },
                        complete: function () {
                            AjaxLoader.HideLoader();
                        }
                    });
                    
                }
            });
        }));

    $("#birthdate-from, #birthdate-to").datepicker({
        dateFormat: globals.formats.jquerUIDatePickerFormat,
        changeMonth: false,
        numberOfMonths: 1,
        maxDate: new Date,
        changeYear: true,
        yearRange: "-100:+0"
    });


    $('#filter-submit').click(function () {
        filterStudents();
        return false;
    });

    $('#clear-filter').click(function () {
        clearFilter();
    });
});

function filterStudents() {
    var searchPhrase = $('#search-phrase').val();
    var birthdateFrom = $('#birthdate-from').val();
    var birthdateTo = $('#birthdate-to').val();

    $.ajax({
        type: 'POST',
        url: studentsGetFilteredUrl,
        data: {
            searchPhrase: searchPhrase,
            birthdateFrom: birthdateFrom,
            birthdateTo: birthdateTo
        },
        beforeSend: function () {
            AjaxLoader.ShowLoader();
        },
        dataType: 'json',
        success: function (response) {
            if (response.IsSuccess) {
                initFilteredList(response.Data);
            } else {
                toastrNotification.init(globals.textAbort, { notificationType: toastrNotification.types.error }).showMessage();
            }
        },
        error: function (response) {
            toastrNotification.init(globals.textAbort, { notificationType: toastrNotification.types.error }).showMessage();
        },
        complete: function () {
            AjaxLoader.HideLoader();
        }
    });
}

function clearFilter() {
    $('#search-phrase').val('');
    $('#birthdate-from').val('');
    $('#birthdate-to').val('');
    filterStudents();
}

function initFilteredList(expenses) {
    var html = $('#students-template').html();
    var compiledStudentsTemplate = Template7.compile(html);
    var html = compiledStudentsTemplate(expenses);
    $('#students-table-body').empty().append(html);
}