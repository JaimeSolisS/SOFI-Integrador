// ==========================================================================================================================================================
//  Version: 20190308
//  Author:  Felipe vera
//  Created Date: 29 de March de 2019
//  Description: Contiene funciones JS para la página de Index de la administración de dashboard
//  Modifications: 
// ==========================================================================================================================================================
// Entidades
function GenericItem() {
    this.RowNumber = '';
    this.FieldKey = '';
    this.FieldValue = '';
}

function IndexInit(LangResources) {
    RegisterPluginDataTable(50);

    SetXEditablePlugin();
    Setupminicolors();
    SetupNodeSortable();

    $('.applyfilters').on('change', function (e) {
        ForceApplyFilters();
        FillDashboardAreaMasterTable();
        SetupNodeSortable();
    });


    SetSelectPlugin();
    SetupOnlyNumbers();
    SetXEditablePlugin();
    RegisterDeleteDashboardAreaTable();
    RegisterEventProductionGoalsDetails();
    RegisterEventNodeExpand();
    RegisterButtonGeneralSetting();
    RegisterButtonShowModalAddDashboard();
    RegisterButtonShowModalCultureField();
    RegisterButtonShowModalAddDashboarAreaDetail();
    RegisterButtonsNodeTable();
    RegisterButtonsReorderSectionModal();

    RegisterButtonDeleteAttachments();
    RegisterButtonPreviewVideo();
    RegisterButtonClosePreviewVideo();

}

function SetupNodeSortable() {
    $(".tbl_node_master").each(function (idx, elt) {
        var $tbody = $(this).find('tbody');
        var parentarea=0
        parentarea = $(this).data("parentarea");

        $tbody.sortable({
            revert: true,
            items: "tr.sortnode_" + parentarea,
            appendTo: "parent",
            opacity: 1,
            containment: '#tbodysortable_' + parentarea,
            placeholder: "placeholder-style",
            forcePlaceholderSize: true,
            cursor: "move",
            delay: 150,
            start: function (event, ui) {
                $(this).find('.placeholder-style td:nth-child(2)').addClass('hidden-td');
                ui.helper.css('display', 'table');

                //checar el tr anterior 
                var $ant = $(ui.item).prev();
                //checar el tr sig, (el sig es el placeholder, por lo que es el sig)
                var $sig = $(ui.item).next().next();
                var ant_section = 0;
                ant_section = $ant.data("sectionseq");
                var sig_section = 0;
                sig_section = $sig.data("sectionseq");
                var actual_section = $(ui.item).data("sectionseq");

                if (actual_section !== ant_section && actual_section !== sig_section) {
                    $(ui.item).data('uniquesection', 1);
                } else {
                    $(ui.item).data('uniquesection', 0);
                }
            },
            stop: function (event, ui) {
                ui.item.css('display', '');

                //checar el tr anterior 
                var $ant = $(ui.item).prev();
                //checar el tr sig
                var $sig = $(ui.item).next();

                var ant_section = 0;
                ant_section = $ant.data("sectionseq");
                var sig_section = 0;
                sig_section = $sig.data("sectionseq");
                var actual_section = $(ui.item).data("sectionseq");
                var uniquesection = $(ui.item).data("uniquesection");
                var ntf_SectionSeparated = $('#ntf_SectionSeparated').text();
                var nft_DifferentSectionNode = $('#nft_DifferentSectionNode').text();
                //es la posision primera o ultima y es solo un elemento
                if (uniquesection === 0 && $ant.length === 0 && sig_section !== actual_section) {
                    notification("", ntf_SectionSeparated, "error");
                    $(this).sortable('cancel');
                } else if (uniquesection === 0 && $sig.length === 0 && ant_section !== actual_section) {
                    notification("", ntf_SectionSeparated, "error");
                    $(this).sortable('cancel');
                }
                else if (ant_section === sig_section && actual_section !== ant_section) { //dentro de un bloque de seccion{                
                    notification("", nft_DifferentSectionNode, "error");
                    $(this).sortable('cancel');
                }
                //else if (uniquesection === 0 && actual_section !== sig_section) {
                //    notification("", nft_DifferentSectionNode, "error");
                //    $(this).sortable('cancel');
                //}
                else {
                    //reacomoda la seq de todos los nodos de la seccion
                    if (uniquesection === 0) {
                        $(this).find(".node_section_" + actual_section).each(function (idx, elt) {
                            var newseq = idx + 1;
                            var $row = $(elt);
                            //actualizar la seq del nodo
                            var pk = $row.data("dashboardareadetailid");
                            var name = 'seq'
                            //llamada ajax a actualizar seq
                            ShowProgressBar();
                            $.post('/CI/Administration/DashboardAreasDetail_UpdateQuickEditable',
                                { name, pk, value: newseq }
                            ).done(function (data) {
                                if (data.ErrorCode !== 0) {
                                    notification("", data.ErrorMessage, "error");
                                } else {
                                    //Actualizar formato de seqnumber
                                    $.get("/CI/Administration/GetSeqNumber", { DashboardAreaDetailID: pk }
                                    ).done(function (dataseq) {
                                        if (dataseq.ErrorCode === 0) {
                                            $row.find('td:eq(1)').text(dataseq.SeqNumber);
                                        }
                                    });                                    
                                }
                                HideProgressBar();
                            });
                        });
                    } else {
                        // es una sola seccion
                        // se tienen que reacomodar todos los nodos de todas las secciones
                        // reactualizar el SectionSeq de todos los del containment
                        var currentsection = 0;
                        var newseq = 0;
                        $('#tbodysortable_' + parentarea + ' .sortnode_' +parentarea).each(function (idx, elt) {
                            var $row = $(elt);
                            if (currentsection !== $row.data("entityid")) {
                                currentsection = $row.data("entityid");
                                newseq++;
                            }
                            if (newseq !== $row.data("sectionseq", newseq)) {
                                //actualizar la seq de la seccion, NO del detail
                                //llamada ajax a actualizar seq
                                var pk = currentsection;
                                var name = 'seq';
                                ShowProgressBar();
                                $.post('/CI/Administration/UpdateQuickEditable',
                                    { name, pk, value: newseq }
                                ).done(function (data) {
                                    if (data.ErrorCode !== 0) {
                                        notification("", data.ErrorMessage, "error");
                                    } else {
                                        //Actualizar formato de seqnumber
                                        $.get("/CI/Administration/GetSeqNumber", { DashboardAreaDetailID: $row.data("dashboardareadetailid") }
                                        ).done(function (dataseq) { 
                                            if (dataseq.ErrorCode === 0) {
                                                $row.find('td:eq(1)').text(dataseq.SeqNumber);
                                            }
                                        });
                                    }
                                    HideProgressBar();
                                });
                                //console.log("seccionid: " + currentsection + " nuevaseq: " + newseq);
                            }

                        });
                    }
                }

            }
        });
    });
    
}


