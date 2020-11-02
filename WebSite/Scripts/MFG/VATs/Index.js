// =============================================================================================================================
//  Version: 20190924
//  Author:  Luis Hernandez
//  Created Date: 23 Sep 2019
//  Description:  VAT JS
//  Modifications: 
// =============================================================================================================================
function IndexInit(LangResources) {

    //Asigna los valores iniciales de los filtros y el plugin Selectpicker
    (function SetupDefaultValues() {
        $("#ddl_Shifts").val(0);
        $("#ddl_ProductionLines").val(0);
        $(".selectpicker").selectpicker();
    }());

    //Carga los datos de la tabla con la informacion de los VATs
    function LoadVATsTable() {
        $('.loading-process-div').show();
        var VATName = $("#txt_VatNumber").val();
        var ShiftIDs = $("#ddl_Shifts").val();
        var ProductionLineIDs = $("#ddl_ProductionLines").val();
        var ProductionProcessID = $("#ddl_ProductionProcess").val();


        if (ProductionProcessID === null) {
            ProductionProcessID = [];
        }
        if (ProductionLineIDs === null) {
            ProductionLineIDs = [];
        }
        if (ShiftIDs === null) {
            ShiftIDs = [];
        }

        jQuery.ajaxSettings.traditional = true;
        $.ajax({
            url: '/MFG/VATs/Search',
            type: 'get',
            traditional: true,
            dataType: "json",
            contextType: "application/json",
            data: { VATName, ShiftIDs, ProductionProcessID, ProductionLineIDs }
        }).done(function (data) {
            $("#div_VATsRecordsTable").html(data.View);
            $("#div_VATsRecordsTable").css("display", "block");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }

    $("#btn_Search").on("click", function () {
        LoadVATsTable();
    });

    $(".filters").on("change keypress", function () {
        $("#div_VATSRecordsTable").css("display", "none");
    });

    $("#btn_New").on("click", function () {
        $('.loading-process-div').show();
        $.get("/MFG/VATs/New").done(function (data) {
            $("#_mo_NewEdit").html(data.View);
            $("#mo_NewEditVAT").modal("show");

            $("#txt_mo_ProductionLines").selectpicker();
            $("#txt_mo_Shifts").selectpicker();
            $('.max-length').maxlength();

            $('.loading-process-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $("#ddl_ProductionProcess").on("change", function () {
        var ProductionProcessID = $("#ddl_ProductionProcess option:selected").val();
        $.get("/MFG/VATs/GetProductionLinesList", { ProductionProcessID }).done(function (data) {
            $("#ddl_ProductionLines").empty();
            $.each(data.list, function (key, value) {
                $("#ddl_ProductionLines").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
            });

            $("#ddl_ProductionLines").val(0);
            $("#ddl_ProductionLines").selectpicker("refresh");
        });
    });

    $(document).on("click", ".delete-vat-record", function () {
        var row = $(this).closest("tr");
        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            var VATID = row.attr("id");
            $.post("/MFG/VATs/Delete", { VATID }).done(function (data) {
                if (data.ErrorCode === 0) {
                    row.remove();
                }
                notification("", data.ErrorMessage, data.notifyType);
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }, LangResources.msg_DeleteVATRecordConfirm);
    });

    $(document).on("click", ".edit-VAT-record", function () {
        var VATID = $(this).closest("tr").attr("id");
        $.get("/MFG/VATs/Edit", { VATID }).done(function (data) {
            $("#_mo_NewEdit").html(data.View);
            $("#mo_NewEditVAT").modal("show");


            $("#txt_mo_ProductionLines").val($("#SelectedProductionLine").val());
            $("#txt_mo_ProductionLines").selectpicker();

            $("#txt_mo_Shifts").val($("#SelectedShift").val());
            $("#txt_mo_Shifts").selectpicker();
            $('.max-length').maxlength();

            $('.loading-process-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $(document).on("click", "#btn_ModalAddNewVAT", function () {
        var VATID = null;
        var VATName = $("#txt_mo_VatNumber").val();
        var ShiftID = $("#txt_mo_Shifts option:selected").val();
        var ProductionLineID = $("#txt_mo_ProductionLines option:selected").val();
        var Enabled = $("#chk_Enabled").is(":checked");

        $.post("/MFG/VATs/Save", { VATID, VATName, ShiftID, ProductionLineID, Enabled }).done(function (data) {
            if (data.ErrorCode === 0) {
                $("#mo_NewEditVAT").modal("toggle");
                LoadVATsTable();
            }
            notification("", data.ErrorMessage, data.notifyType);
        });
    });

    $(document).on("click", "#btn_ModalSaveEditedVAT", function () {
        var VATID = $("#VATID").val();
        var VATName = $("#txt_mo_VatNumber").val();
        var ShiftID = $("#txt_mo_Shifts option:selected").val();
        var ProductionLineID = $("#txt_mo_ProductionLines option:selected").val();
        var Enabled = $("#chk_Enabled").is(":checked");

        $.post("/MFG/VATs/Save", { VATID, VATName, ShiftID, ProductionLineID, Enabled }).done(function (data) {
            if (data.ErrorCode === 0) {
                $("#mo_NewEditVAT").modal("toggle");
                LoadVATsTable();
            }
            notification("", data.ErrorMessage, data.notifyType);
        });
    });

}