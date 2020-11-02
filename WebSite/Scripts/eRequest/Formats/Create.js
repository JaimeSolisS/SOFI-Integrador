// =============================================================================================================================
//  Version: 
//  Author:  
//  Created Date: 
//  Description: Contiene funciones JS para la página de Create de Formats
//  Modifications: 

// =============================================================================================================================
function CreateInit(LangResources) {
    //Plugins
    $(".select").selectpicker({
        liveSearch: false
    });

    $('.max-length').maxlength();
    $('.maxlength').maxlength();
    $(".decimal-1-places").numeric({ decimalPlaces: 1, negative: false });
    $(".datetimepicker").datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        language: LangResources.Culture
    });
    $(".integer").numeric({ decimal: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
    $(".decimal").numeric();
    InitializeDataTableAccess();

    $(document).on('click', '#btn_AddField', function (e) {
        e.stopImmediatePropagation();
        $('#FormatGenericDetailIDVal').val(null);
        $.get("/eRequest/Formats/GetModalNewField").done(function (data) {
            HideProgressBar();

            if (data.ErrorCode == 0) {
                $("#div_Mo_NewField").html(data.View);
                $(".select").selectpicker("refresh");
                $("input.max-length").maxlength();
                $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
                $("#mo_NewField").modal("show");

                $("#ddl_ReferenceList").selectpicker("refresh");
                $("#ddl_ReferenceType").selectpicker("refresh");

            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Modal para agregar nuevo detalle

    $(document).on('click', '#btn_AddDetail', function (e) {
        e.stopImmediatePropagation();
        $.get("/eRequest/Formats/GetModalNewDetail"
        ).done(function (data) {
            HideProgressBar();

            if (data.ErrorCode == 0) {
                $("#div_Mo_NewDetailField").html(data.View);
                $(".select").selectpicker("refresh");
                $("input.positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
                $("input.max-length").maxlength();
                $("#mo_NewDetailField").modal("show");
                $("#ddl_ReferenceList").selectpicker("refresh");
                $("#ddl_ReferenceType").selectpicker("refresh");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    //Guardar Crear Metodo
    $(document).on('click', '#btn_SaveFormat', function (e) {
        e.stopImmediatePropagation();
        SetAlertConfirmCustomBoxActionSuccess(function () {
            var FormatID = $("#SelectFormatID").val();
            var FormatName = $("#txt_name").val();
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

            var itemlist = [];
            $("#tbl_formatsfields").find(" tr:not(:first)").each(function () {
                var tdlist = $(this).find("td");
                var Item = {
                    EntityID: FormatID,
                    SystemModule: $(this).data('SystemModule'),
                    SystemModuleTag: $(this).data('SystemModuleTag'),
                    FieldID: $(tdlist[0]).find("input").val(),
                    CustomTranslation: $(tdlist[1]).find("input").val(),
                    IsVisible: $(tdlist[2]).find("button").data("value"),
                    IsMandatory: $(tdlist[3]).find("button").data("value")
                };
                itemlist.push(Item);
            });
            ShowProgressBar();
            $.post("/eRequest/Formats/SaveFormat", {
                FormatID: FormatID,
                FormatName: FormatName,
                Use2FA: Use2FA,
                DirectApproval: DirectApproval,
                HasDetail: HasDetail,
                HasAttachment: HasAttachment,
                TransactionID: $('#TransactionID').val(),
                FormatPath: $('#btn_ShowPDF').data('fileurl'),
                PositionYInitial: $('#txt_initial_horizontal_position').val(),
                MaxDetail: $('#txt_maximum_number_of_details').val(),
                Interline: $('#txt_line_spacing').val(),
                Fieldslist: itemlist
            }).done(function (data) {
                HideProgressBar();
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    window.location = "/eRequest/Formats/Index";
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_SaveChangesQ, LangResources.msg_SaveChanges, 'Success');
    });
    //
    $(document).on('click', '#btn_ModalAddNewRule', function (e) {
        var RuleName = $('#txt_name_rule').val();
        var FormatID = $("#SelectFormatID").val();
        var FacilityID = $('#FacilityListRuleID option:selected').val();
        var itemlist = [];
        $("#tbl_Rule_Face").find(" tr:not(:first)").each(function () {
            var tdlist = $(this).find("td");
            if ($(tdlist[0]).find("input:checkbox:checked").val() == "on") {
                var Item = {
                    PhaseID: $(this).data('tempid'),
                    Seq: $(tdlist[2]).find("input").val()
                };
                itemlist.push(Item);
            }
        });
        $.post("/eRequest/Formats/CreateNewRule", {
            FormatsLoopsFlow: itemlist,
            FacilitySelectedID: FacilityID,
            TransactionID: $('#TransactionID').val(),
            FormatID: FormatID,
            Description: RuleName
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                ReloadFormatsLoopRulesTable(true);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Editar reglas flujo
    $(document).on('click', '#btn_ModalSaveRule', function (e) {
        var RuleName = $('#txt_name_rule').val();
        var FormatID = $("#SelectFormatID").val();
        var FacilityID = $('#FacilityListRuleID option:selected').val();
        var FormatLoopRuleTempID = $('#btn_ModalSaveRule').data('id');
        var itemlist = [];
        $("#tbl_Rule_Face").find(" tr:not(:first)").each(function () {
            var tdlist = $(this).find("td");
            if ($(tdlist[0]).find("input:checkbox:checked").val() == "on") {
                var Item = {
                    FormatLoopFlowwTempID: $(this).data('formatloopflowtempid'),
                    PhaseID: $(this).data('tempid'),
                    Seq: $(tdlist[2]).find("input").val()
                };
                itemlist.push(Item);
            }
        });
        $.post("/eRequest/Formats/EditRule", {
            FormatLoopRuleTempID: FormatLoopRuleTempID,
            FormatsLoopsFlow: itemlist,
            FacilitySelectedID: FacilityID,
            TransactionID: $('#TransactionID').val(),
            FormatID: FormatID,
            Description: RuleName
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                ReloadFormatsLoopRulesTable(true);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Guardar configuracion de field detail
    $(document).on('click', '#btn_ModalSaveConfigurationDetailListPDF', function (e) {
        var TransactionID = $('#TransactionID').val();
        var FormatID = $("#SelectFormatID").val();
        var FieldType = 'S';
        var FieldNames = '';
        $("#tbl_Rule_FielDetailConfiguration").find(" tr:not(:first)").each(function () {
            var tdlist = $(this).find("td");
            if ($(tdlist[0]).find("input:checkbox:checked").val() == "on") {
                FieldNames = $(this).data('fieldname') + '»' + FieldNames
            }
        });
        $.post("/eRequest/Formats/PDFFilesDetail_TEMP_Add", {
            TransactionID: TransactionID,
            FormatID: FormatID,
            FieldNames: FieldNames,
            FieldType: FieldType
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                $('#mo_NewPDFFieldDetailConfiguration').modal('toggle');
                ReloadTablesConfigurationPFD(LangResources.Culture);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btn_ModalSavePDFSignature', function (e) {
        var TransactionID = $('#TransactionID').val();
        var FormatID = $("#SelectFormatID").val();
        var FieldType = 'I';
        var FieldNames = '';
        $("#tbl_PDFFielSignatureConfig").find(" tr:not(:first)").each(function () {
            var tdlist = $(this).find("td");
            if ($(tdlist[0]).find("input:checkbox:checked").val() == "on") {
                FieldNames = $(this).data('fieldname') + '»' + FieldNames
            }
        });
        $.post("/eRequest/Formats/PDFFilesDetail_TEMP_Add", {
            TransactionID: TransactionID,
            FormatID: FormatID,
            FieldNames: FieldNames,
            FieldType: FieldType
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                $('#mo_pdfsignatureconfigure').modal('toggle');
                ReloadTablesConfigurationPFD(LangResources.Culture);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //DesplegarLoopsRules
    $(document).on('click', 'td.format-loop-rules-detail', function (e) {
        e.stopPropagation();

        var tr = $(this).closest('tr');

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {

            var FormatLoopRuleTempID = tr.data("id");

            ShowProgressBar();

            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Formats/GetFlowTable", {
                FormatLoopRuleTempID: FormatLoopRuleTempID
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="13" style="padding:0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });

            // abrir tablasde rules
            $.get("/eRequest/Formats/GetFormatsLoopsRulesDetailTable", {
                FormatLoopRuleTempID: FormatLoopRuleTempID,
                TransactionID: $('#TransactionID').val(),
                FormatID: $("#SelectFormatID").val()
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="13" style="padding:0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });
    //Buscar Aprobadores
    $(document).on('click', '#btn_AddUserApprover', function (e) {
        e.stopPropagation();
        var Search = $('#SelectUserRule').val();
        ShowProgressBar();
        $.get("/eRequest/Formats/GetSearchUserModal", {
            Search: Search
        }).done(function (data) {
            $("#div_search_user").html(data);
            $(".select").selectpicker();
            $("#mo_SearchUser").modal("show");
            $("#btnSearchUser").click();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        });
        HideProgressBar();
    });
    //Agregar Aprobadores
    $(document).on('click', '#btn_ModalAddNewAprover', function (e) {
        e.stopPropagation();
        var FormatLoopFlowTempID = $(this).data('formatloopflowtempid');
        var DepartmentOriginID = $('#DepartmentForRuleID').val();
        var Configuration = $('#SelectConfigurationRule').val();
        var TransactionID = $('#TransactionID').val();
        var JobPositionID = 0;
        var DepartmentID = 0;
        var Error = false;
        var ErrorMessage = ""
        var UserID = "";
        var isTolerance = 0;
        var Tolerance = 0;
        var ToleranceUoM = 0;
        switch (Configuration) {
            case "D":
                DepartmentID = $('#SelectOnlyDepartment').val();
                break;
            case "P":
                JobPositionID = $('#SelectOnlyPositionRule').val();
                if (JobPositionID == 0) {
                    Error = true;
                    ErrorMessage = ""//
                }
                break;
            case "PD":
                JobPositionID = $('#SelectPositionRule').val();
                DepartmentID = $('#SelectDepartmentRule').val();
                if (JobPositionID == 0) {
                    Error = true;
                    ErrorMessage = ""//
                }
                break;
            case "U":
                var t = document.getElementById("table_usersrule");
                var trs = t.getElementsByTagName("tr");
                var tds = null;
                var secID = [];
                for (var i = 1; i < trs.length; i++) {
                    tds = trs[i].getElementsByTagName("td");
                    secID.push(parseInt(tds[0].innerText));
                }
                UserID = secID.join(',');
                break;
            default:
                Error = true;
                ErrorMessage = ""//
                break;
        }
        if ($('#check_ApproverAfter').is(':checked')) {
            isTolerance = 1;
            Tolerance = $('#SelectNumberUoMApporver').val();
            ToleranceUoM = $('#SelectUoMApporver').val();
        }
        if (Error) {
            notification("", ErrorMessage, "nft_");
        } else {
            ShowProgressBar();
            $.post("/eRequest/Formats/SaveNewApprover", {
                FormatLoopFlowTempID: FormatLoopFlowTempID,
                TransactionID: TransactionID,
                DepartmentOriginID: DepartmentOriginID,
                JobPositionID: JobPositionID,
                DepartmentID: DepartmentID,
                ApproverIDs: UserID,
                AddTolerance: isTolerance,
                Tolerance: Tolerance,
                ToleranceUoM: ToleranceUoM
            }).done(function (data) {
                HideProgressBar();
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    $('#mo_NewApprover').modal('toggle');
                    ReloadFormatsLoopRulesTable(false);
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }
    });
    //Buscar aprobadores para agregar nuevos usuarios
    $(document).on("click", "#btnSearchUser", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/eRequest/Formats/SearchADUsers', {
            UserText: $('#txtSearchUser').val()
        }).done(function (data) {
            $("#div_ResponsiblesResultTable").html(data.View);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Agregar usuario a la tabla
    $(document).on('click', '.addUserAccount', function (e) {
        e.stopImmediatePropagation();
        var info = $(this);
        var User = {
            UserAccountID: info.data("useraccountid"),
            FirstName: info.data("firstname"),
            eMail: info.data("email")
        }

        ShowProgressBar();
        $.post('/eRequest/Formats/AddResponsibleAccount', {
            User: User
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                AddResponsibles(data.ID, info.data("firstname"));
                $('#SelectUserRule').val('');
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    //Desplegar Approvers
    $(document).on('click', 'td.detail-tbl-formatloopflow', function (e) {
        e.stopPropagation();

        var tr = $(this).closest('tr');

        if (tr.hasClass("shown")) {
            $('div.slider_approvers', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {

            var FormatLoopFlowTempID = tr.data("tempid");

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/eRequest/Formats/GetApproversTable", {
                FormatLoopFlowTempID: FormatLoopFlowTempID
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="13" style="padding:0; padding-left: 30px;">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider_approvers', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });
    //Combo box para la parte de mostrar cuando en configurar documento pdf
    $(document).on("change", ".showwhenfieldconfiguration", function (e) {
        e.stopImmediatePropagation();
        var option = $(this).find("option:selected").val();
        var optionselect = $(this);
        switch (option) {
            case "EQUAL":
                ShowProgressBar();
                $.get("/Formats/GetFormats_GetFieldData4ValueID", {
                    ValueID: $(this).closest('td').closest('tr').data('fieldname')
                }).done(function (data) {
                    $('#ComparatorTypeSelected').val(data.DataTypeValue);
                    switch (data.DataTypeValue) {
                        case "date":
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').css("display", "block");
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').find('.div_date').css("display", "block");
                            break;
                        case "text":
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').css("display", "block");
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').find('.div_text').css("display", "block");
                            break;
                        case "number":
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').css("display", "block");
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').find('.div_numeric').css("display", "block");
                            break;
                        case "decimal":
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').css("display", "block");
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').find('.div_decimal').css("display", "block");
                            break;
                        case "select":
                            ShowProgressBar(); 
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').css("display", "block");
                            var selectid = optionselect.closest('td').find('.div_showwhenfieldconfiguration').data("selectid");
                            optionselect.closest('td').find('.div_showwhenfieldconfiguration').find('.div_select').css("display", "block");
                            
                            var select = document.getElementById(selectid);
                            $(select).html('');
                            var ns = document.createElement("option");
                            ns.value = 0;
                            ns.text = "";
                            select.appendChild(ns);
                            $.get("/Formats/GetCatalogList", {
                                CatalogTag: data.CatalogTag
                            }).done(function (data) {
                                $(data.SelectList).each(function () {
                                    var option = document.createElement("option");
                                    option.value = this.CatalogDetailID;
                                    option.text = this.DisplayText;
                                    option.setAttribute('data-valuetable', this.CatalogDetailID);
                                    select.appendChild(option);
                                });
                                optionselect.closest('td').find('.div_showwhenfieldconfiguration').find('.div_select').find('.selectfieldconfiguration').selectpicker('refresh');
                                $(".select").selectpicker({
                                    liveSearch: false
                                });
                            }).fail(function (xhr, textStatus, error) {
                                notification("", error.message, "error");
                            }).always(function () {
                                HideProgressBar();
                            });
                        default:
                            break;
                    }
                    var FileDetailTempID = optionselect.closest("tr").data("filedetailtempid");
                    var ColumnName = optionselect.data('columnname');
                    var Value = optionselect.find("option:selected").data('valuetable');
                    ShowProgressBar();
                    $.post("/eRequest/Formats/SetPDFFilesTempQuickUpdate", {
                        FileDetailTempID: FileDetailTempID,
                        ColumnName: ColumnName,
                        Value: Value
                    }).done(function (data) {
                        HideProgressBar();
                        if (data.ErrorCode == 0) {
                            notification("", data.ErrorMessage, data.notifyType);
                        } else {
                            notification("", data.ErrorMessage, data.notifyType);
                        }
                    }).fail(function (xhr, textStatus, error) {
                        notification("", error.message, "error");
                    }).always(function () {
                        HideProgressBar();
                    });
                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                }).always(function () {
                    HideProgressBar();
                });
                break;
            case "ALLWAYS":
                $(this).closest('td').find('.div_showwhenfieldconfiguration').css("display", "none");
                break;
            default:
                $(this).closest('td').find('.div_showwhenfieldconfiguration').css("display", "none");
                break;
        }
    });
    //Combo box para mostrar los tipos de campos que puedes agregar
    $(document).on("change", "#ddl_Type", function (e) {
        e.stopImmediatePropagation();
        var option = $(this).find("option:selected").text();
        switch (option) {
            case "NUMERIC":
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
            case "CHECKBOX":
                $("#div_length").css("display", "none");
                $("#div_precition").css("display", "none");
                $("#div_catalog").css("display", "none");
                break;
            case "DATE":
                $("#div_length").css("display", "none");
                $("#div_precition").css("display", "none");
                $("#div_catalog").css("display", "none");
                break;
            case "TEXT":
                $("#div_length").css("display", "block");
                $("#div_precition").css("display", "none");
                $("#div_catalog").css("display", "none");
                break;
            default:
                $("#div_length").css("display", "none");
                $("#div_precition").css("display", "none");
                $("#div_catalog").css("display", "none");
                break;
        }
    });

    //Combo box para mostrar los tipos de campos que puedes agregar en detalle
    $(document).on("change", "#ddl_DetailType", function (e) {
        e.stopImmediatePropagation();
        var option = $(this).find("option:selected").text();
        switch (option) {
            case "NUMERIC":
                $("#div_detaillength").css("display", "block");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
            case "DECIMAL":
                $("#div_detaillength").css("display", "block");
                $("#div_detailprecition").css("display", "block");
                $("#div_detailcatalog").css("display", "none");
                break;
            case "LIST":
                $("#div_detaillength").css("display", "none");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "block");
                break;
            case "CHECKBOX":
                $("#div_detaillength").css("display", "none");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
            case "DATE":
                $("#div_detaillength").css("display", "none");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
            case "TEXT":
                $("#div_detaillength").css("display", "block");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
            default:
                $("#div_detaillength").css("display", "none");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
        }
    });
    //Combo box para mostrar los tipos de campos que puedes agregar en detalle
    $(document).on("change", "#SelectConfigurationRule", function (e) {
        e.stopImmediatePropagation();
        var option = $(this).val();
        $(".hide_configuration").css("display", "none");
        $("#case_" + option).css("display", "block");
    });
    //QuickUpdate para los valores de los formatos
    $(document).on("change", ".QuickUpdateSelect", function (e) {
        e.stopImmediatePropagation();
        var FileDetailTempID = $(this).closest("tr").data("filedetailtempid");
        var ColumnName = $(this).data('columnname');
        var Value = $(this).find("option:selected").data('valuetable');
        ShowProgressBar();
        $.post("/eRequest/Formats/SetPDFFilesTempQuickUpdate", {
            FileDetailTempID: FileDetailTempID,
            ColumnName: ColumnName,
            Value: Value
        }).done(function (data) {
            HideProgressBar();            
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Combo box para mostrar los tipos de campos que puedes agregar
    $(document).on("blur", ".QuickUpdateText", function (e) {
        e.stopImmediatePropagation();
        var FileDetailTempID = $(this).closest("tr").data("filedetailtempid");
        var ColumnName = $(this).data('columnname');
        var Value = $(this).val();
        ShowProgressBar();
        $.post("/eRequest/Formats/SetPDFFilesTempQuickUpdate", {
            FileDetailTempID: FileDetailTempID,
            ColumnName: ColumnName,
            Value: Value
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Combo box para mostrar los tipos de campos que puedes agregar
    $(document).on("change", ".QuickUpdateDate", function (e) {
        e.stopImmediatePropagation();
        var FileDetailTempID = $(this).closest("tr").data("filedetailtempid");
        var ColumnName = $(this).data('columnname');
        var Value = $(this).val();
        ShowProgressBar();
        $.post("/eRequest/Formats/SetPDFFilesTempQuickUpdate", {
            FileDetailTempID: FileDetailTempID,
            ColumnName: ColumnName,
            Value: Value
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Add New Field
    $(document).on("click", "#btn_ModalAddNewField", function (e) {
        e.stopImmediatePropagation();
        var option = $("#ddl_Type").find("option:selected").text();
        ShowProgressBar();
        var ParameterLength = null;
        var ParameterPrecision = null;
        var ParameterTag = null;
        var isValid = true;

        switch (option) {
            case "CHECKBOX":
                ParameterLength = 0;
                ParameterPrecision = 0;
                ParameterTag = null;
                break;
            case "DATE":
                ParameterLength = 0;
                ParameterPrecision = 0;
                ParameterTag = null;
                break;
            case "NUMERIC":
                ParameterLength = $("#Length").val();
                ParameterPrecision = 0;
                ParameterTag = null;
                break;
            case "TEXT":
                ParameterLength = $("#Length").val();
                ParameterPrecision = 0;
                ParameterTag = null;
                break;
            case "DECIMAL":
                ParameterLength = $("#Length").val();
                ParameterPrecision = $("#Precition").val();
                ParameterTag = null;
                break;
            case "LIST":
                ParameterLength = 0;
                ParameterPrecision = 0;
                ParameterTag = $("#ddl_list").val();
                break;
            default:
                notification("", LangResources.TypeMandatory, "nft_");
                isValid = false;
                break;

        };

        if (parseFloat(ParameterPrecision) > parseFloat(ParameterLength)) {
            notification("", LangResources.PrecitionLengthInvalid, "nft_");
            isValid = false;
        }

        if (option == "DECIMAL") {
            if (ParameterLength == 0) {
                notification("", LangResources.msg_ErrorLength, "nft_");
                isValid = false;
            }
            if (ParameterPrecision == 0) {
                notification("", LangResources.msg_ErrorPrecision, "nft_");
                isValid = false;
            }
        }

        if (option == "NUMERIC") {
            if (ParameterLength == 0) {
                notification("", LangResources.msg_ErrorLength, "nft_");
                isValid = false;
            }
        }

        if (option == "TEXT") {
            if (ParameterLength == 0) {
                notification("", LangResources.msg_ErrorLength, "nft_");
                isValid = false;
            }
        }

        if ($("#Name").val() == "") {
            notification("", LangResources.NameMandatory, "nft_");
            isValid = false;
        }

        if (isValid) {
            $.post("/eRequest/Formats/SaveNewField", {
                EntityID: $("#SelectFormatID").val(),
                IsVisible: 0,
                IsMandatory: 0,
                ValueID: $("#Name").val(),
                Param1: 1,
                Param2: 0,
                Param3: 0,
                CatalogTagID: ParameterTag,
                DataTypeID: $('#ddl_Type').val(),
                FieldLength: ParameterLength,
                FieldPrecission: ParameterPrecision
            }).done(function (data) {
                HideProgressBar();
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    ReloadFormatsFieldsTable();
                    $('#mo_NewField').modal('toggle');
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            HideProgressBar();
        }
    });
    //Add new detail
    $(document).on("change", "#ddl_DetailType", function (e) {
        e.stopImmediatePropagation();
        var option = $(this).find("option:selected").text();
        switch (option) {
            case "NUMERIC":
                $("#div_detaillength").css("display", "block");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
            case "DECIMAL":
                $("#div_detaillength").css("display", "block");
                $("#div_detailprecition").css("display", "block");
                $("#div_detailcatalog").css("display", "none");
                break;
            case "LIST":
                $("#div_detaillength").css("display", "none");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "block");
                break;
            case "CHECKBOX":
                $("#div_detaillength").css("display", "none");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
            case "DATE":
                $("#div_detaillength").css("display", "none");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
            case "TEXT":
                $("#div_detaillength").css("display", "block");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
            default:
                $("#div_detaillength").css("display", "none");
                $("#div_detailprecition").css("display", "none");
                $("#div_detailcatalog").css("display", "none");
                break;
        }
    });
    //Add New detail
    $(document).on("click", "#btn_ModalAddNewDetail", function () {
        var option = $("#ddl_DetailType").find("option:selected").text();
        ShowProgressBar();
        var ParameterLength = null;
        var ParameterPrecision = null;
        var ParameterTag = null;
        var ESMX = $('#DetailTranslationES').val();
        var ENUS = $('#DetailTranslationEN').val();
        var Detail = $('#txt_detail_description').val();
        var IsMandatory = 0
        var IsEnabled = 0
        var isValid = true;
        if ($('#btn_YdMandatory').hasClass('sel')) {
            IsMandatory = 1;
        }
        if ($('#btn_YdEnable').hasClass('sel')) {
            IsEnabled = 1;
        }
        switch (option) {
            case "CHECKBOX":
                ParameterLength = 0;
                ParameterPrecision = 0;
                ParameterTag = null;
                break;
            case "DATE":
                ParameterLength = 0;
                ParameterPrecision = 0;
                ParameterTag = null;
                break;
            case "NUMERIC":
                ParameterLength = $("#DetailLength").val();
                ParameterPrecision = 0;
                ParameterTag = null;
                break;
            case "TEXT":
                ParameterLength = $("#DetailLength").val();
                ParameterPrecision = 0;
                ParameterTag = null;
                break;
            case "DECIMAL":
                ParameterLength = $("#DetailLength").val();
                ParameterPrecision = $("#detailPrecition").val();
                ParameterTag = null;
                break;
            case "LIST":
                ParameterLength = 0;
                ParameterPrecision = 0;
                ParameterTag = $("#ddl_detaillist").val();
                break;
            default:
                notification("", LangResources.TypeMandatory, "nft_");
                isValid = false;
                break;

        };

        if (parseFloat(ParameterPrecision) > parseFloat(ParameterLength)) {
            notification("", LangResources.PrecitionLengthInvalid, "nft_");
            isValid = false;
        }

        if (option == "DECIMAL") {
            if (ParameterLength == 0) {
                notification("", LangResources.msg_ErrorLength, "nft_");
                isValid = false;
            }
            if (ParameterPrecision == 0) {
                notification("", LangResources.msg_ErrorPrecision, "nft_");
                isValid = false;
            }
        }

        if (option == "NUMERIC") {
            if (ParameterLength == 0) {
                notification("", LangResources.msg_ErrorLength, "nft_");
                isValid = false;
            }
        }
        if (option == "TEXT") {
            if (ParameterLength == 0) {
                notification("", LangResources.msg_ErrorLength, "nft_");
                isValid = false;
            }
        }

        if (ESMX == "") {
            notification("", LangResources.NameMandatoryES, "nft_");
            isValid = false;
        }
        if (ENUS == "") {
            notification("", LangResources.NameMandatoryEN, "nft_");
            isValid = false;
        }

        if (isValid) {
            $.post("/eRequest/Formats/SaveNewDetail", {
                FormatGenericDetailTempID: $('#FormatGenericDetailIDVal').val(),
                FormatID: $("#SelectFormatID").val(),
                NameES: ESMX,
                NameEN: ENUS,
                Description: Detail,
                DataTypeID: $('#ddl_DetailType').val(),
                FieldLength: ParameterLength,
                FieldPrecision: ParameterPrecision,
                CatalogTag: ParameterTag,
                IsMandatory: IsMandatory,
                Enabled: IsEnabled,
                TransactionID: $('#TransactionID').val()
            }).done(function (data) {
                HideProgressBar();

                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                    $('#mo_NewDetailField').modal('toggle');
                    ShowProgressBar();
                    $.get("/eRequest/Formats/TableFormatDetail", {
                        FormatID: $("#SelectFormatID").val(),
                        TransactionID: $('#TransactionID').val()
                    }).done(function (data) {
                        $("#div_tbl_formatDetail").html(data);
                        // InitializeDataTable();
                    }).fail(function (xhr, textStatus, error) {
                        notification("", error.message, "error");
                    }).always(function () {
                        HideProgressBar();
                    });
                    $('#FormatGenericDetailIDVal').val(null);
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            HideProgressBar();
        }

    });
    //Editar datos de un campo de detalle
    $(document).on("click", ".edit-formatdetail", function (e) {
        e.stopImmediatePropagation();
        var FormatGenericDetailTempID = $(this).closest("tr").data("tempid");
        $('#FormatGenericDetailIDVal').val(FormatGenericDetailTempID);
        $.get("/eRequest/Formats/GetModalNewDetail", {
            FormatGenericDetailTempID: FormatGenericDetailTempID
        }).done(function (data) {

            HideProgressBar();

            if (data.ErrorCode == 0) {
                $("#div_Mo_NewDetailField").html(data.View);
                $(".select").selectpicker("refresh");
                $("input.max-length").maxlength();
                $("#mo_NewDetailField").modal("show");

                $("#ddl_ReferenceList").selectpicker("refresh");
                $("#ddl_ReferenceType").selectpicker("refresh");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Editar Aprobador ---------------------------------------------------------------------------------------------------------------------------
    $(document).on("click", ".edit-FormatsLoopsApprovers", function (e) {
        e.stopImmediatePropagation();
        var FormatLoopApproverTempID = $(this).closest("tr").data("id");
        var FormatLoopFlowTempID = $(this).closest("tr").data("tempid");
        $.get("/eRequest/Formats/GetAddNewApprover", {
            FormatLoopApproverTempID: FormatLoopApproverTempID,
            FormatLoopFlowTempID: FormatLoopFlowTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val(),
            SelectedFacility: $('#FacilityListRuleID').val()
        }).done(function (data) {
            HideProgressBar();

            if (data.ErrorCode == 0) {
                $("#div_Mo_NewDetailField").html(data.View);
                $(".select").selectpicker("refresh");
                $("input.max-length").maxlength();
                $("#mo_NewDetailField").modal("show");

                $("#ddl_ReferenceList").selectpicker("refresh");
                $("#ddl_ReferenceType").selectpicker("refresh");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });

    //Borra datos de un campo de detalle
    $(document).on("click", ".delete-formatdetail", function (e) {
        e.stopImmediatePropagation();
        var ColumnName = $(this).closest("tr").data("columname");
        $.get("/eRequest/Formats/DeleteDetail", {
            FormatID: $("#SelectFormatID").val(),
            ColumnName: ColumnName,
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {

            HideProgressBar();

            if (data.ErrorCode == 0) {
                ShowProgressBar();
                $.get("/eRequest/Formats/TableFormatDetail", {
                    FormatID: $("#SelectFormatID").val(),
                    TransactionID: $('#TransactionID').val()
                }).done(function (data) {
                    $("#div_tbl_formatDetail").html(data);
                    // InitializeDataTable();
                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                }).always(function () {
                    HideProgressBar();
                });

            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Botones yes no para el de 2FA
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
    //Botones Yes No para el de direct approval
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
            $('.yes-no-detail').show();
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
            $('.yes-no-detail').hide();
        }
    });

    //Botones yes no para modal de detalles Mandatory
    $(document).on('click', '#btn_YdMandatory', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('sel')) {
            $('#btn_NdMandatory').addClass('btn-default');
            $('#btn_NdMandatory').removeClass('btn-danger');
            $('#btn_NdMandatory').removeClass('sel');
        } else {
            btn.addClass('sel');
            btn.addClass('btn-success');
            btn.removeClass('btn-default');
            $('#btn_NdMandatory').addClass('btn-default');
            $('#btn_NdMandatory').removeClass('btn-danger');
            $('#btn_NdMandatory').removeClass('sel');

        }
    });

    $(document).on('click', '#btn_NdMandatory', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('sel')) {
            $('#btn_YdMandatory').addClass('btn-default');
            $('#btn_YdMandatory').removeClass('btn-success');
            $('#btn_YdMandatory').removeClass('sel');
        } else {
            btn.addClass('sel');
            btn.addClass('btn-danger');
            btn.removeClass('btn-default');
            $('#btn_YdMandatory').addClass('btn-default');
            $('#btn_YdMandatory').removeClass('btn-success');
            $('#btn_YdMandatory').removeClass('sel');
        }
    });

    //Botones yes no para modal de detalles Enable
    $(document).on('click', '#btn_YdEnable', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('sel')) {
            $('#btn_NdEnable').addClass('btn-default');
            $('#btn_NdEnable').removeClass('btn-danger');
            $('#btn_NdEnable').removeClass('sel');
        } else {
            btn.addClass('sel');
            btn.addClass('btn-success');
            btn.removeClass('btn-default');
            $('#btn_NdEnable').addClass('btn-default');
            $('#btn_NdEnable').removeClass('btn-danger');
            $('#btn_NdEnable').removeClass('sel');

        }
    });

    $(document).on('click', '#btn_NdEnable', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('sel')) {
            $('#btn_YdEnable').addClass('btn-default');
            $('#btn_YdEnable').removeClass('btn-success');
            $('#btn_YdEnable').removeClass('sel');
        } else {
            btn.addClass('sel');
            btn.addClass('btn-danger');
            btn.removeClass('btn-default');
            $('#btn_YdEnable').addClass('btn-default');
            $('#btn_YdEnable').removeClass('btn-success');
            $('#btn_YdEnable').removeClass('sel');
        }
    });

    //Botones yes no para tabla de campos del formato
    $(document).on('click', '.btnyesno', function (e) {
        e.stopImmediatePropagation();
        var btn = $(this);
        if (btn.hasClass('btn-success')) {
            btn.removeClass('btn-success');
            btn.addClass('btn-danger');
            btn.text(LangResources.lbl_no);
            btn.data('value', 'false');
        } else {
            btn.addClass('btn-success');
            btn.removeClass('btn-danger');
            btn.text(LangResources.lbl_yes);
            btn.data('value', 'true');
        }

    });

    //edit translation de la fase
    $(document).on('click', '.edit-cultures', function (e) {
        e.stopImmediatePropagation();
        var EntityID = $(this).data('entityid');
        var CatalogID = $(this).data('entityid');
        var CatalogDetailTemp = $(this).data('catalogdetailtempid');
        var description = $(this).data("description");
        $('#l_ValueID').html(description);

        $('#hf_EDC_CatalogDetailID').val(EntityID);
        $('#m_EditCultures').modal({
            backdrop: 'static',
            keyboard: false
        });
        FillDetailsCultures(CatalogDetailTemp, CatalogID)
    });
    //eliminar phase
    $(document).on('click', '.delete-catalogdetail', function (e) {
        e.stopImmediatePropagation();
        var EntityID = $(this).data('entityid');
        var CatalogID = $(this).data('catalogid');
        $.ajax({
            type: "POST",
            url: "/Administration/Catalogs/DeleteCatalogDetail",
            data: { 'CatalogDetailID': EntityID },
            dataType: 'json',
            success: function (response) {
                if (response.ErrorCode === 0) {
                    notification("", response.ErrorMessage, "success");
                    FillDetails($("#CatalogID").val());
                }
                else {
                    notification("", response.ErrorMessage, "error");
                }
            }
        });

        FillDetails(CatalogID);

    });
    //Agregar phase de general
    $(document).on('click', '#btn_addphase', function (e) {
        e.stopImmediatePropagation();
        AddPhase();
    });
    //Delete phase de general
    $(document).on('click', '.delete-phase-temp', function (e) {
        e.stopImmediatePropagation();
        var CatalogDetailTempID = $(this).data("catalogdetailtempid");
        $.get("/eRequest/Formats/FormatPhaseTempDelete", {
            CatalogDetailTempID: CatalogDetailTempID
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                ReloadTblPhases();
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //
    $(document).on('click', '#btn_ModalAddaccess', function (e) {
        e.stopImmediatePropagation();
        var UserListID = '';
        var Facility = $('#FacilityListID').val();
        if (Facility == null) {
            Facility = ''
        } else {
            Facility = $('#FacilityListID').val().join(',');
        }
        $(".responsibles_row  td:first-child").each(function () {
            UserListID = UserListID + '' + $(this).text() + '|';
        });
        $.post("/eRequest/Formats/FormatAccessTempInsert", {
            FormatID: $('#SelectFormatID').val(),
            TransactionID: $('#TransactionID').val(),
            FacilityID: Facility,
            UserListID: UserListID
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                ReloadAccessFormatTable(true);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });

    $(document).on('click', '#btn_addPDFFieldConfiguration', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.post("/eRequest/Formats/PDFFilesDetail_TEMP_HeaderInsert", {
            TransactionID: $('#TransactionID').val(),
            FormatID: $('#SelectFormatID').val(),
            FieldName: $('#ListFieldActiveToPDF').val(),
            FieldType: 'H'
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                ReloadTablesConfigurationPFD(LangResources.Culture);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //btn cambiar traduccion de catalogos
    $(document).on('click', '#btn_save_cat_translation', function (e) {
        e.stopImmediatePropagation();
        var ValueEn = $('#CatalogTranslationEN').val();
        var ValueEs = $('#CatalogTranslationES').val();
        var Tag = $('#CatalogTranslationEN').data('tag');

        $.post("/eRequest/Formats/SaveTranslation", {
            Tag: Tag,
            DescriptionEN: ValueEn,
            DescriptionES: ValueEs
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                $('#m_EditCultures').modal('toggle');
                ShowProgressBar();
                $.get("/eRequest/Formats/GetCatalogsDetail", {
                    FormatID: $("#SelectFormatID").val(),
                    TransactionID: $('#TransactionID').val()
                }).done(function (data) {
                    $("#div_tbl_CatalogsDetail").html('');
                    $("#div_tbl_CatalogsDetail").html(data);
                    // InitializeDataTable();
                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                }).always(function () {
                    HideProgressBar();
                });
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Add a New User
    $(document).on('change', '#txt_AssignResponsibleFormat', function (e) {
        e.stopImmediatePropagation();
        var option = $('#txt_AssignResponsibleFormat option:selected').val();
        if (option == "new") {
            ShowProgressBar();
            $.get("/Formats/GetAddUserModal").done(function (data) {
                $("#div_add_new_user").html(data);
                $(".select").selectpicker();
                $("#mo_AddNewUser").modal("show");
                $('#txt_AssignResponsibleFormat').val(0);
                $('#txt_AssignResponsibleFormat').selectpicker("refresh");

            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }
    });
    //Cambiar faciliti en la parte de reglas
    $(document).on('change', '#FacilityListRuleID', function (e) {
        e.stopImmediatePropagation();
        var FacilityID = $('#FacilityListRuleID option:selected').val();
        ShowProgressBar();
        $.get("/eRequest/Formats/GetTableFormatsLoopRules", {
            FacilityID: FacilityID,
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_tbl_FormatsLoopRules").html('');
            $("#div_tbl_FormatsLoopRules").html(data);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //Cambiar listado de usuarios
    $(document).on('change', '#FacilityListID', function (e) {
        e.stopImmediatePropagation();
        var FacilitiesList = $('#FacilityListID').val();
        if (FacilitiesList != null) {
            FacilitiesList = $('#FacilityListID').val().join(',');
        } else {
            FacilitiesList = ''
        }
        ShowProgressBar();
        $.get("/Formats/GetUserList", {
            FacilitiesList: FacilitiesList
        }).done(function (data) {
            $("#user_list").html(data);
            $(".select").selectpicker();

        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Boton para abrir modal de nueva regla
    $(document).on('click', '#btn_addrule', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/Formats/GetAddNewRuleModal", {
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_rule").html(data);
            $(".icheckbox").iCheck();
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $("#mo_NewRule").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Boton para abrir modal de nueva regla
    $(document).on('click', '#btn_AddFieldDetailConfiguration', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/Formats/GetAddNewPDFDetailConfigureModal", {
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_PDFField_DetailConfiguration").html(data);
            $(".icheckbox").iCheck();
            $("#mo_pdfsignatureconfigure").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    $(document).on('click', '#btn_AddPDFSignature', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/Formats/GetAddNewPDFSignatureConfigureModal", {
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_PDFField_SignatureConfiguration").html(data);
            $(".icheckbox").iCheck();
            $("#mo_pdfsignatureconfigure").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Boton para abrir modal de nueva regla detalle show step 1
    $(document).on('click', '.btn-add-rule', function (e) {
        e.stopImmediatePropagation();
        var FormatLoopRuleTempID = $(this).closest("tr").data("id");
        ShowProgressBar();
        $.get("/Formats/GetCreateRuleStepsModal", {
            FormatLoopRuleTempID: FormatLoopRuleTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_rule").html(data);
            var select = document.getElementById("ListFieldActive");
            $('#RuleDetailTypeSelected').val('AND');
            var ns = document.createElement("option");
            ns.value = 0;
            ns.text = "";
            select.appendChild(ns);
            $("#tbl_formatsfields").find(" tr:not(:first)").each(function () {
                var tdlist = $(this).find("td");
                var option = document.createElement("option");
                if ($(tdlist[2]).find("button").data("value")) {
                    option.value = $(tdlist[2]).find("button").data("id");
                    option.text = $(tdlist[2]).find("button").data("displayval");
                    select.appendChild(option);
                }
            });
            $(".select").selectpicker({
                liveSearch: false
            });
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $('#RuleTypeSelected').val('CONDITION');
            $("#mo_Create_Rule").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });

    //Add new condition
    $(document).on('click', '.tbl_btn_addNewConditionRule', function (e) {
        e.stopImmediatePropagation();
        var FormatLoopRuleTempID = $(this).closest("tr").data("formatloopruletempid");
        var Seq = $(this).closest("tr").data("seq");
        ShowProgressBar();
        $.get("/Formats/GetCreateRuleStepsModal", {
            FormatLoopRuleTempID: FormatLoopRuleTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_rule").html(data);
            var select = document.getElementById("ListFieldActive");
            $('#RuleDetailTypeSelected').val('AND');
            var ns = document.createElement("option");
            ns.value = 0;
            ns.text = "";
            select.appendChild(ns);
            $("#tbl_formatsfields").find(" tr:not(:first)").each(function () {
                var tdlist = $(this).find("td");
                var option = document.createElement("option");
                if ($(tdlist[2]).find("button").data("value")) {
                    option.value = $(tdlist[2]).find("button").data("id");
                    option.text = $(tdlist[2]).find("button").data("displayval");
                    select.appendChild(option);
                }
            });
            $(".select").selectpicker({
                liveSearch: false
            });
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $('#RuleTypeSelected').val('CONDITION');
            $("#mo_Create_Rule").modal("show");
            $('#SeqCreate').val(Seq);
            $('#RuleDetailTypeSelected').val('AND');     
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Add new exception
    $(document).on('click', '.tbl_btn_addNewExceptionRule', function (e) {
        e.stopImmediatePropagation();
        var FormatLoopRuleTempID = $(this).closest("tr").data("formatloopruletempid");
        var Seq = $(this).closest("tr").data("seq");
        ShowProgressBar();
        $.get("/Formats/GetCreateRuleStepsModal", {
            FormatLoopRuleTempID: FormatLoopRuleTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_rule").html(data);
            var select = document.getElementById("ListFieldActive");
            $('#RuleDetailTypeSelected').val('AND');
            var ns = document.createElement("option");
            ns.value = 0;
            ns.text = "";
            select.appendChild(ns);
            $("#tbl_formatsfields").find(" tr:not(:first)").each(function () {
                var tdlist = $(this).find("td");
                var option = document.createElement("option");
                if ($(tdlist[2]).find("button").data("value")) {
                    option.value = $(tdlist[2]).find("button").data("id");
                    option.text = $(tdlist[2]).find("button").data("displayval");
                    select.appendChild(option);
                }
            });
            $(".select").selectpicker({
                liveSearch: false
            });
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $('#RuleTypeSelected').val('CONDITION');
            $("#mo_Create_Rule").modal("show");
            $('#SeqCreate').val(Seq);
            $('#RuleDetailTypeSelected').val('OR');     
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //edit rules detail
    $(document).on('click', '.tbl_btn_EditRule', function (e) {
        e.stopImmediatePropagation();
        var FormatLoopRuleTempID = $(this).closest("tr").data("formatloopruletempid");
        var Seq = $(this).closest("tr").data("seq");
        ShowProgressBar();
        $.get("/Formats/GetCreateRuleStepsModal", {
            FormatLoopRuleTempID: FormatLoopRuleTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_rule").html(data);
            var select = document.getElementById("ListFieldActive");
            $('#RuleDetailTypeSelected').val('AND');
            var ns = document.createElement("option");
            ns.value = 0;
            ns.text = "";
            select.appendChild(ns);
            $("#tbl_formatsfields").find(" tr:not(:first)").each(function () {
                var tdlist = $(this).find("td");
                var option = document.createElement("option");
                if ($(tdlist[2]).find("button").data("value")) {
                    option.value = $(tdlist[2]).find("button").data("id");
                    option.text = $(tdlist[2]).find("button").data("displayval");
                    select.appendChild(option);
                }
            });
            $(".select").selectpicker({
                liveSearch: false
            });
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $('#RuleTypeSelected').val('CONDITION');
            $("#mo_Create_Rule").modal("show");
            $('#div_Step1').hide();
            $('#div_Step3').show();
            $('#SeqCreate').val(Seq);
            $('#RuleDetailTypeSelected').val('AND');
            ShowProgressBar();
            $.get("/eRequest/Formats/GetTblFormatsLoopsRulesDetail", {
                FormatID: $("#SelectFormatID").val(),
                FormatLoopRuleTempID: $("#FormatLoopRuleTempIDCreate").val(),
                TransactionID: $('#TransactionID').val(),
                Seq: $('#SeqCreate').val(),
                FacilityID: $('#FacilityListRuleID').val()
            }).done(function (data) {
                $("#div_tbl_FormatsloopRulesDetail").html('');
                $("#div_tbl_FormatsloopRulesDetail").html(data);
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
            ShowProgressBar();
            $.get("/eRequest/Formats/GetTblFormatsLoopsRulesDetailExceptions", {
                FormatID: $("#SelectFormatID").val(),
                FormatLoopRuleTempID: $("#FormatLoopRuleTempIDCreate").val(),
                TransactionID: $('#TransactionID').val(),
                Seq: $('#SeqCreate').val(),
                FacilityID: $('#FacilityListRuleID').val()
            }).done(function (data) {
                $("#div_tbl_FormatsloopRulesDetailExceptions").html('');
                $("#div_tbl_FormatsloopRulesDetailExceptions").html(data);
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });                    
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Show step 2 from step 1
    $(document).on('click', '#btn_Back_Step2_1', function (e) {
        e.stopImmediatePropagation();
        var SelectText = $('#ListFieldActive option:selected').text();
        $("#remplace1")[0].innerHTML = $("#remplace1")[0].innerHTML.replace(SelectText,"#FIELD#");
        $("#remplace2")[0].innerHTML = $("#remplace2")[0].innerHTML.replace(SelectText,"#FIELD#");
        $("#remplace3")[0].innerHTML = $("#remplace3")[0].innerHTML.replace(SelectText,"#FIELD#");
        $("#remplace4")[0].innerHTML = $("#remplace4")[0].innerHTML.replace(SelectText, "#FIELD#");
        $('#tbody_invalues').html('');
        $(".statehide").css("display", "none");
        $('#ComparatorType').val('0');
        $('#ComparatorType').change();
        $('#div_Step1').show();
        $('#div_Step2').hide();
    });
    
    //Show step 2
    $(document).on('click', '#btn_next_step2', function (e) {
        e.stopImmediatePropagation();
        var SelectText = $('#ListFieldActive option:selected').text();
        var SelectDatePart = $('#SelectDatePart option:selected').text();
        if ($('#ComparatorTypeSelected').val() == 'date') {
            if (SelectDatePart != "") {
                if (SelectText != "") {
                    Step2(SelectText, LangResources.culture);
                } else {
                    notification("", LangResources.msg_SelectFieldFormat, "nft_");
                }
            } else {
                notification("", LangResources.msg_SelectDatePart, "nft_");
            }
        } else {
            if (SelectText != "") {
                Step2(SelectText, LangResources.culture);
            } else {
                notification("", LangResources.msg_SelectFieldFormat, "nft_");
            }
        }
        
    });

    //new exception
    $(document).on('click', '#btn_new_exception', function (e) {
        e.stopImmediatePropagation();
        var seq = $('#SeqCreate').val();
        var FormatLoopRuleTempIDInit = $('#FormatLoopRuleTempIDCreate').val();
        $('#mo_Create_Rule').modal('toggle');
        var FormatLoopRuleTempID = FormatLoopRuleTempIDInit;
        ShowProgressBar();
        $.get("/Formats/GetCreateRuleStepsModal", {
            FormatLoopRuleTempID: FormatLoopRuleTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_rule").html(data);
            var select = document.getElementById("ListFieldActive");
            $('#RuleDetailTypeSelected').val('AND');
            var ns = document.createElement("option");
            ns.value = 0;
            ns.text = "";
            select.appendChild(ns);
            $("#tbl_formatsfields").find(" tr:not(:first)").each(function () {
                var tdlist = $(this).find("td");
                var option = document.createElement("option");
                if ($(tdlist[2]).find("button").data("value")) {
                    option.value = $(tdlist[2]).find("button").data("id");
                    option.text = $(tdlist[2]).find("button").data("displayval");
                    select.appendChild(option);
                }
            });
            $(".select").selectpicker({
                liveSearch: false
            });
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $('#RuleTypeSelected').val('CONDITION');
            $("#mo_Create_Rule").modal("show");
            $('#SeqCreate').val(seq);
            $('#RuleDetailTypeSelected').val('OR')
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    //new exception
    $(document).on('click', '#btn_new_condition', function (e) {
        e.stopImmediatePropagation();
        var seq = $('#SeqCreate').val();
        var FormatLoopRuleTempIDInit = $('#FormatLoopRuleTempIDCreate').val();
        $('#mo_Create_Rule').modal('toggle');
        var FormatLoopRuleTempID = FormatLoopRuleTempIDInit;
        ShowProgressBar();
        $.get("/Formats/GetCreateRuleStepsModal", {
            FormatLoopRuleTempID: FormatLoopRuleTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_rule").html(data);
            var select = document.getElementById("ListFieldActive");
            $('#RuleDetailTypeSelected').val('AND');
            var ns = document.createElement("option");
            ns.value = 0;
            ns.text = "";
            select.appendChild(ns);
            $("#tbl_formatsfields").find(" tr:not(:first)").each(function () {
                var tdlist = $(this).find("td");
                var option = document.createElement("option");
                if ($(tdlist[2]).find("button").data("value")) {
                    option.value = $(tdlist[2]).find("button").data("id");
                    option.text = $(tdlist[2]).find("button").data("displayval");
                    select.appendChild(option);
                }
            });
            $(".select").selectpicker({
                liveSearch: false
            });
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $('#RuleTypeSelected').val('CONDITION');
            $("#mo_Create_Rule").modal("show");
            $('#SeqCreate').val(seq);
            $('#RuleDetailTypeSelected').val('AND')
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    // Para pintar los correctos en el paso 2
    $(document).on("change", "#ComparatorType", function (e) {
        e.stopImmediatePropagation();
        var option = $(this).find("option:selected").text();
        switch (option) {
            case "In":
                $(".statehide").css("display", "none");
                if ($('#ComparatorTypeSelected').val() == 'text') {
                    $("#InText").css("display", "block");
                    $("#InTable").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'date') {
                    $("#InDate").css("display", "block");
                    $("#InTable").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'number') {
                    $("#InNumeric").css("display", "block");
                    $("#InTable").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'select') {
                    $("#InList").css("display", "block");
                    $("#InTable").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'decimal') {
                    $("#InDecimal").css("display", "block");
                    $("#InTable").css("display", "block");
                }
                break;
            case "Between":
                $(".statehide").css("display", "none");
                if ($('#ComparatorTypeSelected').val() == 'text') {
                    $("#BetweenText").css("display", "block");                   
                } else if ($('#ComparatorTypeSelected').val() == 'date') {
                    $("#BetweenDate").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'number') {
                    $("#BetweenNumeric").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'decimal') {
                    $("#BetweenDecimal").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'select') {
                    ComparatorTypeReset(LangResources.msg_ThisFieldCanNotUse);
                } 
                break;
            case "Like":
                $(".statehide").css("display", "none");
                if ($('#ComparatorTypeSelected').val() == 'text') {
                    $("#LikeText").css("display", "block");
                }else if ($('#ComparatorTypeSelected').val() == 'number') {
                    $("#LikeNumeric").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'decimal') {
                    $("#LikeDecimal").css("display", "block");
                } else if ($('#ComparatorTypeSelected').val() == 'date') {
                    ComparatorTypeReset(LangResources.msg_ThisFieldCanNotUse);
                } else if ($('#ComparatorTypeSelected').val() == 'select') {
                    ComparatorTypeReset(LangResources.msg_ThisFieldCanNotUse);
                }  
                break;
            default:
                $(".statehide").css("display", "none");
                break;
        }
    });
    //Calcular si es date para preguntar que tipo de informacion se necesita
    $(document).on("change", "#ListFieldActive", function (e) {
        e.stopImmediatePropagation();
        $('#DatePart').hide();
        var SelectText = $('#ListFieldActive option:selected').text();
        if (SelectText != "") {
            ShowProgressBar();
            $.get("/Formats/GetFormats_GenericFieldData", {
                CatalogDetailID: $('#ListFieldActive').val(),
                FacilityID: $('#FacilityListRuleID').val()
            }).done(function (data) {
                $('#ComparatorTypeSelected').val(data.DataTypeValue);
                $('#IsAdditionalFielSelected').val(data.IsAdditionalField);
                $('#CatalogTagSelected').val(data.CatalogTag);
                $('#TableAdditionalFieldIDSelected').val(data.TableAdditionalFieldID);
                if ($('#ComparatorTypeSelected').val() == 'date') {
                    $('#SelectDatePart').val('0');
                    $('#SelectDatePart').change();
                    $('#DatePart').show();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
           
        }

    });
    //Guardar Regla o Excepcion
    $(document).on("click", "#btn_next_step3", function (e) {
        e.stopImmediatePropagation();
        var IsAdditionalField = $('#IsAdditionalFielSelected').val();
        var FieldID = $('#ListFieldActive').val();
        var DatePartArgument = null;
        if ($('#ComparatorTypeSelected').val() == 'date') {
            DatePartArgument = $('#DatePartSelected').val();
        }        
        if (IsAdditionalField == 1) {
            FieldID = $('#TableAdditionalFieldIDSelected').val();
        }
        var ComparisonOperator = $('#ComparatorType').val();
        var RuleDetailType = $('#RuleDetailTypeSelected').val();
        var ValuesArray = "";
        var Continue = true;
        var option = $('#ComparatorType').find("option:selected").text();
        switch (option) {
            case "In":
                if ($('#ComparatorTypeSelected').val() == 'text') {
                    $(".values_row  td:first-child").each(function () {
                        ValuesArray = $(this).text() + '»' + ValuesArray;
                    });
                } else if ($('#ComparatorTypeSelected').val() == 'date') {
                    $(".values_row  td:first-child").each(function () {
                        ValuesArray = $(this).text() + '»' + ValuesArray;
                    });
                } else if ($('#ComparatorTypeSelected').val() == 'number') {
                    $(".values_row  td:first-child").each(function () {
                        ValuesArray = $(this).text() + '»' + ValuesArray;
                    });
                } else if ($('#ComparatorTypeSelected').val() == 'select') {
                    $(".values_row  td:first-child").each(function () {
                        ValuesArray = $(this).data('catalogdetailid') + '»' + ValuesArray;
                    }); 
                } else if ($('#ComparatorTypeSelected').val() == 'decimal') {
                    $(".values_row  td:first-child").each(function () {
                        ValuesArray = $(this).text() + '»' + ValuesArray;
                    });
                }
                break;
            case "Between":
                if ($('#ComparatorTypeSelected').val() == 'text') {
                    ValuesArray = $('#txt_StartBetweenText').val() + '»' + $('#txt_EndBetweenText').val();
                } else if ($('#ComparatorTypeSelected').val() == 'date') {
                    ValuesArray = $('#txt_StartBetweenDate').val() + '»' + $('#txt_EndBetweenDate').val();
                } else if ($('#ComparatorTypeSelected').val() == 'number') {
                    ValuesArray = $('#txt_StartBetweenNumeric').val() + '»' + $('#txt_EndBetweenNumeric').val();
                } else if ($('#ComparatorTypeSelected').val() == 'decimal') {
                    ValuesArray = $('#txt_StartBetweenDecimal').val() + '»' + $('#txt_EndBetweenDecimal').val();
                } else if ($('#ComparatorTypeSelected').val() == 'select') {
                    Continue = false;
                }
                if (ValuesArray == '»') {
                    ValuesArray = '';
                }
                break;
            case "Like":
                if ($('#ComparatorTypeSelected').val() == 'text') {
                    ValuesArray = $('#txt_LikeText').val();
                } else if ($('#ComparatorTypeSelected').val() == 'number') {
                    ValuesArray = $('#txt_LikeNumeric').val();
                } else if ($('#ComparatorTypeSelected').val() == 'decimal') {
                    ValuesArray = $('#txt_LikeDecimal').val();
                } else if ($('#ComparatorTypeSelected').val() == 'date') {
                    Continue = false;
                } else if ($('#ComparatorTypeSelected').val() == 'select') {
                    Continue = false;
                }
                break;
            default:
                $(".statehide").css("display", "none");
                break;
        }
        if (Continue) {
            ShowProgressBar();
            $.get("/eRequest/Formats/CreateNewRuleDescription", {
                TransactionID: $('#TransactionID').val(),
                FormatLoopRuleTempID: $('#FormatLoopRuleTempIDCreate').val(),
                FieldID: FieldID,
                IsAdditionalField: IsAdditionalField,
                DatePartArgument: DatePartArgument,
                ComparisonOperator: ComparisonOperator,
                RuleDetailType: RuleDetailType,
                Seq: $('#SeqCreate').val(),
                ValuesArray: ValuesArray,
                FacilityID: $('#FacilityListRuleID').val()
            }).done(function (data) {
                HideProgressBar();
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                    $('#div_Step2').hide();
                    $('#SeqCreate').val(data.ID);
                    ShowProgressBar();
                    $.get("/eRequest/Formats/GetTblFormatsLoopsRulesDetail", {
                        FormatID: $("#SelectFormatID").val(),
                        FormatLoopRuleTempID: $("#FormatLoopRuleTempIDCreate").val(),
                        TransactionID: $('#TransactionID').val(),
                        Seq: $('#SeqCreate').val(),
                        FacilityID: $('#FacilityListRuleID').val()
                    }).done(function (data) {
                        $("#div_tbl_FormatsloopRulesDetail").html('');
                        $("#div_tbl_FormatsloopRulesDetail").html(data);
                    }).fail(function (xhr, textStatus, error) {
                        notification("", error.message, "error");
                    }).always(function () {
                        HideProgressBar();
                    });
                    ShowProgressBar();
                    $.get("/eRequest/Formats/GetTblFormatsLoopsRulesDetailExceptions", {
                        FormatID: $("#SelectFormatID").val(),
                        FormatLoopRuleTempID: $("#FormatLoopRuleTempIDCreate").val(),
                        TransactionID: $('#TransactionID').val(),
                        Seq: $('#SeqCreate').val(),
                        FacilityID: $('#FacilityListRuleID').val()
                    }).done(function (data) {
                        $("#div_tbl_FormatsloopRulesDetailExceptions").html('');
                        $("#div_tbl_FormatsloopRulesDetailExceptions").html(data);
                    }).fail(function (xhr, textStatus, error) {
                        notification("", error.message, "error");
                    }).always(function () {
                        HideProgressBar();
                    });                    
                    $('#div_Step3').show();
                    ReloadFormatsLoopRulesTable(false);
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            notification("", LangResourcesmsg_ThisFieldCanNotUse, "nft_");
        }      
    });
    //Boton para abrir modal de nuevo acceso
    $(document).on('click', '#btn_showmodal_accessformat', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get("/Formats/GetNewUserAccessModal").done(function (data) {
            $("#div_add_access_user").html(data);
            $(".select").selectpicker({
                liveSearch: false
            });
            $("#mo_NewAccessFormat").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Boton para editar modal de nueva regla
    $(document).on('click', '.btn-add-phase', function (e) {
        e.stopImmediatePropagation();
        var FormatLoopRuleTempID = $(this).closest("tr").data("id");
        ShowProgressBar();
        $.get("/Formats/GetAddNewRuleModal", {
            FormatLoopRuleTempID: FormatLoopRuleTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val()
        }).done(function (data) {
            $("#div_mo_new_rule").html(data);
            $(".icheckbox").iCheck();
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $("#mo_NewRule").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Boton para agregar modal de nuevo aprobador
    $(document).on('click', '.adddepurty-FormatsLoopsFlow', function (e) {
        e.stopImmediatePropagation();
        var FormatLoopFlowTempID = $(this).closest("tr").data("tempid");
        ShowProgressBar();
        $.get("/Formats/GetAddNewApprover", {
            FormatLoopFlowTempID: FormatLoopFlowTempID,
            FormatID: $("#SelectFormatID").val(),
            TransactionID: $('#TransactionID').val(),
            SelectedFacility: $('#FacilityListRuleID').val()
        }).done(function (data) {
            $("#div_mo_new_approver").html(data);
            $(".select").selectpicker({
                liveSearch: false
            });
            $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
            $('.max-length').maxlength();
            $("#mo_NewApprover").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });
    //Boton para agregar usuario
    $(document).on('click', '#btn_AddUser', function (e) {
        e.stopImmediatePropagation();
        $(this).attr('disabled', true);
        $('#content').hide();
        $('.spinner').removeClass('hide');
        var ProfileField = $('#ProfileID').val();
        var ProfileArrayID = "";

        if (ProfileField != null) {
            for (var i = 0; i < ProfileField.length; i++) {
                ProfileArrayID = ProfileField[i] + "," + ProfileArrayID;
            }
        }
        $.post("/Formats/CreateUser",
            $('#create-form').serialize() + "&ProfileArrayID=" + ProfileArrayID,
            function (data) {

                if (data.ErrorCode == 0) {
                    notification(data.Title, data.ErrorMessage, data.notifyType);
                }
            }).done(function (data) {
                $('.spinner').addClass('hide');
                $('#content').show();
                $('#btn_AddUser').attr('disabled', false);
            });
    });
    //boton para cancelar
    $('.btn-cancel-back').on('click', function () {
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            window.location.href = '/eRequest/Formats/Index';
        }, LangResources.msg_CancelFormatCreate);
    });
    //Tabs
    $(document).on('click', '#generic_tab', function (e) {
        e.stopImmediatePropagation();
        var tab = $(this);
        if (tab.hasClass('panel')) {

        } else {
            tab.addClass('panel-info');
            tab.addClass('panel');
        }
        $('#rules_tab').removeClass('panel');
        $('#rules_tab').removeClass('panel-info');       
        $('#formatPDF_tab').removeClass('panel');
        $('#formatPDF_tab').removeClass('panel-info');
        $('.div_tab_hide').hide();
        $('.div_tab_Generic').show();
    });
    $(document).on('click', '#rules_tab', function (e) {
        e.stopImmediatePropagation();
        var tab = $(this);
        if (tab.hasClass('panel')) {

        } else {
            tab.addClass('panel-info');
            tab.addClass('panel');
        }
        $('#generic_tab').removeClass('panel');
        $('#generic_tab').removeClass('panel-info');
        $('#formatPDF_tab').removeClass('panel');
        $('#formatPDF_tab').removeClass('panel-info');
        $('.div_tab_hide').hide();
        $('.div_tab_Rules').show();

    });
    $(document).on('click', '#formatPDF_tab', function (e) {
        e.stopImmediatePropagation();
        var tab = $(this);
        if (tab.hasClass('panel')) {

        } else {
            tab.addClass('panel-info');
            tab.addClass('panel');
        }
        $('#ListFieldActiveToPDF').html('');
        var select = document.getElementById("ListFieldActiveToPDF");
        var ns = document.createElement("option");
        ns.value = "";
        ns.text = "";
        select.appendChild(ns);
        $("#tbl_formatsfields").find(" tr:not(:first)").each(function () {
            var tdlist = $(this).find("td");
            var option = document.createElement("option");
            if ($(tdlist[2]).find("button").data("value")) {
                option.value = $(tdlist[2]).find("button").data("valueid");
                option.text = $(tdlist[2]).find("button").data("displayval");
                select.appendChild(option);
            }
        });
        $('#ListFieldActiveToPDF').selectpicker('refresh');
        $(".select").selectpicker({
            liveSearch: false
        });
        $('#generic_tab').removeClass('panel');
        $('#generic_tab').removeClass('panel-info');
        $('#rules_tab').removeClass('panel');
        $('#rules_tab').removeClass('panel-info');
        $('.div_tab_hide').hide();
        $('.div_tab_FormatPDF').show();
    });

    $(document).on("click", "#btn_Adduseracces", function (e) {
        e.stopImmediatePropagation();
        var Name = $('#txt_UserAccess option:selected').data('name');
        var UserID = $('#txt_UserAccess').val();
        var FacilityNameList = $('#FacilityListID option:selected').toArray().map(item => item.text);
        var FacilityNameListLength = FacilityNameList.length;
        var FacilityIDList = $('#FacilityListID option:selected').toArray().map(item => item.value);
        if (Name != null && UserID != null) {
            if (pregunta_Validar_Campos(UserID, FacilityIDList)) {
                for (i = 0; i < FacilityNameListLength; i++) {
                    $("#table_accessusers").append(
                        "<tr class='responsibles_row'>" +
                        '<td style="display:none;">' +
                        UserID + ',' + FacilityIDList[i] +
                        "</td>" +
                        "<td>" +
                        Name +
                        "</td>" +
                        "<td>" +
                        FacilityNameList[i] +
                        "</td>" +
                        '<td><button class="btn btn-danger delete-accessuser"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                        "</td>" +
                        "</tr>"
                    );
                }
            }
        }
    });

    //Botones para agregar valores de in
    $(document).on("click", "#btn_add_indate", function (e) {
        e.stopImmediatePropagation();
        var Value = $('#txt_InDate').val();
        if (Value != null && Value != "") {
            $("#tbl_invalues").append(
                "<tr class='values_row'>" +
                "<td>" +
                Value +
                "</td>" +
                '<td><button class="btn btn-danger delete-accessuser"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                "</td>" +
                "</tr>"
            );
            $('#txt_InDate').val('');
        }
    });
    $(document).on("click", "#btn_add_intext", function (e) {
        e.stopImmediatePropagation();
        var Value = $('#txt_InText').val();
        if (Value != null && Value != "") {
            $("#tbl_invalues").append(
                "<tr class='values_row'>" +
                "<td>" +
                Value +
                "</td>" +
                '<td><button class="btn btn-danger delete-accessuser"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                "</td>" +
                "</tr>"
            );
            $('#txt_InText').val('');
        }
    });
    $(document).on("click", "#btn_add_innumeric", function (e) {
        e.stopImmediatePropagation();
        var Value = $('#txt_InNumeric').val();
        if (Value != null && Value != "") {
            $("#tbl_invalues").append(
                "<tr class='values_row'>" +
                "<td>" +
                Value +
                "</td>" +
                '<td><button class="btn btn-danger delete-accessuser"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                "</td>" +
                "</tr>"
            );
            $('#txt_InNumeric').val('');
        }
    });
    $(document).on("click", "#btn_add_indecimal", function (e) {
        e.stopImmediatePropagation();
        var Value = $('#txt_InDecimal').val();
        if (Value != null && Value != "") {
            $("#tbl_invalues").append(
                "<tr class='values_row'>" +
                "<td>" +
                Value +
                "</td>" +
                '<td><button class="btn btn-danger delete-accessuser"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                "</td>" +
                "</tr>"
            );
            $('#txt_InDecimal').val('');
        }
    });
    $(document).on("click", "#btn_add_inList", function (e) {
        e.stopImmediatePropagation();
        var Value = $('#txt_InList').val();
        var Text = $('#txt_InList option:selected').text();
        if (Text != null && Text != "") {
            $("#tbl_invalues").append(
                "<tr class='values_row'>" +
                "<td data-catalogdetailid='" + Value + "'>" +
                Text +
                "</td>" +
                '<td><button class="btn btn-danger delete-accessuser"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                "</td>" +
                "</tr>"
            );
            $('#txt_InDate').val('');
        }
    });
    //Borra datos de un campo de access format

    $(document).on("click", ".delete-accessformat", function (e) {
        e.stopImmediatePropagation();
        var FormatAcceesTempID = $(this).closest("tr").data("tempid");
        $.get("/eRequest/Formats/FormatAccessTempDelete", {
            FormatAccessTempID: FormatAcceesTempID
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                ReloadAccessFormatTable(false);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });

    //Borra file configuration PDF

    $(document).on("click", ".tbl_btn_DeleteConfigurationFieldPDF", function (e) {
        e.stopImmediatePropagation();
        var FileDetailTempID = $(this).closest("tr").data("filedetailtempid");
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.get("/eRequest/Formats/PDFFilesDetail_TEMP_Delete", {
                FileDetailTempID: FileDetailTempID
            }).done(function (data) {
                HideProgressBar();
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                    ReloadTablesConfigurationPFD(LangResources.Culture);
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_Delete_Confirm);
    });

    //Borra condiciones o excepciones de una regla

    $(document).on("click", ".tbl_btn_DeleteRuleDetail", function (e) {
        e.stopImmediatePropagation();
        var FormatLoopRuleDetailTempID = $(this).closest("tr").data("formatloopruledetailtempid");
        $.get("/eRequest/Formats/FormatsLoopsRulesDetail_TEMP_Delete", {
            FormatLoopRuleDetailTempID: FormatLoopRuleDetailTempID
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, data.notifyType);
                ShowProgressBar();
                $.get("/eRequest/Formats/GetTblFormatsLoopsRulesDetail", {
                    FormatID: $("#SelectFormatID").val(),
                    FormatLoopRuleTempID: $("#FormatLoopRuleTempIDCreate").val(),
                    TransactionID: $('#TransactionID').val(),
                    Seq: $('#SeqCreate').val(),
                    FacilityID: $('#FacilityListRuleID').val()
                }).done(function (data) {
                    $("#div_tbl_FormatsloopRulesDetail").html('');
                    $("#div_tbl_FormatsloopRulesDetail").html(data);
                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                }).always(function () {
                    HideProgressBar();
                });
                ShowProgressBar();
                $.get("/eRequest/Formats/GetTblFormatsLoopsRulesDetailExceptions", {
                    FormatID: $("#SelectFormatID").val(),
                    FormatLoopRuleTempID: $("#FormatLoopRuleTempIDCreate").val(),
                    TransactionID: $('#TransactionID').val(),
                    Seq: $('#SeqCreate').val(),
                    FacilityID: $('#FacilityListRuleID').val()
                }).done(function (data) {
                    $("#div_tbl_FormatsloopRulesDetailExceptions").html('');
                    $("#div_tbl_FormatsloopRulesDetailExceptions").html(data);
                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                }).always(function () {
                    HideProgressBar();
                });                   
                ReloadFormatsLoopRulesTable(false);
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    });

    //Borra datos de un campo de una regla

    $(document).on("click", ".tbl_btn_DeleteRule", function (e) {
        e.stopImmediatePropagation();
        var FormatLoopRuleTempID = $(this).closest("tr").data("formatloopruletempid");
        var Seq = parseInt($(this).closest("tr").data("seq"));
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.get("/eRequest/Formats/FormatLoopRuleDetailDelete", {
                FormatLoopRuleTempID: FormatLoopRuleTempID,
                Seq: Seq,
                TransactionID: $('#TransactionID').val()
            }).done(function (data) {
                HideProgressBar();
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                    ReloadFormatsLoopRulesTable(false);
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_Delete_Confirm);
    });

    //Borra Aprobador 
    $(document).on("click", ".delete-FormatsLoopsApprovers", function (e) {
        e.stopImmediatePropagation();
        var FormatLoopApproverTempID = $(this).closest("tr").data("id");
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.get("/eRequest/Formats/FormatLoopApproverTempDelete", {
                FormatLoopApproverTempID: FormatLoopApproverTempID
            }).done(function (data) {
                HideProgressBar();
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                    ReloadFormatsLoopRulesTable(false);
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_Delete_Confirm);
    });
    //Eliminar phase de rules
    $(document).on("click", ".delete-FormatsLoopsFlow", function (e) {
        e.stopImmediatePropagation();
        var FormatLoopFlowTempID = $(this).closest("tr").data("tempid");
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.get("/eRequest/Formats/FormatsLoopsFlowTempDelete", {
                FormatLoopFlowTempID: FormatLoopFlowTempID
            }).done(function (data) {
                HideProgressBar();
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                    ReloadFormatsLoopRulesTable(false);
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_Delete_Confirm);
    });

    //Eliminar Reglas de la lista
    $(document).on("click", ".btn-delete-rule", function (e) {
        e.stopImmediatePropagation();
        var FormatLoopRuleTempID = $(this).closest("tr").data("id");
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.get("/eRequest/Formats/FormatsLoopsRulesDelete", {
                FormatLoopRuleTempID: FormatLoopRuleTempID
            }).done(function (data) {
                HideProgressBar();
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                    ReloadFormatsLoopRulesTable(false);
                } else {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_Delete_Confirm);
    });

    $(document).on("click", ".delete-accessuser", function (e) {
        e.stopImmediatePropagation();
        $(this).closest("tr").remove();
    });


    //Boton para ver el pdf agregado 
    $(document).on('click', '#btn_ShowPDF', function (e) {
        e.stopImmediatePropagation();
        var url = $(this).data('fileurl');
        window.open(url, '_blank');
    });
    $(document).on('click', '#btn_addFile', function (e) {
        e.stopImmediatePropagation();
        $('.dropzone-uploadfile-filetemp').click();
    });
    //DropZone
    $('.dropzone-uploadfile-filetemp').dropzone({
        maxFiles: 1,
        addRemoveLinks: true,
        createImageThumbnails: false,
        acceptedFiles: 'application/pdf',
        previewTemplate: '<div class="uploaded-image" style="display:none;"></div>',
        init: function () {
            this.on("complete", function (file) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    this.removeAllFiles(true);
                }
                var response = JSON.parse(file.xhr.responseText)
                if (response.ErrorCode == 0) {
                    $('#txt_filenameupload').text(response.fName);
                    $('#btn_ShowPDF').data('fileurl',response.pathReturn);
                }
            });
        }
    });

}

function pregunta_Validar_Campos(id, FacilityList) {
    var t = document.getElementById("table_accessusers");
    var trs = t.getElementsByTagName("tr");
    var tds = null;
    var secID = [];
    for (var i = 1; i < trs.length; i++) {
        tds = trs[i].getElementsByTagName("td");
        secID.push(tds[0].innerText);
    }
    var search = -1;
    for (i = 0; i < FacilityList.length; i++) {
        search = secID.indexOf(id + ',' + FacilityList[i]);
        if (search > -1) {
            break;
        }
    }

    if (search > -1) {
        notification("", "No puedes repetir el usuario asignado", "error");  //cambia esta linea para poner una traduccion

        return false;
    } else {
        return true;
    }
}

function pregunta_Validar_Campos_users(id) {
    var t = document.getElementById("table_usersrule");
    var trs = t.getElementsByTagName("tr");
    var tds = null;
    var secID = [];
    for (var i = 1; i < trs.length; i++) {
        tds = trs[i].getElementsByTagName("td");
        secID.push(parseInt(tds[0].innerText));
    }
    var search = secID.indexOf(id);
    if (search > -1) {
        notification("", "No puedes repetir el usuario asignado", "error");  //cambia esta linea para poner una traduccion

        return false;
    } else {
        return true;
    }
}

function FillDetailsCultures(CatalogDetailTempID, CatalogDetailID) {
    $('.loading-process-div').show();

    $.get("/eRequest/Formats/GetCatalogsDetailCultures",
        {
            CatalogDetailTempID: CatalogDetailTempID,
            CatalogDetailID: CatalogDetailID
        }
    ).done(function (data) {
        $('#table_CatalogDetailsCultures').html('');
        $('#table_CatalogDetailsCultures').html(data);

        $('.x-editable').editable({
            onblur: "ignore",
            success: function (response, newValue) {
                if (response.ErrorCode === 0) {
                    notification("", response.ErrorMessage, "success");
                    FillDetails($("#CatalogID").val());
                } else {
                    return response.ErrorMessage;
                }
            }
        });
    }).always(function () {
        $('.loading-process-div').hide();
    });
}

function Fillcultures() {
    var ddlCultures = $("#CultureID");

    var items = "";
    ddlCultures.html(items);
    ddlCultures.val('');
    ddlCultures.selectpicker("refresh");

    $.ajax({
        type: "POST",
        url: "/Administration/Catalogs/CulturesList",
        dataType: 'json',
        success: function (response) {
            $.each(response, function () {
                ddlCultures.append($("<option></option>").val(this['value']).html(this['text']));
            });

            ddlCultures.selectpicker("refresh");
        },
        failure: function (response) {
            console.log(response.responseText);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
}

function FillDetails(CatalogID) {
    $('.loading-process-div').show();
    $.get("/eRequest/Formats/GetCatalogsDetail",
        {
            CatalogID: CatalogID,
            FormatID: $("#SelectFormatID").val()
        }
    ).done(function (data) {
        $('#div_mo_EditDetail').html('');
        $('#div_mo_EditDetail').html(data);
    }).always(function () {
        $('.loading-process-div').hide();
    });
}

function AddPhase() {
    $('.loading-process-div').show();
    var Value = $('#txt_PhaseName').val();
    $.post("/eRequest/Formats/CreateCatalogDetail", {
        ValueID: Value,
        Param1: $("#SelectFormatID").val(),
        Transaction: $('#TransactionID').val()
    }).done(function (data) {
        if (data.ErrorCode == 0) {
            notification("", data.ErrorMessage, data.notifyType);
            ShowProgressBar();
            $.get("/eRequest/Formats/GetCatalogsDetail", {
                FormatID: $("#SelectFormatID").val(),
                TransactionID: $('#TransactionID').val()
            }).done(function (data) {
                //$("#div_tbl_CatalogsDetail").html('');
                $("#div_tbl_CatalogsDetail").html(data);
                $('#txt_PhaseName').val('');
                // InitializeDataTable();
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        } else {
            notification("", data.ErrorMessage, data.notifyType);
        }
    }).always(function () {
        $('.loading-process-div').hide();
    });
}
function ReloadTblPhases() {
    ShowProgressBar();
    $.get("/eRequest/Formats/GetCatalogsDetail", {
        FormatID: $("#SelectFormatID").val(),
        TransactionID: $('#TransactionID').val()
    }).done(function (data) {
        //$("#div_tbl_CatalogsDetail").html('');
        $("#div_tbl_CatalogsDetail").html(data);
        $('#txt_PhaseName').val('');
        // InitializeDataTable();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function InitializeDataTableAccess() {
    $('.accesstable').dataTable({
        "language": {
            "url": "/Base/DataTableLang"
        },
        "pageLength": 25
    });
}

function ReloadAccessFormatTable(OpenModal) {
    ShowProgressBar();
    $.get("/eRequest/Formats/GetAccessFormat", {
        TransactionID: $('#TransactionID').val()
    }).done(function (data) {
        $("#div_tbl_accessformat").html('');
        $("#div_tbl_accessformat").html(data);
        if (OpenModal) {
            $('#mo_NewAccessFormat').modal('toggle');
        }
        InitializeDataTableAccess();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function ReloadTablesConfigurationPFD(LangResourcesCulture) {
    ReloadPDFFieldDetail(LangResourcesCulture);
    ReloadPDFFieldConfiguration(LangResourcesCulture);
    ReloadPDFConfigurationSignature(LangResourcesCulture);
}
function ReloadPDFFieldConfiguration(LangResourcesCulture) {
    ShowProgressBar();
    $.get("/eRequest/Formats/GetPDFFieldConfiguration", {
        FormatID: $("#SelectFormatID").val(),
        TransactionID: $('#TransactionID').val()
    }).done(function (data) {
        $("#div_tbl_PDFFieldConfiguration").html('');
        $("#div_tbl_PDFFieldConfiguration").html(data);
        $(".select").selectpicker({
            liveSearch: false
        });
        $('.maxlength').maxlength();
        $(".decimal-1-places").numeric({ decimalPlaces: 1, negative: false });
        $(".datetimepicker").datepicker({
            autoclose: true,
            format: 'yyyy-mm-dd',
            language: LangResourcesCulture
        });
        $(".integer").numeric({ decimal: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        $(".decimal").numeric();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function ReloadPDFFieldDetail(LangResourcesCulture) {
    ShowProgressBar();
    $.get("/eRequest/Formats/GetPDFFieldDetailConfiguration", {
        FormatID: $("#SelectFormatID").val(),
        TransactionID: $('#TransactionID').val()
    }).done(function (data) {
        $("#div_tbl_PDFFieldDetail").html('');
        $("#div_tbl_PDFFieldDetail").html(data);
        $(".select").selectpicker({
            liveSearch: false
        });
        $('.maxlength').maxlength();
        $(".decimal-1-places").numeric({ decimalPlaces: 1, negative: false });
        $(".datetimepicker").datepicker({
            autoclose: true,
            format: 'yyyy-mm-dd',
            language: LangResourcesCulture
        });
        $(".integer").numeric({ decimal: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        $(".decimal").numeric();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function ReloadPDFConfigurationSignature(LangResourcesCulture) {
    ShowProgressBar();
    $.get("/eRequest/Formats/GetPDFFieldConfigurationSignature", {
        FormatID: $("#SelectFormatID").val(),
        TransactionID: $('#TransactionID').val()
    }).done(function (data) {
        $("#div_tbl_PDFFieldSignature").html('');
        $("#div_tbl_PDFFieldSignature").html(data);
        $(".select").selectpicker({
            liveSearch: false
        });
        $('.maxlength').maxlength();
        $(".decimal-1-places").numeric({ decimalPlaces: 1, negative: false });
        $(".datetimepicker").datepicker({
            autoclose: true,
            format: 'yyyy-mm-dd',
            language: LangResourcesCulture
        });
        $(".integer").numeric({ decimal: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
        $(".decimal").numeric();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function ReloadFormatsFieldsTable() {
    ShowProgressBar();
    $.get("/eRequest/Formats/GetAdditionalFieldTable", {
        FormatID: $("#SelectFormatID").val()
    }).done(function (data) {
        $("#div_tbl_additional_field").html('');
        $("#div_tbl_additional_field").html(data);
        $('.max-length').maxlength();
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}

function ReloadFormatsLoopRulesTable(OpenModal) {
    ShowProgressBar();
    var FacilityID = $('#FacilityListRuleID option:selected').val();
    $.get("/eRequest/Formats/UpdateTblFormatLoopRules", {
        FacilitySelectID: FacilityID,
        TransactionID: $('#TransactionID').val()
    }).done(function (data) {
        $("#div_tbl_FormatsLoopRules").html('');
        $("#div_tbl_FormatsLoopRules").html(data);
        if (OpenModal) {
            $('#mo_NewRule').modal('toggle');
        }
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}

function AddResponsibles(IDToSelect, Name) {
    if (Name != null && IDToSelect != null) {
        if (pregunta_Validar_Campos_users(IDToSelect)) {
            $("#table_usersrule").append(
                "<tr>" +
                '<td style="display:none;">' +
                IDToSelect +
                "</td>" +
                "<td>" +
                Name +
                "</td>" +
                '<td><button class="btn btn-danger delete-accessuser"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                "</td>" +
                "</tr>"
            );
        }
    }
}

function SetAlertConfirmCustomBoxActionSuccess(AcceptFunction, Message, Title, MessageType) {
    if (Message !== '') {
        $('#title_ConfirmDelete_Common_Tag').val(decodeURI(Message));
    }

    var confirmBox = new ConfirmBox();
    confirmBox.yesTag = $('#Yes_Common_Tag').val();
    confirmBox.noTag = $('#Cancel_Common_Tag').val();
    confirmBox.title = Title;
    confirmBox.msg = $('#title_ConfirmDelete_Common_Tag').val();
    confirmBox.showMsg("warning");

    if (MessageType == "info") {
        $(".mb-container").css("background-color", "#2271b3");
        $('#confirmbx_yes').removeClass("btn-danger");
        $('#confirmbx_yes').addClass("btn-info");
    }
    if (MessageType == "Success") {
        $(".mb-container").css("background-color", "#95b75d");
        $('#confirmbx_yes').removeClass("btn-danger");
        $('#confirmbx_yes').addClass("btn-info");
    }


    confirmBox.onAccept = AcceptFunction;
}
function ComparatorTypeReset(LangResourcesmsg_ThisFieldCanNotUse) {
    notification("", LangResourcesmsg_ThisFieldCanNotUse, "nft_");
    $('#ComparatorType').val('0');
    $('#ComparatorType').change();
}


function Step2(SelectText, LangResourcesCulture) {
    $('#div_Step1').hide();
    ShowProgressBar();
    $.get("/Formats/GetFormats_GenericFieldData", {
        CatalogDetailID: $('#ListFieldActive').val(),
        FacilityID: $('#FacilityListRuleID').val()
    }).done(function (data) {
        $('#ComparatorTypeSelected').val(data.DataTypeValue);
        $('#IsAdditionalFielSelected').val(data.IsAdditionalField);
        $('#CatalogTagSelected').val(data.CatalogTag);
        $('#DatePartSelected').val($('#SelectDatePart').val());
        $('#TableAdditionalFieldIDSelected').val(data.TableAdditionalFieldID);
        if ($('#ComparatorTypeSelected').val() == 'select') {
            ShowProgressBar();
            $('#txt_InList').html('');
            var select = document.getElementById("txt_InList");
            var ns = document.createElement("option");
            ns.value = 0;
            ns.text = "";
            select.appendChild(ns);
            $.get("/Formats/GetCatalogList", {
                CatalogTag: $('#CatalogTagSelected').val()
            }).done(function (data) {
                $(data.SelectList).each(function () {
                    var option = document.createElement("option");
                    option.value = this.CatalogDetailID;
                    option.text = this.DisplayText;
                    select.appendChild(option);
                });
                $('#txt_InList').selectpicker('refresh');
                $(".select").selectpicker({
                    liveSearch: false
                });
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });

    $("#remplace1")[0].innerHTML = $("#remplace1")[0].innerHTML.replace("#FIELD#", SelectText);
    $("#remplace2")[0].innerHTML = $("#remplace2")[0].innerHTML.replace("#FIELD#", SelectText);
    $("#remplace3")[0].innerHTML = $("#remplace3")[0].innerHTML.replace("#FIELD#", SelectText);
    $("#remplace4")[0].innerHTML = $("#remplace4")[0].innerHTML.replace("#FIELD#", SelectText);
    $(".datetimepicker").datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        language: LangResourcesCulture
    });
    $('.max-length').maxlength();
    $(".integer").numeric({ decimal: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
    $(".decimal").numeric();
    $('#div_Step2').show();
}