function ForceApplyFilters() {
    $("#div_boxTableInfo").hide();
}

//private methods
function FillDashboardAreaMasterTable() {
    try {
        var DashboardAreaID = 0;
        var FileTypeID = 0;
        var ViewType = 0;

        if ($('#ddl_F_Dashboard').val() != '') {
            DashboardAreaID = parseInt($('#ddl_F_Dashboard').val());
        }

        if ($('#ddl_F_FileType').val() != '' && $('#ddl_F_FileType').val() != null) {
            FileTypeID = parseInt($('#ddl_F_FileType').val());
        }
        if ($('#ddl_F_View').val() != '' && $('#ddl_F_View').val() != null) {
            ViewType = parseInt($('#ddl_F_View').val());
        }

        var DashboardAreaRequest = {
            DashboardAreaID: DashboardAreaID,
            FileTypeID: FileTypeID,
            ViewType,
        };
        ShowProgressBar();
        $.get("/CI/Administration/DashboardAreaList", DashboardAreaRequest).done(function (data) {
            $('#div_boxTableInfo').show();
            $('#div_boxTableInfo').html('');
            $('#div_boxTableInfo').html(data);
            SetXEditablePlugin();
            Setupminicolors();
            RegisterEventNodeExpand()
            SetupNodeSortable();
            HideProgressBar();
        });
    } catch (error) {
        console.log(error.message);
    }
}

function SetXEditablePlugin() {
    $('.x-editable').editable({
        success: function (response, newValue) {
            if (response.ErrorCode == 0) {
                notification("", response.ErrorMessage, "success");
            } else {
                return response.ErrorMessage;
            }
        }
    });
}

function SetSelectPlugin() {
    $(".select").selectpicker();
}

function Setupminicolors() {
    $('.mini-color').minicolors({
        control: $(this).attr('data-control') || 'hue',
        defaultValue: $(this).attr('data-defaultValue') || '',
        format: $(this).attr('data-format') || 'hex',
        keywords: $(this).attr('data-keywords') || '',
        inline: $(this).attr('data-inline') === 'true',
        letterCase: $(this).attr('data-letterCase') || 'lowercase',
        opacity: $(this).attr('data-opacity'),
        position: $(this).attr('data-position') || 'top right',
        swatches: $(this).attr('data-swatches') ? $(this).attr('data-swatches').split('|') : [],
        theme: 'bootstrap'
    });
}

function SetupSectionSortable() {
    $(".sectionsortable").sortable({
        revert: true,
        items: ">tr",
        appendTo: "parent",
        opacity: 1,
        containment: "document",
        placeholder: "placeholder-style",
        cursor: "move",
        delay: 150,
        start: function (event, ui) {
            $(this).find('.placeholder-style td:nth-child(2)').addClass('hidden-td')
            ui.helper.css('display', 'table')
        },
        stop: function (event, ui) {
            ui.item.css('display', '')
            //validar el lugar
            //actualizar el seq
            $(this).children("tr").each(function (idx, elt) {
                var newseq = idx + 1;
                $(elt).children("td").first().text(newseq);
                $(elt).attr("newseq", newseq);
                var pk = $(elt).data("entityid");
                var name ='seq'
                //llamada ajax a actualizar seq
                $.post('/CI/Administration/UpdateQuickEditable',
                    { name, pk, value: newseq }
                ).done(function (data) {
                    console.log(data.ErrorMessage);
                });
            });
        },
        connectWith: "#tbl_NodeSections tbody"
    });
}


function RegisterDeleteDashboardAreaTable() {
    $(document).on('click', '.delete-dashboardArea', function (e) {
        e.stopImmediatePropagation();

        var ID = this.attributes["data-dashboardarea-id"].value;

        /* Control generico para mostrar mensajes */
        var deleteDashboardControls = new ConfirmBoxControl();
        deleteDashboardControls.id.attr("class", "message-box animated fadeIn");
        deleteDashboardControls.btnNo.attr("class", "btn btn-lg mb-control-yes");
        deleteDashboardControls.btnNo.attr("class", "btn btn-lg mb-control-close");
        deleteDashboardControls.id.addClass(deleteDashboardControls.notificationTypeClass('warning'));
        deleteDashboardControls.faIcon.addClass(deleteDashboardControls.faIconCss('warning'));
        deleteDashboardControls.btnNo.addClass(deleteDashboardControls.btnNoCss);
        deleteDashboardControls.btnYes.addClass(deleteDashboardControls.btnYesCss);

        /*default style*/
        deleteDashboardControls.yesTag.text($('#lbl_DeleteButtonTag').text());
        deleteDashboardControls.noTag.text($('#lbl_CancelButtonTag').text());
        deleteDashboardControls.title.text($('#lbl_TitleDeleteDashboardAreaTag').text());
        deleteDashboardControls.msg.text($('#lbl_MsgDeleteDashboardAreaTag').text());
        deleteDashboardControls.id.addClass('open');

        $('#confirmbx_yes').attr("data-id", ID);
        $("#confirmbx_yes").off();
        $('#confirmbx_yes').on('click', function (e) {
            e.stopImmediatePropagation();
            ShowProgressBar();
            var DeleteDashboardAreaParams = {
                ID: parseInt($(this).attr("data-id"))
            };
            var DashboardAreaDeleteRequest = CallWebMethodPOST("/CI/Administration/DashboardArea_Delete", DeleteDashboardAreaParams);

            DashboardAreaDeleteRequest.success(function (data) {
                deleteDashboardControls.id.removeClass('open');
                HideProgressBar();
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    FillDashboardAreaMasterTable();
                } else {
                    notification("", data.ErrorMessage, "error");
                }
            });
        });

    });
}

