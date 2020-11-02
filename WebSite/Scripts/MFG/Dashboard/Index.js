// =============================================================================================================================
//  Version: 20192705
//  Author:  cgarcia
//  Created Date: 27 May 2019
//  Description:  js para dashboard de maquinas
//  Modifications: 
// =============================================================================================================================

function IndexInit(LangResources) {
    var LoadPickers = (function fn_LoadTimePicker() {
        $(".timepicker").timepicker({
            format: 'HH:mm'
        });
        $(".datepickerr").datepicker();
    }());

    /////////////////Funciones aux///////////////
    var elem = document.documentElement;
    /* View in fullscreen */
    function openFullscreen() {
        if (elem.requestFullscreen) {
            elem.requestFullscreen();
        } else if (elem.mozRequestFullScreen) { /* Firefox */
            elem.mozRequestFullScreen();
        } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
            elem.webkitRequestFullscreen();
        } else if (elem.msRequestFullscreen) { /* IE/Edge */
            elem.msRequestFullscreen();
        }
    }
    /* Close fullscreen */
    function closeFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) { /* Firefox */
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { /* IE/Edge */
            document.msExitFullscreen();
        }
    }
    function SetupPanelCollapse() {
        $(".panel-collapse").on("click", function () {
            panel_collapse($(this).parents(".panel"));
            $(this).parents(".dropdown").removeClass("open");
            return false;
        });
    }
    function DrawGauges() {
        $('.canvas-gauge').each(function () {
            var $gauge = $(this);
            var alert = $gauge.data("alert");

            var maxvalue = parseFloat($gauge.data("maxvalue"));
            var minvalue = parseFloat($gauge.data("minvalue"));
            var range = (maxvalue - minvalue) / 4;
            var midrange = range / 2;


            var target = document.getElementById($gauge.attr('id')); // your canvas element
            var gauge = new Gauge(target).setOptions({
                angle: 0,
                lineWidth: 0.2,
                pointer: {
                    length: 0.6,
                    strokeWidth: 0.05,
                    color: '#000000'
                },
                staticZones: [
                    { strokeStyle: "#F03E3E", min: minvalue - range, max: minvalue }, //rojo
                    { strokeStyle: "#FFFF00", min: minvalue, max: minvalue + midrange }, //amarillo
                    { strokeStyle: "#30B32D", min: minvalue + midrange, max: maxvalue - midrange },//verde
                    { strokeStyle: "#FFFF00", min: maxvalue - midrange, max: maxvalue }, //amarillo
                    { strokeStyle: "#F03E3E", min: maxvalue, max: maxvalue + range }//rojo
                ],
                staticLabels: {
                    font: "15px sans-serif",
                    labels: [minvalue - range, minvalue, maxvalue, maxvalue + range],
                    fractionDigits: 1
                },
                limitMax: false,
                limitMin: false,
                strokeColor: '#E0E0E0'
            });
            gauge.maxValue = maxvalue + range; // set max gauge value
            gauge.setMinValue(minvalue - range);  // Prefer setter over gauge.minValue = 0
            gauge.set(parseFloat($gauge.data("paramvalue"))); // set actual value
        });
    }

    function UpdateMachines() {
        $.get("/MFG/Dashboard/UpdateMachinesList",
            {
                Date: $('#txt_Date').val(),
                ShiftID: $('#ddl_Shift').val()
            }
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                //actualizar 
                $('#div_machineslist').html(data.View);
                SetupPanelCollapse();
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });
    }
    function UpdateTasks() {
        $.get("/MFG/Dashboard/UpdateTasksList",
            {
                Date: $('#txt_Date').val()
            }
        ).done(function (datatask) {
            if (datatask.ErrorCode === 0) {
                //actualizar 
                $('#div_taskslist').html(datatask.View);
                SetupPanelCollapse();
            } else {
                notification("", datatask.ErrorMessage, "error");
            }
        });
    }
    ///////////////Config de Plugins///////////////////
    $('.datepicker').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd'
        //language: LangResources.datepicker_lang
    });
    $(".select").selectpicker({
        liveSearch: false
    });

    ////////////////////Eventos DOM////////////////////
    $('.chartslider-full').on('click', function () {
        if ($(this).hasClass("fullscreened")) {//MINIMIZaR 
            $(this).removeClass("fullscreened");
            $(this).find(".fa").removeClass("fa-compress").addClass("fa-expand");
            closeFullscreen();
        } else { //MAXIMIZAR
            openFullscreen();
            $(this).addClass("fullscreened");
            $(this).find(".fa").removeClass("fa-expand").addClass("fa-compress");

            $('#ChartSlider').css('height', $(window).height());
            $('.slick-track').css('height', $(window).height());
        }

    });

    $('#btn_Update').on('click', function () {
        $('.loading-process-div').show();

        $.get("/MFG/Dashboard/UpdateMachinesList",
            {
                Date: $('#txt_Date').val(),
                ShiftID: $('#ddl_Shift').val()
            }
        ).done(function (data) {
            $('.loading-process-div').hide();
            if (data.ErrorCode === 0) {
                //actualizar 
                $('#div_machineslist').html(data.View);

                $('.loading-process-div').show();
                $.get("/MFG/Dashboard/UpdateTasksList",
                    {
                        Date: $('#txt_Date').val()
                    }
                ).done(function (datatask) {
                    $('.loading-process-div').hide();
                    if (datatask.ErrorCode === 0) {
                        //actualizar 
                        $('#div_taskslist').html(datatask.View);
                        $('#Machine_div').hide();
                        $('#Index_div').show();
                        SetupPanelCollapse();
                        //actualizar daycode
                        // $('#txt_daycode').text(datatask.JulianDay);
                    } else {
                        notification("", datatask.ErrorMessage, "error");
                    }
                });
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });

    });

    $(document).on('click', '.machine-image', function () {
        var MachineID = $(this).data("machineid");
        var OperationRecordID = $(this).data("operationrecordid");

        //verificar que la maquina tenga link, si es asi, abrirlo en otra pestaña
        var RelatedLink = $(this).data("relatedlink");
        if (RelatedLink != "" && RelatedLink != null) {
            window.open(RelatedLink, '_blank');
        }

        $('.loading-process-div').show();
        $.get("/MFG/Dashboard/Show", { MachineID, OperationRecordID }
        ).done(function (data) {
            $('.loading-process-div').hide();
            if (data.ErrorCode === 0) {
                $('#Index_div').hide();
                $('#Machine_div').html(data.View);
                $('#Machine_div').show();
                DrawGauges();
                SetupPanelCollapse();
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });
    });

    $(document).on('click', '#btn_back', function () {
        $('#Machine_div').hide();
        $('#Index_div').show();
    });

    setInterval(UpdateMachines, 1000 * 60);//cada 10 min = 1000 * 60 * 10
    setInterval(UpdateTasks, 1000 * 60 * 5);

    $(document).on("click", ".assigned", function () {
        //var dateNow = new Date();

        $('.loading-process-div').show();
        var OperationTaskID = $(this).closest("tr").data("entityid");

        $.get("/MFG/ActionPlan/GetModalAssignTask", { OperationTaskID }).done(function (data) {
            $('.loading-process-div').hide();

            if (data.ErrorCode === 0) {
                $("#div_MO_AssignTask").html(data.View);
                $("#mo_AssignTask").modal("show");
                $(".select").selectpicker();

                if ($("#CultureID").val() == "ES-MX") {
                    $('#assignTask_date').datetimepicker({
                        useCurrent: false,
                        format: 'DD[-]MM[-]YYYY'
                    });
                } else {
                    $('#assignTask_date').datetimepicker({
                        useCurrent: false,
                        format: 'MM[-]DD[-]YYYY'
                    });
                }

                $("#assignTask_time").datetimepicker({
                    format: 'HH:mm'
                });

                $(".input-group-addon").click();
                $(".input-group-addon").click();

            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });

    });

    $(document).on("click", "#btn_AssignResponsableTask", function (e) {
        e.stopImmediatePropagation();

        var OperationTaskID = $("#Mo_OperationTaskID").val();
        var ResponsableID = $("#assignTask_user option:selected").val();
        var TargetDate = $("#assignTask_dates").val();
        var SuggestedAction = $("#assignTask_sugestedAction").val();
        var TargetTime = $("#assignTask_times").val();
        var AttendantUserName = $("#assignTask_attendantUserName").val();

        if (ResponsableID !== 0) {
            $('.loading-process-div').show();
            $.post('/MFG/ActionPlan/UpdateOperationTaskResponsable', {
                OperationTaskID,
                ResponsableID,
                SuggestedAction,
                AttendantUserName,
                TargetDate,
                TargetTime
            }).done(function (data) {
                $('.loading-process-div').hide();
                if (data.ErrorCode === 0) {
                    $("#mo_AssignTask").modal("toggle");
                    notification("", LangResources.ActionTaskUpdateSuccess, "success");
                    UpdateTasks();
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                $('.loading-process-div').hide();
            });
        } else {
            notification("", LangResources.UserIsMandatory, "ntf_");
        }
    });
    UpdateTasks();

    $("#btn_ExportToExcel").on("click", function () {
        $('.loading-process-div').show();
        $.get("/MFG/Dashboard/GetExportToExcelModal").done(function (data) {
            if (data.ErrorCode == 0) {
                $('.loading-process-div').hide();
                $("#div_Mo_ExportToExcel").html(data.View);
                $(".selectpicker").selectpicker("refresh");

                if ($("#CultureIDExcel").val() == "ES-MX") {
                    $(".datepickerr").datetimepicker({
                        format: 'DD[-]MM[-]YYYY'
                    });
                } else {
                    $(".datepickerr").datetimepicker({
                        format: 'MM[-]DD[-]YYYY'
                    });
                }

                $(".timepicker").datetimepicker({
                    format: 'HH:mm'
                });



                $("#mo_ExportToExcel").modal("show");
            } else {
                notification("", data.ErrorMessage, data.notifyType);
            }
        });
    });


    $(document).on("change", "#ddl_ExcelShifts", function () {
        //console.log('selectedD: ' + this.value);
        if (this.value !== "0") {
            $.get("/MFG/Dashboard/GetShiftStartEndHours", { ShiftID: this.value }).done(function (data) {
                $('#txt_StartTimeExcel').val(data.start);
                $('#txt_EndTimeExcel').val(data.end);
            });
        }
    });
}

