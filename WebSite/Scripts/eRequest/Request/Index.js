function ControlDateTypeFilter() {
    if ($("#checkbox_filter_date").is(":checked")) {
        $(".dateTypeFilter").prop("disabled", false);
    } else {
        $(".dateTypeFilter").prop("disabled", true);
    }
    $("#ddl_DateTypes").selectpicker("refresh");
}
function IndexInit(LangResources) {
    ControlDateTypeFilter();
    SetupOnlyNumbers();
    $(".select").selectpicker();
    $('#txt_RequestNumber').maxlength();
    $("#checkbox_filter_date").iCheck();
    $("#ddl_RequestTypes").selectpicker('selectAll');
    $("#ddl_RequestDepartment").selectpicker('selectAll');
    $("#ddl_Facility").selectpicker('selectAll');
    $(".datetimepicker").datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        language: LangResources.culture
    });
    $("#ddl_Status").val($("#RequestDefaultStatus").val().split(","));
    $("#ddl_Status").selectpicker("refresh");


    $(".eRequestFilters").change(function () {
        $("#divTblRequests").empty();
    });

    $(".eRequestFilters").keypress(function () {
        $("#divTblRequests").empty();
    });


    $(document).on('click', '#btn_new_request', function (e) {
        e.stopImmediatePropagation();
        $('#mo_NewRequest').modal({
            backdrop: 'static',
            keyboard: false
        });
    });

    $(document).on('click', '#btn_singnature', function (e) {
        e.stopImmediatePropagation();
        $('#mo_signature').modal({
            backdrop: 'static',
            keyboard: false
        });
    });
    $(document).on('click', '#btn_EditSignature', function (e) {
        e.stopImmediatePropagation();
        $('#mo_signature').modal({
            backdrop: 'static',
            keyboard: false
        });
        $("#signature").jSignature('reset');
        $('#mo_showsignature').modal('toggle');
    });

    $(document).on('click', '.btn-approval', function (e) {
        e.stopImmediatePropagation();
        var RequestLoopFlowID = $(this).data('id');
        var conceptText = $(this).closest("div").data("concept");
        var statusID = $(this).closest("div").data("statusvalueid");

        if (statusID == "Approval") {
            $("#signature_2FA").jSignature('reset');
            $('#txt_coment_approval').val('');
            $('#RequestLoopFlowID').val(RequestLoopFlowID);
            ShowProgressBar();
            $.post("/Request/NextSeq", {
                RequestLoopFlowID: RequestLoopFlowID
            }).done(function (data) {
                if (data.ErrorCode == 0) {
                    ShowProgressBar();
                    $.post("/Request/ApprovalFlowUser", {
                        RequestLoopFlowID: RequestLoopFlowID
                    }).done(function (data) {
                        if (data.ErrorCode == 0) {
                            //Cuando el usuario es el que debe aprobar
                            $('#UserIDLogin').val('');
                            //Si el formato tiene 2FA
                            if (data.ID == 1) {
                                //Abre modal de 2FA
                                $('#mo_2FA').modal({
                                    backdrop: 'static',
                                    keyboard: false
                                });
                                //Valida el codigo de 2FA que se le envio al usuario
                                SendCodeValidation();
                                //Mandamos a vacio el text del codigo
                                $('#txt_code').val('');
                                //Cierra modal de Login
                            } else {
                                $.post("/Request/CheckSignature", {
                                    UserID: ''
                                }).done(function (data) {
                                    //Se revisa si el usuario tiene firma o no 1 si el usuario si tiene firma 0 si no tiene firma
                                    if (data.ErrorCode == 0) {
                                        $("#moTxtConcept").text(conceptText);
                                        //Si tiene configurada la firma muestra el modal para aprobar
                                        $('#mo_phase_approval').modal({
                                            backdrop: 'static',
                                            keyboard: false
                                        });
                                    } else {
                                        notification("", data.ErrorMessage, "error");
                                        //Si no tiene firma lo manda a un modal para que firme 
                                        $('#mo_signature_2FA').modal({
                                            backdrop: 'static',
                                            keyboard: false
                                        });
                                        HideProgressBar();
                                    }
                                }).fail(function (xhr, textStatus, error) {
                                    notification("", data.ErrorMessage, "error");
                                }).always(function () {
                                    HideProgressBar();
                                });
                            }
                        } else {
                            //Cuando el usuario no es el que debe de aprobar
                            notification("", data.ErrorMessage, "error");
                            SetConfirmBoxAction(function () {
                                $('#UserIDLoginType').val('APPROVAL');
                                ShowProgressBar();
                                $.get("/LoginAnotherAccount/GetModalLoginOptions").done(function (data) {
                                    $("#div_Login").html(data);
                                    $("#mo_LoginAccess").modal("show");
                                }).fail(function (xhr, textStatus, error) {
                                    notification("", error.message, "error");
                                }).always(function () {
                                    HideProgressBar();
                                });
                                HideProgressBar();
                            }, LangResources.msg_LoginApprove);
                        }
                    }).fail(function (xhr, textStatus, error) {
                        notification("", data.ErrorMessage, "error");
                    }).always(function () {
                        HideProgressBar();
                    });
                } else {
                    notification("", data.ErrorMessage, "error");
                    HideProgressBar();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", data.ErrorMessage, "error");
            }).always(function () {
                HideProgressBar();
            });
        }
    });

    $(document).on("click", "#btnReturnLoan", function (e) {
        e.stopImmediatePropagation();
        var RequestID = $(this).data("requestid");
        ShowProgressBar();
        $.get('/eRequest/Request/GetReturnLoadModal', {
            RequestID: RequestID
        }).done(function (data) {
            $("#divGenericModal").html(data);
            $("#mo_ReturnMaterial").modal("show");

        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    function LoadMaterialReturnTable(RequestID) {
        ShowProgressBar();
        $.get('/eRequest/Request/GetMaterialReturnTable', {
            RequestID: RequestID
        }).done(function (data) {
            $("#tbl_ReturnMaterial").html(data);
            if ($(".chkMaterialToReturn").length == 0) {
                $("#mo_ReturnMaterial").modal("toggle");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    }

    $(document).on("click", "#btnSaveMaterialReturned", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var RequestID = $(this).data("requestid");
        var RequestGenericDetailIDs = [];
        $(".chkMaterialToReturn:checked").each(function () {
            var requestgenericdetailid = $(this).data("requestgenericdetailid");
            RequestGenericDetailIDs.push(requestgenericdetailid);
        });

        $.post('/eRequest/Request/UpdateRequestDetailReturnDate', {
            RequestGenericDetailIDs: RequestGenericDetailIDs,
            Comments: $("#txtCommentsApproval").val()
        }).done(function (data) {
            LoadMaterialReturnTable(RequestID);
        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("change", "#FacilityID", function (e) {
        e.stopImmediatePropagation();
        var val = $("#FacilityID option:selected").val();
        $("#FormatID").empty();
        if (val != 0) {
            $.get("/eRequest/Request/GetRequestByFacility", {
                FacilityID: val
            }).done(function (data) {
                $.each(data, function (k, v) {
                    $("#FormatID").append('<option value="' + v.value + '">' + v.text + '</option>');
                });
            }).fail(function (xhr, textStatus, error) {
                notification("", data.ErrorMessage, "error");
            }).always(function () {
                $("#FormatID").selectpicker("refresh");
            });
        } else {
            $("#FormatID").selectpicker("refresh");
        }
    });

    $(document).on('click', '#btn_ResetSignature', function (e) {
        e.stopImmediatePropagation();
        $("#signature").jSignature('reset');
    });
    $(document).on('click', '#btn_ResetSignature_2FA', function (e) {
        e.stopImmediatePropagation();
        $("#signature_2FA").jSignature('reset');
    });
    $(document).on('click', '#btn_showsingnature', function (e) {
        e.stopImmediatePropagation();
        $('#mo_showsignature').modal({
            backdrop: 'static',
            keyboard: false
        });
        ShowProgressBar();
        // abrir detalles y devolver detalles de una llamada ajax
        $.get("/eRequest/Request/GetSignature", {
        }).done(function (data) {
            $("#img-signature").attr("src", data.img);
            HideProgressBar();
        });
    });
    $('#signature').jSignature({
        color: "#000",
        lineWidth: 2,
        width: 720,
        height: 250,
        UndoButton: true

    });
    $('#signature_2FA').jSignature({
        color: "#000",
        lineWidth: 2,
        width: 720,
        height: 250,
        UndoButton: true

    });

    $(document).on("click", "#btn_ActiveDiretoryLogin", function () {
        var User = $("#txt_UsrName").val();
        var Pass = $("#txt_UsrPass").val();
        if (User != "" && Pass != "") {
            ShowProgressBar();
            $.post("/LoginAnotherAccount/LoginWithActiveDirectory", {
                Domain: $("#ddl_UsrDomain option:selected").text(),
                User: $("#txt_UsrName").val(),
                Password: $("#txt_UsrPass").val()
            }).done(function (data) {
                if (data.isValid) {
                    if (data.UserID > 0) {
                        $('#UserIDLogin').val(data.UserID);
                        if ($('#UserIDLoginType').val() == 'APPROVAL') {
                            ShowProgressBar();
                            $.post("/Request/ApprovalFlowUserLogin", {
                                RequestLoopFlowID: $('#RequestLoopFlowID').val(),
                                UserID: data.UserID
                            }).done(function (data) {
                                if (data.ErrorCode == 0) {
                                    //Si el formato tiene 2FA
                                    if (data.ID == 1) {
                                        //Abre modal de 2FA
                                        $('#mo_2FA').modal({
                                            backdrop: 'static',
                                            keyboard: false
                                        });
                                        //Valida el codigo de 2FA que se le envio al usuario
                                        SendCodeValidation();
                                        //Mandamos a vacio el text del codigo
                                        $('#txt_code').val('');
                                        //Cierra modal de Login
                                        $('#mo_LoginAccess').modal('toggle');
                                    } else {
                                        //Si el formato no tiene 2FA
                                        $.post("/Request/CheckSignature", {
                                            UserID: $('#UserIDLogin').val()
                                        }).done(function (data) {
                                            //Se revisa si el usuario tiene firma o no 1 si el usuario si tiene firma 0 si no tiene firma
                                            if (data.ErrorCode == 0) {
                                                //Si tiene configurada la firma muestra el modal para aprobar
                                                $('#mo_phase_approval').modal({
                                                    backdrop: 'static',
                                                    keyboard: false
                                                });
                                                $('#mo_LoginAccess').modal('toggle');
                                            } else {
                                                notification("", data.ErrorMessage, "error");
                                                //Si no tiene firma lo manda a un modal para que firme 
                                                $('#mo_signature_2FA').modal({
                                                    backdrop: 'static',
                                                    keyboard: false
                                                });
                                                $('#mo_LoginAccess').modal('toggle');
                                                HideProgressBar();
                                            }
                                        }).fail(function (xhr, textStatus, error) {
                                            notification("", data.ErrorMessage, "error");
                                        }).always(function () {
                                            HideProgressBar();
                                        });
                                    }

                                } else {
                                    notification("", data.ErrorMessage, "error");
                                    $('#UserIDLoginType').val('APPROVAL');
                                    ShowProgressBar();
                                    $.get("/LoginAnotherAccount/GetModalLoginOptions").done(function (data) {
                                        $("#div_Login").html(data);
                                        $("#mo_LoginAccess").modal("show");
                                    }).fail(function (xhr, textStatus, error) {
                                        notification("", error.message, "error");
                                    }).always(function () {
                                        HideProgressBar();
                                    });
                                    HideProgressBar();
                                }
                            }).fail(function (xhr, textStatus, error) {
                                notification("", data.ErrorMessage, "error");
                            }).always(function () {
                                HideProgressBar();
                            });
                        } else {
                            $('#UserIDLogin').val(data.UserID);
                            $('#mo_LoginAccess').modal('toggle');
                            $('#btn_showsingnature').trigger('click');
                        }
                    }
                } else {
                    notification("", data.ErrorMessage, "error");
                    if ($('#UserIDLoginType').val() == 'APPROVAL') {

                    }
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            notification("", LangResources.lbl_UserPassMandatory, "_ntf");
        }
    });





    $(document).on("ifChanged", "#checkbox_filter_date", function (e) {
        e.stopImmediatePropagation();
        $("#divTblRequests").empty();
        ControlDateTypeFilter();
    })


    $(document).on('click', '#btn_Create', function (e) {
        e.stopImmediatePropagation();
        var FacilityID = $('#FacilityID').val();
        var FormatID = $('#FormatID').val();
        if (FacilityID != 0 && FormatID != 0) {
            window.location.href = '/eRequest/Request/Create?FacilityID=' + FacilityID + '&FormatID=' + FormatID;
        } else {
            notification("", LangResources.msg_FacilityFormatMandatory, "error");
        }
    });


    $(document).on('click', '#btn_CreateSignature', function (e) {
        e.stopImmediatePropagation();
        if ($("#signature").jSignature('getData', 'image')[1].length > 5280) {
            var imageStream = $("#signature").jSignature('getData', 'image');
            var img = imageStream[1];
            ShowProgressBar();
            $.post("/Request/SaveSignature", {
                Img64: img
            }).done(function (data) {
                if (data.ErrorCode == 0) {
                    notification(data.Title, data.ErrorMessage, data.notifyType);
                    $('#mo_signature').modal('toggle');
                    $('#btn_showsingnature').trigger('click');
                } else {
                    notification("", data.ErrorMessage, "error");
                    HideProgressBar();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", data.ErrorMessage, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            notification("", "asdasdasd", "_ntf");
        }
    });

    //Guardamos la firma del usuario autentificado

    $(document).on('click', '#btn_CreateSignature_2FA', function (e) {
        e.stopImmediatePropagation();
        var imageStream = $("#signature_2FA").jSignature('getData', 'image');
        var img = imageStream[1];
        var UserID = $('#UserIDLogin').val();
        ShowProgressBar();
        $.post("/Request/SaveSignature", {
            Img64: img,
            UserID: UserID
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification(data.Title, data.ErrorMessage, data.notifyType);
                $('#mo_phase_approval').modal({
                    backdrop: 'static',
                    keyboard: false
                });
                $('#mo_signature_2FA').modal('toggle');
            } else {
                notification("", data.ErrorMessage, "error");
                HideProgressBar();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    });


    $(document).on('click', '.ereq-edit', function (e) {
        e.stopImmediatePropagation();
        var div = $(this).closest('div');
        var FacilityID = $(div)[0].dataset.facility;
        var FormatID = $(div)[0].dataset.formatid;
        var RequestID = $(div)[0].dataset.idrequest;
        window.location.href = '/eRequest/Request/Create?FacilityID=' + FacilityID + '&FormatID=' + FormatID + '&RequestID=' + RequestID;
    });

    //Revisa el codigo de confirmacion que se le envio al usuario

    $(document).on('click', '#btn_2fa_acept', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.post("/Request/CheckCodeValidation", {
            UserID: $('#UserIDLogin').val(),
            FAToken: $('#txt_code').val()
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                //Se revisa si el usuario tiene firma o no 1 si el usuario si tiene firma 0 si no tiene firma
                if (data.ID == 1) {
                    //Si tiene configurada la firma muestra el modal para aprobar
                    $('#mo_2FA').modal('toggle');
                    $('#mo_phase_approval').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                } else if (data.ID == 0) {
                    //Si no tiene firma lo manda a un modal para que firme 
                    $('#mo_2FA').modal('toggle');
                    $('#mo_signature_2FA').modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    notification(data.Title, data.ErrorMessage, "error");
                }
            } else {
                notification("", data.ErrorMessage, "error");
                HideProgressBar();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btn_2fa_forward', function (e) {
        e.stopImmediatePropagation();
        SetConfirmBoxAction(function () {
            SendCodeValidation();
        }, LangResources.msg_CreateNewApproval2FA);

    });

    $(document).on("click", "#btn_Login", function () {
        ShowProgressBar();
        $('#UserIDLoginType').val('SIGNATURE');
        $.get("/LoginAnotherAccount/GetModalLoginOptions").done(function (data) {
            $("#div_Login").html(data);
            $("#mo_LoginAccess").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    RegisterPluginDataTable(50);

    $(document).ready(function () {
        $('.zoom').hover(function () {
            $(this).addClass('transition');
        }, function () {
            $(this).removeClass('transition');
        });
    });

    $("#btn_RequestSearch").click(function (e) {
        ShowProgressBar();
        var formatIDs = "";
        var departmentIDs = "";
        var statusIDs = "";
        var faciltyIDs = "";
        var dateTypeID = null;
        var startDate = null;
        var endDate = null;
        if ($("#ddl_RequestTypes").val() != null) {
            formatIDs = $("#ddl_RequestTypes").val().join(",");
        }
        if ($("#ddl_RequestDepartment").val() != null) {
            departmentIDs = $("#ddl_RequestDepartment").val().join(",");
        }

        if ($("#ddl_Status").val() != null) {
            statusIDs = $("#ddl_Status").val().join(",");
        }
        if ($("#ddl_Facility").val() != null) {
            faciltyIDs = $("#ddl_Facility").val().join(",");
        }

        if ($("#checkbox_filter_date").is(":checked")) {
            dateTypeID = $("#ddl_DateTypes option:selected").val();
            startDate = $("#txt_OpportunitiesProgramStartDate").val();
            endDate = $("#txt_OpportunitiesProgramEndDate").val();
        }

        $.get('/eRequest/Request/SearchRequest', {
            FormatIDs: formatIDs,
            Folio: $("#txt_RequestNumber").val(),
            DepartmentIDs: departmentIDs,
            DateTypeID: dateTypeID,
            StatusIDs: statusIDs,
            FacilityIDs: faciltyIDs,
            StartDate: startDate,
            EndDate: endDate
        }).done(function (data) {
            $("#divTblRequests").html(data);
        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    function CollapseAllList(btn) {
        var div = btn.closest(".div_options");
        //se les quita la clase activa a todos, despues de que se haga el colapso se le agrega la clase al elemento actual
        $(".optionActive").removeClass("optionActive");

        div.removeClass("shown");
        div.find(".display-detail").prop('title', LangResources.lbl_ViewDetail);
        div.find(".display-detail").find("img").attr("src", "/Content/img/buttonicon/Eye-Show.png");
        div.find(".display-log").prop('title', LangResources.lbl_SeeApprovalLog);
        div.find(".display-log").find("img").attr("src", "/Content/img/buttonicon/eReq-log.png");
        div.find(".display-attachment").prop('title', LangResources.lbl_ViewAttachments);
        div.find(".display-attachment").find("img").attr("src", "/Content/img/buttonicon/folder_add.png");
        $('.shown-row').remove();
    }

    $(document).on('click', '.display-detail', function (e) {
        e.stopImmediatePropagation();
        var div = $(this).closest('div');
        var img = $(this)[0].firstElementChild;
        var buttonTitle = $(this);
        if (!(buttonTitle).hasClass("optionActive")) {
            CollapseAllList($(this));
        }
        if (div.hasClass("shown")) {
            $('div.slider', div.next()).slideUp(function () {
                div.next().remove();
                div.removeClass('shown');
                img.src = '/Content/img/buttonicon/Eye-Show.png';
                $(buttonTitle).prop('title', LangResources.lbl_ViewDetail);
            });
        } else {

            var RequestID = $(div)[0].dataset.idrequest;

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Request/GetRequestDetail", {
                RequestID: RequestID
            }).done(function (data) {
                div.after('');
                div.after('<div class="col-xs-12 padding-0"><tr><td colspan="13" class="padding-0">' + data + '</td></tr><div>');
                div.addClass('shown');
                $('div.slider', div.next()).addClass('shown-row');
                $('div.slider', div.next()).slideDown();
                img.src = '/Content/img/buttonicon/Eye-Show-Disable.png';
                $(buttonTitle).prop('title', LangResources.lbl_HideDetail);
                buttonTitle.addClass("optionActive");
                HideProgressBar();
            });
        }
    });

    $(document).on('click', '.display-log', function (e) {
        e.stopImmediatePropagation();

        var div = $(this).closest('div');
        var img = $(this)[0].firstElementChild;
        var buttonTitle = $(this);
        if (!(buttonTitle).hasClass("optionActive")) {
            CollapseAllList($(this));
        }
        if (div.hasClass("shown")) {
            $('div.slider', div.next()).slideUp(function () {
                div.next().remove();
                div.removeClass('shown');
                img.src = '/Content/img/buttonicon/eReq-log.png';
                $(buttonTitle).prop('title', LangResources.lbl_SeeApprovalLog);
            });
        } else {

            var RequestID = $(div)[0].dataset.idrequest;

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Request/GetApprovalLog", {
                RequestID: RequestID
            }).done(function (data) {
                div.after('');
                div.after('<div class="col-xs-12 padding-0"><tr><td colspan="13" class="padding-0">' + data + '</td></tr><div>');
                div.addClass('shown');
                $('div.slider', div.next()).addClass('shown-row');
                $('div.slider', div.next()).slideDown();
                img.src = '/Content/img/buttonicon/eReq-log-colapse.png';
                $(buttonTitle).prop('title', LangResources.lbl_HideApprovalLog);
                buttonTitle.addClass("optionActive");
                HideProgressBar();
            });
        }
    });

    $(document).on('click', '.display-attachment', function (e) {
        e.stopImmediatePropagation();

        var div = $(this).closest('div');
        var img = $(this)[0].firstElementChild;
        var buttonTitle = $(this);
        if (!(buttonTitle).hasClass("optionActive")) {
            CollapseAllList($(this));
        }
        if (div.hasClass("shown")) {
            $('div.slider', div.next()).slideUp(function () {
                div.next().remove();
                div.removeClass('shown');
                img.src = '/Content/img/buttonicon/folder_add.png';
                $(buttonTitle).prop('title', LangResources.lbl_ViewAttachments);
            });
        } else {

            var RequestID = $(div)[0].dataset.idrequest;
            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Request/GetRequestMedia", {
                RequestID: RequestID
            }).done(function (data) {
                div.after('');
                div.after('<div class="col-xs-12 padding-0"><tr><td colspan="13" class="padding-0">' + data + '</td></tr><div>');
                div.addClass('shown');
                $('div.slider', div.next()).addClass('shown-row');
                $('div.slider', div.next()).slideDown();
                img.src = '/Content/img/buttonicon/folder_removed.png';
                $(buttonTitle).prop('title', LangResources.lbl_HideAttachments);
                buttonTitle.addClass("optionActive");
                HideProgressBar();
            });
        }
    });

    $(document).on('click', '.format-media-download', function (e) {
        e.stopImmediatePropagation();

        var row = $(this).closest("tr");
        var FileID = row.data("fileid");

        ShowProgressBar();
        $.get("/Attachments/Download", {
            FileId: FileID,
            AttachmentType: "EREQUESTMEDIA"
        }).done(function (data) {
            eval(data.JSCorefunction);
            HideProgressBar();
        });
    });
    //Boton para rechazar aprobacion
    $(document).on('click', '#btn_Reject_Request', function (e) {
        e.stopImmediatePropagation();
        var RequestLoopFlowID = $('#RequestLoopFlowID').val();
        var Comment = $('#txt_coment_approval').val();
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.post("/Request/RequestsLoopsFlowStatusUpdate", {
                RequestLoopFlowID: RequestLoopFlowID,
                Status: 'C',
                Comment: Comment,
                UserID: $('#UserIDLogin').val()
            }).done(function (data) {
                if (data.ErrorCode == 0) {
                    notification(data.Title, data.ErrorMessage, data.notifyType);
                } else {
                    notification("", data.ErrorMessage, "error");
                    HideProgressBar();
                }

            }).fail(function (xhr, textStatus, error) {
                notification("", data.ErrorMessage, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_RequestCancel);
    });
    //Boton para aprobar aprobacion
    $(document).on('click', '#btn_Approval_Request', function (e) {
        e.stopImmediatePropagation();
        var RequestLoopFlowID = $('#RequestLoopFlowID').val();
        var Comment = $('#txt_coment_approval').val();
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.post("/Request/RequestsLoopsFlowStatusUpdate", {
                RequestLoopFlowID: RequestLoopFlowID,
                Status: 'A',
                Comment: Comment,
                UserID: $('#UserIDLogin').val()
            }).done(function (data) {
                if (data.ErrorCode == 0) {
                    notification(data.Title, data.ErrorMessage, data.notifyType);
                } else {
                    notification("", data.ErrorMessage, "error");
                    HideProgressBar();
                }

            }).fail(function (xhr, textStatus, error) {
                notification("", data.ErrorMessage, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_RequestCancel);
    });

    $(document).on('click', '.cancel-request', function (e) {
        e.stopImmediatePropagation();
        var div = $(this).closest('div');
        var RequestID = $(div)[0].dataset.idrequest;

        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.post("/Request/UpdateStatus", {
                RequestID: RequestID,
                Status: 'Cancelled'
            }).done(function (data) {


                if (data.ErrorCode == 0) {

                    notification(data.Title, data.ErrorMessage, data.notifyType);
                } else {
                    notification("", data.ErrorMessage, "error");
                    HideProgressBar();
                }

            }).fail(function (xhr, textStatus, error) {
                notification("", data.ErrorMessage, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_RequestCancel);


    });

    $(document).on('click', '.show-pdf', function (e) {
        e.stopImmediatePropagation();
        var div = $(this).closest('div');
        var RequestID = $(div)[0].dataset.idrequest;
        var btn = $(this)
        var FormatID = btn.data('formatid');
        ShowProgressBar();
        $.post("/Request/PrinteRequest", {
            RequestID: RequestID,
            FormatID: FormatID
        }).done(function (data) {


            if (data.ErrorCode == 0) {
                //Se abre el pdf
                var dir = window.location.host + data.pdfPath;
                btn[0].dataset.url = dir;
                openFile(btn);
                window.open(dir, '_blank');
            } else {
                notification("", data.ErrorMessage, "error");
                HideProgressBar();
            }

        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    function openFile(sender) {
        try {
            var url = $(sender)[0].dataset.url;
            window.open('' + url + '');
        } catch (err) {

        }
        return false;
    }

    function setStreamSignature() {
        var divCreateSignatureBody = $("#signature");
        if (divCreateSignatureBody.jSignature('getData', 'native').length == 0) {
            notification("Please enter first your signature", data.ErrorMessage, "error");
            return;
        }
        var hdnStreamSignature = $("#signature");
        var imageStream = divCreateSignatureBody.jSignature('getData', 'image');
        hdnStreamSignature.val(imageStream);
    }

    function SendCodeValidation() {
        $.post("/Request/SendCodeValidation", {
            UserID: $('#UserIDLogin').val()
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification(data.Title, data.ErrorMessage, data.notifyType);

            } else {
                notification("", data.ErrorMessage, "error");
                HideProgressBar();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", data.ErrorMessage, "error");
        }).always(function () {
            HideProgressBar();
        });
    }
}