function RegisterEventProductionGoalsDetails() {
    //expandir detalles con plugin DataTable
    $(document).on('click', '#tbl_master td.details-control', function (e) {
        e.stopPropagation();

        var tr = $(this).closest('tr');

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {
            var DashboardAreaID = 0;
            var FileTypeID = 0;
            tr.after('');
            DashboardAreaID = tr.data("entityid");

            if ($('#ddl_F_FileType').val() != '' && $('#ddl_F_FileType').val() != null) {
                FileTypeID = parseInt($('#ddl_F_FileType').val());
            }

            var DashboardAreaRequest = {
                DashboardAreaID: DashboardAreaID,
                FileTypeID: FileTypeID
            };
            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get('/CI/Administration/DashboardAreaDetailList', DashboardAreaRequest
            ).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan = "9" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                SetXEditablePlugin();
                RegisterDeleteDashboardDetailAreaTable();
                RegisterEditDashboardDetailAreaTable();
                HideProgressBar();
            });
        }
    });
}
function RegisterEventNodeExpand() {
    //expandir detalles
    $(document).on('click', '.tbl_node_master td.details-control', function (e) {
        e.stopImmediatePropagation();

        var tr = $(this).closest('tr');

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {
                tr.next().remove();
                tr.removeClass('shown');
            });
        } else {
            var DashboardAreaID = 0;
            var FileTypeID = 0;
            tr.after('');
            DashboardAreaID = tr.data("entityid");
            var DashboardAreaDetailID = tr.data("dashboardareadetailid");

            if ($('#ddl_F_FileType').val() != '' && $('#ddl_F_FileType').val() != null) {
                FileTypeID = parseInt($('#ddl_F_FileType').val());
            }

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get('/CI/Administration/DashboardAreaNodeList', {
                DashboardAreaID: DashboardAreaID,
                FileTypeID: FileTypeID,
                DashboardAreaDetailID
            }
            ).done(function (data) {
                tr.after('');
                tr.after('<tr><td colspan = "12" class="padding-0">' + data + '</td></tr>');
                tr.addClass('shown');
                $('div.slider', tr.next()).slideDown();
                SetXEditablePlugin();
                //sortable for new tables
                SetupNodeSortable();
                Setupminicolors();
                HideProgressBar();
            });
        }
    });
}

function RegisterButtonGeneralSetting() {
    Dropzone.autoDiscover = false;
    $(document).on('click', '#btn_GralSetting', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/CI/Administration/DashboardArea_GeneralSettings', {}
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            var div_MainDashboardManagement = $('#div_MainDashboardManagement');

            $('#mo_GeneralSettings').modal('show');
            RegisterButtonUpdateGeneralSettings();
            RegisterButtonBackGroundImage();
            RegisterButtonChangeVideo();
            LoadDropzoneImageDashboardSettings(".form-dropzone-backgroundscreen");
            LoadDropzoneVideoDashboardSettings(".form-dropzone-videoscreen");
            LoadDropzoneCarouselVideoDashboardSettings('.form-dropzone-carousel');
            RegisterCarouselVideoSortable();
            HideProgressBar();
            div_MainDashboardManagement.hide();
        });

    });
}

function RegisterButtonShowModalAddDashboard() {
    $(document).on('click', '#btn_AddDashboard', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/CI/Administration/DashboardArea_AddDashboardAreaInit', {}
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");


            $('#mo_DashboardAreaUpsert').modal('show');
            RegisterButtonAddDashboardArea();
            SetSelectPlugin();
            HideProgressBar();
        });

    });
}

function RegisterButtonAddDashboardArea() {
    $(document).on('click', '#btn_NewDashboardArea', function (e) {
        e.stopImmediatePropagation();
        var ddl_Add_Size = $('#ddl_Add_Size');
        var txt_Add_SeqTag = $('#txt_Add_SeqTag');
        var SizeID = 0;
        var Sequence = 0;
        var list = [];
        var ParentID = 0;
        ParentID = $(this).data('dashboardareadetailid');
        $('#tb_AddDashboardAreaBody').find('tr').each(function (index, element) {
            var row = element;
            var entity = new GenericItem4Select();
            var txt_Language = getFindControl('txt_languagetext', row, row);

            entity.FieldKey = element.children[0].attributes['data-languageid'].value
            entity.RowNumber = index + 1;
            entity.FieldValue = txt_Language.value;

            list.push(entity);
        });

        if (ddl_Add_Size.val() != null && ddl_Add_Size.val() != '') {
            SizeID = parseInt(ddl_Add_Size.val());
        }

        if (txt_Add_SeqTag.val() != '') {
            Sequence = parseInt(txt_Add_SeqTag.val());
        }

        ShowProgressBar();
        var AddDashboardAreaParamsRequest = CallWebMethodPOST("/CI/Administration/DashboardArea_Upsert", {
            SizeID,
            Sequence,
            list,
            ParentID
        });

        AddDashboardAreaParamsRequest.success(function (data) {
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, "success");
                $('#mo_DashboardAreaUpsert').modal('hide');
                FillDashboardAreaMasterTable();
                if (ParentID !== 0) {
                    //actualizar el tbl_NodeSections  mo_ReorderSections
                    $.get('/CI/Administration/GetNodeSections', {
                        DashboardAreaDetailID: ParentID
                    }
                    ).done(function (data) {
                        $('#div_table_nodesections').html(data.View);   
                        //drag and drop
                        SetupSectionSortable();
                        SetXEditablePlugin();
                        HideProgressBar();                        
                    });
                }

            } else {
                notification("", data.ErrorMessage, "error");
            }
            HideProgressBar();
        });

    });
}

function RegisterButtonBackGroundImage() {
    $(document).on('click', '#btnBackgroudImageChange', function (e) {
        e.stopImmediatePropagation();
        $(".form-dropzone-backgroundscreen").click();
    });
}

function RegisterButtonChangeVideo() {
    $(document).on('click', '#btnBackgroudVideoChange', function (e) {
        e.stopImmediatePropagation();
        $(".form-dropzone-videoscreen").click();
    });
}


function RegisterButtonUpdateGeneralSettings() {
    $(document).on('click', '#btn_UpdateGeneralSettings', function (e) {
        e.stopImmediatePropagation();
        var hf_VideoUrl = $('#hf_VideoUrl');
        var hf_ImageUrl = $('#hf_ImageUrl');
        var hf_TransactionID = $('#hf_TransactionID');
        var txt_CloseWindowAfter = $('#txt_CloseWindowAfter');
        var txt_ScreenSaverInterval = $('#txt_ScreenSaverInterval');
        var screenSaverInterval = 0;
        var closeWindowAfter = 0;

        if (txt_ScreenSaverInterval.val() != '') {
            screenSaverInterval = parseInt(txt_ScreenSaverInterval.val());
        }

        if (txt_CloseWindowAfter.val() != '') {
            closeWindowAfter = parseInt(txt_CloseWindowAfter.val());
        }

        var UpdateGeneralSettingsParams = {
            ImageURL: hf_ImageUrl.val(),
            VideoURL: hf_VideoUrl.val(),
            ScreenSaverInterval: screenSaverInterval,
            ClosedWindowAfter: closeWindowAfter,
            TransactionID: hf_TransactionID.val()
        };
        ShowProgressBar();
        var UpdateGeneralSettingsRequest = CallWebMethodPOST("/CI/Administration/DashboardArea_UpdateGeneralSettings", UpdateGeneralSettingsParams);

        UpdateGeneralSettingsRequest.success(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, "success");
                location.href = $('.btn-back-main').attr('href');
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });

    });
}

