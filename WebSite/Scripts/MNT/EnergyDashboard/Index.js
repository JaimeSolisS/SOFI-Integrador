function IndexInit(LangResources) {

    function DrawGauges(minvalue, maxvalue, paramvalue) {
        var $gauge = $("#canvas-gauge-id");
        var range = (maxvalue - minvalue) / 4;
        var midrange = range / 2;
        var target = document.getElementById($gauge.attr('id')); 
        var gauge = new Gauge(target).setOptions({
            angle: 0,
            lineWidth: 0.2,
            pointer: {
                length: 0.6,
                strokeWidth: 0.05,
                color: '#000000'
            },
            staticZones: [
                { strokeStyle: "#30B32D", min: minvalue - range , max: maxvalue - midrange },//verde
                { strokeStyle: "#FFFF00", min: maxvalue - midrange, max: maxvalue }, //amarillo
                { strokeStyle: "#F03E3E", min: maxvalue, max: maxvalue + range }//rojo
            ],
            staticLabels: {
                font: "15px sans-serif",
                labels: [ minvalue, maxvalue, maxvalue + range],
                fractionDigits: 1
            },
            limitMax: false,
            limitMin: false,
            strokeColor: '#E0E0E0'
        });
        gauge.maxValue = maxvalue + range; // set max gauge value
        gauge.setMinValue(minvalue - range);  // Prefer setter over gauge.minValue = 0
        gauge.set(parseFloat(paramvalue));
        $("#label_total_consumption").text(paramvalue);
    }

    function GetGaugeValues() {
        $.get("/MNT/EnergyDashboard/GetGaugeValues").done(function (data) {
            DrawGauges(data.result[0].MinValue, data.result[0].MaxValue, data.result[0].ParameterValue);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }

    function GetHourValuesByFamiliesChartGauge(e) {
        var ChartDate = $("#txt_ChartGaugeDate").val();
        $('.loading-process-div').show();
        $.get("/MNT/EnergyDashboard/GetHourValuesByFamiliesChartGauge", { ChartDate }).done(function (data) {
            var HoursArray = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23";
            var horas = HoursArray.split(",");
            var series = data.series;
            var YieldGoalValue = 0;
            var MaxValue = data.MaxValue;
            Chart.defaults.global.legend.labels.usePointStyle = true;

            document.getElementById("_Div_GaugeFamiliesChart").innerHTML = '&nbsp;';
            document.getElementById("_Div_GaugeFamiliesChart").innerHTML = '<canvas id="GaugeFamiliesChart" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

            var ctx = document.getElementById("GaugeFamiliesChart").getContext('2d');
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
                    //annotation: {
                    //    annotations: [
                    //        {
                    //            type: "line",
                    //            mode: "horizontal",
                    //            scaleID: "LineAxis",
                    //            value: YieldGoalValue,
                    //            borderColor: "green",
                    //            borderWidth: 2,
                    //            borderDash: [3, 6],
                    //            xMax: 18,
                    //            //label: {
                    //            //    backgroundColor: 'green',
                    //            //    content: 'Goal: ' + YieldGoalValue + '%',
                    //            //    fontSize: 32,
                    //            //    enabled: true,
                    //            //    xPadding: 8,
                    //            //    yPadding: 5,
                    //            //    xAdjust: 500,
                    //            //    yAdjust: -25,
                    //            //    position: 'center'
                    //            //}
                    //        }
                    //    ]
                    //},
                    animation: {
                        onComplete: function (animation) {
                            //console.log('grafica cargada..');
                            $('#CanvasHourMaxValuesChart').height($('#HourMaxValuesChart').height());
                        }
                    }
                }
            });
            $('.loading-process-div').hide();

        }).always(function () {
            $('.loading-process-div').hide();
        });
    }


    GetGaugeValues();
    setInterval(function () {
        GetGaugeValues();
    }, 3600 * 1000);

    $(".datepicker").datepicker({
        format: "mm-yyyy",
        viewMode: "months",
        minViewMode: "months"
    });

    //Este actualiza el index cuando se cambia la fecha que esta abajo del reloj
    $("#txt_ChartDate").on("change", function () {
        var Date = $("#txt_ChartDate").val();
        window.location = "/MNT/EnergyDashboard/Index?Date=" + Date;
    });

    $(document).on('click', '.energy-sensor-family-image', function () {
        var EnergySensorFamilyID = $(this).data("energysensorfamilyid");
        window.location = "/MNT/EnergyDashboard/Details?EnergySensorFamilyID=" + EnergySensorFamilyID;
    });

    //Al dar click en el icono del gauge, se abre el popup que muestra la gráfica del consumo por hora del gauge
    $("#canvas-gauge-id").on("click", function () {
        $('.loading-process-div').show();
        $.get("/MNT/EnergyDashboard/GetModalChartTotalFamilies").done(function (data) {
            $("#div_Mo_ChartFamiliesGauge").html(data.View);
            $("#mo_ChartFamiliesGauge").modal("show");

            //Cuando se carga el modal, se aplica el plugin del datepicker al texbox de la fecha
            $("#txt_ChartGaugeDate").datepicker({
                autoclose: true,
                format: 'yyyy-mm-dd',
                endDate: new Date(),
                autoclose: true,
            }).on('changeDate', function (ev) {
                GetHourValuesByFamiliesChartGauge();
            });

            GetHourValuesByFamiliesChartGauge();

            $('.loading-process-div').hide();
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

}
