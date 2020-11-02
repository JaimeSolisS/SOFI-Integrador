// ==========================================================================================================================================================
//  Version: 20190422
//  Author:  cgarcia
//  Created Date: 22 de abril de 2019
//  Description: Contiene funciones JS para la página de list
//  Modifications: 
// ==========================================================================================================================================================

function ListInit(LangResources) {

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
        $('#OperatorNumber').text(EmployeeNumber);
    } 

    $('#ddl_F_Machine, #ddl_F_Shift, #txt_F_StartDay ,#txt_F_EndDay').on('change', function () {
        $('#Operations_Div').hide();
    });


    $('#new').on('click', function () {
        $('.loading-process-div').show();
        window.location = "/MFG/OperationRecords/New";
        $('.loading-process-div').hide();
    });

    $('#btn_F_Search').on('click', function () {
        $('.loading-process-div').show();
        $.post("/MFG/OperationRecords/Search",
            {
                ddl_F_Machine: $('#ddl_F_Machine').val(),
                ddl_F_Shift: $('#ddl_F_Shift').val(),
                txt_F_StartDay: $('#txt_F_StartDay').val(),
                txt_F_EndDay: $('#txt_F_EndDay').val(),
                ddl_F_Status: $('#ddl_F_Status').val()
            }
        ).done(function (data) {
            $('.loading-process-div').hide();
            if (data.ErrorCode === 0) {
                $('#Operations_Div').html('');
                $('#Operations_Div').html(data.View);
                $('#Operations_Div').show();
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });

    });

    $('#btn_exit').on('click', function () {
        window.close();
    });
};