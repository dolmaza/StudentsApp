$(function () {
    $('.apply-numeric-input').numericInput();

    $('.apply-date-picker').datepicker({ dateFormat: globals.formats.jquerUIDatePickerFormat, maxDate: new Date, changeYear: true, yearRange: "-100:+0" });

    $('#save-updated-student').click(function () {
        var personalNumber = $('#personal-number').val();
        var firstName = $('#first-name').val();
        var lastName = $('#last-name').val();
        var birthDate = $('#birth-date').val();
        var gender = $('#gender').val();

        $.ajax({
            type: 'POST',
            url: saveUrl,
            data: {
                PersonalNumber: personalNumber,
                FirstName: firstName,
                LastName: lastName,
                Birthdate: birthDate,
                Gender: gender
            },
            beforeSend: function () {
                AjaxLoader.ShowLoader();
                validation.hideErrors();
            },
            dataType: 'json',
            success: function (response) {
                if (response.IsSuccess) {
                    toastrNotification.init(globals.textSuccess).showMessage();
                } else if (response.Data.Errors) {
                    validation.init({ errorsJson: response.Data.Errors }).showErrors();
                }else {
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
        return false;
    });


});