function IndexInit(LangResources) {

    $("#ddl_Catalogs").selectpicker();

    function SetupxEditable() {
        $('.x-editable').editable({
            success: function (response, newValue) {
                var identifier = $(this).closest("td").data("identifier");
                var CatalogDetailID = $(this).parent().parent().data("entityid");
                var ColumnName = "";
                var Value = newValue;

                switch (identifier) {
                    case "GasketValueID":
                        ColumnName = "ValueID";
                        break;
                    case "Param1":
                        ColumnName = "Param1"
                        Value = $(".input-sm option:selected").text();
                        break;
                    case "Param2":
                        ColumnName = "Param2"
                        break;
                    case "Param3":
                        ColumnName = "Param3"
                        break;
                }

                $.post("/MFG/Catalogs/SaveChangesOfEditable", { CatalogDetailID, ColumnName, Value }).done(function (data) {
                    if (data.ErrorCode == 0) {
                        notification("", "Success", "success");
                    }
                })

            }
        });

        $('.x-editable').on("shown", function (e, editable) {
        });
    }

    function RegisterPluginMiniColor() {
        $('.mini-color').each(function () {
            $(this).minicolors({
                control: $(this).attr('data-control') || 'hue',
                defaultValue: $(this).attr('data-defaultValue') || '',
                format: $(this).attr('data-format') || 'hex',
                keywords: $(this).attr('data-keywords') || '',
                inline: $(this).attr('data-inline') === 'true',
                letterCase: $(this).attr('data-letterCase') || 'lowercase',
                opacity: $(this).attr('data-opacity'),
                position: $(this).attr('data-position') || 'bottom',
                swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
                theme: 'bootstrap'
            });
        });
    }

    SetupxEditable();

    function SaveMachineChanges(actionName) {
        var MachineID = $("#MachineID").val();
        var MachineName = $("#MachineName").val();
        var MachineDescription = $("#MachineDescription").val();
        var ProductionLineID = $("#ProductionLine option:selected").val();
        var Enabled = $("#MachineEnabled").is(":checked");
        var ReferenceID = $("#ReferenceID").val();
        var MachineCategoryID = $("#MachineCategory option:selected").val();

        $.post("/MFG/MachineSetup/" + actionName, { MachineID, MachineName, MachineDescription, ProductionLineID, Enabled, ReferenceID, MachineCategoryID }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#mo_NewMachine").modal("toggle");
                notification("", "Success", data.notifyType);
                LoadTableOfCatalogs();
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });
    }

    //var ReferenceID = 0;

    $(document).on("change", "#ddl_Catalogs", function () {
        $("#div_show_tables").css("display", "none");
        LoadTableOfCatalogs();
    });

    $(document).on("click", ".delete-record", function () {
        var EntityID = $(this).parent().parent().data("entityid");
        var Identifier = $("#Identifier").val();
        var ConfirmMessage = GetMessageForDeleteConfirm($("#Message").val(), LangResources);

        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.post("/MFG/Catalogs/DeleteCatalogRow", { EntityID, Identifier }).done(function (data) {
                if (data.ErrorCode != 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                } else {
                    LoadTableOfCatalogs();
                    notification("", "Success", data.notifyType);
                }
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }, ConfirmMessage);
    });

    $(document).on("click", ".edit-machine-record", function () {
        var EntityID = $(this).parent().parent().data("entityid");
        GetModalToEditMachine(EntityID, "Edit");
    });

    $(document).on("click", ".edit-machineparameters-record", function () {
        var EntityID = $(this).parent().parent().data("entityid");
        var TypeOfParameter = $(this).closest("tr").find("td:nth-child(2)").text().trim();
        GetModalToEditMachineParameters(EntityID, "Edit", LangResources.btn_Save, TypeOfParameter);
    });

    $(document).on("click", "#btn_NewMachine", function () {
        SaveMachineChanges("SaveNewMachine");
    });

    $(document).on("click", "#btn_EditMachine", function () {
        SaveMachineChanges("UpdateMachines");
    });

    $(document).on("click", "#btn_ModalSaveEditParameter", function () {
        var option = $("#ddl_Type").find("option:selected").text();
        $('.loading-process-div').show();
        var ParameterLength = null;
        var ParameterPrecision = null;
        var ParameterTag = null;
        var isValid = true;
        var ReferenceName = "";
        var ReferenceTypeID = "";
        var ReferenceListID = "";

        if ($("#UseReference").is(":checked")) {
            if ($("#ddl_ReferenceType option:selected").val() == 0) {
                notification("", "Elige Algo", "ntf_");
            } else if ($("#ddl_ReferenceType option:selected").text() == "TEXT") {
                if ($("#Reference").val() == "") {
                    notification("", "Escribe algo", "ntf_");
                } else {
                    ReferenceName = $("#Reference").val();
                    ReferenceTypeID = $("#ddl_ReferenceType option:selected").val();
                    ReferenceListID = null;
                }
            } else {
                if ($("#ddl_ReferenceList option:selected").text() == "") {
                    notification("", "Escoge algo", "ntf_");
                } else {
                    ReferenceName = $("#Reference").val();
                    ReferenceTypeID = $("#ddl_ReferenceType option:selected").val();
                    ReferenceListID = $("#ddl_ReferenceList option:selected").val();
                }
            }
        } else {
            ReferenceName = null;
            ReferenceTypeID = null;
            ReferenceListID = null;
        }

        switch (option) {
            case "INT":
                ParameterLength = $("#Length").val();
                ParameterPrecision = null;
                ParameterTag = null;
                break;
            case "DECIMAL":
                ParameterLength = $("#Length").val();
                ParameterPrecision = $("#Precition").val();
                ParameterTag = null;
                break;
            case "LIST":
                ParameterLength = null;
                ParameterPrecision = null;
                ParameterTag = $("#ddl_list").val();
                break;
            default:

        };

        if (parseFloat(ParameterPrecision) > parseFloat(ParameterLength)) {
            notification("", LangResources.PrecitionLengthInvalid, "nft_");
            isValid = false;
        }

        if (parseFloat(Precition) > parseFloat(Length)) {
            notification("", LangResources.PrecitionLengthValidation, "nft_");
            isValid = false;
        }

        if ($("#Name").val() == "") {
            notification("", LangResources.NameMandatory, "nft_");
            isValid = false;
        }

        if (isValid) {
            $.post("/MFG/Catalogs/UpdateMachineParameters", {
                MachineParameterID: $("#MachineParameterID").val(),
                ParameterName: $("#Name").val(),
                ParameterTypeID: $("#ddl_Type option:selected").val(),
                ParameterLength: ParameterLength,
                ParameterPrecision: ParameterPrecision,
                ParameterTag: ParameterTag,
                UseReference: $("#UseReference").is(":checked"),
                ReferenceName: ReferenceName,
                ReferenceTypeID: ReferenceTypeID,
                ReferenceListID: ReferenceListID,
                IsCavity: $("#IsCavity").is(":checked"),
                Enabled: $("#EnabledParameter").is(":checked")
            }).done(function (data) {
                if (data.ErrorCode == 0) {
                    notification("", LangResources.msg_SuccessEditParameter, data.notifyType);
                    $("#mo_NewParameter").modal("toggle");
                    LoadTableOfCatalogs();
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                //notification("", error.message, "error");
            }).always(function () {
                $('.loading-process-div').hide();
            });
        } else {
            $('.loading-process-div').hide();
        }
    });

    $(document).on("click", "#btn_ModalAddNewParameter", function () {
        var option = $("#ddl_Type").find("option:selected").text();
        $('.loading-process-div').show();
        var ParameterLength = null;
        var ParameterPrecision = null;
        var ParameterTag = null;
        var isValid = true;
        var ReferenceName = "";
        var ReferenceTypeID = "";
        var ReferenceListID = "";

        if ($("#UseReference").is(":checked")) {
            if ($("#ddl_ReferenceType option:selected").val() == 0) {
                notification("", "Elige Algo", "ntf_");
            } else if ($("#ddl_ReferenceType option:selected").text() == "TEXT") {
                if ($("#Reference").val() == "") {
                    notification("", "Escribe algo", "ntf_");
                } else {
                    ReferenceName = $("#Reference").val();
                    ReferenceTypeID = $("#ddl_ReferenceType option:selected").val();
                    ReferenceListID = null;
                }
            } else {
                if ($("#ddl_ReferenceList option:selected").text() == "") {
                    notification("", "Escoge algo", "ntf_");
                } else {
                    ReferenceName = $("#Reference").val();
                    ReferenceTypeID = $("#ddl_ReferenceType option:selected").val();
                    ReferenceListID = $("#ddl_ReferenceList option:selected").val();
                }
            }
        } else {
            ReferenceName = null;
            ReferenceTypeID = null;
            ReferenceListID = null;
        }

        switch (option) {
            case "INT":
                ParameterLength = $("#Length").val();
                ParameterPrecision = null;
                ParameterTag = null;
                break;
            case "DECIMAL":
                ParameterLength = $("#Length").val();
                ParameterPrecision = $("#Precition").val();
                ParameterTag = null;
                break;
            case "LIST":
                ParameterLength = null;
                ParameterPrecision = null;
                ParameterTag = $("#ddl_list").val();
                break;
            default:

        };

        if (parseFloat(ParameterPrecision) > parseFloat(ParameterLength)) {
            notification("", LangResources.PrecitionLengthInvalid, "nft_");
            isValid = false;
        }

        if (parseFloat(Precition) > parseFloat(Length)) {
            notification("", LangResources.PrecitionLengthValidation, "nft_");
            isValid = false;
        }

        if ($("#Name").val() == "") {
            notification("", LangResources.NameMandatory, "nft_");
            isValid = false;
        }

        if (isValid) {
            $.post("/MFG/Catalogs/InsertMachineParameters", {
                ParameterName: $("#Name").val(),
                ParameterTypeID: $("#ddl_Type option:selected").val(),
                ParameterLength: ParameterLength,
                ParameterPrecision: ParameterPrecision,
                ParameterTag: ParameterTag,
                UseReference: $("#UseReference").is(":checked"),
                ReferenceName: ReferenceName,
                ReferenceTypeID: ReferenceTypeID,
                ReferenceListID: ReferenceListID,
                IsCavity: $("#IsCavity").is(":checked"),
                Enabled: $("#EnabledParameter").is(":checked")
            }).done(function (data) {
                if (data.ErrorCode == 0) {
                    notification("", LangResources.msg_SuccessSaveParameter, data.notifyType);
                    $("#mo_NewParameter").modal("toggle");
                    LoadTableOfCatalogs();
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                $('.loading-process-div').hide();
            });
        } else {
            $('.loading-process-div').hide();
        }
    });

    $(document).on("change", "#UseReference", function () {
        if ($(this).is(":checked")) {
            $("#hide_referenceType").css("display", "block");
            $("#hide_referenceName").css("display", "block");
        } else {
            $("#ddl_ReferenceType").val(0);
            $("#ddl_ReferenceType").selectpicker("refresh");
            $("#hide_referenceType").css("display", "none");
            $("#hide_referenceName").css("display", "none");
            $("#hide_referenceList").css("display", "none");
        }
    });

    $(document).on("change", "#ddl_ReferenceType", function () {
        if ($("#ddl_ReferenceType option:selected").text() == "LIST") {
            $("#hide_referenceList").css("display", "block");
        } else {
            $("#hide_referenceList").css("display", "none");
        }
    });

    $(document).on("click", ".edit-gasket-record", function () {
        var EntityID = $(this).parent().parent().data("entityid");
        var optionSelected = $(this).closest("tr").find("td:nth-child(2)").text().trim();
        GetModalToEditGasket(EntityID, "Edit");
    });

    $(document).on("click", "#btn_SaveGasketEdit", function () {
        var CatalogDetailID = $("#CatalogDetailID").val();
        var ValueID = $("#ValueID").val();
        var OperationProcess = $("#OperationProcess option:selected").text();
        var Min = $("#Min").val();
        var Max = $("#Max").val();

        $.post("/MFG/Catalogs/UpdateGaskets", { CatalogDetailID, ValueID, OperationProcess, Min, Max }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification("", "Success", data.notifyType);
                LoadTableOfCatalogs();
                $("#mo_EditGasket").modal("toggle");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    });

    $(document).on("click", "#btn_SaveNewGasket", function () {
        var CatalogDetailID = $("#CatalogDetailID").val();
        var ValueID = $("#ValueID").val();
        var OperationProcess = $("#OperationProcess option:selected").text();
        var Min = $("#Min").val();
        var Max = $("#Max").val();

        $.post("/MFG/Catalogs/InsertGaskets", { CatalogDetailID, ValueID, OperationProcess, Min, Max }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification("", "Success", data.notifyType);
                LoadTableOfCatalogs();
                $("#mo_EditGasket").modal("toggle");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    });


    $(document).on("click", ".edit-defects-record", function () {
        var EntityID = $(this).parent().parent().data("entityid");
        GetModalToEditDefects(EntityID, "Edit");
    });

    $(document).on("click", ".edit-downtimes-record", function () {
        var EntityID = $(this).parent().parent().data("entityid");
        GetModalToEditDownTimes(EntityID, "Edit")
    });

    $(document).on("click", "#btn_SaveCatalogDownTime", function () {
        var CatalogDetailID = $("#CatalogDetailID").val();
        var ValueID = $("#ValueID").val();
        var BackgroundColor = $("#BackgroundColorDowntimes").val();
        var DownDefectsID = 1;

        $.post("/MFG/Catalogs/UpdateCatalog", { CatalogDetailID, ValueID, BackgroundColor, DownDefectsID }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification("", "Success", data.notifyType);
                LoadTableOfCatalogs();
                $("#mo_EditCatalog").modal("toggle");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    });

    $(document).on("click", "#btn_SaveNewCatalogDownTime", function () {
        var CatalogDetailID = $("#CatalogDetailID").val();
        var ValueID = $("#ValueID").val();
        var BackgroundColor = $("#BackgroundColorDowntimes").val();
        var DownDefectsID = 1;

        $.post("/MFG/Catalogs/InsertCatalog", { CatalogDetailID, ValueID, BackgroundColor, DownDefectsID }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification("", "Success", data.notifyType);
                LoadTableOfCatalogs();
                $("#mo_EditCatalog").modal("toggle");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    });

    $(document).on("click", "#btn_SaveNewCatalogDefects", function () {
        var CatalogDetailID = $("#CatalogDetailID").val();
        var ValueID = $("#ValueID").val();
        var backgroundcolor = $("#BackgroundColorDefects").val();
        var DownDefectsID = 2;

        $.post("/MFG/Catalogs/InsertCatalog", { CatalogDetailID, ValueID, backgroundcolor, DownDefectsID }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification("", "Success", data.notifyType);
                LoadTableOfCatalogs();
                $("#mo_EditCatalog").modal("toggle");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    });

    $(document).on("click", "#btn_SaveCatalogDefects", function () {
        var CatalogDetailID = $("#CatalogDetailID").val();
        var ValueID = $("#ValueID").val();
        var backgroundcolor = $("#BackgroundColorDefects").val();
        var DownDefectsID = 2;

        $.post("/MFG/Catalogs/UpdateCatalog", { CatalogDetailID, ValueID, backgroundcolor, DownDefectsID }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification("", "Success", data.notifyType);
                LoadTableOfCatalogs();
                $("#mo_EditCatalog").modal("toggle");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    });

    $(document).on("click", "#btn_NewCatalogRecord", function () {
        var CurrentTable = $("#tablesCatalogs").find("table").attr("id");
        if (CurrentTable != null) {
            switch (CurrentTable.replace("table_", "")) {
                case "Machines":
                    GetModalToEditMachine(0, "New");
                    break;
                case "MachineParameters":
                    GetModalToEditMachineParameters(0, "New");
                    break;
                case "Gasket":
                    GetModalToEditGasket(0, "New");
                    break;
                case "Defects":
                    GetModalToEditDefects(0, "New");
                    break;
                case "Downtimes":
                    GetModalToEditDownTimes(0, "New");
                    break;
            }
        } else {
            //alert("Selecciona un Catalogo");
        }
    });


    $(document).on("change", "#ddl_Type", function () {
        var option = $("#ddl_Type").find("option:selected").text();
        switch (option) {
            case "INT":
                $("#div_length").css("display", "block");
                $("#div_precition").css("display", "none");
                $("#div_catalog").css("display", "none");
                break;
            case "DECIMAL":
                $("#div_length").css("display", "block");
                $("#div_precition").css("display", "block");
                $("#div_catalog").css("display", "none");
                break;
            case "LIST":
                $("#div_length").css("display", "none");
                $("#div_precition").css("display", "none");
                $("#div_catalog").css("display", "block");
                break;
            default:

        };
    });


    //Filtros de las tablas
    $(document).on("keydown", ".ToHideTable", function () {
        $("#div_hidden").css("display", "none");
    });

    $(document).on("change", ".ToHideTable", function () {
        $("#div_hidden").css("display", "none");
    });

    $(document).on("click", "#btn_search_machine", function () {
        $('.loading-process-div').show();
        var MachineName = $("#machine_filter_name").val();
        var ProcessLineID = $("#machine_filter_line option:selected").val();

        $.get("/MFG/Catalogs/SearchMachine", { MachineName, ProcessLineID }).done(function (data) {
            $("#div_show_tables").css("display", "block");
            $("#tablesCatalogs").html(data);
            $("#div_hidden").css("display", "block");
            $(".selectpicker").selectpicker();
            SetupxEditable();
            $('.loading-process-div').hide();
        });
    });

    $(document).on("click", "#btn_search_machine_parameter", function () {
        $('.loading-process-div').show();
        var MachineParameterName = $("#machine_parameter_name").val();
        var MachineParameterType = $("#machine_parameter_type option:selected").val();

        $.get("/MFG/Catalogs/SearchMachineParameters", { MachineParameterName, MachineParameterType }).done(function (data) {            
            $("#div_show_tables").css("display", "block");
            $("#tablesCatalogs").html(data);
            $("#div_hidden").css("display", "block");
            $(".selectpicker").selectpicker();
            SetupxEditable();
            $('.loading-process-div').hide();
        });

    });

    $(document).on("click", "#btn_search_gaskets", function () {
        $('.loading-process-div').show();
        var ValueID = $("#value_id").val();
        var OperationProcess = $("#parameter_value option:selected").val();

        $.get("/MFG/Catalogs/SearchGaskets", { ValueID, OperationProcess }).done(function (data) {            
            $("#div_show_tables").css("display", "block");
            $("#tablesCatalogs").html(data);        
            $("#div_hidden").css("display", "block");
            $(".selectpicker").selectpicker();
            SetupxEditable();
            $('.loading-process-div').hide();
        });
    });

    $(document).on("click", "#btn_search_downtimes", function () {
        $('.loading-process-div').show();
        var ValueID = $("#value_id").val();
        var Param_Value = $("#reason_value option:selected").text();

        $.get("/MFG/Catalogs/SearchDowntimes", { ValueID }).done(function (data) {
            $("#div_show_tables").css("display", "block");
            $("#tablesCatalogs").html(data);
            $("#div_hidden").css("display", "block");
            $(".selectpicker").selectpicker();
            SetupxEditable();
            $('.loading-process-div').hide();
        });

    });

    $(document).on("click", "#btn_search_defects", function () {
        $('.loading-process-div').show();
        var ValueID = $("#value_id").val();
        var Param_Value = $("#reason_value option:selected").text();

        $.get("/MFG/Catalogs/SearchDefects", { ValueID }).done(function (data) {            
            $("#div_show_tables").css("display", "block");
            $("#tablesCatalogs").html(data);
            $("#div_hidden").css("display", "block");
            $(".selectpicker").selectpicker();
            SetupxEditable();
            $('.loading-process-div').hide();
        });;
    });

    //Fin de los filtros de las tablas

    function GetTableOfCatalog(ReferenceID, CatalogID) {
        $('.loading-process-div').show();
        $.get("/MFG/Catalogs/GetTableOfCatalog", { ReferenceID, CatalogID }).done(function (data) {
           
            $("#div_show_tables").css("display", "block");
            $("#tablesCatalogs").html(data);
            RegisterPluginMiniColor();

            $(".selectpicker").selectpicker();
            SetupxEditable();
            $('.loading-process-div').hide();
        });
    }

    function LoadTableOfCatalogs() {
        var id = $("#ddl_Catalogs option:selected").attr("id");
        var ReferenceID = $("#ddl_Catalogs option:selected").data("referenceid");

        debugger;
        if (id == 0) {
            GetTableOfCatalog(ReferenceID, null)
        } else {
            GetTableOfCatalog(ReferenceID, id)
        }
    }

    function GetModalToEditMachine(EntityID, Type) {
        $.get("/MFG/Catalogs/GetModalToEditMachine", { EntityID, Type }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_modal_edit").html(data.View);
                $("#mo_NewMachine").modal("show");

                LoadDropzone('.form-dropzone', function () {
                    ReferenceID = $('#ReferenceID').val();
                    var AttachmentType = $('#AttachmentType').val();

                    $.get("/Attachments/Get",
                        { ReferenceID, AttachmentType }
                    ).done(function (data) {

                        $('#AttachmentsTable').html('');
                        $('#AttachmentsTable').html(data);
                        //$(".selectpicker").selectpicker();

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

            } else {
                LoadTableOfCatalogs();
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    }

    function GetModalToEditMachineParameters(EntityID, Type, Translate, TypeOfParameter) {
        $.get("/MFG/Catalogs/GetModalToEditMachineParameters", { EntityID, Type }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_modal_edit").html(data.View);
                var option = TypeOfParameter;
                switch (option) {
                    case "INT":
                        $("#div_length").css("display", "block");
                        $("#div_precition").css("display", "none");
                        $("#div_catalog").css("display", "none");
                        break;
                    case "DECIMAL":
                        $("#div_length").css("display", "block");
                        $("#div_precition").css("display", "block");
                        $("#div_catalog").css("display", "none");
                        break;
                    case "LIST":
                        $("#div_length").css("display", "none");
                        $("#div_precition").css("display", "none");
                        $("#div_catalog").css("display", "block");
                        break;
                    default:

                };

                $("#mo_NewParameter").modal("show");
                $("#btn_ModalAddNewParameter").text(Translate);//LangResources.btn_Save)

                if ($("#UseReference").is(":checked")) {
                    $("#hide_referenceType").css("display", "block");
                    $("#hide_referenceName").css("display", "block");
                } else {
                    $("#ddl_ReferenceType").val(0);
                    $("#ddl_ReferenceType").selectpicker("refresh");
                    $("#hide_referenceType").css("display", "none");
                    $("#hide_referenceName").css("display", "none");
                    $("#hide_referenceList").css("display", "none");
                }

                $("#ddl_ReferenceType").selectpicker("refresh");
                $("#ddl_Type").val(data.SelectedOption);
                $("#ddl_Type").selectpicker("refresh");
            } else {
                LoadTableOfCatalogs();
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    }

    function GetModalToEditGasket(EntityID, Type) {
        $.get("/MFG/Catalogs/GetModalToEditGasket", { EntityID, Type }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_modal_edit").html(data.View);
                $("#mo_EditGasket").modal("show");
                $("#OperationProcess > option").each(function (index) {
                    if ($(this).text().trim() == optionSelected.trim()) {
                        var value = $(this).val();
                        $("#OperationProcess").val(parseInt(value));
                    };
                });

                $("#OperationProcess").selectpicker("refresh");

            } else {
                LoadTableOfCatalogs();
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    }

    function GetModalToEditDefects(EntityID, Type) {
        $.get("/MFG/Catalogs/GetModalToEditDefects", { EntityID, Type }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_modal_edit").html(data.View);
                $("#mo_EditCatalog").modal("show");

                RegisterPluginMiniColor();
            } else {
                LoadTableOfCatalogs();
                notification("", data.ErrorMessage, data.notifyType);
            }
        });
    }

    function GetModalToEditDownTimes(EntityID, Type) {
        $.get("/MFG/Catalogs/GetModalToEditDownTimes", { EntityID, Type }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_modal_edit").html(data.View);
                $("#mo_EditCatalog").modal("show");

            } else {
                LoadTableOfCatalogs();
                notification("", data.ErrorMessage, data.notifyType);
            }
            RegisterPluginMiniColor();
        });
    }

    function GetMessageForDeleteConfirm(CatalogName, LangResources) {
        var message = "";
        switch (CatalogName) {
            case "Machines":
                message = LangResources.msg_ConfirmToDeleteMachine;
                break;
            case "MachineParameters":
                message = LangResources.msg_ConfirmToDeleteMachineParameter;
                break;
            case "Gaskets":
                message = LangResources.msg_ConfirmToDeleteGasket;
                break;
            case "Defects":
                message = LangResources.msg_ConfirmToDeleteDefect;
                break;
            case "Downtimes":
                message = LangResources.msg_ConfirmToDeleteDowntime;
                break;
        }

        return message;
    }

}