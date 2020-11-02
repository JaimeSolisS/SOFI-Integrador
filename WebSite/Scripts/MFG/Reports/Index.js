// =============================================================================================================================
//  Version: 20191208
//  Author:  cgarcia
//  Created Date: 12 ago 2019
//  Description:  js para index de reporteador
//  Modifications: 
// =============================================================================================================================

function IndexInit(LangResources) {
    //inicializa a 0 el listado
    $('#report_list').val(0).selectpicker('refresh');

    ///////////////Config de Plugins///////////////////
    $('.datepicker').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd'
        //language: LangResources.datepicker_lang
    });

    $(".datetimepicker").datetimepicker({
        format: 'YYYY[-]MM[-]DD HH:mm'
    });

    $('.select').selectpicker('refresh');

    $(".multiple-select").multiselect({
        enableFiltering: true,
        enableCaseInsensitiveFiltering: false,
        includeSelectAllOption: true,
        buttonWidth: '100%',
        allSelectedText: LangResources.TagAll,
        nSelectdText: LangResources.noneSelected
    });

    $(".multiple-select").multiselect('selectAll', false);
    $(".multiple-select").multiselect('updateButtonText');


    $('#report_list').on('change', function () {
        var report = $('#report_list').val();
        $('.params_panel').hide();
        $('#params_report_' + report).show();
    });

}