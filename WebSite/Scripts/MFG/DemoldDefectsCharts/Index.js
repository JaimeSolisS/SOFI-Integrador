// =============================================================================================================================
//  Version: 20191015
//  Author:  Luis Hernandez
//  Created Date: 15 Oct 2019
//  Description:  DemoldDefectsCharts JS
//  Modifications: 
// =============================================================================================================================
function IndexInit(LangResources) {

    function openFullscreen(elem) {
        if (elem != undefined) {
            if (elem.mozRequestFullScreen) { /* Firefox */
                elem.mozRequestFullScreen();
            } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
                elem.webkitRequestFullscreen();
            } else if (elem.msRequestFullscreen) { /* IE/Edge */
                elem.msRequestFullscreen();
            }
        }
    }

    function GetChartData($panel) {
        var category = $panel.data("category");
        var charttype = $panel.data("charttype");

        var ProductionLineIDs = $("#ddl_ProductionLines_" + charttype + "_" + category).val();
        var MoldFamilyIDs = $("#ddl_Families_" + charttype + "_" + category).val();
        var ShiftIDs = $("#ddl_Shifts_" + charttype + "_" + category).val();
        var StartDate = $("#txt_StartDate_" + charttype + "_" + category).val();
        var EndDate = $("#txt_EndDate_" + charttype + "_" + category).val();
        var Design = $("#ddl_design_" + charttype + "_" + category).val();
        var DefectType = category;

        if (ProductionLineIDs != null) {
            ProductionLineIDs = ProductionLineIDs.join(",");
        }

        if (MoldFamilyIDs != null) {
            MoldFamilyIDs = MoldFamilyIDs.join(",");
        }

        if (ShiftIDs != null) {
            ShiftIDs = ShiftIDs.join(",");
        }

        $panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');
        $panel.find(".panel-refresh-layer").width($panel.width()).height($panel.height());
        $panel.addClass("panel-refreshing");
        var GetDataUrl = "";
        if (charttype == "bar") {
            GetDataUrl = "/MFG/DemoldDefectsCharts/GetPercGrossChartData"
        } else {
            GetDataUrl = "/MFG/DemoldDefectsCharts/GetPieChartData"
        }

        $.get(GetDataUrl, {
            ProductionLineIDs,
            MoldFamilyIDs,
            ShiftIDs,
            StartDate,
            EndDate,
            DefectType,
            Design
        }).done(function (data) {
            if (data !== null && data.result.type == "bar") {
                GenerateBarChart(data.result, data.result.type, category);
            } else if (data !== null && data.result.type == "pie") {
                GeneratePieChart(data.result, data.result.type, category);
            }
        }).always(function () {
            $panel.find(".panel-refresh-layer").remove();
            $panel.removeClass("panel-refreshing");
        });


    }

    function GenerateBarChart(v, charttype, category) {
        var Canvas = "canvas_" + charttype + '_' + category;
        var DivCanvas = "_div_canvas_" + charttype + '_' + category;
        var ChartyAxes = [
            {
                id: 'BarAxis',
                type: 'linear',
                position: 'left',
                ticks: {
                    fontSize: 12,
                    beginAtZero: true,
                }
            }];

        Chart.defaults.global.legend.labels.usePointStyle = true;

        $("#" + DivCanvas).empty();
        document.getElementById(DivCanvas).innerHTML = '&nbsp;';
        document.getElementById(DivCanvas).innerHTML = '<canvas id="' + Canvas + '" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

        var ctx = document.getElementById(Canvas).getContext('2d');
        var BarChart = new Chart(ctx, {
            plugins: [ChartDataLabels],
            type: 'bar',
            data: {
                labels: v.label,
                datasets: [{
                    label: 'Defect',
                    data: v.data,
                    backgroundColor: v.backgroundColor,
                    borderColor: [
                        v.backgroundColor
                    ],
                    borderWidth: 1,
                    labelshow: false,
                    FontColor: v.FontColor,
                    Gross: v.Gross
                }]
            },
            options: {
                tooltips: {
                    enabled: true,
                    mode: 'index'
                },
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
                        backgroundColor: function (ctx) {
                            return ctx.dataset.backgroundColor;
                        },
                        clamp: true,
                        borderRadius: 4,
                        color: function (ctx) {
                            return ctx.dataset.FontColor;
                        },
                        font: function (ctx) {
                            var width = ctx.chart.width;
                            var size;
                            size = 20;//Math.round(width / 15);

                            return {
                                size: size,
                                weight: 'bold'
                            };
                        },
                        enabled: true,
                        formatter: function (value, ctx) {
                            var text = value.toFixed(2) + "% \n";
                            return text;
                        },
                    }
                },
                scales: {
                    yAxes: ChartyAxes,
                    //xAxes: [{ stacked: true }],
                    xAxes: [
                        {
                            ticks: {
                                callback: function (label, index, labels) {
                                    if (/\s/.test(label)) {
                                        return label.split(" ");
                                    } else {
                                        return label;
                                    }
                                }
                            },
                            stacked: true
                        }
                    ]
                },
                legend: {
                    display: false,
                    position: 'bottom',
                    labels: {
                        fontSize: 12
                    }
                },
                animation: {
                    onComplete: function (animation) {
                        //ESTA SECCION ES LA QUE AJUSTA EL TAMAÑO DE LA GRÁFICA PARA QUE SE ADAPTE CON LA 
                        //RESOLUCION DE LA PANTALLA(SE EJECUTA CUANDO SE TERMINA DE DIBUJAR LA GRÁFICA)
                        $(Canvas).height($("#" + DivCanvas).height());
                        Resize();
                    }
                }
            }
        });
    }

    function GeneratePieChart(v, charttype, category) {
        var Canvas = "canvas_" + charttype + '_' + category;
        var _Div_Canvas = "_div_canvas_" + charttype + '_' + category;

        Chart.defaults.global.legend.labels.usePointStyle = true;

        document.getElementById(_Div_Canvas).innerHTML = '&nbsp;';
        document.getElementById(_Div_Canvas).innerHTML = '<canvas id="' + Canvas + '" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

        var ctx = document.getElementById(Canvas).getContext('2d');

        var myPieChart = new Chart(ctx, {
            plugins: [ChartDataLabels],
            type: v.type,
            data: {
                labels: v.label,
                datasets: [{
                    data: v.data,
                    backgroundColor: v.backgroundColor,
                    borderColor: [
                        v.borderColor
                    ],
                    borderWidth: 1,
                    FontColor: v.FontColor
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                tooltips: {
                    enabled: false
                },
                plugins: {
                    datalabels: {
                        display: function (context) {
                            //if (context.dataset.data[context.dataIndex] > 0) {
                            //    return 'auto';
                            //}
                            //return false;
                            return 'auto';
                        },
                        backgroundColor: function (ctx) {
                            return "transparent";
                        },
                        formatter: (value, ctx) => {
                            let sum = 0;
                            let dataArr = ctx.chart.data.datasets[0].data;
                            dataArr.map(data => {
                                sum += data;
                            });
                            let percentage = (value * 100 / sum).toFixed(1) + "%";
                            return percentage;
                        },
                        color: function (ctx) {
                            return ctx.dataset.FontColor;
                        },
                        font: function (ctx) {
                            var width = ctx.chart.width;
                            var size;
                            size = 20;//Math.round(width / 15);

                            return {
                                size: size,
                                weight: 'bold'
                            };
                        },
                    }
                }
            }
        });
    }

    //Mostrar u ocultar graficas basado en la seleccion
    $("#ddl_DefectCategories").on("change", function () {
        $(".defect_charts_div").fadeOut();
        var category = $("#ddl_DefectCategories option:selected").text();
        if (category == LangResources.TagAll) {
            $(".defect_charts_div").fadeIn();
        } else {
            $(".defect_charts_div").each(function () {
                if (category === $(this).data("defectcategory")) {
                    $(this).fadeIn();
                }
            });
        }
    });

    $("#btn_DemoldDefectsChartsHelp").click(function () {
        ShowProgressBar();
        $.get("/MFG/DemoldDefectsCharts/GetDemodDefectChartsHelpModal").done(function (data) {
            $("#div_Modal").html(data);
            $("#mo_DemoldDefectsChartsHelp").modal("show");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    //actualizar las graficas basado en el cambio de filtro
    $(document).on("change", ".chart_data_filter", function () {
        var $panel = $(this).closest(".panel");
        GetChartData($panel);
        Resize();
    });

    $(".panel-refresh-chart").on("click", function () {
        var $panel = $(this).parents(".panel");
        GetChartData($panel);
        Resize();
    });

    $(".panel-fullscreen-chart").on("click", function () {
        var panel = $(this).parents(".panel");
        var chartid = panel.data("chartid");

        if (panel.hasClass("panel-fullscreened")) {//MINIMIZaR
            panel.find('.panel-title-box >h3').css("font-size", "100%");
            panel.find('.panel-title-box >span').css("font-size", "100%");
            panel.removeClass("panel-fullscreened").unwrap();
            panel.find(".panel-body").css("height", "220px");
            panel.find(".chart-holder").css("height", "200px");
            panel.find(".chart-holder").css("min-height", "200px");
            panel.find(".panel-fullscreen .fa").removeClass("fa-compress").addClass("fa-expand");

            $(window).resize();
        } else {//MAXIMIZaR
            var hplus = 30; //30 normal
            panel.find(".panel-body,.chart-holder").height($(window).height() - hplus);
            panel.find('.panel-title-box >h3').css("font-size", "150%");
            panel.find('.panel-title-box >span').css("font-size", "120%");
            panel.addClass("panel-fullscreened").wrap('<div class="panel-fullscreen-wrap"></div>');
            panel.find(".panel-fullscreen .fa").removeClass("fa-expand").addClass("fa-compress");

            $(window).resize();
        }
        Resize();
        return false;
    });

    Resize();

    (function SetupDefaultValues() {
        //Plugins
        $(".selectpicker").selectpicker("refresh");
        $('.datepicker').datepicker({
            autoclose: true,
            format: 'yyyy-mm-dd'
        });
        //obtiene datos y dibuja todos los paneles
        $(".DefectChartPanel").each(function () {
            GetChartData($(this));
        });

    }());

    function Resize() {
        var totalWidthResolution = window.screen.width * window.devicePixelRatio;
        var totalHeightResolution = window.screen.height * window.devicePixelRatio;

        if (screen.width < 1601 && screen.width > 1500) {
            $("body").css("zoom", "80%");
            $("#canvas_bar_BU").css("zoom", "70%");
            $("#canvas_bar_DM").css("zoom", "70%");
        } else if ((totalWidthResolution > 1280 && totalWidthResolution < 1285)
            && (totalHeightResolution > 1023 && totalHeightResolution < 1027)) {
            $("#canvas_bar_BU").css("zoom", "80%");
            $("#canvas_bar_DM").css("zoom", "80%");

        }
    }
}