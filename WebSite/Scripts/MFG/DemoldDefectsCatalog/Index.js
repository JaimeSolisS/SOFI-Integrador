// =============================================================================================================================
//  Version: 20191009
//  Author:  Luis Hernandez
//  Created Date: 9 Oct 2019
//  Description:  DemoldDefectsCatalog JS
//  Modifications: 
// =============================================================================================================================
function IndexInit(LangResources) {
    function LoadDemoldDefectsDataTable() {
        var ShiftID = $("#ddl_Shifts option:selected").val();
        var ProductionLineID = $("#ddl_ProductionLines option:selected").val();
        var VATID = $("#VatNumber option:selected").val();
        var StartDate = $("#txt_StartDateExcel").val();
        var EndDate = $("#txt_EndDateExcel").val();
        var InspectorName = $("#txt_Inspector").val();
        var DefectCat = $("#ddl_DefectCat option:selected").val();
        var DefectType = $("#ddl_DefectType option:selected").val();
        var DesignID = $("#ddl_design option:selected").val();
        ShowProgressBar();
        $.get("/MFG/DemoldDefectsCatalog/Search", {
            ShiftID, ProductionLineID, VATID,
            InspectorName, StartDate, EndDate, DefectCat, DefectType, DesignID
        }).done(function (data) {
            $("#div_Tbl_DemoldDefectDetails").html(data.View);
            $("#div_Tbl_DemoldDefectDetails").css("display", "block");
            XEditable();
            HideProgressBar();
        });
    }

    function XEditable() {
        $('.x-editable').editable({
            success: function (response, newValue) {
                var DemoldDefectDetailID = $(this).closest("tr").data("demolddefectdetailid");
                var identifier = $(this).closest("td").data("identifier");

                switch (identifier) {
                    case "quantity":
                        //$(this).parent().parent().data("quantity", newValue);
                        var Quantity = newValue;
                        $.post("/MFG/DemoldDefectsCatalog/Update",
                            { DemoldDefectDetailID, Quantity }
                        ).done(function (data) {

                        });
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

    $("#ddl_ProductionLines").val(0);
   
    SetupOnlyNumbers();
    SetupOnlyDecimal();
    XEditable();

    $('.datepicker').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd'
    });
    $(".selectpicker").selectpicker("refresh");

    $(".datepicker").on("change", function () {
        $("#IsDateChanged").val(true);
    });
    $("#ddl_DefectCat").on("change", function () {
        var Category = $('#ddl_DefectCat option:selected').val();
        var CatalogTag = "DemoldDefectsTypes";
        $.get("/MFG/DemoldDefects/GetDataOfCategory", { Category, CatalogTag }).done(function (data) {
            $("#ddl_DefectType").empty();
            $('#ddl_DefectType').append($('<option></option>').text(LangResources.TagAll).val(0));
            $.each(data.list, function (key, value) {
                $('#ddl_DefectType').append($('<option></option>').text(value.DisplayText).val(value.CatalogDetailID));
            });
            $('#ddl_DefectType').selectpicker('refresh');
        });
    });
    $(".filters").on("change", function () {
        $("#div_Tbl_DemoldDefectDetails").css("display", "none");
    });

    $("#btn_New").on("click", function () {
        ShowProgressBar();
        window.location = "/MFG/DemoldDefects/Index"
    });

    $("#btn_Search").on("click", function () {
        LoadDemoldDefectsDataTable();
    });

    $(document).on("click", ".delete-demolddefectdetail-parameter", function () {
        var Row = $(this).closest("tr");
        var DemoldDefectDetailID = Row.data("demolddefectdetailid");

        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.post("/MFG/DemoldDefectsCatalog/Delete", { DemoldDefectDetailID }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                Row.remove();
            }).fail(function () {
                notification("", data.ErrorMessage, data.notifyType);
            }).always(function () {
                $('.loading-process-div').hide();
            });
            $('.loading-process-div').hide();
        }, LangResources.msg_DeleteDemoldDefectConfirmMessage);
    });



    $("#btn_Alerts").on("click", function () {
        ShowProgressBar();
        $.get("/MFG/DemoldDefectsCatalog/GetModalAlerts").done(function (data) {
            $("#div_mo_NewEdit").html(data.View);

            //$("#ddl_alerts_DefectsCategories").prepend("<option value='a'>" + LangResources.chsn_SelectOption + "</option>");
            //$("#ddl_alerts_DefectsCategories").val("a");
            //$("#ddl_alerts_Shifts").prepend("<option value='a'>" + LangResources.chsn_SelectOption + "</option>");
            //$("#ddl_alerts_Shifts").val("a");
            $("#ddl_alerts_Families").prepend("<option value='a'>" + LangResources.chsn_SelectOption + "</option>");
            $("#ddl_alerts_Families").val("a");

            $(".selectpicker").selectpicker("refresh");
            $("#mo_Alerts").modal("show");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("change", "#ddl_alerts_Families", function () {
        var MoldFamilyID = $('#ddl_alerts_Families option:selected').val();
        $.get("/MFG/DemoldDefects/GetLensTypesList", { MoldFamilyID }).done(function (data) {
            $("#ddl_lenstypes").empty();
            $('#ddl_lenstypes').append($('<option></option>').text(LangResources.TagAll).val(0));
            $.each(data.list, function (key, value) {
                $('#ddl_lenstypes').append($('<option></option>').text(value.LensTypeName).val(value.LensTypeID));
            });
            $('#ddl_lenstypes').selectpicker('refresh');
        });
    });

    $(document).on("click", "#btn_AddAlert", function () {
        var ShiftID = "a";
        var LineID = 0;
        var FamilyID = $("#ddl_alerts_Families option:selected").val();
        var FamilyName = $("#ddl_alerts_Families option:selected").text();
        var DefectID = $("#ddl_alerts_DefectsCategories option:selected").val();
        var DefectCategory = $("#ddl_alerts_DefectsCategories option:selected").text();
        var HourInterval = $("#txt_alerts_HourInterval").val();
        var Gross = $("#txt_alerts_Gross").val();
        var lenstypeID = $("#ddl_lenstypes").val();
        var lenstypeName = $("#ddl_lenstypes option:selected").text();

        if (FamilyID == "a" || DefectCategory == "a" ||
            HourInterval == "" || Gross == "") {
            notification("", "You must fill all fields", "_ntf");
        } else {
            $("#tbody_Alerts").append(
                '<tr class="alert-data-row" data-shiftid="' + ShiftID + '" data-line="'
                + LineID + '" data-family="' + FamilyID + '"  data-lenstype="'
                + lenstypeID + '" data-gross="' + Gross + '" data-defect="'
                + DefectCategory + '" data-hourinterval="' + HourInterval + ' " data-demolddefectalertid="0" >' +
                '   <td>' + FamilyName + ' </td>' +
                '   <td>' + lenstypeName + ' </td>' +
                '   <td>' + Gross + ' %</td>' +
                '   <td>' + DefectCategory + ' </td>' +
                '   <td>' + HourInterval + ' </td>' +
                '   <td style="text-align:center; width:10%"><button class="btn btn-danger delete-demolddefect-alert"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                '</tr>'
            );
        }
    });

    $(document).on("click", ".delete-demolddefect-alert", function () {
        $(this).closest("tr").remove();
    });

    $(document).on("click", "#btn_SaveAlerts", function () {
        var DemoldDefectAlerts = [];

        $(".alert-data-row").each(function () {
            var DemoldDefectAlert = {
                ShiftID: 0,
                ProductionLineID: 0,
                MoldFamilyID: 0,
                LensTypeID: 0,
                Gross: 0,
                DefectCategory: '',
                HourInterval: 1,
                Enabled: true,
                DemoldDefectAlertID:0
            };
            DemoldDefectAlert.ShiftID = $(this).data("shiftid");
            DemoldDefectAlert.ProductionLineID = $(this).data("line");
            DemoldDefectAlert.MoldFamilyID = $(this).data("family");
            DemoldDefectAlert.LensTypeID = $(this).data("lenstype");
            DemoldDefectAlert.Gross = parseFloat($(this).data("gross"));
            DemoldDefectAlert.DefectCategory = $(this).data("defect");
            DemoldDefectAlert.HourInterval = $(this).data("hourinterval");
            DemoldDefectAlert.DemoldDefectAlertID = $(this).data("demolddefectalertid");

            DemoldDefectAlerts.push(DemoldDefectAlert);
        });
        ShowProgressBar();
        $.post("/MFG/DemoldDefectsCatalog/AlertsUpsert",{
            DemoldDefectAlerts
        }).done(function (data) {
            HideProgressBar();
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                $("#mo_Alerts").modal("toggle");
            }
        });

    });
}