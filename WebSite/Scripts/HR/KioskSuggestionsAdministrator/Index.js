// =============================================================================================================================
//  Version: 20200211
//  Author:  Luis Hernandez
//  Created Date: 11 Febrero 2020
//  Description:  Scripts para el administrador de sugerencias
//  Modifications: Se agrego la notificacion de cuando se va a borrar
// =============================================================================================================================

function IndexInit(LangResources) {

    //Functions
    function EnableDateFilterSuggestion() {
        if ($('.check-range-date').is(':checked')) {
            $('#checkdate').val('true');
            $('#txt_SuggestionStartDate').prop("disabled", false);
            $('#txt_SuggestionEndDate').prop("disabled", false);
        } else {
            $('#checkdate').val('false');
            $('#txt_SuggestionStartDate').prop("disabled", true);
            $('#txt_SuggestionEndDate').prop("disabled", true);
        }
    }
    function SearchSuggestions(pageNumber) {
        var CategoryID = $("#ddl_CommentsCategories option:selected").val();
        var StartDate = $("#txt_SuggestionStartDate").val();
        var EndDate = $("#txt_SuggestionEndDate").val();
        var FacilityIDs = $("#ddl_Facilities").val();

        if (!($("#checkbox-filter-date").is(":checked"))) {
            StartDate = null;
            EndDate = null;
        }

        if (FacilityIDs != null) {
            FacilityIDs = $("#ddl_Facilities").val().join();
        } else {
            FacilityIDs = "";
        }

        ShowProgressBar();
        $.get("/KioskSuggestionsAdministrator/Search", {
            CategoryID,
            FacilityIDs,
            StartDate,
            EndDate
        }).done(function (data) {
            $("#div_Tbl_KioskSuggestions").html(data);
            EnableDataTable(100);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    }
    function EnableDataTable(pageSize) {
        $(".datatable").dataTable({
            "language": {
                "url": "/Base/DataTableLang"
            },
            "pageLength": pageSize,
            "order": [[5, "desc"]],
            responsive: true
        });
    }

    //Initialize
    EnableDateFilterSuggestion();
    $("#checkbox-filter-date").iCheck();
    $("select").selectpicker();
    $('#ddl_Facilities').selectpicker('selectAll');
    $(".customdatepicker").datepicker({ format: 'yyyy-mm-dd' });
    EnableDataTable(100);

    //Events
    $(document).on("ifChanged", ".check-range-date", function (e) {
        e.stopImmediatePropagation();
        EnableDateFilterSuggestion();
    });

    $(document).on("click", "#btn_SuggestionSearch", function (e) {
        e.stopImmediatePropagation();
        SearchSuggestions(1);
    });

    $(document).on("change", ".SuggestionFilters", function (e) {
        e.stopImmediatePropagation();
        $("#div_Tbl_KioskSuggestions").empty();
    });

    //$(document).on("click", ".delete-suggestion", function () {
    //	$.post("/KioskSuggestionsAdministrator/Delete", {
    //		KioskEmployeeSuggestionID: $(this).data("kioskemployeesuggestionid")
    //	}).done(function (data) {
    //		notification("", data.ErrorMessage, data.notifyType);
    //		if (data.ErrorCode == 0) {
    //			SearchSuggestions(1);
    //		}
    //	});
    //});

    $(document).on("click", ".delete-suggestion", function (e) {
        e.stopImmediatePropagation();
        var id = $(this).data("kioskemployeesuggestionid");
        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.post("/KioskSuggestionsAdministrator/Delete", {
                KioskEmployeeSuggestionID: id
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    SearchSuggestions(1);
                }
            }).always(function () {
                $('.loading-process-div').hide();
            });;
        }, LangResources.msg_DeleteSuggestionConfirm);
    });
}