
// =============================================================================================================================
//  Version: 20190221
//  Author: Carlos Garcia
//  Created Date: 13 feb 2019
//  Description: Contiene funciones JS para la página de Hi Charts
//  Modifications: 
// =============================================================================================================================
$(function () {
    $(".select").selectpicker();

    var SelectedDefects = localStorage.getItem('ChartDashboard_SelectedDefects_' + $('#ProductionProcessID').val());
    if (SelectedDefects === null || SelectedDefects.length === 0) { //es la primera vez
        SelectedDefects = $('#DashboardSelectedDefects').val();
        localStorage.setItem('ChartDashboard_SelectedDefects_' + $('#ProductionProcessID').val(), SelectedDefects);        
        $.each(SelectedDefects.split(","), function (i, e) {
            $("#SelectedDefects option[value='" + e + "']").prop("selected", true);            
        });
        var array = JSON.parse("[" + SelectedDefects + "]");
        $("#SelectedDefects").selectpicker('val', array);
    } else { //selecciona los valores anteriores
        //console.log(SelectedDefects);
        $.each(SelectedDefects.split(","), function (i, e) {
            $("#SelectedDefects option[value='" + e + "']").prop("selected", true);
        });
        var array = JSON.parse("[" + SelectedDefects + "]");
        $("#SelectedDefects").selectpicker('val', array);
    }
    
    YieldDefectsChart();
    ASNchart();
    Productionchart();
    ScrapChart();
    
    TopDefectsChart();
    SetupDefectChart();

    function YieldDefectsChart() {
        var panel = $('#yield_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var VAID = $('#yieldchart_va').val();
        var Top = $('#yieldchart_top').val();
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var DesignID = $('#yieldchart_Design').val();
        var ShiftID = $('#ShiftID').val();
        $.get("/Home/GetYieldDefectsChartData", { ProductionProcessID, LineID, VAID, DesignID, Top, ShiftID  }).done(function (data) {            
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var series = data.series;
            var YieldGoalValue = 0;
            var MaxValue = data.MaxValue;
            $.get("/Home/GetYieldGoal", { ProductionProcessID, LineID, VAID, DesignID, ShiftID }).done(function (datayield) {
                YieldGoalValue = datayield;

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
                                        // validar el % de relacion con el max
                                        if (context.dataset.type == "bar") {
                                            var idx = context.dataIndex; //indice actual
                                            var datasets = context.chart.data.datasets;//todos los datasets
                                            var val = context.dataset.data[idx]; //ctx.dataset serie actual                                            
                                            var max = 0;
                                            for (i = 1; i < datasets.length; ++i) {
                                                console.log("el valor del dataset= " + i + "[" + idx + "] es:" + datasets[i].data[idx]);
                                                if (parseFloat(datasets[i].data[idx]) > parseFloat(max)) {
                                                    max = datasets[i].data[idx];
                                                }
                                            }
                                            var relacion = (val * 100) / max;
                                            console.log("MaxValue:" + max +" Val:"+val);
                                            console.log("relacion:" + relacion);
                                            if (parseFloat(relacion) > 16) { //solo se muestra si la relacion con el max > 16%
                                                return 'auto';
                                            } else {
                                                return false;
                                            }
                                        }
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
                                clamp:true,
                                borderRadius: 4,
                                color: function (context) {
                                    return context.dataset.pointBorderColor;
                                },
                                font: function (context) {
                                    var width = context.chart.width;
                                    var size;
                                    if (context.dataset.type == "line") {
                                        size = Math.round(width / 68);
                                    } else if (context.dataset.type == "bar") {
                                        var idx = context.dataIndex; //indice actual
                                        var datasets = context.chart.data.datasets;//todos los datasets
                                        var val = context.dataset.data[idx]; //ctx.dataset serie actual
                                        var max = 0;
                                        //calculo de valor maximo en el eje y
                                        for (i = 1; i < datasets.length; ++i) {
                                            if (parseFloat(datasets[i].data[idx]) > parseFloat(max)) {
                                                max = datasets[i].data[idx];
                                            }
                                        }
                                        //relacion del valor que se dibuja vs valor maximo
                                        var relacion = (val * 100) / max;
                                        var dftsize = Math.round(width / 38); //tamanio font en relacion % de la pantalla, entre mayor mas chico el font       
                                        size = (relacion / 100) * dftsize;    
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
                                        return 'end';
                                    } else {
                                        return 'center';
                                    }
                                }
                            }
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
                                    }
                                },
                                {
                                    id: 'BarAxis',
                                    type: 'linear',
                                    position: 'left',
                                    stacked: true,
                                    ticks: {
                                        fontSize: 12,
                                        max: MaxValue,
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
                                        //cornerRadius: 4,
                                        content: 'Goal: ' + YieldGoalValue + '%',
                                        fontSize: 32,
                                        enabled: true,
                                        xPadding: 8,
                                        yPadding: 5,
                                        xAdjust: 500,
                                        yAdjust: -25,
                                        //position: "top",
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


            });


            
        }).done(function () {
           panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
        });
    }
    function UpdateYieldDefectsChart() {
        YieldDefectsChart();
    }

    function ScrapChart() {
        var panel = $('#scrap_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var ShiftID = $('#ShiftID').val();
        var DesignID = $('#scrapchart_Design').val();
        $.get("/Home/GetScrapChartData", { ProductionProcessID, LineID, DesignID, ShiftID }).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var GoalValue = data.GoalValue; //= data.goals.map(Number);
            var pieces = [];

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
                    type: 'line',
                    stacked: true,
                    events: {
                        mounted: function (chartContext, config) {
                            //console.log('cargado');
                            panel.find(".panel-refresh-layer").remove();
                            panel.removeClass("panel-refreshing");
                        }
                    },
                    id:'graficascrap'
                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            position: 'bottom'
                        }
                    }
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
                dataLabels: {
                    enabled: true,
                    formatter: function (val, opt) {
                        return val + "%";
                    },
                    offsetY: 20,
                    style: {
                        fontSize: '15px',
                        colors: ['#ffffff']
                    },
                    dropShadow: {
                        enabled: true
                    }
                },
                series: [
                    {
                        name: 'Scrap',
                        type: 'column',
                        data: pieces
                    }
                ],
                stroke: {
                    width: [1, 1, 4]
                },
                xaxis: {
                    categories : horas
                },
                yaxis:
                    {
                        max: Math.max.apply(null, pieces,GoalValue) *1.5,
                        min: 0,
                        labels: {
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
                                fontSize: '15px',
                                background: '#00E396',
                            },
                            text: 'Goal: ' + GoalValue +'%',
                        }
                    }]
                },
            }

            var chart = new ApexCharts(
                document.querySelector("#ScrapChart"),
                options
            );


            chart.render();
        });
    }
    function UpdateScrapChart(){
        var panel = $('#scrap_panel');
        var lastheight = panel.find(".panel-body").height();
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var ShiftID = $('#ShiftID').val();
        var DesignID = $('#scrapchart_Design').val();
        $.get("/Home/GetScrapChartData", { ProductionProcessID, LineID, DesignID, ShiftID }).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var pieces = [];
            var GoalValue = data.GoalValue;
            for (var i = 0; i < data.pieces.length; i++) {
                if (data.pieces[i] === "null") {
                    pieces.push(null);
                } else {
                    pieces.push(parseFloat(data.pieces[i]));

                }
            }


            ApexCharts.exec('graficascrap', 'updateOptions', {
                yaxis:
                    {
                        //max: GoalValue,//Math.max.apply(null, pieces),
                        max: Math.max.apply(null, pieces, GoalValue) * 1.5,
                        min: 0,
                        labels: {
                            formatter: function (val, index) {
                                return parseFloat(val).toFixed(2);
                            }
                        }
                    },
                xaxis: {
                    categories: horas
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
                            text: 'Goal: ' + GoalValue + '%',
                        }
                    }]
                },
                series: [
                    { data: pieces }
                ]
            },false, true);

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
            panel.find(".panel-body").css("height", lastheight);
        });
    }

    function Productionchart() {
        var panel = $('#prod_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var ShiftID = $('#ShiftID').val();
        var DesignID = $('#production_Design').val();
        $.get("/Home/GetProductionChartData", { ProductionProcessID, LineID, ShiftID, DesignID }).done(function (data) {
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
                    id:'graficadeproduccion'
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
                    //data: [0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100]
                    data: goals
                }, {
                    name: 'Producido',
                    type: 'line',
                    stroke: {
                        curve: 'straight'
                    },
                    data:pieces
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
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var ShiftID = $('#ShiftID').val();
        var DesignID = $('#production_Design').val();
        $.get("/Home/GetProductionChartData", { ProductionProcessID, LineID, ShiftID, DesignID }).done(function (data) {
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
                series:[
                    { data: goals }, { data: pieces }
                ]
                },false, true);

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
            panel.find(".panel-body").css("height", lastheight);
        });
    }

    function ASNchart() {
        var panel = $('#asn_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var ShiftID = $('#ShiftID').val();
        $.get("/Home/GetASNChartData", { ProductionProcessID, LineID, ShiftID}).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var goals=[];
            var pieces =[];
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
                    id: 'graficadeasn'
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
                    //data: [800, 1800, 2800, 3800, 4800, 5800, 6800, 7800, 8800, 9800, 10800, 11800, 12800, 13800]
                    data: goals
                }, {
                    name: 'Producido',
                    type: 'line',
                    stroke: {
                        curve: 'straight'
                    },
                    data:pieces
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
                        fontFamily: 'Helvetica, Arial, sans-serif,Bold',
                        colors: [LabelsColor]
                    },
                    dropShadow: {
                        enabled: true
                    }
                },
                yaxis: [
                    {
                        title: {
                            text: 'Producido',
                        },
                    }
                ],
                tooltip: {
                    shared: true,
                    intersect: false
                }
            }

            var chart = new ApexCharts(
                document.querySelector("#ASNChart"),
                options
            );


            chart.render();
        });

    }
    function UpdateASNchart() {
        var panel = $('#asn_panel');
        var lastheight = panel.find(".panel-body").height();
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var ShiftID = $('#ShiftID').val();
        $.get("/Home/GetASNChartData", { ProductionProcessID, LineID, ShiftID }).done(function (data) {
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
            ApexCharts.exec('graficadeasn', 'updateOptions', {
                xaxis: {
                    categories: horas
                },
                series:[
                    { data: goals }, { data: pieces }
                ]
            }
            ,false, true);

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
            panel.find(".panel-body").css("height", lastheight);
        });
    }

    function TopDefectsChart() {
        //console.log('cominenza cargado...');
        var panel = $('#defect_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var VAID = $('#yieldchart_va').val();
        var Top = $('#topdefects_top').val();
        var SelectedDefects = $('#SelectedDefects').val();
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var DesignID = $('#topdefects_Design').val();
        var ShiftID = $('#ShiftID').val();
        var fontsize = "15px";
        $.post("/Home/GetTopDefectsChart", { ProductionProcessID, LineID, VAID, DesignID, SelectedDefects, ShiftID, fontsize }).done(function (data) {
            var defects = data.defects;
            var pieces = data.pieces;
            var colors = data.colors;
            //console.log(colors);
            var fontcolors = data.fontcolors;
            //console.log(fontcolors);
            var goalspoints = data.goalspoints;
            var MaxGoal = data.MaxGoal;
            var MaxValue = MaxGoal > Math.max.apply(null, pieces) ? MaxGoal : Math.max.apply(null, pieces);
            var LabelsColor = data.LabelsColor;
            var options = {
                chart: {
                    width:  "100%",
                    height: "100%",
                    type: 'bar',
                    events: {
                        dataPointSelection: function (event, chartContext, config) {
                            UpdateDefectChart(config.w.globals.labels[config.dataPointIndex]);
                        },
                        mounted: function (chartContext, config) {
                            panel.find(".panel-refresh-layer").remove();
                            panel.removeClass("panel-refreshing");
                        }
                    },
                    id: 'graficadedefectos'
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
                colors: colors,
                plotOptions: {
                    bar: {
                        columnWidth: '45%',
                        distributed: true,
                        dataLabels: {
                            position: 'bottom'
                        }
                    }
                },
                dataLabels: {
                    enabled: true,
                    offsetY: 10,
                    formatter: function (val, opt) {
                        return val + "%";
                    },
                    style: {
                        fontSize: '15px'  ,
                        //colors: fontcolors
                        colors: [LabelsColor]
                    },
                    dropShadow: {
                        enabled: true
                    }
                },
                series: [{
                    data: pieces
                }],
                xaxis: {
                    categories: defects,
                    labels: {
                        style: {
                            colors: colors,
                            fontSize: '14px'
                        }
                    }
                },
                yaxis: 
                    {
                        max: MaxValue + (MaxValue / 2),
                        min: 0,
                        labels: {
                            formatter: function (val, index) {
                                return parseFloat(val).toFixed(2);
                            }
                        }
                    },
                annotations: {                 
                    points: goalspoints  
                }
            }

            var chart = new ApexCharts(
                document.querySelector("#TopDefectsChart"),
                options
            );

            chart.render();
        });
    }
    function UpdateTopDefectsChart() {

        var panel = $('#defect_panel');
        var lastheight = panel.find(".panel-body").height();
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var VAID = $('#yieldchart_va').val();
        var Top = $('#topdefects_top').val();
        var SelectedDefects = $('#SelectedDefects').val();
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var DesignID = $('#topdefects_Design').val();
        var ShiftID = $('#ShiftID').val();
        var fontsize = "15px";
        $.post("/Home/GetTopDefectsChart", { ProductionProcessID, LineID, VAID, DesignID, SelectedDefects, ShiftID, fontsize }).done(function (data) {
            var defects = data.defects;
            var pieces = data.pieces;
            var colors = data.colors;
            var goalspoints = data.goalspoints;
            var MaxGoal = data.MaxGoal;
            var MaxValue = MaxGoal > Math.max.apply(null, pieces) ? MaxGoal : Math.max.apply(null, pieces);

            ApexCharts.exec('graficadedefectos', 'updateOptions', {
                colors: colors,
                series: [{
                    data: pieces
                }],
                xaxis: {
                    categories: defects
                },
                yaxis: {
                    max: MaxValue + (MaxValue / 2),
                    min: 0,
                    labels: {
                        formatter: function (val, index) {
                            return parseFloat(val).toFixed(2);
                        }
                    }
                },
                annotations: {
                    points: goalspoints
                }
            }, false, true);
        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
            panel.find(".panel-body").css("height", lastheight);
        });
    }
   
    function SetupDefectChart() {
        var options = {
            chart: {
                width: "100%",
                height: "600px",
                type: 'bar',
                id: 'graficadefecto'
            },
            responsive: [
                {
                    breakpoint: 200,
                    options: {
                        plotOptions: {
                            bar: {
                                horizontal: false
                            }
                        }
                    }
                }
            ],
            dataLabels: {
                position: 'bottom',
                enabled: true,
                formatter: function (val, opt) {
                    return val + "%";
                },
                offsetY: 20,
                style: {
                    fontSize: '35px'
                },
                dropShadow: {
                    enabled: true
                }
            },
            series: [
                {
                    type: 'column',
                    data: [null]
                }
            ],
            stroke: {
                width: 4
            },
            xaxis: {
                hideOverlappingLabels: true,
                categories: [1]
            },
            markers: {
                size: 5
            }
        }

        var chart = new ApexCharts(
            document.querySelector("#DefectChart"),
            options
        );

        chart.render();
    }
    function UpdateDefectChart(Defect) {
        var panel = $('#onedefect_panel');
        var head = panel.find(".panel-heading");
        var body = panel.find(".panel-body");
        var footer = panel.find(".panel-footer");
        var hplus = 30;

        panel.find(".panel-body,.chart-holder").height($(window).height() - hplus);
        panel.addClass("panel-fullscreened").wrap('<div class="panel-fullscreen-wrap"></div>');
        panel.find(".panel-fullscreen .fa").removeClass("fa-expand").addClass("fa-compress");

        panel.find('.panel-title-box >h3').css("font-size", "150%");
        panel.find('.panel-title-box >span').css("font-size", "150%");

        $(window).resize();

        $('#defect-name').text(Defect);
        
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        
        var ProductionProcessID = $('#ProductionProcessID').val();
        var LineID = $('#Line').val();
        var VA = $('#topdefects_va').val();
        var DesignID = $('#topdefects_Design').val();
        var ShiftID = $('#ShiftID').val();
        $.get("/Home/GetDefectChart", { ProductionProcessID, LineID, VA, DesignID, Defect, ShiftID  }).done(function (data) {
            var HoursArray = $('#HoursArray').val();
            var horas = HoursArray.split(",");
            var defects = [];
            var color = data.defectColor;
            var GoalValue = data.GoalValue;

            for (var i = 0; i < data.defects.length; i++) {
                if (data.defects[i] === "null") {
                    defects.push(null);
                } else {
                    defects.push(parseFloat(data.defects[i]).toFixed(2));
                }
            }

            ApexCharts.exec('graficadefecto', 'updateOptions', {
                colors: color,
                series: [{
                    data: defects
                }],
                xaxis: {
                    categories: horas
                },
                yaxis: {
                    max: Math.max.apply(null, defects),
                    min: 0,
                    labels: {
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
            }, false, true);


        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");   
           
            });
        $('#onedefect_panel').show();
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
                }else { //default
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

            if (chartid === 'graficadeyield') {
                hplus = 130;
            }

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
                                min:0,
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
                }else {
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

    //solo en un defecto
    $(".panel-unfullscreen-chart").on("click", function () {
        var panel = $('#onedefect_panel');
        panel.removeClass("panel-fullscreened").unwrap();
        panel.find(".panel-body,.chart-holder").css("height", "200px");
        panel.find(".panel-body,.chart-holder").css("min-height", "200px");
        panel.find(".panel-fullscreen .fa").removeClass("fa-compress").addClass("fa-expand");

        $(window).resize();
        $('#onedefect_panel').hide();
    });

    $('#yieldchart_va').on("change", function () {
        UpdateYieldDefectsChart();
    });
    $('#yieldchart_top').on("change", function () {
        UpdateYieldDefectsChart();
    });
    $('#yieldchart_Design').on("change", function () {
        UpdateYieldDefectsChart();
    });

    $('#topdefects_va').on('change', function () {
        UpdateTopDefectsChart();
    });
    $('#topdefects_top').on('change', function () {
        UpdateTopDefectsChart();
    });
    $('#topdefects_Design').on('change', function () {
        UpdateTopDefectsChart();
    });

    $("#SelectedDefects").on("change", function () {
        UpdateTopDefectsChart();      
        //guarda la info de la ultima seleccion
        localStorage.setItem('ChartDashboard_SelectedDefects_'+ $('#ProductionProcessID').val(), $('#SelectedDefects').val().toString());
    });

    $('#ShiftID').on("change", function () {
        $.get("/Home/GetShiftList",
            { ShiftID: $('#ShiftID').val() }
        ).done(function (data) {
            $('#HoursArray').val(data);
            UpdateYieldDefectsChart();
            UpdateScrapChart();
            UpdateProductionchart();
            UpdateASNchart();
            UpdateTopDefectsChart();
        });
    });

    $('#Line').on("change", function () {
        UpdateYieldDefectsChart();
        UpdateScrapChart();
        UpdateProductionchart();
        UpdateASNchart();
        UpdateTopDefectsChart();

    });

    $('#scrapchart_Design').on('change', function () {
        UpdateScrapChart();
    });
    $('#production_Design').on('change', function () {
        UpdateProductionchart();
    });
});