function RegisterButtonShowModalAddDashboarAreaDetail() {
    $(document).on('click', '.add-dashboardareadetail', function (e) {
        e.stopImmediatePropagation();
        var ID = this.attributes["data-dashboardarea-id"].value;

        ShowProgressBar();
        $.get('/CI/Administration/DashboardAreaDetail_Init', { DashboarAreaID: ID }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            $('#mo_DashboardAreaDetailUpsert').modal('show');
            RegisterEventFileTypeControl();
            RegisterButtonAddDashboardAreaDetail();
            LoadDropzoneBackgroundImageDashboardAreasDetailUpsert('.form-dropzone-backgroundimage');
            LoadDropzoneDashboardAreasDetailFileType('.form-dropzone-filetype');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseSourcePath', 'form-dropzone-filetype');
            SetSelectPlugin();
            HideProgressBar();
        });

    });
}

// Quick updates
function RegisterButtonShowModalCultureField() {
    $(document).on('click', '.edit-fieldculture', function (e) {
        e.stopImmediatePropagation();
        var ID = this.attributes["data-entityid"].value;
        var FieldTitle = this.attributes["data-title"].value;

        var updatenodesection = $(this).data('updatenodesection');
        var parentid = $(this).data('parentid');
        ShowProgressBar();
        $.get('/CI/Administration/DashboardArea_CultureListInit', { DashboarAreaID: ID, FieldTitle: FieldTitle }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            $('#btn_CultureInfoUpdate').data('updatenodesection', updatenodesection);
            $('#btn_CultureInfoUpdate').data('parentid', parentid);
            $('#mo_CultureList').modal('show');
            RegisterButtonQuickTranslateUpdate();
            HideProgressBar();
        });

    });
}

function RegisterButtonQuickTranslateUpdate() {
    $(document).on('click', '#btn_CultureInfoUpdate', function (e) {
        e.stopImmediatePropagation();
        var ID = this.attributes["data-entityid"].value;
        var list = [];

        var ParentID = $(this).data('parentid');
        var updatenodesection = $(this).data('updatenodesection');        

        $('#tb_CultureInfoBody').find('tr').each(function (index, element) {
            var row = element;
            var entity = new GenericItem4Select();
            var txt_Language = getFindControl('txt_languagetext', row, row);

            entity.FieldKey = element.children[0].attributes['data-languageid'].value;
            entity.RowNumber = index + 1;
            entity.FieldValue = txt_Language.value;

            list.push(entity);
        });

        var QuickTranslateUpdateParams = {
            DashboardAreaID: ID,
            list: list
        };

        ShowProgressBar();
        var QuickTranslateUpdateRequest = CallWebMethodPOST("/CI/Administration/DashboardArea_QuickTranslateUpdate", QuickTranslateUpdateParams);

        QuickTranslateUpdateRequest.success(function (data) {
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, "success");
                $('#mo_CultureList').modal('hide');
                //si es reorder actualizar el listado
                if (updatenodesection) {
                    //actualizar el tbl_NodeSections  mo_ReorderSections
                    $.get('/CI/Administration/GetNodeSections', {
                        DashboardAreaDetailID: ParentID
                    }
                    ).done(function (data) {
                        $('#div_table_nodesections').html(data.View);
                        //drag and drop
                        SetupSectionSortable();
                        SetXEditablePlugin();
                        HideProgressBar();
                    });
                }
                FillDashboardAreaMasterTable();
            } else {
                notification("", data.ErrorMessage, "error");
            }
            HideProgressBar();
        });

    });
}

// Modal DashboardDetails - Update
function RegisterEventFileTypeControl() {
    $(document).on('change', '#ddl_DAD_FileType', function (e) {
        e.stopImmediatePropagation();

        var ddl_DAD_FileType = $(this);
        var divBoxFileTypeLink = $('#divBoxFileTypeLink');
        var divBoxFileTypeMediaOrFile = $('#divBoxFileTypeMediaOrFile');
        var divBoxFileTypeGalery = $('.divBoxFileTypeGalery');
        var hdnFileTypeValueID = $('#hdnFileTypeValueID');
        var hdnFileExtensions = $('#hdnFileExtensions');

        if (ddl_DAD_FileType != null) {

            divBoxFileTypeLink.addClass("hidden");
            divBoxFileTypeMediaOrFile.addClass("hidden");
            divBoxFileTypeGalery.addClass("hidden");

            var valueid = ddl_DAD_FileType.find(':selected').data('valueid');
            var validextension = ddl_DAD_FileType.find(':selected').data('valid-extension');

            $('#txt_Add_SourcePath').val('');
            $('#txt_Add_SourcePath').attr("data-url", "");

            hdnFileTypeValueID.val(valueid);
            hdnFileExtensions.val(validextension);
            if (ddl_DAD_FileType.val() != '') {
                if (valueid.toUpperCase() == 'L') {
                    divBoxFileTypeLink.removeClass("hidden");
                } else if (valueid.toUpperCase() == 'G') {
                    divBoxFileTypeMediaOrFile.removeClass("hidden");
                    divBoxFileTypeGalery.removeClass("hidden");
                } else {
                    divBoxFileTypeMediaOrFile.removeClass("hidden");
                }

            }
        }

    });
}

