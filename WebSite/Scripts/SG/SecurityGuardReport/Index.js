// =============================================================================================================================
//  Version: 20200816
//  Author:  Luis Hernandez
//  Created Date: 04 Julio 2020
//  Description:  SecurityGuardReport JS
//  Modifications: 

// =============================================================================================================================
function IndexInit(LangResources) {

    $('.dateFilters').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        language: LangResources.culture
    });

    $("#ddl_CheckInPersonTypes").selectpicker('selectAll');

    $(".selectpicker").selectpicker("refresh");

    $("#chk_EnableDates").change(function () {
        if ($(this).is(":checked")) {
            $(".dateFilters").prop("disabled", false);
        } else {
            $(".dateFilters").prop("disabled", true);
        }
    });

    $("#btnSearchSecurityLogs").click(function () {
        var CheckInPersonTypes = "";
        var CheckTypeID = null;
        var StartDate = null;
        var EndDate = null;

        if ($("#ddl_CheckInPersonTypes").val() != null) {
            CheckInPersonTypes = $("#ddl_CheckInPersonTypes").val().join(",");
        };

        if ($("#chk_EnableDates").is(":checked")) {
            CheckTypeID = $("#ddl_DateTypes").val();
            StartDate = $("#txt_StartDate").val();
            EndDate = $("#txt_EndDate").val();
        }

        ShowProgressBar();
        $.get("/SG/SecurityGuardReport/Search", {
            CheckInPersonTypes: CheckInPersonTypes,
            EmployeeNumber: $("#txt_EmpNum").val(),
            PersonName: $("#txt_PersonName").val(),
            VehiclePlates: $("#txt_Plates").val(),
            CheckTypeID: CheckTypeID,
            StartDate: StartDate,
            EndDate: EndDate
        }).done(function (data) {
            $("#divSecurityGuardLog").html(data);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(".securityLogsFilters").change(function () {
        $("#divSecurityGuardLog").empty();
    });

    $(".securityLogsFilters").keypress(function () {
        $("#divSecurityGuardLog").empty();
    });

    $(document).on('click', 'td.details-control', function (e) {
        e.stopPropagation();

        var tr = $(this).closest('tr');
        var securityGuardLogID = $(this).data("securityguardlogid");

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {
            ShowProgressBar();
            $.get("/SG/SecurityGuardReport/GetSecurityGuardTools", {
                SecurityGuardLogID: securityGuardLogID
            }).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan="10" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                HideProgressBar();
            });
        }
    });


}