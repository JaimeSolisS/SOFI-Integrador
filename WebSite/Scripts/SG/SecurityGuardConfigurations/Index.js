// =============================================================================================================================
//  Version: 20200823
//  Author:  Luis Hernandez
//  Created Date: 23 Agosto 2020
//  Description:  SecurityGuardConfigurations JS
//  Modifications: 

// =============================================================================================================================

// Funcionalidad para cambiar de tabs
function openTab(tabName, tab) {
    ShowProgressBar();

    $.get("/SG/SecurityGuardConfigurations/GetTabView", {
        tabName: tabName
    }).done(function (data) {
        //Primero dibuja de nuevo la vista
        $("#" + tabName).html(data);

        //Luego hace todo el movimiento de tabs
        var i, tabcontent, tablinks;
        tabcontent = $(".tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }
        tablinks = $(".tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }
        $("#" + tabName).css('display', 'block');
        tab.addClass("active");
        $("select").selectpicker();
        $('select[multiple]').selectpicker('selectAll');
        $('.max-length').maxlength();
        $(".x-editable").editable({
            success: function (response, newValue) {
                if (response.ErrorCode != 0) {
                    notification("", response.ErrorMessage, response.notifyType);
                    $(".tablinks.active").click();
                }
            }
        });
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function SetDatepickerPlugin(selector, culture) {
    $(selector).datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        language: culture
    });
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
        notification("", data.ErrorMessage, data.notifyType);
        if (data.ErrorCode == 0) {
            $('#mo_NewEditVendorUser').modal('toggle');
            $('#btnSearchSecuritySuppliers').click();
        }
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function ChangeStatusGuard(guardID, enable) {
    ShowProgressBar();
    $.post('/SG/SecurityGuardConfigurations/EnableDisable', {
        guardID: guardID,
        enable: enable
    }).done(function (data) {
        notification("", data.ErrorMessage, data.notifyType);
        if (data.ErrorCode == 0) {
            $("#btnSearchSecurityGuards").click();
        }
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}
function SaveEquipment(equipmentID) {
    ShowProgressBar();
    var equipment = {
        EquipmentID: equipmentID,
        Serial: $('#txtMoNSerie').val(),
        EquipmentName: $('#txtMoEquipmentName').val(),
        EquipmentDescription: $('#txtEquipmentDescription').val(),
        MarkVisitor: $('#txtMoMark').val(),
        ModelVisitor: $('#txtMoModel').val(),
        UseVisitor: $('#txtMoOwner').val(),
        CompanyVisitor: $('#txtMoCompanyDepartment').val(),
        UseVisitor: $('#txtReasonForUse').val(),
        EquipmentTypeID: $('#ddlEquipmentType option:selected').val()
    }

    $.post('/SG/SecurityGuardConfigurations/SaveEquipment', {
        equipment: equipment
    }).done(function (data) {
        notification("", data.ErrorMessage, data.notifyType);
        if (data.ErrorCode == 0) {
            $("#mo_NewEquipment").modal('toggle');
            $('#btnSearchSecurityEquipments').click();
        }
    }).fail(function (xhr, textStatus, error) {
        notification("", error.message, "error");
    }).always(function () {
        HideProgressBar();
    });
}

function IndexInit(LangResources) {
    openTab("Suppliers", $("#supplierTab"));
    $('.selectpicker').selectpicker();

    $(document).on('click', '.tablinks', function (e) {
        e.stopImmediatePropagation();
        var tab = $(this);
        openTab(tab.data("tabname"), tab);
        var btnAddNew = $('.btnNewRecord').first();
        btnAddNew.attr("id", tab.data("btdaddnewid"));
        btnAddNew.text(tab.data("btdaddnewnam"));
    });

    $(document).on('click', '#btnSearchSecuritySuppliers', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var StatusIDs = null;
        if ($("#ddlVendorStatus").val() != null) {
            StatusIDs = $("#ddlVendorStatus").val().join(',');
        }

        $.get('/SG/SecurityGuardConfigurations/SearchVendors', {
            VendorID: $("#ddlVendorCompanies option:selected").val(),
            VendorName: $("#txtVendorName").val(),
            StatusIDs: StatusIDs
        }).done(function (data) {
            $('#divVendorsTable').html(data);
            $(".x-editable").editable();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSearchSecurityGuards', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var StatusIDs = null;
        if ($("#ddlGuardStatus").val() != null) {
            StatusIDs = $("#ddlGuardStatus").val().join(',');
        }

        $.get('/SG/SecurityGuardConfigurations/SearchGuard', {
            GuardName: $("#txtGuardName").val(),
            UniqueNumber: $("#txtGuardBadgeNumber").val(),
            StatusIDs: StatusIDs
        }).done(function (data) {
            $('#divGuardsTable').html(data);
            $(".x-editable").editable();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSearchSecurityBadges', function (e) {
        e.stopImmediatePropagation();
        var BadgesTypes = null;
        if ($("#ddlBadgesList").val() != null) {
            BadgesTypes = $("#ddlBadgesList").val().join(',');
        }
        ShowProgressBar();
        $.get("/SG/SecurityGuardConfigurations/SearchBadges", {
            badgeNumber: $("#txtBadgeName").val(),
            uniqueNumber: $("#txtBadgeNumber").val(),
            badgeTypeIDs: BadgesTypes
        }).done(function (data) {
            $('#divBadgesTable').html(data);
            $(".x-editable").editable();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSearchSecurityEquipments', function (e) {
        e.stopImmediatePropagation();
        var equipmentTypeIDs = null;
        if ($("#ddlEquipmentTypes").val() != null) {
            equipmentTypeIDs = $("#ddlEquipmentTypes").val().join(',');
        }

        ShowProgressBar();
        $.get("/SG/SecurityGuardConfigurations/SearchEquipments", {
            serial: $("#txtNSerie").val(),
            equipmentName: $("#txtEquipmentName").val(),
            equipmentTypeIDs: equipmentTypeIDs,
            upc: $("#txtUPC").val()
        }).done(function (data) {
            $('#divEquipmentTable').html(data);
            //$(".x-editable").editable();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $('#ddlVendorCompanies,#ddlVendorStatus').change(function () {
        $('#divVendorsTable').empty();
    });

    $('#txtVendorName').keydown(function () {
        $('#divVendorsTable').empty();
    });

    $(document).on("click", ".details-control.vendorCompany", function (e) {
        e.stopImmediatePropagation();
        var tr = $(this).closest('tr');
        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {
            var VendorID = tr.data("vendorid");
            ShowProgressBar();
            $.get("/SG/SecurityGuardConfigurations/LoadEmployeesByVendor", {
                VendorID: VendorID,
                FullName: $('#txtVendorName').val()
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="6" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });

    $(document).on('click', '.addEmployeeToCompany', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var vendorID = $(this).closest("tr").data("vendorid");
        $.get('/SG/SecurityGuardConfigurations/GetNewUserVendorModal', {
            VendorID: vendorID,
            SectionName: 'SecurityGuardConfigurations'
        }).done(function (data) {
            $('#div_GenericModalContainer').html(data);
            $('#mo_NewEditVendorUser').modal("show");

            SetDatepickerPlugin('#txtMoVendorUserExpirationDate', LangResources.culture);
            SetupOnlyNumbers();
            $('#txtMoVendorUserInsuranceNumber').val('');
            $('.max-length').maxlength();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSaveNewUserVendor', function (e) {
        e.stopImmediatePropagation();
        SaveVendorUser(false);
    });

    $(document).on('click', '#btnSaveEditedUserVendor', function (e) {
        e.stopImmediatePropagation();
        SaveVendorUser(true);
    });

    $(document).on('click', '.editVendorUser', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var vendorUserID = $(this).closest("tr").data("vendoruserid");
        var vendorID = $(this).closest("tr").data("vendorid");
        $.get('/SG/SecurityGuardConfigurations/GetEditUserVendorModal', {
            vendorUserID: vendorUserID,
            vendorID: vendorID,
            SectionName: 'SecurityGuardConfigurations'
        }).done(function (data) {
            $('#div_GenericModalContainer').html(data);
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

    $(document).on('click', '.deleteVendorUser', function (e) {
        e.stopImmediatePropagation();
        var Row = $(this).closest('tr');
        var vendorUserID = Row.data('vendoruserid');
        var userName = Row.find("td:first-child").text();

        SetConfirmBoxAction(function () {
            $.post('/SG/SecurityGuardConfigurations/DeleteVendorUser', {
                vendorUserID
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    Row.remove();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_ConfirmDeleteVendorUser.replace('##USER##', userName));
    });

    $(document).on("click", ".printLabelVendorUser", function (e) {
        e.stopImmediatePropagation();
        var labelName = " " + $(this).closest('tr').find("td:first-child").text();
        $('#confirmbx_icon').addClass('fa-print');
        SetAlertConfirmCustomBoxAction(function () {

        }, LangResources.msg_PrintLabelConfirm.replace("{0}", ' "' + labelName.trim() + '"'), "imprimir etiqueta", "info");
        $('#confirmbx_icon').removeClass('fa-times');
    });

    $(document).on('click', '.deleteCompany', function (e) {
        e.stopImmediatePropagation();
        var vendorID = $(this).closest('tr').data('vendorid');
        SetConfirmBoxAction(function () {
            $.post('/SG/SecurityGuardConfigurations/DeleteCompany', {
                vendorID: vendorID
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    ShowProgressBar();
                    window.location.reload();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_DeleteCompanyConfirm);

    });

    $(document).on('click', '.deleteBadge', function (e) {
        e.stopImmediatePropagation();
        var badgeID = $(this).closest("tr").data("badgeid");
        SetConfirmBoxAction(function () {
            $.post('/SG/SecurityGuardConfigurations/DeleteBadge', {
                badgeID: badgeID
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    $('#btnSearchSecurityBadges').click();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_DeleteBadgeConfirm);
    });

    $(document).on('click', '#btnAddNewCompany', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/SG/SecurityGuardConfigurations/GetSimpleModal', {
            modalName: "_Mo_NewCompany"
        }).done(function (data) {
            $('#div_GenericModalContainer').html(data);
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
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                $('#mo_NewCompany').modal('toggle');
                window.location.reload();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnAddNewGuard', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/SG/SecurityGuardConfigurations/GetSimpleModal', {
            modalName: "_Mo_NewGuard"
        }).done(function (data) {
            $('#div_GenericModalContainer').html(data);
            $('#mo_NewGuard').modal('show');
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSaveNewGuard', function (e) {
        e.stopImmediatePropagation();

        $.post('/SG/SecurityGuardConfigurations/SaveNewGuard', {
            guardName: $('#txtMoGuardName').val()
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                $('#mo_NewGuard').modal('toggle');
                $("#btnSearchSecurityGuards").click();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnAddNewBadge', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/SG/SecurityGuardConfigurations/GetNewBadgeModal', {

        }).done(function (data) {
            $('#div_GenericModalContainer').html(data);
            $('#mo_NewBadge').modal('show');
            $("#ddlMoBadgeTypes").selectpicker();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSaveNewBadge', function (e) {
        e.stopImmediatePropagation();

        $.post('/SG/SecurityGuardConfigurations/SaveNewBadge', {
            badgeNumber: $('#txtMoBadgeNumber').val(),
            badgeTypeID: $('#ddlMoBadgeTypes option:selected').val()
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                $('#mo_NewBadge').modal('toggle');
                $("#btnSearchSecurityBadges").click();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnAddNewEquipment', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/SG/SecurityGuardConfigurations/GetNewEditEquipmentModal', {

        }).done(function (data) {
            $('#div_GenericModalContainer').html(data);
            $('#mo_NewEquipment').modal('show');
            $("#ddlEquipmentType").selectpicker();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '#btnSaveNewEditEquipment', function (e) {
        e.stopImmediatePropagation();
        var equipmentID = $(this).data("equipmentid");
        SaveEquipment(equipmentID);
    });

    $(document).on('click', '.disableGuard', function (e) {
        e.stopImmediatePropagation();
        var guardID = $(this).closest("tr").data("guardid");
        ChangeStatusGuard(guardID, false);
    });

    $(document).on('click', '.enableGuard', function (e) {
        e.stopImmediatePropagation();
        var guardID = $(this).closest("tr").data("guardid");
        ChangeStatusGuard(guardID, true);
    });

    $(document).on('keydown', '.badgesFilter', function (e) {
        e.stopImmediatePropagation();
        $("#divBadgesTable").empty();
    });

    $(document).on('change', '.badgesFilter', function (e) {
        e.stopImmediatePropagation();
        $("#divBadgesTable").empty();
    });

    $(document).on('keydown', '.guardsFilter', function (e) {
        e.stopImmediatePropagation();
        $("#divGuardsTable").empty();
    });

    $(document).on('change', '.guardsFilter', function (e) {
        e.stopImmediatePropagation();
        $("#divGuardsTable").empty();
    });

    $(document).on('keydown', '.equipmentFilter', function (e) {
        e.stopImmediatePropagation();
        $("#divEquipmentTable").empty();
    });

    $(document).on('change', '.equipmentFilter', function (e) {
        e.stopImmediatePropagation();
        $("#divEquipmentTable").empty();
    });

    $(document).on('click', '.deleteEquipment', function (e) {
        e.stopImmediatePropagation();
        var equipmentID = $(this).closest("tr").data("equipmentid");
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            $.post('/SG/SecurityGuardConfigurations/DeleteEquipment', {
                equipmentID: equipmentID
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    $('#btnSearchSecurityEquipments').click();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }, LangResources.msg_DeleteEquipmentConfirm);
    });

    $(document).on('click', '.editEquipment', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        var equipmentID = $(this).closest("tr").data('equipmentid');
        $.post('/SG/SecurityGuardConfigurations/GetNewEditEquipmentModal', {
            equipmentID: equipmentID
        }).done(function (data) {
            $('#div_GenericModalContainer').html(data);
            $('#mo_NewEquipment').modal('show');
            $("#ddlEquipmentType").selectpicker();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '.printLabel', function (e) {
        e.stopImmediatePropagation();
        var referenceid = $(this).data("referenceid");
        var referencetypeid = $(this).data("referencetypeid");
        var labelname = $(this).closest("tr").find("td:first-child").text();

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
}

