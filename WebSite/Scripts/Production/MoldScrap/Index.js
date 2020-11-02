// =============================================================================================================================
//  Version: 20190305
//  Author:  Felipe Vera
//  Created Date: 05 March 2019
//  Description:  Mold Scrap JS
//  Modifications: 
// =============================================================================================================================

/* Config app */
var app = {
    Area: {
        Production: {
            Moldscrap: {
                ProductionProcessApplyLines: "/Production/MoldScrap/ProductionProcessApplyLines",
                GetAccessList: "/Production/MoldScrap/UsersProcessesLines_GetAccessList",
                MoldScrapList: "/Production/MoldScrap/MoldScrap_GetShiftList",
                MoldScrap_BulkUpsert: "/Production/MoldScrap/MoldScrap_BulkUpsert"
            }
        }
    }
};

function IndexInit(LangResources) {

    $('.datepicker').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd'
        //language: LangResources.datepicker_lang
    });

    $(".select").selectpicker({
        liveSearch: false
    });

    $('#ddl_ProductionProcess').on('change', function (e) {
        ForceApplyFilters();
        var ProductionProcessValue = $(this).val();
        var controls = new IndexGetControls();

        controls.ddl_ProductionLine.empty();
        controls.div_ContainerControlProductionLine.addClass("hidden");

        if (ProductionProcessValue !== '') {
            /* Web Method - Read Setting Apply Lines?  */
            var ProductionProcessParams = {
                CatalogDetailID: parseInt(ProductionProcessValue)
                , ParamIndex: 1
            };

            var ProductionProcessRequest = new CallWebMethod(app.Area.Production.Moldscrap.ProductionProcessApplyLines, ProductionProcessParams);
            ProductionProcessRequest.success(function (response) {

                if (response.ApplyLines) {
                    var ProductionAccessLineParams = {
                        CatalogDetailID: parseInt(ProductionProcessValue)
                    };
                    /* Web Method - List Lines  */
                    var ProductionAccessLineRequest = new CallWebMethod(app.Area.Production.Moldscrap.GetAccessList, ProductionAccessLineParams);
                    ProductionAccessLineRequest.success(function (response) {
                        controls.div_ContainerControlProductionLine.removeClass("hidden");
                        if (response.length > 0) {
                            $.each(response, function () {
                                controls.ddl_ProductionLine.append($("<option></option>").val(this['ProductionLineID']).html(this['ProductionLineName']));
                            });
                        }

                        controls.ddl_ProductionLine.selectpicker("refresh");

                        FillMoldScrapTable();
                    });
                } // Fin ApplyLines
                else {
                    FillMoldScrapTable();
                }
            });
        } else {
            FillMoldScrapTable();
        }
    });

    $('#txt_ScrapDate').on('change', function (e) {
        ForceApplyFilters();
        FillMoldScrapTable();
    });

    $('#ddl_ProductionLine, #ddl_Shift, #ddl_Design').on('change', function (e) {
        ForceApplyFilters();
        FillMoldScrapTable();
    });

    $('#btn_SaveMoldScrapAll').on('click', function (e) {

        var list = [];
        $("#tbl_moldscrap").find(" tr:not(:first)").each(function (index, item) {
            var td = $(this).find("td");
            var inputcontrol = $(td[2]).find("input")[0];

            var entity = {
                RowNumber: index,
                Quantity: inputcontrol.value,
                ScrapID: $(td[2]).find("input").data("entityid"),
                HourValue: $(td[2]).find("input").data("hour"),
            };
            list.push(entity);
        });

        if (list.length > 0) {
            var controls = new IndexGetControls();
            var ProductionProcessID = 0;
            var ProductionLineID = 0;
            var ShiftID = 0;
            var DesignID = 0;

            if (controls.ddl_ProductionProcess.val() !== '') {
                ProductionProcessID = parseInt(controls.ddl_ProductionProcess.val());
            }

            if (controls.ddl_ProductionLine.val() !== '' && controls.ddl_ProductionLine.val() != null) {
                ProductionLineID = parseInt(controls.ddl_ProductionLine.val());
            }

            if (controls.ddl_Shift.val() !== '' && controls.ddl_Shift.val() != null) {
                ShiftID = parseInt(controls.ddl_Shift.val());
            }

            if (controls.ddl_Design.val() !== '' && controls.ddl_Design.val() != null) {
                DesignID = parseInt(controls.ddl_Design.val());
            }

            var BulkUpsertParams = {
                list: list,
                ScrapDate: controls.txt_ScrapDate.val(),
                ProductionProcessID: ProductionProcessID,
                ProductionLineID: ProductionLineID,
                ShiftID: ShiftID,
                DesignID
            };

            var BulkUpsertRequest = new CallWebMethod(app.Area.Production.Moldscrap.MoldScrap_BulkUpsert, BulkUpsertParams);
            BulkUpsertRequest.success(function (data) {
                if (data.ErrorCode === 0) {
                    notification("", data.ErrorMessage, "success");
                    FillMoldScrapTable();
                } else {
                    notification("", data.ErrorMessage, "error");
                }
            });
        }
    });

    SetupOnlyDecimal();
};

function ForceApplyFilters() {
    $(".filters").hide();
}

function FillMoldScrapTable() {
    $('.loading-process-div').show();

    try {
        var controls = new IndexGetControls();
        var ProductionProcessID = 0;
        var ProductionLineID = 0;
        var ShiftID = 0;
        var DesignID = 0;

        if (controls.ddl_ProductionProcess.val() !== '') {
            ProductionProcessID = parseInt(controls.ddl_ProductionProcess.val());
        }

        if (controls.ddl_ProductionLine.val() !== '' && controls.ddl_ProductionLine.val() != null) {
            ProductionLineID = parseInt(controls.ddl_ProductionLine.val());
        }
        if (controls.ddl_Shift.val() !== '' && controls.ddl_Shift.val() != null) {
            ShiftID = parseInt(controls.ddl_Shift.val());
        }
        if (controls.ddl_Design.val() !== '' && controls.ddl_Design.val() != null) {
            DesignID = parseInt(controls.ddl_Design.val());
        }
        var MoldScrapParams = {
            ScrapDate: controls.txt_ScrapDate.val(),
            ProductionProcessID: ProductionProcessID,
            ProductionLineID: ProductionLineID,
            ShiftID: ShiftID,
            DesignID
        };

        $.get(app.Area.Production.Moldscrap.MoldScrapList, MoldScrapParams).done(function (data) {
            controls.div_boxMoldScrapDetail.show();
            controls.div_boxMoldScrapDetail.html('');
            controls.div_boxMoldScrapDetail.html(data);
        });
    } catch (error) {
        console.log(error.message);
    } finally {
        $('.loading-process-div').hide();
    }
}

function IndexGetControls() {
    this.ddl_ProductionProcess = $('#ddl_ProductionProcess');
    this.div_ContainerControlProductionLine = $('#div_ContainerControlProductionLine');
    this.ddl_ProductionLine = $('#ddl_ProductionLine');
    this.txt_ScrapDate = $('#txt_ScrapDate');
    this.div_boxMoldScrapDetail = $('#div_boxMoldScrapDetail');
    this.btn_SaveMoldScrapAll = $('#btn_SaveMoldScrapAll');
    this.ddl_Shift = $('#ddl_Shift');
    this.ddl_Design = $('#ddl_Design');
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

