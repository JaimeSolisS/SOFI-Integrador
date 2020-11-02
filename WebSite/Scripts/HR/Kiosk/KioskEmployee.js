// =============================================================================================================================
//  Version: 20191213
//  Author:  Luis Hernandez
//  Created Date: 13 Dic 2019
//  Description:  KioskEmployee HR JS
//  Modifications: 

//import { lang } from "moment";

// =============================================================================================================================
function SetCarouselVideoNav() {
    //para cada video cargado de crea una img del seg 5, para usar como preview
    //$('.carousel-video').find('video').each(function (i) {
    $('.SlideMediaContent').each(function (i) {
        var $vid = $(this);
        if ($vid.data("mediatype") == "video") {
            var video = $vid.get(0);
            video.currentTime = 5;
            video.onloadeddata = function () {
                //Captura de la img del video
                var canvas = document.createElement("canvas");
                canvas.width = 200;
                canvas.height = 125;
                canvas.getContext('2d').drawImage(video, 0, 0, canvas.width, canvas.height);
                //actualizacion del src del img
                var img = document.getElementById("imgpreview_" + $vid.attr("id"));
                img.src = canvas.toDataURL();
                //se actualiza el carusel, debido a la llamada async
                $('.carousel-video-nav').slick('refresh');
                video.currentTime = 0;
            };
        } else if ($vid.data("mediatype") == "image") {
            var img = document.getElementById("imgpreview_" + $vid.attr("id"));
            img.src = $vid.attr('src');
        }
    });

    //plugin de carousel con los preview
    $('.carousel-video-nav').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        asNavFor: '.carousel-video',
        dots: true,
        centerMode: true,
        focusOnSelect: true
    });

}
function registerPluginCarouselVideo() {
    // Slider Dashboard Video
    // ref : https://stackoverflow.com/questions/31521763/slick-js-and-html5-video-autoplay-and-pause-on-video
    var carouselvideo = $('.carousel-video');
    carouselvideo.on('init', function (slick) {

        carouselvideo.data('slide', 1);
        carouselvideo.slick('slickPause');

    });

    carouselvideo.on('afterChange', function (event, slick, currentSlide) {
        var i = currentSlide + 1;
        var selector = '#slideMedia' + i;

        if ($(selector).data("mediatype") == "video") {
            carouselvideo.slick('slickPause');
            carouselvideo.data('slide', i);

            $(selector).get(0).play();
            $(selector).off();
        };

        $(selector).bind("ended", function () {
            carouselvideo.slick('slickPlay');

        });
    });

    carouselvideo.on('beforeChange', function (slick, currentSlide, nextSlide) {
        var i = nextSlide + 1;
        var selector = '#slideMedia' + i;
        if ($(selector).data("mediatype") == "video") {
            $(selector).get(0).pause();
            $(selector).off();
        };
    });
}
function start_CarouselVideo() {
    var carouselvideo = $('.carousel-video');
    carouselvideo.slick('slickPlay');
}
function stop_CarouselVideo() {
    var carouselvideo = $('.carousel-video');
    var selector = '#slideMedia' + carouselvideo.data('slide');
    if ($(selector).data("mediatype") == "video") {
        $(selector).get(0).pause();
        carouselvideo.slick('slickPause');
    }
}
function EnableCarousel() {
    var carouselvideo = $('.carousel-video');
    carouselvideo.slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        autoplay: true,
        lazyLoad: 'ondemand',
        autoplaySpeed: 4000,
        fade: true,
        asNavFor: '.carousel-video-nav'
    });

    SetCarouselVideoNav();


    //registerPluginCarouselVideo();
    //start_CarouselVideo();

}
function LoadOpportunitiesProgramPrivateRecords() {
    ShowProgressBar();
    $.get("/KioskEmployee/GetOpportunitiesProgramRecords", {
        OpportunityNumber: $("#Mo_VacantNumberFilter").val(),
        EmployeeNumber: $("#EmployeeID").val()
    }).done(function (data) {
        $("#div_tbl_opportunitiesprogram").html(data);
        if ($(".private-opportunityprogram-record").length == 1) {
            $(".private-opportunityprogram-record").first().find(".details-control-oppotunityprogram").click();
        }
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function SearchPrivateRequests() {
    ShowProgressBar();
    $.get("/KioskEmployee/GetPrivateRequestsTable", {
        EmployeeID: $("#EmployeeID").val(),
        RequestNumber: $("#Mo_RequestNumberFilter").val()
    }).done(function (data) {
        $("#div_Tbl_PrivateRequests").html(data);
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function ShowFullDetail(Name, Footer) {
    // Colocar los valores
    $('#h3_Name').text(Name);
    $('#s_Footer').text(Footer);
    $('#pnl_Detail').removeClass('hidden');
    $('#a_fullscreen').click();

}
function CleanControlsFullDetail() {
    $("#pdf_file").addClass('hidden');
    $("#pdf_file").attr("src", '');
}
function UserVerificationCode() {
    var EmployeeID = sessionStorage.getItem('EmployeeID');
    if (EmployeeID != $("#EmployeeID").val()) {
        $("#main_kiosk").css("display", "none");
        ShowProgressBar();
        window.location = "/HR/Kiosk/Index";
    }
}
function ShowMessageBox(colorClass, warningClassIcon, warningMessage, message) {
    $("#message-box-generic-alert").removeClass("message-box-warning");
    $("#message-box-generic-alert").removeClass("message-box-success");
    $("#message-box-generic-alert").removeClass("message-box-info");
    $("#message-box-generic-alert-title").removeClass("fa-warning");

    $("#message-box-generic-alert").addClass(colorClass);
    $("#message-box-generic-alert-title").text(warningMessage);
    $("#message-box-generic-alert-title").addClass(warningClassIcon);

    $("#message-box-generic-alert-legend").text(message);
    $('#message-box-generic-alert').toggleClass("open");
}

function KioskEmployeeInit(LangResources) {
    UserVerificationCode();
    var mousetimeout;
    var messageboxinterval;
    var SessionTimeWarning = 0;

    function set_KioskTimers() {
        clearTimeout(mousetimeout);
        mousetimeout = null;
        SessionTimeWarning = $("#SessionTimeWarning").val();

        //Se calcula a los cuantos segundos aparecerá el mensaje, sería el tiempo total de la sesión, menos los segundos especificados
        var ConfirmationTimeMessage = ($("#CloseSessionAfter").val() * 1000);
        //var ConfirmationTimeMessage = ($("#CloseSessionAfter").val() - $("#SessionTimeWarning").val()) * 1000;

        //Si el mensaje de alerta no está aberto, se ejecuta el segundo conteo, de esta manera se evitar un freezeo en el conteo de segundos restantes del alert
        if (!($("#message-box-generic-alert").is(":visible"))) {
            mousetimeout = setTimeout(function () {
                $("#message-box-generic-alert-title").text(LangResources.lbl_TimeRunningOutSessionTitle);
                $('#message-box-generic-alert').toggleClass("open");
                $("#btn_GenericMessageBoxOK").text(LangResources.lbl_Continue);
                RemainingSessionSeconds();
            }, ConfirmationTimeMessage);
        }
    }

    function RemainingSessionSeconds() {
        /*
         * Dado a que solo puedes presionar "Continuar" en la ventana de alerta, 
         * se dio por detener los timers que estubieran corriendo para evitar cualquier tipo de 
         * comportamiento extraño no previsto
        */
        clearTimeout(mousetimeout);
        clearInterval(messageboxinterval);

        messageboxinterval = setInterval(function () {
            //Actualizar cantidad de segundos restantes en la leyenda de la alerta
            $("#message-box-generic-alert-legend").text(LangResources.lbl_TimeRunningOutSessionMsg.replace("{0}", SessionTimeWarning));

            //Resetear conteo al darle click al boton de continuar
            $("#btn_GenericMessageBoxOK").click(function () {
                clearInterval(messageboxinterval);
                set_KioskTimers();
            });

            SessionTimeWarning--;
            if (SessionTimeWarning == 0) {
                ShowProgressBar();
                clearInterval(messageboxinterval);
                //cuando la cuenta llegue a 0, se regresará a la pantalla principal del kiosco
                window.location.href = "/HR/Kiosk/Index";
                return false;
            }
        }, 1000);
    }

    set_KioskTimers();

    function SetupNumpad() {
        $.fn.numpad.defaults.gridTpl = '<table class="table modal-content"></table>';
        $.fn.numpad.defaults.backgroundTpl = '<div class="modal-backdrop in"></div>';
        $.fn.numpad.defaults.displayTpl = '<input type="password" class="form-control  input-lg" id="txt_user_code" />';
        $.fn.numpad.defaults.buttonNumberTpl = '<button type="button" class="btn-info btn-lg"></button>';
        $.fn.numpad.defaults.buttonFunctionTpl = '<button type="button" class="btn-lg" style="width: 100%;"></button>';
        $.fn.numpad.defaults.decimalSeparator = '.';
        $.fn.numpad.defaults.textDone = 'OK';
        $.fn.numpad.defaults.textDelete = LangResources.lbl_Del;
        $.fn.numpad.defaults.textClear = LangResources.lbl_Clear;
        $.fn.numpad.defaults.textCancel = LangResources.btnCancel;

        $.fn.numpad.defaults.onKeypadCreate = function () {
            $(this).find('.done').addClass('btn-success');
        };

        $('.control-numpad').numpad();
    }

    function ShowHidePagesRequests(activepage) {
        $('.custompager_public').hide();
        var PageCount = $('#PageCount_public').val();

        $('#custom_page_public_1').show();
        var rango_ini = activepage - 5;
        for (var i = activepage; i > rango_ini; i--) {
            $('#custom_page_public_' + i).show();
        }
        if (rango_ini > 1) {
            $('<li class="temp_li"><a >...</a></li>').insertAfter('#custom_page_public_1');
        }

        var rango_fin = activepage + 5;
        for (var i = activepage; i < rango_fin; i++) {
            $('#custom_page_public_' + i).show();
        }
        if (rango_fin < PageCount) {
            $('<li class="temp_li"><a >...</a></li>').insertBefore('#custom_page_public_' + PageCount);
        }
        $('#custom_page_public_' + PageCount).show();

        $('.custompager_public').removeClass("active");
        $('#custom_page_public_' + activepage).addClass("active");
    }

    function openFullscreen(elem) {
        if (elem != undefined) {
            //elem.webkitRequestFullscreen();
            //if (elem.requestFullscreen) {
            //    elem.requestFullscreen();
            //} else
            if (elem.mozRequestFullScreen) { /* Firefox */
                elem.mozRequestFullScreen();
            } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
                elem.webkitRequestFullscreen();
            } else if (elem.msRequestFullscreen) { /* IE/Edge */
                elem.msRequestFullscreen();
            }
        }
    }

    function ReloadSuggestionsTable(EmployeeID, DivID) {
        $.get("/HR/kioskEmployee/ReloadSuggestionsPrivateTable", {
            EmployeeID: EmployeeID
        }).done(function (data) {
            $(DivID).html(data);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }

    $("select").selectpicker();
    $("#div_btn_login").css("display", "none");
    $("#btn_Suggestions").css("display", "none");
    $("#div_btn_logout").css("display", "block").css("height", "100%");

    $(document).on("click", "#div_GetSuggestionModal", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/HR/KioskEmployee/GetPrivateSuggestionsModal", {
            EmployeeID: $("#EmployeeID").val()
        }).done(function (data) {
            $("#div_Generic_Modal").html(data);
            $("#mo_PrivateSuggestions").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#div_GetRequestsModal", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/HR/KioskEmployee/GetPrivateRequestsModal", {
            EmployeeID: $("#EmployeeID").val()
        }).done(function (data) {
            $("#div_Request_Modal").html(data);
            $("#mo_PrivateRequests").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_AddPrivateSuggestion", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskEmployee/GetSuggestionModal").done(function (data) {
            $("#div_Mo_Suggestions").html(data);
            $("#mo_Suggestions").modal("show");
            $("#txta_Comment").maxlength();

        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });

    $(document).on("click", "#btn_AddPrivateRequest", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskEmployee/GetWizardModal", {
            IsPublic: false
        }).done(function (data) {
            $("#div_Generic_Modal_2").html(data.View);
            $("#span_RequesterName").text($("#userinfo_name").text());
            $("#mo_RequestWizard").modal("show");
            $("#txt_User").maxlength();
            SlideForm(LangResources);

            //Paso 1: Automcompletado
            AutomcompletPrivateUserInfo();

        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });

    function AutomcompletPrivateUserInfo() {
        $.get('/HR/KioskEmployee/AutocompleteUserInfo', {
            UserCode: $("#EmployeeID").val()
        }).done(function (data) {

            $("#btn_next").css("display", "block");
            $("#ddl_Department").val(data.DepartmentID);
            $("#ddl_Shift").val(data.ShiftID);
            $(".selectpicker").selectpicker("refresh");
            $("#span_RequesterName").text(data.UserFullName);
            $("#div_RequesterName").css("display", "block");
            $("#txt_User").val($("#EmployeeID").val());
            $("#btn_next").click();

        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }

    $(document).on("click", "#btn_ClosePrivateRequest", function (e) {
        e.stopImmediatePropagation();
        var QtyCount = $(".request_row").length;
        $("#span_request_count").text(QtyCount);
        $("#mo_PrivateRequests").modal("toggle");
    });

    $(document).on("click", "#btn_ClosePrivateSuggestion", function (e) {
        e.stopImmediatePropagation();
        var QtyCount = $(".private_suggestion_row").length;
        $("#span_suggestions_count").text(QtyCount);
        $("#mo_PrivateSuggestions").modal("toggle");
    });

    $(document).on("click", "#btn_SaveComment", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.post("/KioskEmployee/SaveSuggestions", {
            KioskCommentCategoryID: $("#ddl_KioskCommentCategories option:selected").val(),
            Comment: $("#txta_Comment").val(),
            EmployeeID: $("#EmployeeID").val()
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);

            if (data.ErrorCode == 0) {
                $("#mo_Suggestions").modal("toggle");
                ReloadSuggestionsTable($("#EmployeeID").val(), "#div_Tbl_PrivateSuggestions");
            } else if (data.ErrorCode == 1) {
                $("#div_ddl_KioskCommentCategories").effect("shake", "slow");
                $("#ddl_KioskCommentCategories").effect("highlight", "slow");
            } else if (data.ErrorCode == 2) {
                var val = $("#txta_Comment").val();

                $("#txta_Comment").focus().val("").val(val);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });


    $(document).on("click", "#btn_Logout", function (e) {
        e.stopImmediatePropagation();
        SetAlertConfirmCustomBoxAction(function () {
            ShowProgressBar();
            window.location = "/HR/Kiosk/Index";
        }, LangResources.msg_LogoutConfirm, LangResources.lbl_Warning, "info");
    });

    $(document).on("click", ".delete-suggestion", function (e) {
        e.stopImmediatePropagation();
        var KioskEmployeeSuggestionID = $(this).data("kioskemployeesuggestionid");
        SetConfirmBoxAction(function () {
            $.post("/KioskSuggestionsAdministrator/Delete", {
                KioskEmployeeSuggestionID: KioskEmployeeSuggestionID
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    ReloadSuggestionsTable($("#EmployeeID").val(), "#div_Tbl_PrivateSuggestions");
                }
            });
        }, LangResources.msg_DeleteSuggestionConfirm);
    });

    //Begin Request Section
    $(document).on("click", "#btn_NewPrivateRequest", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskEmployee/GetWizardModal", {
            IsPublic: false
        }).done(function (data) {
            $("#div_RequestWizard").html(data.View);
            $("#mo_RequestWizard").modal("show");

            $("#span_RequesterName").text($("#userinfo_name").text());

            SlideForm(LangResources);

        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("change", ".RequestFilters", function (e) {
        e.stopImmediatePropagation();
        $("#div_tbl_PublicRequests").css("display", "none");
    });

    $(document).on("click", ".view-request", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskRequestAdministrator/GetViewRequestModal", {
            RequestID: $(this).data("requestid"),
            IsPublic: true
        }).done(function (data) {
            $("#root_modal").html(data);
            $("#mo_RequestView").modal("show");
            HideProgressBar();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_CloseSinglePublicRequestView", function (e) {
        e.stopImmediatePropagation();
        $("#mo_RequestView").modal("toggle");
    });

    $(document).on("click", "#finger_test", function (e) {
        e.stopImmediatePropagation();
        $.post("/KioskEmployee/TestFinger").done(function (data) {
            console.log(data);
        });
    });

    $(document).on("click", "#div_OpportunitiesProgramEmployee", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskEmployee/GetModalOpportunitiesProgram", {
            EmployeeID: $("#EmployeeID").val()
        }).done(function (data) {
            $("#div_Generic_Modal").html(data);
            $("#mo_PrivateOpportunitiesProgram").modal("show");
            HideProgressBar();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_mo_SearchVacantNumber", function (e) {
        e.stopImmediatePropagation();
        LoadOpportunitiesProgramPrivateRecords();
    });

    $(document).on("keydown", "#Mo_VacantNumberFilter", function (e) {
        e.stopImmediatePropagation();
        $("#div_tbl_opportunitiesprogram").empty();
    });

    $(document).on("click", ".details-control-oppotunityprogram", function (e) {
        e.stopImmediatePropagation();
        var tr = $(this).closest('tr');
        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {
            var OpportunityProgramID = tr.data("opportunityprogramid");
            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/KioskEmployee/GetOpportunitiesProgramDetailsRecords", {
                OpportunityProgramID: OpportunityProgramID,
                EmployeeID: $("#EmployeeID").val()
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="10" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                tr.find("span.span_NotificationsQty").first().text("0");
                HideProgressBar();
            });
        }
    });

    $('.modal').on('shown.bs.modal', function (e) {
        $('.carousel-video').resize();
    })

    $(document).on('click', '.opportunity-details-info', function (e) {
        e.stopImmediatePropagation();
        var Row = $(this).closest("tr");
        var OpportunityProgramID = Row.data("opportunityprogramid");

        ShowProgressBar();
        if (Row.data("descriptiontypevalueid") == "Image") {
            $.get("/KioskEmployee/GetOpportunityProgramMedia", {
                OpportunityProgramID: OpportunityProgramID,
                AttachmentType: "OPPORTUNITYPROGRAMMEDIA"
            }).done(function (data) {
                $("#div_OpportunityProgramMediaText").html(data);
                $("#mo_OpportunityProgramMedia").modal("show");
                EnableCarousel();

                setTimeout(function () {
                    $('.carousel-video').resize();
                }, 1000);

                HideProgressBar();
            });
        } else {
            $.get("/KioskEmployee/GetOpportunityProgramText", {
                OpportunityProgramID: OpportunityProgramID,
            }).done(function (data) {
                data = data.replace(/&lt;/g, "<").replace(/&gt;/, ">").replace(/&amp;lt;/g, "<").replace(/&amp;gt;/g, ">").replace(/&quot;/g, '"').replace(/&amp;gt;/g, ">")

                $("#div_OpportunityProgramMediaText").html(data);
                var HTMLEncode = $("#div_OpportunityProgramHTML").html();
                $("#div_OpportunityProgramHTML").empty();
                $("#div_OpportunityProgramHTML").append(HTMLEncode);
                $("#mo_OpportunityProgramText").modal("show");
                HideProgressBar();
            });
        }
    });

    $(document).on("click", "#btn_CloseOpportunitiesProgramModal", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        window.location.reload();
        //var QtyCount = $(".private-opportunityprogram-record").length;
        //$("#span_available_vacancies").text(QtyCount);
        //$("#mo_PrivateOpportunitiesProgram").modal("toggle");
    });

    $(document).on("click", "#close_opportunityprogrammedia_modal", function (e) {
        e.stopImmediatePropagation();
        $("#mo_OpportunityProgramMedia").modal("toggle");
    });

    $(document).on("click", "#close_opportunityprogramtext_modal", function (e) {
        e.stopImmediatePropagation();
        $("#mo_OpportunityProgramText").modal("toggle");
    });

    $(document).on("keydown", "#Mo_RequestNumberFilter", function (e) {
        e.stopImmediatePropagation();
        $("#div_Tbl_PrivateRequests").empty();
    });

    $(document).on("click", "#btn_mo_SearchRequestNumber", function (e) {
        e.stopImmediatePropagation();
        SearchPrivateRequests();
    });

    function ChangeRequestStatus(requestID, newStatus, Message) {
        ShowProgressBar();
        $.post("/KioskRequestAdministrator/ChangeStatusRequest", {
            RequestIDs: requestID,
            NewStatus: newStatus,
            Comments: Message
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                SearchPrivateRequests();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    }

    $(document).on("click", ".btn_TblFinishRequest", function (e) {
        e.stopImmediatePropagation();
        var RequestID = $(this).data("requestid");
        SetConfirmBoxAction(function () {
            ChangeRequestStatus(RequestID, "MarkDone", LangResources.msg_FinishedByUser);
        }, LangResources.msg_MarkRequestConfirmMessage);
    });


    $(document).on("click", ".btn_TblCancelRequest", function (e) {
        e.stopImmediatePropagation();
        var RequestID = $(this).data("requestid");
        SetConfirmBoxAction(function () {
            ChangeRequestStatus(RequestID, "Cancelled", LangResources.lbl_CancelledByUser);
        }, LangResources.msg_CancelRequestConfirmMessage);
    });

    $(document).on("click", ".details-control-private-request", function (e) {
        e.stopImmediatePropagation();
        var tr = $(this).closest('tr');
        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {
            var RequestID = tr.data("requestid");
            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/HR/KioskRequestAdministrator/GetRequestLog", {
                RequestID
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="10" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });

    $(document).on("click", ".opportunity-apply-vacancy", function (e) {
        e.stopImmediatePropagation();
        var tr = $(this).closest("tr");
        var OpportunityNumber = tr.find("td:nth-child(1)").text();
        var ConfirmMessage = LangResources.msg_ApplyVacancyConfirm.replace("#VACANCY#", OpportunityNumber);
        SetAlertConfirmCustomBoxAction(function () {
            ShowProgressBar();
            $.post("/KioskEmployee/ApplyOpportunnity", {
                OpportunityProgramID: tr.data("opportunityprogramid"),
                CandidateID: $("#EmployeeID").val()
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    LoadOpportunitiesProgramPrivateRecords();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, ConfirmMessage, LangResources.lbl_Warning, "info");
    });

    $(document).on("click", "#btn_Suggestions", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskEmployee/GetSuggestionModal").done(function (data) {
            $("#div_Mo_Suggestions").html(data);
            $("#ddl_KioskCommentCategories").selectpicker();
            $("#mo_Suggestions").modal("show");
            HideProgressBar();
        });
    });


    SetupNumpad();

    //Eventos para el logout
    $('body').on('mousemove', function () {
        set_KioskTimers();
        $('#if_link').css('pointer-events', 'auto');
        $('#pdf_file').css('pointer-events', 'auto');
    });
    $('body').on('touchmove', function () {
        set_KioskTimers();
        $('#if_link').css('pointer-events', 'auto');
        $('#pdf_file').css('pointer-events', 'auto');
    });

    openFullscreen(document.documentElement);

    $(document).on("click", "#btn_next", function (e) {
        e.stopImmediatePropagation();
        var ActiveSlideform = $(".slideform-slide.active");

        switch (ActiveSlideform.attr("id")) {
            case "slideform-1":
                if ($("#txt_User").val() == "") {
                    notification("", LangResources.msg_PleaseIdentify, "error");
                    $("#txt_User").focus();
                    $("#txt_User").effect("shake", "slow");

                } else if ($("#ddl_Department option:selected").val() == 0) {
                    notification("", LangResources.msg_ChooseADepartment, "error");
                    $("#ddl_Department").effect("shake", "slow");
                } else if ($("#ddl_Shift option:selected").val() == 0) {
                    notification("", LangResources.msg_ChooseAShift, "error");
                    $("#ddl_Shift").effect("shake", "slow");
                } else {
                    $("#btn_next_slideform").click();
                }

                break;
            case "slideform-2":
                //Verifica si ya se escogió un tipo de solicitud
                if ($(".RequestButtonActive").length > 0) {
                    $("#btn_next_slideform").click();
                } else {
                    notification("", LangResources.msg_ChooseRequestType, "error");
                }
                break;
            case "slideform-3":

                if ($("#txta_RequestDescription").val() != "") {

                } else {
                    notification("", LangResources.msg_MandatoryDescription, "error");
                }
                break;
        }

        //if ($(".slideform-slide.active").attr("id") == 'slideform-3') {
        //    $("#txta_RequestDescription").focus();
        //}

        if ($(".slideform-btn-next-mask").is(":disabled")) {
            $("#div_btn_next").css("display", "none");
            $("#div_btn_save").css("display", "block");
        }

    });

    $(document).on("click", "#btn_prev_slideform", function (e) {
        e.stopImmediatePropagation();
        $("#div_btn_next").css("display", "block");
        $("#div_btn_save").css("display", "none");
    });

    $(document).on("click", "#btn_RequisitionSave", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var RequesterUserName = null;
        var RequestUserID = null;
        var IsPublic = $(this).data("ispublic");

        if (IsPublic == "True") {
            RequesterUserName = $("#txt_User").val();
        } else {
            //RequestUserID = $("#UserCode").val();
            RequesterUserName = $("#UserCode").val();
        }

        $.post("/KioskRequestAdministrator/SaveRequest", {
            RequestTypeID: $(".RequestButtonActive").data("requesttype"),
            RequestUserID: RequestUserID,
            RequesterUserName: RequesterUserName,
            RequestDescription: $("#txta_RequestDescription").val(),
            DepartmentID: $("#ddl_Department option:selected").val(),
            ShiftID: $("#ddl_Shift option:selected").val(),
            IsPublic: false
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#mo_RequestWizard").modal("toggle");
                ShowMessageBox("message-box-success", "fa-check", LangResources.lbl_Success, data.ErrorMessage)
                SearchPrivateRequests();
            } else {
                ShowMessageBox("message-box-danger", "fa-warning", LangResources.lbl_Error, data.ErrorMessage)
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", ".RequestButton", function (e) {
        e.stopImmediatePropagation();
        $(".RequestButton").removeClass("RequestButtonActive");
        $(this).addClass("RequestButtonActive");
        $("#btn_next").click();
    });

    $(document).on("click", "#div_GetPrePayrollModal", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/HR/KioskEmployee/GetPrePayRollModal", {
            EmployeeNumber: $("#EmployeeID").val()
        }).done(function (data) {
            $("#div_Generic_Modal").html(data);
            $("#_mo_PrePayroll").modal("show");
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_ClosePrePayrollModal", function (e) {
        e.stopImmediatePropagation();
        $("#_mo_PrePayroll").modal("toggle");
    });

    $(document).on("click", "#div_PaymentReceiptsEmployee", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/HR/KioskEmployee/GetPaymentReceiptsModal", {
            EmployeeNumber: $("#EmployeeID").val()
        }).done(function (data) {
            $("#div_Generic_Modal").html(data);
            $("#_mo_PaymentReceipts").modal("show");
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_ClosePaymentReceiptsModal", function (e) {
        e.stopImmediatePropagation();
        $("#_mo_PaymentReceipts").modal("toggle");
    });

    $(document).on('click', '.view-pdf-receipt', function (e) {
        e.stopImmediatePropagation();
        var Folio = $(this).data("folio");
        ShowProgressBar();
        $.get("/HR/KioskEmployee/GetAttachmentOfReceipt", {
            EmployeeNumber: $("#EmployeeID").val(),
            Folio: Folio
        }).done(function (data) {
            var Path = window.location.protocol + "//" + window.location.host + data.AttachmentPath;
            ShowFullDetail($(this).data("receiptnumber"), "");
            $("#pdf_file").attr("src", Path);
            $("#pdf_file").removeClass('hidden');
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });

    $(document).on("click", "#btn_ReturnToRecipe", function (e) {
        e.stopImmediatePropagation();
        var fullpath = $("#pdf_file").attr("src");
        $.post("/HR/KioskEmployee/ReleaseUselessFiles", {
            EmployeeNumber: $("#EmployeeID").val(),
            Invoice: fullpath
        }).done(function (data) {
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
            CleanControlsFullDetail();
            $('#pnl_Detail').addClass('hidden');
            $('#a_fullscreen').click();
        });
    });

    $(document).on("click", ".send-attachment-receipt", function (e) {
        e.stopImmediatePropagation();

        var ReceiptNumber = $(this).data("receiptnumber").replace(" | ", " ");
        var eMailAddressTo = $("#user_email").text().trim();
        var Invoice = $(this).data("folio");

        if (eMailAddressTo != "" && eMailAddressTo != null) {
            SetAlertConfirmCustomBoxAction(function () {
                ShowProgressBar();
                $.get("/HR/KioskEmployee/SendAttachmentsReceipt", {
                    EmployeeNumber: $("#EmployeeID").val(),
                    Invoices: Invoice,
                    ReceiptNumber: ReceiptNumber,
                    eMailAddressTo: eMailAddressTo
                }).done(function (data) {
                    HideProgressBar();
                    if (data.ErrorCode == 0) {
                        notification("", LangResources.lbl_EmailReceiptSendedSuccess, data.notifyType);
                    }
                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                }).always(function () {
                    HideProgressBar();
                });
            }, LangResources.msg_IsYourEmailQuestion.replace("##EMAIL##", eMailAddressTo)
                , LangResources.lbl_EmailVerification, "info");
        } else {
            notification("", LangResources.lbl_UserWithoutEmail, "error");
        }
    });

    $(document).on("click", "#div_GetCoursesModal", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/HR/KioskEmployee/GetCoursesModal", {
            EmployeeNumber: $("#EmployeeID").val()
        }).done(function (data) {
            $("#div_Generic_Modal").html(data);
            $("#_mo_Courses").modal("show");
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_CloseCoursesModal", function (e) {
        e.stopImmediatePropagation();
        $("#_mo_Courses").modal("toggle");
    });

    $(document).on("click", "#btn_ClosePointsExchangeModal", function (e) {
        e.stopImmediatePropagation();
        $("#_mo_PointsExchange").modal("toggle");
    });

    $(document).on("click", "#div_PointsExchange", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/HR/KioskEmployee/GetUserPointsMovementModal", {
            EmployeeNumber: $("#EmployeeID").val(),
            RFC: $("#span_rfc").text().trim()
        }).done(function (data) {
            $("#div_Generic_Modal").html(data);
            $("#_mo_PointsExchange").modal("show");
            HideProgressBar();
        });
    });

    $(document).on("click", ".AddItemToList", function (e) {
        e.stopImmediatePropagation();
        var tr = $(this).closest("tr");
        tr.addClass("select-item-from-list");
        var CurrentElement = tr.find(".QtyItemOfList").first();
        AddSubstractItemsPoints(true, CurrentElement);
    });

    $(document).on("click", ".RemoveItemToList", function (e) {
        e.stopImmediatePropagation();
        var tr = $(this).closest("tr");
        tr.addClass("select-item-from-list");
        var CurrentElement = tr.find(".QtyItemOfList").first();
        AddSubstractItemsPoints(false, CurrentElement);
    });

    $(document).on("click", "#btn_GeneratePetition", function (e) {
        e.stopImmediatePropagation();
        var CurrentCostPoints = parseInt($("#span_TotalItemsPoints").text());
        if (CurrentCostPoints > 0) {
            var AvailablePoints = parseInt($("#span_RemainingPoints").text());
            if (AvailablePoints >= CurrentCostPoints) {
                SetAlertConfirmCustomBoxAction(function () {
                    GeneratePointsPetition();
                }, LangResources.lbl_PointsExchangeConfirmMessage, LangResources.lbl_PointsExchangeConfirmTitle, "info");
            } else {
                notification("", LangResources.lbl_InsufficientPoints, "error");
            }
        } else {
            notification("", LangResources.lbl_NoItemsSelected, "error");
        }
    });

    function AddSubstractItemsPoints(IsSum, CurrentElement) {
        var points = 0;
        var CurrentQty = parseInt(CurrentElement.text());
        if (IsSum) {
            CurrentElement.text(CurrentQty += 1);
        } else {
            CurrentQty = CurrentQty - 1;
            if (CurrentQty < 0) {
                CurrentQty = 0
            }
            CurrentElement.text(CurrentQty);
        }
        $(".select-item-from-list").each(function () {
            points += parseInt($(this).find(".QtyItemOfList").first().text()) * parseInt($(this).find(".ItemCostList").first().text());
        });
        $("#span_TotalItemsPoints").text(points);
    }

    function GeneratePointsPetition() {
        var listaArticulos = [];
        var clave = $("#EmployeeID").val();
        var rfc = $("#span_rfc").text().trim();

        $(".select-item-from-list").each(function () {
            var puntos = parseInt($(this).find(".QtyItemOfList").first().text()) * parseInt($(this).find(".ItemCostList").first().text());
            if (puntos > 0) {
                var articulos = {
                    clave: clave,
                    idConcepto: $(this).data("entityid"),
                    puntos: puntos
                }
                listaArticulos.push(articulos);
            }
        });

        if (listaArticulos.length > 0) {
            $.post("/HR/KioskEmployee/GeneratePointsPetition", {
                clave: clave,
                rfc: rfc,
                listaArticulos: listaArticulos
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType)
            });
        } else { notification("", LangResources.lbl_NoItemsSelected, "_nrf") }

    }

    $(document).on("click", ".inscribe-to-course", function (e) {
        e.stopImmediatePropagation();
        var CourseID = $(this).data("courseid");
        var Name = $(this).data("name");

        SetAlertConfirmCustomBoxAction(function () {
            ShowProgressBar();
            $.get("/HR/KioskEmployee/InscribeUserToCourse", {
                CourseID: CourseID,
                EmployeeNumber: $("#EmployeeID").val(),
                Name: Name
            }).done(function (data) {
                $("#div_tbl_AvailableCourses").html(data.AvailableTable);
                $("#div_tbl_InscribedCourses").html(data.InscribedTable);
                $("#AvailableCoursesCount").text(data.AvailableCoursesCount);
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.lbl_CourseInscriptionQuestion, LangResources.lbl_CourseInscription, "info");

    });

    $(document).on("click", "#btn_SendMarkedReceipts", function (e) {
        e.stopImmediatePropagation();
        var Invoices = "";
        var ReceiptsName = "";
        var eMailAddressTo = $("#user_email").text().trim();


        $(".chk_Recepit:checked").each(function () {
            Invoices += "," + $(this).data("folio");
            ReceiptsName += "," + $(this).data("receipt");
        });

        if (eMailAddressTo != "" && eMailAddressTo != null) {
            if (Invoices != "") {
                SetAlertConfirmCustomBoxAction(function () {
                    Invoices = Invoices.substr(1, Invoices.length);
                    ShowProgressBar();
                    $.post("/HR/KioskEmployee/SendAttachmentsReceipt", {
                        EmployeeNumber: $("#EmployeeID").val(),
                        Invoices: Invoices,
                        ReceiptNumber: ReceiptsName,
                        eMailAddressTo
                    }).done(function (data) {
                        HideProgressBar();
                        if (data.ErrorCode == 0) {
                            notification("", LangResources.lbl_EmailReceiptSendedSuccess, data.notifyType);
                        }
                    }).fail(function (xhr, textStatus, error) {
                        notification("", error.message, "error");
                    }).always(function () {
                        HideProgressBar();
                    });
                }, LangResources.msg_IsYourEmailQuestion.replace("##EMAIL##", eMailAddressTo)
                    , LangResources.lbl_EmailVerification, "info");
            } else {
                notification("", LangResources.lbl_SelectAtLeastOneReceipt, "error");
            }
        } else {
            notification("", LangResources.lbl_UserWithoutEmail, "error");
        }
    });

    $(document).on("click", "#div_B7", function (e) {
        e.stopImmediatePropagation();
        var justificationText = $(this).data("b7justification");
        ShowMessageBox("message-box-info", "fa-info", "    " + LangResources.lbl_Detail, justificationText);
    });
}


