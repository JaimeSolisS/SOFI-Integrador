// =============================================================================================================================
//  Version: 20200804
//  Author:  Luis Hernandez
//  Created Date: 04 Julio 2020
//  Description:  SecurityGuard JS
//  Modifications: 

// =============================================================================================================================

function IndexInit(LangResources) {

    var CurrentTools = [];

    function ApplyMaxLength() {
        $(".max-length").maxlength();
    }

    function SetDatepickerPlugin(selector, culture) {
        $(selector).datepicker({
            autoclose: true,
            format: 'yyyy-mm-dd',
            language: culture
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

    function LoadDropzoneSelector(form_selector, imgpreview_selector, instruction_div_selector) {
        Dropzone.autoDiscover = false;
        $(form_selector).dropzone({
            addRemoveLinks: true,
            createImageThumbnails: false,
            maxFiles: 1,
            acceptedFiles: 'image/*',
            previewsContainer: false,
            init: function () {

                this.on("addedfile", function (file) {
                    ShowProgressBar();
                });

                this.on("maxfilesexceeded", function (file) {
                    this.removeFile(data);
                    alert("No more files please!");
                    return false;
                });

                this.on("complete", function (file) {
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        this.removeAllFiles(true);
                    }
                    HideProgressBar();

                    var data = JSON.parse(file.xhr.responseText);
                    if (data.ErrorCode == 0) {
                        if (data.filesnames[0] != '') {
                            $(instruction_div_selector).css("display-none");
                            $(imgpreview_selector).attr('src', data.RelativePath + data.filesnames[0]);
                            $("#div_ToolPhotoMask").removeClass("display-none");
                            $("#div_ToolPhoto").addClass("display-none");
                        }
                    }

                });
            }
        });
    }

    function LoadDropzoneSelectorButton(form_selector, imgpreview_selector, referenceid, attachmenttype, updateimgbtn_selector) {
        Dropzone.autoDiscover = false;
        $(form_selector).dropzone({
            url: "~/Attachments/SaveDropzoneJsUploadedFiles",
            addRemoveLinks: true,
            capture: true,
            createImageThumbnails: false,
            maxFiles: 1,
            acceptedFiles: 'image/*',
            previewsContainer: false,
            init: function () {
                this.on("processing", function (file) {
                    this.options.url = "../../Attachments/SaveDropzoneJsUploadedFiles?ReferenceID=" + referenceid + "&AttachmentType=" + attachmenttype + "&CompanyID=null";
                });

                this.on("addedfile", function (file) {
                    ShowProgressBar();
                });

                this.on("maxfilesexceeded", function (file) {
                    this.removeFile(data);
                    alert("No more files please!");
                    return false;
                });

                this.on("complete", function (file) {
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        this.removeAllFiles(true);
                    }
                    HideProgressBar();

                    var data = JSON.parse(file.xhr.responseText);
                    if (data.ErrorCode == 0) {
                        if (data.filesnames[0] != '') {
                            $(imgpreview_selector).attr('src', data.RelativePath + data.filesnames[0]);
                            $(updateimgbtn_selector).removeClass("display-none");
                        }
                    }
                });
            }
        });
    }

    function validateVehicleTools() {
        if ($("#btnYesUseVehicle").is(":checked") || $("#btnYesUseTools").is(":checked")) {
            $("#btn_RegistryCheckIn").text(LangResources.btn_Next);
            $("#btn_RegistryCheckIn").attr("id", "btn_NextCheckInStep2");
        } else {
            $("#btn_NextCheckInStep2").text(LangResources.lbl_Registry);
            $("#btn_NextCheckInStep2").attr("id", "btn_RegistryCheckIn");
        };
    }

    //Check In events Begin 
    function GetCheckInStep2(PersonalType, VendorTypeID) {
        ShowProgressBar();
        $.get("/SG/SecurityGuard/GetCheckInStep2", {
            AccessCode: $("#txt_ScanBox").val(),
            PersonTypeID: $("#ddl_PersonCheckIngTypeList option:selected").val(),
            PersonalType: PersonalType,
            VendorTypeID: VendorTypeID
        }).done(function (data) {
            $("#div_GenericModal_2").html(data);
            ApplyMaxLength();
            $("#mo_CheckInStep2").modal("show");
            if ($(".row_equipment").length > 0) {
                $("#div_EquipmentTable").removeClass("display-none");
            }

            validateVehicleTools();

            LoadDropzone('#form-dropzone-identification', function () {
                var ReferenceID = $('#ReferenceID').val();
                var AttachmentType = $('#AttachmentType').val();
                ShowProgressBar();
                $.get("/Attachments/Get",
                    { ReferenceID, AttachmentType }
                ).done(function (data) {
                    $('#AttachmentsTableIdentification').html(data);
                    $("#div_DropInstruction").empty();
                    var Name = $("#AttachmentsTableIdentification tr").eq(1).find("td:nth-child(1)").text();

                    $("#img_identificationMask").empty();
                    $("#img_identificationMask").prepend('<img src="../../Files/TempFiles/' + ReferenceID + '/' + Name + '"  height="150" id="img_IdentificationPreviewMask" />');

                    //Habilitar div para mostrar la imagen con la funcionalidad de expandir
                    $("#div_IdentificationPhoto").addClass("display-none");
                    $("#div_IdentificationPhotoMask").removeClass("display-none");
                }).fail(function (xhr, textStatus, error) {
                    ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
                }).always(function () {
                    HideProgressBar();
                });
            });

        }).fail(function (xhr, textStatus, error) {
            ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
        }).always(function () {
            HideProgressBar();
        });
    }

    function GetCheckInStepTools(AddMoreTools) {
        ShowProgressBar();
        $.get("/SG/SecurityGuard/GetCheckInStepTools", {
            AccesCode: $("#txt_ScanBox").val(),
            TempAttachmentID: $("#TempAttachmentID").val(),
            AddMoreTools: AddMoreTools
        }).done(function (data) {
            var ModalIDSelector = "#" + data.ModalID;
            $("#div_GenericModal_Tools").html(data.View);
            $(ModalIDSelector).modal("show");
            var widthPixels = $(".div_toolpath").first().width();
            $(".div_toolpath").css("height", widthPixels);

            $("#div_AddMoreTools").css("height", 135);
            $("#div_AddMoreTools").css("width", 150);

            /*
             * mo_CheckInTools es el modal que se abre si hay herramientas previas guardadas, en ese caso
             * guardamos un array de herramientas temporales para trabajar con él, de este modo podemos agregar
             * o quitar elementos sin alterar la base de datos, y al final, postear los elegidos
            */
            if (ModalIDSelector == "#mo_CheckInTools") {
                if (CurrentTools.length == 0) {
                    $(".div_toolcontainer").each(function () {
                        var Tool = {
                            ToolName: $(this).find(".div_toolname").first().text(),
                            ToolImgPath: $(this).find(".div_toolpath").attr("src")
                        }
                        CurrentTools.push(Tool);
                    });
                }

                $("#selectedTools, #availableTools").sortable({
                    connectWith: ".connectedSortable"
                }).disableSelection();
            }

            LoadDropzoneSelector("#form-dropzone-tool", "#img_ToolPreviewMask", "#div_DropTool");
            ApplyMaxLength();

        }).fail(function (xhr, textStatus, error) {
            ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
        }).always(function () {
            HideProgressBar();
        });
    }

    function ValidateToEnableStep3() {
        if ($("#txt_WhoVisits").val().length > 5 &&
            $("#ddl_BadgesList option:selected").val() != 0 &&
            $("#ddl_IdentificationsList option:selected").val() != 0) {

            $("#btn_NextCheckInStep2").removeClass("disabled");
            $("#btn_RegistryCheckIn").removeClass("disabled");
        } else {
            $("#btn_NextCheckInStep2").addClass("disabled");
            $("#btn_RegistryCheckIn").addClass("disabled");
        }
    }

    function ValidateToEnableStep4() {
        if ($("#txt_VehiclePlates").val() != "" &&
            $("#ddl_VehicleMarks option:selected").val() != 0 &&
            $("#txt_ResponsibleGuard").val() != "") {
            $("#btn_NextCheckInStepVehicle").removeClass("disabled");
            $("#btn_RegistryCheckIn").removeClass("disabled");

        } else {
            $("#btn_NextCheckInStepVehicle").addClass("disabled");
            $("#btn_RegistryCheckIn").addClass("disabled");

        }
    }

    function SetDialogSize() {
        $("#confirmbx_msg").css("font-size", "40px");
        $("#confirmbx_yesTag").css("font-size", "30px");
        $("#confirmbx_noTag").css("font-size", "30px");
    }

    function AddTool(isAddAndNew) {
        var IsValid = true;
        var ErrorMessage = "";

        //Verifica que la herramienta tenga todos sus datos
        if ($("#txt_ToolName").val() == null || $("#txt_ToolName").val() == "") {
            IsValid = false;
            ErrorMessage = LangResources.lbl_ToolNameMandatory;
        } else if ($("#img_ToolPreviewMask").attr("src") == null || $("#img_ToolPreviewMask").attr("src") == "") {
            IsValid = false;
            ErrorMessage = LangResources.lbl_ToolImgMandatory;
        }

        //Si todo esta bien, agrega la herramienta a los temporales, sino, muestra un mensaje
        if (IsValid) {

            ////Extraer el tempid del attachment
            //var AttachmnetTempID = $('#img_ToolPreviewMask').attr('src').replace('\\Files\\TempFiles\\', '');
            //var IndexToSubstring = AttachmnetTempID.indexOf('\\');
            //AttachmnetTempID = AttachmnetTempID.substring(0, IndexToSubstring);

            var SecurityTool = {
                SecurityGuardLogID: $("#TempAttachmentID").val(),
                ToolName: $("#txt_ToolName").val(),
                ToolImgPath: $("#img_ToolPreviewMask").attr("src") //.replace(AttachmnetTempID, $("#TempAttachmentID").val())
                //ReferenceID: $("#TempReferenceToolID").val()
            }

            $.post("/SG/SecurityGuard/SaveTools", {
                SecurityTool: SecurityTool
            }).done(function (data) {
                if (isAddAndNew) {
                    $("#div_ToolPhotoMask").addClass("display-none");
                    $("#div_ToolPhoto").removeClass("display-none");
                    $("#div_DropTool").removeClass("display-none");
                    $("#txt_ToolName").val("");

                    //Habilita un boton de terminar, en caso de que siempre no se quiera agregar otra herramienta
                    $("#btn_FinishAddTool").removeClass("display-none");
                } else {
                    $("#mo_CheckInToolsRegister").modal("toggle");
                    if ($('#btnYesUseVehicle').is(':checked')) {
                        $("#btn_NextCheckInStepVehicle").click();
                    } else {
                        $('#btn_NextCheckInStep2').click();
                    }
                }

            }).fail(function (xhr, textStatus, error) {
                ShowMessageBox("message-box-danger", "fa-warning", LangResources.lbl_Warning, error.message);
            }).always(function () {
                HideProgressBar();
            });

        } else {
            ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, error.message);
        }
    }

    function SaveVendorUser(isEdited) {
        var vendorUserEntity = {
            VendorUserID: $('#MoVendorUserID').val(),
            VendorID: $('#lblMoCompanyID').data('vendorid'),
            FullName: $('#txtMoVendorUserName').val(),
            AccessCode: $('#txtMoVendorUserAccessCode').val(),
            InsuranceNumber: $('#txtMoVendorUserInsuranceNumber').val(),
            ExpirationDate: $('#txtMoVendorUserExpirationDate').val(),
            Enabled: $('#btnMoVendorUserActive').is(':checked')
        };

        ShowProgressBar();
        $.post('/SG/SecurityGuardConfigurations/SaveVendorUser', {
            vendorUser: vendorUserEntity,
            isEdited: isEdited
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                ShowMessageBox("message-box-success", "fa-check", LangResources.lbl_Success, data.ErrorMessage);
                $('#mo_NewEditVendorUser').modal('toggle');
                ReloadVendorUsersList($('#ddl_VendorList option:selected').val());
            } else {
                ShowMessageBox("message-box-danger", "fa-warning", LangResources.lbl_Error, error.message);
            }
        }).fail(function (xhr, textStatus, error) {
            ShowMessageBox("message-box-warning", "fa-times", LangResources.lbl_Error, error.message);
        }).always(function () {
            HideProgressBar();
        });
    }

    function ReloadVendorUsersList(vendorID) {
        $('#divVendorUsersTable').empty();
        $.get('/SG/SecurityGuard/LoadEmployeesByCompany', {
            vendorID: vendorID
        }).done(function (data) {
            $('#divVendorUsersTable').html(data);
        }).fail(function (xhr, textStatus, error) {
            ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
        }).always(function () {
            HideProgressBar();
        });
    }

    function SearchEmployeeInTable() {
        var text = $('#txt_VendorUser').val().toUpperCase();
        $(".row_supplier_label td:first-child").each(function (e) {
            var Row = $(this).closest("tr").first();
            if ($(this).text().toUpperCase().indexOf(text) >= 0) {
                Row.css("display", "table-row");
            } else if (text == "" || $(this).text().indexOf(text) < 0) {
                Row.css("display", "none");
            }
        });
    }

    $("#div_CheckIn").click(function () {
        ShowProgressBar();
        $.get("/SG/SecurityGuard/GetCheckInStep1").done(function (data) {
            $("#div_GenericModal_1").html(data);
            ApplyMaxLength();
            $("#mo_CheckInStep1").modal("show");
        }).fail(function (xhr, textStatus, error) {
            ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_FinishAddTool", function (e) {
        e.stopImmediatePropagation();
        GetCheckInStepTools(false);
        $("#mo_CheckInTools").modal("show");
        $("#mo_CheckInToolsRegister").modal("toggle");

        //$.post("/SG/SecurityGuard/GetCheckinToolsModal", {
        //    AccesCode: $("#txt_ScanBox").val(),
        //    TempAttachmentID: $("#TempAttachmentID").val()
        //}).done(function (data) {
        //    $("#div_GenericModal_ToolsList").html(data);
        //    $("#mo_CheckInTools").modal("show");
        //    $("#mo_CheckInToolsRegister").modal("toggle");
        //}).fail(function (xhr, textStatus, error) {
        //    ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
        //}).always(function () {
        //    HideProgressBar();
        //});
    });

    $(document).on("click", "#div_AddMoreTools", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        //Actualizar el listado de herramientas
        CurrentTools = [];
        $(".div_toolcontainer").each(function () {
            var Tool = {
                SecurityGuardLogID: $("#TempAttachmentID").val(),
                ToolName: $(this).find(".div_toolname").text(),
                ToolImgPath: $(this).find(".div_toolpath").attr("src")
            }
            CurrentTools.push(Tool);
        });

        //Abrir el modal para agregar otra herramienta
        GetCheckInStepTools(true);
        $("#mo_CheckInTools").modal("toggle");

    });

    $(document).on("click", "#btn_AddAndNewTool", function (e) {
        e.stopImmediatePropagation();
        AddTool(true);
    });

    $(document).on("click", "#btn_AddNewTool", function (e) {
        e.stopImmediatePropagation();
        AddTool(false);
    });

    $(document).on("click", "#div_CheckOut", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/SG/SecurityGuard/GetCheckOut").done(function (data) {
            $("#div_GenericModal_1").html(data);
            $("#mo_CheckOut").modal("show");
        }).fail(function (xhr, textStatus, error) {
            ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, error.message);
        }).always(function () {
            HideProgressBar();
        });

    });

    $(document).on("show.bs.modal", "#mo_CheckInStep1", function (e) {
        e.stopImmediatePropagation();
        //$(".selectpicker").selectpicker();
        setTimeout(function () {
            $("#txt_ScanBox").focus();
            $("#txt_ScanBox").css("background-color", "#FFEA77");
        }, 1000);
    });

    $(document).on("show.bs.modal", "#mo_CheckInStep2", function (e) {
        e.stopImmediatePropagation();
        //$(".selectpicker").selectpicker();
        setTimeout(function () {
            $("#txt_WhoVisits").focus();
            $("#txt_WhoVisits").css("background-color", "#FFEA77");
        }, 1000);
    });

    $(document).on("show.bs.modal", "#mo_CheckOut", function (e) {
        e.stopImmediatePropagation();
        //$(".selectpicker").selectpicker();
        setTimeout(function () {
            $("#txt_ScanBox").focus();
            $("#txt_ScanBox").css("background-color", "#FFEA77");
        }, 1000);
    });

    $(document).on("change", ".vehicleOptions,.toolsOptions", function (e) {
        e.stopImmediatePropagation();
        validateVehicleTools();
    });

    $(document).on("blur", "input[type=text]", function (e) {
        e.stopImmediatePropagation();
        $("input[type=text]").css("background-color", "transparent");
    });

    $(document).on("click", "input[type=text]", function (e) {
        e.stopImmediatePropagation();
        $(this).css("background-color", "#FFEA77");
    });

    $(document).on("change", "#ddl_PersonCheckIngTypeList", function (e) {
        e.stopImmediatePropagation();
        var option = $(this).val();
        var valueid = $("#ddl_PersonCheckIngTypeList option:selected").data("valueid");
        ForRegistry(false);
        if (option == 0) {
            $("#btn_NextCheckInStep").addClass("disabled");
        } else if (option == $("#VisitorTypeID").val()) {
            $("#btn_NextCheckInStep").removeClass("disabled");
            $("#txt_PersonName").attr('readonly', false);
            $("#btn_NextCheckInStep").data("vendortypeid", 0);
            $("#div_ExpirationDate").css("display", "none");
        }

        $("#btn_NextCheckInStep").data("personaltype", valueid);
    });

    $(document).on("click", "#btn_NextCheckInStep", function (e) {
        e.stopImmediatePropagation();
        var IsValid = true;
        var personaltype = $("#btn_NextCheckInStep").data("personaltype");
        var vendortypeid = $("#btn_NextCheckInStep").data("vendortypeid");

        if ($("#ddl_PersonCheckIngTypeList option:selected").val() == $("#VisitorTypeID").val() && $("#txt_PersonName").val() == "") {
            ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, LangResources.msg_PleaseIdentifyYourself);
            IsValid = false;
        }


        if (IsValid) {
            if ($("#UseQuestionnaire").val() == "True") {
                ShowProgressBar();
                $.get("/SG/SecurityGuard/GetCovidQuestionnaire", {

                }).done(function (data) {
                    $("#div_GenericModal_Questionnaire").html(data);
                    $("#mo_CovidQuestionnaire").modal("show");
                    $("#btnQuestionnaire").data("personaltype", personaltype);
                    $("#btnQuestionnaire").data("vendortypeid", vendortypeid);

                }).fail(function (xhr, textStatus, error) {
                    ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
                }).always(function () {
                    HideProgressBar();
                });
            } else if ($("#txt_PersonName").val() != "") {
                GetCheckInStep2(personaltype, vendortypeid);
            } else {
                ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, LangResources.msg_PleaseIdentifyYourself);
            }
        }

    });

    $(document).on("click", "#btnQuestionnaire", function (e) {
        e.stopImmediatePropagation();
        var isValid = true;
        var personaltype = $("#btnQuestionnaire").data("personaltype");
        var vendortypeid = $("#btnQuestionnaire").data("vendortypeid");

        //Si hay una sola pregunta marcada como 'Si', automaticamente no es apto para pasar
        $(".questions").each(function () {
            if ($(this).find(".answer").first().is(":checked")) {
                isValid = false;
            }
        });
        if (isValid) {
            GetCheckInStep2(personaltype, vendortypeid);
        } else {
            ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, LangResources.msg_CovidAlertQuestion);
        }
    });

    $(document).on("click", "#btn_NextCheckInStep2", function (e) {
        e.stopImmediatePropagation();
        var personaltype = $("#btn_NextCheckInStep2").data("personaltype");

        if ($("#btnYesUseVehicle").is(":checked")) {
            ShowProgressBar();
            $.get("/SG/SecurityGuard/GetCheckInStepVehicle", {
                AccessCode: $("#txt_ScanBox").val(),
                PersonTypeID: $("#ddl_PersonCheckIngTypeList option:selected").val()
            }).done(function (data) {
                $("#div_GenericModal_Vehicle").html(data);
                ApplyMaxLength();
                $("#mo_CheckInVehicle").modal("show");
                $("#btn_NextCheckInStepVehicle").data("personaltype", personaltype);

                if ($("#btnYesUseTools").is(":checked")) {
                    $("#btn_RegistryCheckIn").text(LangResources.btn_Next);
                    $("#btn_RegistryCheckIn").attr("id", "btn_NextCheckInStepVehicle");
                }

            }).fail(function (xhr, textStatus, error) {
                ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
            }).always(function () {
                HideProgressBar();
            });
        } else if ($("#btnYesUseTools").is(":checked")) {
            GetCheckInStepTools(false);
        }
    });

    $(document).on("click", "#btn_NextCheckInStepVehicle", function (e) {
        e.stopImmediatePropagation();
        var personaltype = $("#btn_NextCheckInStepVehicle").data("personaltype");

        $.get('/SG/SecurityGuardConfigurations/ValidateGuardCode', {
            uniqueNumber: $("#txt_ResponsibleGuard").val()
        }).done(function (data) {
            if (data.IsValidGuardCode) {
                if ($("#btnYesUseTools").is(":checked")) {
                    GetCheckInStepTools(false);
                }
            } else {
                ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, LangResources.msg_DisableMissingGuard);
            }
        });
    });

    $(document).on("keyup", "#txt_WhoVisits", function (e) {
        e.stopImmediatePropagation();
        ValidateToEnableStep3();
    });

    $(document).on("change", "#ddl_BadgesList,#ddl_IdentificationsList", function (e) {
        e.stopImmediatePropagation();
        ValidateToEnableStep3();
    });

    $(document).on("keyup", "#txt_VehiclePlates,#txt_ResponsibleGuard", function (e) {
        e.stopImmediatePropagation();
        if ($("#txt_VehiclePlates").val().length > 5 && $("#txt_ResponsibleGuard").val().length > 3) {
            ValidateToEnableStep4();
        }
    });

    $(document).on("change", "#ddl_VehicleMarks", function (e) {
        e.stopImmediatePropagation();
        ValidateToEnableStep4();
    });

    $(document).on("keypress", "#txt_EquipmentCode", function (e) {
        e.stopImmediatePropagation();
        if (e.keyCode == 13) {
            ShowProgressBar();
            $.get("/SG/SecurityGuard/GetEquipmentInfo", {
                UPC: $("#txt_EquipmentCode").val()
            }).done(function (data) {
                if (data.length > 0) {
                    var isValid = true;
                    var RowsCount = $(".row_equipment").length + 1;

                    $(".row_equipment").each(function () {
                        var a = $(this).data("upc");
                        var b = $("#txt_EquipmentCode").val();
                        if ($(this).data("upc") == $("#txt_EquipmentCode").val()) {
                            isValid = false;
                        }
                    });

                    if (isValid) {
                        $("#EqupmentTable tbody").append(
                            '<tr class="row_equipment" data-equipmentid="' + data[0].EquipmentID + '" data-upc="' + $("#txt_EquipmentCode").val() + '">' +
                            '   <td class="fa-2x">' + RowsCount + '</td>' +
                            '   <td class="fa-2x">' + data[0].EquipmentName + '</td>' +
                            '   <td class="fa-2x">' + data[0].EquipmentTypeName + '</td>' +
                            '   <td class="fa-2x" title="' + LangResources.lbl_Delete + '">' +
                            '       <button class= "btn btn-danger delete-equipment-row"><span class="glyphicon glyphicon-trash fsize-25"></span></button>' +
                            '   </td > ' +
                            '</tr>'
                        );
                        $("#txt_EquipmentCode").val("");
                        $("#div_EquipmentTable").removeClass("display-none");

                    } else {
                        ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, LangResources.msg_EquipmentIsScanned);
                    }
                } else {
                    ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, LangResources.msg_EquipmentNotFound);
                }
            }).fail(function (xhr, textStatus, error) {
                ShowMessageBox("message-box-danger", "fa-times", LangResources.lbl_Error, error.message);
            }).always(function () {
                HideProgressBar();
            });
        }
    });

    $(document).on("click", ".delete-equipment-row", function (e) {
        e.stopImmediatePropagation();
        var Row = $(this).closest("tr");
        SetConfirmBoxAction(function () {
            Row.remove();
        }, LangResources.msg_EquipmentRowDeleteConfirm);
        $("#confirmbx_msg").css("font-size", "40px");
    });

    $(document).on("click", ".dimiss-modal", function (e) {
        e.stopImmediatePropagation();
        var modalID = $(this).data("modalid");
        $("#" + modalID).modal("toggle");
    });

    $(document).on("click", "#div_IdentificationPhotoMask", function (e) {
        e.stopImmediatePropagation();
        var srcPath = $("#img_IdentificationPreviewMask").attr("src");

        $.get("/SG/SecurityGuard/GetFullScreenImgView").done(function (data) {
            $("#div_FullScreenImgModal").html(data);
            $("#mo_FullScreenImage").modal("show");
            $("#img_FullScreenPrview").attr("src", srcPath);
            $("#btn_UpdateFullScreenImage").data("imgsection", "identification");

            var ReferenceID = $("#btn_UploadFullScreenImage").data("referenceid");
            var AttachmentType = $("#btn_UploadFullScreenImage").data("attachmenttype");
            $("#btn_UpdateFullScreenImage").data("referenceimgcontainer", "#img_IdentificationPreviewMask");
            LoadDropzoneSelectorButton("#btn_UploadFullScreenImage", "#img_FullScreenPrview", ReferenceID, AttachmentType, "#btn_UpdateFullScreenImage");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#div_ToolPhotoMask", function (e) {
        e.stopImmediatePropagation();
        var srcPath = $("#img_ToolPreviewMask").attr("src");

        $.get("/SG/SecurityGuard/GetFullScreenImgView").done(function (data) {
            $("#div_FullScreenImgModal").html(data);
            $("#mo_FullScreenImage").modal("show");
            $("#img_FullScreenPrview").attr("src", srcPath);
            $("#btn_UpdateFullScreenImage").data("imgsection", "tool");

            var ReferenceID = $("#btn_UploadFullScreenImage").data("referenceid");
            var AttachmentType = $("#btn_UploadFullScreenImage").data("attachmenttype");
            $("#btn_UpdateFullScreenImage").data("referenceimgcontainer", "#img_ToolPreviewMask");
            LoadDropzoneSelectorButton("#btn_UploadFullScreenImage", "#img_FullScreenPrview", ReferenceID, AttachmentType, "#btn_UpdateFullScreenImage");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_UpdateFullScreenImage", function (e) {
        e.stopImmediatePropagation();
        var srcPath = $("#img_FullScreenPrview").attr("src");
        var divContainer = $("#btn_UpdateFullScreenImage").data("referenceimgcontainer");
        var ReferenceID = $("#btn_UploadFullScreenImage").data("referenceid");
        var sectionReference = $("#btn_UpdateFullScreenImage").data("imgsection");

        var srcPath = $('#img_FullScreenPrview').attr('src');
        var index = srcPath.lastIndexOf('\\');
        var fileName = srcPath.substr(index + 1, srcPath.length);

        if (sectionReference == "tool") {
            $("#TempReferenceToolID").val(ReferenceID);
        } else if (sectionReference == "identification") {
            $("#TempReferenceID").val(ReferenceID);
        } else if (sectionReference == "toolContainer") {
            $(divContainer).addClass("imgChanged");
            $(divContainer).data("newreferenceid", ReferenceID);
            $(divContainer).data("filename", fileName);
        }
        $(divContainer).attr("src", srcPath);
        $("#mo_FullScreenImage").modal("toggle");
    });

    $(document).on("click", "#btnCancelTools", function (e) {
        e.stopImmediatePropagation();

    });

    $("#div_PrintBadgesLabels").on("click", function (e) {
        ShowProgressBar();
        $.get("/SG/SecurityGuard/GetPrintBadgesLabelsModal", {

        }).done(function (data) {
            $("#div_GenericModal_1").html(data);
            $("#mo_PrintBadgesLabel").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#div_PrintSuppliersLabels", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();

        $.get("/SG/SecurityGuard/GetPrintSuppliersLabelsModal", {

        }).done(function (data) {
            $("#div_GenericModal_1").html(data);
            $("#mo_PrintSuppliersLabel").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("keyup", "#txt_VendorUser", function (e) {
        e.stopImmediatePropagation();
        SearchEmployeeInTable();
    });

    $(document).on('click', '#btnAddNewCompany', function (e) {
        e.stopImmediatePropagation();
        $.get('/SG/SecurityGuard/GetNewCompanyModal', {

        }).done(function (data) {
            $('#div_GenericModal_2').html(data);
            $('#mo_NewCompany').modal('show');
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSaveNewCompany', function (e) {
        e.stopImmediatePropagation();
        var vendor = {
            OrganizationID: 1,
            VendorName: $('#txtCompanyName').val(),
            Enabled: true
        }

        $.post('/SG/SecurityGuardConfigurations/SaveNewCompany', {
            vendor: vendor
        }).done(function (data) {
            var VendorID = data.ID;
            if (data.ErrorCode == 0) {
                $.post('/SG/SecurityGuard/GetCompaniesList', {

                }).done(function (dataCompany) {
                    $("#ddl_VendorList").empty();
                    $.each(dataCompany, function (k, v) {
                        if (v.value == VendorID) {
                            $("#ddl_VendorList").append('<option value="' + v.value + '" selected>' + v.text + '</option>');
                            ReloadVendorUsersList(v.value);
                        } else {
                            $("#ddl_VendorList").append('<option value="' + v.value + '">' + v.text + '</option>');
                        }
                    });
                    $('#mo_NewCompany').modal('toggle');
                }).fail(function (xhr, textStatus, error) {
                    ShowMessageBox("message-box-danger", "fa-times", LangResources.lbl_Error, error.message);
                }).always(function () {
                    HideProgressBar();
                });
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '.boxremove', function (e) {
        e.stopImmediatePropagation();
        var element = $(this).closest(".div_toolcontainer").first();
        element.find(".div_toolpath").addClass("ToDisable");
        element.removeClass("ToAble");
        element.find(".boxremove").removeClass("boxremove").addClass("boxadd");
        $("#availableTools").append(element[0].outerHTML);

        element.remove();
    });

    $(document).on('click', '.boxadd', function (e) {
        e.stopImmediatePropagation();
        var element = $(this).closest(".div_toolcontainer").first();
        element.find(".div_toolpath").removeClass("ToDisable");
        element.addClass("ToAble");
        element.find(".boxadd").removeClass("boxadd").addClass("boxremove");
        $("#selectedTools").append(element[0].outerHTML);

        element.remove();
    });

    $(document).on("click", ".div_toolcontainer", function (e) {
        e.stopImmediatePropagation();
        var divID = $(this).find(".div_toolpath").attr("id");
        var srcPath = $("#" + divID).attr("src");

        $.get("/SG/SecurityGuard/GetFullScreenImgView").done(function (data) {
            $("#div_FullScreenImgModal").html(data);
            $("#mo_FullScreenImage").modal("show");
            $("#img_FullScreenPrview").attr("src", srcPath);
            $("#btn_UpdateFullScreenImage").data("imgsection", "toolContainer");

            var ReferenceID = $("#btn_UploadFullScreenImage").data("referenceid");
            var AttachmentType = $("#btn_UploadFullScreenImage").data("attachmenttype");
            $("#btn_UpdateFullScreenImage").data("referenceimgcontainer", "#" + divID);
            $('#divToolName').removeClass('display-none');
            //$("#btn_UpdateFullScreenImage").data("tooltempreference", "#" + divID);
            LoadDropzoneSelectorButton("#btn_UploadFullScreenImage", "#img_FullScreenPrview", ReferenceID, AttachmentType, "#btn_UpdateFullScreenImage");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('keydown', '#txtToolName', function (e) {
        e.stopImmediatePropagation();
        //$('#btn_UpdateFullScreenImage').removeClass('display-none');
    });

    $(document).on("click", "#btn_RefreshBadgesList", function (e) {
        ShowProgressBar();
        var vendortypeid = $(this).data("vendortypeid");
        $.get("/SG/SecurityGuard/GetBadgesList", {
            vendortypeid: vendortypeid
        }).done(function (data) {
            $("#ddl_BadgesList").empty();
            $.each(data, function (k, v) {
                $("#ddl_BadgesList").append('<option value="' + v.value + '">' + v.text + '</option>')
            });
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("keypress", "#txt_ScanBox", function (e) {
        e.stopImmediatePropagation();
        if (e.keyCode == 13) {
            ShowProgressBar();
            $.get("/SG/SecurityGuard/GetPersonInfo", {
                EmployeeCode: $("#txt_ScanBox").val()
            }).done(function (data) {

                $("#SecurityGuardLogID").val(data.SecurityGuardLogID);

                if (data.UserTypeID != 0) {
                    $("#ddl_PersonCheckIngTypeList").val(data.UserTypeID);
                }
                $("#txt_PersonName").val(data.UserName);

                if (data.UserName != "" && data.UserTypeName != "Visitor") {
                    $("#txt_PersonName").attr('readonly', true);
                    $("#txt_PersonName").css('background-color', 'transparent');
                    $("#txt_PersonName").css('color', 'black');
                } else {
                    $("#txt_PersonName").attr('readonly', false);
                }

                if (data.UserTypeName != "Employee") { //si es un proveedor o un visitante, cargar datos normalmente
                    $("#CurrentVendorUserID").val(data.VendorUserID)

                    if (data.UserTypeName == "Visitor") {
                        ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, LangResources.msg_UserNotInSystem);
                    }

                    if (data.ExpirationDate != "" && data.ExpirationDate != null) {
                        $("#txt_ExpirationDate").val(data.ExpirationDate);
                        $("#div_ExpirationDate").css("display", "block");
                    } else {
                        $("#div_ExpirationDate").css("display", "none");
                    }

                    $("#btn_NextCheckInStep").data("personaltype", data.UserTypeName);
                    $("#btn_NextCheckInStep").data("vendortypeid", data.VendorTypeID);

                    if (!(data.SessionStateStarted)) { //Si no tienes una sesion previa iniciada, entonces si carga el siguiente modal
                        if (data.VendorTypeID != 0) {
                            ForRegistry(false);
                        } else {
                            $("#btn_NextCheckInStep").addClass('disabled');
                        }

                        if (data.IsExpired) {
                            ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, LangResources.msg_ExpirationDate);
                        }

                    } else {
                        ShowMessageBox("message-box-info", "fa-warning", LangResources.lbl_Warning, LangResources.msg_CheckInSessionStarted);
                    }


                } else { //si es un empleado, ajustar para marcar entrada sin mas
                    ForRegistry(true);
                }
            }).fail(function (xhr, textStatus, error) {
                ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
            }).always(function () {
                HideProgressBar();
            });
        }
    });

    function ForRegistry(ForRegistry) {
        if (ForRegistry) {
            $("#btn_NextCheckInStep").text(LangResources.lbl_Registry);
            $("#btn_NextCheckInStep").attr("id", "btn_RegistryCheckIn");
            $("#btn_RegistryCheckIn").removeClass("disabled");
        } else {
            $("#btn_RegistryCheckIn").text(LangResources.btn_Next);
            $("#btn_RegistryCheckIn").attr("id", "btn_NextCheckInStep");
            $("#btn_NextCheckInStep").removeClass("disabled");
        }
    }



    $(document).on("keypress", "#txt_ScanBoxCheckOut", function (e) {
        e.stopImmediatePropagation();
        if (e.keyCode == 13) {
            $.get("/SG/SecurityGuard/GetCheckOutPersonInfo", {
                AccessCode: $("#txt_ScanBoxCheckOut").val()
            }).done(function (data) {
                $("#imgCheckOutImg").attr("src", data.FileImgPath);
                $("#divImgCheckOutImg").css("display", "block");
                console.log(data.SecurityGuardLogID);

                var date = new Date(parseInt(data.CurrentDate.substr(6)));

                $("#ddl_PersonCheckIngTypeList").val(data.UserTypeID);
                $("#txt_PersonName").val(data.UserName);
                $("#CheckOutSecurityGuardLogID").val(data.SecurityGuardLogID);
                $("#btnRegistryCheckOut").removeClass("disabled");

                if (data.UserTypeName == "Employee") {
                    $("#btnRegistryCheckOut").data("usertype", "employee")
                } else {
                    $("#btnRegistryCheckOut").data("usertype", "")
                }

            }).fail(function (xhr, textStatus, error) {
                ShowMessageBox("message-box-danger", "fa-times", LangResources.lbl_Error, error.message);
            }).always(function () {
                HideProgressBar();
            });
        }
    });


    $(document).on("click", "#btn_RegistryCheckIn", function (e) {
        e.stopImmediatePropagation();

        var EquipmentsEntityIDs = "";
        $(".row_equipment").each(function () {
            EquipmentsEntityIDs += "," + $(this).data("equipmentid");
        });

        if (EquipmentsEntityIDs != "") {
            EquipmentsEntityIDs = EquipmentsEntityIDs.substring(1, EquipmentsEntityIDs.length);
        }

        var CheckInEntity = {
            SecurityGuardLogID: $("#SecurityGuardLogID").val(),
            PersonID: $("#txt_ScanBox").val(),
            PersonName: $("#txt_PersonName").val(),
            PersonTypeID: $("#ddl_PersonCheckIngTypeList option:selected").val(),
            WhoVisit: $("#txt_WhoVisits").val(),
            VehiclePlates: $("#txt_VehiclePlates").val(),
            VehicleMarkID: $("#ddl_VehicleMarks").val(),
            SecurityOfficerID: $("#txt_ResponsibleGuard").val(),
            BadgeID: $("#ddl_BadgesList option:selected").val(),
            IdentificationTypeID: $("#ddl_IdentificationsList option:selected").val(),
            IdentificationImgPath: $("#img_IdentificationPreviewMask").attr("src"),
            EquipmentIDs: EquipmentsEntityIDs
        }

        ShowProgressBar();
        $.post("/SG/SecurityGuard/SaveCheck", {
            SecurityLogEntity: CheckInEntity,
            IsCheckIn: true
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                /*Se dejó preparado el cuestionario, pero realmente no se tiene que guardar nada, 
                 * porque con una respuesta positiva automáticamente el usuario ya no puede entrar
                 */
                //if ($("#UseQuestionnaire").val() == "True") {
                //    var CheckListsDetail = [];
                //    var QuestionMaster = {
                //        CheckListTemplateID: $("#CheckListTemplateID").val(),
                //        ReferenceID: data.ID
                //        //El referencetype lo saco en el sp
                //    }

                //    $(".questions").each(function () {
                //        var QuestionsDetail = {
                //            //El CheckListMasterID se va a obtener con el scopeidentity del QuestionMaster
                //            Seq: $(this).data("seq"),
                //            Question: $(this).find(".question").first().text(),
                //            Answer: $(this).find(".answer").first().is(":checked")
                //        }

                //        CheckListsDetail.push(QuestionsDetail);
                //    });
                //}

                $("#btn_GenericMessageBoxOK").click(function () {
                    ShowProgressBar();
                    window.location.reload();
                });

                //Copiar imagen de la identificación
                $.post("/Attachments/Copy", {
                    ReferenceID: $("#TempReferenceID").val(),
                    AttachmentType: "TEMPID",
                    NewReferenceID: data.ID,
                    NewAttachmentType: "IDENTIFICATIONTYPES"
                }).done(function (dataIdent) {

                });


                //Copiar imagenes de las herramientas NUEVAS
                var IsCorrectCopy = true;
                $(".imgNew").each(function () {
                    var ReferenceID = $(this).first().data("referenceid");
                    $.post("/Attachments/Copy", {
                        ReferenceID: ReferenceID,
                        AttachmentType: "TEMPID",
                        NewReferenceID: data.ID,
                        NewAttachmentType: "TOOLSIMG"
                    }).done(function (dataIdent) {

                    });
                });

                $(".imgNew").each(function () {
                    var ToolName = $(this).data("temptoolname");
                    var FileName = $(this).data("filename");
                    var VendorUserTool = {
                        VendorUserID: $("#CurrentVendorUserID").val(),
                        ToolName: ToolName,
                        ToolImgPath: "/Files/SG/ToolsImagesFiles/" + data.ID + "/" + FileName
                    }

                    $.post('/SG/SecurityGuard/AddToolToHistorial', {
                        VendorUserTool: VendorUserTool
                    });
                });

                /*
                 * Actualizar imagenes viejas (solo las que tienen la clase imgChanged son a las que se les actualizó la imagen, 
                 * de modo que no recorrerá nada si no hay cambios)
                 */
                $(".imgChanged:not(.imgNew)").each(function () {
                    var ReferenceID = $(this).first().data("newreferenceid");
                    $.post("/Attachments/Copy", {
                        ReferenceID: ReferenceID,
                        AttachmentType: "TEMPID",
                        NewReferenceID: data.ID,
                        NewAttachmentType: "TOOLSIMG"
                    }).done(function (dataOld) {

                    });
                });

                $(".imgChanged:not(.imgNew)").each(function () {
                    var securityGuardToolID = $(this).data("securityguardtoolid");
                    var FileName = $(this).data("filename");
                    $.post('/SG/SecurityGuard/UpdateImgPath', {
                        SecurityGuardToolID: securityGuardToolID,
                        ToolImgPath: "/Files/SG/ToolsImagesFiles/" + data.ID + "/" + FileName
                    });
                });

                var toolsToDisable = "";
                $(".ToDisable").each(function () {
                    toolsToDisable += "," + $(this).data("securityguardtoolid");
                });
                toolsToDisable = toolsToDisable.substring(1, toolsToDisable.length);

                var toolsToAble = [];
                $(".ToAble").not(".loadedTools").each(function () {
                    var toolName = $(this).find(".div_toolname").first().text();
                    var toolImgPath = $(this).find(".div_toolpath").first().attr("src");

                    var tool = {
                        SecurityGuardLogID: data.ID,
                        ToolName: toolName,
                        ToolImgPath: toolImgPath
                    }
                    toolsToAble.push(tool);
                });

                if (IsCorrectCopy) {
                    $.post("/SG/SecurityGuard/UpdateTempData", {
                        OldSecurityGuardLogID: $("#TempAttachmentID").val(),
                        NewSecurityGuardLogID: data.ID,
                        ToolsToDisable: toolsToDisable,
                        ToolsToAble: toolsToAble
                    }).done(function (dataUpdate) {
                        if (dataUpdate.ErrorCode == 0) {

                        }
                    });
                }

                ShowMessageBox("message-box-success", "fa-check", LangResources.lbl_Warning, data.ErrorMessage);
            } else {
                ShowMessageBox("message-box-danger", "fa-warning", LangResources.lbl_Error, data.ErrorMessage);
            }
        }).fail(function (xhr, textStatus, error) {
            ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, error.message);
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#btnRegistryCheckOut", function (e) {
        e.stopImmediatePropagation();

        var CheckInEntity = {
            SecurityGuardLogID: $("#CheckOutSecurityGuardLogID").val(),
            PersonID: $("#txt_ScanBoxCheckOut").val(),
            PersonName: $("#txt_PersonName").val(),
            PersonTypeID: $("#ddl_PersonCheckIngTypeList option:selected").val()
        }

        $.post("/SG/SecurityGuard/SaveCheck", {
            SecurityLogEntity: CheckInEntity,
            IsCheckIn: false
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                ShowMessageBox("message-box-success", "fa-check", LangResources.lbl_Success, data.ErrorMessage);
            } else {
                ShowMessageBox("message-box-danger", "fa-warning", LangResources.lbl_Error, data.ErrorMessage);
            }
            $("#btn_GenericMessageBoxOK").click(function () {
                window.location.reload();
            });

        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });


    $(document).on('change', '#ddl_VendorList', function (e) {
        e.stopImmediatePropagation();
        var vendorID = $('#ddl_VendorList option:selected').val();
        ReloadVendorUsersList(vendorID);
    });

    $(document).on('click', '#btnClearSearchBox', function (e) {
        e.stopImmediatePropagation();
        $('#txt_VendorUser').val('');
        SearchEmployeeInTable();
    });

    $(document).on('click', '#btnAddVendorUser', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var vendorID = $('#ddl_VendorList').val();
        if (vendorID != 0) {
            $.get('/SG/SecurityGuardConfigurations/GetNewUserVendorModal', {
                VendorID: vendorID,
                SectionName: 'SecurityGuard'
            }).done(function (data) {
                $('#div_GenericModal_2').html(data);
                $('#mo_NewEditVendorUser').modal("show");

                SetDatepickerPlugin('#txtMoVendorUserExpirationDate', LangResources.culture);
                SetupOnlyNumbers();
                $('#txtMoVendorUserInsuranceNumber').val('');
                $('.max-length').maxlength();
            }).fail(function (xhr, textStatus, error) {
                ShowMessageBox("message-box-danger", "fa-times", LangResources.lbl_Error, error.message);
            }).always(function () {
                HideProgressBar();
            });
        } else {
            ShowMessageBox("message-box-warning", "fa-warning", LangResources.lbl_Warning, "Selecciona una compañia");
            HideProgressBar();
        }
    });

    $(document).on('click', '.deleteVendorUser', function (e) {
        e.stopImmediatePropagation();
        var Row = $(this).closest('tr');
        var vendorUserID = Row.data('vendoruserid');
        var userName = Row.find("td:first-child").text();
        SetDialogSize();
        SetConfirmBoxAction(function () {
            $.post('/SG/SecurityGuardConfigurations/DeleteVendorUser', {
                vendorUserID
            }).done(function (data) {
                if (data.ErrorCode == 0) {
                    ShowMessageBox("message-box-success", "fa-check", LangResources.lbl_Success, data.ErrorMessage);
                    Row.remove();
                } else {
                    ShowMessageBox("message-box-danger", "fa-times", LangResources.lbl_Error, data.ErrorMessage);
                }
            }).fail(function (xhr, textStatus, error) {
                ShowMessageBox("message-box-danger", "fa-times", LangResources.lbl_Error, error.message);
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_ConfirmDeleteVendorUser.replace('##USER##', userName));
    });

    $(document).on('click', '.editVendorUser', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var vendorUserID = $(this).closest("tr").data("vendoruserid");
        $.get('/SG/SecurityGuardConfigurations/GetEditUserVendorModal', {
            vendorUserID: vendorUserID,
            vendorID: $('#ddl_VendorList option:selected').val(),
            SectionName: 'SecurityGuard'
        }).done(function (data) {
            $('#div_GenericModal_2').html(data);
            $('#mo_NewEditVendorUser').modal("show");

            SetDatepickerPlugin('#txtMoVendorUserExpirationDate', LangResources.culture);
            SetupOnlyNumbers();
            if ($('#txtMoVendorUserInsuranceNumber').val() == '0') {
                $('#txtMoVendorUserInsuranceNumber').val('');
            }
            $('#txtMoVendorUserAccessCode').attr("readonly", true).addClass('disabled-style-none');;
            $('#ddlMoCompaniesList').selectpicker();
            $('.max-length').maxlength();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSaveEditedUserVendor', function (e) {
        e.stopImmediatePropagation();
        SaveVendorUser(true);
    });

    //Check In events End 

    $(document).on('click', '#btnSaveNewUserVendor', function (e) {
        e.stopImmediatePropagation();
        SaveVendorUser(false);
    });

    $(document).on('click', '.printLabel', function (e) {
        e.stopImmediatePropagation();
        var referenceid = $(this).data("referenceid");
        var referencetypeid = $(this).data("referencetypeid");
        var labelname = $(this).closest("tr").find("td:first-child").text();
        SetDialogSize();
        SetConfirmBoxAction(function () {
            $.post('/SG/SecurityGuardConfigurations/PrintLabel', {
                ReferenceID: referenceid,
                ReferenceTypeID: referencetypeid
            }).done(function (data) {

            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_PrintLabelConfirm.replace("{0}", " " + labelname));
    });

    $(document).on("click", ".btnHomeButton", function (e) {
        e.stopImmediatePropagation();
        SetDialogSize();
        SetConfirmBoxAction(function () {
            $(".modal").modal("toggle");
        }, LangResources.msg_HomeLinkConfirmation);
    });

    $(document).on("click", "#btnClearVisit", function (e) {
        e.stopImmediatePropagation();
        $("#txt_WhoVisits").val("");
        $("#txt_WhoVisits").focus();
        $("#txt_WhoVisits").css("background-color", "#FFEA77");
    });
}

