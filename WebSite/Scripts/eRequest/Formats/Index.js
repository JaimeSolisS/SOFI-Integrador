function IndexInit(LangResources) {
    $(".max-length").maxlength();


    $(document).on('click', '#btn_new_Format', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/eRequest/Formats/GetNewFormatModal").done(function (data) {
            $("#div_mo_new_create_format").html(data);
            $(".max-length").maxlength();
            $("#mo_Create_New_Format").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    //Botones yes no para usar doble verificacion

    $(document).on('click', '#btn_Y2fa', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('fa')) {
            $('#btn_N2fa').addClass('btn-default');
            $('#btn_N2fa').removeClass('btn-danger');
            $('#btn_N2fa').removeClass('fa');
        } else {
            btn.addClass('fa');
            btn.addClass('btn-success');
            btn.removeClass('btn-default');
            $('#btn_N2fa').addClass('btn-default');
            $('#btn_N2fa').removeClass('btn-danger');
            $('#btn_N2fa').removeClass('fa');

        }
    });

    $(document).on('click', '#btn_N2fa', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('fa')) {
            $('#btn_Y2fa').addClass('btn-default');
            $('#btn_Y2fa').removeClass('btn-success');
            $('#btn_Y2fa').removeClass('fa');
        } else {
            btn.addClass('fa');
            btn.addClass('btn-danger');
            btn.removeClass('btn-default');
            $('#btn_Y2fa').addClass('btn-default');
            $('#btn_Y2fa').removeClass('btn-success');
            $('#btn_Y2fa').removeClass('fa');
        }
    });

    //Enviar a aprobacion directa

    $(document).on('click', '#btn_YDirectA', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('fa')) {
            $('#btn_NDirectA').addClass('btn-default');
            $('#btn_NDirectA').removeClass('btn-danger');
            $('#btn_NDirectA').removeClass('fa');
        } else {
            btn.addClass('fa');
            btn.addClass('btn-success');
            btn.removeClass('btn-default');
            $('#btn_NDirectA').addClass('btn-default');
            $('#btn_NDirectA').removeClass('btn-danger');
            $('#btn_NDirectA').removeClass('fa');
        }
    });

    $(document).on('click', '#btn_NDirectA', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('fa')) {
            $('#btn_YDirectA').addClass('btn-default');
            $('#btn_YDirectA').removeClass('btn-success');
            $('#btn_YDirectA').removeClass('fa');
        } else {
            btn.addClass('fa');
            btn.addClass('btn-danger');
            btn.removeClass('btn-default');
            $('#btn_YDirectA').addClass('btn-default');
            $('#btn_YDirectA').removeClass('btn-success');
            $('#btn_YDirectA').removeClass('fa');
        }
    });

    //Botones yes no para allow attachments
    $(document).on('click', '#btn_YAA', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('fa')) {
            $('#btn_NAA').addClass('btn-default');
            $('#btn_NAA').removeClass('btn-danger');
            $('#btn_NAA').removeClass('fa');
        } else {
            btn.addClass('fa');
            btn.addClass('btn-success');
            btn.removeClass('btn-default');
            $('#btn_NAA').addClass('btn-default');
            $('#btn_NAA').removeClass('btn-danger');
            $('#btn_NAA').removeClass('fa');

        }
    });

    $(document).on('click', '#btn_NAA', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('fa')) {
            $('#btn_YAA').addClass('btn-default');
            $('#btn_YAA').removeClass('btn-success');
            $('#btn_YAA').removeClass('fa');
        } else {
            btn.addClass('fa');
            btn.addClass('btn-danger');
            btn.removeClass('btn-default');
            $('#btn_YAA').addClass('btn-default');
            $('#btn_YAA').removeClass('btn-success');
            $('#btn_YAA').removeClass('fa');
        }
    });

    //Botones yes no para has detaill
    $(document).on('click', '#btn_YHD', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('fa')) {
            $('#btn_NHD').addClass('btn-default');
            $('#btn_NHD').removeClass('btn-danger');
            $('#btn_NHD').removeClass('fa');
        } else {
            btn.addClass('fa');
            btn.addClass('btn-success');
            btn.removeClass('btn-default');
            $('#btn_NHD').addClass('btn-default');
            $('#btn_NHD').removeClass('btn-danger');
            $('#btn_NHD').removeClass('fa');
        }
    });

    $(document).on('click', '#btn_NHD', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('fa')) {
            $('#btn_YHD').addClass('btn-default');
            $('#btn_YHD').removeClass('btn-success');
            $('#btn_YHD').removeClass('fa');
        } else {
            btn.addClass('fa');
            btn.addClass('btn-danger');
            btn.removeClass('btn-default');
            $('#btn_YHD').addClass('btn-default');
            $('#btn_YHD').removeClass('btn-success');
            $('#btn_YHD').removeClass('fa');
        }
    });
    //Create New Format
    $(document).on('click', '#btn_CreateNewFormat', function (e) {
        e.stopImmediatePropagation();
        var FormatName = $('#FormatName').val();
        var Use2FA = 0;
        var DirectApproval = 0;
        var HasDetail = 0;
        var HasAttachment = 0;
        if ($('#btn_Y2fa').hasClass('fa')) {
            Use2FA = 1;
        }
        if ($('#btn_YDirectA').hasClass('fa')) {
            DirectApproval = 1;
        }
        if ($('#btn_YAA').hasClass('fa')) {
            HasAttachment = 1;
        }
        if ($('#btn_YHD').hasClass('fa')) {
            HasDetail = 1;
        }
        ShowProgressBar();
        $.post("/eRequest/Formats/CreateNewFormat", {
            FormatName: FormatName,
            Use2FA: Use2FA,
            DirectApproval: DirectApproval,
            HasDetail: HasDetail,
            HasAttachment: HasAttachment,
            Enabled: 0
        }).done(function (data) {
            HideProgressBar();
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                window.location = "/eRequest/Formats/Create?FormatID=" + data.ID;
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Create New Format
    $(document).on('click', '.editformat', function (e) {
        e.stopImmediatePropagation();
        var FormatID = $(this).data('formatid');
        ShowProgressBar();
        window.location = "/eRequest/Formats/Create?FormatID=" + FormatID;
        HideProgressBar();
    });

    //Esconder al usar filtros
    $(".FormatFilters").change(function () {
        $("#divTblFormats").empty();
    });

    $(".FormatFilters").keypress(function () {
        $("#divTblFormats").empty();
    });


    $("#btn_FormatSearch").click(function (e) {
        ShowProgressBar();
        var FormatName = "";
        var Approver = "";
        FormatName = $('#txt_Filter_FormatName').val();
        Apprver = $('#txt_Filter_Approver').val();
        $.get('/eRequest/Formats/SearchFormat', {
            FormatName: FormatName,
            Approver: Approver
        }).done(function (data) {
            $("#divTblFormats").html(data);
        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    RegisterPluginDataTable(50);
    $(document).on('click', 'td.details-control', function (e) {
        e.stopImmediatePropagation();
        var tr = $(this).closest('tr');
        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {
            var FormatID = tr.data("formatid");
            ShowProgressBar();
            $.get("/eRequest/Formats/GetFormatMenu", {
                FormatID: FormatID
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="13" style="padding:0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });

    function CollapseAllList(btn) {
        var div = btn.closest(".div_options");
        //se les quita la clase activa a todos, despues de que se haga el colapso se le agrega la clase al elemento actual
        $(".optionActive").removeClass("optionActive");
        div.removeClass("shown");
        $('.shown-row').remove();
    }

    //Desplegar
    function CollapseAllList(btn) {
        var div = btn.closest(".div_options");
        //se les quita la clase activa a todos, despues de que se haga el colapso se le agrega la clase al elemento actual
        $(".optionActive").removeClass("optionActive");
        div.removeClass("shown");
        $('.shown-row').remove();
    }
    $(document).on('click', '.requestedfieldsformat', function (e) {
        e.stopImmediatePropagation();
        var div = $(this).closest('div');
        var buttonTitle = $(this);
        if (!(buttonTitle).hasClass("optionActive")) {
            CollapseAllList($(this));
        }
        if (div.hasClass("shown")) {
            $('div.slider', div.next()).slideUp(function () {
                div.next().remove();
                div.removeClass('shown');
            });
        } else {

            var FormatID = $(this).data('formatid');

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Formats/GetFormatsFieldsTable", {
                FormatID: FormatID
            }).done(function (data) {
                div.after('');
                div.after('<div class="col-xs-12 padding-0"><tr><td colspan="13" class="padding-0">' + data + '</td></tr><div>');
                div.addClass('shown');
                $('div.slider', div.next()).addClass('shown-row');
                $('div.slider', div.next()).slideDown();
                buttonTitle.addClass("optionActive");
                HideProgressBar();
            });
        }
    });
    $(document).on('click', '.detailformat', function (e) {
        e.stopImmediatePropagation();
        var div = $(this).closest('div');
        var buttonTitle = $(this);
        if (!(buttonTitle).hasClass("optionActive")) {
            CollapseAllList($(this));
        }
        if (div.hasClass("shown")) {
            $('div.slider', div.next()).slideUp(function () {
                div.next().remove();
                div.removeClass('shown');
            });
        } else {

            var FormatID = $(this).data('formatid');

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Formats/GetFormatsDetailFieldsTable", {
                FormatID: FormatID
            }).done(function (data) {
                div.after('');
                div.after('<div class="col-xs-12 padding-0"><tr><td colspan="13" class="padding-0">' + data + '</td></tr><div>');
                div.addClass('shown');
                $('div.slider', div.next()).addClass('shown-row');
                $('div.slider', div.next()).slideDown();
                buttonTitle.addClass("optionActive");
                HideProgressBar();
            });
        }
    });
    $(document).on('click', '.formatphases', function (e) {
        e.stopImmediatePropagation();
        var div = $(this).closest('div');
        var buttonTitle = $(this);
        if (!(buttonTitle).hasClass("optionActive")) {
            CollapseAllList($(this));
        }
        if (div.hasClass("shown")) {
            $('div.slider', div.next()).slideUp(function () {
                div.next().remove();
                div.removeClass('shown');
            });
        } else {

            var FormatID = $(this).data('formatid');

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Formats/GetListPhasesTable", {
                FormatID: FormatID
            }).done(function (data) {
                div.after('');
                div.after('<div class="col-xs-12 padding-0"><tr><td colspan="13" class="padding-0">' + data + '</td></tr><div>');
                div.addClass('shown');
                $('div.slider', div.next()).addClass('shown-row');
                $('div.slider', div.next()).slideDown();
                buttonTitle.addClass("optionActive");
                HideProgressBar();
            });
        }
    });
    $(document).on('click', '.formataccess', function (e) {
        e.stopImmediatePropagation();
        var div = $(this).closest('div');
        var buttonTitle = $(this);
        if (!(buttonTitle).hasClass("optionActive")) {
            CollapseAllList($(this));
        }
        if (div.hasClass("shown")) {
            $('div.slider', div.next()).slideUp(function () {
                div.next().remove();
                div.removeClass('shown');
            });
        } else {

            var FormatID = $(this).data('formatid');

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Formats/GetFormatAccessListTable", {
                FormatID: FormatID
            }).done(function (data) {
                div.after('');
                div.after('<div class="col-xs-12 padding-0"><tr><td colspan="13" class="padding-0">' + data + '</td></tr><div>');
                div.addClass('shown');
                $('div.slider', div.next()).addClass('shown-row');
                $('div.slider', div.next()).slideDown();
                buttonTitle.addClass("optionActive");
                HideProgressBar();
            });
        }
    });
}