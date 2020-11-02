// =============================================================================================================================
//  Version: 20200211
//  Author:  Luis Hernandez
//  Created Date: 11 Febrero 2020
//  Description:  Scripts para el programa oportunidades
//  Modifications: 
// =============================================================================================================================

function IndexInit(LangResources) {

    //Functions
    function EnableDateFilterOpportunitiesProgram() {
        if ($('.check-range-date').is(':checked')) {
            $('#checkdate').val('true');
            $('#txt_OpportunitiesProgramStartDate').prop("disabled", false);
            $('#txt_OpportunitiesProgramEndDate').prop("disabled", false);
        } else {
            $('#checkdate').val('false');
            $('#txt_OpportunitiesProgramStartDate').prop("disabled", true);
            $('#txt_OpportunitiesProgramEndDate').prop("disabled", true);
        }
    }
    function SearchOpportunitiesPrograms(pageNumber) {
        var DepartmentID = $("#ddl_Departments option:selected").val();
        var StartDate = $("#txt_OpportunitiesProgramStartDate").val();
        var EndDate = $("#txt_OpportunitiesProgramEndDate").val();
        var ShiftIDs = $("#ddl_Shifts").val();
        var Grades = $("#ddl_Grades").val();


        if (!($("#checkbox_filter_date").is(":checked"))) {
            StartDate = null;
            EndDate = null;
        }

        if (ShiftIDs != null) {
            ShiftIDs = ShiftIDs.join(",");
        }

        if (Grades != null) {
            Grades = Grades.join(",");
        }

        ShowProgressBar();
        $.get("/OpportunitiesProgram/Search", {
            OpportunityNumber: $("#txt_NumVacant").val(),
            DateTypeID: $("#ddl_DateTypes option:selected").val(),
            DepartmentID: DepartmentID,
            ShiftIDs: ShiftIDs,
            Grades: Grades,
            StartDate: StartDate,
            EndDate: EndDate
        }).done(function (data) {
            $("#div_Tbl_KioskOpportunitiesPrograms").html(data);
            InitializeDataTable();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }
    function EnabledDisabledOpportinities(OpportunityProgramID, IsEnabled, NotificationTypeID) {
        if (NotificationTypeID == null) {
            NotificationTypeID = $("#_Mo_NotificationTypeID").val();
        }

        $.post("/OpportunitiesProgram/SetEnableDisable", {
            OpportunityProgramID: OpportunityProgramID,
            Enabled: IsEnabled,
            NotificationTypeID: NotificationTypeID
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                SearchOpportunitiesPrograms(1);
            }
        });
    }
    function OpenModalNotification(Row, MassiveOrSingleCandidate) {

        if (Row.find("td:nth-child(7)").text().trim() != "0") {
            var OpportunityID = Row.data("opportunityprogramid");
            var CandidateID = Row.data("candidateid");
            var OpportunityNumber = Row.find('td:eq(1)').text();

            if (MassiveOrSingleCandidate == "Single") {
                OpportunityNumber = Row.parent().parent().parent().parent().parent().parent().find('td:eq(1)').text();
            }

            ShowProgressBar();
            $.get("/OpportunitiesProgram/GetNotificationModal", {
                MassiveOrSingleCandidate: MassiveOrSingleCandidate
            }).done(function (data) {
                $("#div_modal").html(data);
                $("#mo_Notification").modal("show");
                $("#OpportunityProgramID").val(OpportunityID);
                $("#span_vacant_numbers").text(OpportunityNumber);
                $("#Mo_NotificationCandidateID").val(CandidateID);

                $(".max-length").maxlength();
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            notification("", LangResources.msg_ThereNotCandidatesToNotify, "_ntf")
        }
    }
    function OpenModalDiscardAccept(Row) {
        var OpportunityID = Row.data("opportunityprogramid");
        var CandidateID = Row.data("candidateid");
        var OpportunityNumber = Row.find('td:eq(1)').text();
        var IsDiscarted = Row.data("isdiscarted");
        var OpportunityNumber = Row.parent().parent().parent().parent().parent().parent().find('td:eq(1)').text();

        ShowProgressBar();
        $.get("/OpportunitiesProgram/GetDiscardCandidateModal", {
            CandidateID: CandidateID,
            IsDiscarted: IsDiscarted
        }).done(function (data) {
            $("#div_modal").html(data);
            $("#mo_Notification").modal("show");
            $("#OpportunityProgramID").val(OpportunityID);
            $("#span_vacant_numbers").text(OpportunityNumber);
            $("#Mo_NotificationCandidateID").val(CandidateID);
            $("#Mo_CandidateIsDiscarted").val(IsDiscarted);

            $(".max-length").maxlength();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }
    function SendNotifications(CandidateID) {
        $.post("/OpportunitiesProgram/SendNotifications", {
            OpportunityProgramID: $("#OpportunityProgramID").val(),
            Comment: $("#txta_Mo_NotificationComments").val(),
            CandidateID: CandidateID
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                $("#mo_Notification").modal("toggle");
                $("#OpportunityProgramID").val(0);
                $("#_Mo_NotificationTypeID").val(0);
            }
        });
    }
    function InitializeDatePicker() {
        var CultureID = $("#CultureID").val();
        var region = $('.SelectCulture_' + CultureID).data("langabreviation");
        $(".customdatepicker").datepicker({
            format: 'yyyy-mm-dd',
            language: region
        });
    }
    function InitializeDataTable() {


        $(".dataTable").dataTable({
            "language": {
                "url": "/Base/DataTableLang"
            },
            order: [[7, "desc"]],
            "pageLength": 100
        });
    }


    //Initialize
    EnableDateFilterOpportunitiesProgram();
    $('#txt_NumVacant').maxlength();
    $("#checkbox_filter_date").iCheck();
    $("select").selectpicker();
    $("select[multiple]").selectpicker("selectAll");
    InitializeDatePicker();
    InitializeDataTable();
    $(".max-length").maxlength();

    //Events
    $(document).on("ifChanged", ".check-range-date", function (e) {
        e.stopImmediatePropagation();
        EnableDateFilterOpportunitiesProgram();
    });

    $(document).on("click", "#btn_OpportunitiesProgramSearch", function (e) {
        e.stopImmediatePropagation();
        SearchOpportunitiesPrograms(1);
    });

    $(document).on("click", "#btn_NewOpportunity", function (e) {
        ShowProgressBar();
    });

    $(document).on("click", "#btn_CloseNewOpportunityModal", function (e) {
        $("#mo_NewOpportunity").modal("toggle");
    });

    $(document).on("click", ".edit-opportunity-program", function (e) {
        e.stopImmediatePropagation();
        var OpportunityProgramID = $(this).closest("tr").data("opportunityprogramid");

        ShowProgressBar();
        window.location = "/HR/OpportunitiesProgram/NewEditOpportunity?OpportunityProgramID=" + OpportunityProgramID;
    });

    //expandir detalles con plugin DataTable
    $(document).on('click', 'td.details-control', function (e) {
        e.stopPropagation();

        var tr = $(this).closest('tr');

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {

            var OpportinutyProgramID = tr.data("opportunityprogramid");

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/HR/OpportunitiesProgram/GetCandidatesTable", {
                OpportinutyProgramID
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="13" style="padding:0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });


    //Eventos de botones de la tabla de oportunidaes
    $(document).on("click", ".disable-opportunity-program-single", function () {
        var Row = $(this).closest("tr");
        SetConfirmBoxAction(function () {
            var OpportunityProgramID = Row.data("opportunityprogramid");
            EnabledDisabledOpportinities(OpportunityProgramID, false, $("#NotificationTypeID").val());
        }, LangResources.msg_DisableVacantConfirm);
    });

    $(document).on("click", ".enable-opportunity-program-single", function () {
        var Row = $(this).closest("tr");
        SetConfirmBoxAction(function () {
            var OpportunityProgramID = Row.data("opportunityprogramid");
            EnabledDisabledOpportinities(OpportunityProgramID, true, $("#NotificationTypeID").val());
        }, LangResources.msg_EnableVacantConfirm);
    });

    $(document).on("click", ".notify-opportunity-program-single", function () {
        OpenModalNotification($(this).closest("tr"), "Massive");
    });

    $(document).on("click", ".notify-opportunity-program-candidate", function () {
        OpenModalNotification($(this).closest("tr"), "Single");
    });

    $(document).on("click", "#btn_CloseSendNotificationModal", function () {
        $("#mo_Notification").modal("toggle");
        $("#OpportunityProgramID").val(0);
        $("#_Mo_NotificationTypeID").val(0);
    });


    $(document).on("click", "#btn_Mo_OpportunityProgram_SendMassiveNotification", function () {
        SetAlertConfirmCustomBoxAction(function () {
            SendNotifications();
        }, LangResources.msg_SendNotificationConfirm, LangResources.lbl_Warning, "info");
    });

    $(document).on("click", "#btn_Mo_OpportunityProgram_SendSingleNotification", function () {
        var CandidateID = $("#Mo_NotificationCandidateID").val();
        SetAlertConfirmCustomBoxAction(function () {
            SendNotifications(CandidateID);
        }, LangResources.msg_SendNotificationConfirm, LangResources.lbl_Warning, "info");

    });

    $(document).on("click", ".discard-opportunity-program-candidate", function () {
        OpenModalDiscardAccept($(this).closest("tr"));
    });

    $(document).on("click", ".accept-opportunity-program-candidate", function () {
        OpenModalDiscardAccept($(this).closest("tr"));
    });

    $(document).on("click", "#btn_Mo_OpportunityProgram_AcceptDiscardCandidate", function () {
        var ExpandControl = $("#candidates_row_" + $("#Mo_NotificationCandidateID").val()).parent().parent().parent().parent().parent().parent().find('td:eq(0)');
        var IsDiscarted = $("#Mo_CandidateIsDiscarted").val();

        $.post("/OpportunitiesProgram/UpdateCandidates", {
            OPCandidateID: $("#Mo_NotificationCandidateID").val(),
            OpportunityProgramID: $("#OpportunityProgramID").val(),
            ShortMessage: $("#txta_Mo_NotificationComments").val(),
            IsDiscarted: IsDiscarted
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                $("#mo_Notification").modal("toggle");
                ExpandControl.click();
            }
        });
    });

    $(document).on("change", ".OpportunitiesProgramFilters", function () {
        $("#div_Tbl_KioskOpportunitiesPrograms").empty();
    });

    $(document).on("keydown", ".OpportunitiesProgramFilters", function () {
        $("#div_Tbl_KioskOpportunitiesPrograms").empty();
    });

}