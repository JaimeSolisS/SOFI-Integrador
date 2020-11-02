// =============================================================================================================================
//  Version: 20180701
//  Author:  Guillermo Sánchez
//  Created Date: Domingo 1 de Julio de 2018
//  Description: Contiene funciones JS para la página de Edit de la administración de usuarios
//  Modifications: 

// =============================================================================================================================
function EditInit(LangResources) {

    function ReLoadProductionLines() {
        $("#ddl_ProductionLine").empty();
        $(".company-row").each(function () {
            var FacilityID = $(this).data("facility-id");
            $.post("/Administration/Users/GetProductionLines",
                {
                    FacilityID
                }
            ).done(function (data) {
                $.each(data.list, function () {
                    $("#ddl_ProductionLine").append(
                        "<option value = '" + this["ProductionLineID"] + "' data-productionprocessid='" + this["ProductionProcessID"] + "'>" + this["LineNumber"] + " -- " + this["FacilityName"] + "</option>"
                    )
                })
            });
        })
    };

    function LoadFacilitiesList() {
        $("#Plants").empty();
        var CompanyID = $("#Companies option:selected").val();
        $.get("/Administration/Users/LoadFacilities",
            {
                CompanyID
            }
        ).done(function (data) {
            if (data.ErrorCode == 0) {

                $.each(data.faciliesList, function () {
                    $("#Plants").append(
                        "<option value='" + this["FacilityID"] + "'>" + this["FacilityName"] + "</option>"
                    );
                })

            } else {
                notification("", data.ErrorMessage, "error");
            }
        })
    }

    LoadFacilitiesList();

    //Plugins
    $(".select").selectpicker({
        liveSearch: false
    });

    $("#chk_all").on("change", function () {
        var checkboxes = $(".chk_lines_user");
        checkboxes.prop('checked', $(this).is(':checked'));
    })

    $('#check-all-lines').on('click', function () {
        if ($(this).is(':checked')) {
            $('.line-check').prop("checked", true);
        } else {
            $('.line-check').prop("checked", false);
        }

    });

    $('.x-editable').editable({
        success: function (response, newValue) {
            if (response.ErrorCode == 0) {
                notification(response.Title, response.ErrorMessage, response.notifyType);
            } else {
                return response.ErrorMessage;
            }
        }
    });

    $('#SelectedProfiles').editable({
        placement: "bottom",
        success: function (response, newValue) {
            if (response.ErrorCode == 0) {
                notification(response.Title, response.ErrorMessage, response.notifyType);
            } else {
                return response.ErrorMessage;
            }
        }
    });

    SetupXeditableLines();

    //SetExpandOption(table);

    $('#OrganizationID').on('change', function (e) {
        var organizationID = $(this).val();

        $('#FacilityID').empty();
        $('#FacilityID').trigger('chosen:updated');

        $.get("/Administration/Users/GetCompanies",
            { OrganizationID: organizationID }
        ).done(function (data) {
            $('#CompanyID').empty();

            $.each(data, function () {
                $('#CompanyID').append($("<option />").val(this.CompanyID).text(this.CompanyName));
            });

            $('#CompanyID').trigger('chosen:updated');
        });
    });

    $('#CompanyID').on('change', function (e) {
        var companyID = $(this).val();
        var UserID = $('#ID').val();

        $.get("/Administration/Users/GetFacilities4Company",
            { CompanyID: companyID, UserID: UserID }
        ).done(function (data) {
            $('#FacilityID').empty();

            $.each(data, function () {
                $('#FacilityID').append($("<option />").val(this.FacilityID).text(this.FacilityName));
            });

            $('#FacilityID').trigger('chosen:updated');
        });

    });

    $('#btn_new_facility').on('click', function () {
        $('#btn_new_facility').attr('disabled', true);
        $('.loading-process-div').show();

        $.post("/Users/AddFacility",
            $('#add-facility-form').serialize()
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                var UserID = $('#ID').val();
                $.get("/Users/GetUserFacilities",
                    { UserID: UserID }
                ).done(function (data) {
                    $('#UserFacilitiesTable').html('');
                    $('#UserFacilitiesTable').html(data);
                });
            } else {
                notification(data.Title, data.ErrorMessage, data.notifyType);
            }

        }).always(function () {
            $('.loading-process-div').hide();
            $('#btn_new_facility').attr('disabled', false);
        });
    });

    $('#chk_Enabled').on('click', function () {
        var Enabled = $(this).is(':checked');
        if (Enabled) {
            $('#alert_user_disabled').hide();
        } else {
            $('#alert_user_disabled').show();
        }
        $('.loading-process-div').show();
        $.post("/Administration/Users/UpdateEditable",
            { name: 'Enabled', pk: $("#UserID").val(), value: Enabled }
        ).done(function (data) {
            if (data.ErrorCode !== 0) {
                notification("", response.ErrorMessage, "error");
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $(document).on('click', '.del-userfacility-row', function (e) {
        e.stopImmediatePropagation();

        var entityid = $(this).data("entityid");
        var facilityid = $(this).data("facilityid");
        var companyid = $(this).data("companyid");
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
                    $('.loading-process-div').show();

                    $.get("/Users/DeleteUserFacility",
                        { UserID: entityid, CompanyID: companyid, FacilityID: facilityid }
                    ).done(function (data) {
                        notification(data.Title, data.ErrorMessage, data.notifyType);

                        if (data.ErrorCode === 0) {
                            row.remove();
                        }
                    }).always(function () {
                        $('.loading-process-div').hide();
                    });

                }
            }
        });
    });

    $('#btn_new_Line').on('click', function () {
        $('#btn_new_Line').attr('disabled', true);
        $('.loading-process-div').show();

        $.post("/Users/AddLine",
            $('#add-line-form').serialize()
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                UpdateUserLines();
                UpdateUserLinesList();
            } else {
                notification(data.Title, data.ErrorMessage, data.notifyType);
            }
        }).always(function () {
            $('.loading-process-div').hide();
            $('#btn_new_Line').attr('disabled', false);
        });
    });

    $(document).on('click', '.del-userline-row', function (e) {
        e.stopImmediatePropagation();

        var entityid = $(this).data("entityid");
        var lineid = $(this).data("lineid");
        var row = $(this).parent().parent();

        Lobibox.confirm({
            title: LangResources.lbl_Delete,
            msg: LangResources.title_ConfirmDeleteLine,
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
                    $('.loading-process-div').show();

                    $.get("/Users/DeleteLine",
                        { UserID: entityid, ProductionLineID: lineid }
                    ).done(function (data) {
                        if (data.ErrorCode === 0) {
                            UpdateUserLines();
                            UpdateUserLinesList();
                        }
                        notification(data.Title, data.ErrorMessage, data.notifyType);
                    }).always(function () {
                        $('.loading-process-div').hide();
                    });

                }
            }
        });
    });

    $('#PS_Organization').on('change', function (e) {
        var OrganizationID = $(this).val();

        FillCompanies(OrganizationID);

    });

    $('#PS_Company').on('change', function (e) {
        var companyID = $(this).val();
        var UserID = $('#ID').val();

        FillFacilities(companyID, UserID);

    });

    $('#PS_Facility').on('change', function (e) {
        var FacilityID = $(this).val();
        var UserID = $('#ID').val();

        FillWarehouses(FacilityID, UserID);
    });

    $('#SelectedProfiles').on('change', function () {
        $('.loading-process-div').show();
        $.post("/Administration/Users/UpdateProfiles",
            { UserID: $("#UserID").val(), SelectedIDs: $('#SelectedProfiles').val().join(",") }
        ).done(function (data) {
            if (data.ErrorCode !== 0) {
                notification("", response.ErrorMessage, "error");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    //$('#btn_SaveUserLines').on('click', function () {
    //    var itemlist = [];
    //    $('#UserLinesTable tbody').find("input:checked").each(function () {
    //        var Item = {
    //            RowNumber: $(this).data("entityid"),
    //            FieldKey: $(this).data("processid"),
    //        };
    //        itemlist.push(Item);
    //    });

    //    if (itemlist === '') {
    //        notification('', LangResources.chsn_SelectOption, 'error');
    //        return false;
    //    }
    //    $('.loading-process-div').show();
    //    $.post("/Administration/Users/UpdateUserLines",
    //        { UserID: $("#UserID").val(), SelectedIDs: itemlist }
    //    ).done(function (data) {
    //        if (data.ErrorCode == 0) {
    //            //messaje de todo OK
    //            notification("", data.ErrorMessage, "success");
    //        } else {
    //            notification("", data.ErrorMessage, "error");
    //        }
    //    }).always(function () {
    //        $('.loading-process-div').hide();
    //    });
    //});

    $("#Companies").on("change", function () {
        LoadFacilitiesList();
    })

    $('#add_CompanyPlant').on('click', function () {
        $('.loading-process-div').show();
        var isValid = true;
        $('.company-row').each(function () {
            var companyNew = $("#Companies option:selected").text();
            var facilityNew = $("#Plants option:selected").text();

            var company = $(this).find("td:first-child").text();
            var facility = $(this).find("td:nth-child(2)").text();

            if (companyNew == company && facilityNew == facility) {
                isValid = false;
                notification("", LangResources.AddDuplicate, "warning");
            }
        })

        if (isValid) {
            $("#tbl_complants").append(
                "<tr class='company-row' data-company-id=" + $("#Companies option:selected").val() + " data-facility-id=" + $("#Plants option:selected").val() + ">" +
                "<td>" +
                $("#Companies option:selected").text() +
                "</td>" +
                "<td>" +
                $("#Plants option:selected").text() +
                "</td>" +
                '<td>' +
                '<button class="btn btn-danger delete-row delete_facility"><span class="glyphicon glyphicon-trash"></span></button>' +
                "</td>" +
                "</tr>"
            );


            var UserID = $("#UserID").val();
            var CompanyID = $("#Companies option:selected").val();
            var FacilityID = $("#Plants option:selected").val();

            $.post("/Administration/Users/SaveUserFacility",
                {
                    UserID,
                    CompanyID,
                    FacilityID
                }
            ).done(function (data) {
                if (data.ErrorCode == 0) {
                    $('.loading-process-div').hide();
                    //messaje de todo OK
                    notification("", data.ErrorMessage, "success");
                    ReLoadProductionLines();

                } else {
                    notification("", data.ErrorMessage, "error");
                }
            });
        }
    });

    $("#add_Line").on("click", function () {
        $('.loading-process-div').show();
        var ProductionProcessID = $("#ddl_ProductionLine option:selected").data("productionprocessid");
        var ProductionLineID = $("#ddl_ProductionLine option:selected").val()

        var UserID = $("#UserID").val();
        var LineNumber = $("#ddl_ProductionLine option:selected").text();

        $.post("/Administration/Users/SaveUserLine",
            {
                UserID,
                ProductionLineID,
                ProductionProcessID
            }
        ).done(function (data) {
            $('.loading-process-div').hide();

            if (data.ErrorCode == 0) {
                $("#tbl_PLines").append(
                    '<tr data-productionlineid="' + ProductionLineID + '" data-productionprocessid="' + ProductionProcessID + '">' +
                    "<td>" +
                    $("#ddl_ProductionLine option:selected").text() +
                    "</td>" +
                    "<td>" +
                    '<button class= "btn btn-danger delete-row"><span class="glyphicon glyphicon-trash"></span></button>' +
                    "</td>" +
                    "</tr>"
                )

                //messaje de todo OK
                notification("", data.ErrorMessage, "success");
                ReLoadProductionLines();



            } else {
                notification("", data.ErrorMessage, "error");
            }
        });
    });

    $(document).on("click", ".delete-row", function () {
        $('.loading-process-div').show();
        var row = $(this).closest("tr");
        var UserID = $("#UserID").val();
        var ProductionLineID = $(this).closest("tr").data("productionlineid");

        $.post("/Administration/Users/DeleteUserLine",
            {
                UserID,
                ProductionLineID
            }
        ).done(function (data) {
            if (data.ErrorCode == 0) {
                $('.loading-process-div').hide();
                notification("", data.ErrorMessage, "success");
                row.remove();
                ReLoadProductionLines();
            } else {
                notification("", data.ErrorMessage, "error");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });;
    })

    $(document).on("click", ".delete_facility", function () {
        $('.loading-process-div').show();
        var UserID = $("#UserID").val();
        var CompanyID = $(this).closest('tr').data("company-id");
        var FacilityID = $(this).closest('tr').data("facility-id");
        $(this).closest('tr').remove();

        $.post("/Administration/Users/Delete",
            {
                UserID,
                CompanyID,
                FacilityID
            }
        ).done(function (data) {
            if (data.ErrorCode == 0) {
                $('.loading-process-div').hide();
                //messaje de todo OK
                notification("", data.ErrorMessage, "success");
            } else {
                notification("", data.ErrorMessage, "error");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });;

        ReLoadProductionLines();
    })
}

function UpdateUserLines() {
    $.get("/Users/GetUserLines",
        { UserID: $('#ID').val() }
    ).done(function (data) {
        $('#UserLinesTable').html('');
        $('#UserLinesTable').html(data);
        SetupXeditableLines();
    });
}

function SetupXeditableLines() {
    $('.x-editable-lines').editable({
        success: function (response, newValue) {
            if (response.ErrorCode == 0) {
                UpdateUserLines();
                //notification(response.Title, response.ErrorMessage, response.notifyType);
            } else {
                return response.ErrorMessage;
            }
        }
    });
}

function UpdateUserLinesList() {
    $.get("/Users/GetUserLinesList",
        { UserID: $('#ID').val() }
    ).done(function (data) {
        $('#ProductionLineID').html('');
        $.each(data, function () {
            $('#ProductionLineID').append($("<option></option>").val(this['value']).html(this['text']));
        });
        $('#ProductionLineID').val('').trigger("chosen:updated");
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        $('.loading-process-div').hide();
    });;
}

function FillCompanies(OrganizationID) {

    $('#PS_Facility').empty();
    $('#PS_Facility').trigger('chosen:updated');

    $('#PS_Warehouse').empty();
    $('#PS_Warehouse').trigger('chosen:updated');

    $.get("/Administration/Users/GetCompanies",
        { OrganizationID: OrganizationID }
    ).done(function (data) {
        $('#PS_Company').empty();

        $.each(data, function () {
            $('#PS_Company').append($("<option />").val(this.CompanyID).text(this.CompanyName));
        });

        $('#PS_Company').trigger('chosen:updated');

        if (data.length === 1) {
            var CompanyID = $('#PS_Company').val();
            var UserID = $('#ID').val();

            FillFacilities(CompanyID, UserID);
        }
    });
}

function FillFacilities(CompanyID, UserID) {

    $('#PS_Warehouse').empty();
    $('#PS_Warehouse').trigger('chosen:updated');

    $.get("/Administration/Users/GetFacilities4Company",
        { CompanyID: CompanyID, UserID: UserID }
    ).done(function (data) {
        $('#PS_Facility').empty();

        $.each(data, function () {
            $('#PS_Facility').append($("<option />").val(this.FacilityID).text(this.FacilityName));
        });

        $('#PS_Facility').trigger('chosen:updated');

        if (data.length === 1) {
            var FacilityID = $('#PS_Facility').val();
            var UserID = $('#ID').val();

            FillWarehouses(FacilityID, UserID);
        }
    });
}
