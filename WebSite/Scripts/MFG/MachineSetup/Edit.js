// ==========================================================================================================================================================
//  Version: 20190702
//  Author:  Luis Hernandez
//  Created Date: 02 de Julio de 2019
//  Description: Contiene funciones JS para la página de Edit de Machine Setup
//  Modifications: 
// ==========================================================================================================================================================

function EditInit(LangResources) {

    //funciones
    function getlastformula() {
        var formula = $('#ValueFormula').val('');
        var res = formula.split(" ");
    }
    $('.max-length').maxlength();
    function Sortable() {
        $(".sortable").sortable({
            revert: true,
            items: ">tr",
            appendTo: "parent",
            opacity: 1,
            containment: "document",
            placeholder: "placeholder-style",
            cursor: "move",
            delay: 150,
            start: function (event, ui) {
                $(this).find('.placeholder-style td:nth-child(2)').addClass('hidden-td')
                ui.helper.css('display', 'table')
            },
            stop: function (event, ui) {
                ui.item.css('display', '')
            },
            connectWith: ".parameter_table tbody"
        });
    }

    function SetupxEditable() {

        $('*[data-identifier="alert"] .x-editable').on("click", function () {
            var optionText = $(this).text();
            $(".input-sm option").each(function () {
                if ($(this).text() == optionText) {
                    var value = $(this).val();
                    $(".input-sm").val(value);
                }
            });
        });

        $('*[data-identifier="mandatory"] .x-editable').on("click", function () {
            var optionText = $(this).text();
            $(".input-sm option").each(function () {
                if ($(this).text() == optionText) {
                    var value = $(this).val();
                    $(".input-sm").val(value);
                }
            });
        });

        $('.x-editable').editable({
            success: function (response, newValue) {
                var identifier = $(this).closest("td").data("identifier");

                switch (identifier) {
                    case "machinename":
                        $(this).parent().parent().data("machinename", newValue);
                        $(this).parent().parent().data("machineid", $(".input-sm option:selected").val());
                        break;
                    case "materialname":
                        $(this).parent().parent().data("materialname", newValue);
                        $(this).parent().parent().data("materialid", $(".input-sm option:selected").val());
                        break;
                    case "cycletime":
                        $(this).parent().parent().data("cycletime", newValue);
                        break;
                    case "productionprocess":
                        $(this).parent().parent().data("productionprocess", newValue);
                        $(this).parent().parent().data("productionprocessid", $(".input-sm option:selected").val());
                        break;
                    case "parameter":
                        $(this).parent().parent().data("parametername", newValue);
                        $(this).parent().parent().data("machineparameterid", $(".input-sm option:selected").val());
                        break;
                    case "uom":
                        $(this).parent().parent().data("parameteruomid", newValue);
                        break;
                    case "mandatory":
                        var bool = false;
                        var YesID = $("#YesID").val();
                        if (newValue == YesID) {
                            bool = true;
                        } else {
                            bool = false;
                        }
                        $(this).parent().parent().data("ismandatory", bool);
                        break;
                    case "alert":
                        var bool = false;
                        var YesID = $("#YesID").val();
                        if (newValue == YesID) {
                            bool = true;
                        } else {
                            bool = false;
                        }
                        $(this).parent().parent().data("isalert", bool);
                        break;
                    case "min":
                        var maxval = $(this).closest("tr").find("td:nth-child(8)").text().trim();
                        if (parseFloat(newValue) > parseFloat(maxval)) {
                            notification("", LangResources.MinMaxInvalid, "ntf_");
                            return false;
                        } else {
                            $(this).parent().parent().data("minvalue", newValue);
                        }
                        break;
                    case "max":
                        var minval = $(this).closest("tr").find("td:nth-child(7)").text().trim();
                        if (parseFloat(newValue) < parseFloat(minval)) {
                            notification("", LangResources.MinMaxInvalid, "ntf_");
                            return false;
                        } else {
                            $(this).parent().parent().data("maxvalue", newValue);
                        }

                        break;
                    default:

                }

            }
        });

        $('.x-editable').on("shown", function (e, editable) {
            SetupOnlyDecimal();
            $("input.max-length").maxlength();

        });
    }

    function GetMachinesList(MachineVal) {
        $.get("/MFG/MachineSetup/GetMachineList").done(function (data) {
            if (data.ErrorCode == 0) {
                $("#ddl_MachineSetupName").empty();
                $("#ddl_MachineSetupName").addClass("ddl_MachineSetupName_class");
                $("#ddl_MachineSetupName").append("<option value='a'>" + LangResources.SelectMachine + "</option>");
                $("#ddl_MachineSetupName").append("<option value='0' class='new-machine'>--New Machine--</option>");
                $.each(data.list, function () {
                    $("#ddl_MachineSetupName").append("<option value='" + this["MachineID"] + "'>" + this["MachineName"] + "</option>");
                })

                if (MachineVal != null) {
                    $("#ddl_MachineSetupName").val(MachineVal);
                }

                $("#ddl_MachineSetupName").selectpicker("refresh");

            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }

    function GetMaterialList(MaterialVal) {
        ShowProgressBar();
        $.get("/MFG/MachineSetup/GetMaterialList").done(function (data) {
            if (data.ErrorCode == 0) {
                $("#ddl_Material").empty();
                $("#ddl_Material").addClass("ddl_Material_class");
                $("#ddl_Material").append("<option value='a'>" + LangResources.SelectMaterial + "</option>");
                $("#ddl_Material").append("<option value='0' class='new-material'>--New Material--</option>");
                $.each(data.list, function () {
                    $("#ddl_Material").append("<option value='" + this["CatalogDetailID"] + "'>" + this["DisplayText"] + "</option>");
                })

                if (MaterialVal != null) {
                    $("#ddl_Material").val(MaterialVal);
                }

                $("#ddl_Material").selectpicker("refresh");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }

    function GetParametersList(ParameterVal) {
         ShowProgressBar();
        $.get("/MFG/MachineSetup/GetParametersList").done(function (data) {
            if (data.ErrorCode == 0) {
                $("#AD_Parameter").empty();
                $("#AD_Parameter").addClass("ddl_Parameter_class");
                $("#AD_Parameter").append("<option value='a'>" + LangResources.SelectParameter + "</option>");
                $("#AD_Parameter").append("<option value='0' class='new-parameter'>--New Parameter--</option>");
                $.each(data.list, function () {
                    $("#AD_Parameter").append("<option value='" + this["MachineParameterID"] + "' data-typelist=" + this["ParameterListID"] + ">" + this["ParameterName"] + "</option>");
                });

                if (ParameterVal != null) {
                    $("#AD_Parameter").val(ParameterVal);
                }
                $("#AD_Parameter").selectpicker("refresh");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }

    function GetSectionsList(SectionVal) {
        ShowProgressBar();
        $.get("/MFG/MachineSetup/GetSectionsList").done(function (data) {
            if (data.ErrorCode == 0) {
                $("#ddl_section").empty();
                $("#ddl_section").addClass("ddl_Section_class");
                $("#ddl_section").append("<option value='a'>" + LangResources.SelectSection + "</option>");
                $("#ddl_section").append("<option value='0' class='new-section'>--New Section--</option>");
                $.each(data.list, function () {
                    $("#ddl_section").append("<option value='" + this["CatalogDetailID"] + "'>" + this["DisplayText"] + "</option>");
                })

                if (SectionVal != null) {
                    $("#ddl_section").val(SectionVal);
                }

                $("#ddl_section").selectpicker("refresh");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }

    function RecalculateSequence() {
        var start = 1;
        $('.machine-setup-parameter-row td:first-child').each(function () {
            $(this).text(start);
            start++;
        });
    }

    function HideSectionTableWithoutParamenters() {
        $(".row_section ").each(function () {
            var row = $(this).closest("tr");
            var SectionID = row.data("catalogdetailid");

            if ($("#ListSectionWithParameters option").length == 0) {
                row.hide();
            } else {
                $.each($("#ListSectionWithParameters option"), function () {
                    var SelectOption = $(this).val();
                    if (SectionID == SelectOption) {
                        row.show();
                    }
                });
            }
        });
    };

    function LoadParameterListInSection() {
         ShowProgressBar();
        $(".row_section").each(function () {
            var tr = $(this).closest("tr");
            var MachineSetupID = $("#MachineSetupID").val();
            var ParametersetupID = tr.data("catalogdetailid");

            ShowProgressBar();
            $.get("/MFG/MachineSetup/LoadMachineSetupParameters", { MachineSetupID, ParametersetupID }
            ).done(function (data) {
                tr.after('');
                tr.after('<tr class="section_' + ParametersetupID + '"><td colspan="6" class="padding-0" >' + data + '</td></tr>');
                tr.next("tr").hide();
                SetupxEditable();
                Sortable();
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
                HideProgressBar();

                $(".machine-setup-parameter-row").each(function () {
                    eval("'" + $(this).closest("tr").data("functionfvalue") + "'");
                    eval("'" + $(this).closest("tr").data("functionfminvalue") + "'");
                    eval("'" + $(this).closest("tr").data("functionfmaxvalue") + "'");

                    if ($(this).closest("tr").data("functionminvalue").startsWith("m")) {
                        $(this).find("td:nth-child(7)").text(LangResources.lbl_IsCalculated);
                        $(this).find("td:nth-child(7)").addClass("edit_formula formula_type_min xeditable_link_style");
                        $(this).find("td:nth-child(7)").data("formula-type", "min");
                        //$(this).find("td:nth-child(7)").append('<label class="xeditable_link_style">' + LangResources.lbl_IsCalculated+'</label>');
                    }

                    if ($(this).closest("tr").data("functionmaxvalue").startsWith("m")) {
                        $(this).find("td:nth-child(8)").text(LangResources.lbl_IsCalculated);
                        $(this).find("td:nth-child(8)").addClass("edit_formula formula_type_max xeditable_link_style");
                        $(this).find("td:nth-child(8)").data("formula-type", "max");
                    }

                });

            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        });
    }

    HideSectionTableWithoutParamenters();

    SetupxEditable();

    Sortable();

    SetupOnlyDecimal();

    GetMachinesList();

    GetMaterialList();

    GetParametersList();

    LoadParameterListInSection();

    $("input.max-length").maxlength();

    $(".selectpicker").selectpicker("refresh");

    $(".details-control").on("click", function (e) {
        var tr = $(this).closest("tr");
        var ParametersetupID = tr.data("catalogdetailid");

        $(".section_" + ParametersetupID).removeAttr("hidden");

        if (tr.hasClass("shown")) {
            $(".section_" + ParametersetupID).hide();
            tr.removeClass('shown');
        } else {
            $(".section_" + ParametersetupID).show();
            tr.addClass('shown');
        }

    });

    $(document).on('click', '.delete-machine-material', function (e) {
        e.stopImmediatePropagation();
        $(this).closest('tr').remove();
    });

    $(document).on('click', '.delete-machine-setup-parameter', function (e) {
        e.stopImmediatePropagation();
        $(this).closest('tr').remove();
        RecalculateSequence();
    });

    $(document).on("click", ".clean-machine-setup-section", function (e) {
        var ParameterSectionID = $(this).closest('tr').data("catalogdetailid");
        $("#tbl_Section_" + ParameterSectionID + " .machine-setup-parameter-row").each(function () {
            $(this).addClass("hidden machine_parameter_hidden");
        });
    });

    $(document).on("click", "#btn_AddSetupParameter", function (e) {
        e.stopImmediatePropagation();
        var isValid = true;
        var ParameterName = $("#AD_Parameter option:selected").text();

        $(".machine-setup-parameter-row").each(function () {
            var ExistParam = $(this).find("td:nth-child(2)").text().trim();
            if (ExistParam == ParameterName.trim()) {
                isValid = false;
            }
        });

        if (isValid) {
            var MachineSetupID = $("#MachineSetupID").val();
            var UoM = $("#AD_UoM option:selected").text();
            var Seq = 1;
            var MachineParameterID = $("#AD_Parameter option:selected").val();
            var ParameterUoMID = $("#AD_UoM option:selected").val();
            var IsMandatory = $("#IsMandatory").is(':checked');
            var IsAlert = $("#IsAlert").is(':checked');
            var MinValue = $("#AD_Min").val();
            var MaxValue = $("#AD_Max").val();
            var ParameterSectionID = $("#ddl_section option:selected").val();
            var SectionName = $("#ddl_section option:selected").text().replace(/ /g, "_");
            var ValueIsTypeFormula = $(this).data("valueistypeformula");
            var MinIsTypeFormula = $(this).data("ministypeformula");
            var MaxIsTypeFormula = $(this).data("maxistypeformula");


            if (typeof ValueIsTypeFormula === "undefined") {
                ValueIsTypeFormula = null;
            }
            if (typeof MinIsTypeFormula === "undefined") {
                MinIsTypeFormula = null;
            }
            if (typeof MaxIsTypeFormula === "undefined") {
                MaxIsTypeFormula = null;
            }

            var tr = $("#" + SectionName).closest("tr");
            var ParameterSectionID = tr.data("catalogdetailid");
            var MachineSetupID = $("#MachineSetupID").val();

            //Validaciones de campos
            if (parseFloat(MinValue) > parseFloat(MaxValue)) {
                notification("", LangResources.MinMaxInvalid, "ntf_");
                isValid = false;
            }

            if ($("#ddl_section option:selected").val() == 0 || $("#ddl_section option:selected").val() == "a") {
                notification("", LangResources.msg_RequiredSection, "nft_");
                isValid = false;
            }

            if ($("#AD_Parameter option:selected").val() == "a") {
                notification("", LangResources.RequiredParameter, "ntf_");
                isValid = false;
            }

            if ($(".div_no_list").css('display') == 'block' && $("#AD_UoM option:selected").val() == 0) {
                notification("", LangResources.RequiredUoM, "ntf_");
                isValid = false;
            }

            if ($(".div_no_list").css('display') == 'block' && MinValue == "" && !$("#MinIsFormula").is(":checked")) {
                notification("", LangResources.RequiredMinValue, "ntf_");
                isValid = false;
            }

            if ($(".div_no_list").css('display') == 'block' && MaxValue == "" && !$("#MaxIsFormula").is(":checked")) {
                notification("", LangResources.RequiredMaxValue, "ntf_");
                isValid = false;
            }

            var MandatoryText = "";
            if ($("#IsMandatory").is(':checked')) {
                MandatoryText = LangResources.IfYes;
            } else {
                MandatoryText = LangResources.IfNo;
            }

            var AlertText = "";
            if ($("#IsAlert").is(':checked')) {
                AlertText = LangResources.IfYes;
            } else {
                AlertText = LangResources.IfNo;
            }
            //Fin de validaciones de campos

            if (isValid) {
                //Si los parametros de la tabla estan expandidos, solo haz en append
                if (tr.hasClass("shown")) {
                    $("#tbl_Section_" + ParameterSectionID).append(
                        "<tr class='machine-setup-parameter-row' " +
                        " data-machinesetupid=" + MachineSetupID +
                        " data-parametersectionid=" + ParameterSectionID +
                        " data-parametername=" + ParameterName +
                        " data-uom=" + UoM +
                        " data-seq=" + Seq +
                        " data-machineparameterid=" + MachineParameterID +
                        " data-parameteruomid=" + ParameterUoMID +
                        " data-ismandatory=" + IsMandatory +
                        " data-isalert=" + IsAlert +
                        " data-minvalue=" + MinValue +
                        " data-maxvalue=" + MaxValue +
                        " data-functionvalue='" + ValueIsTypeFormula + "'" +
                        " data-functionminvalue='" + MinIsTypeFormula + "'" +
                        " data-functionmaxvalue='" + MaxIsTypeFormula + "'" +
                        ">" +
                        "<td>" +
                        Seq +
                        "</td>" +

                        "<td data-identifier='parameter'>" +
                        '<a href="#" class="x-editable" data-type="select"' +
                        'data-value="' + ParameterName + '"' +
                        'data-source="/MFG/MachineSetup/ParameterList" data-title="' + LangResources.lbl_Parameter + '">' + ParameterName + '</a>' +
                        "</td>" +

                        "<td>" +
                        //LangResources.lbl_IsCalculated +
                        "</td >" +

                        "<td data-identifier='uom'>" +
                        '<a href="#" class="x-editable" data-type="select" data-value=' + $("#AD_UoM option:selected").val() + ' data-source="/MFG/MachineSetup/UoMList" data-title="' + LangResources.lbl_UoM + '">' +
                        $("#AD_UoM option:selected").text() +
                        '</a> ' +
                        "</td>" +

                        '<td data-identifier="mandatory">' +
                        '<a href="#" class="x-editable" data-type="select" data-value="' + IsMandatory + '" data-source="/MFG/MachineSetup/YesNoList"' +
                        'data-title="' + LangResources.lbl_Mandatory + '" >' + MandatoryText + '</a >' +
                        '</td>' +

                        '<td data-identifier="alert">' +
                        '<a href="#" class="x-editable" data-type="select" data-value="' + IsAlert + '" data-source="/MFG/MachineSetup/YesNoList"' +
                        'data-title="' + LangResources.IsAlert + '" >' + AlertText + '</a >' +
                        '</td>' +

                        "<td data-identifier='min'>" +
                        '<a href="#" class="x-editable" data-type="text"' +
                        'data-value="' + MinValue + '"' +
                        'data-title="Min" >' + MinValue + '</a >' +
                        "</td>" +

                        "<td data-identifier='max'>" +
                        '<a href="#" class="x-editable" data-type="text"' +
                        'data-value="' + MaxValue + '"' +
                        'data-title="Max" >' + MaxValue + '</a >' +
                        "</td>" +

                        '<td><button class="btn btn-danger delete-machine-material"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                        "</tr>"
                    );
                } else {
                    /*
                     * Si nos parametros de la tabla no estan cargados, cargalos, y luego haz el append, esto se hizo asi porque la 
                     * tabla de parametros, no existe hasta que se le pica al boton de expandir, asi que el append no se podría hacer
                    */
                    ShowProgressBar();
                    $.get("/MFG/MachineSetup/LoadMachineSetupParameters", { ParameterSectionID, MachineSetupID }
                    ).done(function (data) {
                        tr.after('');
                        tr.after('<tr><td colspan="6" class="padding-0" >' + data + '</td></tr>');
                        tr.addClass('shown');

                        $("#tbl_Section_" + ParameterSectionID).append(
                            "<tr class='machine-setup-parameter-row' " +
                            " data-machinesetupid=" + MachineSetupID +
                            " data-parametersectionid=" + ParameterSectionID +
                            " data-parametername=" + ParameterName +
                            " data-uom=" + UoM +
                            " data-seq=" + Seq +
                            " data-machineparameterid=" + MachineParameterID +
                            " data-parameteruomid=" + ParameterUoMID +
                            " data-ismandatory=" + IsMandatory +
                            " data-isalert=" + IsAlert +
                            " data-minvalue=" + MinValue +
                            " data-maxvalue=" + MaxValue +
                            " data-functionvalue='" + ValueIsTypeFormula + "'" +
                            " data-functionminvalue='" + MinIsTypeFormula + "'" +
                            " data-functionmaxvalue='" + MaxIsTypeFormula + "'" +
                            ">" +
                            "<td>" +
                            Seq +
                            "</td>" +

                            "<td data-identifier='parameter'>" +
                            '<a href="#" class="x-editable" data-type="select"' +
                            'data-value="' + ParameterName + '"' +
                            'data-source="/MFG/MachineSetup/ParameterList" data-title="' + LangResources.lbl_Parameter + '">' + ParameterName + '</a>' +
                            "</td>" +

                            "<td>" +
                            //LangResources.lbl_IsCalculated +
                            "</td >" +

                            "<td data-identifier='uom'>" +
                            '<a href="#" class="x-editable" data-type="select" data-value=' + $("#AD_UoM option:selected").val() + ' data-source="/MFG/MachineSetup/UoMList" data-title="' + LangResources.lbl_UoM + '">' +
                            $("#AD_UoM option:selected").text() +
                            '</a> ' +
                            "</td>" +

                            '<td data-identifier="mandatory">' +
                            '<a href="#" class="x-editable" data-type="select" data-value="' + IsMandatory + '" data-source="/MFG/MachineSetup/YesNoList"' +
                            'data-title="' + LangResources.lbl_Mandatory + '" >' + MandatoryText + '</a >' +
                            '</td>' +

                            '<td data-identifier="alert">' +
                            '<a href="#" class="x-editable" data-type="select" data-value="' + IsAlert + '" data-source="/MFG/MachineSetup/YesNoList"' +
                            'data-title="' + LangResources.IsAlert + '" >' + AlertText + '</a >' +
                            '</td>' +

                            "<td data-identifier='min'>" +
                            '<a href="#" class="x-editable" data-type="text"' +
                            'data-value="' + MinValue + '"' +
                            'data-title="Min" >' + MinValue + '</a >' +
                            "</td>" +

                            "<td data-identifier='max'>" +
                            '<a href="#" class="x-editable" data-type="text"' +
                            'data-value="' + MaxValue + '"' +
                            'data-title="Max" >' + MaxValue + '</a >' +
                            "</td>" +

                            '<td><button class="btn btn-danger delete-machine-material"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                            "</tr>"
                        );


                        SetupxEditable();
                        $('div.slider', tr.next()).slideDown();
                        HideProgressBar();
                    }).fail(function (xhr, textStatus, error) {
                        notification("", error.message, "error");
                    }).always(function () {
                        HideProgressBar();
                    });
                }

                $(".class_Section_" + ParameterSectionID).show();

                tr.next("tr").show();
                RecalculateSequence();
                SetupxEditable();
                $("#ValueIsTypeFormula").val("");
                $("#MinIsTypeFormula").val("");
                $("#MaxIsTypeFormula").val("");
                $(this).data("valueistypeformula", null);
                $(this).data("ministypeformula", null);
                $(this).data("maxistypeformula", null);
                $(".close").click();
            }
        } else {
            notification("", LangResources.msg_ParameterExistInTable, "_ntf");
        }
    });

    $(document).on("click", "#btn_back", function () {
        $(this).modal("hide");
        $("#ParameterChoosedToFormula").val("");
    });

    $(document).on("click", "#btn_NewMachine", function () {
        var MachineName = $("#MachineName").val();
        var MachineDescription = $("#MachineDescription").val();

        if (MachineName == "" || MachineDescription == "") {
            notification("", LangResources.MachDescObligtory, "ntf_");
        } else {
             ShowProgressBar();
            $.post("/MFG/MachineSetup/SaveNewMachine", {
                MachineName,
                MachineDescription,
                ProductionLineID: $("#ProductionLine option:selected").val(),
                Enabled: $('#MachineEnabled').is(':checked'),
                ReferenceID: $("#ReferenceID").val(),
                MachineCategoryID: $("#MachineCategory option:selected").val()
            }).done(function (data) {
                HideProgressBar();



                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    $("#mo_NewMachine").modal("toggle");
                    GetMachinesList(data.ID);
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }
    });

    $(document).on("click", "#btn_AddMaterial", function () {
        var MaterialName = $("#MaterialName").val();

         ShowProgressBar();
        $.post("/MFG/MachineSetup/SaveNewMaterial", {
            MaterialName
        }).done(function (data) {
            HideProgressBar();
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                $("#mo_NewMaterial").modal("toggle");
                GetMaterialList(data.ID);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("change", "#AD_Parameter", function () {
        if ($("#AD_Parameter option:selected").data("typelist") != null) {
            $(".div_no_list").css("display", "none");
        } else {
            $(".div_no_list").css("display", "block");
        }

        if ($(this).val() == 0 && $("#AD_Parameter option:selected").hasClass("new-parameter")) {
             ShowProgressBar();
            //$("#ddl_section option[value='0']").prop('selected', true);
            //$("#ddl_section").selectpicker("refresh");

            GetParametersList();

            $.get("/MFG/MachineSetup/GetModalNewParameter").done(function (data) {
                HideProgressBar();

                if (data.ErrorCode == 0) {
                    $("#div_Mo_NewParameter").html(data.View);
                    $(".select").selectpicker("refresh");
                    $("input.max-length").maxlength();
                    $("#mo_NewParameter").modal("show");

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
        }
    });

    $(document).on("click", "#btn_AddSection", function () {
        var SectionName = $("#SectionName").val();
        //var SectionDescrition = $("#SectionDescription").val();
        $.post("/MFG/MachineSetup/SaveNewSection",
            {
                SectionName
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                //si todo esta bien
                $("#btn_back_new_section").click();

                GetSectionsList(data.ID);
                $("#tbl_machine_setup").append(
                    '<tr class="font-size-sm row_section class_Section_' + data.ID + '" data-catalogdetailid="' + data.ID + '"" id = "' + SectionName.replace(" ", "_") + '">' +
                    '<td class="details-control" width="5%">' +
                    '</td>' +
                    '<td data-identifier="section">' +
                    '<a href="#" class="x-editable editable editable-click" data-type="text" data-value="' + SectionName + '" data-title="Section">' + SectionName + '</a>' +
                    '</td>' +
                    '<td width="13%" style="text-align:center;">' +
                    '<button class="btn btn-danger clean-machine-setup-section"><span class="glyphicon glyphicon-trash"></span></button>' +
                    '</td>' +
                    '</tr>'
                );


                var structure =
                    '<div class="slider">' +
                    '<table class="table display table-condensed table-strdisplay table table-striped table-bordered table-condensed-xs margin-noneiped parameter_table" id="tbl_Section_' + data.ID + '" cellspacing="0">' +
                    '<thead>' +
                    '<tr class="font-size-sm bg-primary">' +
                    '<th>#</th>' +
                    '<th>' + LangResources.lbl_Parameter + '</th>' +
                    '<th>' + LangResources.lbl_UoM + '</th>' +
                    '<th>' + LangResources.lbl_Mandatory + '</th>' +
                    '<th>' + LangResources.IsAlert + '</th>' +
                    '<th>' + 'Min' + '</th>' +
                    '<th>' + 'Max' + '</th>' +
                    '<th style="width:5%">' + 'Opciones' + '</th>' +
                    '</tr>' +
                    '</thead>' +
                    '<tbody>' +
                    '<tr style="border-color:transparent"><td colspan="8"></td></tr>' +
                    '</tbody>' +
                    '</table>' +
                    '</div>';


                var tr = $("#" + SectionName.replace(" ", "_") + "");
                tr.after('');
                tr.after('<tr class="section_' + data.ID + '"><td colspan="6" class="padding-0" >' + structure + '</td></tr>');
                tr.next("tr").show();
                SetupxEditable();
                Sortable();
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
                HideProgressBar();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("change", "#ddl_Type", function () {
        var option = $(this).find("option:selected").text();
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

        }
    });

    $(document).on("click", "#btn_ModalAddNewParameter", function () {
        var option = $("#ddl_Type").find("option:selected").text();
         ShowProgressBar();
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
            $.post("/MFG/MachineSetup/SaveNewParameter", {
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
                HideProgressBar();
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    $("#div_Mo_NewMachine").html(data.View);
                    $("#mo_NewMachine").modal("show");

                    GetParametersList(data.ID);
                    $(".close-2").click();
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

    $('#ddl_MachineSetupName').on("change", function () {
        if ($(this).val() == 0 && $("#ddl_MachineSetupName option:selected").hasClass("new-machine")) {
            $("#ddl_MachineSetupName").val("a");
             ShowProgressBar();

            $.get("/MFG/MachineSetup/GetModalNewMachine").done(function (data) {
                HideProgressBar();

                if (data.ErrorCode == 0) {
                    $("#div_Mo_NewMachine").html(data.View);
                    $("#mo_NewMachine").modal("show");
                    $("input.max-length").maxlength();
                    $("textarea.max-length").maxlength();
                    $("input.max-length").maxlength();

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
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }
    });

    $('#ddl_Material').on("change", function () {
        if ($(this).val() == 0 && $("#ddl_Material option:selected").hasClass("new-material")) {
            $("#ddl_Material").val("a");
             ShowProgressBar();

            $.get("/MFG/MachineSetup/GetModalNewMaterial").done(function (data) {
                HideProgressBar();

                if (data.ErrorCode == 0) {
                    $("#div_Mo_NewMaterial").html(data.View);
                    $("input.max-length").maxlength();
                    $("#mo_NewMaterial").modal("show");
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

    $("#btn_Seccion").on("click", function () {
         ShowProgressBar();

        $.get("/MFG/MachineSetup/GetModalAddSetupParameter").done(function (data) {

            if (data.ErrorCode == 0) {
                $("#div_Mo_AddSetupParameter").html(data.View);
                GetParametersList();
                GetSectionsList();
                $(".select").selectpicker("refresh");
                $("#mo_AddSetupParameter").modal("show");

            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
            HideProgressBar();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        });
    });

    $('#btn_save').on('click', function () {
        var MachineSetupName = $("#MachineSetupName").val();
        var MachineSetupID = $("#MachineSetupID").val();
        var MaterialSetupList = [];
        var MachineSetupParametrsList = [];
        var TempListDeletedSections = [];
        var Enabled = false;
        var Seq = 1;

        var TempListDeletedSection = {
            MachineSetupID: 0,
            ParameterSectionID: 0
        };
        TempListDeletedSections.push(TempListDeletedSection);

        if ($("#MachineSetupName").val() == "") {
            notification('', LangResources.NameMandatory, 'ntf_');
            return false;
        }

        if ($('.machine_material_row').length == 0) {
            notification('', LangResources.MessageNoMaterial, 'ntf_');
            return false;
        }

        if ($('.machine-setup-parameter-row').length == 0) {
            notification('', LangResources.MessageNoParameter, 'ntf_');
            return false;
        }

         ShowProgressBar();

        if ($('#Enabled').is(':checked')) {
            Enabled = true;
        }

        $('.machine_material_row').each(function () {
            var Item = {
                MachineID: $(this).data("machineid"),
                MaterialID: $(this).data("materialid"),
                CycleTime: $(this).find("td:nth-child(3)").text(),
                ProductionProcessID: $(this).data("productionprocessid")
            };
            MaterialSetupList.push(Item);
        });
        $('.machine-setup-parameter-row:not(.hidden)').each(function () {
            var tr = $(this).closest("tbody");
            var parent = tr.parent().attr("id").replace("tbl_Section_", "");
            var MachineParameterID = $(this).data("machineparameterid");

            var Item = {
                MachineSetupID: $(this).data("machinesetupid"),
                ParameterSectionID: parent,
                ParameterName: $(this).data("parametername"),
                UoM: $(this).data("uom"),
                Seq: Seq,
                MachineParameterID: MachineParameterID,
                ParameterUoMID: $(this).data("parameteruomid"),
                IsMandatory: $(this).data("ismandatory"),
                IsAlert: $(this).data("isalert"),
                MinValue: $(this).data("minvalue"),
                MaxValue: $(this).data("maxvalue"),
                FunctionValue: $(this).data("functionvalue"),
                FunctionMinValue: $(this).data("functionminvalue"),
                FunctionMaxValue: $(this).data("functionmaxvalue")
            };
            MachineSetupParametrsList.push(Item);
            Seq++;
        });
        $(".machine_parameter_hidden").each(function () {
            var tr = $(this).closest("tbody");
            var parent = tr.parent().attr("id").replace("tbl_Section_", "");
            var Item = {
                MachineSetupID: $(this).data("machinesetupid"),
                ParameterSectionID: parent
            };
            TempListDeletedSections.push(Item);
        });

        $.post("/MFG/MachineSetup/Save",
            {
                MachineSetupID,
                MachineSetupName,
                Enabled,
                MaterialSetupList,
                MachineSetupParametrsList,
                TempListDeletedSections
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {

            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $('#add_machine_material').on('click', function (e) {
        e.stopImmediatePropagation();

        if ($("#CycleTime").val() == "") {
            notification("", LangResources.CycleTimeMandatory, "ntf_");
            return false;
        }

        var isValid = true;
        $('.machine_material_row').each(function () {
            var machine = $(this).find("td:first-child").text();
            var material = $(this).find("td:nth-child(2)").text();

            var machineSelected = $("#ddl_MachineSetupName option:selected").text();
            var materialSelected = $("#ddl_Material option:selected").text();

            if (machine == machineSelected && material == materialSelected) {
                notification("", LangResources.MessageMachineMaterialRepeat, "ntf_");
                isValid = false;
            }
        });

        var $machine = $("#ddl_MachineSetupName option:selected");
        var $material = $("#ddl_Material option:selected");

        if ($("#ddl_ProductionProcess option:selected").val() == 0) {
            notification("", LangResources.msg_ProductionProcessMandatory, "ntf_");
            isValid = false;
        }

        if ($("#ddl_MachineSetupName").prop('selectedIndex') != 0 && $("#ddl_Material").prop('selectedIndex') != 0
            && $machine != "a" && $material != "a" && $machine != 0 && $material != 0) {
            if (isValid) {
                $("#tbl_edit").append(
                    "<tr data-machineid=" + $machine.val() + " data-materialid=" + $material.val() + " class='machine_material_row' data-productionprocessid=" + $("#ddl_ProductionProcess option:selected").val() + ">" +
                    '<td data-identifier="machinename">' +
                    '    <a href="#" class="x-editable" data-type="select"' +
                    '        data-value="' + $machine.val() + '"' +
                    '        data-source="/MFG/MachineSetup/MachinesList" data-title="' + LangResources.lbl_Machine + '">' + $machine.text() + '</a>' +
                    '</td>' +

                    '<td data-identifier="materialname">' +
                    '    <a href="#" class="x-editable" data-type="select"' +
                    '        data-value="' + $material.val() + '"' +
                    '        data-source="/MFG/MachineSetup/MaterialsList" data-title="' + LangResources.lbl_Material + '">' + $material.text() + '</a>' +
                    '</td>' +

                    '<td data-identifier="cycletime">' +
                    '    <a href="#" class="x-editable" data-type="text"' +
                    '        data-value="' + $("#CycleTime").val() + '"' +
                    '        data-source="/MFG/MachineSetup/MaterialList" data-title="' + LangResources.lbl_CycleTime + '">' + $("#CycleTime").val() + '</a>' +
                    '</td>' +

                    '<td data-identifier="productionprocess">' +
                    '    <a href="#" class="x-editable" data-type="select"' +
                    '        data-value="' + $("#ddl_ProductionProcess option:selected").val() + '"' +
                    '        data-source="/MFG/MachineSetup/ProductionProcessList" data-title="' + LangResources.lbl_ProductionProcess + '">' + $("#ddl_ProductionProcess option:selected").text() + '</a>' +
                    '</td>' +
                    '<td><button class="btn btn-danger delete-machine-material"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                    "<tr>"
                );
                SetupxEditable();
            }
        } else {
            notification("", LangResources.MachMatMandatory, "ntf_");
        }

        GetMachinesList("a");
        GetMaterialList("a");
        $("#CycleTime").val("");
        $("#ddl_ProductionProcess").val(0);
        $(".selectpicker").selectpicker("refresh");
    });

    $(document).on("change", "#ddl_section", function () {
        var option = $("#ddl_section option:selected");
        if (option.val() == 0) {
            $.get("/MFG/MachineSetup/GetModalNewSection").done(function (data) {
                HideProgressBar();

                if (data.ErrorCode == 0) {
                    HideProgressBar();
                    $("#div_Mo_NewSection").html(data.View);
                    GetParametersList();
                    GetSectionsList();
                    $(".select").selectpicker("refresh");
                    $("#mo_NewSection").modal("show");

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

    $(document).on("change", "#ddl_ReferenceType", function () {
        if ($("#ddl_ReferenceType option:selected").text() == "LIST") {
            $("#hide_referenceList").css("display", "block");
        } else {
            $("#hide_referenceList").css("display", "none");
        }
    });

    ////////////////////////////////Formula Parameter/////////////////////////////
    $(document).on("change", ".isFormula", function () {
        var ValueSectionID = $(this).attr("id");
        if ($(this).is(":checked")) {
            if ($("#AD_Parameter option:selected").val() != "0" && $("#AD_Parameter option:selected").val() != "a") {
                 ShowProgressBar();

                switch (ValueSectionID) {
                    case "MinIsFormula":
                        $("#AD_Min").css("display", "none");
                        break;
                    case "MaxIsFormula":
                        $("#AD_Max").css("display", "none");
                        break;
                    default:

                }

                $.get("/MFG/MachineSetup/GetModalParametersFormulas", { ValueSectionID }).done(function (data) {
                    HideProgressBar();

                    if (data.ErrorCode == 0) {
                        $("#div_Mo_ParametersFormula").html(data.View);
                        $("input.max-length").maxlength();
                        $("#mo_ParamsFormulas").modal("show");
                    } else {
                        notification("", data.ErrorMessage, data.notifyType);
                    }
                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                }).always(function () {
                    HideProgressBar();
                });
            } else {
                notification("", "Select the parameter to assign formula", "_ntf");
            }
        } else {
            switch (ValueSectionID) {
                case "ValueIsFormula":
                    $("#ValueIsTypeFormula").val("");
                    break;
                case "MinIsFormula":
                    $("#AD_Min").css("display", "block");
                    isMinFormula = null;
                    $("#MinIsTypeFormula").val("");
                    break;
                case "MaxIsFormula":
                    $("#AD_Max").css("display", "block");
                    $("#MaxIsTypeFormula").val("");
                    break;
                default:
            }
        }
    });

    $(document).on("click", "#btn_addtoformula", function () {
        var $li = $("ul#function_options li.active");
        var inputid = $($li).data("inputid");
        var val = $(inputid).val().replace(/ /g, "_");

        $('#ValueFormula').val($('#ValueFormula').val() + ' ' + val);
    });

    $(document).on("click", "#btn_clearformula", function () {
        $('#ValueFormula').val('');
    });

    $(document).on("click", "#btn_delformula", function () {
        var s = $('#ValueFormula').val();
        var withoutLastSpace = s.slice(0, s.lastIndexOf(" "));
        $('#ValueFormula').val(withoutLastSpace);
    });

    $(document).on("change", "#formula_operator,#formula_Parameter", function () {
        $('#btn_addtoformula').click();
    });

    $(document).on("click", "#btn_AddParamForula", function () {
        var openParenthesses = 0;
        var closeParenthesses = 0;
        var MainParameter = $("#AD_Parameter option:selected").text().replace(/ /g, "_").replace("%", "").replace("(", "").replace(")", "");
        var getData = "m" + MainParameter + " " + $("#ValueFormula").val().replace(/ /g, ",").replace("%", "").trim();

        //Validacion de la formula, que tenga el formato correcto
        var str = $("#ValueFormula").val().trimLeft();
        var res = str.match(/^([(][ ])*(([param_][A-Za-z_0-9%]+)|[0-9.]+)((([ ][+*/-][ ])*)*(([(][ ])*([param_]+[A-Za-z_0-9%]+)|[0-9.]+)([ ][)])*)*([ ][)])*/g);
        if (str == res) {

            for (var i = 0; i < str.length; i++) {
                switch (str.charAt(i)) {
                    case "(":
                        openParenthesses++;
                        break;
                    case ")":
                        closeParenthesses++;
                        break;
                    default:
                }
            }

            if (openParenthesses == closeParenthesses) {

                switch ($(this).data("identifier")) {
                    case "ValueIsFormula":
                        //$("#ValueIsTypeFormula").val(getData);
                        $("#btn_AddSetupParameter").data("valueistypeformula", getData);
                        break;
                    case "MinIsFormula":
                        //$("#MinIsTypeFormula").val(getData);
                        $("#btn_AddSetupParameter").data("ministypeformula", getData);
                        break;
                    case "MaxIsFormula":
                        //$("#MaxIsTypeFormula").val(getData);
                        $("#btn_AddSetupParameter").data("maxistypeformula", getData);
                        break;
                    default:
                }

                notification("", LangResources.msg_ParameterFormulaAdded + $("#AD_Parameter option:selected").text(), "success");
                $("#mo_ParamsFormulas").modal("toggle");
            } else {
                notification("", LangResources.msg_ParameterFormulaError + $("#AD_Parameter option:selected").text(), "error");
            }
        } else {
            notification("", LangResources.msg_ParameterFormulaError + $("#AD_Parameter option:selected").text(), "error");
        }
    });

    $(document).on("click", "#btn_EditParamForula", function () {
        var openParenthesses = 0;
        var closeParenthesses = 0;
        var CurrentRowOfParameter = $("#" + $(this).data("currentrowofparameter"));

        var ParameterChoosedToFormula = $(this).data("parameterchoosedtoformula");
        ParameterChoosedToFormula = ParameterChoosedToFormula.replace(/ /g, "_").replace("%", "").replace("(", "").replace(")", "");
        var getData = "m" + ParameterChoosedToFormula + " " + $("#ValueFormula").val().replace(/ /g, ",").replace("%", "").trim();

        var str = $("#ValueFormula").val().trimLeft();
        var res = str.match(/^([(][ ])*(([param_][A-Za-z_0-9%]+)|[0-9.]+)((([ ][+*/-][ ])*)*(([(][ ])*([param_]+[A-Za-z_0-9%]+)|[0-9.]+)([ ][)])*)*([ ][)])*/g);
        if (str == res) {
            for (var i = 0; i < str.length; i++) {
                switch (str.charAt(i)) {
                    case "(":
                        openParenthesses++;
                        break;
                    case ")":
                        closeParenthesses++;
                        break;
                    default:
                }
            }

            if (openParenthesses == closeParenthesses) {
                switch ($(this).data("identifier")) {
                    case "value":
                        CurrentRowOfParameter.data("functionvalue", getData);
                        break;
                    case "min":
                        CurrentRowOfParameter.data("functionminvalue", getData);
                        break;
                    case "max":
                        CurrentRowOfParameter.data("functionmaxvalue", getData);
                        break;
                    default:
                }



                notification("", LangResources.msg_ParameterFormulaAdded + ParameterChoosedToFormula, "success");
                $("#mo_ParamsFormulas").modal("toggle");
            } else {
                notification("", LangResources.msg_ParameterFormulaError + ParameterChoosedToFormula, "error");
            }
        } else {
            notification("", LangResources.msg_ParameterFormulaError + ParameterChoosedToFormula, "error");
        }

        $("#ParameterChoosedToFormula").val("");
        CurrentRowOdParameter = null;
    });

    $(document).on("click", ".edit_formula", function () {
        var MachineSetupParameterID = $(this).closest("tr").data("machinesetupparameterid");
        //var ValueSectionID = $(this).attr("class");
        var CurrentRowOdParameter = $(this).closest("tr").attr("id");
        var ParameterChoosedToFormula = $(this).closest("tr").find("td:nth-child(2)").text().trim();
        var ValueSectionID = $(this).data("formula-type");
        ShowProgressBar();
        $.get("/MFG/MachineSetup/GetModalParametersFormulas", { ValueSectionID, MachineSetupParameterID, ParameterChoosedToFormula, CurrentRowOdParameter }).done(function (data) {

            if (data.ErrorCode == 0) {
                $("#div_Mo_ParametersFormula").html(data.View);
                $("input.max-length").maxlength();
                $(".selectpicker").selectpicker();
                $('#btn_EditParamForula').data("parameterchoosedtoformula", ParameterChoosedToFormula);
                $("#mo_ParamsFormulas").modal("show");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });
    ////////////////////////////////Formula Parameter/////////////////////////////
}