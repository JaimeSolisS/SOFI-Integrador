// =============================================================================================================================
//  Version: 20180701
//  Author:  Guillermo Sánchez
//  Created Date: Domingo 1 de Julio de 2018
//  Description: Contiene funciones JS para la página de Create de la administración de usuarios
//  Modifications: 

// =============================================================================================================================
function CreateInit(LangResources) {
    //Plugins
    $(".select").selectpicker({
        liveSearch: false
    });

    $('#btn_AddUser').on('click', function () {
        $(this).attr('disabled', true);
        $('#content').hide();
        $('.spinner').removeClass('hide');
        var ProfileField = $('#ProfileID').val();
        var ProfileArrayID = "";

        if (ProfileField != null) {
            for (var i = 0; i < ProfileField.length; i++) {
                ProfileArrayID = ProfileField[i] + "," + ProfileArrayID ;
            }
        }
        $.post("/Users/Create",
            $('#create-form').serialize() + "&ProfileArrayID=" + ProfileArrayID,
            function (data) {
                notification(data.Title, data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    window.location.href = '/Administration/Users/Edit/Edit?UserID=' + data.ID;
                }
            }).done(function (data) {
                $('.spinner').addClass('hide');
                $('#content').show();
                $('#btn_AddUser').attr('disabled', false);
            });
    });

    //$('.searchuser').hide();
    RegisterStyleSearchUser();
    RegisterFindUser();
}

function RegisterStyleSearchUser() {
    $('#filter-form').find("#UserAccountID").keydown(function () {
        $('.searchuser').hide();
    });
}

function RegisterFindUser() {
    $('#btn_SearchUser').on('click', function () {
        // Efecto de carga
        ShowProgressBar();
        var UserAccountID = $('#UserAccountID');

        if (UserAccountID.val() != '') {
            var SearchUserRequest = CallWebMethodPOST('/Users/SearchOnAD', { txt_F_UserAccount: UserAccountID.val() });

            SearchUserRequest.success(function (data) {
                if (data.FirstName != '') {
                    $('#FirstName').val(data.FirstName);
                    $('#LastName').val(data.LastName);
                    $('#eMail').val(data.EMail);
                    $('.searchuser').show();
                }
            });
        }
    });
}