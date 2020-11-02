// ==========================================================================================================================================================
//  Version: 20190208
//  Author:  Guillermo Sánchez
//  Created Date: 30 de Jun de 2018
//  Description: Contiene funciones JS para la página de Index de la administración de usuarios
//  Modifications: 
//  G.Sánchez (8 Febrero 2019). cambiar leyendas al eliminar un usuario, no actualizar toda la pagina, solo el listado despues de borrar
// ==========================================================================================================================================================
function IndexInit(LangResources) {

    RegisterPluginDataTable(50);
    RegisterButtonSearch();
    HideMainList();
    $(".select").selectpicker({
        liveSearch: false
    });
    RegisterDisableButton();

    $(document).on('click', '.edit-user', function (e) {
        e.stopImmediatePropagation();
        var EntityID = $(this).data('entityid');
        window.location.href = '/Administration/Users/Edit?UserID=' + EntityID;
    });
}

function RegisterButtonSearch() {
    $('#btn_search').on('click', function () {
        // Efecto de carga
        ShowProgressBar();

        $.post("/Users/Search",
            $('#filter-form').serialize(),
            function (data) {
                $('#table_Users').html('');
                $('#table_Users').html(data);
            }).done(function (data) {
                // Finalizar efecto de carga
                HideProgressBar();

                $("#table_Users").show();
                RegisterPluginDataTable(50);
            });
    });
}


function HideMainList() {
    $('.applyfilters').on('change', function (e) {
        $('#table_Users').hide();
    });
}

function ForceApplyFilters() {
    $('#table_Users').hide();
}

function RegisterDisableButton() {
    $(document).on('click', '.disable-user', function (e) {
        e.stopImmediatePropagation();

        var UserID = $(this).data('entityid');

        var userDeleteParams = {
            UserID: parseInt(UserID)
        };

        /* Control generico para mostrar mensajes */
        var deleteUserBox = null;
        deleteUserBox = new ConfirmBox();
        deleteUserBox.yesTag = $('#lbl_DeleteButtonTag').text();
        deleteUserBox.noTag = $('#lbl_CancelButtonTag').text();
        deleteUserBox.title = $('#lbl_TitleDeleteUserTag').text();
        deleteUserBox.msg = $('#lbl_MsgDeleteUserTag').text();
        deleteUserBox.doClickYes(userDeleteParams);
        deleteUserBox.showMsg('warning');

        /* funcion OnClick de  */
        deleteUserBox.onAccept = function (data) {

            var DeleteUserRequest = CallWebMethodPOST('/Users/Disable', data);

            DeleteUserRequest.success(function (data) {
                if (data.ErrorCode == 0) {
                    notification(data.Title, data.ErrorMessage, data.notifyType);
                    $('#btn_search').click();
                } else {
                    notification(data.Title, data.ErrorMessage, data.notifyType);
                }
            });
        };

    });
}