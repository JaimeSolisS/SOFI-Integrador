// =============================================================================================================================
//  Version: 20190312
//  Author: Carlos Garcia
//  Created Date: 12 mar 2019
//  Description: Contiene funciones JS para la página de IT Charts
//  Modifications: 
// =============================================================================================================================
$(function () {
    function ProjectsChart() {
        var panel = $('#projects_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");


        var Year = $("#proyects_year").val();
        $.get("/Home/GetProjectsChartData", { Year }).done(function (data) {
            var projectscount = data.count;
            var Projects = data.Projects
            //en base al data devuelto llenar datasets y anotations
            var chart_annotations = $.map(data.series, function (p, i) {
                return {
                    type: 'line',
                    //mode: 'vertical',
                    mode: 'horizontal',
                    //scaleID: 'LineAxis',
                    scaleID: 'BarAxis',
                    //value: p.start_date,//valor
                    value: p.seq,
                    borderColor: 'blue',
                    borderWidth: 0.5,
                    borderDash: [1, 6],
                    label: {
                        enabled: true,
                        backgroundColor: '#66bdff',
                        position: "left",
                        fontColor: "#000000",
                        fontSize: ($('#ChartProjects').height() / 2) / 20,
                        enabled: true,
                        content: p.Annotations,
                        yAdjust: 12,
                        xAdjust: p.xAdjust * -($('#myChart').width() / 350),
                    }
                };
            });
            var ylabels = $.map(data.series, function (p, i) {
                return {
                    value: p.seq,
                    name: p.ProjectName
                };
            });
            //console.log(testanotations);
            var chart_datasets = $.map(data.series, function (p, i) {
                return {
                    backgroundColor: p.backgroundColor,
                    borderColor: p.backgroundColor,
                    fill: false,
                    borderWidth: ($('#ChartProjects').height() / 2) / 20,//variable pantalla
                    pointRadius: 0,
                    data: [
                        {
                            x: p.start_date,
                            y: p.seq
                        }, {
                            x: p.end_date,
                            y: p.seq
                        }
                    ]
                };
            });
            //console.log(chart_datasets);

            var ctx = document.getElementById("ChartProjects");

            var scatterChart = new Chart(ctx, {
                plugins: [ChartDataLabels],
                type: 'line',
                data: {
                    datasets: chart_datasets
                },
                options: {
                    plugins: {
                        //Change options for ALL labels of THIS CHART
                        datalabels: {
                            display: false,
                        },
                    },
                    legend: {
                        display: false,
                    },
                    annotation: {
                        annotations: chart_annotations
                    },
                    scales: {
                        xAxes: [{
                            id: 'LineAxis',
                            type: 'time',
                            time: {
                                parser: 'MM/DD/YYYY',
                                tooltipFormat: 'MMM YY'
                            }
                        }],
                        yAxes: [{
                            id: 'BarAxis',
                            position: 'right',
                            gridLines: {
                                color: "rgba(0, 0, 0, 0)",
                            },
                            scaleLabel: {
                                display: false
                            },
                            ticks: {
                                display: false,
                                callback: function (value, index, values) {
                                    return '$' + value;
                                },
                                beginAtZero: true,
                                max: projectscount + 1
                            }
                        }, {
                            id: 'BarAxis2',
                            position: 'left',
                            gridLines: {
                                color: "rgba(0, 0, 0, 0)",
                            },
                            scaleLabel: {
                                display: false
                            },
                            ticks: {
                                display: true,
                                stepSize: 1,
                                fontStyle: 'bold',
                                callback: function (value) {
                                    var proj = ylabels.find(function (pro) {
                                        return pro.value === value;
                                    });
                                    if (typeof proj !== 'undefined') {
                                        //console.log(proj.name);
                                        return proj.name
                                    }
                                },
                                beginAtZero: true,
                                max: projectscount + 1
                            }
                        },

                        ]
                    }
                }
            });

            //console.log($('#ChartProjects').height());
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
        });
    }
    function UpdateProjectsChart() {
        ProjectsChart();
    }
    function TicketsChart() {
        var panel = $('#tickets_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");

        var Year = $("#tickets_year").val();
        $.get("/Home/GetTicketsChartData", { Year }).done(function (data) {
            var MonthsArray;
            var series = data.series;
            $.ajax({
                async: false,
                type: 'get',
                url: '/Home/GetMonthsList',
                data: { Year },
                success: function (data) {
                    //console.log(data);
                    if (!data) {
                        //alert(data);
                        return;
                    }
                    MonthsArray = data;
                },
            });
            var labels = MonthsArray.split(",");

            var options = {
                chart: {
                    width: "100%",
                    height: "100%",
                    resized: true,
                    type: 'bar',
                    stacked: true,
                    events: {
                        mounted: function (chartContext, config) {
                            //console.log('cargado');
                            panel.find(".panel-refresh-layer").remove();
                            panel.removeClass("panel-refreshing");
                        }
                    },
                    toolbar: {
                        show: true
                    },
                    zoom: {
                        enabled: true
                    },
                    id: 'graficatickets'
                },
                responsive: [{
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
                }],
                plotOptions: {
                    bar: {
                        horizontal: false,
                    },
                },
                dataLabels: {
                    enabled: true,
                    dropShadow: {
                        enabled: true
                    }
                },
                series: series,
                xaxis: {
                    type: 'datetime',
                    categories: labels,
                },
                tooltip: {
                    x: {
                        format: 'MMM'
                    },
                },
                legend: {
                    position: 'bottom',
                },
                fill: {
                    opacity: 1
                },
            }

            var chart = new ApexCharts(
                document.querySelector("#TicketsChart"),
                options
            );

            chart.render();

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
        });
    }
    function UpdateTicketsChart() {
        //TicketsChart();
        var panel = $('#tickets_panel');
        var lastheight = panel.find(".panel-body").height();
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var Year = $("#tickets_year").val();
        $.get("/Home/GetTicketsChartData", { Year }).done(function (data) {
            var MonthsArray;
            var series = data.series;
            $.ajax({
                async: false,
                type: 'get',
                url: '/Home/GetMonthsList',
                data: { Year },
                success: function (data) {
                    //console.log(data);
                    if (!data) {
                        //alert(data);
                        return;
                    }
                    MonthsArray = data;
                },
            });
            var labels = MonthsArray.split(",");

            ApexCharts.exec('graficatickets', 'updateOptions', {
                xaxis: {
                    type: 'datetime',
                    categories: labels
                },
                series: series
            }, false, true);

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
            panel.find(".panel-body").css("height", lastheight);
        });

    }
    function NetworkChart() {
        var panel = $('#network_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var Year = $("#services_year").val();
        $.get("/Home/GetServicesChartData", { Year }).done(function (data) {
            var series = data.series;
            var MonthsArray;
            $.ajax({
                async: false,
                type: 'get',
                url: '/Home/GetMonthsList',
                data: { Year },
                success: function (data) {
                    //console.log(data);
                    if (!data) {
                        //alert(data);
                        return;
                    }
                    MonthsArray = data;
                },
            });
            var labels = MonthsArray.split(",");
            var options = {
                chart: {
                    width: "100%",
                    height: "100%",
                    resized: true,
                    type: 'area',
                    events: {
                        mounted: function (chartContext, config) {
                            panel.find(".panel-refresh-layer").remove();
                            panel.removeClass("panel-refreshing");
                        }
                    },
                    id: 'graficanetwork'
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
                    dropShadow: {
                        enabled: true
                    }
                },
                markers: {
                    size: 6
                },
                stroke: {
                    curve: 'smooth'
                },
                series: series,
                xaxis: {
                    type: 'datetime',
                    categories: labels,
                },
                tooltip: {
                    x: {
                        format: 'MMM'
                    },
                }
            }

            var chart = new ApexCharts(
                document.querySelector("#NetworkChart"),
                options
            );
            chart.render();
        });

    }
    function UpdateNetworkChart() {
        //NetworkChart();
        var panel = $('#network_panel');
        var lastheight = panel.find(".panel-body").height();
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");
        var Year = $("#services_year").val();
        $.get("/Home/GetServicesChartData", { Year }).done(function (data) {
            var series = data.series;
            var MonthsArray;
            $.ajax({
                async: false,
                type: 'get',
                url: '/Home/GetMonthsList',
                data: { Year },
                success: function (data) {
                    //console.log(data);
                    if (!data) {
                        //alert(data);
                        return;
                    }
                    MonthsArray = data;
                },
            });
            var labels = MonthsArray.split(",");

            ApexCharts.exec('graficanetwork', 'updateOptions', {
                xaxis: {
                    type: 'datetime',
                    categories: labels
                },
                series: series
            }, false, true);

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
            panel.find(".panel-body").css("height", lastheight);
        });
    }
    function TopTicketsChart() {
        var panel = $('#toptickets_panel');
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");

        var Top = $("#toptickets_top").val();
        var Year = $("#toptickets_year").val();
        var Month = $("#toptickets_month").val();
        $.get("/Home/GetTopTicketsChartData", { Year, Month, Top }).done(function (data) {
            var Types = data.Types;
            var Tickets = data.Tickets;

            var options = {
                chart: {
                    width: "100%",
                    height: "100%",
                    type: 'bar',
                    events: {
                        mounted: function (chartContext, config) {
                            panel.find(".panel-refresh-layer").remove();
                            panel.removeClass("panel-refreshing");
                        }
                    },
                    id: 'graficatoptickets'
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
                        return val;
                    },
                    style: {
                        fontSize: '15px'
                    },
                    dropShadow: {
                        enabled: true
                    }
                },
                series: [{
                    data: Tickets
                }],
                xaxis: {
                    categories: Types,
                    labels: {
                        style: {
                            fontSize: '14px'
                        }
                    }
                },
                yaxis:
                {
                    max: Math.max.apply(null, Tickets),
                    min: 0
                }
            }

            var chart = new ApexCharts(
                document.querySelector("#TopTicketsChart"),
                options
            );

            chart.render();

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
        });
    }
    function UpdateTopTicketsChart() {
        var panel = $('#toptickets_panel');
        var lastheight = panel.find(".panel-body").height();
        panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");

        var Top = $("#toptickets_top").val();
        var Year = $("#toptickets_year").val();
        var Month = $("#toptickets_month").val();
        $.get("/Home/GetTopTicketsChartData", { Year, Month, Top }).done(function (data) {
            var Types = data.Types;
            var Tickets = data.Tickets;

            ApexCharts.exec('graficatoptickets', 'updateOptions', {
                xaxis: {
                    categories: Types
                },
                yaxis:
                {
                    max: Math.max.apply(null, Tickets),
                    min: 0
                },
                series: [{
                    data: Tickets
                }],
            }, false, true);

        }).done(function () {
            panel.find(".panel-refresh-layer").remove();
            panel.removeClass("panel-refreshing");
            panel.find(".panel-body").css("height", lastheight);
        });
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
                if (chartid === 'graficatoptickets') {
                    var Tickets;
                    var Top = $("#toptickets_top").val();
                    var Year = $("#toptickets_year").val();
                    var Month = $("#toptickets_month").val();
                    $.ajax({
                        async: false,
                        type: 'post',
                        url: '/Home/GetTopTicketsChartData',
                        data: { Year, Month, Top },
                        success: function (data) {
                            Tickets = data.Tickets;
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
                        yaxis: [
                            {
                                labels: {
                                    style: {
                                        fontSize: '12px'
                                    }
                                },
                                max: Math.max.apply(null, Tickets),
                                min: 0
                            }
                        ],
                        legend: { fontSize: '12px' },
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

            } else {
                if (chartid === 'graficaproyectos') {
                    ProjectsChart();
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
                if (chartid === 'graficatoptickets') {
                    var Tickets;
                    var Top = $("#toptickets_top").val();
                    var Year = $("#toptickets_year").val();
                    var Month = $("#toptickets_month").val();
                    $.ajax({
                        async: false,
                        type: 'post',
                        url: '/Home/GetTopTicketsChartData',
                        data: { Year, Month, Top },
                        success: function (data) {
                            Tickets = data.Tickets;
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
                        yaxis: [
                            {
                                labels: {
                                    style: {
                                        fontSize: '25px'
                                    }
                                },
                                max: Math.max.apply(null, Tickets),
                                min: 0
                            }
                        ],
                        legend: { fontSize: '25px' },
                    }, false, true);
                } else { //DFT update
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

            } else {
                if (chartid === 'graficaproyectos') {
                    ProjectsChart();
                }
            }
            $(window).resize();

        }
        return false;
    });
    $("#proyects_year").on('change', function () {
        UpdateProjectsChart();
    });
    $("#tickets_year").on('change', function () {
        UpdateTicketsChart();
    });
    $("#services_year").on('change', function () {
        UpdateNetworkChart();
    });
    $("#toptickets_top").on('change', function () {
        UpdateTopTicketsChart();
    });
    $("#toptickets_year").on('change', function () {
        UpdateTopTicketsChart();
    });
    $("#toptickets_month").on('change', function () {
        UpdateTopTicketsChart();
    });


    $(".select").selectpicker();

    ProjectsChart();
    TicketsChart();
    NetworkChart();
    TopTicketsChart();

    LoadGenericCharts("IT");

});




