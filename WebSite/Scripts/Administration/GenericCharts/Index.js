// =============================================================================================================================
//  Version: 20191018
//  Author:  Luis Hernandez
//  Created Date: 10 September 2019
//  Description:  Generic Charts JS
//  Modifications: Clipboard copy function
// =============================================================================================================================

function IndexInit(LangResources) {
    function ClearFiltersDiv() {
        $("#div_Main_DataChartAdminFilters").css("display", "none");
        $("#div_DataChartAdminFilters").empty();
    }

    function GeneratePreviewChart(xArray, ChartType, ChartEntities, panel, MaxValue) {
        if (ChartType === "stacked") {
            ChartType = "bar";
        }
        var panel = $('#consumption_by_day_panel');
        panel.append('<div class="panel-refresh-layer"><img src="/Content/img/loaders/default.gif"/></div>');
        panel.find(".panel-refresh-layer").width(panel.width()).height(panel.height());
        panel.addClass("panel-refreshing");

        var xValues = xArray.split(",");
        var series = ChartEntities;
        var ChartyAxes = [
            {
                id: 'LineAxis',
                type: 'linear',
                position: 'right',
                ticks: {
                    fontSize: 12,
                    max: parseFloat(MaxValue) * 1.16,
                    beginAtZero: true
                },
                display: false
            },
            {
                id: 'BarAxis',
                type: 'linear',
                position: 'left',
                ticks: {
                    fontSize: 12,
                    max: parseFloat(MaxValue) * 1.16,
                    beginAtZero: true
                },
                display:true
            }];
        if (ChartType !== "line") {
            ChartyAxes = [
                {
                    id: 'BarAxis',
                    type: 'linear',
                    position: 'left',
                    ticks: {
                        fontSize: 12,
                        max: parseFloat(MaxValue) * 1.16,
                        beginAtZero: true
                    }
                }];
        }

        Chart.defaults.global.legend.labels.usePointStyle = true;

        var ctx = document.getElementById("GenericChartPreview").getContext('2d');
        var PreviewChart = new Chart(ctx, {
            plugins: [ChartDataLabels],
            type: 'bar',
            data: {
                labels: xValues,
                datasets: series
            },
            options: {
                tooltips: {
                    enabled: true,
                    mode: 'single'
                },
                responsive: true,
                plugins: {
                    // Change options for ALL labels of THIS CHART
                    datalabels: {
                        display: function (ctx) {
                            if (ctx.dataset.labelshow && ctx.dataset.data[ctx.dataIndex] > 0) {
                                return 'auto';
                            }
                            return false;
                        },
                        backgroundColor: function (ctx) {
                            return ctx.dataset.labelfontbgcolor;
                        },
                        clamp: true,
                        borderRadius: 4,
                        color: function (ctx) {
                            return ctx.dataset.labelfontcolor;
                        },
                        font: function (ctx) {
                            var width = ctx.chart.width;
                            var size;
                            size = Math.round(width / ctx.dataset.labelfontsize);
                            //if (ctx.dataset.type === "line") {
                            //    size = Math.round(width / ctx.dataset.labelfontsize);
                            //} else {
                            //    size = Math.round(width / 50);
                            //}
                            return {
                                size: size,
                                weight: 'bold'
                            };
                        },
                        formatter: function (value, ctx) {
                            value = GetFormatedData(value, ctx.dataset.Format);
                            return value;
                        },
                        anchor: function (ctx) {
                            return 'center';
                        },
                        rotation: function (ctx) {
                            return ctx.dataset.labelrotation;
                        }
                    }
                },
                scales: {
                    yAxes: ChartyAxes,
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
                        //ESTA SECCION ES LA QUE AJUSTA EL TAMAÑO DE LA GRÁFICA PARA QUE SE ADAPTE CON LA RESOLUCION DE LA PANTALLA
                        $('#GenericChartPreview').height($('#_Div_GenericChartPreview').height());

                    }
                }
            }
        });

        panel.find(".panel-refresh-layer").remove();
        panel.removeClass("panel-refreshing");

    }

    function SaveGenericChartData() {
        var GenericChartDataEntityData = [];
        var FitlerInfo = ",,";
        var GenericChartID = $("#ddl_ChartsOfArea option:selected").val();
        $(".table_data_row").each(function () {
            var value = $(this).find("td:nth-child(1)").text();
            if (value !== "") {
                var GenericChartDataEntity = {
                    //GenericChartDataID: null,
                    GenericChartHeaderDataID: $("#GenericChartHeaderDataIDLoaded").val(),
                    Field1: null,
                    Field2: null,
                    Field3: null,
                    Field4: null,
                    Field5: null,
                    Field6: null,
                    Field7: null,
                    Field8: null,
                    Field9: null,
                    Field10: null,
                    Field11: null,
                    Field12: null,
                    Field13: null,
                    Field14: null,
                    Field15: null,
                    Field16: null,
                    Field17: null,
                    Field18: null,
                    Field19: null,
                    Field20: null
                };

                //GenericChartDataEntity.GenericChartHeaderDataID = $("#GenericChartHeaderDataID").val();

                var HeadersCount = $("#tbl_GenericChartData th").length;
                for (var i = 1; i <= HeadersCount; i++) {
                    value = $(this).find("td:nth-child(" + i + ")").find("input").val();
                    if (value === null || value === "") {
                        value = 0;
                    }

                    switch (i) {
                        case 1:
                            GenericChartDataEntity.Field1 = $(this).find("td:nth-child(" + i + ")").text();
                            break;
                        case 2:
                            GenericChartDataEntity.Field2 = value;
                            break;
                        case 3:
                            GenericChartDataEntity.Field3 = value;
                            break;
                        case 4:
                            GenericChartDataEntity.Field4 = value;
                            break;
                        case 5:
                            GenericChartDataEntity.Field5 = value;
                            break;
                        case 6:
                            GenericChartDataEntity.Field6 = value;
                            break;
                        case 7:
                            GenericChartDataEntity.Field7 = value;
                            break;
                        case 8:
                            GenericChartDataEntity.Field8 = value;
                            break;
                        case 9:
                            GenericChartDataEntity.Field9 = value;
                            break;
                        case 10:
                            GenericChartDataEntity.Field10 = value;
                            break;
                        case 11:
                            GenericChartDataEntity.Field11 = value;
                            break;
                        case 12:
                            GenericChartDataEntity.Field12 = value;
                            break;
                        case 13:
                            GenericChartDataEntity.Field13 = value;
                            break;
                        case 14:
                            GenericChartDataEntity.Field14 = value;
                            break;
                        case 15:
                            GenericChartDataEntity.Field15 = value;
                            break;
                        case 16:
                            GenericChartDataEntity.Field16 = value;
                            break;
                        case 17:
                            GenericChartDataEntity.Field17 = value;
                            break;
                        case 18:
                            GenericChartDataEntity.Field18 = value;
                            break;
                        case 19:
                            GenericChartDataEntity.Field19 = value;
                            break;
                        case 20:
                            GenericChartDataEntity.Field20 = value;
                            break;
                    }
                }

                GenericChartDataEntityData.push(GenericChartDataEntity);
            }
        });

        $(".filters_of_chart").each(function () {
            if ($(this).val() !== "") {
                if ($(this).data("filtertypename") === "LIST") {
                    FitlerInfo = FitlerInfo + "," + $(this).data("entityid") + "=" + $(this).find("option:selected").val();
                } else {
                    FitlerInfo = FitlerInfo + "," + $(this).val();
                }
            }

            FitlerInfo = FitlerInfo.replace(",,,", "");

        });

        $.post("/Administration/GenericCharts/SaveDataChart", { FitlerInfo, GenericChartID, GenericChartDataEntityData }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            $("#ContainsData").val(true);
        });
    }

    function SetupxEditable() {
        $('.x-editable').editable({
            success: function (response, newValue) {
                var GenericChartAxisID = $(this).closest("th").data("identifier");
                var AxisName = newValue;
                $.post("/Administration/GenericChartsAxis/UpdateAxisName", { GenericChartAxisID, AxisName }).done(function () {

                });
            }
        });
    }


    $(".selectpicker").selectpicker();

    $(document).on("change", ".filters_of_chart", function () {
        var ValuesFromFilters = "";
        var isAbleToShow = true;

        var GenericChartID = $("#ddl_ChartsOfArea option:selected").val();
        $(".filters_of_chart").each(function () {
            if ($(this).val() !== "") {
                if ($(this).val() !== 0) {
                    ValuesFromFilters = ValuesFromFilters + "," + $(this).data("entityid") + "=" + $(this).val();
                } else {
                    isAbleToShow = false;
                }
            }
        });

        if (isAbleToShow) {
            ValuesFromFilters = ValuesFromFilters.replace(",", "");
            $('.loading-process-div').show();
            $.get("/Administration/GenericCharts/Search", { GenericChartID, ValuesFromFilters }).done(function (data) {
                $("#div_DataChartAdminFieldsTable").html(data);
                $("#hideIfNotFilter").css("display", "block");
                $("#data_zone").css("display", "block");
                $('.loading-process-div').hide();
                SetupxEditable();
            });
        }
    });

    //carga las areas del catalogo
    $("#ddl_ChartsAreas").on("change", function () {
        $('.loading-process-div').show();
        ClearFiltersDiv();
        var ChartAreaID = $("#ddl_ChartsAreas option:selected").val();
        $.get("/Administration/GenericCharts/GetChartsByAreaList", { ChartAreaID }).done(function (data) {
            if (data.ErrorCode === 0) {
                var datos = data.ChartsOfAreaList;

                $("#ddl_ChartsOfArea").empty();
                $("#ddl_ChartsOfArea").append("<option value='0'>" + LangResources.chsn_SelectOption + "</option>");
                //$("#ddl_family").append("<option value='a' class='new-family'>--" + LangResources.lbl_NewFamily + "--</option>");

                $.each(datos, function () {
                    $("#ddl_ChartsOfArea").append("<option value='" + this["Value"] + "'>" + this["Text"] + "</option>");
                });

                $("#ddl_ChartsOfArea").selectpicker("refresh");
            }
            $('.loading-process-div').hide();
        });
    });

    $("#ddl_ChartsOfArea").on("change", function (e) {
        e.stopPropagation();
        ClearFiltersDiv();
        $('.loading-process-div').show();
        var GenericChartID = $("#ddl_ChartsOfArea option:selected").val();
        var GenericChartHeaderDataID = $("#GenericChartHeaderDataID").val();
        var OptionSelected = 0;
        if (GenericChartID !== 0) {

            //Carga los fltros
            $.get("/Administration/GenericChartsFilters/GetFiltersByChart", { GenericChartID }).done(function (data) {
                $("#div_DataChartAdminFilters").html(data);
                $("#div_Main_DataChartAdminFilters").css("display", "block");
                $(".selectpicker").selectpicker();
                $(".filter_datepicker").datepicker();

                //Carga los datos de los filtros
                $(".filters_of_chart").each(function () {
                    var Filter = $(this);
                    var CatalogID = Filter.data("entityid");
                    var FilterType = Filter.data("filtertypename");
                    $.get("/Administration/GenericCharts/LoadFilterInfo", { CatalogID }).done(function (data) {
                        if (typeof FilterType !== "undefined") {
                            if (FilterType.replace("#ddl_", "").toUpperCase() !== "DATE") {                            
                                var elements = data.FilterElementsList;
                                if (typeof elements !== "undefined") {
                                    Filter.append("<option value='0'>" + LangResources.chsn_SelectOption + "</option>");
                                    $.each(elements, function () {
                                        var item = $(this)[0];
                                        Filter.append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                                    });

                                    Filter.selectpicker("refresh");
                                }
                            }
                        }
                    });
                });

                //Carga los campos de la gráfica
                $.get("/Administration/GenericCharts/GetFieldsOfChart", { GenericChartID }).done(function (data) {
                    $("#div_DataChartAdminFieldsTable").html(data);
                }).always(function () {
                    $('.loading-process-div').hide();
                });
            }).always(function () {
                $('.loading-process-div').hide();
            });
        } else {
            $("#div_DataChartAdminFilters").empty();
            $("#div_DataChartAdminFieldsTable").empty();
            $('.loading-process-div').hide();
        }
    });

    $(document).on("click", "#btn_ChartPreview", function () {
        var panel = "";
        if ($("#tbl_GenericChartData tr").length > 1) {
            var MaxValue=0;
            $.get("/Administration/GenericCharts/GetModalChartPreview").done(function (datachart) {
                $("#div_mo_CharPreview").html(datachart.View);
                var xArray = "";
                var data = [];
                MaxValue = 0;
                var HeadersCount = $("#tbl_GenericChartData th").length;
                $(".table_data_row").each(function () {
                    var value = $(this).find("td:nth-child(1)").text();
                    if (value !== "") {
                        xArray += "," + value;
                    }
                });

                var ChartEntities = [];
                for (var i = 2; i <= HeadersCount; i++) {
                    var AxisData = $("#headers").find("th:nth-child(" + i + ")");
                    var ChartType = AxisData.data("datacharttypename").toLowerCase(); 
                    var ColorData = AxisData.data("color");

                    data = [];
                    var newmax = 0;
                    //recorido de serie por serie
                    //newmax = Math.max.apply(null, data); //cgarcia 28 oct 2019
                    $(".Field_" + i).each(function () {
                        if ($(this).val() > 0) {
                            if (parseFloat($(this).val()) > parseFloat(newmax)) {
                                newmax = $(this).val();
                            }
                            data.push($(this).val());
                        } else {
                            data.push("null");
                        }                       
                    });
                    //calculo de valor maximo de las series
                    if (parseFloat(newmax) > parseFloat(MaxValue)) {
                        MaxValue = newmax;
                    }

                    var ChartEntity = {
                        label: AxisData.text().trim(), 
                        yAxisID: null,
                        type: null,
                        borderColor: 0,
                        backgroundColor: null,
                        borderWidht: 0,
                        data: data,
                        Format: AxisData.data("formattype"),
                        showLine: AxisData.data("showline") == "True" ? true : false,
                        labelrotation: AxisData.data("labelrotation"),
                        labelshow: AxisData.data("labelshow") == "True" ? true : false,
                        labelfontsize : AxisData.data("labelfontsize"),
                        labelfontcolor: AxisData.data("labelfontcolor"),
                        labelfontbgcolor: AxisData.data("labelfontbgcolor")
                    };

                    if (ChartType === "stacked") {
                        ChartType = "bar";
                    }
                    ChartEntity.type = ChartType;

                    if (ChartType === "line") {                       
                        ChartEntity.yAxisID = "LineAxis";
                        ChartEntity.borderColor = ColorData;
                        ChartEntity.backgroundColor = "transparent";
                    } else {
                        ChartEntity.yAxisID = "BarAxis";
                        ChartEntity.backgroundColor = ColorData;
                    }


                    ChartEntities.push(ChartEntity);
                }

                xArray = xArray.replace(",", "");
                GeneratePreviewChart(xArray, ChartType, ChartEntities,panel, MaxValue);


                $("#mo_CharPreview").modal("show");
            });
        } else {
            notification("", LangResources.msg_PreviewChartEmptyData, "_ntf");
        }
    });

    $(document).on("click", "#btn_SaveDataChartChart", function () {
        if ($("#tbl_GenericChartData tr").length > 1) {
            if ($("#ContainsData").val().toLowerCase() === "true") {
                SetConfirmBoxAction(function () {
                    SaveGenericChartData();
                }, LangResources.msg_ChartContainsData);
            } else {
                SaveGenericChartData();
            }
        } else {
            notification("", LangResources.msg_PreviewChartEmptyData, "_ntf");
        }
    });

}

