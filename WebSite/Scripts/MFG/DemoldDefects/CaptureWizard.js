// =============================================================================================================================
//  Version: 20190930
//  Author:  Luis Hernandez
//  Created Date: 30 Sep 2019
//  Description:  DemoldDefect JS
//  Modifications: 
// =============================================================================================================================
function IndexInit(LangResources) {
    (function () {
        SlideForm(LangResources);
        if ($("#ExceptionMessage").val() != "") {

            $("#message-box-generic-alert").removeClass("message-box-warning");
            $("#message-box-generic-alert").addClass("message-box-info");
            $("#message-box-generic-alert-title").text(LangResources.lbl_Warning);
            $("#message-box-generic-alert-legend").text($("#ExceptionMessage").val());
            $('#message-box-generic-alert').toggleClass("open");

        }
    })();

    function ColorButtonsControl(object, objectClass) {
        $(objectClass).addClass("btn-generic");
        $(objectClass).removeClass("choosed");
        $(object).removeClass("btn-generic")
            .css("backgroundColor", "orange");
        $(object).addClass("choosed");
        //.css("font-size", "40px");
    }

    function SetupxEditable() {
        $('.x-editable').editable({
            placement: 'bottom',
            success: function (response, newValue) {
                var identifier = $(this).data("identifier");
                switch (identifier) {
                    case "inspector":
                        $(this).data("value", newValue);
                        break;
                    case "line":
                        $(this).data("entityid", newValue);
                        $(this).data("value", newValue);
                        break;
                    case "vat":
                        $(this).data("value", newValue);
                        $("#vat_id").val(newValue);
                        break;
                    default:
                }
            }
        });

        $('.x-editable').on("shown", function (e, editable) {
            $(".editable-input").find("input[type='text']").css({ 'cssText': 'margin-top:0px; border:solid !important; border-width:1px !important' });
            $(".btn-default").css("background-color", "lightgray");
            $(".editable-input").find("input[type='select']").selectpicker();
        });
    }

    SetupxEditable();

    $("#btn_prev_slideform").prop("disabled", true);

    $("#btn_Back").on("click", function () {
        ShowProgressBar();
        //var ShiftID = $("#span_shift").data("entityid");
        var ProductionProcessID = $("#production_process_id").val();
        var ProductionLineID = $("#span_line").data("entityid");
        var VATID = $("#vat_id").val();
        var InspectorName = $("#span_inspector").text();
        window.location = "/MFG/DemoldDefects/Index?ProductionProcessID=" + ProductionProcessID
            + "&ProductionLineID=" + ProductionLineID
            + "&InspectorName=" + InspectorName
            + "&VATID=" + VATID;
    });

    $("#btn_next_slideform").prop('disabled', 1);

    $(document).on("click", ".familyoption", function () {
        $(".familyoption").css("backgroundColor", "#649afd");
        $(this).attr('style', 'background-color: orange !important');

        var moldfamilyid = $(this).data("moldfamilyid");
        var moldfamilyname = $(this).data("moldfamilyname");

        $("#lbl_family").text(moldfamilyname);
        $("#span_family").text(moldfamilyname);
        $("#span_familyid").val(moldfamilyid);

        var MoldFamilyID = moldfamilyid;
        ShowProgressBar();
        $.get("/MFG/DemoldDefects/GetLensTypesList", { MoldFamilyID }).done(function (data) {
            $("#lens_container").empty();

            $.each(data.list, function (key, value) {
                $("#lens_container").append(
                    '<button id="' + value.LensTypeID + '" type="button" class="btn-info btn-pretty btn-lg lentypesoption btn-generic" ' +
                    'data-lenstypeid="' + value.LensTypeID + '"' +
                    'data-issimplevision="' + value.IsSimpleVision + '"' +
                    'data-lenstypename="' + value.LensTypeName + '" >' + value.LensTypeName +
                    ' </button >'
                );
            });

            HideProgressBar();
        });

        showNextCard("family");
    });

    $(document).on("click", ".catbasesoption", function () {
        ColorButtonsControl($(this), ".catbasesoption");

        var Category = $(this).find("span").text();
        var CatalogTag = "DemoldDefectBases";
        $.get("/MFG/DemoldDefects/GetDataOfCategory", { Category, CatalogTag }).done(function (data) {
            $("#base_container").empty();

            $.each(data.list, function (key, value) {
                $("#base_container").append(
                    '<button id="' + value.CatalogDetailID + '" type="button" class="btn-info btn-pretty btn-lg basesoption btn-generic col-sm-4" ' +
                    ' ><span>' + value.DisplayText +
                    ' </span></button >'
                );
            });
        });
        $("#lbl_catbase").text(Category);
        showNextCard("base");

    });

    $(document).on("click", ".cataditionoption", function () {
        ColorButtonsControl($(this), ".cataditionoption");

        var Category = $(this).find("span").text();
        var CatalogTag = "DemoldDefectAditions";

        if ($(this).data("noadditon") == "1") {
            $("#lbl_catadition").text(LangResources.lbl_NoAddition);
            $("#lbl_adition").text(LangResources.lbl_NoAddition);
            $("#NoAddition").val(true);
            showNextCard("adition");
            showNextCard("adition");
        } else {
            $.get("/MFG/DemoldDefects/GetDataOfCategory", { Category, CatalogTag }).done(function (data) {
                $("#adition_container").empty();

                $.each(data.list, function (key, value) {
                    $("#adition_container").append(
                        '<button id="' + value.CatalogDetailID + '" type="button" class="btn-info btn-pretty btn-lg aditionoption btn-generic col-sm-3" ' +
                        ' ><span>' + value.DisplayText +
                        ' </span></button >'
                    );
                });
            });
            $("#NoAddition").val(false);
            $("#lbl_catadition").text(Category);
            showNextCard("adition");
        }

    });

    $(document).on("click", ".catdefectsyoption", function () {
        ColorButtonsControl($(this), ".catdefectsyoption");
        $(".catdefectsyoption").removeClass('active');
        $(this).addClass('active');
        $("#defects_container").empty();
        var Category = $(this).find("span").text();
        var CatalogTag = "DemoldDefectsTypes";
        $.get("/MFG/DemoldDefects/GetDataOfCategory", { Category, CatalogTag }).done(function (data) {

            $.each(data.list, function (key, value) {
                $("#defects_container").append(
                    '<button id="' + value.CatalogDetailID + '" type="button" class="btn-info btn-pretty btn-lg defectoption btn-generic col-sm-6" ' +
                    ' ><span>' + value.DisplayText +
                    ' </span></button >'
                );
            });
        });

        showNextCard("defects");
    });

    $(document).on("click", ".lentypesoption", function () {
        ColorButtonsControl($(this), ".lentypesoption");

        var lenstypename = $(this).data("lenstypename");
        $("#span_lentype").text(lenstypename);
        $("#lbl_lentype").text(lenstypename);
        $("#span_lentypeid").val($(this).attr("id"));
        showNextCard("family");
    });

    $(document).on("click", ".aditionoption", function () {
        ColorButtonsControl($(this), ".aditionoption");

        $("#span_adition").text($(this).find("span").text());
        $("#span_aditionid").val($(this).attr("id"));
        $("#lbl_adition").text($(this).find("span").text());
        showNextCard("adition");

    });

    $(document).on("click", ".basesoption", function () {
        ColorButtonsControl($(this), ".basesoption");

        $("#span_base").text($(this).find("span").text());
        $("#span_baseid").val($(this).attr("id"));
        $("#lbl_base").text($(this).find("span").text());

        showNextCard("base");

    });

    $(document).on("click", ".sideoption", function () {
        ColorButtonsControl($(this), ".sideoption");

        $("#span_side").text($(this).find("span").text());
        $("#span_sideid").val($(this).attr("id"));
        $("#lbl_side").text($(this).find("span").text());
        showNextCard("side");
    });

    $(document).on("click", ".defectoption", function () {

        if ($(".slideform-slide_2").hasClass("active")) {
            $("#btn_next_slideform").fadeOut();
        }

        ColorButtonsControl($(this), ".defectoption");
        $("#span_defecttypeid").val($(this).find("input").attr("id"));
        $(".defectoption").removeClass("active");
        $(this).addClass("active");
    });

    $("#add_unit_quantity").on("click", function () {

        if ($("#chk_BiggerQty").is(":checked")) {
            var CurrentQuantity = 0;

            if ($("#txt_biggerQty").val() == "" || $("#txt_biggerQty").val() == null) {
                CurrentQuantity = 0;
            } else {
                CurrentQuantity = parseInt($("#txt_biggerQty").val());
            }

            $("#txt_biggerQty").val(CurrentQuantity + 1);
        } else {
            var CurrentQuantity = parseInt($("#defect_quantity").text());
            $("#defect_quantity").text(CurrentQuantity + 1);
        }

    });

    $("#remove_unit_quantity").on("click", function () {
        if ($("#chk_BiggerQty").is(":checked")) {
            var CurrentQuantity = 0;

            if ($("#txt_biggerQty").val() == "" || $("#txt_biggerQty").val() == null) {
                CurrentQuantity = 0;
            } else {
                CurrentQuantity = parseInt($("#txt_biggerQty").val());
            }

            CurrentQuantity = CurrentQuantity - 1;
            if (CurrentQuantity < 1) {
                $("#txt_biggerQty").val(1);
            } else {
                $("#txt_biggerQty").val(CurrentQuantity);
            }

        } else {
            var CurrentQuantity = parseInt($("#defect_quantity").text());
            CurrentQuantity = CurrentQuantity - 1;
            if (CurrentQuantity < 1) {
                $("#defect_quantity").text(1);
            } else {
                $("#defect_quantity").text(CurrentQuantity);
            }
        }
    });

    $("#add_defect_quantity").on("click", function () {
        var NotInTable = true;
        var defect = $(".defectoption.active");
        var defectcat = $(".catdefectsyoption.active");
        var date = new Date();
        var FormatedDate = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDay() + ' ' + date.getHours() + ':' + date.getMinutes();

        var defect_quantity = 0;

        if ($("#chk_BiggerQty").is(":checked")) {
            defect_quantity = $("#txt_biggerQty").val();
        } else {
            defect_quantity = $("#defect_quantity").text();
        }

        if ($(".defectoption.active").length > 0) {

            $(".defect-row").each(function () {
                if ($(this).data("entityid") == defect.attr("id")) {
                    $(this).find("td:nth-child(2)").text(parseInt($(this).find("td:nth-child(2)").text()) + parseInt($("#defect_quantity").text()));
                    $(this).find("td:nth-child(3)").empty();//.text($("#span_inspector").text());
                    $(this).find("td:nth-child(3)").append(
                        LangResources.lbl_On + ' : ' + FormatedDate +
                        '<br /> ' +
                        LangResources.lbl_By + ' : ' + $("#span_inspector"
                        ).text());
                    NotInTable = false;
                }
            });

            if (NotInTable) {
                $("#defect_table tbody").prepend( // cambio de append
                    '<tr class="defect-row" data-entityid="' + defect.attr("id") + '">' +
                    '   <td>' +
                    defectcat.text() + ' - ' + defect.text() +
                    '   </td>' +
                    '   <td>' +
                    defect_quantity +
                    '   </td>' +
                    '<td>' +
                    LangResources.lbl_On + ' : ' + FormatedDate +
                    '<br /> ' +
                    LangResources.lbl_By + ' : ' + $("#span_inspector").text() +
                    '</td> ' +
                    ' <td style="width:13%; text-align:center">' +
                    '     <button class="btn btn-danger delete-defect" type="button" title="' + LangResources.tt_Edit + '">' +
                    '     <span class="glyphicon glyphicon-remove-circle"></span >' +
                    '     </button>' +
                    ' </td>' +
                    '</tr>'
                );
            }

            $("#defect_quantity").text(1);
            $("#txt_biggerQty").val("");
            SumAndValidateToSave();
        } else {
            notification("", LangResources.msg_DefectRequired, "_nft");
        }
    });

    function SumAndValidateToSave() {
        var suma = 0;
        $(".defect-row").each(function () {
            suma = suma + parseInt($(this).find("td:nth-child(2)").text());
        });
        $("#total_defects").text(suma);
        if (suma > 0) {
            $("#div_btn_save").fadeIn();
            //$("#btn_next_slideform").prop('disabled', 1);
        } else {
            $("#div_btn_save").fadeOut();
        }
    }

    $(document).on("click", ".delete-defect", function () {
        var Row = $(this).closest("tr");
        SetConfirmBoxAction(function () {
            Row.remove();
            SumAndValidateToSave();
        }, LangResources.msg_DeleteDemoldDefectConfirmMessage);
    });

    $("#btn_save").on("click", function () {
        ShowProgressBar();
        var DefectDate = null;
        var DefectMoldDetailsEntities = [];
        var ProductID = $("#ProductID").val();
        var LensGross = parseInt($("#span_plannigqty").text());

        var LineID = $("#span_line").data("entityid");
        var ShiftID = $("#span_vat_number").data("shiftid");
        var InspectorName = $("#span_inspector").data("value");
        var VATID = $("#vat_id").val();

        var DemoldDefectEntity = {
            ProductionLineID: LineID,
            ShiftID: ShiftID,
            InspectorName: InspectorName,
            StatusID: 'O',
            VATID: VATID,
            DefectDate: null,
            InspectorNameDetail: null
        };

        $(".defect-row").each(function () {
            var DemoldDefectDetailEntity = {
                DemoldDefectID: null,
                MoldFamilyID: $("#span_familyid").val(),
                BaseID: parseInt($("#span_baseid").val()),
                AdditionID: parseInt($("#span_aditionid").val()),
                SideEyeID: $("#span_sideid").val(),
                Quantity: parseInt($(this).find("td:nth-child(2)").text()),
                DemoldDefectTypeID: $(this).data("entityid"),
                InspectorNameDetail: $(this).find("td:nth-child(3)").text().split(':')[3].trim(),
                ProductID: $("#ProductID").val(),
                LensTypeID: $("#span_lentypeid").val()
            };
            DefectMoldDetailsEntities.push(DemoldDefectDetailEntity);
        });

        //var path = window.location.href;
        //var index = path.indexOf("?");
        //var host = path.substring(0, index);


        $.post("/DemoldDefects/Save", { DemoldDefectEntity, DefectMoldDetailsEntities, DefectDate, ProductID, LensGross }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);
            if (data.ErrorCode == 0) {
                $("#wizard_percbar_div").css("width", "100%");
                $("#wizard_percbar_span").text("100");

               var host = window.location.origin + "/MFG/DemoldDefects/ReloadCaptureWizard?DemoldDefectID=" + data.ID;
                window.location.href = host;

            } else {
                HideProgressBar();
            }
        }).always(function () {

        });
    });

    $(document).on("change", "#chk_BiggerQty", function () {
        if ($(this).is(":checked")) {
            $("#txt_biggerQty").prop("disabled", false);
            $("#defect_quantity").css("color", "lightgray");
        } else {
            $("#txt_biggerQty").prop("disabled", true);
            $("#defect_quantity").css("color", "black");
        }
    });

    //Seccion del Wizard

    updateNav("family");
    updateNav("base");
    updateNav("adition");
    updateNav("side");
    updateNav("defects");

    function enableToSave() {
        if ($(".card-front-family").find("h10").length > 0 && $(".card-front-base").find("h10").length > 0
            //&& $(".card-front-adition").find("h10").length > 0
            //&& $(".card-front-side").find("h10").length > 0
        ) {
            $("#btn_next_slideform").prop('disabled', 0);
        }
    }

    function updateNav(ControlName) {
        var className = "." + ControlName + "toRight";
        var hasAnyRemovedCard = $(className).length ? true : false,
            isCardLast = $(".card-middle-" + ControlName).length ? false : true;

        if (hasAnyRemovedCard) {
            $("#btn-back-" + ControlName).removeClass('back-btn-hide');
        } else {
            $("#btn-back-" + ControlName).addClass('back-btn-hide');
            $(".card-front-" + ControlName).addClass('noBack');
        }

        if (isCardLast) {
            $("#btn-next-" + ControlName).hide();
            $("#btn-finish-" + ControlName).removeClass("hide");
        } else {
            $("#btn-next-" + ControlName).show();
            $("#btn-finish-" + ControlName).addClass("hide");
        }

        enableToSave();
    }

    function showNextCard(ControlName) {
        //Check if there is only one card left
        if ($(".card-middle-" + ControlName).length > 0) {
            var currentCard = $(".card-front-" + ControlName),
                middleCard = $(".card-middle-" + ControlName),
                backCard = $(".card-back-" + ControlName),
                outCard = $(".card-out-" + ControlName).eq(0);

            //Remove the front card
            currentCard.removeClass('card-front-' + ControlName).addClass(ControlName + 'toRight');
            //change the card places
            middleCard.removeClass('card-middle-' + ControlName).addClass('card-front-' + ControlName);
            backCard.removeClass('card-back-' + ControlName).addClass('card-middle-' + ControlName);
            outCard.removeClass('card-out-' + ControlName).addClass('card-back-' + ControlName);

            updateNav(ControlName);
        }
    }

    function showPreviousCard(ControlName) {
        var className = "." + ControlName + "toRight";
        var currentCard = $(".card-front-" + ControlName),
            middleCard = $(".card-middle-" + ControlName),
            backCard = $(".card-back-" + ControlName),
            lastRemovedCard = $(className).slice(-1);

        lastRemovedCard.removeClass(ControlName + 'toRight').addClass('card-front-' + ControlName);
        currentCard.removeClass('card-front-' + ControlName).addClass('card-middle-' + ControlName);
        middleCard.removeClass('card-middle-' + ControlName).addClass('card-back-' + ControlName);
        backCard.removeClass('card-back-' + ControlName).addClass('card-out-' + ControlName);

        updateNav(ControlName);
    }

    // Eventos y validaciones que ejecutan el script de slideform

    $(".btn-next").on("click", function () {
        var id = $(this).attr("id");
        var ControlName = id.substring(id.lastIndexOf("-") + 1, id.length);
        showNextCard(ControlName);
        SumAndValidateToSave();

    });

    $(".btn-back").on("click", function () {
        var id = $(this).attr("id");
        var ControlName = id.substring(id.lastIndexOf("-") + 1, id.length);

        if (ControlName == "adition") {
            if ($("#NoAddition").val() == "true") {
                showPreviousCard(ControlName);
                showPreviousCard(ControlName);
            } else {
                showPreviousCard(ControlName);
            }
        } else {
            showPreviousCard(ControlName);
        }
    });

    $("#btn_ShowAll").on("click", function () {
        if ($(this).hasClass("buttonpressed")) {
            $(this).text(LangResources.btn_ShowAll);
            $(this).removeClass("buttonpressed");
            var VATName = $('#span_vat_number').text();
            var ProductionProcessID = $("#production_process_id").val();
            var ProductionLineName = $('#span_line').text();
            ShowProgressBar();
            $.get("/MFG/DemoldDefects/GetProductionFamiliesList", { VATName, ProductionProcessID, ProductionLineName }).done(function (data) {
                $("#families_container").empty();
                $.each(data.list, function (k, value) {
                    $("#families_container").append(
                        '<button id="' + value.CatalogDetailID + '" type="button" class="btn-info btn-pretty btn-lg familyoption col-sm-2 " ' +
                        'data-moldfamilyid="' + value.CatalogDetailID + '"' +
                        'data-moldfamilyname="' + value.ValueID + '" >' + value.ValueID +
                        ' </button >'
                    );
                });
                HideProgressBar();
            });
        } else {
            $(this).text(LangResources.btn_ShowScheduledFamilies);
            $(this).addClass("buttonpressed");
            ShowProgressBar();
            $.get("/MFG/DemoldDefects/GetAllFamiliesList").done(function (data) {
                $("#families_container").empty();
                $.each(data.list, function (k, value) {
                    $("#families_container").append(
                        '<button id="' + value.CatalogDetailID + '" type="button" class="btn-info btn-pretty btn-lg familyoption col-sm-2" ' +
                        'data-moldfamilyid="' + value.CatalogDetailID + '"' +
                        'data-moldfamilyname="' + value.DisplayText + '" >' + value.DisplayText +
                        ' </button >'
                    );
                });
                HideProgressBar();
            });
        }

    });

    $(document).on("click", "#btn_prev_slideform", function () {
        $("#div_btn_next").css("display", "block");
        $("#div_btn_save").css("display", "none");
        $("#btn_prev_slideform").prop("disabled", true);
        $(".slideform-btn-prev").click();
        SetupxEditable();
    });

    $(document).on("click", "#btn_next", function (e) {
        var CaseControl = 1;
        var IsSimpleVision = $(".lentypesoption.choosed").data("issimplevision");
        var LenType = $("#span_lentype").text();
        var Base = $("#span_base").text();
        var Addition = $("#span_adition").text();
        var Side = $("#span_side").text();

        if (IsSimpleVision == "1") {
            Addition = null;
            Side = null;
        }

        var VAT = $("#span_vat_number").data("title");
        var ProductionProcessID = $("#production_process_id").val();
        var Line = $("#span_line").data("title");

        var LineID = $("#span_line").data("entityid");
        var VATID = $("#vat_id").val();
        var InspectorName = $("#span_inspector").data("value");

        ShowProgressBar();
        //Valida si el producto existe
        $.get("/MFG/DemoldDefects/ValidateToNextFase", { CaseControl, LenType, Base, Addition, Side, VAT, ProductionProcessID, Line }).done(function (data) {
            if (data.ErrorCode === 0) {
                var lensdata = data.ErrorMessage.split("|");

                $("#span_lenssku").text(lensdata[0]);
                $("#span_lensdesc").text(lensdata[1]);
                $("#span_lensgasket").text(lensdata[2]);
                $("#span_plannigqty").text(lensdata[4]);
                $("#ProductID").val(lensdata[5]);

                if (IsSimpleVision == "1") {
                    $("#span_adition").text("");
                    $("#span_side").text("");
                }


                $.get("/MFG/DemoldDefects/FillDefectTable", {
                    ProductionLineID: LineID,
                    VATID: VATID,
                    InspectorName: InspectorName,
                    ProductID: $("#ProductID").val()
                }).done(function (data) {
                    var Sumatoria = 0;
                    $("#defect_table tbody").empty();
                    $.each(data.list, function () {
                        Sumatoria = $(this)[0].Quantity;
                        $("#defect_table tbody").prepend( // cambio de append
                            '<tr class="defect-row" data-entityid="' + $(this)[0].DemoldDefectTypeID + '">' +
                            '   <td>' +
                            $(this)[0].DemoldDefectCategoryName + ' - ' + $(this)[0].DemoldDefectTypeName +
                            '   </td>' +
                            '   <td>' +
                            $(this)[0].Quantity +
                            '   </td>' +
                            '<td>' +
                            LangResources.lbl_On + ' : ' + $(this)[0].DateLastMaintFormat +
                            '<br /> ' +
                            LangResources.lbl_By + ' : ' + $(this)[0].InspectorNameDetail +
                            '</td> ' +
                            '   <td style="width:15%; text-align:center">' +
                            '       <button class="btn btn-danger delete-defect" type="button" title="' + LangResources.tt_Edit + '">' +
                            '       <span class="glyphicon glyphicon-remove-circle"></span >' +
                            '       </button>' +
                            '   </td>' +
                            '</tr>'
                        );
                    });
                    $("#total_defects").text(Sumatoria);
                    //$form.goForward();
                    HideProgressBar();
                    $("#btn_prev_slideform").prop("disabled", false);
                    $("#div_btn_next").css("display", "none");
                    if (data.list.length > 0) {
                        $("#div_btn_save").css("display", "block");
                    }
                    $("#btn_next_slideform").click();


                    $(".FilterDefects").editable("destroy");
                });



            } else {
                HideProgressBar();
                $("#message-box-generic-alert-title").text(LangResources.lbl_Warning);
                $("#message-box-generic-alert-legend").text(data.ErrorMessage);
                $('#message-box-generic-alert').toggleClass("open");
            }
        });

    });

}