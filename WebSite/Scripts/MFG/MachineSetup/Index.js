// =============================================================================================================================
//  Version: 20191705
//  Author:  Luis Hernandez
//  Created Date: 17 May 2019
//  Description:  MachineSetup JS
//  Modifications: 
// =============================================================================================================================

/* Config app */
var app = {
    Area: {
        MFG: {
            MachineSetup: {
                MachineSetup_AddDetails: "/MSF/MachineSetup/MachineSetup_AddDetails"
            }
        }
    }
};

function IndexGetControls() {
    this.div_MPE = $('#div_MPE');
}

function MachineSetupEntity() {
    this.MachineSetupID = 0;
    this.MachineSetupName = '';
    this.Enabled = true;
    this.MaterialID = 181;
    this.MachineID = 2;
}

function IndexInit(LangResources) {

    //$(".select").selectpicker({
    //    liveSearch: false
    //});

    $(".filterMachineSetup").on("change", function () {
        $("#div_boxMachineSetupInfo").css("display", "none");
    });

    $(".filterMachineSetup").on("keydown", function () {
        $("#div_boxMachineSetupInfo").css("display", "none");
    });

    $(document).on('click', '.edit-machine-setup', function (e) {
        e.stopImmediatePropagation();
        var entityID = $(this).data("entityid");

        $('.loading-process-div').show();
        window.location = "/MFG/MachineSetup/Edit?MachineSetupID=" + entityID;
    });

    $(document).on('click', '.delete-machine-setup', function (e) {
        e.stopImmediatePropagation();
        var entityID = $(this).closest("tr").data("entityid");
        var row = $(this).closest("tr");

        $('.loading-process-div').show();
        $.post("/MFG/MachineSetup/DeleteMachinesetup",
            {
                entityID
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                row.remove();
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $(".select").selectpicker();

    RegisterEventMachineSetupDetail();

    $(document).on('click', '.edit-machine-setup', function (e) {
        e.stopImmediatePropagation();

        var machineSetupID = this.attributes["data-machine-setup-id"].value;
        var entity = new MachineSetupEntity();
        entity.MachineSetupID = machineSetupID;
    });

    $(document).on('click', '#btn_search', function (e) {
        e.stopImmediatePropagation();

        var setup = $("#txt_setup").val();
        var machines = $("#dpw_machines").val();
        var material = $("#dpw_material").val();

        $.ajax({
            url: '/MFG/MachineSetup/Search',
            type: "post",
            data: { MachineSetupName: setup, MachineID: machines, MaterialID: material }
        }).done(function (data) {
            $("#div_boxMachineSetupInfo").empty();
            $("#div_boxMachineSetupInfo").append(data);
            $("#div_boxMachineSetupInfo").css("display", "block");
        });
    });

    //RegisterEventDefectProcessDetail();
    //RegisterEventEditDefectProcess();

    $('#ddl_ProductionProcess').on('change', function (e) {
        e.stopImmediatePropagation();

        ForceApplyFilters();
        var ProductionProcessValue = $(this).val();
        var controls = new IndexGetControls();

        controls.ddl_ProductionLine.selectpicker('val', '[0]');
        controls.ddl_ProductionLine.selectpicker('refresh');
        controls.div_ContainerControlProductionLine.addClass("hidden");

        if (ProductionProcessValue !== '') {
            /* Web Method - Read Setting Apply Lines?  */
            var ProductionProcessParams = {
                CatalogDetailID: parseInt(ProductionProcessValue)
                , ParamIndex: 1
            };
            var ProductionProcessRequest = new CallWebMethod(app.Area.Production.Defects.ProductionProcessApplyLines, ProductionProcessParams);
            ProductionProcessRequest.success(function (response) {
                ShowProgressBar();
                if (response.ApplyLines) {
                    /* Web Method - List Lines  */
                    controls.div_ContainerControlProductionLine.removeClass("hidden");
                } // Fin ApplyLines
                //FillDefectInfoTable();
            });
        } else {
            //FillDefectInfoTable();
            //HideProgressBar();
        }
    });

    $('.applyfilters').on('change', function (e) {
        e.stopImmediatePropagation();
        //ForceApplyFilters();
        //FillDefectInfoTable();
    });

    //SetupOnlyDecimal();

    //RegisterEventDeleteAllDetails();

}

function RegisterEventMachineSetupDetail() {
    var table = $('#tbl_machine_setup');

    //expandir detalles con plugin DataTable
    $(document).on('click', 'td.details-control', function (e) {
        e.stopPropagation();

        var tr = $(this).closest('tr');

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {
            var MachineSetupID = tr.data("entityid");
            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get("/MFG/MachineSetup/LoadMaterials", { MachineSetupID }
            ).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="6" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                //RegisterEventDeleteDefectProcess();
                HideProgressBar();
            });
        }
    });
}


