// =============================================================================================================================
//  Version: 20191202
//  Author:  Luis Hernandez
//  Created Date: 2 Dic 2019
//  Description:  Kiosk HR JS
//  Modifications: 
// =============================================================================================================================
function ChangeTabs(evt, backSymbol) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(backSymbol).style.display = "block";
    evt.currentTarget.className += " active";
}
function SetupNumpad(Resources) {
    $.fn.numpad.defaults.gridTpl = '<table class="table modal-content"></table>';
    $.fn.numpad.defaults.backgroundTpl = '<div class="modal-backdrop in"></div>';
    $.fn.numpad.defaults.displayTpl = '<input type="password" class="form-control  input-lg" id="txt_user_code" />';
    $.fn.numpad.defaults.buttonNumberTpl = '<button type="button" class="btn-info btn-lg"></button>';
    $.fn.numpad.defaults.buttonFunctionTpl = '<button type="button" class="btn-lg" style="width: 100%;"></button>';
    $.fn.numpad.defaults.decimalSeparator = '.';
    $.fn.numpad.defaults.textDone = 'OK';
    $.fn.numpad.defaults.textDelete = Resources.lbl_Del;
    $.fn.numpad.defaults.textClear = Resources.lbl_Clear;
    $.fn.numpad.defaults.textCancel = Resources.btnCancel;

    $.fn.numpad.defaults.onKeypadCreate = function () {
        $(this).find('.done').addClass('btn-success');
    };

    $('.control-numpad').numpad();
}
function NextScreenVideo() {
    var carouselvideo = $('.carousel-video');

    // El carrusel inicia en indice 0
    var seq = carouselvideo.slick('slickCurrentSlide') + 2; //parseInt($('#hd_CurrentSlideVideSeq').val()) + 1;

    // Mover tambien el carrusel al siguiente video y pausarlo (al establecer el siguiente se hacce play solo)
    carouselvideo.slick('slickNext');
    setTimeout(function () { stop_CarouselVideo(); }, 100);
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
function ResetDashboardAreas(Name, ParentID) {
    ShowProgressBar();
    // Recargar zona de Dashboard Areas

    $.get("/HR/Kiosk/GetAreas"
        , { ParentID: ParentID })
        .done(function (data) {
            $('#div_DashboardAreas').html('');
            $('#div_DashboardAreas').html(data);

            $("#div_FilterAlert").removeClass('hidden');
            HideProgressBar();
        });
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
function computePostionDashboardVideo() {
    var div_boxPanelRight = $('#div_boxPanelRight');
    var div_pluginClock = $('#div_pluginClock');
    var carouselvideo = $('.carousel-video');
    var carosuelVideoContainer = $('#div_boxCarouselVideoContainer');

    var videoHeight = carosuelVideoContainer.height(); // Video Height
    var mainPanelHeight = div_boxPanelRight.height(); // 100%
    var div_pluginClockHeight = div_pluginClock.height();
    var h = parseInt(mainPanelHeight - videoHeight - div_pluginClockHeight);
    var result = Math.abs(parseInt(h / 2));
    //carosuelVideoContainer.css({ "margin-top": result.toString() + "px" });
}
function CleanControlsFullDetail() {
    $("#pdf_file").addClass('hidden');
    $("#pdf_file").attr("src", '');
    $("#div_link").addClass('hidden');
    $("#div_link").html('');
    $("#div_video").addClass('hidden');
    $('#v_video').get(0).pause();
    $("#i_detail").addClass('hidden');
    $("#i_detail").attr("src", '');
    //$('#div_Gallery').html('');
    $("#div_Gallery").addClass('hidden');
    try {
        $('.SliderFade').slick('unslick');
    }
    catch (e) {
        console.log('No sick available');
    }
}
function ShowFullDetail(Name, Footer) {
    // Ocultar todos los controles
    CleanControlsFullDetail();
    PreviewDocument(true);

    // Colocar los valores
    $('#h3_Name').text(Name);
    $('#s_Footer').text(Footer);
    $('#pnl_Detail').removeClass('hidden');
    $('#a_fullscreen').click();

    //TODO:Solucionar el video pause
    stop_CarouselVideo();
    closeWindowAfterTimer_Active = true;
}
function SetVideo(sourcepath) {
    var videoID = 'v_video';
    var sourceID = 's_video';
    var newposter = 'media/video-poster2.jpg';

    $('#' + videoID).get(0).pause();
    $('#' + sourceID).attr('src', sourcepath);

    $('#' + videoID).get(0).load();
    //$('#'+videoID).attr('poster', newposter); //Change video poster
    $('#' + videoID).get(0).play();

    $("#div_video").removeClass('hidden');
}
function SetGallerySettings(sourcepath) {

    $.get("/HR/Kiosk/GetGalleryDetail",
        {
            SourcePath: sourcepath
        })
        .done(function (data) {
            $(".SliderFade").html(data);

            $(".SliderFade").slick({
                centerMode: true,
                centerPadding: '60px',
                slidesToShow: 1,
                responsive: [
                    {
                        breakpoint: 768,
                        settings: {
                            arrows: false,
                            centerMode: true,
                            centerPadding: '80px',
                            slidesToShow: 1
                        }
                    },
                    {
                        breakpoint: 480,
                        settings: {
                            arrows: false,
                            centerMode: true,
                            centerPadding: '80px',
                            slidesToShow: 1
                        }
                    }
                ]
            });
            $('.image-full-detail').height($('#body_pnl_Detail').height());
        });
}
function PreviewDocument(IsPreview) {
    var div_boxPanelRight = $('#div_boxPanelRight');
    var div_boxPanelLeft = $('#div_boxPanelLeft');

    if (IsPreview) {
        div_boxPanelLeft.removeClass("split");
        div_boxPanelLeft.removeClass("left");

        div_boxPanelRight.removeClass("split");
        div_boxPanelRight.removeClass("right");
        div_boxPanelRight.addClass("hidden");
    } else {
        div_boxPanelLeft.addClass("split");
        div_boxPanelLeft.addClass("left");

        div_boxPanelRight.addClass("split");
        div_boxPanelRight.addClass("right");
        div_boxPanelRight.removeClass("hidden");

    }
}
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
function PanelCollapseFunction() {
    $(".panel-collapse").on("click", function (e) {
        e.stopImmediatePropagation();
        panel_collapse($(this).parents(".panel"));
        $(this).parents(".dropdown").removeClass("open");
        return false;
    });
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
function SearchRequestData(PageNumber, IsPublic, ClaveEmpleado) {
    //variables
    var RequestTypeIDs = $("#ddl_RequestTypes").val();
    var RequestStatusIDs = $("#ddl_RequestStatus").val();
    var DepartmentIDs = $("#ddl_RequestDepartment").val();
    var StartDate = $("#txt_RequestStartDate").val();
    var EndDate = $("#txt_RequestEndDate").val();

    //validaciones

    if (RequestTypeIDs != null) {
        RequestTypeIDs = $("#ddl_RequestTypes").val().join();
    }
    if (RequestStatusIDs != null) {
        RequestStatusIDs = $("#ddl_RequestStatus").val().join();
    }
    if (DepartmentIDs != null) {
        DepartmentIDs = $("#ddl_RequestDepartment").val().join();
    }

    ShowProgressBar();
    $.get("/KioskEmployee/SearchRequestData", {
        RequestNumber: $("#txt_RequestNumber").val(),
        RequestTypeIDs: RequestTypeIDs,
        RequestStatusIDs: RequestStatusIDs,
        RequestUserID: ClaveEmpleado,
        StartDate: StartDate,
        EndDate: EndDate,
        DepartmentIDs: DepartmentIDs,
        EmployeeInfo: $("#txt_RequestEmployee").val(),
        RequestDescription: $("#txt_RequestDescription").val(),
        IsPublic: IsPublic,
        PageNumber: PageNumber,
        OnlyRead: true
    }).done(function (data) {
        $("#div_tbl_PublicRequests").html(data);
        $("#div_tbl_PublicRequests").css("display", "block");
        ShowHidePagesRequests(PageNumber);

        $(".request_row").each(function () {
            if ($(this).data("requestnumber") == $("#txt_RequestNumber").val()) {
                console.log($(this).find(".details-control"));
                $(this).find(".details-control").click();
            }
        });

    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
        //$(".content-frame").css("height", "100vh");
    });
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

function IndexInit(Resources) {

    var mousetimeout;
    var screensaver_active = false;
    var idletime = Resources.ScreenSaverInterval;

    var fn_CloseWindowAfter;
    var closeWindowAfterTimer_Active = false;
    var closewindow_afterTimer = Resources.CloseWindowAfterIterval;

    SetupNumpad(Resources);
    //closewindow_afterTimer = Resources.CloseWindowAfterIterval;


    $(document).on("click", "#btn_NewPublicRequest", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskEmployee/GetWizardModal", {
            IsPublic: true
        }).done(function (data) {
            $("#div_RequestWizard").html(data.View);
            $("#mo_RequestWizard").modal("show");
            $("#txt_User").maxlength();
            $("#ddl_Department").selectpicker();
            $("#ddl_Shift").selectpicker();
            SlideForm(Resources);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $('#mo_Welcome').modal({
        backdrop: 'static',
        keyboard: false
    });

    // Cerrar panel de detalle
    $('.containerpanel-remove').on('click', function () {
        CleanControlsFullDetail();
        PreviewDocument(false);

        closeWindowAfterTimer_Active = false;
        clearTimeout(fn_CloseWindowAfter);
        fn_CloseWindowAfter = null;

        $('#pnl_Detail').addClass('hidden');
        $('#a_fullscreen').click();

        registerPluginCarouselVideo();
        // computePostionDashboardVideo();
        start_CarouselVideo();
    });

    $(document).on('click', '.CI_PDFFile', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var sourcepath = $(this).data('sourcepath');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        ShowFullDetail(Name, Footer);

        $("#pdf_file").attr("src", sourcepath);
        stop_CarouselVideo();
        $("#pdf_file").removeClass('hidden');
    });

    $(document).on('click', '.CI_Link', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var href = $(this).data('href');
        var IsWindow = $(this).data('iswindow');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }


        if (IsWindow == 'True') {
            window.open(href, '', 'width=1000, height=600');
            $('#mo_Welcome').modal({
                backdrop: 'static',
                keyboard: false
            });
        } else {
            // Mostrar area de detalle
            ShowFullDetail(Name, Footer);
            $("#div_link").html('<iframe id="iframe_id" sandbox="allow-presentation allow-same-origin allow-scripts allow-forms allow-modals allow-top-navigation allow-top-navigation-by-user-activation" src="' + href + '" style="width:100%;height:100%;" />');
            $("#div_link").removeClass('hidden');
        }
        stop_CarouselVideo();
        //desactiva cursor, mouse over de inicio 
        //$('#if_link').css('pointer-events', 'none');
    });

    $(document).on('click', '.CI_Video', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var sourcepath = $(this).data('sourcepath');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        //stop_CarouselVideo();
        ShowFullDetail(Name, Footer);
        SetVideo(sourcepath);

    });

    $(document).on('click', '.CI_Image', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var sourcepath = $(this).data('sourcepath');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        ShowFullDetail(Name, Footer);

        $("#i_detail").attr("src", sourcepath);
        stop_CarouselVideo();
        $("#i_detail").removeClass('hidden');
    });

    $(document).on('click', '.CI_Gallery', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var sourcepath = $(this).data('sourcepath');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        ShowFullDetail(Name, Footer);

        // TODO: Colocar todo el contenido dinamico
        //$('#div_Gallery').html('');
        stop_CarouselVideo();
        $("#div_Gallery").removeClass('hidden');
        SetGallerySettings(sourcepath);
    });

    $('#welcome-close').on('click', function () {
        var elem = document.documentElement;
        openFullscreen(elem);
        registerPluginCarouselVideo();
        computePostionDashboardVideo();
    });

    $('#refresh-dashboard').on('click', function () {
        location.reload();
    });

    $(document).on("click", "#btn_Login", function () {
        ShowProgressBar();
        $.get("/HR/Kiosk/GetModalLoginOptions").done(function (data) {
            $("#div_Login").html(data);

            ChangeTabs(event, "user_fingerprint");
            $("#tab_user_fingerprint").addClass("active");

            $("#txt_UserNum").focus();
            $(".selectpicker").selectpicker();

            $("#mo_LoginOption").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", ".LoginNumPad", function () {
        var KeyValue = $(this).data("numvalue");
        if (KeyValue == "C") {
            $("#txt_UserNum").val("");
            $("#txt_UserNum").focus();
        } else if (KeyValue == "OK") {
            $("#btn_Login").text(Resources.title_Login);
            ShowProgressBar();
            $.post("/KioskEmployee/LoginValidation", {
                UserCode: $("#txt_UserNum").val()
            }).done(function (data) {
                if (data.IsValid) {
                    $("#user_validation").css("display", "none");
                    $("#finger_scan_validation").css("display", "block");
                    sessionStorage.setItem('EmployeeID', $("#txt_UserNum").val());

                    setTimeout(function () {
                        ShowProgressBar();
                        window.location = "/HR/KioskEmployee/Login?UserCode=" + $("#txt_UserNum").val()
                    }, 5 * 1000);

                } else {
                    $("#message-box-generic-alert-title").text(Resources.lbl_Warning);
                    $("#message-box-generic-alert-legend").text(Resources.msg_InvalidUser);
                    $('#message-box-generic-alert').toggleClass("open");
                }
            }).always(function () {
                HideProgressBar();
            });
        } else {
            if ($("#txt_UserNum").val().length < 20) {
                $("#txt_UserNum").val($("#txt_UserNum").val() + KeyValue);
                $("#txt_UserNum").focus();
            }
        }
    });

    $(document).on("click", "#btn_ActiveDiretoryLogin", function () {
        ShowProgressBar();
        $.post("/KioskEmployee/LoginWithActiveDirectory", {
            Domain: $("#ddl_UsrDomain option:selected").text(),
            User: $("#txt_UsrName").val(),
            Password: $("#txt_UsrPass").val()
        }).done(function (data) {
            if (data.isValid) {
                if (data.UserCode != "" && data.UserCode != null) {
                    sessionStorage.setItem('EmployeeID', data.UserCode);
                    window.location = "/HR/KioskEmployee/Login?UserCode=" + data.UserCode
                } else {
                    notification("", Resources.lbl_UserWithoutEmployeeNumber, "error");
                }
            } else {
                notification("", Resources.lbl_UserPasswordIncorrect, "error");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", ".user_password_field", function () {
        $(".user_password_field").css("background-color", "transparent")
        $(this).css("background-color", "#ffde61")
    });

    var carouselvideo = $('.carousel-video');
    carouselvideo.slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        autoplay: true,
        lazyLoad: 'ondemand',
        autoplaySpeed: ($("#CarouselTransitionTime").val() * 1000),
        fade: true,
        asNavFor: '.carousel-video-nav',
        customPaging: function (slider, i) {
            var title = $(slider.$slides[i]).data('title');
            return '<small>' + title + '</small>';
        },
    }).on("afterChange", function (e, slick) {
        //Se debe detener primero la reproducción, porque la transición es mas rápida que el ajuste de valores
        carouselvideo.slick("setOption", "autoplay", false);
        var slideTime = $('div[data-slick-index="' + slick.currentSlide + '"]').first().find(".SlideMediaContent").data("transitiontime")
        carouselvideo.slick("setOption", "autoplaySpeed", slideTime * 1000);
        carouselvideo.slick("setOption", "autoplay", true);
    });

    //function set_KioskTimers() {
    //    //console.log('set_KioskTimers....');
    //    clearTimeout(mousetimeout);
    //    mousetimeout = null;

    //    clearTimeout();//reset timers
    //    mousetimeout = setTimeout(function () {
    //        //show_screensaver();
    //        console.log("TODO: Aqui se colocaria la funcion de logout despues de cierto tiempo de inactividad");
    //    }, 1000 * idletime); // 5 secs	

    //}

    // #endregion

    SetCarouselVideoNav();

    $("#div_btn_login").css("height", "100%");

    $(document).on("click", "#btn_Suggestions", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskEmployee/GetSuggestionModal").done(function (data) {
            $("#div_Mo_Suggestions").html(data);
            $("#ddl_KioskCommentCategories").selectpicker();
            $("#mo_Suggestions").modal("show");
            $("#txta_Comment").maxlength();
            HideProgressBar();
        });
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
            } else if (data.ErrorCode == 1) {
                $("#div_ddl_KioskCommentCategories").effect("shake", "slow");
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

    $(document).on("shown.bs.modal", "#mo_RequestWizard", function (e) {
        e.stopImmediatePropagation();
        $("#txt_User").focus();
    });

    //como son varios los slideform que tienen validaciones, esta el case, para controlarlas individualmente
    $(document).on("click", "#btn_next", function (e) {
        e.stopImmediatePropagation();
        var ActiveSlideform = $(".slideform-slide.active");

        switch (ActiveSlideform.attr("id")) {
            case "slideform-1":
                if ($("#txt_User").val() == "") {
                    notification("", Resources.msg_PleaseIdentify, "error");
                    $("#txt_User").focus();
                    $("#txt_User").effect("shake", "slow");

                } else if ($("#ddl_Department option:selected").val() == 0) {
                    notification("", Resources.msg_ChooseADepartment, "error");
                    $("#ddl_Department").effect("shake", "slow");
                } else if ($("#ddl_Shift option:selected").val() == 0) {
                    notification("", Resources.msg_ChooseAShift, "error");
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
                    notification("", Resources.msg_ChooseRequestType, "error");
                }
                break;
            case "slideform-3":

                if ($("#txta_RequestDescription").val() != "") {

                } else {
                    notification("", Resources.msg_MandatoryDescription, "error");
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
        var RequestUserID = null;

        $.post("/KioskRequestAdministrator/SaveRequest", {
            RequestTypeID: $(".RequestButtonActive").data("requesttype"),
            RequestUserID: RequestUserID,
            RequesterUserName: $("#txt_User").val(),
            RequestDescription: $("#txta_RequestDescription").val(),
            DepartmentID: $("#ddl_Department option:selected").val(),
            ShiftID: $("#ddl_Shift option:selected").val(),
        }).done(function (data) {
            //notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                $("#mo_RequestWizard").modal("toggle");
                ShowMessageBox("message-box-success", "fa-check", Resources.lbl_Success, data.ErrorMessage)
            } else {
                ShowMessageBox("message-box-danger", "fa-warning", Resources.lbl_Error, data.ErrorMessage)
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

    $(document).on("click", ".check-range-date", function (e) {
        e.stopImmediatePropagation();
        //EnableDateFilterRequest();
    });

    $(document).on("change", ".FieldRequestWizard1", function (e) {
        e.stopImmediatePropagation();
        HideProgressBar();
        var Array = $(".FieldRequestWizard1");
        var Count = 0;
        $.each(Array, function (k, v) {
            if (v.value != null && v.value != "" && v.value != 0) {
                Count++;
            }
        });
        //if (Count == 3) {
        //    $("#btn_next").click();
        //}
    });

    $(document).on("click", "#btn_ClosePublicRequestsView", function (e) {
        e.stopImmediatePropagation();
        $("#mo_PublicRequisitionList").modal("toggle");
    });

    $(document).on("click", "#btn_CheckPublicRequest", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskEmployee/GetModalRequisitions", {
            IsPublic: true,
            OnlyRead: true
        }).done(function (data) {
            $("#div_PublicRequisitionList").html(data);
            $("#mo_PublicRequisitionList").modal("show");

            $("#ddl_RequestTypes").selectpicker('selectAll');

            $("select").selectpicker("refresh");
            $(".customdatepicker").datepicker({
                autoclose: true,
                format: 'yyyy-mm-dd',
                language: Resources.culture
            });

            ShowHidePagesRequests(1);
            $(".max-length").maxlength();

            PanelCollapseFunction();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_RequestSearch", function (e) {
        e.stopImmediatePropagation();
        var PageNumber = 1;
        var Isublic = $(this).data("ispublic");
        var ClaveEmpleado = $(this).data("claveempleado");
        SearchRequestData(PageNumber, Isublic, ClaveEmpleado);
    });

    $(document).on("click", ".custompager", function () {
        var PageNumber = $(this).data("page");
        var Isublic = $(this).data("ispublic");
        var ClaveEmpleado = $(this).data("claveempleado");
        SearchRequestData(PageNumber, Isublic, ClaveEmpleado);
    });

    //Muestra el loader  en lugares que no son
    $(document).on("keyup", "#txt_RequestNumber", function (e) {
        e.stopImmediatePropagation();
        if ($("#txt_RequestNumber").val().length >= 3) {
            $.get("/KioskRequestAdministrator/GetRequestNumbers", {
                RequestNumber: $("#txt_RequestNumber").val()
            }).done(function (data) {
                $("#dtl_RequestNumbers").empty();
                $.each(data.RequestNumbersList, function (k, v) {
                    $("#dtl_RequestNumbers").append('<option>' + v.RequestNumber + '</option>')
                });
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {

            });
        }
    });

    $(document).bind("input", "#txt_RequestNumber", function (e) {
        if (e.which != 0) {
            SearchRequestData(1, true, null);
            $("#txt_RequestNumber").blur();

        } else {
            e.stopImmediatePropagation();
            $("#div_tbl_PublicRequests").css("display", "none");
            $(".content-frame").css("height", "100vh");
        }
    });

    //expandir detalles con plugin DataTable
    $(document).on('click', 'td.details-control', function (e) {
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
                tr.after('<tr><td colspan="10" style="padding-bottom: 0em; padding-top: 0em; padding-right: 0em;">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {

            });
        }
    });

    $(document).on("change", "#ddl_KioskCommentCategories", function (e) {
        e.stopImmediatePropagation();
        $("#txta_Comment").focus();
    });

    $(document).on("click", "#txt_UserNum", function (e) {
        e.stopImmediatePropagation();
        $("#txt_UserNum").css("background-color", "#ffde61");
    });

    $(document).on("blur", "#txt_UserNum", function (e) {
        e.stopImmediatePropagation();
        $("#txt_UserNum").css("background-color", "transparent");
    });

    $(document).on("keyup", "#txt_User", function (e) {
        e.stopImmediatePropagation();
        var UserCode = $(this).val();
        if (UserCode.length >= 5) {
            ShowProgressBar();
            $.get('/HR/KioskEmployee/AutocompleteUserInfo', {
                UserCode: UserCode
            }).done(function (data) {

                if (data.IsValidUser) {
                    $("#btn_next").css("display", "block");
                    $("#ddl_Department").val(data.DepartmentID);
                    $("#ddl_Shift").val(data.ShiftID);
                    $(".selectpicker").selectpicker("refresh");
                    $("#span_RequesterName").text(data.UserFullName);
                    $("#div_RequesterName").css("display", "block");
                } else {
                    $("#btn_next").css("display", "none");
                    $("#div_RequesterName").css("display", "none");
                }

            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }
    });

    $(document).on("shown.bs.modal", "#mo_LoginOption", function (e) {
        e.stopImmediatePropagation();
        $("#txt_UserNum").focus();
        $("#txt_UserNum").css("background-color", "#ffde61");
        $(".max-length").maxlength();
    });

    $(document).on("click", "#tab_user_password", function (e) {
        e.stopImmediatePropagation();
        if ($("#txt_UsrName").val() == "") {
            $("#txt_UsrName").focus();
        } else if ($("#txt_UsrPass").val() == "") {
            $("#txt_UsrPass").focus();
        }
    });

    $(document).on("keydown", "#txt_UsrName", function (e) {
        e.stopImmediatePropagation();
        if (e.keyCode == 13) {
            if ($("#txt_UsrPass").val() == "") {
                $("#txt_UsrPass").focus();
            } else {
                $("#btn_ActiveDiretoryLogin").click();
            }
        }
    });

    $(document).on("keydown", "#txt_UsrPass", function (e) {
        e.stopImmediatePropagation();
        if (e.keyCode == 13) {
            if ($("#txt_UsrName").val() != "") {
                $("#btn_ActiveDiretoryLogin").click();
            }
        }
    });
}
