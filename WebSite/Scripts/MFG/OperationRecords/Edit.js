// ==========================================================================================================================================================
//  Version: 20190423
//  Author:  cgarcia
//  Created Date: 23 de abril de 2019
//  Description: Contiene funciones JS para la página de Edit
//  Modifications: 
// ==========================================================================================================================================================

function EditInit(LangResources) {
    SetUpSelectPicker();
    $('.datepicker').datepicker({
        autoclose: true,
        format: 'HH:mm'
        //language: LangResources.datepicker_lang
    });

    $('#enter').on('click', function () {
        var EmployeNumber = $('#EmployeNumber').val();
        $('.loading-process-div').show();
        $.post("/MFG/Login/Index", { EmployeNumber }).done(function (data) {
            if (data.ErrorCode !== 0) {
                $('.loading-process-div').hide();
                notification("", data.ErrorMessage, "error");
            } else {
                localStorage.setItem('MFG_OperationRecords_EmployeeNumber', EmployeNumber);
                //window.location = "/MFG/OperationRecords/List";
                window.location = "/MFG/OperationRecords/New";
            }
        });
    });

    $(document).on('click', '.done', function (e) {
        e.stopPropagation();
        $('#enter').click();
    });

    $('#btn_exit').on('click', function () {
        window.close();
    });

    SetupOnlyDecimal();
    SetupOnlyNumbers();
    $('.max-length').maxlength();
    SetupNumpad();
    SetupProdNumpad();

    var EmployeeNumber = localStorage.getItem('MFG_OperationRecords_EmployeeNumber');
    if (EmployeeNumber !== null || EmployeeNumber.length !== 0) {
        $('#OperatorNumber').text(EmployeeNumber);
    }

    //setInterval(CheckEveryHour, 1000 * 60 * 15); //revisar cada 5 min
    //IsCaptured();


    $('.cancel-btn ,#btn_backlist').on('click', function () {
        $('.loading-process-div').show();
        window.location = "/MFG/OperationRecords/List";
        $('.loading-process-div').hide();
    });
    $('#save_setup').on('click', function () {
        $('.loading-process-div').show();
        var itemlist = [];
        $('.setup-param-input').each(function () {
            var setupparamid = $(this).data('setupparamid');
            var typevalue = $(this).data('typevalue');
            var valor = null;
            var valuelist = null;
            if (typevalue === "LIST") {
                //valuelist = $(this).find("input[class='param-val']:checked").val();
                //busca de los botones como lista el que esta seleccionado
                valuelist = $(this).find('.active').find('input[type="radio"]').val();
            } else {
                valor = $(this).find(".param-val").val();
            }
            var ref = $(this).find(".param-ref").val();
            var mandatory = $(this).data('ismandatory');
            var machineparameterid = $(this).data('machineparameterid');
            var Item = {
                OperationSetupParameterID: setupparamid,
                Value: valor,
                ValueList: valuelist,
                Reference: ref,
                IsMandatory: mandatory,
                MachineParameterID: machineparameterid
            }
            itemlist.push(Item);
        });

        //console.log("setupparamid: " + setupparamid + " val: " + valor + " ref: "+ref);

        $.post("/MFG/OperationRecords/SaveSetup",
            { OperationSetupID: $('#OperationSetupID').val(), ListParameters: itemlist }
        ).done(function (data) {
            //notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                //si todo esta bien
            } else {

                $("#message-box-generic-alert-title").text(LangResources.lbl_Warning);
                $("#message-box-generic-alert-legend").text(data.ErrorMessage);
                $('#message-box-generic-alert').toggleClass("open");
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });

    });

    //production
    $('#log_production').on('click', function () {
        //var $initialshot = $('#initialshot');
        //$initialshot.val() !== "" && $initialshot.val() !== "0"
        //llamada syncrona
        ValidateInitialShot(function (result) {
            if (result.ErrorCode === 0) {
                if (result.entity.InitialShotNumber !== 0) {
                    var Hour = new Date().getHours();
                    $('#Hour').val(Hour);
                    UpdateRejectsTableHour();
                    $('#OperationProductionDetailID').val('');
                    $('#ShotNumber').val('0');
                    $('#productionrejectqty').val('0');
                    $('#newproductiondetail_title').text(LangResources.lbl_Newproduction);
                    ShowHideProduction();
                } else {
                    notification("", LangResources.ntf_NoInitialshot, "error");
                }
            } else {
                notification("", result.ErrorMessage, "error");
            }
        });



    });
    $('#save_production').on('click', function () {
        $('.loading-process-div').show();
        $.post("/MFG/OperationRecords/SaveProduction",
            {
                OperationProductionID: $('#OperationProductionID').val(),
                OperationRecordID: $('#OperationRecordID').val(),
                //CycleTime: $('#cycletime').val(),
                CavitiesNumber: $('#cavitiesnumber').html(),
                //ProducedQty: $('#producedqty').val(),
                InitialShotNumber: $('#initialshot').val()
                //FinalShotNumber: $('#finalshot').val()
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                UpdateProductionData();
                UpdateProductionDetailsTable();
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });
    //guarda al dale ok al teclado
    $(document).on('click', '.btn-done-initial-shot', function () {
        $('.loading-process-div').show();
        $.post("/MFG/OperationRecords/SaveProduction",
            {
                OperationProductionID: $('#OperationProductionID').val(),
                OperationRecordID: $('#OperationRecordID').val(),
                //CycleTime: $('#cycletime').val(),
                CavitiesNumber: $('#cavitiesnumber').html(),
                //ProducedQty: $('#producedqty').val(),
                InitialShotNumber: $('#initialshot').val()
                //FinalShotNumber: $('#finalshot').val()
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                UpdateProductionData();
                UpdateProductionDetailsTable();
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $('#save_production_details').on('click', function () {
        $('.loading-process-div').show();
        $.post("/MFG/OperationRecords/SaveProductionDetailLog",
            {
                OperationProductionID: $('#OperationProductionID').val(),
                CurrentShotNumber: $('#ShotNumber').val(),
                RecalculateProductionHours: $('#RecalculateProductionHours').is(':checked')
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                //si todo esta bien
                //actualizar tabla
                UpdateProductionDetailsTable();
                //limpiar datos
                $('#ShotNumber').val('');
                //actualizar datos de prod
                UpdateProductionData();
                //click de back
                $('#log_production').click();
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });
    $(document).on('click', '.edit-productiondetail', function (e) {
        e.stopPropagation();
        $('.loading-process-div').show();
        var OperationProductionDetailID = $(this).data("entityid");
        $.get("/MFG/OperationRecords/GetProductionDetail", { OperationProductionDetailID }
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                //si todo esta bien, dibujar los datos del edit
                $('#newproductiondetail_title').text(LangResources.lbl_EditProductionHour + '  ' + data.entity.Hour); //en que hora              
                $("#Hour").val(data.entity.Hour);
                $('#ShotNumber').val(data.entity.ShotNumber);
                $('#OperationProductionDetailID').val(data.entity.OperationProductionDetailID);

                $('.select').selectpicker('refresh');
                UpdateRejectsTableHour();
                ShowHideProduction();
                $('#div_edit_shot').hide();
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });

    });
    $('#add_productionreject').on('click', function () {
        $('.loading-process-div').show();
        $.post("/MFG/OperationRecords/SaveReject",
            {
                ReferenceID: $('#OperationRecordID').val(),
                RejectTypeID: $('#ddl_ProductionReject :selected').val(),
                Quantity: $('#productionrejectqty').val(),
                Hour: $('#Hour').val()
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                //mostrar el listado y actualizart tabla de downtimes
                UpdateRejectsTableHour();
                UpdateRejectsTable();
                UpdateProductionData();
                UpdateProductionDetailsTable();
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });
    $('#save_productionreject').on('click', function () {
        $('.loading-process-div').show();
        var ProductionRejectID = $(this).data("entityid");
        $.post("/MFG/OperationRecords/SaveReject",
            {
                ProductionRejectID,
                ReferenceID: $('#OperationRecordID').val(),
                RejectTypeID: $('#ddl_ProductionReject :selected').val(),
                Quantity: $('#productionrejectqty').val(),
                Hour: $('#Hour').val()
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                $("#ddl_ProductionReject").val(0);
                $('#productionrejectqty').val('');
                $('.select').selectpicker('refresh');

                $('#save_productionreject').hide();
                $('#cancel_productionreject').hide();

                $('#add_productionreject').show();
                $('#productionrejectstable_div').show();
                //mostrar el listado y actualizart tabla de downtimes
                UpdateRejectsTableHour();
                UpdateRejectsTable();
                UpdateProductionData();
                UpdateProductionDetailsTable();
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });
    $('#cancel_productionreject').on('click', function () {
        $("#ddl_ProductionReject").val(0);
        $('#productionrejectqty').val('');
        $('.select').selectpicker('refresh');

        $('#save_productionreject').hide();
        $('#cancel_productionreject').hide();

        $('#add_productionreject').show();
        $('#productionrejectstable_div').show();
    });
    $(document).on('click', '.edit-reject', function (e) {
        e.stopPropagation();
        $('.loading-process-div').show();
        var ProductionRejectID = $(this).data("entityid");
        $.get("/MFG/OperationRecords/GetReject", { ProductionRejectID }
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                $("#ddl_ProductionReject").val(data.entity.RejectTypeID);
                $('#productionrejectqty').val(data.entity.Quantity);
                $('#save_productionreject').show();
                $('#cancel_productionreject').show();

                $('#save_productionreject').data("entityid", data.entity.ProductionRejectID);
                //
                $('#add_productionreject').hide();
                $('#productionrejectstable_div').hide();

                $('.select').selectpicker('refresh');
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });

    });
    $(document).on('click', '.delete-reject', function (e) {
        e.stopPropagation();
        var ProductionRejectID = $(this).data("entityid");
        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.post("/MFG/OperationRecords/DeleteReject", { ProductionRejectID }
            ).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                //UpdateRejectsTable();
                UpdateRejectsTableHour();
                UpdateProductionDetailsTable();
                UpdateProductionData();
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }, LangResources.ntf_MsgDeleteReject);
    });

    //downtimes
    $('#new_downtime').on('click', function () {
        $("#div_cambio_de_insertos").css("display", "none");
        $('#txt_Cambio_de_insertos').val("");
        ValidateInitialShot(function (result) {
            if (result.ErrorCode === 0) {
                if (result.entity.InitialShotNumber !== 0) {
                    $('#newdowntime_title').text(LangResources.btn_New);
                    // $("#ddl_Departments").val("0");
                    $('#save_downtimes').data("entityid", 0);
                    $("#ddl_Reasons").val("0");
                    $('#downtimecomments').val("");
                    var Hour = new Date().getHours();
                    var mins = new Date().getMinutes();
                    $('#StartTime').val("");
                    $('#Endtime').val("");

                    Setuptimepicker();
                    $('.timepicker').timepicker('setTime', Hour + ':' + mins);

                    $('.select').selectpicker('refresh');
                    ShowHideDowntime();
                } else {
                    notification("", LangResources.ntf_NoInitialshot, "error");
                }
            } else {
                notification("", result.ErrorMessage, "error");
            }
        });
    });
    $('#save_downtimes').on('click', function () {
        var ChangeInserts = null;
        var Option = $("#ddl_Reasons option:selected").text();
        if (Option == "Cambio de insertos") {
            ChangeInserts = $("#div_cambio_de_insertos").val();
        }

        $('.loading-process-div').show();
        var DownTimeID = $(this).data("entityid");
        $.post("/MFG/OperationRecords/SaveDowntime",
            {
                DownTimeID,
                ReferenceID: $('#OperationRecordID').val(),
                StartTime: $('#StartTime').val(),
                Endtime: $('#Endtime').val(),
                ReasonID: $('#ddl_Reasons').val(),
                Comments: $('#downtimecomments').val(),
                CloseTime: $('#closetime').is(":checked"),
                ChangeInserts: ChangeInserts
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                //mostrar el listado y actualizart tabla de downtimes
                UpdateDowntimesTable();
                ShowHideDowntime();
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });
    $(document).on('click', '.edit-downtime', function (e) {
        e.stopPropagation();
        $('.loading-process-div').show();
        var DownTimeID = $(this).data("entityid");
        $.get("/MFG/OperationRecords/GetDowntime", { DownTimeID }
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                //si todo esta bien, dibujar los datos del edit
                $('#newdowntime_title').text(LangResources.lbl_Edit);
                $("#ddl_Reasons").val(data.entity.ReasonID);
                $('#txt_Cambio_de_insertos').val("");
                var Option = $("#ddl_Reasons option:selected").text();
                if (Option == "Cambio de insertos") {
                    $('#txt_Cambio_de_insertos').val(data.entity.InsertsQuantity);
                    $("#div_cambio_de_insertos").show();
                }
                $('#downtimecomments').val(data.entity.Comments);

                $('#StartTime').val(data.entity.StartDateFormat);
                $('#Endtime').val(data.entity.EndDateFormat);
                $('#totaltime').text(data.entity.TimeFormat);
                Setuptimepicker();
                $('#StartTime').timepicker('setTime', data.entity.StartDateFormat);
                $('#Endtime').timepicker('setTime', data.entity.EndDateFormat);
                if (data.entity.StatusValue === "C") {
                    $('#closetime').prop('checked', false);
                } else if (data.entity.StatusValue === "O") {
                    $('#closetime').prop('checked', true);
                }

                $('#save_downtimes').data("entityid", data.entity.DownTimeID);
                $('.select').selectpicker('refresh');
                ShowHideDowntime();
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });

    });
    $(document).on('click', '.delete-downtime', function (e) {
        e.stopPropagation();
        var DownTimeID = $(this).data("entityid");
        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.post("/MFG/OperationRecords/DeleteDowntime", { DownTimeID }
            ).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                UpdateDowntimesTable();
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }, LangResources.ntf_MsgDeleteDowntime);
    });
    $('#closetime').on('click', function () {
        if ($(this).is(":checked")) {
            $('#Endtime').val("");
        }
        $('#Endtime').attr("disabled", $(this).is(":checked"));
    });
    $(document).on("change", "#ddl_Reasons", function () {
        var Option = $("#ddl_Reasons option:selected").text();
        if (Option == "Cambio de insertos") {
            $("#div_cambio_de_insertos").css("display", "block");
        } else {
            $("#div_cambio_de_insertos").css("display", "none");
            $('#txt_Cambio_de_insertos').val("");
        };
    });

    //rejects
    $('#new_reject').on('click', function () {
        $('#newreject_title').text(LangResources.btn_New);
        $('#ddl_Reject').val("0");
        $('#rejectqty').val("");
        $('.select').selectpicker('refresh');
        ShowHideReject();
    });
    $('#save_reject').on('click', function () {
        $('.loading-process-div').show();
        var ProductionRejectID = $(this).data("entityid");
        $.post("/MFG/OperationRecords/SaveReject",
            {
                ProductionRejectID,
                ReferenceID: $('#OperationRecordID').val(),
                RejectTypeID: $('#ddl_Reject').val(),
                Quantity: $('#rejectqty').val()
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                //mostrar el listado y actualizart tabla de downtimes
                //UpdateRejectsTable();
                UpdateRejectsTableHour();
                UpdateProductionDetailsTable();
                UpdateProductionData();
                ShowHideReject();
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    function SetUpSelectPicker() {
        $(".select").selectpicker({
            //liveSearch: false
        });
    }
    function UpdateDowntimesTable() {
        var OperationRecordID = $('#OperationRecordID').val();
        $('.loading-process-div').show();
        $.get("/MFG/OperationRecords/DowntimesList", { OperationRecordID }
        ).done(function (data) {
            $('#downtimestable_div').html(data);
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }
    function UpdateRejectsTable() {
        var OperationRecordID = $('#OperationRecordID').val();
        $('.loading-process-div').show();
        $.get("/MFG/OperationRecords/RejectList", { OperationRecordID }
        ).done(function (data) {
            $('.rejectstable_div').html(data);
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }
    function UpdateRejectsTableHour() {
        //var Hour = $('#ddl_Hours option:selected').val();
        var Hour = $('#Hour').val();
        var OperationRecordID = $('#OperationRecordID').val();
        //actualizar listado de rejects
        $('.loading-process-div').show();
        $.get("/MFG/OperationRecords/GetProductionRejectList",
            {
                OperationRecordID,
                Hour
            }).done(function (data) {
                if (data.ErrorCode === 0) {
                    $('#productionrejectstable_div').html(data.View);
                }
            }).always(function () {
                $('.loading-process-div').hide();
            });
    }
    function UpdateProductionDetailsTable() {
        var OperationProductionID = $('#OperationProductionID').val();
        $('.loading-process-div').show();
        $.get("/MFG/OperationRecords/ProductionDetailsList", { OperationProductionID }
        ).done(function (data) {
            $('#productiondetailstable_div').html(data);
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }
    function UpdateProductionData() {
        var OperationRecordID = $('#OperationRecordID').val();
        $.get("/MFG/OperationRecords/GetProduction", { OperationRecordID }
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                $('#TotalShotNumber').text(data.production.TotalShotNumber);
                $('#ProducedQty').text(data.production.ProducedQty);
                $('#RejectQty').text(data.production.RejectQty);
                $('#FinalShotNumber').text(data.production.FinalShotNumber);
            }
        });
    }
    function Setuptimepicker() {
        $('.timepicker').timepicker({
            minuteStep: 1,
            showSeconds: false,
            showMeridian: false,
            disableFocus: false,
            showInputs: false,
            allowInputToggle: true,
            snapToStep: true
        });
    }
    function ShowHideDowntime() {
        if ($('#new_downtime').hasClass("buttonpressed")) {
            $('#downtimestable_div').show();
            $('#newdowntime_div').hide();

            $('#new_downtime').html('<i class="fa fa-plus"></i> ' + LangResources.btn_New + '');
            $('#new_downtime').removeClass("buttonpressed");
        } else {
            $('#downtimestable_div').hide();
            $('#newdowntime_div').show();
            $('#new_downtime').html('<i class="fa fa-chevron-left"></i> ' + LangResources.btn_Back + '');
            $('#new_downtime').addClass("buttonpressed");
        }
    }
    function ShowHideReject() {
        if ($('#new_reject').hasClass("buttonpressed")) {
            $('.rejectstable_div').show();
            $('#newreject_div').hide();

            $('#new_reject').html('<i class="fa fa-plus"></i> ' + LangResources.btn_New + '');
            $('#new_reject').removeClass("buttonpressed");
        } else {
            $('.rejectstable_div').hide();
            $('#newreject_div').show();
            $('#new_reject').html('<i class="fa fa-chevron-left"></i> ' + LangResources.btn_Back + '');
            $('#new_reject').addClass("buttonpressed");
        }
    }
    function ShowHideProduction() {
        if ($('#log_production').hasClass("buttonpressed")) {
            $('.production_details_divs').show();
            $('.production_details_form').hide();
            $('#log_production').html('<i class="fa fa-tasks"></i> ' + LangResources.btn_LogProduction + '');
            $('#log_production').removeClass("buttonpressed");
        }
        else {
            $('.production_details_divs').hide();
            $('.production_details_form').show();

            $('#log_production').html('<i class="fa fa-chevron-left"></i> ' + LangResources.btn_Back + '');
            $('#log_production').addClass("buttonpressed");
        }
    }
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

        $.fn.numpad.defaults.onKeypadCreate = function () { $(this).find('.done').addClass('btn-success'); };
        $('.control-numpad').numpad();
    }
    function SetupProdNumpad() {
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
            $(this).find('.done').addClass('btn-done-initial-shot');
        };
        $('.control-prod-numpad').numpad();
    }
    function ValidateInitialShot(callback) {
        var OperationProductionID = $('#OperationProductionID').val();
        //llamada syncrona
        $.ajax({
            url: "/MFG/OperationRecords/GetInitialShot",
            data: { OperationProductionID },
            success: callback
        });

    }
    function CheckEveryHour() {
        IsCaptured();
        //var mins = new Date().getMinutes();
        //var secs = new Date().getSeconds();

        //if (parseInt(mins) <= 3 ) { //mins == "00" para hora exacta //parseInt(secs) <= 5 cada 5 seg revisa 
        //    //revisar en la BD si esta capturada una hora
        //    IsCaptured();
        //}
    }
    function IsCaptured() {
        var Hour = new Date().getHours();
        $.get("/MFG/OperationRecords/IsCaptured", {
            OperationProductionID: $('#OperationProductionID').val(),
            Hour
        }).done(function (data) {
            if (data.ErrorCode === 0) {
                if (!data.IsCaptured) {
                    $('.nav-tabs a[href="#production_div"]').tab('show');
                    $('#log_production').click();
                    //mostrar MSG
                    $('#lbl_capture_hour').text(Hour);
                    $('#message-box-warning').removeClass("open");
                    $('#message-box-warning').toggleClass("open");
                    //console.log('registrar produccion');
                }
            }
        });
    }

    $('#btn_closerecord').on('click', function () {
        $('#message-box-close').toggleClass("open");
    });
    $('#confirmbx_close').on('click', function () {
        $('.loading-process-div').show();
        $.post("/MFG/OperationRecords/Close",
            {
                OperationRecordID: $('#OperationRecordID').val(),
                OperationProductionID: $('#OperationProductionID').val(),
                LastShotNumber: $('#LastShotNumber').val()
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                window.location = "/MFG/OperationRecords/New";
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $('#btn_closeshiftrecord').on('click', function () {
        $('#message-box-closeshift').toggleClass("open");
    });
    $('#confirmbx_closeshift').on('click', function () {
        $('.loading-process-div').show();
        $.post("/MFG/OperationRecords/CloseShift",
            {
                OperationRecordID: $('#OperationRecordID').val(),
                OperationProductionID: $('#OperationProductionID').val(),
                LastShotNumber: $('#LastShiftShotNumber').val()
            }
        ).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode === 0) {
                window.location = "/MFG/OperationRecords/New";
            }
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $(".mb-control-close").on("click", function () {
        $(this).parents(".message-box").removeClass("open");
        $('.nav-tabs a[href="#production_div"]').tab('show');
        return false;
    });
    $('#btn_exit').on('click', function () {
        window.close();
    });

    function GetFunctions() {
        $(".isCalculated").each(function (e) {
            var functionvalue = $(this).data("functionvalue");
            //console.log(functionvalue);
            if (functionvalue != null && functionvalue != "") {
                eval(functionvalue);
            }

            $(this).attr('disabled', 'disabled');
            $(this).css("background-color", "transparent");
            $(this).css("color", "black");
        })
    }

    GetFunctions();

    //ChangeNaN();

    //$(document).on("change", ".param-val", function () {
    //    ChangeNaN();
    //});

    //function ChangeNaN() {
    //    if (isNaN($(".isCalculated").val()) || $(".isCalculated").val() == "" ) {
    //        $(".isCalculated").val("");
    //    }
    //}
}