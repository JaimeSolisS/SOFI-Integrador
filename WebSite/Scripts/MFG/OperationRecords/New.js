// ==========================================================================================================================================================
//  Version: 20190524
//  Author:  cgarcia
//  Created Date: 24 de may de 2019
//  Description: Contiene funciones JS para la página de new
//  Modifications: 
// ==========================================================================================================================================================
function NewInit(LangResources) {
    //Plugins
    $(".select").selectpicker({
        liveSearch: false
    });
    $('.datepicker').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd'
        //language: LangResources.datepicker_lang
    });

    var EmployeeNumber = localStorage.getItem('MFG_OperationRecords_EmployeeNumber');
    if (EmployeeNumber !== null || EmployeeNumber.length !== 0) {
        $('#OperatorNumber').val(EmployeeNumber);
    }

    //eventos de elemtos estaticos 
    $('#create_operation').on('click', function () {
        var MachineID = $('#ddl_Machine').val();
        var MachineSetupID = $('#ddl_Setup').val();
        var MaterialID = $('#ddl_Material').val();
        var OperatorNumber = $('#OperatorNumber').val();
        $('.loading-process-div').show();

        $.post("/MFG/OperationRecords/Create", {
             MachineID ,
             MachineSetupID ,
             MaterialID ,
             OperatorNumber 
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                window.location = "/MFG/OperationRecords/Edit?OperationRecordID=" + data.ID;
            } else {
                $('.loading-process-div').hide();
                notification("", data.ErrorMessage, data.notifyType);
            }
            }).always(function () {
                
            });
        
    });
    $('#cancel').on('click', function () {
        $('.loading-process-div').show();
        //mandar el machineid como opcional
        var MachineID = $("#ddl_Machine").val();
        window.location = "/MFG/OperationRecords/List?MachineID=" + MachineID;
    });
    $("#ddl_Machine").change(function () {
        GetSetupsDDL();
    });
    $("#ddl_Material").change(function () {
        GetSetupsDDL();
    });
    //funciones aux
    function GetSetupsDDL() {
        var MachineID = $('#ddl_Machine option:selected').val();
        var MaterialID = $('#ddl_Material option:selected').val();
        if (MachineID !== 0 && MaterialID !=0 ) {
            $('.loading-process-div').show();
            $.get("/MFG/OperationRecords/GetSetupsDDL", { MachineID, MaterialID }
            ).done(function (data) {
                if (data.ErrorCode == 0) {
                    //console.log(data.List);
                    $('#ddl_Setup').empty();
                    $.each(data.List, function (index, item) {
                        $('#ddl_Setup').append($('<option></option>').text(item.Text).val(item.Value));
                    });
                    $('#ddl_Setup').selectpicker('refresh');
                }

            }).always(function () {
                $('.loading-process-div').hide();
            });
        }


    }

    $('#btn_exit').on('click', function () {
        window.close();
    });
}