function RegisterButtonAddDashboardAreaDetail() {
    $(document).on('click', '#btn_NewDashboardAreasDetail', function (e) {
        e.stopImmediatePropagation();
        // controls
        var ddl_DAD_FileType = $('#ddl_DAD_FileType');
        var txt_DAD_Footer = $('#txt_DAD_Footer');
        var ddl_Add_Size = $('#ddl_Add_Size');
        var ddl_Add_BackColor = $('#ddl_Add_BackColor');
        var ddl_Add_FontColor = $('#ddl_Add_FontColor');
        var ddl_Add_Icon = $('#ddl_Add_Icon');
        var txt_Add_BackGroundImage = $('#txt_Add_BackGroundImage');
        var txt_Add_SeqTag = $('#txt_Add_SeqTag');
        var txt_Add_Href = $('#txt_Add_Href');
        var txt_Add_SourcePath = $('#txt_Add_SourcePath');
        var ddl_Add_DataEffect = $('#ddl_Add_DataEffect');
        var txt_Add_DataEffectDuration = $('#txt_Add_DataEffectDuration');
        var ddl_ParentNode = $('#ddl_ParentNode');
        var ddl_Section = $('#ddl_Section');

        var entity = new DashboardAreaDetail();
        var list = [];
        var TransactionDetailID = $('#TransactionDetailID').val();
        entity.DashboardAreaDetailID = $(this).data('entityid');
        //entity.DashboardAreaID = $(this).data('dashboardareaid');
        //entity.ParentDashboardAreaDetailID = $('#ddl_ParentNode').val();
        //entity.ParentDashboardAreaDetailID = $(this).data('parentdashboardareadetailid');

        if (ddl_DAD_FileType.val() != null && ddl_DAD_FileType.val() != '') {
            entity.FileTypeID = parseInt(ddl_DAD_FileType.val());
            entity.FileTypeValueID = $('#hdnFileTypeValueID').val();
        }
        entity.Footer = txt_DAD_Footer.val();
        if (ddl_Add_Size.val() != null && ddl_Add_Size.val() != '') {
            entity.SizeID = parseInt(ddl_Add_Size.val());
        }
        if (ddl_Add_BackColor.val() != null && ddl_Add_BackColor.val() != '') {
            entity.BackColorID = parseInt(ddl_Add_BackColor.val());
        }
        if (ddl_Add_FontColor.val() != null && ddl_Add_FontColor.val() != '') {
            entity.FontColorID = parseInt(ddl_Add_FontColor.val());
        }
        if (ddl_Add_Icon.val() != null && ddl_Add_Icon.val() != '') {
            entity.IconID = parseInt(ddl_Add_Icon.val());
        }
        if (txt_Add_BackGroundImage.val() != '') {
            entity.BackgroundImage = txt_Add_BackGroundImage.data('url');
        }
        if (txt_Add_SeqTag.val() != '') {
            entity.Seq = parseInt(txt_Add_SeqTag.val());
        }
        entity.HRef = txt_Add_Href.val();
        if (txt_Add_SourcePath.val() != '') {
            entity.SourcePath = txt_Add_SourcePath.data('url');
        }
        if (ddl_Add_DataEffect.val() != null && ddl_Add_DataEffect.val() != '') {
            entity.DataEffectID = parseInt(ddl_Add_DataEffect.val());
        }
        if (txt_Add_DataEffectDuration.val() != '') {
            entity.DataEffectDuration = parseInt(txt_Add_DataEffectDuration.val());
        }

        if (ddl_ParentNode.val() != null && ddl_ParentNode.val() != '') {
            entity.ParentDashboardAreaDetailID = parseInt(ddl_ParentNode.val());
        }
        if (ddl_Section.val() != null && ddl_Section.val() != '') {
            entity.DashboardAreaID = parseInt(ddl_Section.val());
        }

        $('#tb_AddDashboardAreaDetailBody').find('tr').each(function (index, element) {
            var row = element;
            var entity = new GenericItem4Select();
            var txt_Language = getFindControl('txt_languagetext', row, row);

            entity.FieldKey = element.children[0].attributes['data-languageid'].value
            entity.RowNumber = index + 1;
            entity.FieldValue = txt_Language.value;

            list.push(entity);
        });


        var AddDashboardAreaDetailParams = {
            entity: entity,
            list: list,
            TransactionDetailID: TransactionDetailID
        };

        ShowProgressBar();
        var AddDashboardAreaDetailRequest = CallWebMethodPOST("/CI/Administration/DashboardAreaDetail_Upsert", AddDashboardAreaDetailParams);

        AddDashboardAreaDetailRequest.success(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, "success");
                $('#mo_DashboardAreaDetailUpsert').modal('hide');
                FillDashboardAreaMasterTable();
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });

    });
}

