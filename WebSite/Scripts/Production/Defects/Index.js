// =============================================================================================================================
//  Version: 20190305
//  Author:  Felipe Vera
//  Created Date: 06 March 2019
//  Description:  Defects JS
//  Modifications: 
// =============================================================================================================================

/* Config app */
var app = {
    Area: {
        Production: {
            Defects: {
                ProductionProcessApplyLines: "/Production/Defects/ProductionProcessApplyLines",
                DefectInfo_List: "/Production/Defects/DefectInfo_List",
                GetAccessList: "/Production/MoldScrap/UsersProcessesLines_GetAccessList",
                Defect_AddDetails: "/Production/Defects/Defect_AddDetails",
                DefectProcess_Add: "/Production/Defects/DefectProcess_Add",
                DefectProcessInfo_List: "/Production/Defects/DefectProcessInfo_List",
                DefectProcess_Delete: "/Production/Defects/DefectProcess_Delete",
                Defect_EditDetails: "/Production/Defects/Defect_EditDetails",
                DefectProcess_DeleteAllDetails: "/Production/Defects/DefectProcess_DeleteAllDetails"
            }
        }
    }
};

function IndexInit(LangResources) {

    $(".select").selectpicker({
        liveSearch: false
    });

    $(document).on('click', '.add-defect-details', function (e) {
        e.stopImmediatePropagation();

        var defectID = this.attributes["data-defectid"].value;
        var defectName = this.attributes["data-defect-name"].value;

        var entity = new DefectProcessEntity();
        entity.DefectID = defectID;
        entity.DefectProcessID = 0;

        $.get(app.Area.Production.Defects.Defect_AddDetails, entity).done(function (data) {
            var controls = new IndexGetControls();
            controls.div_MPE.removeClass("hidden");
            controls.div_MPE.html('');
            controls.div_MPE.html(data);

            RegisterEventProductionProcess();
            RegisterPluginMiniColor();
            RegisterPluginSelect();
            RegisterEventAddDefectProcess();
            ClearControlsAddDefectDetail();
            $('#ddl_AD_DefectName').val(defectID);
            $('#ddl_AD_DefectName').selectpicker('refresh');
            $("#mo_AddDefectDetail").modal('show');
        });
    });

    RegisterEventDefectProcessDetail();
    RegisterEventEditDefectProcess();

    $('#ddl_ProductionProcess').on('change', function (e) {
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
                FillDefectInfoTable();
            });
        } else {
            FillDefectInfoTable();
            HideProgressBar();
        }
    });

    $('.applyfilters').on('change', function (e) {
        ForceApplyFilters();
        FillDefectInfoTable();
    });

    SetupOnlyDecimal();

    RegisterEventDeleteAllDetails();

}

function RegisterEventProductionProcess() {
    var controls = new CreateDefectDetailsControls();
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
                    , ParamIndex: 1
                };

                var ProductionProcessRequest = new CallWebMethod(app.Area.Production.Defects.ProductionProcessApplyLines, ProductionProcessParams);
                ProductionProcessRequest.success(function (response) {
                    ShowProgressBar();
                    if (response.ApplyLines) {
                        controls.div_ContainerControlProductionLine.removeClass("hidden");
                    } // Fin ApplyLines
                });
            } else {
                HideProgressBar();
            }
        }
    });
}

function RegisterPluginSelect() {
    $(".select").selectpicker();
}

function RegisterPluginMiniColor() {
    $('.mini-color').each(function () {
        $(this).minicolors({
            control: $(this).attr('data-control') || 'hue',
            defaultValue: $(this).attr('data-defaultValue') || '',
            format: $(this).attr('data-format') || 'hex',
            keywords: $(this).attr('data-keywords') || '',
            inline: $(this).attr('data-inline') === 'true',
            letterCase: $(this).attr('data-letterCase') || 'lowercase',
            opacity: $(this).attr('data-opacity'),
            position: $(this).attr('data-position') || 'bottom',
            swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
            theme: 'bootstrap'
        });
    });
}


