// =============================================================================================================================
//  Version: 20190209
//  Author: Carlos Garcia
//  Created Date: 09 ene  2019
//  Description: Contiene funciones JS para la página de edit de Profiles
//  Modifications: 

// =============================================================================================================================
function EditInit(LangResources) {
    //configuration of jstree for show menus as tree
    $('#AppProfileMenus').jstree({
        plugins: ['checkbox', 'types', 'search', 'contextmenu'],
        "types": {
            "default": {
                icon: 'fa fa-bars'
            }
        },
        "contextmenu": {
            "items": function ($node) {
                var tree = $("#AppProfileMenus").jstree(true);
                return {
                    "Create": {
                        "separator_before": false,
                        "separator_after": false,
                        "label": LangResources.lbl_SelectEvents,
                        //"icon": "/Content/img/icons/more.png",
                        "action": function (obj) {
                            $.get("/Profiles/ShowEvents",
                                {
                                    ProfileID: $('#ProfileID').val(), MenuID: $node.id
                                })
                                .done(function (data) {
                                    debugger;
                                    if (data.ErrorCode == 0) {
                                        $("#events_table").html(data.View);
                                        $('#event_dashboard').show();
                                    }
                                });
                        }
                    }
                };
            }
        },
        'core': {
            "multiple": true,
            "check_callback": false,
            'themes': {
                "responsive": true,
                'variant': 'larg',
                'stripes': false,
                'dots': false
            },
            'data': {
                'url': '/Profiles/AppMenus?ProfileArrayID=' + $('#ProfileID').val(),
                'data': function (node) {
                    return { 'id': node.id };
                }
            }
        }
    }).on('show_contextmenu.jstree', function (e, reference, element) {
        if (reference.node.children.length > 0) {
            $('.vakata-context').remove();
        }
    });
    //plugin for search in jstree
    $('#jstree-search').on('input', function () {
        $('#AppProfileMenus').jstree(true).search($(this).val());
    });
    //plugin to show checkbox in jstree
    $('#AppProfileMenus').on('ready.jstree', function (e, data) {
        $(this).addClass('jstree-custom-checkboxes');
    });
    //get al the selected checkbox of jstree
    $('#AppProfileMenus').on('changed.jstree', function (e, data) {
        var checked_ids = $("#AppProfileMenus").jstree("get_checked");
        $('#SelectedMenusId').val(checked_ids.join(","));
    }).jstree();


    //eventos de elementos estaticos
    $('#btn_Save').on("click", function () {
        var EventsIDs = '';

        $("#events_table").find(" tr").each(function () {
            var TypeID = "0";
            var tdlist = $(this).find("td");
            var Checked = $(tdlist[0]).find(".Event").prop("checked");
            var ID = $(tdlist[0]).find(".Event").data("id");
            if (Checked) {
                var ReadOnly = $(tdlist[1]).find(".ReadOnly").prop("checked");
                if (ReadOnly) { TypeID = $(tdlist[1]).find(".ReadOnly").data("id"); }

                var FullAccess = $(tdlist[1]).find(".FullAccess").prop("checked");
                if (FullAccess) { TypeID = $(tdlist[1]).find(".FullAccess").data("id"); }

                EventsIDs = EventsIDs + ID + "-" + TypeID + ",";
            }
        });

        if (EventsIDs.length > 1) {
            EventsIDs = EventsIDs.slice(0, -1);
        }

        // Guarda los datos de menus seleccionados
        $('.loading-process-div').show();
        $.post("/Profiles/Edit",
            {
                ProfileID: $('#ProfileID').val(),
                ProfileName: $('#EditProfile_ProfileName').val(),
                OrganizationID: $('#OrganizationID').val(),
                SelectedMenusId: $('#SelectedMenusId').val(),
            },
            function (data) {
                if (data.ErrorCode > 0) {
                    notification(data.Title, data.ErrorMessage, data.notifyType);
                    return false;
                }
            }, "json").done(function () {
                $('#btn_Save').focus();
            }).always(function () {
                $('.loading-process-div').hide();
            });

        // Guardar los eventos del último menú
        var _MenuID = $("#events_table").find("table").data("menuid");
        if (_MenuID !== null && _MenuID !== undefined) {
            $('.loading-process-div').show();
            $.post("/Profiles/UpdateEvents",
                {
                    ProfileID: $('#ProfileID').val(),
                    MenuID: _MenuID,
                    SelectedEventsId: EventsIDs
                }).done(function (data) {
                    if (data.ErrorCode === 0) {
                        $('.loading-process-div').show();
                        $.post("/Profiles/Edit",
                            {
                                ProfileID: $('#ProfileID').val(),
                                ProfileName: $('#EditProfile_ProfileName').val(),
                                OrganizationID: $('#OrganizationID').val(),
                                SelectedMenusId: $('#SelectedMenusId').val(),
                            },
                            function (dataProfile) {
                                notification(dataProfile.Title, dataProfile.ErrorMessage, dataProfile.notifyType);
                            }, "json").done(function () {
                                $('#btn_Save').focus();
                            }).always(function () {
                                $('.loading-process-div').hide();
                            });
                    }
                    //notification(data.Title, data.ErrorMessage, data.notifyType);
                }).always(function () {
                    $('.loading-process-div').hide();
                });
        }
    });
}