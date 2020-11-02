// =============================================================================================================================
//  Version: 20200103
//  Author:  Luis Hernandez
//  Created Date: 3 Enero 2020
//  Description:  KioskUserRequestAdmin HR JS
//  Modifications: 
// =============================================================================================================================

function IndexInit(LangResources) {



    function EnableDateFilterRequests() {
        if ($('.check-range-date').is(':checked')) {
            $('#checkdate').val('true');
            $('#txt_RequestStartDate').prop("disabled", false);
            $('#txt_RequestEndDate').prop("disabled", false);
        } else {
            $('#checkdate').val('false');
            $('#txt_RequestStartDate').prop("disabled", true);
            $('#txt_RequestEndDate').prop("disabled", true);
        }
    }

    var StatusToSelect = $("#SelectedDefaultStatus").val().split(",");

    $('#txt_RequestNumber').maxlength();
    $('#txt_Responsible').maxlength();
    function ShowHidePagesRequests(activepage, PublicPrivateSeparator) {
        $('.custompager_' + PublicPrivateSeparator).hide();
        var PageCount = $('#PageCount_' + PublicPrivateSeparator).val();

        $('#custom_page_' + PublicPrivateSeparator + '_1').show();
        var rango_ini = activepage - 5;
        for (var i = activepage; i > rango_ini; i--) {
            $('#custom_page_' + PublicPrivateSeparator + '_' + i).show();
        }
        if (rango_ini > 1) {
            $('<li class="temp_li"><a >...</a></li>').insertAfter('#custom_page_' + PublicPrivateSeparator + '_1');
        }

        var rango_fin = activepage + 5;
        for (var i = activepage; i < rango_fin; i++) {
            $('#custom_page_' + PublicPrivateSeparator + '_' + i).show();
        }
        if (rango_fin < PageCount) {
            $('<li class="temp_li"><a >...</a></li>').insertBefore('#custom_page_' + PublicPrivateSeparator + '_' + PageCount);
        }
        $('#custom_page_' + PublicPrivateSeparator + '_' + PageCount).show();

        $('.custompager_' + PublicPrivateSeparator).removeClass("active");
        $('#custom_page_' + PublicPrivateSeparator + '_' + activepage).addClass("active");
    }
    function ChangeStatus(NewStatus, RequestResponsibleID) {
        var RequestIDs = [];
        var SingleRequestID = $("#Mo_AssRes_RequestID").val();
        if (SingleRequestID != null && SingleRequestID != "" && SingleRequestID != "0") {
            RequestIDs.push($("#Mo_AssRes_RequestID").val());
        } else {
            $(".chk_all_request:checked").each(function () {
                RequestIDs.push($(this).data("requestid"));
            });
        }

        ShowProgressBar();
        $.post("/KioskRequestAdministrator/ChangeStatusRequest", {
            RequestIDs: RequestIDs.join(),
            NewStatus: NewStatus,
            RequestResponsibleID: RequestResponsibleID,
            Comments: $('#txta_Comment').val()
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);

            if (data.ErrorCode == 0) {
                if (NewStatus == "Assigned") {
                    $("#mo_AssignRequest").modal("toggle");
                } else {
                    $("#mo_RequestStatusChange").modal("toggle");
                }

                SearchRequestData();
            }

        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    }
    function ValidateSelectedRequisitions() {
        var RequestIDs = [];
        var ReturnValue = true;

        $(".chk_all_request:checked").each(function () {
            RequestIDs.push($(this).data("requestid"));
        });
        if (RequestIDs.length == 0) {
            ReturnValue = false;
        }

        return ReturnValue;
    }
    function SearchRequestData() {
        //variables
        var RequestTypeIDs = $("#ddl_RequestTypes").val();
        var RequestStatusIDs = $("#ddl_RequestStatus").val();
        var DepartmentIDs = $("#ddl_RequestDepartment").val();
        var StartDate = $("#txt_RequestStartDate").val();
        var EndDate = $("#txt_RequestEndDate").val();
        var RequestResponsible = $("#txt_Responsible").val();
        var FacilityIDs = $("#ddl_UserFacilities").val();
        var ShiftIDs = $("#ddl_ShiftsList").val();

        //validaciones
        if (!($("#checkbox-filter-date").is(":checked"))) {
            StartDate = null;
            EndDate = null;
        }
        if (RequestTypeIDs != null) {
            RequestTypeIDs = $("#ddl_RequestTypes").val().join();
        }
        if (RequestStatusIDs != null) {
            RequestStatusIDs = $("#ddl_RequestStatus").val().join();
        }

        if (DepartmentIDs != null) {
            DepartmentIDs = $("#ddl_RequestDepartment").val();
        }

        if (FacilityIDs != null) {
            FacilityIDs = $("#ddl_UserFacilities").val().join(',');
        }

        if (FacilityIDs == null) {
            FacilityIDs = "";
        }

        if (ShiftIDs != null) {
            ShiftIDs = $("#ddl_ShiftsList").val().join(',');
        }

        if (ShiftIDs == null) {
            ShiftIDs = "";
        }

        ShowProgressBar();
        $.get("/KioskRequestAdministrator/SearchRequestData", {
            RequestNumber: $("#txt_RequestNumber").val(),
            RequestTypeIDs: RequestTypeIDs,
            RequestStatusIDs: RequestStatusIDs,
            StartDate: StartDate,
            EndDate: EndDate,
            DepartmentIDs: DepartmentIDs,
            EmployeeInfo: $("#txt_RequestEmployee").val(),
            RequestDescription: $("#txt_RequestDescription").val(),
            RequestResponsible: RequestResponsible,
            FacilityIDs: FacilityIDs,
            ShiftIDs: ShiftIDs
        }).done(function (data) {
            $('#div_RequestList').html(data);
            RegisterPluginDataTable(100);

            $(".request_row").each(function () {
                if ($(this).data("requestnumber") == $("#txt_RequestNumber").val()) {
                    $(this).find(".details-control").click();
                }
            });
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }

    function PrepareMultipleList(selector, SpecialOptionsToSelect) {
        var PrepareMultipleSelected = "";
        if (SpecialOptionsToSelect == "All") {
            $(selector + " option").each(function (k, v) {
                $(this).attr("selected", true);
                $(this).addClass("selected");
                $(this).selectpicker("refresh");
                PrepareMultipleSelected += ", " + $(this).text();
            });
        } else {
            //Para que esta parte funcione, se uso una lista normal para usar el ValueID de los catalogos
            $(selector + " option").each(function (k, v) {
                if (SpecialOptionsToSelect.includes($(this).data("valueid"))) {
                    $(this).attr("selected", true);
                    $(this).addClass("selected");
                    $(this).selectpicker("refresh");
                    PrepareMultipleSelected += ", " + $(this).text();
                }
            });
        }
        PrepareMultipleSelected = PrepareMultipleSelected.substr(2, PrepareMultipleSelected.length);
        $(selector).attr("title", PrepareMultipleSelected);
        $(selector).selectpicker("refresh");
    }
    function RequestStatusChange(StatusName, RequestNumber, AlertText, StatusTypeChange, RequestID) {
        var IsValid = false;
        if (RequestNumber != null) {
            IsValid = true
        } else {
            IsValid = ValidateSelectedRequisitions();
        }

        if (IsValid) {
            var RequestNumbers = "";
            if (RequestNumber == null) {
                $(".chk_all_request:checked").each(function () {
                    RequestNumbers += ", " + $(this).closest("tr").data("requestnumber");
                });
                RequestNumbers = RequestNumbers.substring(2, RequestNumbers.length);
            } else {
                RequestNumbers = RequestNumber
            }

            ShowProgressBar();
            $.get("/KioskRequestAdministrator/GetRequestStatusChangeModal", {
                StatusTypeChange: StatusTypeChange,
                RequestID: RequestID
            }).done(function (data) {
                $("#root_modal").html(data);
                $("#mo_RequestStatusChange").modal("show");

                $("#ttl_StatusChangeTitle").text(StatusName);
                $("#span_RequestNumbers").text(RequestNumbers);

                $("#span_AlertText").text(AlertText);

                HideProgressBar();
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            notification("", LangResources.msg_RequisitionMandatory, "_ntf");
        }
    }

    //Date Picker
    $('.datepicker-demo').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        language: LangResources.culture
    });
    //Initialize
    $("select").selectpicker();
    $("#ddl_UserFacilities").selectpicker('selectAll');
    $("#ddl_ShiftsList").selectpicker('selectAll');
    //ShowHidePagesRequests(1, "public");
    $("#tab_PublicRequestsList").addClass("active");
    $(".customdatepicker").datepicker({ format: 'yyyy-mm-dd' });
    $("#checkbox-filter-date").iCheck();
    PrepareMultipleList("#ddl_RequestTypes", "All");
    PrepareMultipleList("#ddl_RequestStatus", StatusToSelect);
    EnableDateFilterRequests();
    $("#request_count").text($(".notification_count").length);

    if ($("#PostedRequestNumber").val() != null) {
        $("#txt_RequestNumber").val($("#PostedRequestNumber").val());
        $("#pnlRequestFilters").css("display", "none");
    }
    SearchRequestData();

    //Events
    $(document).on("ifChanged", ".check-range-date", function (e) {
        e.stopImmediatePropagation();
        EnableDateFilterRequests();
    });

    $(document).on("click", "#btn_RequestSearch", function (e) {
        e.stopImmediatePropagation();
        SearchRequestData();
    });

    $(document).on("click", ".view-request", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/KioskRequestAdministrator/GetViewRequestModal", {
            RequestID: $(this).data("requestid")
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

    $(document).on("change", ".RequestFilters", function (e) {
        e.stopImmediatePropagation();
        $("#div_RequestList").html("");
    });

    $(document).on("keydown", ".RequestFilters", function (e) {
        e.stopImmediatePropagation();
        $("#div_RequestList").html("");
    });

    $(document).on("change", ".chk_all_requests", function (e) {
        e.stopImmediatePropagation();
        if ($(this).is(":checked")) {
            $(".chk_all_request").prop('checked', true);
        } else {
            $(".chk_all_request").prop('checked', false);
        }
    });


    //Begin: Eventos para cambios de status por lotes

    $(document).on("click", "#btn_AssignRequest", function (e) {
        e.stopImmediatePropagation();

        if (ValidateSelectedRequisitions()) {
            var RequestIDs = ""
            $(".chk_all_request:checked").each(function () {
                RequestIDs += ", " + $(this).data("requestnumber");
            });
            RequestIDs = RequestIDs.substr(2, RequestIDs.length);

            ShowProgressBar();
            $.get("/KioskRequestAdministrator/GetAssignResponsibleModal", {

            }).done(function (data) {
                $("#root_modal").html(data);
                $("#ddl_ResponsiblesList").selectpicker();
                $("#mo_AssignRequest").modal("show");
                //$("#txt_AssignResponsible").focus();
                $("#span_RequestNumbers").text(RequestIDs);
                //AutoCompleteDrop();
                HideProgressBar();
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            notification("", LangResources.msg_RequisitionMandatory, "_ntf");
        }

    });

    $(document).on("click", "#btn_MarkDoneRequest", function (e) {
        e.stopImmediatePropagation();
        RequestStatusChange(LangResources.lbl_MarkDone, null, null, "MarkDone");
    });

    $(document).on("click", "#btn_CancelRequest", function (e) {
        e.stopImmediatePropagation();
        RequestStatusChange(LangResources.lbl_CancelRequest, null, LangResources.lbl_CancelRequestAlertText, "Cancel");
    });

    $(document).on("click", "#btn_CloseRequest", function (e) {
        e.stopImmediatePropagation();
        RequestStatusChange(LangResources.btn_CloseRequest, null, null, "Close");
    });

    $(document).on("click", "#btn_RejectRequest", function (e) {
        e.stopImmediatePropagation();
        RequestStatusChange(LangResources.lbl_RejectRequest, null, LangResources.lbl_RejectRequestAlertText, "Reject");
    });

    $(document).on("click", "#btn_ReopenRequest", function (e) {
        e.stopImmediatePropagation();
        RequestStatusChange(LangResources.lbl_ReOpenRequest, null, LangResources.lbl_ReOpenRequestAlertText, "ReOpen");
    });

    $(document).on("click", "#btn_SaveAssignResponsible", function (e) {
        e.stopImmediatePropagation();
        SetConfirmBoxAction(function () {
            var idAsigned = $("#txt_AssignResponsible option:selected").val();
            ChangeStatus("Assigned", idAsigned);
        }, LangResources.msg_ConfirmMarkRequestAssign);
    });

    //End: Eventos para cambios de status por lotes




    //Guardar cambios de estado en requisiciones
    $(document).on("click", "#btn_SaveRequisitionNewStatus", function (e) {
        e.stopImmediatePropagation();
        var WarningMessage = $(this).data("warningmessage");
        var StatusType = $(this).data("statustype");

        SetConfirmBoxAction(function () {
            ChangeStatus(StatusType, "");
        }, WarningMessage);
    });

    //Paginado
    $(document).on("click", ".custompager", function (e) {
        e.stopImmediatePropagation();
        SearchRequestData();
    });



    //Begin: Eventos para cambios de status individuales

    $(document).on("click", "#btn_CloseSinglePublicRequestView", function (e) {
        e.stopImmediatePropagation();
        $("#mo_RequestView").modal("toggle");
    });

    $(document).on("click", ".btn_TblAssignResponsible", function (e) {
        e.stopImmediatePropagation();
        var RequestID = $(this).data("requestid");
        var RequestNumber = $(this).closest("tr").data("requestnumber");

        ShowProgressBar();
        $.get("/KioskRequestAdministrator/GetAssignResponsibleModal", {
            RequestID: RequestID
        }).done(function (data) {

            $("#root_modal").html(data);
            $("#ddl_ResponsiblesList").selectpicker();
            $("#mo_AssignRequest").modal("show");

            //$("#txt_AssignResponsible").focus();
            $("#Mo_AssRes_RequestID").val(RequestID);
            $("#span_RequestNumbers").text(RequestNumber);
            //AutoCompleteDrop();
            //document.getElementsByName("Mo_AssRes_RequestID")[0].value = RequestID

            HideProgressBar();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", ".btn_TblCancelRequest", function (e) {
        e.stopImmediatePropagation();
        var RequestNumber = $(this).closest("tr").data("requestnumber");
        var RequestID = $(this).closest("tr").data("requestid");
        RequestStatusChange(LangResources.lbl_CancelRequest, RequestNumber, LangResources.lbl_CancelRequestAlertText, "Cancel", RequestID);
    });

    $(document).on("click", ".btn_TblRejectRequest", function (e) {
        e.stopImmediatePropagation();
        var RequestNumber = $(this).closest("tr").data("requestnumber");
        var RequestID = $(this).closest("tr").data("requestid");
        RequestStatusChange(LangResources.lbl_RejectRequest, RequestNumber, LangResources.lbl_RejectRequestAlertText, "Reject", RequestID);
    });

    $(document).on("click", ".btn_TblMarkDoneRequest", function (e) {
        e.stopImmediatePropagation();
        var RequestNumber = $(this).closest("tr").data("requestnumber");
        var RequestID = $(this).closest("tr").data("requestid");
        RequestStatusChange(LangResources.lbl_MarkDone, RequestNumber, null, "MarkDone", RequestID);

    });

    $(document).on("click", ".btn_TblReOpen", function (e) {
        e.stopImmediatePropagation();
        var RequestNumber = $(this).closest("tr").data("requestnumber");
        var RequestID = $(this).closest("tr").data("requestid");
        RequestStatusChange(LangResources.lbl_ReOpenRequest, RequestNumber, LangResources.lbl_ReOpenRequestAlertText, "ReOpen", RequestID);

    });

    $(document).on("click", ".btn_TblClose", function (e) {
        e.stopImmediatePropagation();
        var RequestNumber = $(this).closest("tr").data("requestnumber");
        var RequestID = $(this).closest("tr").data("requestid");
        RequestStatusChange(LangResources.lbl_CloseRequest, RequestNumber, LangResources.lbl_RejectRequestAlertText, "Close", RequestID);

    });
    //End: Eventos para cambios de status individuales



    $("#ddl_RequestStatus").change(function () {
        if ($("#ddl_RequestStatus option:selected").length == 0) {
            $("#ddl_RequestStatus").attr("title", "");
            $("#ddl_RequestStatus").selectpicker("refresh");
        }
    });

    $("#ddl_RequestTypes").change(function () {
        if ($("#ddl_RequestTypes option:selected").length == 0) {
            $("#ddl_RequestTypes").attr("title", "");
            $("#ddl_RequestTypes").selectpicker("refresh");
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
                tr.after('<tr><td colspan="13" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });

    $(document).on('click', ".btn-read-notify", function (e) {
        e.stopImmediatePropagation();
        var button = $(this);
        var RequestNotificationIDs = "";
        $(".notification_count").each(function () {
            RequestNotificationIDs += ", " + $(this).data("requestnotificationid");
        });

        RequestNotificationIDs = RequestNotificationIDs.substr(2, RequestNotificationIDs.length);
        $.post("/KioskRequestAdministrator/SetUserNotificationsReaded", {
            RequestNotificationIDs: RequestNotificationIDs
        }).done(function (data) {
            $("#request_count").text("0");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            //HideProgressBar();
        });
    });

    $(document).on("click", "#btnSearchUser", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/HR/KioskRequestAdministrator/SearchADUsers', {
            UserText: $('#txtSearchUser').val()
        }).done(function (data) {
            $("#div_ResponsiblesResultTable").html(data.View);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '.addUserAccount', function (e) {
        e.stopImmediatePropagation();
        var info = $(this);
        var User = {
            UserAccountID: info.data("useraccountid"),
            FirstName: info.data("firstname"),
            eMail: info.data("email")
        }

        ShowProgressBar();
        $.post('/HR/KioskRequestAdministrator/AddResponsibleAccount', {
            User: User
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);

            if (data.ErrorCode == 0) {
                LoadResponsibles(data.ID);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });


    function AutoCompleteDrop() {
        ShowProgressBar();

        $.get("/KioskRequestAdministrator/FilterResponsible", {
            SearchUserInfo: $("#txt_AssignResponsible").val()
        }).done(function (data) {
            console.log(data);
            $("#txt_AssignResponsible").empty();
            $.each(data.ResponsiblesList, function (k, v) {
                $("#txt_AssignResponsible").append(
                    "<option id=" + v.ID + " value=" + v.ID + " data-name=" + v.FullName + " data-empnumber=" + v.EmployeeNumber + " data-department=" + v.DepartmentName + ">" + v.FullName + "</option>"
                );
                idSelectedAsign = v.ID;
                //$("#datalist-responsibles").append($("<option>").attr('value', v.FullName).attr("data-id", v.UserID));

            });
            $("#txt_AssignResponsible").selectpicker();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    }

    function LoadResponsibles(IDToSelect) {
        $.get("/KioskRequestAdministrator/GetRequestResponsibles", {
            UserText: null
        }).done(function (data) {
            $("#txt_AssignResponsible").empty();
            $.each(data.RequestResponsiblesList, function (k, v) {
                $("#txt_AssignResponsible").append('<option value="' + v.ID + '">' + v.FullName + '</option>')
            });

            if (IDToSelect != null) {
                $("#txt_AssignResponsible").val(IDToSelect);
                $("#txt_AssignResponsible").selectpicker("refresh");
            }
            LoadUserInfo();
            $("#mo_AddNewUser").modal("toggle");
        });
    }

    function LoadUserInfo() {
        var option = $("#txt_AssignResponsible option:selected");
        var Department = option.data("department");
        var EmpNumber = option.data("employeenumber");

        $("#Mo_ResponsibleName").text(option.text());
        if (Department != null && Department != "") {
            $("#Mo_ResponsibleDepartment").text(Department);
        }
        if (EmpNumber != null && EmpNumber != "") {
            $("#Mo_ResponsibleNum").text(EmpNumber);
        }
    }

    $(document).on("keyup", "#txt_Responsible", function (e) {
        e.stopImmediatePropagation();
        var long = $("#txt_Responsible").val().length;
        if (long > 1) {
            $.get("/KioskRequestAdministrator/GetRequestResponsibles", {
                UserText: $("#txt_Responsible").val()
            }).done(function (data) {
                $("#dtl_RequestResponsibles").empty();
                $.each(data.RequestResponsiblesList, function (k, v) {
                    $("#dtl_RequestResponsibles").append('<option>' + v.FullName + '</option>')
                });
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            });
        }
    });

    $(document).on("keydown", "#txt_RequestNumber,#txt_Responsible", function (e) {
        e.stopImmediatePropagation();
        if (e.keyCode == 13) {
            return false;
        }
    })

    $(document).on("keyup", "#txt_RequestNumber", function (e) {
        e.stopImmediatePropagation();
        var long = $("#txt_RequestNumber").val().length;
        if (long > 1) {
            $.get("/KioskRequestAdministrator/GetRequestNumbers", {
                RequestNumber: $("#txt_RequestNumber").val()
            }).done(function (data) {
                $("#dtl_RequestNumbers").empty();
                $.each(data.RequestNumbersList, function (k, v) {
                    $("#dtl_RequestNumbers").append('<option>' + v.RequestNumber + '</option>')
                });
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            });
        }
    });

    $(document).on("shown.bs.modal", "#mo_AssignRequest", function (e) {
        $("#txta_Comment").focus();
        $("#txta_Comment").maxlength();
        $("#txt_AssignResponsible").selectpicker();
    });

    //Add a New User
    $(document).on('change', '#txt_AssignResponsible', function (e) {
        e.stopImmediatePropagation();
        var option = $('#txt_AssignResponsible option:selected').val();
        if (option == "new") {
            ShowProgressBar();
            $.get("/KioskRequestAdministrator/GetAddUserModal").done(function (data) {
                $("#div_add_new_user").html(data);
                $(".select").selectpicker();
                $("#mo_AddNewUser").modal("show");
                $('#txt_AssignResponsible').val(0);
                $('#txt_AssignResponsible').selectpicker("refresh");

            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            LoadUserInfo();
        }
    });

    $(document).on("click", "#btn_CloseAddUserModal", function (e) {
        $("#mo_AddNewUser").modal("toggle");
    });

    $(document).on("click", "#btn_AddUser", function (e) {
        var ProfileField = $('#ProfileID').val();
        var ProfileArrayID = "";

        if (ProfileField != null) {
            for (var i = 0; i < ProfileField.length; i++) {
                ProfileArrayID = ProfileField[i] + "," + ProfileArrayID;
            }
        }

        ShowProgressBar();
        $.post("/Users/Create", {
            UserAccountID: $("#UserAccountID").val(),
            eMail: $("#eMail").val(),
            EmployeeNumber: $("#EmployeeNumber").val(),
            FirstName: $("#FirstName").val(),
            LastName: $("#LastName").val(),
            ProfileID: $("#ProfileID").val(),
            DepartmentID: $("#DepartmentID").val(),
            ShiftID: $("#ShiftID").val(),
            DefaultCultureID: $("#DefaultCultureID").val(),
            ProfileArrayID: ProfileArrayID
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                LoadResponsibles(data.ID);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

}

function ReadNotification(id) {
    var idElement = id;
    var count;
    var RequestNotificationIDs = "";
    RequestNotificationIDs = id;

    ShowProgressBar();
    $.post("/KioskRequestAdministrator/SetUserNotificationsReaded", {
        RequestNotificationIDs: RequestNotificationIDs
    }).done(function (data) {
        count = $("#request_count").text() - 1;
        $("#request_count").text(count);
        $("#request_notification_" + idElement).css("display", "none").remove();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });

}

function RequestLogNotificationMo(NotificationID) {
    ShowProgressBar();
    $.get("/KioskRequestAdministrator/GetRequestLogNotificationID", {
        RequestLogNotificationID: NotificationID
    }).done(function (data) {
        $("#root_modal").html(data);
        $("#mo_RequestLogNotification").modal("show");
        HideProgressBar();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}

function ReadAllPrivate() {
    var RequestNotificationIDs = "";
    $(".notification_count").each(function () {
        RequestNotificationIDs += ", " + $(this).data("requestnotificationid");
    });

    RequestNotificationIDs = RequestNotificationIDs.substr(2, RequestNotificationIDs.length);
    $.post("/KioskRequestAdministrator/SetUserNotificationsReaded", {
        RequestNotificationIDs: RequestNotificationIDs
    }).done(function (data) {
        $("#request_count").text("0");
        document.getElementById('mCSB_1').remove();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        //HideProgressBar();
    });

}