//NodeTable buttons Actions
function RegisterButtonsNodeTable() {
    //boton de reorder
    $(document).on('click', '.reorder-sections', function (e) {
        e.stopImmediatePropagation();
        var DashboardAreaDetailID = $(this).data("dashboardareadetailid");
        var NodeName = $(this).data("nodename");
        ShowProgressBar();
        // abrir detalles y devolver detalles de una llamada ajax
        $.get('/CI/Administration/GetReorderSectionsModal', {
            DashboardAreaDetailID
        }
        ).done(function (data) {
            HideProgressBar();
            $('#div_ReorderSections').html(data.View);
            $('#lbl_nodename').text(NodeName);
            $('#btn_AddNodeSection').data('dashboardareadetailid', DashboardAreaDetailID);
            SetupSectionSortable();
            SetXEditablePlugin();
            $('#mo_ReorderSections').modal('show');
        });

    });
    //boton de add node
    $(document).on('click', '.add-node', function (e) {
        e.stopImmediatePropagation();
        var DashboarAreaID = $(this).data("dashboardareaid");
        var DashboardAreaDetailID = $(this).data("dashboardareadetailid");
        ShowProgressBar();
        $.get('/CI/Administration/DashboardAreaDetail_Init', { DashboarAreaID, DashboardAreaDetailID }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            $('#mo_DashboardAreaDetailUpsert').modal('show');
            RegisterEventFileTypeControl();
            RegisterButtonAddDashboardAreaDetail();
            LoadDropzoneBackgroundImageDashboardAreasDetailUpsert('.form-dropzone-backgroundimage');
            LoadDropzoneDashboardAreasDetailFileType('.form-dropzone-filetype');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseSourcePath', 'form-dropzone-filetype');
            RegisterEventParentNodeChange();
            SetSelectPlugin();
            SetupNodeSortable();
            HideProgressBar();
        });
    });
    //boton de edit node
    $(document).on('click', '.edit-node', function (e) {
        e.stopImmediatePropagation();

        var DashboarAreaID = $(this).data("dashboardareaid");
        var DashboardAreaDetailID = $(this).data("dashboardareadetailid");

        ShowProgressBar();
        $.get('/CI/Administration/DashboardAreaDetail_EditInit', { DashboarAreaID, DashboardAreaDetailID }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            $('#mo_DashboardAreaDetailUpsert').modal('show');
            RegisterEventFileTypeControl();
            RegisterButtonAddDashboardAreaDetail();
            LoadDropzoneBackgroundImageDashboardAreasDetailUpsert('.form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            RegisterEventParentNodeChange();
            SetSelectPlugin();
            HideProgressBar();
        });
    });
    //boton de delete
    $(document).on('click', '.delete-node', function (e) {
        e.stopImmediatePropagation();

        var DashboarAreaID = $(this).data("dashboardareaid");
        var DashboardAreaDetailID = $(this).data("dashboardareadetailid");

        var ID = DashboardAreaDetailID;

        /* Control generico para mostrar mensajes */
        var deleteDashboardDetailControls = new ConfirmBoxControl();
        deleteDashboardDetailControls.id.attr("class", "message-box animated fadeIn");
        deleteDashboardDetailControls.btnNo.attr("class", "btn btn-lg mb-control-yes");
        deleteDashboardDetailControls.btnNo.attr("class", "btn btn-lg mb-control-close");
        deleteDashboardDetailControls.id.addClass(deleteDashboardDetailControls.notificationTypeClass('warning'));
        deleteDashboardDetailControls.faIcon.addClass(deleteDashboardDetailControls.faIconCss('warning'));
        deleteDashboardDetailControls.btnNo.addClass(deleteDashboardDetailControls.btnNoCss);
        deleteDashboardDetailControls.btnYes.addClass(deleteDashboardDetailControls.btnYesCss);

        /*default style*/
        deleteDashboardDetailControls.yesTag.text($('#lbl_DeleteButtonTag').text());
        deleteDashboardDetailControls.noTag.text($('#lbl_CancelButtonTag').text());
        deleteDashboardDetailControls.title.text($('#lbl_TitleDeleteDashboardAreaTag').text());
        deleteDashboardDetailControls.msg.text($('#lbl_MsgDeleteDashboardAreaDetailTag').text());
        deleteDashboardDetailControls.id.addClass('open');

        $('#confirmbx_yes').attr("data-id", ID);
        $("#confirmbx_yes").off();
        $('#confirmbx_yes').on('click', function (e) {
            e.stopImmediatePropagation();
            ShowProgressBar();
            var DeleteDashboardAreaDetailParams = {
                ID: parseInt($(this).attr("data-id"))
            };
            var DashboardAreaDetailDeleteRequest = CallWebMethodPOST("/CI/Administration/DashboardAreaDetail_Delete", DeleteDashboardAreaDetailParams);

            DashboardAreaDetailDeleteRequest.success(function (data) {
                deleteDashboardDetailControls.id.removeClass('open');
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    FillDashboardAreaMasterTable();
                } else {
                    notification("", data.ErrorMessage, "error");
                }
                HideProgressBar();
            });
        });
    });
}

function RegisterEventParentNodeChange() {
    $(document).on('change', '#ddl_ParentNode', function (e) {
        e.stopImmediatePropagation();
        var $ddl = $('#ddl_Section');

        ShowProgressBar();
        var ParentID = $('#ddl_ParentNode option:selected').val();
        $.get('/CI/Administration/GetParentNodeSections', { ParentID }
        ).done(function (data) {
            if (data.ErrorCode == 0) {
                $ddl.empty();
                $.each(data.List, function (index, item) {
                    $ddl.append($('<option></option>').text(item.Text).val(item.Value));
                });
                $ddl.selectpicker('refresh');
            }
            HideProgressBar();
        });
    });
}

//Reorder Section buttons Actions
function RegisterButtonsReorderSectionModal() {
    //btn_AddNodeSection
    $(document).on('click', '#btn_AddNodeSection', function (e) {
        e.stopImmediatePropagation();
        var DashboardAreaDetailID = $(this).data('dashboardareadetailid');
        ShowProgressBar();
        $.get('/CI/Administration/DashboardArea_AddDashboardAreaInit', {}
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            $('#div_DashboardAreaUpsert_Seq').addClass('hidden');
            $('#btn_NewDashboardArea').data('dashboardareadetailid', DashboardAreaDetailID);
            $('#mo_DashboardAreaUpsert').modal('show');
            RegisterButtonAddDashboardArea();
            SetSelectPlugin();
            HideProgressBar();
        });

    });
    //delete node section
    $(document).on('click', '.delete-nodesection', function (e) {
        e.stopImmediatePropagation();

        //$('#confirmbx_generic').css("z-index", "10");
        //ShowProgressBar();
        var ParentID = $(this).data("parentid");
        var DashboarAreaID = $(this).data("dashboardareaid");
        var ID = DashboarAreaID;

        /* Control generico para mostrar mensajes */
        var deleteDashboardDetailControls = new ConfirmBoxControl();
        deleteDashboardDetailControls.id.attr("class", "message-box animated fadeIn");
        deleteDashboardDetailControls.btnNo.attr("class", "btn btn-lg mb-control-yes");
        deleteDashboardDetailControls.btnNo.attr("class", "btn btn-lg mb-control-close");
        deleteDashboardDetailControls.id.addClass(deleteDashboardDetailControls.notificationTypeClass('warning'));
        deleteDashboardDetailControls.faIcon.addClass(deleteDashboardDetailControls.faIconCss('warning'));
        deleteDashboardDetailControls.btnNo.addClass(deleteDashboardDetailControls.btnNoCss);
        deleteDashboardDetailControls.btnYes.addClass(deleteDashboardDetailControls.btnYesCss);

        /*default style*/
        deleteDashboardDetailControls.yesTag.text($('#lbl_DeleteButtonTag').text());
        deleteDashboardDetailControls.noTag.text($('#lbl_CancelButtonTag').text());
        deleteDashboardDetailControls.title.text($('#lbl_TitleDeleteDashboardAreaTag').text());
        deleteDashboardDetailControls.msg.text($('#lbl_MsgDeleteDashboardAreaDetailTag').text());
        deleteDashboardDetailControls.id.addClass('open');

        $('#confirmbx_yes').attr("data-id", ID);
        $("#confirmbx_yes").off();
        $('#confirmbx_yes').on('click', function (e) {
            e.stopImmediatePropagation();
            ShowProgressBar();
            var DashboardAreaDetailDeleteRequest = CallWebMethodPOST("/CI/Administration/DashboardArea_Delete", { ID });

            DashboardAreaDetailDeleteRequest.success(function (data) {
                deleteDashboardDetailControls.id.removeClass('open');
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    //actualizar el tbl_NodeSections  mo_ReorderSections
                    $.get('/CI/Administration/GetNodeSections', {
                        DashboardAreaDetailID: ParentID
                    }
                    ).done(function (data) {
                        $('#div_table_nodesections').html(data.View);
                        //actualizar plugins
                        SetupSectionSortable();
                        SetXEditablePlugin();
                    });

                } else {
                    notification("", data.ErrorMessage, "error");
                }
                HideProgressBar();
            });
        });
    });
}

