// =============================================================================================================================
//  Version: 20190924
//  Author:  Luis Hernandez
//  Created Date: 24 Sep 2019
//  Description:  MoldFamilies JS
//  Modifications: 
// =============================================================================================================================
function IndexInit(LangResources) {

    //Carga los datos de la tabla tomando en cuenta los filtros
    function LoadMoldFamiliesTable() {
        var MoldFamiliesID = $("#ddl_FamilyNames").val();
        var MoldLensTypesID = $("#ddl_LensTypes").val();

        if (MoldFamiliesID === null) {
            MoldFamiliesID = [];
        }
        if (MoldLensTypesID === null) {
            MoldLensTypesID = [];
        }


        jQuery.ajaxSettings.traditional = true;
        $.ajax({
            url: '/MFG/MoldFamilies/Search',
            type: 'get',
            traditional: true,
            dataType: "json",
            contextType: "application/json",
            data: { MoldFamiliesID, MoldLensTypesID }
        }).done(function (data) {
            $("#div_MoldFamiliesRecordsTable").html(data.View);
            $("#div_MoldFamiliesRecordsTable").css("display", "block");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    }

    //Esta funcion recoarga la lista de familias por si se agrego una nueva o se edito una existente, de este modo se reflejará el cambio al instante
    function ReloadMoldFamiliesList() {
        var optionSelected = $("#ddl_FamilyNames option:selected").val();
        $.get("/MFG/MoldFamilies/GetMoldFamiliesList").done(function (data) {
            $("#ddl_FamilyNames").empty();
            $.each(data.list, function (key, value) {
                $("#ddl_FamilyNames").append("<option value='" + value.Value + "'>" + value.Text + "</option>");
            });

            $("#ddl_FamilyNames").val(optionSelected);
            $("#ddl_FamilyNames").selectpicker("refresh");
        });
    }

    //Asigna los valores iniciales de los filtros y el plugin Selectpicker
    (function SetupDefaultValues() {
        $("#ddl_FamilyNames").val(0);
        $("#ddl_LensTypes").val(0);

        if ($("#ExcelImportMessage").val() != "") {
            notification("", $("#ExcelImportMessage").val(), "success");
        }

        $(".selectpicker").selectpicker();
    }());

    $("#btn_New").on("click", function () {
        $('.loading-process-div').show();
        $.get("/MFG/MoldFamilies/New").done(function (data) {
            $("#_mo_NewEdit").html(data.View);
            $("#mo_NewEditMoldFamily").modal("show");

            $("#txt_mo_LenTypes").selectpicker();
            $('.max-length').maxlength();

            $(".slider").removeClass("slider");
            $(".text_center").css("text-align", "center");
            $('.loading-process-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $("#btn_Search").on("click", function () {
        LoadMoldFamiliesTable();
    });

    $(".filters").on("change", function () {
        $("#div_MoldFamiliesRecordsTable").css("display", "none");
    });

    $("#btn_excel").on("click", function () {
        $("#file").click();
    });

    $("#file").on("change", function () {
        $("#submit_Excelinfo").click();
        $('.loading-process-div').show();

    });

    //expandir detalles con plugin DataTable
    $(document).on('click', 'td.details-control', function (e) {
        e.stopPropagation();

        var tr = $(this).closest('tr');
        var MoldLensTypeIDs = $("#ddl_LensTypes").val();

        if (MoldLensTypeIDs === null) {
            MoldLensTypeIDs = [];
        }

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {

            var MoldFamilyID = tr.data("entityid");

            ShowProgressBar();

            // abrir detalles y devolver detalles de una llamada ajax
            jQuery.ajaxSettings.traditional = true;
            $.ajax({
                url: '/MFG/MoldFamilies/LoadLensOfFamily',
                type: 'get',
                traditional: true,
                dataType: "json",
                contextType: "application/json",
                data: { MoldFamilyID, MoldLensTypeIDs }
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="6" class="padding-0">' + data.View + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }
    });

    $(document).on("click", "#addLenToFamily", function () {
        var IsValidToAdd = true;
        var LensTypeName = $("#txt_mo_LenTypes option:selected").text();
        var LensTypeID = $("#txt_mo_LenTypes option:selected").val();

        if (LensTypeID != 0) {

            $("#table_LensTypes tr").each(function () {
                if (LensTypeID == $(this).data("entityid")) {
                    notification("", LangResources.msg_LenTypeAlreadyAssigned, "_ntf");
                    IsValidToAdd = false;
                }
            });

            if (IsValidToAdd) {
                $("#table_LensTypes").append(
                    '<tr data-entityid="' + LensTypeID + '" class="LenTypesRow">' +
                    '   <td>' + LensTypeName + '</td>' +
                    '    <td text-align: center">' +
                    '        <button class="btn btn-danger delete-moldfamily-record"> <span class="glyphicon glyphicon-trash"></span></button>' +
                    '    </td>' +
                    '</tr >'
                );
            }
        } else {
            notification("", LangResources.msg_SelectLensType, "_ntf");
        }
    });

    $(document).on("click", "#btn_ModalSaveNewMoldFamily", function () {
        var MoldFamilyID = null;
        var MoldFamilyName = $("#txt_mo_FamilyName").val();
        var LensTypeIDs = "";
        var Enabled = $("#chk_Enabled").is(":checked");

        $(".LenTypesRow").each(function () {
            LensTypeIDs += "," + $(this).data("entityid");
        });

        LensTypeIDs = LensTypeIDs.replace(",", "");
        $.post("/MFG/MoldFamilies/Save", { MoldFamilyID, MoldFamilyName, LensTypeIDs, Enabled }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#mo_NewEditMoldFamily").modal("toggle");
                ReloadMoldFamiliesList();
                LoadMoldFamiliesTable();
            };
            notification("", data.ErrorMessage, data.notifyType);
        });
    });

    $(document).on("click", "#btn_ModalSaveEditedMoldFamily", function () {
        var MoldFamilyID = $("#MoldFamilyID").val();
        var MoldFamilyName = $("#txt_mo_FamilyName").val();
        var LensTypeIDs = "";
        var Enabled = $("#chk_Enabled").is(":checked");

        $(".LenTypesRow").each(function () {
            LensTypeIDs += "," + $(this).data("entityid");
        });

        LensTypeIDs = LensTypeIDs.replace(",", "");
        $.post("/MFG/MoldFamilies/Save", { MoldFamilyID, MoldFamilyName, LensTypeIDs, Enabled }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#mo_NewEditMoldFamily").modal("toggle");
                LoadMoldFamiliesTable();
                ReloadMoldFamiliesList();
            };
            notification("", data.ErrorMessage, data.notifyType);
        });
    });

    $(document).on("click", ".delete-lentype-record", function () {
        var row = $(this).closest("tr");
        var lens = $(this).closest("#table_MoldFamilies tbody").find("tr.LenTypesRow.table_record").length;//row.find("td:nth-child(3)").text();

        if (row.hasClass("table_record")) {
            if (lens > 1) {
                var MoldFamilyLenTypeID = row.data("moldfamilylenid");
                SetConfirmBoxAction(function () {
                    $.post("/MFG/MoldFamilies/DeleteLenTypeFromFamily", { MoldFamilyLenTypeID }).done(function (data) {
                        if (data.ErrorCode == 0) {
                            row.remove();
                            LoadMoldFamiliesTable();
                            notification("", LangResources.msg_MoldFamilyDeleted, data.notifyType);
                        } else {
                            notification("", data.ErrorMessage, data.notifyType);
                        }
                    });
                }, LangResources.msg_DeleteLenTypeRecordConfirm);
            } else {
                notification("", LangResources.msg_LenTypesRequiredInTable, "_nft");
            }
        } else {
            row.remove();
        }
    });

    $(document).on("click", ".delete-moldfamily-record", function () {
        var row = $(this).closest("tr");
        var MoldFamilyID = row.data("entityid");
        SetConfirmBoxAction(function () {
            $.post("/MFG/MoldFamilies/DeleteMoldFamily", { MoldFamilyID }).done(function (data) {
                if (data.ErrorCode == 0) {
                    row.remove();
                    notification("", LangResources.msg_MoldFamilyDeleted, data.notifyType);
                } else {
                    notification("", "error", data.notifyType);
                }
            });
        }, LangResources.msg_DeleteMoldFamilyRecordConfirm);
    });

    $(document).on("click", ".edit-moldfamily-record", function () {
        $('.loading-process-div').show();
        var row = $(this).closest('tr');
        var MoldFamilyID = row.data("entityid");

        if (row.hasClass("shown")) {
            $('div.slider', row.next()).slideUp(function () {
                row.next().remove();
                row.removeClass('shown');
            });
        }

        $.get("/MFG/MoldFamilies/Edit", { MoldFamilyID }).done(function (data) {
            $("#_mo_NewEdit").html(data.View);
            $("#mo_NewEditMoldFamily").modal("show");

            $("#txt_mo_LenTypes").selectpicker();
            $('.max-length').maxlength();

            $(".slider").removeClass("slider");
            $(".text_center").css("text-align", "center");
            $('.loading-process-div').hide();
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });
}