// =============================================================================================================================
//  Version: 20190930
//  Author:  Luis Hernandez
//  Created Date: 30 Sep 2019
//  Description:  DemoldDefect JS
//  Modifications: 
// =============================================================================================================================
function IndexInit(LangResources) {
    function SetupNumpad() {
        // These defaults will be applied to all NumPads within this document!
        $.fn.numpad.defaults.gridTpl = '<table class="table modal-content"></table>';
        $.fn.numpad.defaults.backgroundTpl = '<div class="modal-backdrop in"></div>';
        $.fn.numpad.defaults.displayTpl = '<input type="text" class="form-control  input-lg" />';
        $.fn.numpad.defaults.buttonNumberTpl = '<button type="button" class="btn-info btn-lg"></button>';
        $.fn.numpad.defaults.buttonFunctionTpl = '<button type="button" class="btn-lg" style="width: 100%;"></button>';
        $.fn.numpad.defaults.decimalSeparator = '.';
        $.fn.numpad.defaults.textDone = 'OK';
        $.fn.numpad.defaults.textDelete = LangResources.lbl_Del;
        $.fn.numpad.defaults.textClear = LangResources.lbl_Clear;
        $.fn.numpad.defaults.textCancel = LangResources.btnCancel;

        $.fn.numpad.defaults.onKeypadCreate = function () {
            $(this).find('.done').addClass('btn-success');
        };
        $(document).on('click', '.done', function () {
            $("#demold_defect_login").click();
        });
        
        $('.control-numpad').numpad();
    }

    function LoadProductionLines() {
        var ProductionProcessID = parseInt($("#ddl_ProductionProcess option:selected").val());

        //autoseleccionar 1.5 clear
        $("#ddl_ProductionProcess option").each(function () {
            if ($(this).text() == '1.5 Clear') {
                ProductionProcessID = $(this).val();
                $("#ddl_ProductionProcess").val(ProductionProcessID);
                $("#ddl_ProductionProcess").selectpicker("refresh");
            }
        });

        $.get("/MFG/DemoldDefects/GetLinesList", { ProductionProcessID }).done(function (data) {
            $("#ddl_Lines").empty();
            $("#ddl_Lines").append("<option value='0'>" + LangResources.chsn_SelectOption + "</option>");

            $.each(data.list, function (key, value) {
                $("#ddl_Lines").append("<option value='" + value.ProductionLineID + "'>" + value.LineNumber + "</option>");
            });

            $("#ddl_Lines").val($("#val_ProductionLineID").val());
            $("#ddl_Lines").selectpicker("refresh");

            LoadVats();
        });
    }

    function LoadVats() {
        $("#ddl_VATs").empty();
        if ($("#ddl_Shifts option:selected").val() !== 0 && $("#ddl_Lines option:selected").val() !== 0) {
            $('.loading-process-div').show();
            var ShiftIDs = $("#ddl_Shifts").val();
            var ProductionLineIDs = $("#ddl_Lines").val();

            if (ProductionLineIDs === null) {
                ProductionLineIDs = [];
            }
            if (ShiftIDs === null) {
                ShiftIDs = [];
            }


            jQuery.ajaxSettings.traditional = true;
            $.ajax({
                url: '/MFG/DemoldDefects/GetVATList',
                type: 'get',
                traditional: true,
                dataType: "json",
                contextType: "application/json",
                data: { ShiftIDs, ProductionLineIDs }
            }).done(function (data) {
                $.each(data.list, function (key, value) {
                    $("#ddl_VATs").append("<option value='" + value.VATID + "'>" + value.VATName + "</option>");
                });

                $("#ddl_VATs").val($("#val_VATID").val());
                $("#ddl_VATs").selectpicker("refresh");
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }
    }


    //Asigna los valores iniciales de los filtros y el plugin Selectpicker
    (function SetupDefaultValues() {
        SetupNumpad();
        LoadProductionLines();
        $(".selectpicker").selectpicker("refresh");

    }());

    $("#ddl_Shifts, #ddl_Lines").on("change", function () {
        LoadVats();
    });

    $("#demold_defect_login").on("click", function () {
        var ShiftID = $("#ddl_Shifts option:selected").val();
        var ShiftName = $("#ddl_Shifts option:selected").text();
        var ProductionProcessID = $("#ddl_ProductionProcess option:selected").val();
        var ProductionLineID = $("#ddl_Lines option:selected").val();
        var ProductionLineName = $("#ddl_Lines option:selected").text();
        var VATID = $("#ddl_VATs option:selected").val();
        var VATName = $("#ddl_VATs option:selected").text();

        var InspectorName = $("#InspectorName").val();

        if (typeof VATID !== "undefined") {
            if (ShiftID !== 0 && ProductionLineID !== 0 && VATID !== 0 && InspectorName !== "") {
                ShowProgressBar();
                window.location = "/MFG/DemoldDefects/CaptureWizard?ShiftID=" + ShiftID
                    + "&ShiftName=" + ShiftName
                    + "&ProductionProcessID=" + ProductionProcessID
                    + "&ProductionLineID=" + ProductionLineID
                    + "&ProductionLineName=" + ProductionLineName
                    + "&InspectorName=" + InspectorName
                    + "&VATID=" + VATID
                    + "&VATName=" + VATName
                    + "&ProductionProcessID=" + VATName
                    ;
            } else {
                notification("", LangResources.msg_SelectAllFields, "_ntf");
            }
        } else {
            notification("", LangResources.msg_SelectAllFields, "_ntf");
        }
    });

    $("#ddl_ProductionProcess").on("change", function () {
        LoadProductionLines();
    });
}