/*lHernandez : Esta funcion se encarga de tomar la informacion pegada en el 
               area de texto para despues acomodarla en la tabla con los campos 
               que va a llevar la grafica
*/
function GetInfoCopied(data) {
    setTimeout(function () {
        $('.loading-process-div').show();
        $("#tbl_GenericChartData tbody").empty();
        var count = 1;
        var HeadersCount = $("#tbl_GenericChartData th").length;

        //se leen los datos del clipboard y se separan
        var CopiedText = data.value.split("\n");

        //Se copian las registros de los datos que hay en el clipboard
        $.each(CopiedText, function (key, value) {
            var columns = value.split("	");
            $("#tbl_GenericChartData").append("<tr class='table_data_row' id='tr_" + count + "'>");
            if (columns.length > 1) {
                $.each(columns, function (key, value) {
                    //si hay mas informacion copiada que la de la tabla, solo agrega las columnas que alcance
                    if (key < HeadersCount) {
                        if (key === 0) {
                            $("#tr_" + count).append("<td>" + value + "</td>");
                        } else {
                            value = value.replace(/[A-Za-z\s]/g, '').replace(/[%&/()!"#$_\s]/g, '').replace(/[,\s]/g, '');
                            $("#tr_" + count).append("<td><input type='text' class='form-control onlydecimals Field_" + parseInt(key + 1) + "' value='" + value + "'></td>");
                        }
                    }
                });
                count = count + 1;
            }
        });

        SetupOnlyDecimal();
        $('.loading-process-div').hide();

        $("#txt_PasteClipboardData").val("");
    }, 500);
}