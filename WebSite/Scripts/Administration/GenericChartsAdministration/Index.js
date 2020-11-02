function IndexInit(LangResources) {

    $(".selectpicker").selectpicker();

    $("#ddl_ChartsAreas").on("change", function () {
        $('.loading-pcrocess-div').show();
        //ClearFiltersDiv();
        var ChartAreaID = $("#ddl_ChartsAreas option:selected").val();
        $.get("/Administration/GenericCharts/GetChartsByAreaList", { ChartAreaID }).done(function (data) {
            if (data.ErrorCode == 0) {
                var datos = data.ChartsOfAreaList;

                $("#ddl_ChartsOfArea").empty();
                $("#ddl_ChartsOfArea").append("<option value='0'>" + LangResources.TagAll + "</option>");
                //$("#ddl_family").append("<option value='a' class='new-family'>--" + LangResources.lbl_NewFamily + "--</option>");

                $.each(datos, function () {
                    $("#ddl_ChartsOfArea").append("<option value='" + this["Value"] + "'>" + this["Text"] + "</option>");
                });

                $("#ddl_ChartsOfArea").selectpicker("refresh");
            }
            $('.loading-pcrocess-div').hide();
        });
    });

    $("#ddl_ChartsOfArea").on("change", function (e) {
        e.stopPropagation();
        //ClearFiltersDiv();
        $('.loading-pcrocess-div').show();
        var GenericChartID = $("#ddl_ChartsOfArea option:selected").val();
        if (GenericChartID != 0) {

            $.get("/Administration/GenericChartsAdministration/LoadDataChart", { GenericChartID }).done(function (data) {
                $("#div_DataChartAdminFilters").html(data);
                $("#div_Main_DataChartAdminFilters").css("display", "block");
                $(".selectpicker").selectpicker();


                //Carga los campos de la gráfica
                $.get("/Administration/GenericCharts/GetFieldsOfChart", { GenericChartID }).done(function (data) {
                    $("#div_DataChartAdminFieldsTable").html(data);
                }).always(function () {
                    $('.loading-pcrocess-div').hide();
                });


            }).always(function () {
                $('.loading-pcrocess-div').hide();
            });
        }
    });

    $(document).on("click", ".delete-record", function () {
        var GenericChartID = $(this).parent().parent().data("entityid");

        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.post("/Administration/GenericCharts/Delete", { GenericChartID }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                LoadGenericChartsTable();
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }, LangResources.msg_DeleteChartConfirm);
    });

    $(document).on("click", ".edit-record", function (e) {
        e.stopImmediatePropagation();
        var GenericChartID = $(this).parent().parent().data("entityid");
        $('.loading-process-div').show();
        window.location = "/Administration/GenericChartsAdministration/Edit?GenericChartID=" + GenericChartID;

    });

    $("#btn_NewChart").on("click", function (e) {
        e.stopImmediatePropagation();
        $('.loading-process-div').show();
        window.location = "/Administration/GenericChartsAdministration/New";

    });

    $("#btn_Search").on("click", function (e) {
        var ChartAreaID = $("#ddl_ChartsAreas option:selected").val();
        var GenericChartID = $("#ddl_ChartsOfArea option:selected").val();

        $.get("/Administration/GenericChartsAdministration/Search", { ChartAreaID, GenericChartID }).done(function (data) {
            $("#div_DataChartAdminTable").html(data);
        });
    });


    function LoadGenericChartsTable() {
        $('.loading-process-div').show();
        $.get("/Administration/GenericCharts/LoadGenericChartsTable").done(function (data) {
            $("#div_DataChartAdmin").css("display", "block");
            $("#div_DataChartAdminTable").html(data);
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }
}

