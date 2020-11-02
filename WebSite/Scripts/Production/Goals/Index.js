// =============================================================================================================================
//  Version: 20190307
//  Author:  Felipe Vera
//  Created Date: 07 March 2019
//  Description:  Goals JS
//  Modifications: 
// =============================================================================================================================
/* Config app */
var app = {
    Area: {
        Production: {
            Goals: {
                ProductionProcessApplyLines: "/Production/Goals/ProductionProcessApplyLines",
                ProductionGoals_List: "/Production/Goals/ProductionGoals_List",
                LoadNewGoalInfo: "/Production/Goals/ProductionGoal_LoadNewGoalInfo",
                ProductionGoal_Add: "/Production/Goals/ProductionGoal_Add",
                ProductionGoal_Delete: "/Production/Goals/ProductionGoal_Delete",
                ProductionGoalDetail_List: "/Production/Goals/ProductionGoalDetail_List",
                ProductionGoalDetail_BulkUpsert: "/Production/Goals/ProductionGoalDetail_BulkUpsert",
                ProductionGoal_Upsert: "/Production/Goals/ProductionGoal_Upsert",
                ProductionGoal_Edit: "/Production/Goals/ProductionGoal_Edit"
            },
            Moldscrap: {
                GetAccessList: "/Production/MoldScrap/UsersProcessesLines_GetAccessList"

            }
        }
    }
};

function IndexInit(LangResources) {

    $(".select").selectpicker({
        liveSearch: false
    });

    $('.applyfilters').on('change', function (e) {
        ForceApplyFilters();
        FillProductionGoalMasterTable();
    });

    $('#ddl_ProductionProcess').on('change', function (e) {
        ForceApplyFilters();
        var ProductionProcessValue = $(this).val();
        var controls = new IndexGetControls();

        controls.ddl_ProductionLine.selectpicker('val', '[0]');
        controls.ddl_ProductionLine.selectpicker('refresh');
        controls.div_ContainerControlProductionLine.addClass("hidden");

        if (ProductionProcessValue != '') {
            /* Web Method - Read Setting Apply Lines?  */
            var ProductionProcessParams = {
                CatalogDetailID: parseInt(ProductionProcessValue)
            };

            var ProductionProcessRequest = new CallWebMethod(app.Area.Production.Goals.ProductionProcessApplyLines, ProductionProcessParams);
            ProductionProcessRequest.success(function (response) {

                if (response.ApplyLines) {
                    controls.div_ContainerControlProductionLine.removeClass("hidden");
                } // Fin ApplyLines

                FillProductionGoalMasterTable();

            });
        } else {
            FillProductionGoalMasterTable();
        }
    });

    $('#btn_AddNewProductionGoal').on('click', function (e) {
        $.get(app.Area.Production.Goals.LoadNewGoalInfo).done(function (data) {
         
            var controls = new IndexGetControls();
            controls.div_MPE.removeClass("hidden");
            controls.div_MPE.html('');
            controls.div_MPE.html(data);

            RegisterEventSaveProductionGoal();
            RegisterPluginSelect();
            RegisterEventProductionProcess();
            ClearControlsAddProductionGoal();
            $("#mo_AddProductionGoal").modal('show');
        });

    });

    SetupOnlyDecimal();

    RegisterDeleteProductionGoalMasterTable();

    RegisterEventProductionGoalsDetails();

    RegisterEventSaveProductionGoalDetails();

    RegisterEditProductionGoalMasterTable();

}

//private methods
function FillProductionGoalMasterTable() {
    try {
        var controls = new IndexGetControls();
        var ProductionProcessID = 0;
        var ProductionLineID = 0;
        var VAID = 0;
        var DesignID = 0;
        var ShiftID = 0;
        var CatalogGoalID;

        if (controls.ddl_ProductionProcess.val() != '') {
            ProductionProcessID = parseInt(controls.ddl_ProductionProcess.val());
        }

        if (controls.ddl_ProductionLine.val() != '' && controls.ddl_ProductionLine.val() != null) {
            ProductionLineID = parseInt(controls.ddl_ProductionLine.val());
        }

        if (controls.ddl_VA.val() != '') {
            VAID = parseInt(controls.ddl_VA.val());
        }

        if (controls.ddl_Design.val() != '') {
            DesignID = parseInt(controls.ddl_Design.val());
        }
        if (controls.ddl_Shift.val() != '') {
            ShiftID = parseInt(controls.ddl_Shift.val());
        }
        if (controls.ddl_CatalogGoal.val() != '') {
            CatalogGoalID = parseInt(controls.ddl_CatalogGoal.val());
        }

        var ProductionGoalParams = {
            ProductionProcessID: ProductionProcessID,
            ProductionLineID: ProductionLineID,
            VAID: VAID,
            DesignID: DesignID,
            ShiftID: ShiftID,
            CatalogGoalID: CatalogGoalID
        };

        $.get(app.Area.Production.Goals.ProductionGoals_List, ProductionGoalParams).done(function (data) {
            controls.div_boxProductionGoalInfo.show();
            controls.div_boxProductionGoalInfo.html('');
            controls.div_boxProductionGoalInfo.html(data);


        });
    } catch (error) {
        console.log(error.message);
    }
}

