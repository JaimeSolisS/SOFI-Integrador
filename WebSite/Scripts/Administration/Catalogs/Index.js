// =============================================================================================================================
//  Version: 20180710
//  Author:  Nore Del Angel
//  Created Date: 10 de Jul de 2018
//  Description: Contiene funciones JS para la página de Index de la administración de catalogos
//  Modifications: 

// =============================================================================================================================
function IndexInit(LangResources) {

    GetCatalogs('');

    $('#OrganizationID').on('change', function (e) {
        var OrganizationID = $(this).val();
        var ddlFacilities = $("#FacilityID");

        // Limpiar combo de facility
        var items = "";
        $("#FacilityID").html(items);
        $("#FacilityID").val('');
        $("#FacilityID").selectpicker("refresh");

        $.ajax({
            type: "POST",
            url: "/Administration/Catalogs/GetFacilities",
            data: { 'OrganizationID': OrganizationID },
            dataType: 'json',
            success: function (response) {
                $.each(response, function () {
                    ddlFacilities.append($("<option></option>").val(this['FacilityID']).html(this['FacilityName']));
                });

                ddlFacilities.selectpicker("refresh");
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });

    });

    $('#chk_IsSystemValue').on('click', function () {
        $('#hf_IsSystemValue').val($(this).is(':checked'));
    });

    $(document).on('click', '#btn_new_catalog', function () {
        $('#m_CreateCatalog').modal({
            backdrop: 'static',
            keyboard: false
        });
    });
    $('#btn_Create').on('click', function () {
        $('.loading-process-div').show();
        $.post("/Administration/Catalogs/CreateNewCatalog",
            $('#create-form').serialize(),
            function (data) {
                if (data.ErrorCode === 0) {

                    var CatalogID = data.ID;
                    $("#hf_ADF_CatalogID").val(CatalogID);

                    // Establecer el nuevo catalogo como el seleccionado
                    GetCatalogs(CatalogID);

                    // Guardar los parametros y al finalizar mostrar los campos del catalogo
                    //SaveParameters(true);
                    $('#m_CreateCatalog').modal('hide');
                }
                notification(data.Title, data.ErrorMessage, data.notifyType);
            }).always(function () {
                $('.loading-process-div').hide();
            });
    });

    $('#CatalogID').on('change', function (e) {
        var CatalogID = $("#CatalogID").val();
        $('#div_table_CatalogsDetail').addClass('hidden');
        FillCatalogFields(CatalogID);
    });

    $('#btn_edit_Params').on('click', function () {
        $('.loading-process-div').show();
        var CatalogID = $("#CatalogID").val();
        GetCatalogInfo(CatalogID);

        $.get("/Administration/Catalogs/GetCatalogsParameters",
            { CatalogID: CatalogID }
        ).done(function (data) {
            $('#table_CatalogsParameters').html('');
            $('#table_CatalogsParameters').html(data);
            $('#_mo_EditCatalog').modal({
                backdrop: 'static',
                keyboard: false
            });
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });


    $('#btn_new_detail').on('click', function () {
        $('.loading-process-div').show();
        var CatalogID = $("#hf_ADF_CatalogID").val();

        $(this).attr('disabled', true);
        //$('#content').hide();
        //$('.spinner').removeClass('hide');

        $.post("/Administration/Catalogs/CreateCatalogDetail",
            $('#add-detail-form').serialize(),
            function (data) {
                notification(data.Title, data.ErrorMessage, data.notifyType);

                if (data.ErrorCode === 0) {
                    $('#ValueID').val('');
                    $('#Param1').val('');
                    $('#Param2').val('');
                    $('#Param3').val('');
                    $('#Param4').val('');

                    FillDetails(CatalogID);
                }
            }).done(function (data) {
                $('#btn_new_detail').attr('disabled', false);
                //$('.spinner').addClass('hide');
                //$('#content').show();
            }).always(function () {
                $('.loading-process-div').hide();
            });
    });

    $('#btn_Configure').on('click', function () {
        SaveParameters(false);
    });

    $(document).on('click', '.del-detail-row', function (e) {
        $('.loading-process-div').show();
        e.stopImmediatePropagation();
        var CatalogDetailID = $(this).data("catalogdetailid");

        var row = $(this).parent().parent();
        Lobibox.confirm({
            title: LangResources.lbl_Delete,
            msg: LangResources.title_ConfirmDelete,
            buttons: {
                yes: {
                    'class': 'btn btn-success',
                    text: LangResources.btn_ConfirmYES,
                    closeOnClick: true
                },
                no: {
                    'class': 'btn btn-danger',
                    text: LangResources.btn_ConfirmNO,
                    closeOnClick: true
                }
            },
            callback: function (lobibox, type) {
                if (type === 'yes') {
                    $.get("/Administration/Catalogs/DeleteCatalogDetail",
                        { CatalogDetailID: CatalogDetailID }
                    ).done(function (data) {
                        if (data.ErrorCode === 0) {
                            notification("", data.ErrorMessage, "success");
                            row.remove();
                        } else {
                            notification("", data.ErrorMessage, "error");
                        }
                    }).always(function () {
                        $('.loading-process-div').hide();
                    });

                }
            }
        });
    });

    $('#btn_new_Culture').on('click', function () {
        var CatalogDetailID = $("#hf_EDC_CatalogDetailID").val();
        $('.loading-process-div').show();
        //$('#content').hide();
        //$('.spinner').removeClass('hide');

        $.post("/Administration/Catalogs/CreateCatalogDetailCulture",
            $('#add-detailculture-form').serialize(),
            function (data) {
                notification(data.Title, data.ErrorMessage, data.notifyType);
                if (data.ErrorCode === 0) {
                    FillDetailsCultures(CatalogDetailID);
                    $('#ddlCultures').val('');
                    $("#ddlCultures").selectpicker("refresh");
                    $('#TranslationDescription').val('');
                }
            }).done(function (data) {
                //$('.spinner').addClass('hide');
                //$('#content').show();
            }).always(function () {
                $('.loading-process-div').hide();
            });
    });

    //Edit Catalog
    $('#E_C_OrganizationID').on('change', function (e) {
        var OrganizationID = $(this).val();
        var ddlFacilities = $("#E_C_FacilityID");
        FillFacilities(OrganizationID, ddlFacilities, '');
    });

    $('#btn_EditCatalog').on('click', function () {
        $('.loading-process-div').show();
        $.post("/Administration/Catalogs/EditCatalog",
            $('#edit-form').serialize(),
            function (data) {
                if (data.ErrorCode === 0) {
                    var CatalogID = $("#hf_E_C_CatalogID").val();//var CatalogID = data.ID;
                    $("#hf_ADF_CatalogID").val(CatalogID);

                    //Establecer el catalogo como el seleccionado
                    GetCatalogs(CatalogID);

                    // Guardar los parametros y al finalizar mostrar los campos del catalogo
                    SaveParameters(true);
                    $('#_mo_EditCatalog').modal('hide');
                }
                else {
                    $('.loading-process-div').hide();
                }
                notification(data.Title, data.ErrorMessage, data.notifyType);
            });
    });

    $('#chk_E_C_IsSystemValue').on('click', function () {
        $('#hf_E_C_IsSystemValue').val($(this).is(':checked'));
    });

}

function setCheckboxvalue(chk) {
    $(chk).data("value", $(chk).is(':checked'));
}

function FillCatalogFields(CatalogID) {
    if (CatalogID !== '0') {
        $('#btn_edit_Params').removeClass('hidden');
        $('#add-detail-form').removeClass('hidden');
        $('#hf_ADF_CatalogID').val(CatalogID);

        SetEditableControls(CatalogID);
        FillDetails(CatalogID);
        SetDetailsDescriptions(CatalogID);
    }
    else {
        $('#btn_edit_Params').addClass('hidden');
        $('#add-detail-form').addClass('hidden');
        $("#_CatalogsDetailTable tr").remove();
    }
}

function SetDetailsDescriptions(CatalogID) {
    $('.loading-process-div').show();
    $.ajax({
        type: "POST",
        url: "/Administration/Catalogs/GetParametersConfiguration",
        data: { 'CatalogID': CatalogID },
        dataType: 'json',
        success: function (response) {
            $.each(response, function () {
                switch (this['ParamID']) {
                    case 1:
                        if (this['ParamName'] !== null) {
                            $("#lbl_Param1").html(this['ParamName']);
                            $("#div_Param1").removeClass('hidden');
                        }
                        else {
                            $("#div_Param1").addClass('hidden');
                        }
                        break;
                    case 2:
                        if (this['ParamName'] !== null) {
                            $("#lbl_Param2").html(this['ParamName']);
                            $("#div_Param2").removeClass('hidden');
                        }
                        else {
                            $("#div_Param2").addClass('hidden');
                        }
                        break;
                    case 3:
                        if (this['ParamName'] !== null) {
                            $("#lbl_Param3").html(this['ParamName']);
                            $("#div_Param3").removeClass('hidden');
                        }
                        else {
                            $("#div_Param3").addClass('hidden');
                        }
                        break;
                    case 4:
                        if (this['ParamName'] !== null) {
                            $("#lbl_Param4").html(this['ParamName']);
                            $("#div_Param4").removeClass('hidden');
                        }
                        else {
                            $("#div_Param4").addClass('hidden');
                        }
                        break;
                }
            });
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    }).always(function () {
        $('.loading-process-div').hide();
    });
}

function FillDetails(CatalogID) {
    $('.loading-process-div').show();
    $.get("/Administration/Catalogs/GetCatalogsDetail",
        { CatalogID: CatalogID }
    ).done(function (data) {
        $('#div_table_CatalogsDetail').removeClass('hidden');
        $('#table_CatalogsDetail').html('');
        $('#table_CatalogsDetail').html(data);

        SetXEditablePlugin();
        SetupDataTable();

        $('.edit-cultures').on('click', function () {
            var EntityID = $(this).data('entityid');
            var description = $(this).data("description");
            $('#l_ValueID').html(description);

            $('#hf_EDC_CatalogDetailID').val(EntityID);
            $('#m_EditCultures').modal({
                backdrop: 'static',
                keyboard: false
            });
            Fillcultures();
            FillDetailsCultures(EntityID);
        });
        $('.delete-detail').on('click', function () {
            var EntityID = $(this).data('entityid');

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

        });
    }).always(function () {
        $('.loading-process-div').hide();
    });
}

function FillDetailsCultures(CatalogDetailID) {
    $('.loading-process-div').show();

    $.get("/Administration/Catalogs/GetCatalogsDetailCultures",
        { CatalogDetailID: CatalogDetailID }
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

function SaveParameters(IsNewCatalog) {
    $('.loading-process-div').show();
    var CatalogID = $("#hf_ADF_CatalogID").val();
    var ddlCatalog = $("#CatalogID");
    var itemlist = [];

    // Establecer los valores de la tabla
    $("#tb_CatalogsParameters").find(" tr:not(:first)").each(function () {
        var tdlist = $(this).find("td");
        var Check = $(tdlist[1]).html();
        var Item = {
            CatalogID: CatalogID,
            ParamID: $(tdlist[0]).html(),
            Configured: $(tdlist[1]).find("input").data("value"),
            ParamName: $(tdlist[2]).find("input").val(),
            Description: $(tdlist[3]).find("input").val()
        };
        itemlist.push(Item);
    });

    $.ajax({
        url: '/Administration/Catalogs/InsertCatalogsParameters',
        dataType: "json",
        data: JSON.stringify({ Parameterslist: itemlist }),
        type: "POST",
        contentType: "application/json; charset=utf-8"
    }).done(function (data) {
        if (data.ErrorCode === 0) {
            // Ocultar el popup que aplica
            if (IsNewCatalog === false) {
                notification("", data.ErrorMessage, "success");
            }
            else {
                // Ocultar el popup y cargar los campos del catalogo
                $('#m_CreateCatalog').modal('hide');
            }
            FillCatalogFields(CatalogID);

        } else {
            notification("", data.ErrorMessage, "error");
        }
    }).always(function () {
        $('.loading-process-div').hide();
    });
}

function GetCatalogs(CatalogIDSelected) {
    var ddlCatalogs = $("#CatalogID");

    // Limpiar combos de States y Cities
    var items = "";
    ddlCatalogs.html(items);
    ddlCatalogs.val('');

    $.ajax({
        type: "POST",
        url: "/Administration/Catalogs/GetCatalogs",
        dataType: 'json',
        success: function (response) {
            $.each(response, function () {
                ddlCatalogs.append($("<option></option>").val(this['CatalogID']).html(this['CatalogName']));
            });
            ddlCatalogs.val(CatalogIDSelected);
            ddlCatalogs.selectpicker("refresh");
        },
        failure: function (response) {
            console.log(response.responseText);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
}

function SetEditableControls(CatalogID) {
    $('#btn_edit_Params').removeClass('hidden');
    $('#add-detail-form').removeClass('hidden');
    //$.ajax({
    //    type: "POST",
    //    url: "/Administration/Catalogs/GetAccess",
    //    data: { 'CatalogID': CatalogID },
    //    dataType: 'json',
    //    success: function (response) {
    //        if (response.length === 0) {
    //            $('#btn_edit_Params').addClass('hidden');
    //            $('#add-detail-form').addClass('hidden');
    //        }
    //        else {
    //            $('#btn_edit_Params').removeClass('hidden');
    //            $('#add-detail-form').removeClass('hidden');
    //        }
    //    },
    //    failure: function (response) {
    //        console.log(response.responseText);
    //    },
    //    error: function (response) {
    //        console.log(response.responseText);
    //    }
    //});
}

function SetXEditablePlugin() {
    $('.x-editable').editable({
        onblur: "ignore",
        success: function (response, newValue) {
            if (response.ErrorCode === 0) {
                notification("", response.ErrorMessage, "success");
                var CatalogID = $("#CatalogID").val();
                FillCatalogFields(CatalogID);
            } else {
                return response.ErrorMessage;
            }
        }
    });
}

function FillFacilities(OrganizationID, ddlFacilities, FacilityID) {
    var items = "";
    ddlFacilities.html(items);
    ddlFacilities.val('');
    //ddlFacilities.trigger("chosen:updated");

    $.ajax({
        type: "POST",
        url: "/Administration/Catalogs/GetFacilities",
        data: { 'OrganizationID': OrganizationID },
        dataType: 'json',
        success: function (response) {
            $.each(response, function () {
                ddlFacilities.append($("<option></option>").val(this['FacilityID']).html(this['FacilityName']));
            });

            if (FacilityID !== '') {
                ddlFacilities.val(FacilityID);
            }
            ddlFacilities.selectpicker("refresh");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function GetCatalogInfo(CatalogID) {
    $.ajax({
        type: "POST",
        url: "/Administration/Catalogs/GetCatalogInfo",
        data: { 'CatalogID': CatalogID },
        dataType: 'json',
        success: function (response) {
            //if (response.Enabled === true) { $('#div_Warning').addClass('hidden') }
            //else { $('#div_Warning').removeClass('hidden') }
            $('#hf_E_C_CatalogID').val(CatalogID);
            $('#l_E_C_CatalogTag').val(response.CatalogTag);
            $('#E_C_CatalogName').val(response.CatalogName);
            $('#E_C_CatalogDescription').val(response.CatalogDescription);
            $('#E_C_OrganizationID').val(response.OrganizationID);
            $('#E_C_OrganizationID').selectpicker("refresh");

            $('#chk_E_C_IsSystemValue').prop('checked', response.IsSystemValue);

            FillFacilities(response.OrganizationID, $('#E_C_FacilityID'), response.FacilityID);
            //$('#E_C_FacilityID').val(response.FacilityID);
            //$('#E_C_FacilityID').trigger("chosen:updated");
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

function SetupDataTable() {
    $(".datatable").dataTable({
        "language": {
            "url": "/Base/DataTableLang"
        },
        "pageLength": 100,
        responsive: true
    });
}
