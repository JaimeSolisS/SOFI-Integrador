// ==========================================================================================================================================================
//  Version: 20190517
//  Author:  Jorge Reyes
//  Created Date: 17 de mayo de 2019
//  Description: Contiene funciones JS para la página de Index
//  Modifications: 
// ==========================================================================================================================================================

//var app = {
//    Area: {
//        QA: {
//            Filmetric: {
//                DetailInfo_List: "/QA/Filmetric/DetailInfo_List",
//            }
//        }
//    }
//};

    function BtnOnclick(DetailId) {
        $.ajax({
            type: 'POST',
            url: '@Url.Content("Filmetric/DetailInfo_List")',
            data: {
                detailID: DetailId
            },
            success: function (data) {
                $('#divpopup').css("display", "block");
                $('#btnExpand').css("display", "none");
                $('#divpopup')[0].innerHTML = data;
            }
        });
    }
    function CollapseDiv() {
        $('#divpopup').css("display", "none");
    $('#btnExpand').css("display", "block");
    }




//function Details() {
    
//            // abrir detalles y devolver detalles de una llamada ajax
//            $.get(app.Area.QA.Filmetric.DetailInfo_List, detail
//            ).done(function (data) {
//                tr.after('');
//                tr.after('<tr><td colspan = "3" class="padding-0">' + data + '</td></tr>');
//                tr.addClass('shown');
//                $('div.slider', tr.next()).slideDown();
//                RegisterEventDeleteDefectProcess();
//                HideProgressBar();
//            });
//        };


function IndexGetControls() {
    this.FilmetricInspectionBox = $('#Operations_Div');
    this.ddl_ProductionProcess = $('#ddl_ProductionProcess');
    this.div_ContainerControlProductionLine = $('#div_ContainerControlProductionLine');
    this.ddl_ProductionLine = $('#ddl_ProductionLine');
    this.ddl_VA = $("#ddl_VA");

}

function FilmetricInspectionDetailEntity() {
    this.FilmetricInspectionDetailID = 0;
    this.FilmetricInspectionID = 0;
    this.FilmID = 0;
    this.FilmName = '';
    this.Value;

}

function IndexInit(LangResources) {

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
        window.location = "/QA/Filmetric/New";
        $('.loading-process-div').hide(); 
    });


    $('#return').on('click', function () {
        $('.loading-process-div').show();
        window.location = "/QA/Filmetric/";
        $('.loading-process-div').hide();
    });

    $('#dashboard').on('click', function () {
        $('.loading-process-div').show();
        window.location = "/QA/Filmetric/dashboard";
        $('.loading-process-div').hide();
    });

    $('#btn_F_Search').on('click', function () {
        $('.loading-process-div').show();
        $.post("/QA/Controllers/Filmetric/Search",
            {
                ddl_F_Product: $('#ddl_F_Product').val(),
                ddl_F_Substract: $('#ddl_F_Substract').val(),
                ddl_F_Base: $('#ddl_F_Base').val(),
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