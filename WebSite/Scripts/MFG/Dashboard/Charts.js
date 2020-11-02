// =============================================================================================================================
//  Version: 20190624
//  Author:  cgarcia
//  Created Date: 24 jun 2019
//  Description:  js para charts de gasket de maquinas
//  Modifications: 
// =============================================================================================================================

function ChartsInit(LangResources) {
    $('.datepicker').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd'
        //language: LangResources.datepicker_lang
    });
    $(".select").selectpicker();

    function UpdateLabelHours() {
        var ShiftID = $("#ShiftID").val();
        $.get("/MFG/Dashboard/GetLabelHours", { ShiftID }).done(function (data) {
            $('#HoursArray').val(data);
        });
    }


    function YieldDefectsChart() {
        UpdateLabelHours();
        var panel = $('#yield_panel');
        panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");


        var MachinesIDs = [];//$('#yieldchart_SelectedMachines option:selected').val();

        $('#yieldchart_SelectedMachines option:selected').each(function () {
            MachinesIDs.push(parseInt($(this).val()));
        });


        var ProductionProcessID = $('#yieldchart_process').val();
        var MaterialID = $('#yieldchart_materials').val();
        var OperationDate = $('#txt_Date').val();
        var ShiftID = $('#ShiftID').val();

        jQuery.ajaxSettings.traditional = true
        $.ajax({
            url: '/MFG/Dashboard/GetYieldDefectsChartData',
            type: 'get',
            traditional: true,
            dataType: "json",
            contextType: "application/json",
            data: { MachinesIDs, ProductionProcessID, MaterialID, OperationDate, ShiftID },

        }).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var series = data.series;
            var YieldGoalValue = 0;
            var MaxValue = data.MaxValue;
            Chart.defaults.global.legend.labels.usePointStyle = true;

            document.getElementById("YieldDefectsChart").innerHTML = '&nbsp;';
            document.getElementById("YieldDefectsChart").innerHTML = '<canvas id="ChartYieldDefects" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

            var ctx = document.getElementById("ChartYieldDefects").getContext('2d');
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
                                return value + '%';
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
                                    max: 110,
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
                                label: {
                                    backgroundColor: 'green',
                                    content: 'Goal: ' + YieldGoalValue + '%',
                                    fontSize: 32,
                                    enabled: true,
                                    xPadding: 8,
                                    yPadding: 5,
                                    xAdjust: 500,
                                    yAdjust: -25,
                                    position: 'center'
                                }
                            }
                        ]
                    },
                    animation: {
                        onComplete: function (animation) {
                            //console.log('grafica cargada..');
                            $('#ChartYieldDefects').height($('#YieldDefectsChart').height());
                        }
                    }
                }
            });



        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
        });
    }
    function UpdateYieldDefectsChart() {
        YieldDefectsChart();
    }

    function EERDowntimesChart() {
        UpdateLabelHours();
        var panel = $('#EER_panel');
        panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');

        var MachinesIDs = [];
        $('#eerchart_SelectedMachines option:selected').each(function () {
            MachinesIDs.push(parseInt($(this).val()));
        });


        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        //var MachinesIDs = $('#eerchart_SelectedMachines').val();
        var ProductionProcessID = $('#eerchart_process').val();
        var MaterialID = $('#eerchart_materials').val();
        var OperationDate = $('#txt_Date').val();
        var ShiftID = $('#ShiftID').val();

        $.ajax({
            url: '/MFG/Dashboard/GetYieldDowntimesChartData',
            type: 'get',
            traditional: true,
            dataType: "json",
            contextType: "application/json",
            data: { MachinesIDs, ProductionProcessID, MaterialID, OperationDate, ShiftID },

        }).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var series = data.series;
            var YieldGoalValue = 0;
            var MaxValue = data.MaxValue - 30;

            Chart.defaults.global.legend.labels.usePointStyle = true;

            document.getElementById("TopDefectsChart").innerHTML = '&nbsp;';
            document.getElementById("TopDefectsChart").innerHTML = '<canvas id="ChartEEM" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

            var ctx = document.getElementById("ChartEEM").getContext('2d');

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
                                return value + '%';
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
                                    max: 110,
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
                                label: {
                                    backgroundColor: 'green',
                                    content: 'Goal: ' + YieldGoalValue + '%',
                                    fontSize: 32,
                                    enabled: true,
                                    xPadding: 8,
                                    yPadding: 5,
                                    xAdjust: 500,
                                    yAdjust: -25,
                                    position: 'center'
                                }
                            }
                        ]
                    },
                    animation: {
                        onComplete: function (animation) {
                            //console.log('grafica cargada..');
                            $('#ChartEEM').height($('#TopDefectsChart').height());
                        }
                    }
                }
            });

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
        });
    }
    function UpdateTopDefectsChart() {
        EERDowntimesChart();
    }

    function Productionchart() {
        var panel = $('#prod_panel');
        panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");

        var MachinesIDs = [];
        $('#prodchart_SelectedMachines option:selected').each(function () {
            MachinesIDs.push(parseInt($(this).val()));
        });
        var OperationDate = $('#txt_Date').val();
        var ShiftID = $('#ShiftID').val();

        $.get("/MFG/Dashboard/GetProductionChartData", { MachinesIDs, OperationDate, ShiftID }).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            //console.log(horas);
            var goals = [];//= data.goals.map(Number);
            var pieces = [];
            var LabelsColor = data.LabelsColor;
            for (var i = 0; i < data.goals.length; i++) {
                goals.push(parseFloat(data.goals[i]));
            }
            for (var i = 0; i < data.pieces.length; i++) {
                if (data.pieces[i] === "null") {
                    pieces.push(null);
                } else {
                    pieces.push(parseFloat(data.pieces[i]));

                }
            }

            var options = {
                chart: {
                    width: "100%",
                    height: "100%",
                    resized: true,
                    type: 'line',
                    events: {
                        mounted: function (chartContext, config) {
                            panel.find(".panel-refresh-layer").remove();
                            panel.removeClass("panel-refreshing");
                        }
                    },
                    id: 'graficadeproduccion'
                },
                grid: {
                    padding: {
                        left: 30
                    },
                },
                responsive: [
                    {
                        breakpoint: 200,
                        options: {
                            plotOptions: {
                                bar: {
                                    horizontal: false
                                }
                            },
                            legend: {
                                position: "bottom"
                            },
                            height: 880

                        }
                    }
                ],
                series: [
                    {
                        name: 'Meta',
                        type: 'area',
                        stroke: {
                            curve: 'smooth'
                        },
                        data: goals
                    }, {
                        name: 'Producido',
                        type: 'line',
                        stroke: {
                            curve: 'straight'
                        },
                        data: pieces
                    }],
                fill: {
                    type: 'solid',
                    opacity: [0.35, 1],
                },
                xaxis: {
                    type: 'category',
                    tickPlacement: 'between',
                    hideOverlappingLabels: true,
                    categories: horas
                },
                markers: {
                    size: 6
                },
                dataLabels: {
                    enabled: true,
                    offsetY: 20,
                    formatter: function (val) {
                        if (val == null) {
                            return "";
                        } else {
                            return val;
                        }
                    },
                    style: {
                        fontSize: '15px',
                        fontFamily: 'Helvetica, Arial, sans-serif',
                        colors: [LabelsColor]
                    },
                    dropShadow: {
                        enabled: true
                    }
                },
                yaxis: [
                    {
                        title: {
                            text: 'Producido'
                        }
                    }
                ],
                tooltip: {
                    shared: true,
                    intersect: false
                },
            }

            var chart = new ApexCharts(
                document.querySelector("#Productionchart"),
                options
            );
            chart.render();
        });
    }
    function UpdateProductionchart() {
        var panel = $('#prod_panel');
        var lastheight = panel.find(".panel-body").height();
        panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var MachinesIDs = [];
        $('#prodchart_SelectedMachines option:selected').each(function () {
            MachinesIDs.push(parseInt($(this).val()));
        });
        var OperationDate = $('#txt_Date').val();
        var ShiftID = $('#ShiftID').val();

        $.get("/MFG/Dashboard/GetProductionChartData", { MachinesIDs, OperationDate, ShiftID }).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var pieces = [];
            var goals = [];
            for (var i = 0; i < data.pieces.length; i++) {
                if (data.pieces[i] === "null") {
                    pieces.push(null);
                } else {
                    pieces.push(parseFloat(data.pieces[i]));

                }
            }
            for (var i = 0; i < data.goals.length; i++) {
                if (data.goals[i] === "null") {
                    goals.push(null);
                } else {
                    goals.push(parseFloat(data.goals[i]));

                }
            }
            ApexCharts.exec('graficadeproduccion', 'updateOptions', {
                xaxis: {
                    categories: horas
                },
                series: [
                    { data: goals }, { data: pieces }
                ]
            }, false, true);

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
            panel.find(".panel-body").css("height", lastheight);
        });
    }

    function DowntimesChart() {
        UpdateLabelHours();
        var panel = $('#downtimes_panel');
        panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');

        var MachinesIDs = [];
        $('#dtchart_SelectedMachines option:selected').each(function () {
            MachinesIDs.push(parseInt($(this).val()));
        });

        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");

        var ProductionProcessID = $('#dtchart_process').val();
        var MaterialID = $('#dtchart_materials').val();
        var OperationDate = $('#txt_Date').val();
        var ShiftID = $('#ShiftID').val();

        $.ajax({
            url: '/MFG/Dashboard/GetDowntimesChartData',
            type: 'get',
            traditional: true,
            dataType: "json",
            contextType: "application/json",
            data: { MachinesIDs, ProductionProcessID, MaterialID, OperationDate, ShiftID }
        }).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var series = data.series;
            var YieldGoalValue = 0;

            Chart.defaults.global.legend.labels.usePointStyle = true;

            document.getElementById("DowntimeChart").innerHTML = '&nbsp;';
            document.getElementById("DowntimeChart").innerHTML = '<canvas id="ChartDT" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

            var ctx = document.getElementById("ChartDT").getContext('2d');

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
                                return value + ' Min';
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
                                    max: 110,
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
                    animation: {
                        onComplete: function (animation) {
                            //console.log('grafica cargada..');
                            $('#ChartDT').height($('#DowntimeChart').height());
                        }
                    }
                }
            });

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
        });
    }
    function UpdateDowntimesChart() {
        DowntimesChart();
    }

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

        if (panel.hasClass("panel-fullscreened")) {//MINIMIZaR
            panel.find('.panel-title-box >h3').css("font-size", "100%");
            panel.find('.panel-title-box >span').css("font-size", "100%");

            panel.removeClass("panel-fullscreened").unwrap();
            panel.find(".panel-body").css("height", "214px");
            panel.find(".chart-holder").css("height", "200px");
            panel.find(".chart-holder").css("min-height", "200px");
            panel.find(".panel-fullscreen .fa").removeClass("fa-compress").addClass("fa-expand");

            if (chartplugin !== 'chart.js') {
                if (chartid === 'graficadedefectos') {
                    var VAID = $('#yieldchart_va').val();
                    var Top = $('#topdefects_top').val();
                    var SelectedDefects = $('#SelectedDefects').val();
                    var ProductionProcessID = $('#ProductionProcessID').val();
                    var LineID = $('#Line').val();
                    var DesignID = $('#topdefects_Design').val();
                    var ShiftID = $('#ShiftID').val();
                    var fontSize = '15px';
                    var goalspoints;
                    $.ajax({
                        async: false,
                        type: 'post',
                        url: '/Home/GetTopDefectsGoalsPoints',
                        data: { ProductionProcessID, LineID, VAID, DesignID, SelectedDefects, ShiftID, fontSize },
                        success: function (data) {
                            //console.log(data);
                            goalspoints = data.goalspoints;
                        }
                    });
                    ApexCharts.exec(chartid, 'updateOptions', {
                        dataLabels: {
                            enabled: true,
                            offsetY: 20,
                            style: {
                                fontSize: '15px'
                            },
                            dropShadow: {
                                enabled: true
                            }
                        },
                        xaxis: {
                            labels: {
                                style: {
                                    fontSize: '12px'
                                }
                            }
                        },
                        legend: { fontSize: '12px' },
                        annotations: {
                            points: goalspoints
                        }
                    }, false, true);
                } else if (chartid === 'graficascrap') {
                    var GoalValue = 0;
                    var ProductionProcessID = $('#ProductionProcessID').val();
                    var LineID = $('#Line').val();
                    var ShiftID = $('#ShiftID').val();
                    $.ajax({
                        async: false,
                        type: 'get',
                        url: '/Home/GetScrapGoal',
                        data: { ProductionProcessID, LineID, ShiftID },
                        success: function (data) {
                            //console.log(data);
                            GoalValue = data.GoalValue;
                        },
                    });
                    ApexCharts.exec(chartid, 'updateOptions', {
                        dataLabels: {
                            enabled: true,
                            offsetY: 20,
                            style: {
                                fontSize: '15px'
                            },
                            dropShadow: {
                                enabled: true
                            }
                        },
                        yaxis:
                        {
                            //max: GoalValue,
                            //max: Math.max.apply(null, pieces, GoalValue),
                            min: 0,
                            labels: {
                                style: {
                                    fontSize: '12px'
                                },
                                formatter: function (val, index) {
                                    return parseFloat(val).toFixed(2);
                                }
                            }
                        },
                        xaxis: {
                            labels: {
                                style: {
                                    fontSize: '12px'
                                }
                            }
                        },
                        annotations: {
                            position: 'front',
                            yaxis: [{
                                y: GoalValue,
                                borderColor: '#00E396',
                                label: {
                                    borderColor: '#00E396',
                                    borderWidth: 4,
                                    style: {
                                        color: '#fff',
                                        fontSize: '15px',
                                        background: '#00E396',
                                    },
                                    text: 'Goal: ' + GoalValue,
                                }
                            }]
                        },
                        legend: { fontSize: '12px' }
                    }, false, true);
                } else { //default
                    ApexCharts.exec(chartid, 'updateOptions', {
                        dataLabels: {
                            enabled: true,
                            offsetY: 20,
                            style: {
                                fontSize: '15px'
                            },
                            dropShadow: {
                                enabled: true
                            }
                        },
                        xaxis: {
                            labels: {
                                style: {
                                    fontSize: '12px'
                                }
                            }
                        },
                        yaxis: [
                            {
                                labels: {
                                    style: {
                                        fontSize: '12px'
                                    }
                                }
                            }
                        ],
                        legend: { fontSize: '12px' }
                    }, false, true);
                }

            }

            $(window).resize();

        } else {//MAXIMIZaR
            var hplus = 30; //30 normal

            panel.find(".panel-body,.chart-holder").height($(window).height() - hplus);

            panel.find('.panel-title-box >h3').css("font-size", "150%");
            panel.find('.panel-title-box >span').css("font-size", "120%");

            panel.addClass("panel-fullscreened").wrap('<div class="panel-fullscreen-wrap"></div>');
            panel.find(".panel-fullscreen .fa").removeClass("fa-expand").addClass("fa-compress");

            if (chartplugin !== 'chart.js') {
                if (chartid === 'graficadedefectos') {
                    var VAID = $('#yieldchart_va').val();
                    var Top = $('#topdefects_top').val();
                    var SelectedDefects = $('#SelectedDefects').val();
                    var ProductionProcessID = $('#ProductionProcessID').val();
                    var LineID = $('#Line').val();
                    var DesignID = $('#topdefects_Design').val();
                    var ShiftID = $('#ShiftID').val();
                    var fontSize = '35px';
                    var goalspoints;
                    $.ajax({
                        async: false,
                        type: 'post',
                        url: '/Home/GetTopDefectsGoalsPoints',
                        data: { ProductionProcessID, LineID, VAID, DesignID, SelectedDefects, ShiftID, fontSize },
                        success: function (data) {
                            //console.log(data);
                            goalspoints = data.goalspoints;
                        }
                    });
                    ApexCharts.exec(chartid, 'updateOptions', {
                        dataLabels: {
                            enabled: true,
                            offsetY: 20,
                            style: {
                                fontSize: '35px'
                            },
                            dropShadow: {
                                enabled: true
                            }
                        },
                        xaxis: {
                            labels: {
                                style: {
                                    fontSize: '25px'
                                }
                            }
                        },
                        legend: { fontSize: '25px' },
                        annotations: {
                            points: goalspoints
                        }
                    }, false, true);
                } else if (chartid === 'graficascrap') {
                    var GoalValue = 0;
                    var ProductionProcessID = $('#ProductionProcessID').val();
                    var LineID = $('#Line').val();
                    var ShiftID = $('#ShiftID').val();
                    $.ajax({
                        async: false,
                        type: 'get',
                        url: '/Home/GetScrapGoal',
                        data: { ProductionProcessID, LineID, ShiftID },
                        success: function (data) {
                            //console.log(data);
                            GoalValue = data.GoalValue;
                        }
                    });
                    ApexCharts.exec(chartid, 'updateOptions', {
                        dataLabels: {
                            enabled: true,
                            offsetY: 20,
                            style: {
                                fontSize: '35px'
                            },
                            dropShadow: {
                                enabled: true
                            }
                        },
                        xaxis: {
                            labels: {
                                style: {
                                    fontSize: '25px'
                                }
                            }
                        },
                        yaxis:
                        {
                            //max: Math.max.apply(null, pieces),
                            //max: GoalValue,
                            //max: Math.max.apply(null, pieces, GoalValue),
                            min: 0,
                            labels: {
                                style: {
                                    fontSize: '25px'
                                },
                                formatter: function (val, index) {
                                    return parseFloat(val).toFixed(2);
                                }
                            }
                        },
                        annotations: {
                            position: 'front',
                            yaxis: [{
                                y: GoalValue,
                                borderColor: '#00E396',
                                label: {
                                    borderColor: '#00E396',
                                    borderWidth: 4,
                                    style: {
                                        color: '#fff',
                                        fontSize: '35px',
                                        background: '#00E396',
                                    },
                                    text: 'Goal: ' + GoalValue,
                                }
                            }]
                        },
                        legend: { fontSize: '25px' }
                    }, false, true);
                } else {
                    ApexCharts.exec(chartid, 'updateOptions', {
                        dataLabels: {
                            enabled: true,
                            offsetY: 20,
                            style: {
                                fontSize: '35px'
                            },
                            dropShadow: {
                                enabled: true
                            }
                        },
                        xaxis: {
                            labels: {
                                style: {
                                    fontSize: '25px'
                                }
                            }
                        },
                        yaxis: [
                            {
                                labels: {
                                    style: {
                                        fontSize: '25px'
                                    }
                                }
                            }
                        ],
                        legend: { fontSize: '25px' }
                    }, false, true);
                }

            }
            $(window).resize();

        }
        return false;
    });

    $('#yieldchart_SelectedMachines,#yieldchart_process,#yieldchart_materials').on("change", function () {
        UpdateYieldDefectsChart();
    });
    $('#eerchart_SelectedMachines,#eerchart_process,#eerchart_materials').on("change", function () {
        EERDowntimesChart();
    });
    $('#prodchart_SelectedMachines').on("change", function () {
        UpdateProductionchart();
    });
    $('#dtchart_SelectedMachines,#dtchart_process,#dtchart_materials').on("change", function () {
        UpdateDowntimesChart();
    });
    $('#ShiftID,#txt_Date').on("change", function () {
        UpdateYieldDefectsChart();
        //EERDowntimesChart();
        DowntimesChart();
        UpdateProductionchart();
    });

    function OpenFiltersReportConfirmationModal(ActionName, IsProductionRate, MachineIDs, ProcessID, MaterialID) {
        if (MachineIDs != null) {
            MachineIDs = MachineIDs.join(",");
            MachineIDs = "," + MachineIDs + ",";
        }

        ShowProgressBar();
        $.get("/MFG/Dashboard/GetFiltersReportConfirmationModal", {
            MachineIDs: MachineIDs,
            ProcessID: ProcessID,
            MaterialID: MaterialID,
            ActionName: ActionName,
            IsProductionRate: IsProductionRate
        }).done(function (data) {

            $("#div_FiltersReportConfirmation").html(data);
            $("#mo_FiltersReportConfirmation").modal("show");

            $("#ddl_Machines").selectpicker();
            $("#ddl_Process").selectpicker();
            $("#ddl_Materials").selectpicker();

            $("#Mo_ExcelReport_ShiftID").val($("#ShiftID option:selected").val());
            $("#Mo_ExcelReport_Date").val($("#txt_Date").val());

            if (MachineIDs != null) {
                $("#ddl_Machines option").each(function () {
                    var option = "," + $(this)[0].value + ",";
                    if (MachineIDs.indexOf(option) >= 0) {
                        $(this).prop("selected", true);
                    }
                });
            }

            $("#ddl_Machines").selectpicker("refresh");

            HideProgressBar();
        }).fail(function () {
            HideProgressBar();
        });
    }

    $(document).on("click", "#btn_YieldDefectsExcelReport", function () {
        OpenFiltersReportConfirmationModal("DashboardGasket_YieldDefectsReport", false, $("#yieldchart_SelectedMachines").val(), $("#yieldchart_process option:selected").val(), $("#yieldchart_materials option:selected").val());
    });

    $(document).on("click", "#btn_DowntimesExcelReport", function () {
        OpenFiltersReportConfirmationModal("DashboardGasket_DowntimesReport", false, $("#dtchart_SelectedMachines").val(), $("#dtchart_process option:selected").val(), $("#dtchart_materials option:selected").val());
    });

    $(document).on("click", "#btn_ProductionExcelReport", function () {
        OpenFiltersReportConfirmationModal("DashboardGasket_ProductionReport", true, $("#prodchart_SelectedMachines").val());
    });

    $(document).on("click", "#btn_CloseModalReport", function () {
        $("#mo_FiltersReportConfirmation").modal("toggle");
    });

    YieldDefectsChart();
    //EERDowntimesChart();
    DowntimesChart();
    Productionchart();
}