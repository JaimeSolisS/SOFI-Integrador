// =============================================================================================================================
//  Version: 20190506
//  Author:  Luis Hernandez
//  Created Date: 05 Jun 2019
//  Description:  MachineSetup JS
//  Modifications: 
// =============================================================================================================================

function IndexInit(LangResources) {

    function LoadOperationTask() {
        $('.loading-process-div').show();
        $.post('/MFG/ActionPlan/Search').done(function (data) {
            $("#div_Tbl_OperationTask").empty();
            $("#div_Tbl_OperationTask").append(data);
            $('.loading-process-div').hide();
            $("#div_Tbl_OperationTask").css("display", "block");
            $(".details-control").on("click", function () {
                var tr = $(this).closest("tr");

                if (tr.hasClass("shown")) {
                    $('div.slider', tr.next()).slideUp(function () {
                        tr.next().remove();
                        tr.removeClass('shown');
                    });
                } else {

                    var OperationTaskID = tr.data("entityid");
                    ShowProgressBar();
                    $.get("/MFG/ActionPlan/LoadOperationTaskDetails", { OperationTaskID }
                    ).done(function (data) {
                        tr.after('');
                        tr.after('<tr><td colspan="11" class="padding-0">' + data + '</td></tr>');
                        tr.addClass('shown');
                        $('div.slider', tr.next()).slideDown();
                        HideProgressBar();
                    });
                }
            });
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }
    function EnableDateFilter() {
        if ($("#EnabledOpTask").is(":checked")) {
            $(".DateDisable").prop('disabled', false);
        } else {
            $("#ddl_DateTypeOpTask").val(0);
            $(".DateDisable").prop('disabled', true);
        }
        $(".DateDisable").selectpicker("refresh");
    }
    $(".selectpicker").selectpicker();

    $(".datepicker").datepicker();

    $(".datetimepicker").datetimepicker({
        format: 'YYYY[-]MM[-]DD HH:mm'
    });

    $("input.max-length").maxlength();

    (function SetInputValues() {
        EnableDateFilter();
    })();

    $("#btn_search").on("click", function (e) {
        e.stopImmediatePropagation();

        var MachineID = $("#ddl_MachinesOpTask option:selected").val();
        var ShiftID = $("#ddl_ShiftsOpTask option:selected").val();
        var StatusID = $("#ddl_StatusOpTask option:selected").val();
        var ResponsibleName = $("#ResponsableOpTask").val();
        var DateType = $("#ddl_DateTypeOpTask option:selected").val();
        var Enabled = $("#EnabledOpTask").is(":checked");
        var StartDate = $("#StartDateOpTask").val();
        var EndDate = $("#EndDateOpTask").val();

        $('.loading-process-div').show();
        $.post('/MFG/ActionPlan/Search', {
            ResponsibleName,
            MachineID,
            ShiftID,
            DateType,
            StartDate,
            EndDate,
            StatusID
        }).done(function (data) {
            $("#div_Tbl_OperationTask").empty();
            $("#div_Tbl_OperationTask").append(data);
            $("#div_Tbl_OperationTask").css("display", "block");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });

    });

    $("#EnabledOpTask").on("change", function () {
        EnableDateFilter();
    });

    $(".details-control").on("click", function () {
        var tr = $(this).closest("tr");

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {

            var OperationTaskID = tr.data("entityid");
            ShowProgressBar();
            $.get("/MFG/ActionPlan/LoadOperationTaskDetails", { OperationTaskID }
            ).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="11" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });

    $(document).on("click", ".assigned", function () {
        //var dateNow = new Date();

        $('.loading-process-div').show();
        var OperationTaskID = $(this).closest("tr").data("entityid");

        $.get("/MFG/ActionPlan/GetModalAssignTask", { OperationTaskID }).done(function (data) {
            $('.loading-process-div').hide();

            if (data.ErrorCode == 0) {
                $("#div_MO_AssignTask").html(data.View);

                $("#mo_AssignTask").modal("show");
                $(".select").selectpicker();

                if ($("#CultureID").val() == "ES-MX") {
                    $('#assignTask_date').datetimepicker({
                        useCurrent: false,
                        format: 'DD[-]MM[-]YYYY'
                    });
                } else {
                    $('#assignTask_date').datetimepicker({
                        useCurrent: false,
                        format: 'MM[-]DD[-]YYYY'
                    });
                }

                $("#assignTask_time").datetimepicker({
                    format: 'HH:mm'
                });

            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    });

    $(document).on("click", ".closed", function () {
        $('.loading-process-div').show();
        var OperationTaskID = $(this).closest("tr").data("entityid");

        $.get("/MFG/ActionPlan/GetModalCloseTask", { OperationTaskID }).done(function (data) {
            $('.loading-process-div').hide();

            if (data.ErrorCode == 0) {
                $("#div_MO_CloseTask").html(data.View);
                $("#mo_CloseTask").modal("show");
                $("#closeTask_date").datepicker().datepicker('setDate', new Date());
                $(".select").selectpicker();
                //Configuracion de attachments
                LoadDropzone('.form-dropzone', function () {
                    var ReferenceID = $('#ReferenceID').val();
                    var AttachmentType = $('#AttachmentType').val();
                    $.get("/Attachments/Get",
                        { ReferenceID, AttachmentType }
                    ).done(function (data) {
                        $('#AttachmentsTable').html('');
                        $('#AttachmentsTable').html(data);
                        $(".selectpicker").selectpicker();
                    });
                });
                SetDownloadAttachments('.download-attachment');
                SetupUpdateTypeAttachment('.attachment-file-type');
                SetDeleteAttachments('.delete-attachment-row', LangResources, function () {
                    var ReferenceID = $('#ReferenceID').val();
                    var AttachmentType = $('#AttachmentType').val();
                    $.get("/Attachments/Get",
                        { ReferenceID, AttachmentType }
                    ).done(function (data) {
                        $('#AttachmentsTable').html('');
                        $('#AttachmentsTable').html(data);
                        $(".selectpicker").selectpicker();
                    });
                });
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });
    });

    $(document).on("click", ".cancelled", function (e) {
        e.stopPropagation();

        //SetConfirmBoxAction(function () {
        //    alert("alert");
        //}, LangResources.ntf_MsgDeleteReject);

        $('.loading-process-div').show();
        var OperationTaskID = $(this).closest("tr").data("entityid");

        $.post("/MFG/ActionPlan/SetOperationTaskCancelled", { OperationTaskID }).done(function (data) {
            $('.loading-process-div').hide();

            if (data.ErrorCode == 0) {
                notification("", LangResources.msg_TaskCancelled, data.notifyType);
                LoadOperationTask();

            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });
    });

    $(document).on("click", ".view_attachments", function () {
        $('.loading-process-div').show();
        var OperationTaskDetailID = $(this).closest("tr").data("entityid");

        $.get("/MFG/ActionPlan/GetModalAttachments", { OperationTaskDetailID }).done(function (data) {
            $('.loading-process-div').hide();

            if (data.ErrorCode == 0) {
                $("#div_MO_Attachments").html(data.View);
                $("#mo_Attachments").modal("show");

                LoadDropzone('.form-dropzone', function () {
                    var ReferenceID = $('#ReferenceID').val();
                    var AttachmentType = $('#AttachmentType').val();
                    $.get("/Attachments/Get",
                        { ReferenceID, AttachmentType }
                    ).done(function (data) {
                        $('#AttachmentsTable').html('');
                        $('#AttachmentsTable').html(data);
                        $(".selectpicker").selectpicker();
                    });
                });
                SetDownloadAttachments('.download-attachment');
                SetupUpdateTypeAttachment('.attachment-file-type');
                SetDeleteAttachments('.delete-attachment-row', LangResources, function () {
                    var ReferenceID = $('#ReferenceID').val();
                    var AttachmentType = $('#AttachmentType').val();
                    $.get("/Attachments/Get",
                        { ReferenceID, AttachmentType }
                    ).done(function (data) {
                        $('#AttachmentsTable').html('');
                        $('#AttachmentsTable').html(data);
                        $(".selectpicker").selectpicker();
                    });
                });

            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });
    })

    $(document).on("keydown", ".date_disable", function () {
        return false;
    });

    $(document).on("click", "#btn_AssignResponsableTask", function (e) {
        e.stopImmediatePropagation();

        var OperationTaskID = $("#Mo_OperationTaskID").val();
        var ResponsableID = $("#assignTask_user option:selected").val();
        var TargetDate = $("#assignTask_dates").val();
        var SuggestedAction = $("#assignTask_sugestedAction").val();
        var TargetTime = $("#assignTask_times").val();
        var AttendantUserName = $("#assignTask_attendantUserName").val();


        if (ResponsableID != 0) {
            $('.loading-process-div').show();
            $.post('/MFG/ActionPlan/UpdateOperationTaskResponsable', {
                OperationTaskID,
                ResponsableID,
                SuggestedAction,
                AttendantUserName,
                TargetDate,
                TargetTime
            }).done(function (data) {
                $('.loading-process-div').hide();
                if (data.ErrorCode == 0) {
                    $("#mo_AssignTask").modal("toggle");
                    notification("", LangResources.ActionTaskUpdateSuccess, "success");
                    LoadOperationTask();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                $('.loading-process-div').hide();
            });
        } else {
            notification("", LangResources.UserIsMandatory, "ntf_");
        }
    });

    $(document).on("click", "#btn_CloseTask", function () {
        $('.loading-process-div').show();
        var OperationTaskDetailID = 0;

        var isValid = true;
        var OperationTaskID = $("#Mo_OperationTaskID").val();
        var Comments = $("#txta_comments").val();
        var Close = $("#closeOpTask").is(":checked");
        var CloseDate = $("#closeTask_date").val();

        var ReferenceID = $('#ReferenceID').val();
        var AttachmentType = $('#AttachmentType').val();
        var NewReferenceID = 0;
        var NewAttachmentType = AttachmentType;

        if (Comments == "") {
            isValid = false;
        }

        if (isValid) {
            $.post("/MFG/ActionPlan/SaveCloseTask", {
                OperationTaskID,
                Comments,
                Close,
                CloseDate
            }).done(function (data) {
                $('.loading-process-div').hide();

                if (data.ErrorCode == 0) {
                    notification("", LangResources.msg_TaskUpdate, data.notifyType);
                    var ID = data.ID;

                    NewReferenceID = ID;
                    $.post("/Attachments/Copy", {
                        ReferenceID,
                        AttachmentType,
                        NewReferenceID,
                        NewAttachmentType
                    }).done(function (data) {
                        $('.loading-process-div').hide();

                        if (data.ErrorCode == 0) {
                            notification("", LangResources.msg_TaskUpdate, data.notifyType);
                            $('.loading-process-div').hide();
                            //LoadOperationTask();
                            $('#btn_search').click();
                        } else {
                            notification("", data.ErrorMessage, data.notifyType);
                        }
                    });

                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            });
        }

        if (!isValid) {
            notification("", 'You must write a comment for this task.', 'error');
            $('.loading-process-div').hide();
        } else {
            $("#mo_CloseTask").modal("toggle");
        }

    });
}