function RegisterEventAddDefectProcess() {
    var controls = new CreateDefectDetailsControls();
    controls.btn_AddDefectDetail.on('click', function (e) {
        ShowProgressBar();
        var defect = new DefectProcessEntity();

        var defectprocessid = $(this).data('defect-processid');
        defect.DefectProcessID = defectprocessid;
        if (controls.ddl_ProductionProcess.val() != '' && controls.ddl_ProductionProcess.val() != null) {
            defect.ProductionProcessID = parseInt(controls.ddl_ProductionProcess.val());
        }

        if (controls.ddl_ProductionLine.val() != '' && controls.ddl_ProductionLine.val() != null) {
            defect.ProductionLineID = parseInt(controls.ddl_ProductionLine.val());
        }

        if (controls.ddl_VA.val() != '' && controls.ddl_VA.val() != null) {
            defect.VAID = parseInt(controls.ddl_VA.val());
        }

        if (controls.ddl_Design.val() != '' && controls.ddl_Design.val() != null) {
            defect.DesignID = parseInt(controls.ddl_Design.val());
        }

        if (controls.txt_AD_DefectColor.val() != '') {
            defect.Color = controls.txt_AD_DefectColor.val();
        }

        if (controls.txt_AD_DefectFontColor.val() != '') {
            defect.FontColor = controls.txt_AD_DefectFontColor.val();
        }

        if (controls.txt_AD_Goal.val() != '') {
            defect.GoalValue = parseFloat(controls.txt_AD_Goal.val());
        }

        if (controls.ddl_AD_DefectName.val() != '' && controls.ddl_AD_DefectName.val() != null) {
            defect.DefectID = parseInt(controls.ddl_AD_DefectName.val());
        }


        if (controls.ddl_Shift.val() != '' && controls.ddl_Shift.val() != null) {
            defect.ShiftID = parseInt(controls.ddl_Shift.val());
        }
        var DefectAddDetailRequest = CallWebMethod(app.Area.Production.Defects.DefectProcess_Add, defect);
        DefectAddDetailRequest.success(function (data) {
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, "success");
                $("#mo_AddDefectDetail").modal('hide');
                FillDefectInfoTable();
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });
    });
}

function DefectProcessEntity() {
    this.DefectProcessID = 0;
    this.ProductionProcessID = 0;
    this.ProductionLineID = 0;
    this.VAID = 0;
    this.DesignID = 0;
    this.DefectID = 0;
    this.Color = '';
    this.FontColor = '';
    this.GoalValue = 0;
    this.ShiftID = 0;
}

function ForceApplyFilters() {
    $(".filters").hide();
}

function IndexGetControls() {
    this.div_boxDefectInfo = $('#div_boxDefectInfo');
    this.ddl_ProductionProcess = $('#ddl_ProductionProcess');
    this.div_ContainerControlProductionLine = $('#div_ContainerControlProductionLine');
    this.ddl_ProductionLine = $('#ddl_ProductionLine');
    this.ddl_VA = $("#ddl_VA");
    this.ddl_Design = $("#ddl_Design");
    this.div_MPE = $('#div_MPE');
    this.ddl_Shift = $('#ddl_Shift');
}

/*Establece los mismos nombres -> alias para ejecutar metodo en comun para cargar las lineas */
function CreateDefectDetailsControls() {
    this.ddl_AD_DefectName = $('#ddl_AD_DefectName');
    this.ddl_ProductionProcess = $('#ddl_AD_ProductionProcess');
    this.div_ContainerControlProductionLine = $('#div_AD_ContainerControlProductionLine');
    this.ddl_ProductionLine = $('#ddl_AD_ProductionLine');
    this.ddl_VA = $("#ddl_AD_VA");
    this.ddl_Design = $("#ddl_AD_Design");
    this.div_MPE = $('#div_MPE');
    this.txt_AD_DefectColor = $('#txt_AD_DefectColor');
    this.txt_AD_DefectFontColor = $('#txt_AD_DefectFontColor');
    this.txt_AD_Goal = $('#txt_AD_Goal');
    this.btn_AddDefectDetail = $('#btn_AddDefectDetail');
    this.ddl_Shift = $('#ddl_AD_Shift');
}

function ClearControlsAddDefectDetail() {
    var controls = new CreateDefectDetailsControls();
    controls.ddl_ProductionProcess.selectpicker('val', '[0]');
    controls.ddl_ProductionLine.selectpicker('val', '[0]');
    controls.ddl_VA.selectpicker('val', '[0]');
    controls.ddl_Design.selectpicker('val', '[0]');
    controls.txt_AD_DefectColor.val('#000000');
    controls.txt_AD_DefectFontColor.val('#000000');
    controls.txt_AD_Goal.val('0');
}

function CallWebMethod(url, parameters) {

    return $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(parameters),
        contentType: 'application/json;',
        dataType: 'json'
    }).always(function () {
        HideProgressBar();
    });
}

function FillDefectInfoTable() {
    try {
        var controls = new IndexGetControls();
        var ProductionProcessID = 0;
        var ProductionLineID = 0;
        var VAID = 0;
        var DesignID = 0;
        var ShiftID = 0;

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

        if (controls.ddl_Shift.val() != '' && controls.ddl_Shift.val() != null) {
            ShiftID = parseInt(controls.ddl_Shift.val());
        }
        var DefectInfoParams = {
            ProductionProcessID: ProductionProcessID,
            ProductionLineID: ProductionLineID,
            VAID: VAID,
            DesignID: DesignID,
            ShiftID: ShiftID
        };

        $.get(app.Area.Production.Defects.DefectInfo_List, DefectInfoParams).done(function (data) {
            controls.div_boxDefectInfo.show();
            controls.div_boxDefectInfo.html('');
            controls.div_boxDefectInfo.html(data);
            HideProgressBar();
        });
    } catch (error) {
        console.log(error.message);
    }
}

