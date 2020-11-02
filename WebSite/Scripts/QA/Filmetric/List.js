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

    $('#new').on('click', function () {
        $('.loading-process-div').show();
        window.location = "/QA/Filmetrics/New";
        $('.loading-process-div').hide();
    });

    $('#btn_F_Search').on('click', function () {
        $('.loading-process-div').show();
        $.post("/QA/Filmetrics/Search",
            {
                ddl_F_Machine: $('#ddl_F_Machine').val(),
                ddl_F_Shift: $('#ddl_F_Shift').val(),
                txt_F_StartDay: $('#txt_F_StartDay').val(),
                txt_F_EndDay: $('#txt_F_EndDay').val()
            }
        ).done(function (data) {
            $('.loading-process-div').hide();
            if (data.ErrorCode === 0) {
                $('#Operations_Div').html('');
                $('#Operations_Div').html(data.View);
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });

    });

};