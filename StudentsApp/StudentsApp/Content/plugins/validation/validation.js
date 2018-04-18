var validation = {
    errorsJson: null,
    
    init: function(options) {
        if (options) {
            validation.errorsJson = options.errorsJson ? options.errorsJson : validation.errorsJson;
        }

        return validation;
    },

    showErrors: function() {
        
        $.each(validation.errorsJson,
            function(index, error) {
                var selector = '.code-' + error.Code;

                $(selector).addClass('has-error');
                $(selector).find('.error-text').text(error.ErrorMessage);
            });
        
    },

    hideErrors: function() {
        $('.has-error .error-text').text('');
        $('.has-error').removeClass('has-error');
    }
};