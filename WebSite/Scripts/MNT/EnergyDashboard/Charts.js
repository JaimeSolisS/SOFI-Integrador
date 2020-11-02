function ChartInit(LangResources) {

    function GetHourValuesByDayChartData(e) {
        var panel = $('#consumption_by_day_panel');
        panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var EnergySensorID = $("#CurrentSensor option:selected").val();
        var Date = $("#txt_ChartDate").val();

        $.get("/MNT/EnergyDashboard/GetHourValuesByDayChartData", { EnergySensorID, Date }).done(function (data) {
            var HoursArray = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23";
            var horas = HoursArray.split(",");
            var series = data.series;
            var YieldGoalValue = 0;
            var MaxValue = data.MaxValue;
            Chart.defaults.global.legend.labels.usePointStyle = true;

            document.getElementById("_Div_ConsumptionByDay").innerHTML = '&nbsp;';
            document.getElementById("_Div_ConsumptionByDay").innerHTML = '<canvas id="ConsumptionByDay" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

            var ctx = document.getElementById("ConsumptionByDay").getContext('2d');
            var myChart = new Chart(ctx, {
                plugins: [ChartDataLabels],
                type: 'bar',
                data: {
                    labels: horas,
                    datasets: series
                },
                options: {
                    responsive: true,
                    plugins: {
                        // Change options for ALL labels of THIS CHART
                        datalabels: {
                            display: function (context) {
                                if (context.dataset.data[context.dataIndex] > 0) {
                                    return 'auto';
                                }
                                return false;
                            },
                            backgroundColor: function (context) {
                                if (context.dataset.backgroundColor == null) {
                                    return context.dataset.borderColor;
                                } else {
                                    return context.dataset.backgroundColor;
                                }
                            },
                            //clamp:true,
                            borderRadius: 4,
                            color: 'white',
                            font: function (context) {
                                var width = context.chart.width;
                                var size;
                                if (context.dataset.type == "line") {
                                    size = Math.round(width / 68);
                                } else {
                                    size = Math.round(width / 50);
                                }
                                return {
                                    size: size,
                                    weight: 'bold'
                                };
                            },
                            formatter: function (value) {
                                return value;
                            },
                            align: function (ctx) {
                                if (ctx.dataset.type == "line") {
                                    return 'start';
                                } else {
                                    //return 'center';
                                    var idx = ctx.dataIndex;
                                    var val = ctx.dataset.data[idx];
                                    var datasets = ctx.chart.data.datasets;
                                    var min = val;
                                    var max = val;
                                    var i, ilen, ival;

                                    for (i = 0, ilen = datasets.length; i < ilen; ++i) {
                                        if (i === ctx.datasetIndex) {
                                            continue;
                                        }

                                        ival = datasets[i].data[idx];
                                        min = Math.min(min, ival);
                                        max = Math.max(max, ival);

                                        if (val > min && val < max) {
                                            return 'center';
                                        }
                                    }

                                    return val >= min ? 'start' : 'end';
                                }

                            },
                            offset: function (ctx) {
                                if (ctx.dataset.type == "line") {
                                    return 0;
                                } else {
                                    var idx = ctx.dataIndex;
                                    var val = ctx.dataset.data[idx]; //ctx.dataset serie actual
                                    var serieidx = ctx.datasetIndex; //indice del dataset
                                    var datasets = ctx.chart.data.datasets;//todos los datasets
                                    var min = val;
                                    var max = val;
                                    var i, ilen, ival;
                                    var ant = ctx.dataset.data[idx - 1];

                                    for (i = 0, ilen = datasets.length; i < ilen; ++i) {
                                        if (i === ctx.datasetIndex) {
                                            continue;
                                        }

                                        ival = datasets[i].data[idx];
                                        min = Math.min(min, ival);
                                        max = Math.max(max, ival);

                                        if (val > min && val < max) {
                                            return 0;
                                        }
                                    }

                                    return val >= min ? -40 : 30;

                                }
                            }
                        },
                    },
                    scales: {
                        yAxes: [
                            {
                                id: 'LineAxis',
                                type: 'linear',
                                position: 'right',
                                ticks: {
                                    fontSize: 12,
                                    //max: 110,
                                    max: data.MaxValue,
                                    min: 0
                                },
                            },
                            {
                                id: 'BarAxis',
                                type: 'linear',
                                position: 'left',
                                stacked: true,
                                ticks: {
                                    fontSize: 12,
                                    max: MaxValue, //2,
                                    beginAtZero: true
                                }
                            }],
                        xAxes: [{ stacked: true }]
                    },
                    legend: {
                        display: true,
                        position: 'bottom',
                        labels: {
                            fontSize: 12
                        }
                    },
                    annotation: {
                        annotations: [
                            {
                                type: "line",
                                mode: "horizontal",
                                scaleID: "LineAxis",
                                value: YieldGoalValue,
                                borderColor: "green",
                                borderWidth: 2,
                                borderDash: [3, 6],
                                xMax: 18,
                                //label: {
                                //    backgroundColor: 'green',
                                //    content: 'Goal: ' + YieldGoalValue + '%',
                                //    fontSize: 32,
                                //    enabled: true,
                                //    xPadding: 8,
                                //    yPadding: 5,
                                //    xAdjust: 500,
                                //    yAdjust: -25,
                                //    position: 'center'
                                //}
                            }
                        ]
                    },
                    animation: {
                        onComplete: function (animation) {
                            //ESTA SECCION ES LA QUE AJUSTA EL TAMAÑO DE LA GRÁFICA PARA QUE SE ADAPTE CON LA RESOLUCION DE LA PANTALLA
                            $('#ConsumptionGlobal').height($('#_Div_ConsumptionGlobal').height());

                        }
                    }
                }
            });

            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");

        });
    }

    function GetHourValuesGlobalChartData() {
        var panel = $('#global_consumption_panel');
        panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var EnergySensorID = $("#CurrentSensor option:selected").val();
        var StartDate = $("#txt_ChartStartDate").val();
        var EndDate = $("#txt_ChartEndDate").val();
        var EnergySensorFamilyID = $("#EnergySensorFamilyID").val();

        $.get("/MNT/EnergyDashboard/GetHourValuesGlobalChartData", { EnergySensorID, EnergySensorFamilyID, StartDate, EndDate }).done(function (data) {
            var parseLabel = $.parseJSON(data.labels);
            var parseData = $.parseJSON(data.dataset);
            var parseBackgroundColor = $.parseJSON(data.color);

            var labels = [];
            var data = [];
            var BackroundColor = [];
            $.each(parseLabel, function (key, value) {
                labels.push($(this)[0].Name);
            });
            $.each(parseBackgroundColor, function (key, value) {
                BackroundColor.push($(this)[0].BackgroundColor);
            });
            $.each(parseData, function (key, value) {
                data.push($(this)[0].Percentage);
            });

            document.getElementById("_Div_ConsumptionGlobal").innerHTML = '&nbsp;';
            document.getElementById("_Div_ConsumptionGlobal").innerHTML = '<canvas id="ConsumptionGlobal" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

            var ctx = document.getElementById("ConsumptionGlobal").getContext('2d');

            var myPieChart = new Chart(ctx, {
                plugins: [ChartDataLabels],
                type: 'pie',
                data: {
                    datasets: [{
                        data: data,
                        backgroundColor: BackroundColor
                    }],
                    labels: labels,
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    tooltips: {
                        enabled: false
                    },
                    plugins: {
                        datalabels: {
                            formatter: (value, ctx) => {
                                let sum = 0;
                                let dataArr = ctx.chart.data.datasets[0].data;
                                dataArr.map(data => {
                                    sum += data;
                                });
                                let percentage = (value * 100 / sum).toFixed(1) + "%";
                                return percentage;
                            },
                            color: '#fff',
                        }
                    }
                }
            });

            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");

        });
    }

    function UpdateValuesByHourChar() {
        GetHourValuesByDayChartData();
    }

    $(document).on("click", "#btn_SetSameValue", function () {
        $(".MaxValueByHour").val($("#txt_SetSameValue").val());
    });

    //se inicializa cada control independiente por el evento changeDate, que es particular
    $("#txt_ChartDate").datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        endDate: new Date(),
        autoclose: true,
    }).on('changeDate', function (ev) {
        GetHourValuesByDayChartData();
    });
    $("#txt_ChartStartDate,#txt_ChartEndDate").datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        endDate: new Date(),
        autoclose: true,
    }).on('changeDate', function (ev) {
        GetHourValuesGlobalChartData();
    });

    $("#CurrentSensor").val(parseInt($("#EnergySensorID").val()));
    $(".selectpicker").selectpicker("refresh");

    $("#btn_SetSensorAlarm").on("click", function (e) {
        $('.loading-process-div').show();

        var EnergySensorID = $("#EnergySensorID").val();

        $.post("/MNT/EnergySensors/GetModalAlarmConfiguration", { EnergySensorID }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_Mo_AlarmConfiguration").html(data.View);
                $("#mo_AlarmsConfiguration").modal("show");
                $("#modalSensorName").text($("#CurrentSensor option:selected").text());
                $("#ddl_SensorsConfigList").selectpicker();

            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $(document).on("click", "#btn_SaveAlarmconfiguracion", function () {
        var count = 0;
        var MaxValuesFourHours = [];
        var EnergySensorID = $("#EnergySensorID").val();

        $('.MaxValueByHour').each(function () {
            var ValueHour = $(this).closest("tr").find("td:nth-child(1)").text();
            ValueHour = ValueHour.substr(0, ValueHour.indexOf(" "));
            var MaxValue = $("#txt_ValueForHour_" + count).val();

            var Item = {
                ValueHour: ValueHour,
                MaxValue: MaxValue
            };
            MaxValuesFourHours.push(Item);
            count++
        });

        $.post("/MNT/EnergyDashboard/SaveAlarmConfiguration", { EnergySensorID, MaxValuesFourHours }).done(function (data) {
            notification("", "Alarm Configuration Saves", data.notifyType);
            GetHourValuesByDayChartData();
        }).fail(function () {
            notification("", data.ErrorMessage, data.notifyType);
        }).always(function () {
            $('.loading-process-div').hide();
        });


        $("#mo_AlarmsConfiguration").modal("toggle");

    });

    $(document).on("change", "#ddl_SensorsConfigList", function () {
        var EnergySensorID = $("#ddl_SensorsConfigList option:selected").attr("value");
        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.get("/MNT/EnergySensors/GetEnergySensorValuesList", { EnergySensorID }).done(function (data) {
                var a = data.list;
                $.each(data.list, function (index, value) {
                    $("#txt_ValueForHour_" + value.Value).val(value.Text);
                });
            }).fail(function () {
                notification("", data.ErrorMessage, data.notifyType);
            }).always(function () {
                $('.loading-process-div').hide();
            });

        }, LangResources.msg_SetConfigurationValuesForAlarm);

    });

    $("#CurrentSensor").on("change", function (e) {
        UpdateValuesByHourChar();
    });

    GetHourValuesByDayChartData();
    GetHourValuesGlobalChartData();

    $(".panel-refresh").on("click", function () {
        var panel = $(this).parents(".panel");
        var callback = panel.data("updatefunc");
        var x = eval(callback)
        if (typeof x == 'function') {
            x();
        }
    });
    $(".panel-fullscreen-chart").on("click", function () {
        //panel_fullscreen($(this).parents(".panel"));
        var panel = $(this).parents(".panel");
        var chartid = panel.data("chartid");
        var chartplugin = panel.data("chartplugin");
        var panel_ratio = $(window).height() * 0.4 + "px";

        if (panel.hasClass("panel-fullscreened")) {//MINIMIZaR
            panel.find('.panel-title-box >h3').css("font-size", "100%");
            panel.find('.panel-title-box >span').css("font-size", "100%");

            panel.removeClass("panel-fullscreened").unwrap();
            panel.find(".panel-body").css("height", panel_ratio);
            panel.find(".chart-holder").css("height", panel_ratio);
            panel.find(".chart-holder").css("min-height", panel_ratio);
            panel.find(".panel-fullscreen .fa").removeClass("fa-compress").addClass("fa-expand");

            $(window).resize();

        } else {//MAXIMIZaR
            var full_screen_panel_ratio = 0.85; //30 normal

            panel.find(".panel-body,.chart-holder").height($(window).height() * full_screen_panel_ratio);

            panel.find('.panel-title-box >h3').css("font-size", "150%");
            panel.find('.panel-title-box >span').css("font-size", "120%");

            panel.addClass("panel-fullscreened").wrap('<div class="panel-fullscreen-wrap"></div>');
            panel.find(".panel-fullscreen .fa").removeClass("fa-expand").addClass("fa-compress");

            $(window).resize();

        }
        return false;
    });

    $("#btn_ConsumptionExcelReport").on("click", function () {
        var EnergySensorID = $("#EnergySensorID").val();
        $.get("/MNT/EnergyDashboard/GetModalExporToExcel", { EnergySensorID }).done(function (data) {

            $("#div_Mo_ExportToExcel").html(data.View);
            $("#mo_ExportToExcel").modal("show");

            $("#txt_StartDate, #txt_EndDate").datetimepicker({
                format: 'YYYY-MM-DD HH:mm'
            });

            //$("#modalSensorName").text($("#CurrentSensor option:selected").text());
            //$("#ddl_SensorsConfigList").selectpicker();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });
}
