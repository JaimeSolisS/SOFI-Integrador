function EditInit(LangResources) {

    function RegisterPluginMiniColor() {    
        $('.mini-color').minicolors({
            control: 'hue',
            format: 'hex',
            keywords: 'transparent',
            letterCase: 'lowercase',
            opacity: true,
            position: 'top right',
            swatches: ['#000000', '#ffffff', 'transparent'],
            theme: 'bootstrap'
        });      
    }

    RegisterPluginMiniColor();

    function FilterTypeOptionChange(DefaultValueID) {
        var option = $("#ddl_FilterType option:selected");
        if (option.text() === "list".toUpperCase()) {
            $(".show_if_type_list").css("display", "block");
            $(".show_if_default_value").css("display", "block");
            $(".show_if_default_formula").css("display", "none");
            $(".txt_InputValue").css("display", "none");
            $("#ddl_FilterDefaultValuesList");

            $("#ddl_FilterDefaultValuesList").empty();
            var ValueID = $("#ddl_FilterTypeList option:selected").text();
            $.get("/Administration/GenericChartsFilters/GetListOfLists", { ValueID }).done(function (data) {

                $.each(data.ListOfLists, function () {
                    $("#ddl_FilterDefaultValuesList").append("<option value='" + $(this)[0].Value + "'>" + $(this)[0].Text + "</option>");
                });

                $("#ddl_FilterDefaultValuesList").val(DefaultValueID);
                $("#ddl_FilterDefaultValuesList").selectpicker("refresh");

            });

        } else {
            $(".show_if_type_list").css("display", "none");
        }
    }

    $('.max-length').maxlength();

    function FiltersTableReload() {
        $('.loading-process-div').show();
        var GenericChartID = $("#GenericChartID").val();
        $.get("/Administration/GenericChartsFilters/FiltersTableReload", { GenericChartID }).done(function (data) {
            $("#div_DataChartFiltersTable").html(data);
            $('.loading-process-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }

    function FilterAddValidation() {
        var ErrorMessages = [];
        if ($("#txt_FilterName").val() === "") {
            ErrorMessages.push(LangResources.msg_FilterNameRequired);
        }
        if ($("#ddl_FilterType option:selected").val() === 0) {
            ErrorMessages.push(LangResources.msg_FilterCategoryRequired);
        }

        if ($("#ddl_FilterType option:selected").text() === "LIST") {
            if ($("#ddl_FilterTypeList option:selected").val() === 0) {
                ErrorMessages.push(LangResources.msg_FilterTypeListRequired);
            }
        }

        if ($("#ddl_DefaultValue option:selected").text() === "Formula") {
            if ($("#ddl_FilterFormulasList option:selected").val() === 0) {
                ErrorMessages.push(LangResources.msg_FilterFormulaRequired);
            }
        }

        if ($("#ddl_DefaultValue option:selected").text() === "Value") {
            if ($("#ddl_FilterDefaultValuesList option:selected").val() === 0) {
                ErrorMessages.push(LangResources.msg_FilterValueRequired);
            }
        }

        return ErrorMessages;
    }

    $(".selectpicker").selectpicker();

    $("#btn_NewFilter").on("click", function (e) {
        e.stopPropagation();
        var IsEdit = false;
        $('.loading-process-div').show();
        $.get("/Administration/GenericChartsAdministration/GetModalNewEditFilter", { IsEdit }).done(function (data) {

            $("#div_Mo_NewEditFilter").html(data.View);
            $("#mo_ChartFilter").modal("show");
            $(".selectpicker").selectpicker();

            FilterTypeOptionChange();
            $('.max-length').maxlength();

        }).always(function () {
            $('.loading-process-div').hide();
        });
    });


    (function () {
        //selecciona los valores de las listas de la grafica que se esta editando
        $("#ddl_ChartsAreas").val($("#GenericChartAreaID").val()).selectpicker("refresh");
        $("#ddl_ChartsTypes").val($("#GenericChartTypeID").val()).selectpicker("refresh");
    })();

    $(document).on("change", "#ddl_FilterType", function () {
        FilterTypeOptionChange();
    });

    $(document).on("change", "#ddl_DefaultValue", function () {
        FilterDefaultValueOptionChange();
    });

    $(document).on("click", "#btn_SaveNewFilter", function () {
        var ErrorMessages = FilterAddValidation();
        if (ErrorMessages.length > 0) {
            $.each(ErrorMessages, function (key, value) {
                notification("", value, "_ntf");
            });
        } else {
            var DefaultValueVar = "";
            var FilterListID = null;
            var DefaultValueVal = 0;
            var option = $("#ddl_FilterType option:selected");

            if (option.text() === "LIST") {
                FilterListID = option.val();
                DefaultValueVar = $("#ddl_FilterDefaultValuesList option:selected").text();
                DefaultValueVal = $("#ddl_FilterDefaultValuesList option:selected").val();
            } else {

                option = $("#ddl_DefaultValue option:selected");

                if (option.text().toUpperCase() === "formula".toUpperCase()) {
                    DefaultValueVar = $("#ddl_FilterFormulasList option:selected").text();
                }

                if (option.text().toUpperCase() === "value".toUpperCase()) {
                    DefaultValueVar = $("#ddl_FilterDefaultValuesList option:selected").text();
                    DefaultValueVal = $("#ddl_FilterDefaultValuesList option:selected").val();
                }

                if (option.text().toUpperCase() === "input data".toUpperCase()) {
                    DefaultValueVar = $("#txt_InputValue").val();
                }
            }

            var IsCatalogList = $("#ddl_FilterTypeList option:selected").val();
            var CatalogName = "";
            if (IsCatalogList !== 0) {
                CatalogName = $("#ddl_FilterTypeList option:selected").text();
            }

            var Seq = $("#table_Filters tr").length;
            $("#table_Filters").append(
                '<tr data-filtertypeid=' + $("#ddl_FilterType option:selected").val() + '" data-filterlistid="' +
                $("#ddl_FilterTypeList option:selected").val() + '" data-filterenabled="' + $("#FilterEnabled").is(":checked") +
                '" data-defaultvalue="' + DefaultValueVal + '" class="chart-filter-row">' +
                "<td>" +
                Seq +
                "</td>" +
                "<td>" +
                $("#txt_FilterName").val() +
                "</td>" +
                "<td>" +
                $("#ddl_FilterType option:selected").text() +
                "</td>" +
                "<td>" +
                CatalogName +
                "</td>" +
                "<td>" +
                DefaultValueVar +
                "</td>" +
                '<td style="width:15%; text-align:center">' +
                '<button class="btn btn-success text-primary edit-filter-record"><span class="fa fa-lg fa-edit" style="color:white !important"></span></button>' +
                '<button class="btn btn-danger delete-filter-record"><span class="glyphicon glyphicon-trash"></span></button>' +
                '</td>' +
                "</tr>"
            );
            $("#mo_ChartFilter").modal("toggle");
        }
    });

    $(document).on("click", "#btn_SaveEditedFilter", function () {
        var option = $("#ddl_FilterType option:selected");
        var DefaultValueVar = "";
        var DefaultValueVal = 0;
        var FilterListID = null;

        if (option.text().toUpperCase() === "LIST") {
            FilterListID = option.val();
        }

        option = $("#ddl_DefaultValue option:selected");

        if (option.text().toUpperCase() === "formula".toUpperCase()) {
            DefaultValueVar = $("#ddl_FilterFormulasList option:selected").text();
        }

        if (option.text().toUpperCase() === "value".toUpperCase() ||
            $("#ddl_FilterType option:selected").text().toUpperCase() === "LIST") {
            DefaultValueVar = $("#ddl_FilterDefaultValuesList option:selected").text();
            DefaultValueVal = $("#ddl_FilterDefaultValuesList option:selected").val();
        }

        if (option.text().toUpperCase() === "input data".toUpperCase()) {
            DefaultValueVar = $("#txt_InputValue").val();
        }

        var IsCatalogList = $("#ddl_FilterTypeList option:selected").val();
        var CatalogName = "";
        if (IsCatalogList !== 0) {
            CatalogName = $("#ddl_FilterTypeList option:selected").text();
        }

        //ddl_FilterTypeList
        var Row = CurrentRow;
        Row.find("td:nth-child(2)").html($("#txt_FilterName").val());
        Row.find("td:nth-child(3)").html($("#ddl_FilterType option:selected").text());
        Row.find("td:nth-child(4)").html(CatalogName);
        Row.find("td:nth-child(5)").html(DefaultValueVar);
        Row.data("filtertypeid", $("#ddl_FilterType option:selected").val());
        Row.data("filterlistid", $("#ddl_FilterTypeList option:selected").val());
        Row.data("filterenabled", $("#ddl_FilterType option:selected").val());
        Row.data("defaultvalue", DefaultValueVal);


        $("#mo_ChartFilter").modal("toggle");
    });

    /////////////////////Axis Events and Functions////////////////////

    $(document).on("click", "#btn_SaveAxis", function () {
        var isedit = $(this).data("isedit");
        //validar si que tipo de axis es
        var IsAxisX = false;
        if ($("#ddl_AxisCategories option:selected").text() === "X Axis") {
            IsAxisX = true;
        }
        var xaxisnumber = $("#table_Axis tbody").find('[data-isaxex="True"]').length;
        //validar si ya hay un xaxis
        if (IsAxisX && xaxisnumber === 1) {
            notification("", LangResources.msg_xAxisExis, "error");
        } else {
            if (isedit) {
                var axisid = $(this).data("axisid");
                var Row = $('#' + axisid);
                //actualizar campos del tr
                Row.find("td:nth-child(2)").html($("#ddl_AxisCategories option:selected").text());
                Row.find("td:nth-child(3)").html($("#txt_AxisName").val());
                Row.find("td:nth-child(4)").html($("#ddl_DataTypes option:selected").text());
                Row.find("td:nth-child(6)").html($("#ddl_FormatData option:selected").text());
                Row.find("td:nth-child(7)").html($("#ddl_DataChartsTypes option:selected").text());
                //actualizar los data-
                Row.data("isaxex", IsAxisX);
                Row.data("axischarttypeid", $("#ddl_DataChartsTypes option:selected").val());
                Row.data("axiscategory", $("#ddl_AxisCategories option:selected").text());
                Row.data("axisdatatypeid", $("#ddl_DataTypes option:selected").val());
                Row.data("color", $("#txt_Color").val());
                Row.data("axistypeid", $("#ddl_AxisCategories option:selected").val());
                Row.data("formattypeid", $("#ddl_FormatData option:selected").val());
                Row.data("prefix", $("#ddl_FormatData option:selected").text() );
                Row.data("showline", $("#chk_ShowLine").is(":checked")); 
                Row.data("labelrotation", $("#txt_Datalabelrotation").val()); 
                Row.data("labelshow", $("#chk_DatalabelShow").is(":checked")); 
                Row.data("labelfontsize", $("#txt_DatalabelFontSize").val()); 
                Row.data("labelfontcolor", $("#txt_DatalabelColor").val()); 
                Row.data("labelfontbgcolor", $("#txt_DatalabelBackgroundColor").val());

                Row.find("td:nth-child(5)").html('<input type="hidden" class="mini-color BackgroundColor" value="' + $("#txt_Color").val() + '" name="BackgroundColor" disabled />');
            } else {
                //agregar un nuevo tr con los datos
                var Seq = $("#table_Axis tr").length;
                $("#table_Axis").append(
                    '<tr id="axis_' + Seq + '"' +
                    'class="chart-axis-row"' +
                    'data-isaxex="' + IsAxisX + '"' +
                    'data-axischarttypeid="' + $("#ddl_DataChartsTypes option:selected").val() + '"' +
                    'data-axiscategory="' + $("#ddl_AxisCategories option:selected").text() + '"' +
                    'data-axisdatatypeid="' + $("#ddl_DataTypes option:selected").val() + '"' +
                    'data-color="' + $("#txt_Color").val() + '"' +
                    'data-axistypeid="' + $("#ddl_AxisCategories option:selected").val() + '"' +
                    'data-formattypeid="' + $("#ddl_FormatData option:selected").val() + '"' +
                    'data-prefix="' + $("#ddl_FormatData option:selected").text() + '"' +
                    'data-showline="' + $("#chk_ShowLine").is(":checked") + '"' +
                    'data-labelrotation="' + $("#txt_Datalabelrotation").val() + '"' +
                    'data-labelshow="' + $("#chk_DatalabelShow").is(":checked") + '"' +
                    'data-labelfontsize="' + $("#txt_DatalabelFontSize").val() + '"' +
                    'data-labelfontcolor="' + $("#txt_DatalabelColor").val() + '"' +
                    'data-labelfontbgcolor="' + $("#txt_DatalabelBackgroundColor").val() + '"' +
                    '>' +
                    "<td>" + Seq + "</td>" +
                    "<td>" + $("#ddl_AxisCategories option:selected").text() + "</td>" +
                    "<td>" + $("#txt_AxisName").val() + "</td>" +
                    "<td>" + $("#ddl_DataTypes option:selected").text() + "</td>" +
                    "<td>" + '<input type="hidden" class="mini-color BackgroundColor" value="' + $("#txt_Color").val() + '" name="BackgroundColor" disabled />' + "</td>" +
                    "<td>" + $("#ddl_FormatData option:selected").text() + "</td>" +
                    "<td>" + $("#ddl_DataChartsTypes option:selected").text() + "</td>" +
                    '<td style="width:15%; text-align:center">' +
                    '<button class="btn btn-success text-primary edit-axis-record"><span class="fa fa-lg fa-edit" style="color:white !important"></span></button>' +
                    '<button class="btn btn-danger delete-axis-record"><span class="glyphicon glyphicon-trash"></span></button>' +
                    '</td>' +
                    "</tr>"
                );
            }
            RegisterPluginMiniColor();
            $("#mo_ChartAxis").modal("toggle");
        }

    });

    $("#btn_NewAxis").on("click", function (e) {   
        $('#txt_AxisName').val('');
        //ddl_AxisCategories
        $("#ddl_AxisCategories").val(0).selectpicker("refresh");
        //ddl_DataTypes
        $("#ddl_DataTypes").val(0).selectpicker("refresh");
        //chk_ShowLine
        $("#chk_ShowLine").prop('checked', true);

        //ocultar opc y axis
        $(".y_axis_fields").css("display", "none");
        $("#aux_Categories").removeClass("col-sm-6");
        $("#aux_Categories").addClass("col-sm-12");
        $("#aux_ddl_DataTypes").removeClass("col-sm-3");
        $("#aux_ddl_DataTypes").addClass("col-sm-12");

        RegisterPluginMiniColor();
        $(".selectpicker").selectpicker();
        $('.max-length').maxlength();

        //agregar data para saver si es edicion o nuevo
        $('#btn_SaveAxis').data("isedit", false);
        $("#mo_ChartAxis").modal("show");
    });

    $(document).on("click", ".edit-axis-record", function (e) {
        e.stopPropagation();
        var Row = $(this).closest("tr");
        $("#txt_AxisName").val(Row.find("td:nth-child(3)").text());
        $("#ddl_AxisCategories").val(Row.data("axistypeid")).selectpicker("refresh");
        $("#ddl_DataChartsTypes").val(Row.data("axischarttypeid")).selectpicker("refresh");
        $("#ddl_DataTypes").val(Row.data("axisdatatypeid")).selectpicker("refresh");
        $("#ddl_FormatData").val(Row.data("formattypeid")).selectpicker("refresh");

        $("#txt_Color").val(Row.find("td:nth-child(5)").find("input").val());
        $("#txt_Color").minicolors('value', Row.find("td:nth-child(5)").find("input").val());

        $("#txt_Datalabelrotation").val(Row.data("labelrotation"));

        $("#chk_ShowLine").prop('checked', Row.data("showline") == "True" ? true : false);
        $("#chk_DatalabelShow").prop('checked', Row.data("labelshow") == "True" ? true : false);

        $("#txt_DatalabelFontSize").val(Row.data("labelfontsize"));
        $("#txt_DatalabelColor").val(Row.data("labelfontcolor"));
        $("#txt_DatalabelColor").minicolors('value', Row.data("labelfontcolor"));
        $("#txt_DatalabelBackgroundColor").val(Row.data("labelfontbgcolor"));
        $("#txt_DatalabelBackgroundColor").minicolors('value', Row.data("labelfontbgcolor"));

        if ($("#ddl_AxisCategories option:selected").text() === "Y Axis") {
            $(".y_axis_fields").css("display", "block");
            $("#aux_Categories").removeClass("col-sm-12");
            $("#aux_Categories").addClass("col-sm-6");
            $("#aux_ddl_DataTypes").removeClass("col-sm-12");
            $("#aux_ddl_DataTypes").addClass("col-sm-3");
        } else {
            $(".y_axis_fields").css("display", "none");
            $("#aux_Categories").removeClass("col-sm-6");
            $("#aux_Categories").addClass("col-sm-12");
            $("#aux_ddl_DataTypes").removeClass("col-sm-3");
            $("#aux_ddl_DataTypes").addClass("col-sm-12");
        }

        $('.max-length').maxlength();
        $("#txt_FormatDataPreview").val($("#ddl_FormatData option:selected").data("param1"));

        //agregar data para saver si es edicion o nuevo
        $('#btn_SaveAxis').data("isedit", true);
        $('#btn_SaveAxis').data("axisid", Row.attr("id"));
        $("#mo_ChartAxis").modal("show");
    });

    $(document).on("click", ".delete-axis-record", function () {
        var row = $(this).closest("tr");
        SetConfirmBoxAction(function () {
            row.remove();
        }, LangResources.msg_ConfirmDeleteAxis);
    });

    $(document).on("change", "#ddl_AxisCategories", function () {
        if ($("#ddl_AxisCategories option:selected").text() === "Y Axis") {
            $(".y_axis_fields").css("display", "block");
            $("#aux_Categories").removeClass("col-sm-12");
            $("#aux_Categories").addClass("col-sm-6");
            $("#aux_ddl_DataTypes").removeClass("col-sm-12");
            $("#aux_ddl_DataTypes").addClass("col-sm-3");
        } else {
            $(".y_axis_fields").css("display", "none");
            $("#aux_Categories").removeClass("col-sm-6");
            $("#aux_Categories").addClass("col-sm-12");
            $("#aux_ddl_DataTypes").removeClass("col-sm-3");
            $("#aux_ddl_DataTypes").addClass("col-sm-12");
        }
    });

    /////////////////////Axis Events and Functions////////////////////

    $(document).on("click", ".delete-filter-record", function () {
        //var GenericChartFilterID = $(this).parent().parent().data("entityid");
        var row = $(this).closest("tr");
        SetConfirmBoxAction(function () {
            row.remove();
        }, LangResources.msg_ConfirmDeleteFilter);
    });

    $(document).on("click", ".edit-filter-record", function (e) {
        e.stopPropagation();
        var IsEdit = true;
        var Row = $(this).closest("tr");
        CurrentRow = Row;
        //var GenericChartFilterID = $(this).parent().parent().data("entityid");
        $('.loading-process-div').show();
        $.get("/Administration/GenericChartsAdministration/GetModalNewEditFilter", { IsEdit }).done(function (data) {
            $("#div_Mo_NewEditFilter").html(data.View);
            $("#mo_ChartFilter").modal("show");

            $("#txt_FilterName").val(Row.find("td:nth-child(2)").text());
            $("#ddl_FilterType").val(Row.data("filtertypeid"));
            $("#ddl_FilterTypeList").val(Row.data("filterlistid"));
            $("#ddl_DefaultValue").val(Row.find("td:nth-child(5)").text());


            FilterTypeOptionChange(Row.data("defaultvalue"));
            $('.max-length').maxlength();

            $('.loading-process-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $(document).on("change", "#ddl_FilterTypeList", function () {
        $("#ddl_FilterDefaultValuesList").empty();
        var ValueID = $("#ddl_FilterTypeList option:selected").text();
        $.get("/Administration/GenericChartsFilters/GetListOfLists", { ValueID }).done(function (data) {
            $.each(data.ListOfLists, function () {
                $("#ddl_FilterDefaultValuesList").append("<option value='" + $(this)[0].Value + "'>" + $(this)[0].Text + "</option>");
            });

            $("#ddl_FilterDefaultValuesList").selectpicker("refresh");

        });

    });


    $("#btn_SaveChart").on("click", function () {
        $('.loading-process-div').show();
        var GenericChartHeaderDataID = $("#GenericChartHeaderDataID").val();
        var GenericChartID = $("#GenericChartID").val();
        var GenericChartDataEntities = [];
        var GenericChartsFilterEntities = [];
        var GenericChartsAxisEntities = [];


        var ChartDataEntity = {
            GenericChartID: GenericChartID,
            ChartName: $("#txt_ChartName").val(),
            ChartTitle: $("#txt_ChartTitle").val(),
            ChartAreaID: $("#ddl_ChartsAreas option:selected").val(),
            ChartTypeID: $("#ddl_ChartsTypes option:selected").val(),
            Enabled: $("#chk_Enabled").is(":checked")
        };

        GenericChartDataEntities.push(ChartDataEntity);

        $(".chart-filter-row").each(function () {
            var a = $(this).data("filtertypeid");

            var GenericChartsFilters = {
                GenericChartFilterID: parseInt($(this).data("entityid")),
                GenericChartID: GenericChartID,
                FilterName: $(this).find("td:nth-child(2)").text(),
                FilterTypeID: parseInt($(this).data("filtertypeid")),
                FilterListID: parseInt($(this).data("filterlistid")),
                DefaultValue: $(this).data("defaultvalue"), //$(this).find("td:nth-child(5)").val(),
                DefaultValueFormula: null,
                Enabled: $(this).data("filterenabled")
            };

            GenericChartsFilterEntities.push(GenericChartsFilters);
        });

        $(".chart-axis-row").each(function () {

            var GenericChartsAxes = {
                GenericChartAxisID: null,
                GenericChartID: GenericChartID,
                AxisName: $(this).find("td:nth-child(3)").text(),
                AxisTypeID: $(this).data("axistypeid"),
                AxisChartTypeID: $(this).data("axischarttypeid"),
                AxisDatatypeID: $(this).data("axisdatatypeid"),
                AxisColor: $(this).find("td:nth-child(5)").find("input").val(),
                AxisFormat: $(this).data("formattypeid"),
                ShowLine: $(this).data("showline"),
                DataLabelRotation: $(this).data("labelrotation"),
                DataLabelShow: $(this).data("labelshow"),
                DataLabelFontSize: $(this).data("labelfontsize"),
                DataLabelFontColor: $(this).data("labelfontcolor"),
                DataLabelFontBGColor: $(this).data("labelfontbgcolor")
            };
            GenericChartsAxisEntities.push(GenericChartsAxes);
        });


        $.post("/Administration/GenericChartsAdministration/InsertChart", {
            GenericChartHeaderDataID,
            GenericChartID,
            GenericChartDataEntities,
            GenericChartsFilterEntities,
            GenericChartsAxisEntities
        }).done(function (data) {
            $('.loading-process-div').hide();
            notification("", data.ErrorMessage, data.notifyType);
            if (data.notifyType !== "error") {
                $('.loading-process-div').show();
                window.location = "/Administration/GenericChartsAdministration/Index";
            }
        });

    });


    $(document).on("change", "#ddl_FormatData", function () {
        $("#txt_FormatDataPreview").val($("#ddl_FormatData option:selected").data("param1"));
    });
}