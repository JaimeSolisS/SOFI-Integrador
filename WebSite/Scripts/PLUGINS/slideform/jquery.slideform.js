"use strict";

/**
 * Slideform - A plugin to make full height sliders with forms.
 *
 * Author: Andrés Smerkin <andres@intrepits.com>
 */
function SlideForm(LangResources, userOptions, args) {

    // The form object.
    var $form = $("#form_slide");

    // Slides List
    var $slides = $form.find('.slideform-slide');

    // The slideshows wrapper, or tracker.
    var $wrapper = null;

    // The track is where the wrapper slides in.
    var $track = null;

    // We start our current slide in 0.
    var $current = 0;

    // The max slide we've reached.
    var $max = 0;

    // Our default options. These will be overriden with the user defined options.
    var $options = {
        nextButtonText: 'OK',
        prevButtonText: 'Anterior',
        submitButtonText: 'Enviar',
        validate: null,
        submit: null
    }

    // Here we should load all user defined options into our array. Undefined
    // options will default to our main options object.
    if (typeof userOptions !== 'undefined') {
        for (var attr in userOptions) { $options[attr] = userOptions[attr]; }
    }

    // Let's initialize the slider, validations, etc.
    init();




    /*
     * Binds for checkbox and radio buttons, to enable or disable behaviors
     */
    $form.find('input[type=radio] + span, input[type=checkbox] + span').on('click', function () {

        // Getting our element behind the span.
        var input = $(this).prev();

        if ((input.attr('type') != 'radio' || !input.is(':checked')) && !input.is(':disabled')) {
            input.prop('checked', !input.is(':checked')).change();
        }

    });


    /*
     * When a value change, we need to watch the result to trigger condition updates.
     * Here probably we could get an array of form values, and then use theat
     */
    $('input, textarea, select').on('blur change', function (event) {
        triggerConditions();
    });


    // We adjust slides size on resizing.
    $(window).resize(function () {
        setSlidesHeight();
    });



    function init() {

        $form.addClass('slideform-form')
            .addClass('slideform-initialized');

        // Adding the track.
        $track = $('<div />', { class: 'slideform-track' })
            .insertBefore($form.find('.slideform-slide').eq(0));

        // Adding the wrapper.
        $wrapper = $('<div />', { class: 'slideform-wrapper' })
            .appendTo($track)
            .append($form.find('.slideform-slide'));


        setSlidesHeight();

        /*
         * If there is a validation array, and we detecte jQuery validation is
         * present, then we validate the form.
         */
        enableValidation();


        /*
         * We need to update buttons status so users can't go beyond the form
         * if they have not filled all fields correctly.
         */
        updateButtonsStatus($current);



        /*
         * We need to add the continue buttons via Javascript, so we don't bloat
         * the html file with this type of content, only referred to our work.
         */
        //$form.find('.slideform-slide').each(function (k, v) {

        //    // Setting the active slide.
        //    if (k == 0) $(this).addClass('active');

        //    if (k < $slides.length - 2) {
        //        $(this).find('.slideform-group').append('<button class="slideform-btn slideform-btn-next">' + $options.nextButtonText + '</button>');
        //    } else if (k < $slides.length - 1) {
        //        $(this).find('.slideform-group').append('<button type="submit" class="slideform-btn slideform-btn-submit"><i class="icon-check"></i>&nbsp;&nbsp;' + $options.submitButtonText + '</button>')
        //    }
        //});

        /*
         * Let's disable all inputs within elements with conditions. The only
         * way these can be enabled is if the parent conditiosn are met.
         */
        $('[data-condition]').find('input, select, textarea, button').prop('disabled', true);

    }


    $form.on('submit', function (event) {
        if ($options.submit) {
            event.preventDefault();
            $options.submit(event, $form);
        }
    });



    /*
     * Enables jQuery validation for our elements. Useful to contain all the logic
     * related to validation initialization.
     *
     * We want to keep this function private.
     */
    function enableValidation() {

        /*
         * We don't load validation if these are not defined.
         */
        if (!$options.validate || !$.validator) return;

        var data = {
            // We change where errors are shown for radio buttons and checkboxes.
            errorPlacement: function (error, element) {
                if (element.attr("type") == "radio" || element.attr('type') == 'checkbox') {
                    error.appendTo(element.closest('div'));
                } else {
                    error.insertAfter(element);
                }
            }
        }

        if (typeof $options.validate.rules !== 'undefined') {
            data.rules = $options.validate.rules;
        }

        if (typeof $options.validate.messages !== 'undefined') {
            data.messages = $options.validate.messages;
        }

        $form.validate(data);
    }



    /*
     * Go to a Specific slide.
     */
    $form.goTo = function (slide) {
        var translation = slide * (100 / $slides.length);
        var percentage = 0;

        if (slide - 1 <= 0) {
            percentage = 0;
        } else if (slide == ($slides.length - 1)) {
            percentage = 100;
        } else {
            percentage = (slide - 1) * (100 / ($slides.length - 2));
        }

        // We translate the wrapper to produce the sliding effect.
        $wrapper.css('transform', 'translateY(' + translation * -1 + '%)');

        // We also update the progress bar.
        $('.slideform-progress-bar span').css('width', percentage + '%');

        // We need to scroll top the active slide.
        $slides.filter('.active').animate({ scrollTop: 0 }, 'fast');

        // Let's set the active class.
        $slides.eq(slide).addClass('active').siblings().removeClass('active');

        // Let's set the new max if available.
        $max = (slide > $max) ? slide : $max;

        // Let's update buttons status
        updateButtonsStatus(slide);
    }

    /*
     * Going Forward to the next slide.
     */
    $form.goForward = function () {

        // First let's check validation rules and we go forward if they allow us
        if (!validateSlideInputs($current)) return;

        // If there are no more slides, we just return.
        if ($current == ($slides.length - 1)) return;

        // Let's go to the next slide.
        $form.goTo(++$current);
    }


    /*
     * Going Backwards to the previous slide.
     */
    $form.goBack = function () {
        if ($current == 0) return;

        $current--;
        $form.goTo($current);
    }


    /**
     * Sets the parent track height to all slides.
     * @return {[type]} [description]
     */
    function setSlidesHeight() {
        $form.find('.slideform-slide').css('height', $track.outerHeight());
    }


    /**
     * Updating buttons status.
     * @param  integer slideNumber the slide number
     * @return void
     */
    function updateButtonsStatus(slideNumber) {
        $form.find('.slideform-btn-next-mask').prop('disabled', (slideNumber == $slides.length - 1));

        $form.find('.slideform-btn-prev').prop('disabled', (slideNumber == 0));

        //$form.find('footer .slideform-btn-next').prop('disabled', (slideNumber >= $max));
    }


    /**
     * Validates the current slide ( active slide ) inputs. Returns true if all
     * imputs are valid.
     *
     * @param  integer slideNumber Slide Number
     * @return void
     */
    function validateSlideInputs(slideNumber) {

        if (!$.validator) return true;

        var valid = true;

        $slides.eq(slideNumber).find('input, textarea, select').each(function () {
            if (!$(this).valid()) valid = false;
        });

        return valid;
    }


    /**
     * Triggers and excecutes or hides different conditions.
     * @return {[type]} [description]
     */
    function triggerConditions() {

        var input = {};
        var array = $form.serializeArray().map(function (i) { input[i.name] = i.value });

        // watching chnges.
        $form.find('[data-condition]').each(function () {

            var valid = eval($(this).data('condition'));

            // We toggle true or false, depending if the condition is valid or not.
            $(this).find('input, textarea, select').prop('disabled', !valid);

            if (valid) {
                $(this).slideDown();
            } else {
                $(this).slideUp();
            }
        });
    }


    function callFunction(name, args) {
        var fn = '$form.' + name + '()';
        return eval(fn);
    }




    $form.on('click', '.slideform-btn', function (event) {
        event.preventDefault();
        var btn = $(this);

        if (btn.hasClass('slideform-btn-next-trt')) {
            ///////////////////////////////// Integrar este cofigo en  CaptureWizard.js /////////////////////////
            //string LenType, string Base, string Addition, string Side, string VAT, string ProductionProcess, string Line
            //var CaseControl = 1;
            //var LenType = $("#span_lentype").text();
            //var Base = $("#span_base").text();
            //var Addition = $("#span_adition").text();
            //var Side = $("#span_side").text();

            //var VAT = $("#span_vat_number").text();
            //var ProductionProcessID = $("#production_process_id").val();
            //var Line = $("#span_line").text();


            ////Valida si el producto existe
            //$.get("/MFG/DemoldDefects/ValidateToNextFase", { CaseControl, LenType, Base, Addition, Side, VAT, ProductionProcessID, Line }).done(function (data) {
            //    if (data.ErrorCode === 0) {
            //        var lensdata = data.ErrorMessage.split("|");
            //        $("#span_lenssku").text(lensdata[0]);
            //        $("#span_lensdesc").text(lensdata[1]);
            //        $("#span_lensgasket").text(lensdata[2]);
            //        $("#span_plannigqty").text(lensdata[4]);
            //        $("#ProductID").val(lensdata[5]);


            //        $.get("/MFG/DemoldDefects/FillDefectTable", {
            //            ProductionLineID: $("#span_line").data("entityid"),
            //            VATID: $("#vat_id").val(),
            //            InspectorName: $("#span_inspector").text(),
            //            ProductID: $("#ProductID").val()
            //        }).done(function (data) {
            //            var Sumatoria = 0;
            //            $("#defect_table tbody").empty();
            //            $.each(data.list, function () {
            //                Sumatoria = $(this)[0].Quantity;
            //                $("#defect_table tbody").prepend( // cambio de append
            //                    '<tr class="defect-row" data-entityid="' + $(this)[0].DemoldDefectTypeID + '">' +
            //                    '   <td>' +
            //                            $(this)[0].DemoldDefectCategoryName + ' - ' + $(this)[0].DemoldDefectTypeName +
            //                    '   </td>' +
            //                    '   <td>' +
            //                            $(this)[0].Quantity +
            //                    '   </td>' +
            //                        '<td>' +
            //                            LangResources.lbl_On + ' : ' + $(this)[0].DateLastMaintFormat +
            //                            '<br /> ' +
            //                            LangResources.lbl_By + ' : ' + $(this)[0].InspectorNameDetail +
            //                        '</td> ' +
            //                    '   <td style="width:15%; text-align:center">' +
            //                    '       <button class="btn btn-danger delete-defect" type="button" title="' + LangResources.tt_Edit + '">' +
            //                    '       <span class="glyphicon glyphicon-remove-circle"></span >' +
            //                    '       </button>' +
            //                    '   </td>' +
            //                    '</tr>'
            //                );
            //            });
            //            $("#total_defects").text(Sumatoria);
            //            $form.goForward();
            //        });

            //    } else {
            //        //notification("", data.Message, "_ntf");
            //        $("#message-box-generic-alert-title").text(LangResources.lbl_Warning);
            //        $("#message-box-generic-alert-legend").text(data.ErrorMessage);
            //        $('#message-box-generic-alert').toggleClass("open");
            //    }
            //});
            ///////////////////////////////// Integrar este cofigo en  CaptureWizard.js /////////////////////////
        }

        if (btn.hasClass('slideform-btn-next')) {
            $form.goForward();
        };

        if (btn.hasClass('slideform-btn-prev')) {
            $form.goBack();
        }

        if (btn.hasClass('slideform-btn-submit')) {
            $form.submit();
        };

    });

    $form.on('goForward', function () {
        $form.goForward();
    });

    $form.on('goBack', function () {
        $form.goBack();
    });

    $form.on('goTo', function (event, slide) {
        $form.goTo(slide);
    });
}

