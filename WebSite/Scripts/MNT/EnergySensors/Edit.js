function EditInit(LangResources) {

    $(".selectpicker").selectpicker("refresh");


    function GetEnergySensorFamiliesList(LangResources, DefaultValue) {
        $('.loading-process-div').show();
        $.get("/MNT/EnergySensors/GetEnergySensorFamiliesList").done(function (data) {
            if (data.ErrorCode == 0) {
                var datos = data.FamiliesList;

                $("#ddl_family").empty();
                $("#ddl_family").append("<option value='0'>" + LangResources.lbl_SelectFamily + "</option>");
                $("#ddl_family").append("<option value='a' class='new-family'>--" + LangResources.lbl_NewFamily + "--</option>");


                $.each(datos, function () {
                    $("#ddl_family").append("<option value='" + this["Value"] + "'>" + this["Text"] + "</option>");
                });

                if ($("#EnergySensorID").val() != 0) {
                    var EnergySensorFamilyID = $("#EnergySensorFamilyID").val();
                    $("#ddl_family").val(EnergySensorFamilyID);
                    $("#ddl_family").selectpicker("refresh");

                }

                if (DefaultValue != null) {
                    $("#ddl_family").val(DefaultValue);
                }

                $("#ddl_family").selectpicker("refresh");
            }
            $('.loading-pcrocess-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }

    function GetEnergySensorUsesList(LangResources) {
        $('.loading-process-div').show();
        $.get("/MNT/EnergySensors/GetEnergySensorUsesList").done(function (data) {
            if (data.ErrorCode == 0) {
                var datos = data.UsesList;

                $("#ddl_Use").append("<option value='0'>" + LangResources.lbl_SelectUse + "</option>");
                //$("#ddl_Use").append("<option value='a' class='new-use'>--" + LangResources.lbl_NewUse + "--</option>");


                $.each(datos, function () {
                    $("#ddl_Use").append("<option value='" + this["Value"] + "'>" + this["Text"] + "</option>");
                });

                if ($("#EnergySensorID").val() != 0) {
                    var SensorUseID = $("#SensorUseID").val();
                    $("#ddl_Use").val(SensorUseID);
                }

                $("#ddl_Use").selectpicker("refresh");
            }
            $('.loading-process-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }

    function GetEnergySensorUnitsList(LangResources) {
        $('.loading-process-div').show();
        $.get("/MNT/EnergySensors/GetEnergySensorUnitsList").done(function (data) {
            if (data.ErrorCode == 0) {
                var datos = data.UnitsList;

                $("#ddl_unit").append("<option value='0'>" + LangResources.lbl_SelectUnit + "</option>");
                //$("#ddl_unit").append("<option value='a' class='new-unit'>--" + "nueva unidad" + "--</option>");


                $.each(datos, function () {
                    $("#ddl_unit").append("<option value='" + this["Value"] + "'>" + this["Text"] + "</option>");
                });

                if ($("#EnergySensorID").val() != 0) {
                    var UnitID = $("#UnitID").val();
                    $("#ddl_unit").val(UnitID);
                }

                $("#ddl_unit").selectpicker("refresh");
            }
            $('.loading-process-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }

    function SaveAlarmConfiguration(LangResources, EnergySensorID, MaxValuesFourHours) {
        $('.loading-process-div').show();
        $.post("/MNT/EnergySensors/SaveAlarmConfiguration", { EnergySensorID, MaxValuesFourHours }).done(function () {

        }).fail(function () {
            //notification("", data.ErrorMessage, data.notifyType);
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }

    var ReferenceID = 0;

    GetEnergySensorFamiliesList(LangResources, null);
    GetEnergySensorUsesList(LangResources);
    GetEnergySensorUnitsList(LangResources);

    SetupOnlyDecimal();
    SetupOnlyNumbers();

    if ($("#h_sensorName").text() == "") {
        $("#h_sensorName").text(LangResources.lbl_NewSensor);
    }

    $("#ddl_family").on("change", function (e) {
        e.stopPropagation();
        if ($(this).val() == "a") {
            $.get("/MNT/EnergySensors/GetModalAddNewFamily").done(function (data) {
                if (data.ErrorCode == 0) {
                    $("#div_Mo_AddNewFamily").html(data.View);
                    $('.max-length').maxlength();
                    $("#mo_AddNewFamily").modal("show");
                    SetupOnlyDecimal();
                }
            })
        }
    });

    $("#btn_upload_image").on("click", function () {
        $('.loading-process-div').show();
        var EnergySensorID = null;
        var AttachmentType = "ENERGYSENSORID";

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
        }).fail(function () {
            //notification("", data.ErrorMessage, data.notifyType);
        }).always(function () {
            $('.loading-process-div').hide();
        });

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
        var EnergySensorFamilyID = null;

        $.post("/MNT/EnergySensors/AddNewFamily", { EnergySensorFamilyID, Name, MaxValue, ImagePath, Enabled }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#mo_AddNewFamily").modal("toggle");
                notification("", LangResources.msg_NewFamilyAdded, "success");
                NewReferenceID = data.ID;

                GetEnergySensorFamiliesList(LangResources, NewReferenceID);


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
                    //notification("", data.ErrorMessage, "error");
                }
            } else {
                notification("", data.ErrorMessage, "error");
            }
        })
    });

    $(document).on("click", "#btn_SaveNewEnergySensor", function () {
        var EnergySensorID = null;
        var EnergySensorFamilyID = $("#ddl_family option:selected").attr("value");
        var SensorName = $("#txt_name").val();
        var SensorUseID = $("#ddl_Use option:selected").attr("value");
        var SensorUnitID = $("#ddl_unit option:selected").attr("value");
        var IndexKey = $("#txt_index_key").val();
        var DeviceID = $("#txt_device_id").val();
        var Enabled = true;
        var NewReferenceID = 0;
        var AttachmentType = "TEMPID";
        var NewAttachmentType = "ENERGYSENSORID";

        var MaxValuesFourHours = [];
        var count = 0;
        $('.MaxValueByHour').each(function () {
            var MaxValue = $("#txt_ValurForHour_" + count).val();

            var Item = {
                ValueHour: count,
                MaxValue: MaxValue
            };
            MaxValuesFourHours.push(Item);
            count++
        });

        $.post("/MNT/EnergySensors/AddEnergySensor", { EnergySensorID, EnergySensorFamilyID, SensorName, SensorUseID, SensorUnitID, IndexKey, DeviceID, Enabled }).done(function (data) {
            NewReferenceID = data.ID;
            if (data.ErrorCode == 0) {

                SaveAlarmConfiguration(LangResources, NewReferenceID, MaxValuesFourHours);

                if ($("#AttachmentsTable tr").length > 1) {
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
                            notification("", data.ErrorMessage, data.notifyType);
                        }
                    });
                }

                notification("", LangResources.msg_SensorAdded, "success");
            } else {
                notification("", data.ErrorMessage, "error");
            }
        }).fail(function (xhr, textStatus, error) {
            //notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });

    });

    $(document).on("click", "#btn_SaveEditedEnergySensor", function () {
        var EnergySensorID = $("#EnergySensorID").val();
        var EnergySensorFamilyID = $("#ddl_family option:selected").attr("value");
        var SensorName = $("#txt_name").val();
        var SensorUseID = $("#ddl_Use option:selected").attr("value");
        var SensorUnitID = $("#ddl_unit option:selected").attr("value");
        var IndexKey = $("#txt_index_key").val();
        var DeviceID = $("#txt_device_id").val();
        var Enabled = true;
        var NewReferenceID = EnergySensorID;
        var NewAttachmentType = "ENERGYSENSORID";
        var AttachmentType = $('#AttachmentType').val();

        var MaxValuesFourHours = [];
        var count = 0;
        $('.MaxValueByHour').each(function () {
            var MaxValue = $("#txt_ValurForHour_" + count).val();

            var Item = {
                ValueHour: count,
                MaxValue: MaxValue
            };
            MaxValuesFourHours.push(Item);
            count++
        });

        $.post("/MNT/EnergySensors/SaveEditedEnergySensor", { EnergySensorID, EnergySensorFamilyID, SensorName, SensorUseID, SensorUnitID, IndexKey, DeviceID, Enabled }).done(function (data) {
            if (data.ErrorCode == 0) {

                if (MaxValuesFourHours.length > 0) {
                    SaveAlarmConfiguration(LangResources, EnergySensorID, MaxValuesFourHours);
                }
                if ($("#AttachmentsTable tr").length > 1) {
                    $.post("/Attachments/Copy", {
                        ReferenceID,
                        AttachmentType,
                        NewReferenceID,
                        NewAttachmentType
                    }).done(function (data) {
                        $('.loading-process-div').hide();

                        if (data.ErrorCode == 0) {
                            //notification("", "asdasdasd", data.notifyType);
                            $('.loading-process-div').hide();
                            LoadOperationTask();
                        } else {
                            notification("", data.ErrorMessage, data.notifyType);
                        }
                    });
                }

                notification("", LangResources.msg_SensorUpdated, "success");
            } else {
                notification("", data.ErrorMessage, "error");
            }
        }).fail(function (xhr, textStatus, error) {
            //notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $(document).on("click", "#btn_SaveTempImg", function () {
        ReferenceID = $('#ReferenceID').val();
        AttachmentType = $('#AttachmentType').val();
        $("#mo_Attachments").modal("toggle");
    });

    $(document).on("click", "#btn_SetSameValue", function () {
        $(".MaxValueByHour").val($("#txt_SetSameValue").val());
    });

    $(document).on("click", "#btn_cancel", function () {
        MaxValuesFourHours = [];
        $("#mo_AlarmsConfiguration").modal("toggle");
    });

    $(document).on("click", "#btn_upload_image_family", function () {
        $('.loading-process-div').show();
        var EnergySensorID = null;
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

    $(document).on("change", "#ddl_SensorsConfigList", function () {
        var EnergySensorID = $("#ddl_SensorsConfigList option:selected").attr("value");
        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.get("/MNT/EnergySensors/GetEnergySensorValuesList", { EnergySensorID }).done(function (data) {
                var a = data.list;
                $.each(data.list, function (index, value) {
                    $("#txt_ValurForHour_" + value.Value).val(value.Text);
                });
            }).fail(function () {
                notification("", data.ErrorMessage, data.notifyType);
            }).always(function () {
                $('.loading-process-div').hide();
            });

        }, LangResources.msg_SetConfigurationValuesForAlarm);

    });

    LoadDropzone('.form-dropzone', function () {
        ReferenceID = $('#ReferenceID').val();
        var AttachmentType = $('#AttachmentType').val();

        $.get("/Attachments/Get",
            { ReferenceID, AttachmentType }
        ).done(function (data) {
            
            $('#AttachmentsTable').html('');
            $('#AttachmentsTable').html(data);
            $(".selectpicker").selectpicker();

            var Name = $("#AttachmentsTable tr").eq(1).find("td:nth-child(1)").text();

            $("#img_sensor").empty();
            $("#img_sensor").prepend('<img src="../../Files/TempFiles/' + ReferenceID + '/' + Name + '"  height="150" />');

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
}