// master table events
function RegisterEditProductionGoalMasterTable() {
    $(document).on('click', '.edit-production-goal', function (e) {
        e.stopImmediatePropagation();

        var ID = this.attributes["data-production-goalid"].value;
        var ProductionGoalEditParams = {
            GoalID: parseInt(ID)
        };

        $.get(app.Area.Production.Goals.ProductionGoal_Edit, ProductionGoalEditParams).done(function (data) {
            ClearControlsAddProductionGoal();
            var controls = new IndexGetControls();
            controls.div_MPE.removeClass("hidden");
            controls.div_MPE.html('');
            controls.div_MPE.html(data);

            RegisterEventSaveProductionGoal();
            RegisterPluginSelect();
            RegisterEventProductionProcess();
           
            $("#mo_AddProductionGoal").modal('show');
        });

    });
}

// master table events
function RegisterDeleteProductionGoalMasterTable() {
    $(document).on('click', '.delete-production-goal', function (e) {
        e.stopImmediatePropagation();

        var ID = this.attributes["data-production-goalid"].value;

        /* Control generico para mostrar mensajes */
        var confirmBox = new ConfirmBox();
        confirmBox.yesTag = $('#lbl_DeleteButtonTag').text();
        confirmBox.noTag = $('#lbl_CancelButtonTag').text();
        confirmBox.title = $('#lbl_TitleDeleteDefectDetailTag').text();
        confirmBox.msg = $('#lbl_MsgDeleteGoalTag').text();
        confirmBox.showMsg('warning');

        /* funcion OnClick de  */
        confirmBox.onAccept = function () {

            var ProductionGoalDeleteParams = {
                GoalID: parseInt(ID)
            };

            var DeleteDefectDetailsRequest = CallWebMethod(app.Area.Production.Goals.ProductionGoal_Delete, ProductionGoalDeleteParams);

            DeleteDefectDetailsRequest.success(function (data) {
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    FillProductionGoalMasterTable();
                } else {
                    notification("", data.ErrorMessage, "error");
                }
            });
        };

        
    });
}

function RegisterEventProductionGoalsDetails() {
    var table = $('#tbl_productiondetails');

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

            var controls = new IndexGetControls();
            var entity = new ProductionGoalEntity();


            if (controls.ddl_ProductionProcess.val() != '' && controls.ddl_ProductionProcess.val() != null) {
                entity.ProductionProcessID = parseInt(controls.ddl_ProductionProcess.val());
            }

            if (controls.ddl_ProductionLine.val() != '' && controls.ddl_ProductionLine.val() != null) {
                entity.ProductionLineID = parseInt(controls.ddl_ProductionLine.val());
            }

            if (controls.ddl_VA.val() != '' && controls.ddl_VA.val() != null) {
                entity.VAID = parseInt(controls.ddl_VA.val());
            }

            if (controls.ddl_Design.val() != '' && controls.ddl_Design.val() != null) {
                entity.DesignID = parseInt(controls.ddl_Design.val());
            }

            if (controls.ddl_Shift.val() != '' && controls.ddl_Shift.val() != null) {
                entity.ShiftID = parseInt(controls.ddl_Shift.val());
            }

            entity.GoalID = tr.data("entityid");
            // abrir detalles y devolver detalles de una llamada ajax
            $.get(app.Area.Production.Goals.ProductionGoalDetail_List, entity
            ).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan = "9" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
            });
        }
    });
}
// modal events
function ClearControlsAddProductionGoal() {
    var controls = new CreateNewProductionGoalControls();
    controls.ddl_AD_GoalName.selectpicker('val', '[0]');
    controls.ddl_ProductionProcess.selectpicker('val', '[0]');
    controls.ddl_ProductionLine.selectpicker('val', '[0]');
    controls.ddl_AD_Shift.selectpicker('val', '[0]');
    //controls.div_ContainerControlProductionLine.addClass('hidden');
    controls.ddl_VA.selectpicker('val', '[0]');
    controls.ddl_Design.selectpicker('val', '[0]');
    controls.txt_AD_Goal.val('0');
}

function RegisterEventSaveProductionGoal() {
    var controls = new CreateNewProductionGoalControls();
    controls.btn_SaveProductionGoal.on('click', function (e) {
        var entity = new ProductionGoalEntity();

        entity.GoalID = $(this).data('goalid');
        if (controls.ddl_AD_GoalName.val() != '' && controls.ddl_AD_GoalName.val() != null) {
            entity.GoalNameID = parseInt(controls.ddl_AD_GoalName.val());
        }

        if (controls.ddl_ProductionProcess.val() != '' && controls.ddl_ProductionProcess.val() != null) {
            entity.ProductionProcessID = parseInt(controls.ddl_ProductionProcess.val());
        }

        if (controls.ddl_ProductionLine.val() != '' && controls.ddl_ProductionLine.val() != null) {
            entity.ProductionLineID = parseInt(controls.ddl_ProductionLine.val());
        }

        if (controls.ddl_VA.val() != '' && controls.ddl_VA.val() != null) {
            entity.VAID = parseInt(controls.ddl_VA.val());
        }

        if (controls.ddl_Design.val() != '' && controls.ddl_Design.val() != null) {
            entity.DesignID = parseInt(controls.ddl_Design.val());
        }

        if (controls.ddl_AD_Shift.val() != '' && controls.ddl_AD_Shift.val() != null) {
            entity.ShiftID = parseInt(controls.ddl_AD_Shift.val());
        }

        if (controls.txt_AD_Goal.val() != '') {
            entity.GoalValue = parseFloat(controls.txt_AD_Goal.val());
        }

        var ProductionGoal_NewItemRequest = CallWebMethod(app.Area.Production.Goals.ProductionGoal_Upsert, entity);
        ProductionGoal_NewItemRequest.success(function (data) {
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, "success");
                $("#mo_AddProductionGoal").modal('hide');
                FillProductionGoalMasterTable();
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });
    });
}