//entities
function GenericItem4Select() {
    this.RowNumber = 0;
    this.FieldKey = 0;
    this.FieldValue = '';
}

//entities
function DashboardAreaDetail() {
    this.DashboardAreaDetailID = 0;
    this.DashboardAreaID = 0;
    this.FileTypeID = 0;
    this.FileTypeValueID = '';
    this.Footer = '';
    this.SizeID = 0;
    this.BackColorID = 0;
    this.FontColorID = 0;
    this.IconID = 0;
    this.BackgroundImage = '';
    this.Seq = '';
    this.HRef = '';
    this.DataEffectID = 0;
    this.DataEffectDuration = 0;
    this.SourcePath = '';
    this.Enabled = true;
    this.ParentDashboardAreaDetailID = 0;
}

function LoadDropzoneImageDashboardSettings(form_selector) {
    Dropzone.autoDiscover = false;
    $(form_selector).dropzone({
        addRemoveLinks: true,
        createImageThumbnails: false,
        maxFiles: 1,
        acceptedFiles: 'image/*',
        init: function () {
            this.on("maxfilesexceeded", function (file) {
                this.removeFile(data);
                alert("No more files please!");
                return false;
            });
            this.on("complete", function (file) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    this.removeAllFiles(true);
                }
                var data = JSON.parse(file.xhr.responseText);
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    if (data.FilePath != '') {
                        $('#hf_ImageUrl').val(data.FilePath);
                        $('#div_DashBoardSettingsImage').css('background-image', "url('" + data.FilePath + "')");
                    }
                }

            });
        }
    });
}

function LoadDropzoneVideoDashboardSettings(form_selector) {
    Dropzone.autoDiscover = false;
    $(form_selector).dropzone({
        addRemoveLinks: true,
        createImageThumbnails: false,
        maxFiles: 1,
        acceptedFiles: '.mp4',
        init: function () {
            this.on("maxfilesexceeded", function (file) {
                this.removeFile(data);
                alert("No more files please!");
                return false;
            });
            this.on("complete", function (file) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    this.removeAllFiles(true);
                }
                var data = JSON.parse(file.xhr.responseText);
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    if (data.FilePath != '') {
                        $('#hf_VideoUrl').val(data.FilePath);
                        $('#s_video').attr('src', data.FilePath);
                    }
                }

            });
        }
    });
}

function LoadDropzoneBackgroundImageDashboardAreasDetailUpsert(form_selector) {
    Dropzone.autoDiscover = false;
    $(form_selector).dropzone({
        addRemoveLinks: true,
        createImageThumbnails: false,
        maxFiles: 1,
        acceptedFiles: 'image/*',
        init: function () {
            this.on("maxfilesexceeded", function (file) {
                this.removeFile(data);
                alert("No more files please!");
                return false;
            });
            this.on("complete", function (file) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    this.removeAllFiles(true);
                }
                var data = JSON.parse(file.xhr.responseText);
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    if (data.FilePath != '') {
                        $('#txt_Add_BackGroundImage').val(data.FileName);
                        $('#txt_Add_BackGroundImage').attr('data-url', data.FilePath);
                    }
                }

            });
        }
    });
}

function LoadDropzoneDashboardAreasDetailFileType(form_selector) {
    Dropzone.autoDiscover = false;
    $(form_selector).dropzone({
        addRemoveLinks: true,
        createImageThumbnails: false,
        maxFiles: 500,
        init: function () {
            this.on("sending", function (file) {
                $('.loading-process-div').show();
            });
            this.on("maxfilesexceeded", function (file) {
                $('.loading-process-div').hide();
                this.removeFile(data);
                alert("No more files please!");
                return false;
            });
            this.on("complete", function (file) {
                $('.loading-process-div').hide();
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    this.removeAllFiles(true);
                }
                var data = JSON.parse(file.xhr.responseText);
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    if (data.FilePath != '') {
                        $('#txt_Add_SourcePath').val(data.FileName);
                        $('#txt_Add_SourcePath').attr("data-url", data.FilePath);
                    }
                }
            });
        }
    });
}

function RegisterButtonBrowseUrl(selector, dropzone) {
    $(document).on('click', '#' + selector, function (e) {
        e.stopImmediatePropagation();
        if (selector == 'btn_BrowseBackGroundImage') {
            /*Variable global para cambiar el tipo de archivo que acepta*/
            //$('.dz-hidden-input').attr('accept', 'image/*');
        }
        $("." + dropzone).click();
    });
}

function RegisterDeleteDashboardDetailAreaTable() {
    $(document).on('click', '.delete-dashboardAreaDetail', function (e) {
        e.stopImmediatePropagation();

        var ID = this.attributes["data-dashboardarea-id"].value;

        /* Control generico para mostrar mensajes */
        var deleteDashboardDetailControls = new ConfirmBoxControl();
        deleteDashboardDetailControls.id.attr("class", "message-box animated fadeIn");
        deleteDashboardDetailControls.btnNo.attr("class", "btn btn-lg mb-control-yes");
        deleteDashboardDetailControls.btnNo.attr("class", "btn btn-lg mb-control-close");
        deleteDashboardDetailControls.id.addClass(deleteDashboardDetailControls.notificationTypeClass('warning'));
        deleteDashboardDetailControls.faIcon.addClass(deleteDashboardDetailControls.faIconCss('warning'));
        deleteDashboardDetailControls.btnNo.addClass(deleteDashboardDetailControls.btnNoCss);
        deleteDashboardDetailControls.btnYes.addClass(deleteDashboardDetailControls.btnYesCss);

        /*default style*/
        deleteDashboardDetailControls.yesTag.text($('#lbl_DeleteButtonTag').text());
        deleteDashboardDetailControls.noTag.text($('#lbl_CancelButtonTag').text());
        deleteDashboardDetailControls.title.text($('#lbl_TitleDeleteDashboardAreaTag').text());
        deleteDashboardDetailControls.msg.text($('#lbl_MsgDeleteDashboardAreaDetailTag').text());
        deleteDashboardDetailControls.id.addClass('open');

        $('#confirmbx_yes').attr("data-id", ID);
        $("#confirmbx_yes").off();
        $('#confirmbx_yes').on('click', function (e) {
            e.stopImmediatePropagation();
            ShowProgressBar();
            var DeleteDashboardAreaDetailParams = {
                ID: parseInt($(this).attr("data-id"))
            };
            var DashboardAreaDetailDeleteRequest = CallWebMethodPOST("/CI/Administration/DashboardAreaDetail_Delete", DeleteDashboardAreaDetailParams);

            DashboardAreaDetailDeleteRequest.success(function (data) {
                deleteDashboardDetailControls.id.removeClass('open');
                HideProgressBar();
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    FillDashboardAreaMasterTable();
                } else {
                    notification("", data.ErrorMessage, "error");
                }
            });
        });

    });
}

