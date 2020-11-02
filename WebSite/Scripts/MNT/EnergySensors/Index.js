function IndexInit(LangResources) {

    $(".selectpicker").selectpicker();

    $("select").change(function () {
        $("#div_EnergySensorsDataPanel").css("display", "none");
    });

    $(document).on("click", ".energySensorCopyAs", function () {
        var EnergySensorID = $(this).closest("tr").attr("id");
        var SensorName = $(this).closest("tr").find("td:nth-child(2)").text().trim();

        $.post("/MNT/EnergySensors/GetModalCopyEnergySensorRecord", { EnergySensorID, SensorName }).done(function (data) {
            if (data.ErrorCode == 0) {
                $("#div_Mo_CopyEnergySensor").html(data.View);
                $("#mo_CopyEnergySensor").modal("show");
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });
    });

    $(document).on("click", ".energySensorDelete", function () {
        var row = $(this).closest("tr");
        var EnergySensorID = row.attr("id");

        SetConfirmBoxAction(function () {
            $('.loading-process-div').show();
            $.post("/MNT/EnergySensors/DeleteEnergySensor", { EnergySensorID }).done(function (data) {
                if (data.ErrorCode == 0) {

                    notification("", LangResources.msg_SensorDeleted, "success");
                    row.remove();

                } else {
                    notification("", error.message, "error");
                }
            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                $('.loading-process-div').hide();
            });
        }, LangResources.msg_DeleteConfirmationEnergySensor);

    });

    $(document).on("click", ".energySensorEdit", function (e) {
        e.stopImmediatePropagation();
        var EnergySensorID = $(this).closest("tr").attr("id");
        $('.loading-process-div').show();
        window.location = "/MNT/EnergySensors/Edit?EnergySensorID=" + EnergySensorID;
    });

    $("#btn_search").on("click", function () {
        var SensorFamiliesIDs = $("#ddl_Families").val();
        var SensorNames = $("#ddl_SensorNames").val();
        var SensorUsesIDs = $("#ddl_Uses").val();

        if (SensorNames == LangResources.lbl_SelectName) {
            SensorNames = "";
        }

        $.ajax({
            url: '/MNT/EnergySensors/Search',
            type: 'get',
            traditional: true,
            dataType: "json",
            contextType: "application/json",
            data: { SensorFamiliesIDs, SensorNames, SensorUsesIDs }
        }).done(function (data) {
            $("#div_Tbl_EnergySensors").html(data.View);
            $("#div_EnergySensorsDataPanel").css("display", "block");
            $("#div_Tbl_EnergySensors").css("display", "block");

        });
    });

    $(document).on("click", "#btn_SaveEnergySensorCopy", function () {
        var EnergySensorID = $("#EnergySensorID").val();
        var SensorName = $("#txt_NewNameForCopiedRecord").val();

        $.post("/MNT/EnergySensors/SaveCopiedEnergySensorRecord", {
            EnergySensorID, SensorName
        }).done(function (data) {
            if (data.ErrorCode == 0) {

                notification("", LangResources.msg_SensorCopied, "success");
                document.location.reload(true);
            } else {
                //notification("", error.message, "error");
                notification("", data.ErrorMessage, "error");

            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            $('.loading-process-div').hide();
        });

    });

}