function RegisterPluginSelect() {
    $(".select").selectpicker();
}

function RegisterEventSaveProductionGoalDetails() {
    $(document).on('click', '.save-production-details', function (e) {

        e.stopImmediatePropagation();

        var ID = this.attributes["data-production-goalid"].value;

        var list = [];
        $("#tbl_ProductionGoalDetails_" + ID).find(" tr:not(:first)").each(function (index, item) {
            var td = $(this).find("td");
            var inputcontrol = $(td[1]).find("input")[0];

            var entity = {
                RowNumber: index,
                GoalValue: inputcontrol.value,
                GoalDetailID: $(td[1]).find("input").data("entityid"),
                HourValue: $(td[1]).find("input").data("hour"),
            };
            list.push(entity);
        });

        if (list.length > 0) {


            var BulkUpsertParams = {
                list: list,
                GoalID: ID
            };

            var BulkUpsertRequest = new CallWebMethod(app.Area.Production.Goals.ProductionGoalDetail_BulkUpsert, BulkUpsertParams);
            BulkUpsertRequest.success(function (data) {
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    FillProductionGoalMasterTable();
                } else {
                    notification("", data.ErrorMessage, "error");
                }
            });
        }
    });
}

function RegisterEventProductionProcess() {
    var controls = new CreateNewProductionGoalControls();
    controls.ddl_ProductionProcess.on('change', function (e) {
        var ProductionProcessValue = $(this).val();
        controls.ddl_ProductionLine.selectpicker('val', '[0]');
        controls.ddl_ProductionLine.selectpicker('refresh');
        if (ProductionProcessValue != null) {
            controls.div_ContainerControlProductionLine.addClass("hidden");

            if (ProductionProcessValue !== '') {
                /* Web Method - Read Setting Apply Lines?  */
                var ProductionProcessParams = {
                    CatalogDetailID: parseInt(ProductionProcessValue)
                };

                var ProductionProcessRequest = new CallWebMethod(app.Area.Production.Goals.ProductionProcessApplyLines, ProductionProcessParams);
                ProductionProcessRequest.success(function (response) {
                    if (response.ApplyLines) {
                        controls.div_ContainerControlProductionLine.removeClass("hidden");
                    } // Fin ApplyLines
                });
            }
        }
    });

}

//common methods
function ForceApplyFilters() {
    //$(".filters").hide();
}

function CallWebMethod(url, parameters) {

    return $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(parameters),
        contentType: 'application/json;',
        dataType: 'json'
    });
}

//Entities
function IndexGetControls() {
    this.div_boxProductionGoalInfo = $('#div_boxProductionGoalInfo');
    this.ddl_ProductionProcess = $('#ddl_ProductionProcess');
    this.div_ContainerControlProductionLine = $('#div_ContainerControlProductionLine');
    this.ddl_ProductionLine = $('#ddl_ProductionLine');
    this.ddl_VA = $("#ddl_VA");
    this.ddl_Design = $("#ddl_Design");
    this.ddl_Shift = $("#ddl_Shift");
    this.div_MPE = $('#div_MPE');
    this.ddl_CatalogGoal = $('#ddl_CatalogGoal');
}

function CreateNewProductionGoalControls() {
    this.ddl_AD_GoalName = $('#ddl_AD_GoalName');
    this.ddl_ProductionProcess = $('#ddl_AD_ProductionProcess');
    this.div_ContainerControlProductionLine = $('#div_AD_ContainerControlProductionLine');
    this.ddl_ProductionLine = $('#ddl_AD_ProductionLine');
    this.ddl_VA = $("#ddl_AD_VA");
    this.ddl_Design = $("#ddl_AD_Design");
    this.ddl_AD_Shift = $('#ddl_AD_Shift');
    this.div_MPE = $('#div_MPE');
    this.txt_AD_Goal = $('#txt_AD_Goal');
    this.btn_SaveProductionGoal = $('#btn_SaveProductionGoal');
}

function ProductionGoalEntity() {
    this.GoalID = 0;
    this.GoalNameID = 0;
    this.ProductionProcessID = 0;
    this.ProductionLineID = 0;
    this.VAID = 0;
    this.DesignID = 0;
    this.ShiftID = 0;
    this.GoalValue = 0;
}