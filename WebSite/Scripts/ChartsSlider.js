// =============================================================================================================================
//  Version: 20190221
//  Author: Carlos Garcia
//  Created Date: 21 feb 2019
//  Description: Contiene funciones JS para la página de ChartSlider
//  Modifications: 
// =============================================================================================================================
function IndexInit(LangResources, DEBUG) {
    var elem = document.documentElement;

    //old_console_log = console.log;
    //console.log = function () {
    //    if (DEBUG) {
    //        old_console_log.apply(this, arguments);
    //    }
    //}


    $('.chartslider-home').on('click', function () {
        window.history.back();
    });

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

    $('.chartslider-pause').on('click', function () {
        if ($(this).hasClass("paused")) {
            $(this).removeClass("paused");
            $(this).find(".fa").removeClass("fa-play").addClass("fa-pause");
            $('#ChartSlider').slick('slickPlay');
        } else {
            $(this).addClass("paused");
            $(this).find(".fa").removeClass("fa-pause").addClass("fa-play");
            $('#ChartSlider').slick('slickPause');
        }

    });

    $('.chartslider-config').on('click', function () {
        $('#ChartSlider').slick('slickPause');
        $('.loading-process-div').show();
        var CompName = $('#ComputerNameHiden').val();
        $.get("/ChartsSlider/GetShowConfig", { CompName }).done(function (data) {
            if (data.ErrorCode === 0) {
                $('#config_modal').html('');
                $('#config_modal').html(data.View);
                //leer "cookie"
                var TimerSetup = localStorage.getItem('TimerSetup');
                if (TimerSetup !== null) {
                    $('#TimerSetup').val(TimerSetup);
                }
                $('.select').selectpicker();
                SetSortable();
                LoadDropzone('.btn_BrowseFile');
                //oculta las opciones que no son del shift y muestra las del shift actual
                $('.ChartsArray').hide();
                $('.SettingShiftID_' + $("#CurrentShift").val()).show();
                $('.loading-process-div').hide();
                $('#m_Config').modal();
            }
        });

    });

    $(document).on('change', '#ConfigShiftList', function () {
        var selectedshiftid = $(this).val();
        $('.ChartsArray').hide();
        $('.SettingShiftID_' + selectedshiftid).show();
    });

    //CLAVE
    $(document).on('click', '#btn_save', function () {
        $('.loading-process-div').show();
        var TimerSetup = $('#TimerSetup').val();
        localStorage.setItem('TimerSetup', TimerSetup);

        $(".removeChart").each(function (index, item) {
            var chartid = $(item).data("entityid");
            var slideIndex = $('#ChartDiv_' + chartid).parent().data("slick-index");
            $('#ChartSlider').slick('slickRemove', slideIndex);
            $('#options_row_chart_' + chartid).remove();
            $.get("/ChartsSlider/DeleteChartConfig", { Chart_SettingID: chartid }).done(function (data) {

            });
        });
        $('.removeChart').remove();

        //guardar en la bd los cambios
        var chartlist = [];
        var optionchartlist = [];
        $(".ChartsArray").each(function (index, item) {
            var Chart_SettingID = $(item).data("entityid");
            var tdlist = $(item).find("td");
            $('.OptionInput_' + Chart_SettingID).find(".form-control").each(function () { //opciones de un Chart_Setting
                var $input = $(this);
                if ($($input).val() !== "") {
                    var opt = {
                        Chart_SetttingDetailID: $input.data("entityid"),
                        OptionValue: $($input).val().toString(),
                        Chart_SettingID: Chart_SettingID
                    }
                    optionchartlist.push(opt);
                }
            });

            var Item = { //un Chart_Setting
                Chart_SettingID,
                Seq: $(tdlist[0]).text(),
                IntervalRefreshTime: $('#RefreshChart_' + Chart_SettingID).val()
                //Options: optionchartlist
            };
            chartlist.push(Item);
        });

        $.post("/ChartsSlider/UpdateChartConfig", { Settings: chartlist, Options: optionchartlist }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            HideProgressBar();
            if (data.ErrorCode === 0) {
                $('#m_Config').modal('toggle');
                location.reload();
            }
        });
    });



    $(document).on('click', '.delete-chart-row', function () {
        var $row = $(this).parent().parent();
        $row.addClass("removeChart");
        $row.hide();

    });
    $(document).on('click', '#add-chart', function () {
        var ChartID = $('#ChartList :selected').val();
        //var ChartName = $('#ChartList :selected').text();
        var CompName = $('#ComputerNameHiden').val();
        var ShiftID = $('#ConfigShiftList :selected').val();
        $.get("/ChartsSlider/AddChartConfig", { ChartID, CompName, ShiftID }).done(function (data) {
            if (data.ErrorCode === 0) {
                //agragar en la tabla de charts
                $("#charts_tbody").append(data.View);
                $('.select').selectpicker();
                SetSortable();
                //LoadDropzone('.btn_BrowseFile'); //se duplica el avento
            }
        });

    });
    $(document).on('click', '.config-chart-row', function () {
        var ChartID = $(this).data("entityid");
        var $row = $(this).parent().parent();
        var $button = $(this);
        if ($button.hasClass("configdisplayed")) {
            $button.removeClass("configdisplayed");
            $('#options_row_chart_' + ChartID).hide();
        } else {
            $button.addClass("configdisplayed");
            $('#options_row_chart_' + ChartID).show();
        }

    });

    function LoadDropzone(form_selector) {
        Dropzone.autoDiscover = false;
        $(form_selector).dropzone({
            addRemoveLinks: true,
            url: "ChartsSlider/UploadFile",
            createImageThumbnails: false,
            maxFiles: 1,
            acceptedFiles: 'image/*',
            init: function () {
                this.on("processing", function (file) {
                    var entityid = $(this.element).data("entityid");
                    this.options.url = "ChartsSlider/UploadFile?SetttingDetailID=" + entityid;
                });
                this.on("sending", function (file) {
                    $('.loading-process-div').show();
                });
                this.on("maxfilesexceeded", function (file) {
                    this.removeFile(data);
                    alert("No more files please!");
                    return false;
                });
                this.on("complete", function (file) {
                    $('.loading-process-div').hide();
                    if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                        this.removeAllFiles(true);
                    }
                    var $textbox = $(this.element.parentElement).find('.form-control');
                    var data = JSON.parse(file.xhr.responseText);
                    notification("", data.ErrorMessage, data.notifyType);
                    if (data.ErrorCode == 0) {
                        $($textbox).val(data.fileName);
                        //$($textbox).attr('placeholder', data.fileName);
                        //if (data.FilePath != '') {
                        //    $($textbox).attr('placeholder', 'dasdas');
                        //    //$($textbox).val(data.FileName);
                        //    //$($textbox).attr('data-url', data.FilePath);
                        //}
                    }

                });
            }
        });
    }
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

    function SetupCarousel() {
        $('.loading-process-div').show();
        var TimerSetup = localStorage.getItem('TimerSetup');
        if (TimerSetup === null) {
            TimerSetup = $('#TimerSetup').val();//10;
        }
        //console.log('timer:' + TimerSetup);
        TimerSetup *= 1000;

        $('#ChartSlider').slick({
            autoplay: false,
            autoplaySpeed: TimerSetup,
            pauseOnFocus: false,
            pauseOnHover: false,
            dots: false,
            prevArrow: false,
            nextArrow: false,
            speed: 500,
            fade: true,
            slidesToShow: 1,
            useTransform: false,
            useCSS: false,
            cssEase: 'linear'
        });

        $('#ChartSlider').slick('slickPlay');
        $('.loading-process-div').hide();
    }

    function YieldDefectsChart(ChartSettingID) {
        if ($("#ChartDiv_" + ChartSettingID).length == 1) { //validar si existe
            var VAID = $('#chrtopt_16_' + ChartSettingID).val();
            var Top = $('#chrtopt_15_' + ChartSettingID).val();
            var ProductionProcessID = $('#chrtopt_30_' + ChartSettingID).val();
            var LineID = $('#chrtopt_31_' + ChartSettingID).val();
            var DesignID = $('#chrtopt_32_' + ChartSettingID).val();
            var ShiftID = $('#chrtopt_35_' + ChartSettingID).val();
            var chartcanvas = "ChartCanvas_" + ChartSettingID;

            // $('.loading-process-div').show();
            $.get("/Home/GetShiftList", { ShiftID }).done(function (shiftdata) {
                var HoursArray;
                HoursArray = shiftdata;

                $.get("/Home/GetYieldDefectsChartData", { ProductionProcessID, LineID, VAID, DesignID, Top, ShiftID }).done(function (data) {
                    var horas = HoursArray.split(",");
                    var series = data.series;
                    var YieldGoalValue = 0;
                    var MaxValue = data.MaxValue;
                    var ChartTitle = '';
                    $.get("/Home/GetYieldGoal", { ProductionProcessID, LineID, VAID, DesignID, ShiftID }).done(function (datayield) {
                        YieldGoalValue = datayield;


                        Chart.defaults.global.legend.labels.usePointStyle = true;
                        if ($('#' + chartcanvas).length) {
                            $('#' + chartcanvas).html('&nbsp;');
                            $('#' + chartcanvas).html('<canvas id="ChartCanvas_' + ChartSettingID + '" ></canvas>');

                            var ctx = document.getElementById(chartcanvas).getContext('2d');
                            try {
                                //$('.loading-process-div').show();
                                var myChart = new Chart(ctx, {
                                    plugins: [ChartDataLabels],
                                    type: 'bar',
                                    data: {
                                        labels: horas,
                                        datasets: series
                                    },
                                    options: {
                                        animation: {
                                            onComplete: function () {
                                                //$('.loading-process-div').hide(); 
                                            }
                                        },
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
                                                                //console.log("el valor del dataset= " + i + "[" + idx + "] es:" + datasets[i].data[idx]);
                                                                if (parseFloat(datasets[i].data[idx]) > parseFloat(max)) {
                                                                    max = datasets[i].data[idx];
                                                                }
                                                            }
                                                            var relacion = (val * 100) / max;
                                                            //console.log("MaxValue:" + max + " Val:" + val);
                                                            //console.log("relacion:" + relacion);
                                                            if (parseFloat(relacion) > 15) { //solo se muestra si la relacion con el max > 15%
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
                                                clamp: true,
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
                                                        var dftsize = Math.round(width / 35); //tamanio font en relacion % de la pantalla, entre mayor mas chico el font       
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
                                                        fontSize: 24,
                                                        max: 110,
                                                        beginAtZero: true
                                                    }
                                                },
                                                {
                                                    id: 'BarAxis',
                                                    type: 'linear',
                                                    position: 'left',
                                                    stacked: true,
                                                    ticks: {
                                                        fontSize: 24,
                                                        max: MaxValue, //2,
                                                        //suggestedMax: 4,
                                                        beginAtZero: true
                                                    }
                                                }],
                                            xAxes: [{
                                                stacked: true,
                                                ticks: {
                                                    fontSize: 24
                                                },
                                            }]
                                        },
                                        legend: {
                                            display: true,
                                            position: 'bottom',
                                            labels: {
                                                fontSize: 24
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
                                                        xAdjust: 390,
                                                        yAdjust: -25,
                                                        position: 'center'
                                                    }
                                                }
                                            ]
                                        }
                                    }
                                });
                            } catch (e) {
                                console.error(e);
                            }
                        }

                    });

                });
            }).always(function () {
                //$('.loading-process-div').hide();
            });

        }
    }

    function ScrapChart(Chart_SettingID) {
        if ($("#ChartDiv_" + Chart_SettingID).length == 1) { //validar si existe
            var ProductionProcessID = $('#chrtopt_30_' + Chart_SettingID).val();
            var LineID = $('#chrtopt_31_' + Chart_SettingID).val();
            var ShiftID = $('#chrtopt_35_' + Chart_SettingID).val();
            var DesignID = $('#chrtopt_32_' + Chart_SettingID).val();

            $.get("/Home/GetShiftList", { ShiftID }).done(function (shiftdata) {
                var HoursArray;
                HoursArray = shiftdata;

                $.get("/Home/GetScrapChartData", { ProductionProcessID, LineID, ShiftID, DesignID }).done(function (data) {
                    var horas = HoursArray.split(",");
                    var GoalValue = data.GoalValue;
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
                            //width: "100%",
                            height: 600,
                            //width: "100%",
                            type: 'line',
                            stacked: true,
                            id: 'ApexChart_' + Chart_SettingID
                        },
                        plotOptions: {
                            bar: {
                                dataLabels: {
                                    position: 'bottom'
                                }
                            }
                        },
                        dataLabels: {
                            enabled: true,
                            formatter: function (val, opt) {
                                return val + "%";
                            },
                            offsetY: 20,
                            style: {
                                fontSize: '35px',
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
                            categories: horas,
                            labels: {
                                style: {
                                    fontSize: '25px'
                                }
                            }
                        },
                        yaxis: [
                            {
                                //max: GoalValue > Math.max.apply(null, pieces) ? GoalValue : Math.max.apply(null, pieces),
                                max: Math.max.apply(null, pieces, GoalValue) * 1.5,
                                min: 0,
                                labels: {
                                    formatter: function (val, index) {
                                        return parseFloat(val).toFixed(2); //val.toFixed(2);
                                    },
                                    style: {
                                        fontSize: '25px'
                                    }
                                }
                            }],
                        annotations: {
                            position: 'front',
                            yaxis: [{
                                y: GoalValue,
                                borderColor: '#00E396',
                                label: {
                                    borderColor: '#00E396',
                                    borderWidth: 7,
                                    style: {
                                        color: '#fff',
                                        fontSize: '30px',
                                        background: '#00E396',
                                    },
                                    text: 'Goal: ' + GoalValue + '%',
                                }
                            }]
                        },
                        legend: { fontSize: '25px' }
                    }
                    var chart = new ApexCharts(
                        document.querySelector("#ChartDiv_" + Chart_SettingID),
                        options
                    );

                    try {
                        if ($("#ChartDiv_" + Chart_SettingID).length == 1) {
                            chart.render();
                        }
                    } catch (e) {
                        console.error(e);
                    }


                });
            });
        }

    }
    function UpdateScrapChart(Chart_SettingID) {
        var activeid = $('.slick-active').data('chartsettingid');
        if (activeid == Chart_SettingID) {
            //console.log('chartUpdated:' + Chart_SettingID);
            var ProductionProcessID = $('#chrtopt_30_' + Chart_SettingID).val();
            var LineID = $('#chrtopt_31_' + Chart_SettingID).val();
            var ShiftID = $('#chrtopt_35_' + Chart_SettingID).val();
            var DesignID = $('#chrtopt_32_' + Chart_SettingID).val();
            $.get("/Home/GetScrapChartData", { ProductionProcessID, LineID, ShiftID, DesignID }).done(function (data) {
                var HoursArray;
                $.get("/Home/GetShiftList", { ShiftID }).done(function (hoursdata) {
                    if (!hoursdata) {
                        //alert(data);
                        return;
                    }
                    HoursArray = hoursdata;

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
                    try {
                        ApexCharts.exec('ApexChart_' + Chart_SettingID, 'updateOptions', {
                            yaxis: [
                                {
                                    //max: GoalValue > Math.max.apply(null, pieces) ? GoalValue : Math.max.apply(null, pieces),
                                    max: Math.max.apply(null, pieces, GoalValue) * 1.5,
                                    min: 0,
                                    labels: {
                                        formatter: function (val, index) {
                                            return parseFloat(val).toFixed(2);
                                        },
                                        style: {
                                            fontSize: '25px'
                                        }
                                    }
                                }],
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
                                            fontSize: '30px',
                                            background: '#00E396',
                                        },
                                        text: 'Goal: ' + GoalValue + '%',
                                    }
                                }]
                            },
                            series: [
                                { data: pieces }
                            ]
                        }, false, true);
                    } catch (e) {
                        console.error(e);
                    }




                });

            });


        }
    }

    function Productionchart(Chart_SettingID) {
        if ($("#ChartDiv_" + Chart_SettingID).length == 1) { //validar si existe
            var ProductionProcessID = $('#chrtopt_30_' + Chart_SettingID).val();
            var LineID = $('#chrtopt_31_' + Chart_SettingID).val();
            var ShiftID = $('#chrtopt_35_' + Chart_SettingID).val();
            var DesignID = $('#chrtopt_32_' + Chart_SettingID).val();

            $.get("/Home/GetShiftList", { ShiftID }).done(function (shiftdata) {
                var HoursArray;
                HoursArray = shiftdata;

                $.get("/Home/GetProductionChartData", { ProductionProcessID, LineID, ShiftID, DesignID }).done(function (data) {
                    var horas = HoursArray.split(",");
                    var goals = [];
                    var pieces = [];
                    var LabelsColor = data.LabelsColor;
                    for (var i = 0; i < data.goals.length; i++) {
                        goals.push(parseFloat(data.goals[i]));
                    }
                    for (var j = 0; j < data.pieces.length; j++) {
                        if (data.pieces[j] === "null") {
                            pieces.push(null);
                        } else {
                            pieces.push(parseFloat(data.pieces[j]));

                        }
                    }

                    var options = {
                        chart: {
                            //width: "100%",
                            //height: "100%",
                            height: 600,
                            //resized: true,
                            type: 'line',
                            id: 'ApexChart_' + Chart_SettingID,
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
                        series: [{
                            name: 'Goal',
                            type: 'area',
                            stroke: {
                                curve: 'smooth'
                            },
                            data: goals
                        }, {
                            name: 'Produced',
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
                            categories: horas,
                            labels: {
                                style: {
                                    fontSize: '25px'
                                }
                            }
                        },
                        markers: {
                            size: 6
                        },
                        dataLabels: {
                            enabled: true,
                            offsetY: 20,
                            formatter: function (val) {
                                if (val === null) {
                                    return "";
                                } else {
                                    return val;
                                }
                            },
                            style: {
                                fontSize: '35px',
                                colors: [LabelsColor]
                            },
                            dropShadow: {
                                enabled: true
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
                        tooltip: {
                            shared: true,
                            intersect: false
                        },
                        legend: { fontSize: '25px' }

                    }
                    var chart = new ApexCharts(
                        document.querySelector("#ChartDiv_" + Chart_SettingID),
                        options
                    );
                    if ($("#ChartDiv_" + Chart_SettingID).length == 1) {
                        chart.render();
                    }
                });
            });
        }
    }
    function UpdateProductionchart(Chart_SettingID) {
        var activeid = $('.slick-active').data('chartsettingid');
        if (activeid == Chart_SettingID) {
            var ProductionProcessID = $('#chrtopt_30_' + Chart_SettingID).val();
            var LineID = $('#chrtopt_31_' + Chart_SettingID).val();
            var ShiftID = $('#chrtopt_35_' + Chart_SettingID).val();
            var DesignID = $('#chrtopt_32_' + Chart_SettingID).val();

            $.get("/Home/GetShiftList", { ShiftID }).done(function (shiftdata) {
                var HoursArray;
                HoursArray = shiftdata;

                $.get("/Home/GetProductionChartData", { ProductionProcessID, LineID, ShiftID, DesignID }).done(function (data) {
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
                    try {
                        ApexCharts.exec('ApexChart_' + Chart_SettingID, 'updateOptions', {
                            xaxis: {
                                categories: horas
                            },
                            series: [
                                { data: goals }, { data: pieces }
                            ]
                        }, false, true);
                    } catch (e) {
                        console.error(e);
                    }


                });
            });
        }

    }

    function ASNchart(Chart_SettingID) {
        if ($("#ChartDiv_" + Chart_SettingID).length == 1) { //validar si existe
            var ProductionProcessID = $('#chrtopt_30_' + Chart_SettingID).val();
            var LineID = $('#chrtopt_31_' + Chart_SettingID).val();
            var ShiftID = $('#chrtopt_35_' + Chart_SettingID).val();

            $.get("/Home/GetShiftList", { ShiftID }).done(function (shiftdata) {
                var HoursArray;
                HoursArray = shiftdata;
                $.get("/Home/GetASNChartData", { ProductionProcessID, LineID, ShiftID }).done(function (data) {
                    var horas = HoursArray.split(",");
                    var goals = [];
                    var pieces = [];
                    var LabelsColor = data.LabelsColor;
                    for (var i = 0; i < data.goals.length; i++) {
                        goals.push(parseFloat(data.goals[i]));
                    }
                    for (var j = 0; j < data.pieces.length; j++) {
                        if (data.pieces[j] === "null") {
                            pieces.push(null);
                        } else {
                            pieces.push(parseFloat(data.pieces[j]));

                        }
                    }

                    var options = {
                        chart: {
                            height: 600,
                            type: 'line',
                            id: 'ApexChart_' + Chart_SettingID,
                        },
                        responsive: [
                            {
                                breakpoint: 200
                            }
                        ],
                        series: [{
                            name: 'Goal',
                            type: 'area',
                            stroke: {
                                curve: 'smooth'
                            },
                            data: goals
                        }, {
                            name: 'Palletization',
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
                            categories: horas,
                            labels: {
                                style: {
                                    fontSize: '25px'
                                }
                            }
                        },
                        markers: {
                            size: 6
                        },
                        dataLabels: {
                            enabled: true,
                            offsetY: 20,
                            formatter: function (val) {
                                if (val === null) {
                                    return "";
                                } else {
                                    return val;
                                }
                            },
                            style: {
                                fontSize: '35px',
                                fontFamily: 'Helvetica, Arial, sans-serif',
                                colors: [LabelsColor]
                            },
                            dropShadow: {
                                enabled: true
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
                        tooltip: {
                            shared: true,
                            intersect: false
                        },
                        legend: { fontSize: '25px' }
                    }
                    var chart = new ApexCharts(
                        document.querySelector("#ChartDiv_" + Chart_SettingID),
                        options
                    );
                    if ($("#ChartDiv_" + Chart_SettingID).length == 1) {
                        chart.render();
                    }

                    return true;
                });
            });
        }

    }
    function UpdateASNchart(Chart_SettingID) {
        var activeid = $('.slick-active').data('chartsettingid');
        if (activeid == Chart_SettingID) {
            var ProductionProcessID = $('#chrtopt_30_' + Chart_SettingID).val();
            var LineID = $('#chrtopt_31_' + Chart_SettingID).val();
            var ShiftID = $('#chrtopt_35_' + Chart_SettingID).val();
            $.get("/Home/GetShiftList", { ShiftID }).done(function (shiftdata) {
                var HoursArray;
                HoursArray = shiftdata;

                $.get("/Home/GetASNChartData", { ProductionProcessID, LineID, ShiftID }).done(function (data) {
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
                    try {
                        ApexCharts.exec('ApexChart_' + Chart_SettingID, 'updateOptions', {
                            xaxis: {
                                categories: horas
                            },
                            series: [
                                { data: goals }, { data: pieces }
                            ]
                        }
                            , false, true);
                    } catch (e) {
                        console.error(e);
                    }

                });
            });

        }

    }

    function TopDefectsChart(Chart_SettingID) {
        var VAID = $('#chrtopt_16_' + Chart_SettingID).val();
        var SelectedDefects = $('#chrtopt_26_' + Chart_SettingID).val();
        var ProductionProcessID = $('#chrtopt_30_' + Chart_SettingID).val();
        var LineID = $('#chrtopt_31_' + Chart_SettingID).val();
        var DesignID = $('#chrtopt_32_' + Chart_SettingID).val();
        var ShiftID = $('#chrtopt_35_' + Chart_SettingID).val();
        var fontsize = "35px";

        $.post("/Home/GetTopDefectsChart", { ProductionProcessID, LineID, VAID, DesignID, SelectedDefects, ShiftID, fontsize }).done(function (data) {
            var defects = data.defects;
            var pieces = data.pieces;
            var colors = data.colors;
            var fontcolors = data.fontcolors;
            var goalspoints = data.goalspoints;
            var MaxGoal = data.MaxGoal;
            var MaxValue = MaxGoal > Math.max.apply(null, pieces) ? MaxGoal : Math.max.apply(null, pieces);

            var options = {
                chart: {
                    height: 600,
                    type: 'bar',
                    id: 'ApexChart_' + Chart_SettingID,
                    events: {
                        mounted: function () {
                            $('.loading-process-div').hide();
                        }
                    }
                },
                responsive: [
                    {
                        breakpoint: 200
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
                    offsetY: 20,
                    formatter: function (val, opt) {
                        return val + "%";
                    },
                    style: {
                        fontSize: '40px',
                        colors: ['black']
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
                            fontSize: '20px'
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
                        },
                        style: {
                            fontSize: '20px'
                        }
                    }
                },
                legend: {
                    fontSize: '50px',

                },
                annotations: {
                    points: goalspoints
                }
            }
            if ($("#ChartDiv_" + Chart_SettingID).length) { //validar si existe
                $('.loading-process-div').show();
                var chart = new ApexCharts(
                    document.querySelector("#ChartDiv_" + Chart_SettingID),
                    options
                );
                chart.render();
            }
        });
    }
    function UpdateTopDefectsChart(Chart_SettingID) {
        var activeid = $('.slick-active').data('chartsettingid');
        if (activeid == Chart_SettingID) {
            //console.log('chartUpdated:' + Chart_SettingID);
            var VAID = $('#chrtopt_16_' + Chart_SettingID).val();
            var SelectedDefects = $('#chrtopt_26_' + Chart_SettingID).val();
            var ProductionProcessID = $('#chrtopt_30_' + Chart_SettingID).val();
            var LineID = $('#chrtopt_31_' + Chart_SettingID).val();
            var DesignID = $('#chrtopt_32_' + Chart_SettingID).val();
            var ShiftID = $('#chrtopt_35_' + Chart_SettingID).val();
            var fontsize = "35px";
            $.post("/Home/GetTopDefectsChart", { ProductionProcessID, LineID, VAID, DesignID, SelectedDefects, ShiftID, fontsize }).done(function (data) {
                var defects = data.defects;
                var pieces = data.pieces;
                var colors = data.colors;
                var goalspoints = data.goalspoints;
                var MaxGoal = data.MaxGoal;
                var MaxValue = MaxGoal > Math.max.apply(null, pieces) ? MaxGoal : Math.max.apply(null, pieces);
                try {
                    ApexCharts.exec('ApexChart_' + Chart_SettingID, 'updateOptions', {
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
                } catch (e) {
                    console.error(e);
                }

            });
        }
    }

    function SetChartImage(Chart_SettingID) {
        if ($("#ChartDiv_" + Chart_SettingID).length) { //validar si existe
            $('.loading-process-div').show();
            $("#ChartDiv_" + Chart_SettingID).css('height', $(window).height() - 100);
            $.get("/ChartsSlider/GetChartImage", { Chart_SettingID }).done(function (data) {
                $('.loading-process-div').hide();
                if (data.ErrorCode == 0) {
                    $("#ChartDiv_" + Chart_SettingID)
                        .append('<img src="' + data.ImagePath.replace("~", "") + '" style="object-fit: contain;height: 100%;width: 100%;" />');
                }
            });
        }
    }
    function SetChartUrl(Chart_SettingID) {
        if ($("#ChartDiv_" + Chart_SettingID).length) { //validar si existe
            $('.loading-process-div').show();
            $("#ChartDiv_" + Chart_SettingID).css('height', $(window).height() - 100);
            $.get("/ChartsSlider/GetChartUrl", { Chart_SettingID }).done(function (data) {
                $('.loading-process-div').hide();
                if (data.ErrorCode == 0) {
                    $("#ChartDiv_" + Chart_SettingID).html('');
                    $("#ChartDiv_" + Chart_SettingID)
                        .append('<iframe src="' + data.UrlPath + '" style="height: 100%;width: 100%;" />');
                }
            });
        }
    }

    function SetSortable() {
        //$("#chart_table tbody").sortable().disableSelection();
        $("#chart_table tbody").sortable({
            stop: function (event, ui) {
                SetNewOrder();
            },
            start: function (event, ui) {
                SetOrder();
            }
        });
    }
    function SetOrder() {
        $('.sortable-tr').each(function (i) {
            $(this).data('order', i + 1);

        });
    }
    function SetNewOrder() {
        $('.sortable-tr').each(function (i) {
            $(this).data('neworder', i + 1);
            $(this).find('td').first().text(i + 1);
        });
    }

    function GetCurrentShift() {
        //console.log('Get Current Shift..');
        $.get("/ChartsSlider/GetCurrentShift").done(function (data) {
            //console.log('Get Shift:' + data.ShiftID + ' Current Shift:' + $("#CurrentShift").val());
            //oculatar/mostrar los slides basado en el shiftid 
            if (parseInt(data.ShiftID) !== parseInt($("#CurrentShift").val())) {
                console.log('Shift Change..');
                location.reload(); //quickfix no muestra nada en el cambio de turno
                var filterClass = '.ChartShiftID_' + data.ShiftID;
                $('#ChartSlider').slick('slickUnfilter');
                $('#ChartSlider').slick('slickFilter', filterClass);
            }
            $("#CurrentShift").val(data.ShiftID);
        });
    }

    function Init() {

        var CompName = localStorage.getItem('ChartSlider_ComputerName');
        if (CompName === null || CompName.length === 0) {
            localStorage.setItem('ChartSlider_ComputerName', $('#ComputerNameHiden').val());
            CompName = $('#ComputerNameHiden').val();
        } else {
            $('#ComputerNameHiden').val(CompName);
        }

        //llamada a un sp para traer la info en base al ComputerName
        console.log('traer la info en base al ComputerName..' + CompName);
        $('.loading-process-div').show();
        $.get("/ChartsSlider/GetShowConfig", { CompName }).done(function (data) {
            if (data.ErrorCode === 0) {
                $('#config_modal').html('');
                $('#config_modal').html(data.View);

                //dibujar los slides, en base a la config traida
                //console.log('dibujar los slides, en base a la config traida..');
                $('.loading-process-div').show();
                $.get("/ChartsSlider/GetChartDivs", { CompName }).done(function (data) {
                    if (data.ErrorCode === 0) {
                        $('#ChartSlider').html('');
                        $('#ChartSlider').html(data.View);

                        //oculta las opciones que no son del shift y muestra las del shift actual
                        $('.ChartsArray').hide();
                        $('.SettingShiftID_' + $("#CurrentShift").val()).show();
                        $('.chartsdivs').hide();
                        $('.ChartShiftID_' + $("#CurrentShift").val()).show();

                        //funcion generica para cada tipo de grafica
                        //recorrer los settings y dibujar cada grafica
                        //console.log('recorrer los settings y dibujar cada grafica..');
                        $('.loading-process-div').show();
                        $("#chart_table").find('tbody tr').each(function (index, item) {
                            var Chart_SettingID = $(item).data("entityid");
                            var chartid = $(item).data("chartid");
                            var IntervalRefreshTime = $('#RefreshChart_' + Chart_SettingID).val() * 1000;
                            switch (chartid) {
                                case 1:
                                    YieldDefectsChart(Chart_SettingID);
                                    setInterval(function () {
                                        YieldDefectsChart(Chart_SettingID);
                                        //console.log('chartUpdated:' + Chart_SettingID);
                                    }, IntervalRefreshTime);

                                    break;
                                case 2:
                                    ScrapChart(Chart_SettingID);
                                    setInterval(function () {
                                        UpdateScrapChart(Chart_SettingID);
                                        //ScrapChart(Chart_SettingID);
                                        //console.log('UpdateScrapChart:' + Chart_SettingID);
                                    }, IntervalRefreshTime);

                                    break;
                                case 3:
                                    Productionchart(Chart_SettingID);
                                    setInterval(function () {
                                        UpdateProductionchart(Chart_SettingID);
                                        //console.log('chartUpdated:' + Chart_SettingID);
                                    }, IntervalRefreshTime);

                                    break;
                                case 4:
                                    ASNchart(Chart_SettingID);
                                    setInterval(function () {
                                        UpdateASNchart(Chart_SettingID);
                                        //console.log('chartUpdated:' + Chart_SettingID);
                                    }, IntervalRefreshTime);

                                    break;
                                case 5:
                                    $('.loading-process-div').show();
                                    TopDefectsChart(Chart_SettingID);
                                    setInterval(function () {
                                        UpdateTopDefectsChart(Chart_SettingID);
                                        //console.log('chartUpdated:' + Chart_SettingID);
                                    }, IntervalRefreshTime);
                                    $('.loading-process-div').hide();
                                    break;
                                case 6:
                                    $('.loading-process-div').show();
                                    SetChartImage(Chart_SettingID);
                                    $('.loading-process-div').hide();
                                    break;
                                case 7:
                                    $('.loading-process-div').show();
                                    SetChartUrl(Chart_SettingID);
                                    setInterval(function () {
                                        SetChartUrl(Chart_SettingID);
                                        //console.log('chartUpdated:' + Chart_SettingID);
                                    }, IntervalRefreshTime);
                                    $('.loading-process-div').hide();
                                    break;
                                default:
                                    break;
                            }
                        });

                        //console.log('SetupCarousel..');
                        SetupCarousel();
                        //mostrar en base al turno
                        GetCurrentShift();
                        var filterClass = '.ChartShiftID_' + $("#CurrentShift").val();
                        $('#ChartSlider').slick('slickFilter', filterClass);
                        //verificar el turno actualiza 6 sec
                        setInterval(function () {
                            GetCurrentShift();
                        }, 6000);
                        console.log('end of config..');

                        $('.loading-process-div').hide();
                    }
                    $('.loading-process-div').hide();
                });
            }
            $('.loading-process-div').hide();
        });
    }

    Init();
};