function RegisterEditDashboardDetailAreaTable() {
    $(document).on('click', '.edit-dashboardAreaDetail', function (e) {
        e.stopImmediatePropagation();

        var ID = this.attributes["data-dashboardarea-id"].value;
        var DashboardAreaDetailID = this.attributes["data-entityid"].value;

        ShowProgressBar();
        $.get('/CI/Administration/DashboardAreaDetail_EditInit', { DashboarAreaID: ID, DashboardAreaDetailID: DashboardAreaDetailID }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            $('#mo_DashboardAreaDetailUpsert').modal('show');
            RegisterEventFileTypeControl();
            RegisterButtonAddDashboardAreaDetail();
            LoadDropzoneBackgroundImageDashboardAreasDetailUpsert('.form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            SetSelectPlugin();
            HideProgressBar();
        });

    });
}


function GetGeneralSettingsAttachments() {
    var hf_TransactionID = $('#hf_TransactionID');
    $.get('/CI/Administration/DashboardCarouselVideos_List', { TransactionID: hf_TransactionID.val() }
    ).done(function (data) {
        var div_boxGeneralSettingTable = $('#div_boxGeneralSettingTable');
        div_boxGeneralSettingTable.html('');
        div_boxGeneralSettingTable.html(data);
        RegisterCarouselVideoSortable();
        HideProgressBar();
    });
}

function RegisterButtonDeleteAttachments() {
    $(document).on('click', '.settings-delete', function (e) {
        e.stopImmediatePropagation();

        var FileID = $(this).data('fileid');

        /* Control generico para mostrar mensajes */
        var deleteFileBox = new ConfirmBox();
        deleteFileBox.yesTag = $('#lbl_DeleteButtonTag').text();
        deleteFileBox.noTag = $('#lbl_CancelButtonTag').text();
        deleteFileBox.title = $('#lbl_TitleDeleteFileTag').text();
        deleteFileBox.msg = $('#lbl_MsgDeleteFileTag').text();
        deleteFileBox.showMsg('warning');


        /* funcion OnClick de  */
        deleteFileBox.onAccept = function () {

            var DeleteDashboardAreaDetailParams = {
                ID: FileID
            };

            var DeleteCarouselVideoSettingsRequest = CallWebMethodPOST("/CI/Administration/DashboardCarouselVideos_Delete", DeleteDashboardAreaDetailParams);

            DeleteCarouselVideoSettingsRequest.success(function (data) {
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    GetGeneralSettingsAttachments();
                } else {
                    HideProgressBar();
                    notification("", data.ErrorMessage, "error");
                }
            });
        };

    });
}

function RegisterButtonPreviewVideo() {
    $(document).on('click', '.settings-play-video', function (e) {
        e.stopImmediatePropagation();

        var url = $(this).data('url');
        var filename = $(this).data('filename');
        var lbl_FileNameTag = $('#lbl_FileNameTag');
        var preview_video = $('#preview_video');
        var spreview_video = $('#spreview_video');

        lbl_FileNameTag.html(filename);

        spreview_video.attr('src', url);

        preview_video.get(0).load();
        preview_video.get(0).play();

        //openFullscreen(preview_video.get(0));
        //$('#screensaver').fadeOut();
        //$('#screensaver').removeClass('hidden');
        $('#mo_PreviewVideo').modal('show');


    });
}

function RegisterButtonClosePreviewVideo() {
    $(document).on('click', '.close-preview-video', function (e) {
        e.stopImmediatePropagation();
        var preview_video = $('#preview_video');
        preview_video.get(0).pause();
        //$('#screensaver').addClass('hidden');
        $('#mo_PreviewVideo').modal('hide');
    });
}

function LoadDropzoneCarouselVideoDashboardSettings(form_selector) {
    Dropzone.autoDiscover = false;
    $(form_selector).dropzone({
        addRemoveLinks: true,
        timeout: null,
        createImageThumbnails: false,
        maxFiles: 10,
        paramName: 'file',
        chunking: true,
        forceChunking: true,
        maxFilesize: 1025, // megabytes
        chunkSize: 1000000, // bytes
        acceptedFiles: '.mp4',
        init: function () {
            this.on("maxfilesexceeded", function (file) {
                this.removeFile(data);
                alert("No more files please!");
                return false;
            });
            this.on("complete", function (file) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    this.removeAllFiles(true);
                }
                // Carga de Archivo . Cargar adjuntos
                var data = JSON.parse(file.xhr.responseText);
                if (data.ErrorCode === 0) {
                    if (data.FilePath != '') {
                        ShowProgressBar();
                        GetGeneralSettingsAttachments();
                    }
                }
                else {
                    notification('', data.ErrorMessage, 'error')
                }
            });
        }
    });
}

function RegisterCarouselVideoSortable() {
    $(".sortable-table tbody").sortable({
        stop: function (event, ui) {
            var list = [];

            $(this).children().each(function (index, row) {
                var id = $(row).data('pk');

                var entity = new GenericItem();
                entity.FieldKey = index + 1;
                entity.RowNumber = index + 1;
                entity.FieldValue = id;
                list.push(entity);
            });

            if (list.length > 0) {

                var Params = {
                    TransactionID: $('#hf_TransactionID').val(),
                    list: list
                };

                var Request = CallWebMethodPOST("/CI/Administration/DashboardCarouselVideo_SetSortable", Params);

                Request.success(function (data) {
                    if (data.ErrorCode == 0) {
                        GetGeneralSettingsAttachments();
                    } else {
                        HideProgressBar();
                        notification("", data.ErrorMessage, "error");
                    }
                });
            }

        }
    });
}

function openFullscreen(myVideo) {
    console.log("hitting")
    var elem = myVideo
    console.log(elem)
    if (elem.requestFullscreen) {
        elem.requestFullscreen();
    } else if (elem.mozRequestFullScreen) { /* Firefox */
        elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari & Opera */
        elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen) { /* IE/Edge */
        elem.msRequestFullscreen();
    }
}