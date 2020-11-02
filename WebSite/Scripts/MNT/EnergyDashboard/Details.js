function DetailsInit(LangResources) {
    $(document).on('click', '.energy-sensor-image', function () {
        var EnergySensorID = $(this).data("energysensorid");
        var EnergySensorFamilyID = $("#EnergySensorFamilyID").val();
        window.location = "/MNT/EnergyDashboard/Chart?EnergySensorID=" + EnergySensorID + "+&EnergySensorFamilyID=" + EnergySensorFamilyID;
    });
}