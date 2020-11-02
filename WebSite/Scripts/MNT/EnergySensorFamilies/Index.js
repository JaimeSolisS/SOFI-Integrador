function IndexInit(LangResources) {
    var ReferenceID = 0;

    $('.max-length').maxlength();

    $(document).on("click", ".familyEnergySensorDelete", function (e) {
        e.stopPropagation();
        var row = $(this).closest("tr");
        var EnergySensorFamilyID = row.attr("id");

        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.post("/MNT/EnergySensorFamilies/DeleteFamily", { EnergySensorFamilyID }).done(function (data) {
                if (data.ErrorCode == 0) {

                    notification("", LangResources.msg_FamilyDeleted, "success");
                    //document.location.reload(true);
                    row.remove();

                } else {
                    notification("", error.message, "error");
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }, LangResources.msg_ConfirmDeleteFamily);
    });

    $(document).on("click", ".familyEnergySensorEdit", function () {
        var EnergySensorFamilyID = $(this).closest("tr").attr("id");
        $.get("/MNT/EnergySensorFamilies/GetModalAddEditFamily", { EnergySensorFamilyID }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_Mo_AddNewFamily").html(data.View);
                $("#mo_AddNewFamily").modal("show");
                SetupOnlyDecimal();
            }
        })

    });

    $("#txt_FamilyName").on("keydown", function () {
        $("#div_FamiliesEnergySensorsDataPanel").css("display", "none");
    });

    $("#btn_search").on("click", function () {
        var FamilyName = $("#txt_FamilyName").val();
        Search(FamilyName);
    });

    $("#btn_NewFamily").on("click", function (e) {
        e.stopPropagation();
        var EnergySensorFamilyID = null;
        $('.loading-process-div').show();
        $.get("/MNT/EnergySensorFamilies/GetModalAddEditFamily", { EnergySensorFamilyID }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_Mo_AddNewFamily").html(data.View);
                $('.max-length').maxlength();
                $('.loading-process-div').hide();
                $("#mo_AddNewFamily").modal("show");
                SetupOnlyDecimal();
            }
        })
    });

    $(document).on("click", "#btn_AddNewFamily", function (e) {
        var Name = $("#AdNewF_Name").val();
        if (Name == null) { Name = "" };
        var MaxValue = $("#AdNewF_MaxValue").val();
        var ImagePath = "";
        var Enabled = true;
        ReferenceID = $('#ReferenceID').val();
        var AttachmentType = $('#AttachmentType').val();
        NewAttachmentType = AttachmentType;
        var EnergySensorFailyID = null;

        $.post("/MNT/EnergySensors/AddNewFamily", { EnergySensorFailyID, Name, MaxValue, ImagePath, Enabled }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#mo_AddNewFamily").modal("toggle");
                notification("", LangResources.msg_NewFamilyAdded, "success");
                NewReferenceID = data.ID;

                Search("");


                if (data.ErrorCode == 0) {
                    $.post("/Attachments/Copy", {
                        ReferenceID,
                        AttachmentType,
                        NewReferenceID,
                        NewAttachmentType
                    }).done(function (data) {
                        $('.loading-process-div').hide();
                        if (data.ErrorCode == 0) {
                            $('.loading-process-div').hide();
                        }
                    });
                } else {
                    //notification("", data.ErrorMessage, "error");
                }
            } else {
                //notification("", data.ErrorMessage, "error");
            }
        })
    });

    $(document).on("click", "#btn_SaveEditedFamily", function (e) {
        var Name = $("#AdNewF_Name").val();
        if (Name == null) { Name = "" };
        var MaxValue = $("#AdNewF_MaxValue").val();
        var ImagePath = "";
        var Enabled = true;
        ReferenceID = $('#ReferenceID').val();
        //var AttachmentType = $('#AttachmentType').val();
        var AttachmentType = "ENERGYSENSORFAMILYID";
        NewAttachmentType = AttachmentType;
        var EnergySensorFamilyID = $("#EnergySensorFamilyID").val();
        debugger;
        
        $.post("/MNT/EnergySensors/AddNewFamily", { EnergySensorFamilyID, Name, MaxValue, ImagePath, Enabled }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#mo_AddNewFamily").modal("toggle");
                notification("", LangResources.msg_EditedFamilySaved, "success");
                NewReferenceID = $("#EnergySensorFamilyID").val();//data.ID;

                Search("");


                if (data.ErrorCode == 0) {

                    $.post("/Attachments/Copy", {
                        ReferenceID,
                        AttachmentType,
                        NewReferenceID,
                        NewAttachmentType
                    }).done(function (data) {
                        $('.loading-process-div').hide();
                        if (data.ErrorCode == 0) {
                            $('.loading-process-div').hide();
                        } else {
                            //notification("", data.ErrorMessage, data.notifyType);
                        }
                    });
                } else {
                    notification("", data.ErrorMessage, "error");
                }
            } else {
                //notification("", data.ErrorMessage, "error");
            }
        })
    });

    $(document).on("click", "#btn_upload_image_family", function () {
        $('.loading-process-div').show();
        var EnergySensorID = $("#EnergySensorFamilyID").val();;
        var AttachmentType = "ENERGYSENSORFAMILYID";


        $.get("/MNT/EnergySensors/GetModalAttachments", { EnergySensorID, AttachmentType }).done(function (data) {
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

    });

    $(document).on("click", "#btn_SaveTempImg", function () {
        ReferenceID = $('#ReferenceID').val();
        AttachmentType = $('#AttachmentType').val();
        $("#mo_Attachments").modal("toggle");
    });

}

function Search(FamilyName) {
    $.get("/MNT/EnergySensorFamilies/Search", { FamilyName }).done(function (data) {
        $("#div_Tbl_FamiliesEnergySensors").html(data.View);
        $("#div_Tbl_FamiliesEnergySensors").css("display", "block");
        $("#div_FamiliesEnergySensorsDataPanel").css("display", "block");
    });

}