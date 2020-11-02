// =============================================================================================================================
//  Version: 20190209
//  Author: Carlos Garcia
//  Created Date: 09 feb 2019
//  Description: Contiene funciones JS para la página de Index de Profiles
//  Modifications: 
// =============================================================================================================================
// Variables
var app = {
    Controllers: {
        Administration: {
            Profiles: '/Administration/Profiles/'
        }
    }
};

$(".select").selectpicker({
    //liveSearch: false
});

$(function () {

    RegisterPluginDataTable(100);
    $('#btn_add_profile').on('click', function () {
        $.post("/Administration/Profiles/Create",
            { ProfileName: $('#NewProfileName').val() }
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                $('#m_Create').modal('toggle');
                FillProfileTable();
            }
            notification('', data.ErrorMessage, data.notifyType);
        });
    });

    //eventos de elementos dinamico
    $(document).on('click', '.delete-profile', function (e) {
        e.stopImmediatePropagation();
        var profileID = $(this).data('entityid');

        /* Control generico para mostrar mensajes */
        var deleteProfileBox = new ConfirmBox();
        deleteProfileBox.yesTag = $('#lbl_DeleteButtonTag').text();
        deleteProfileBox.noTag = $('#lbl_CancelButtonTag').text();
        deleteProfileBox.title = $('#lbl_TitleDeleteProfileTag').text();
        deleteProfileBox.msg = $('#lbl_MsgDeleteProfileTag').text();
        deleteProfileBox.showMsg('warning');

        /* funcion OnClick de  */
        deleteProfileBox.onAccept = function () {

            var ProfileDeleteParams = {
                ProfileID: parseInt(profileID)
            };

            var DeleteDefectDetailsRequest = CallWebMethodPOST('/Administration/Profiles/Delete', ProfileDeleteParams);

            DeleteDefectDetailsRequest.success(function (data) {
                if (data.ErrorCode === 0) {
                    FillProfileTable();
                } 
                notification(data.Title, data.ErrorMessage, data.notifyType);
            });
        };

    });

    // main functions
    $('.applyfilters').on('change', function (e) {
        ForceApplyFilters();
        FillProfileTable();
    });

    function ForceApplyFilters() {
        $(".filters").hide();
    }
});

function IndexGetControls() {
    this.btn_add_profile = $('#btn_add_profile');
    this.ddl_F_Profile = $('#ddl_F_Profile');
    this.div_boxProfileInfo = $('#div_boxProfileInfo');
}

function FillProfileTable() {
    try {
        var controls = new IndexGetControls();
        var ProfileID = 0;

        if (controls.ddl_F_Profile.val() !== '') {
            ProfileID = parseInt(controls.ddl_F_Profile.val());
        }

        var ProfileRequest = {
            ProfileID: ProfileID
        };

        $.get(app.Controllers.Administration.Profiles + 'Profiles_List', ProfileRequest).done(function (data) {
            controls.div_boxProfileInfo.show();
            controls.div_boxProfileInfo.html('');
            controls.div_boxProfileInfo.html(data);
            RegisterPluginDataTable(100);
        });
    } catch (error) {
        console.log(error.message);
    }
}


$('#btn_new_profile').on('click', function () {
    $('#NewProfileName').val('');
    $('#m_Create').modal();
});