function RegisterEventDefectProcessDetail() {
    var table = $('#tbl_defect');

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
            var defect = new DefectProcessEntity();

            if (controls.ddl_ProductionProcess.val() != '' && controls.ddl_ProductionProcess.val() != null) {
                defect.ProductionProcessID = parseInt(controls.ddl_ProductionProcess.val());
            }

            if (controls.ddl_ProductionLine.val() != '' && controls.ddl_ProductionLine.val() != null) {
                defect.ProductionLineID = parseInt(controls.ddl_ProductionLine.val());
            }

            if (controls.ddl_VA.val() != '' && controls.ddl_VA.val() != null) {
                defect.VAID = parseInt(controls.ddl_VA.val());
            }

            if (controls.ddl_Design.val() != '' && controls.ddl_Design.val() != null) {
                defect.DesignID = parseInt(controls.ddl_Design.val());
            }

            if (controls.ddl_Shift.val() != '' && controls.ddl_Shift.val() != null) {
                defect.ShiftID = parseInt(controls.ddl_Shift.val());
            }
            defect.DefectID = tr.data("entityid");
            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get(app.Area.Production.Defects.DefectProcessInfo_List, defect
            ).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan = "3" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                RegisterEventDeleteDefectProcess();
                HideProgressBar();
            });
        }
    });
}

function RegisterEventDeleteDefectProcess() {
    $(document).on('click', '.delete-defect-details', function (e) {
        e.stopImmediatePropagation();

        var defectProcessID = this.attributes["data-defect-processid"].value;

        /* Control generico para mostrar mensajes */
        var confirmBox = new ConfirmBox();
        confirmBox.yesTag = $('#lbl_DeleteButtonTag').text();
        confirmBox.noTag = $('#lbl_CancelButtonTag').text();
        confirmBox.title = $('#lbl_TitleDeleteDefectDetailTag').text();
        confirmBox.msg = $('#lbl_MsgDeleteDefectDetailTag').text();
        confirmBox.showMsg('warning');

        /* funcion OnClick de  */
        confirmBox.onAccept = function () {
            ShowProgressBar();
            var DeleteDefectDetailsParams = {
                DefectProcessID: parseInt(defectProcessID)
            };

            var DeleteDefectDetailsRequest = CallWebMethod(app.Area.Production.Defects.DefectProcess_Delete, DeleteDefectDetailsParams);

            DeleteDefectDetailsRequest.success(function (data) {
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    FillDefectInfoTable();
                } else {
                    notification("", data.ErrorMessage, "error");
                }
            });

        };

    });
};

function RegisterEventEditDefectProcess() {
    $(document).on('click', '.edit-defect-details', function (e) {
        e.stopImmediatePropagation();

        var defectProcessID = this.attributes["data-defect-processid"].value;
        var defectID = this.attributes["data-defectid"].value;

        var entity = new DefectProcessEntity();
        entity.DefectID = defectID;
        entity.DefectProcessID = defectProcessID;
        ShowProgressBar();

        $.get(app.Area.Production.Defects.Defect_EditDetails, entity).done(function (data) {
            var controls = new IndexGetControls();
            controls.div_MPE.removeClass("hidden");
            controls.div_MPE.html('');
            controls.div_MPE.html(data);

            RegisterEventProductionProcess();
            RegisterPluginMiniColor();
            RegisterPluginSelect();
            RegisterEventAddDefectProcess();
            HideProgressBar();
            //$('#ddl_AD_DefectName').val(defectID);
            //$('#ddl_AD_DefectName').selectpicker('refresh');
            $("#mo_AddDefectDetail").modal('show');
        });
    });
}

function RegisterEventDeleteAllDetails() {
    $(document).on('click', '.delete-defect-all-details', function (e) {
        e.stopImmediatePropagation();

        var defectID = this.attributes["data-defectid"].value;

        /* Control generico para mostrar mensajes */
        var confirmBox2 = new ConfirmBox();
        confirmBox2.yesTag = $('#lbl_DeleteButtonTag').text();
        confirmBox2.noTag = $('#lbl_CancelButtonTag').text();
        confirmBox2.title = $('#lbl_TitleDeleteDefectDetailTag').text();
        confirmBox2.msg = $('#lbl_MsgDeleteDefectTag').text();
        confirmBox2.showMsg('warning');

        /* funcion OnClick de  */
        confirmBox2.onAccept = function () {

            var DeleteDefectAllDetailsParams = {
                DefectID: parseInt(defectID)
            };
            ShowProgressBar();
            var DeleteDefectAllDetailsRequest = CallWebMethod(app.Area.Production.Defects.DefectProcess_DeleteAllDetails, DeleteDefectAllDetailsParams);

            DeleteDefectAllDetailsRequest.success(function (data) {
                if (data.ErrorCode === 0) {
                    notification("", data.ErrorMessage, "success");
                    FillDefectInfoTable();
                } else {
                    notification("", data.ErrorMessage, "error");
                }
            });
        };

    });
}
