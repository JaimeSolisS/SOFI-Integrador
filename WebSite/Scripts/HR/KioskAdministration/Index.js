// =============================================================================================================================
//  Version: 20191206
//  Author:  Luis Hernandez
//  Created Date: 6 Dic 2019
//  Description:  KioskAdministration HR JS
//  Modifications: 
// =============================================================================================================================
function ChangeTabs(evt, backSymbol) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(backSymbol).style.display = "block";
    evt.currentTarget.className += " active";
}

function SetVisualTypeStyle() {
    if ($("#chkHrefVisualizationMode").is(":checked")) {
        $("#rdoOptionyes").addClass("activeOption");
        $("#rdoOptionNo").removeClass("activeOption");
    } else {
        $("#rdoOptionNo").addClass("activeOption");
        $("#rdoOptionyes").removeClass("activeOption");
    }
}

function IndexInit(LangResources) {

    //entities
    //Esta entidad se utiliza para guardar la configuracion de los idomas de los popups de edicion
    function GenericItem4Select() {
        this.RowNumber = 0;
        this.FieldKey = 0;
        this.FieldValue = '';
    }

    function KioskAreaDetail() {
        this.KioskAreaDetailID = 0;
        this.KioskAreaID = 0;
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
        this.ParentKioskAreaDetailID = 0;
        this.BackgroundColor = "#000000";
        this.FontColor = "#000000";
    }

    function RecalculateCarouselMediaSeq() {
        var start = 1;
        $('.carrusel-media-row').each(function () {
            var row = $(this).closest("tr");
            if (row.data("sequence") != start) {

                $.post("/Attachments/Properties_QuickUpsert", {
                    FileId: $(this).data("pk"),
                    PropertyName: "Seq",
                    PropertyValue: start,
                    PropertyTypeID: 0
                }).done(function (dataQuick) {
                    if (dataQuick.ErrorCode == 0) {

                    }
                });

            }
            row.find("td:first-child").text(start);
            start++;
        });
    }

    function SetupNodeSortable() {
        $(".tbl_node_master").each(function (idx, elt) {
            var $tbody = $(this).find('tbody');
            var parentarea = 0
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
                //sort: function (event, ui) {
                //    //console.log(ui.item);
                //    //console.log(event);
                //    //var tr = ui.item;
                //    //if (tr.hasClass("shown")) {
                //    //    return false;
                //        $(this).sortable("cancel");
                //    //}
                //},
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

                    //Si el nodo está expandido, cancelar reordenamiento
                    var tr = ui.item;
                    if (tr.hasClass("shown")) {
                        $(this).sortable("cancel");
                        tr.next().remove();
                        tr.removeClass('shown');

                    }

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
                                var pk = $row.data("kioskareadetailid");
                                var name = 'seq'
                                //llamada ajax a actualizar seq
                                ShowProgressBar();
                                $.post('/HR/KioskAdministration/KioskAreasDetail_UpdateQuickEditable',
                                    { name, pk, value: newseq }
                                ).done(function (data) {
                                    if (data.ErrorCode !== 0) {
                                        notification("", data.ErrorMessage, "error");
                                    } else {
                                        //Actualizar formato de seqnumber
                                        $.get("/HR/KioskAdministration/GetSeqNumber", { KioskAreaDetailID: pk }
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
                            $('#tbodysortable_' + parentarea + ' .sortnode_' + parentarea).each(function (idx, elt) {
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
                                    $.post('/HR/KioskAdministration/UpdateQuickEditable',
                                        { name, pk, value: newseq }
                                    ).done(function (data) {
                                        if (data.ErrorCode !== 0) {
                                            notification("", data.ErrorMessage, "error");
                                        } else {
                                            //Actualizar formato de seqnumber
                                            $.get("/HR/KioskAdministration/GetSeqNumber", { KioskAreaDetailID: $row.data("kioskareadetailid") }
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

    //private methods
    function FillKioskAreaMasterTable() {
        try {
            var KioskAreaID = 0;
            var FileTypeID = 0;
            var ViewType = 0;

            if ($('#ddl_F_Kiosk').val() != '') {
                KioskAreaID = parseInt($('#ddl_F_Kiosk').val());
            }

            if ($('#ddl_F_FileType').val() != '' && $('#ddl_F_FileType').val() != null) {
                FileTypeID = parseInt($('#ddl_F_FileType').val());
            }
            if ($('#ddl_F_View').val() != '' && $('#ddl_F_View').val() != null) {
                ViewType = parseInt($('#ddl_F_View').val());
            }

            var KioskAreaRequest = {
                KioskAreaID: KioskAreaID,
                FileTypeID: FileTypeID,
                ViewType,
                IsBool: true
            };
            ShowProgressBar();
            $.get("/HR/KioskAdministration/KioskAreaList", KioskAreaRequest).done(function (data) {
                $('#div_boxTableInfo').show();
                $('#div_boxTableInfo').html('');
                $('#div_boxTableInfo').html(data);
                SetXEditablePlugin();
                Setupminicolors();
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
        $("select").selectpicker();
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
                    var name = 'seq'
                    //llamada ajax a actualizar seq
                    $.post('/HR/KioskAdministration/UpdateQuickEditable',
                        { name, pk, value: newseq }
                    ).done(function (data) {
                        console.log(data.ErrorMessage);
                    });
                });
            },
            connectWith: "#tbl_NodeSections tbody"
        });
    }

    function LoadDropzoneBackgroundImageKioskAreasDetailUpsert(form_selector) {
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

    function LoadDropzoneKioskAreasDetailFileType(form_selector) {
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

    function RegisterCarouselVideoSortable() {
        $(".sortable-table tbody").sortable({
            stop: function (event, ui) {
                RecalculateCarouselMediaSeq();
            }
        });
    }
    function SetBorderToWhiteColor() {
        if ($("#kiosk_area_detail_backgroundcolor").val() == "#ffffff") {
            $("#input_kiosk_area_detail_backgroundcolor").css("border", "solid");
            $("#input_kiosk_area_detail_backgroundcolor").css("border-width", "1px");
        } else {
            $("#input_kiosk_area_detail_backgroundcolor").css("border", "none");
        }

        if ($("#kiosk_area_detail_fontcolor").val() == "#ffffff") {
            $("#input_kiosk_area_detail_fontcolor").css("border", "solid");
            $("#input_kiosk_area_detail_fontcolor").css("border-width", "1px");
        } else {
            $("#input_kiosk_area_detail_fontcolor").css("border", "none");
        }
    }

    $(document).on("change", "#kiosk_area_detail_fontcolor", function () {
        SetBorderToWhiteColor();
    });
    $(document).on("change", "#kiosk_area_detail_backgroundcolor", function () {
        SetBorderToWhiteColor();
    });

    $('.applyfilters').on('change', function (e) {
        $("#div_boxTableInfo").hide();
        FillKioskAreaMasterTable();
        SetupNodeSortable();
    });


    //root functions
    $(document).on('click', '#btn_AddRootSection', function (e) {
        e.stopImmediatePropagation();
        var NodeName = "seccion raiz";
        ShowProgressBar();

        $.get('/HR/KioskAdministration/GetReorderSectionsModal').done(function (data) {
            HideProgressBar();
            $('#div_ReorderSections').html(data.View);
            $('#lbl_nodename').text(NodeName);
            //$('#btn_AddNodeSection').data('kioskareadetailid', KioskAreaDetailID);
            SetupSectionSortable();
            SetXEditablePlugin();
            $('#mo_ReorderSections').modal('show');
        });

    });


    //Nodos Index

    $(document).on('click', '.reorder-sections', function (e) {
        e.stopImmediatePropagation();
        var KioskAreaDetailID = $(this).data("kioskareadetailid");
        var NodeName = $(this).data("nodename");
        ShowProgressBar();
        // abrir detalles y devolver detalles de una llamada ajax
        $.get('/HR/KioskAdministration/GetReorderSectionsModal', {
            KioskAreaDetailID
        }).done(function (data) {
            HideProgressBar();
            $('#div_ReorderSections').html(data.View);
            $('#lbl_nodename').text(NodeName);
            $('#btn_AddNodeSection').data('kioskareadetailid', KioskAreaDetailID);
            SetupSectionSortable();
            SetXEditablePlugin();
            $('#mo_ReorderSections').modal('show');
        });

    });


    $(document).on('click', '#btn_AddRootNode', function (e) {
        e.stopImmediatePropagation();
        var KioskAreaID = $(this).data("kioskareaid");
        var KioskAreaDetailID = $(this).data("kioskareadetailid");
        var IsRoot = true;//$(this).data("isroot");

        ShowProgressBar();
        $.get('/HR/KioskAdministration/KioskAreaDetail_Init', { KioskAreaID, KioskAreaDetailID, IsRoot }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            SetVisualTypeStyle();
            SetNodeTypeSizes();
            $('#mo_KioskAreaDetailUpsert').modal('show');
            LoadDropzoneBackgroundImageKioskAreasDetailUpsert('.form-dropzone-backgroundimage');
            LoadDropzoneKioskAreasDetailFileType('.form-dropzone-filetype');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseSourcePath', 'form-dropzone-filetype');
            SetSelectPlugin();
            SetupNodeSortable();
            HideProgressBar();
            Setupminicolors();

            SetBorderToWhiteColor();

            ChangeTabs(event, 'backgroundImage');
            $('#div_MPE').css("display", "block");
            $("#tab_backgroundImage").addClass("active");

        });
    });

    $(document).on('click', '.add-node', function (e) {
        e.stopImmediatePropagation();
        var KioskAreaID = $(this).data("kioskareaid");
        var KioskAreaDetailID = $(this).data("kioskareadetailid");
        var IsRoot = false;//$(this).data("isroot");

        ShowProgressBar();
        $.get('/HR/KioskAdministration/KioskAreaDetail_Init', { KioskAreaID, KioskAreaDetailID, IsRoot }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            SetVisualTypeStyle();
            SetNodeTypeSizes();
            $('#mo_KioskAreaDetailUpsert').modal('show');
            LoadDropzoneBackgroundImageKioskAreasDetailUpsert('.form-dropzone-backgroundimage');
            LoadDropzoneKioskAreasDetailFileType('.form-dropzone-filetype');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseSourcePath', 'form-dropzone-filetype');
            SetSelectPlugin();
            SetupNodeSortable();
            HideProgressBar();
            Setupminicolors();

            SetBorderToWhiteColor();
            ChangeTabs(event, 'backgroundImage');
            $("#tab_backgroundImage").addClass("active");

        });
    });

    $(document).on("click", ".visuzalizationOptions", function (e) {
        e.stopImmediatePropagation();
        SetVisualTypeStyle();
    });

    $(document).on('click', '.edit-node', function (e) {
        e.stopImmediatePropagation();

        var KioskAreaID = $(this).data("kioskareaid");
        var KioskAreaDetailID = $(this).data("kioskareadetailid");
        var IsRoot = false;//$(this).data("isroot");

        ShowProgressBar();
        $.get('/KioskAdministration/KioskAreaDetail_EditInit', { KioskAreaID, KioskAreaDetailID, IsRoot }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            SetVisualTypeStyle();
            SetNodeTypeSizes();
            $('#mo_KioskAreaDetailUpsert').modal('show');
            LoadDropzoneBackgroundImageKioskAreasDetailUpsert('.form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            LoadDropzoneKioskAreasDetailFileType('.form-dropzone-filetype');
            RegisterButtonBrowseUrl('btn_BrowseSourcePath', 'form-dropzone-filetype');
            SetSelectPlugin();
            HideProgressBar();
            Setupminicolors();

            SetBorderToWhiteColor();
            ChangeTabs(event, 'backgroundImage');
            $("#tab_backgroundImage").addClass("active");

        });
    });

    $(document).on('click', '.delete-node', function (e) {
        e.stopImmediatePropagation();
        var row = $(this).closest("tr");
        SetConfirmBoxAction(function () {
            e.stopImmediatePropagation();
            ShowProgressBar();

            $.post("/HR/KioskAdministration/KioskAreaDetail_Delete", {
                ID: parseInt(row.data("kioskareadetailid"))
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    row.remove();
                }
                HideProgressBar();
            });
        }, LangResources.msg_DeleteNodeConfirmMessage);
    });

    $(document).on('click', '.details-control', function (e) {
        e.stopImmediatePropagation();

        var tr = $(this).closest('tr');

        if (tr.hasClass("shown")) {
            $('div.slider', tr.next()).slideUp(function () {

                var newTR = tr.next();
                var isRoot = newTR.find(".add-node").data("isroot");

                if (!(isRoot)) {
                    tr.removeClass('shown');
                    tr.next().remove();
                }

            });
        } else {
            var FileTypeID = 0;
            tr.after('');
            if ($('#ddl_F_FileType').val() != '' && $('#ddl_F_FileType').val() != null) {
                FileTypeID = parseInt($('#ddl_F_FileType').val());
            }

            ShowProgressBar();
            // abrir detalles y devolver detalles de una llamada ajax
            $.get('/HR/KioskAdministration/KioskAreaNodeList', {
                KioskAreaID: tr.data("entityid"),
                FileTypeID: FileTypeID,
                KioskAreaDetailID: tr.data("kioskareadetailid")
            }
            ).done(function (data) {
                //tr.after('');
                tr.after('<tr><td colspan="12" class="padding-0 noroot">' + data + '</td></tr>');
                tr.addClass('shown');
                var newTR = tr.next();
                newTR.find(".edit-node").data("isroot", false);
                newTR.find(".add-node").data("isroot", false);
                $('div.slider', newTR).slideDown();
                SetXEditablePlugin();
                //sortable for new tables

                SetupNodeSortable();
                Setupminicolors();
                HideProgressBar();
            });
        }
    });

    //CRUD nodos
    $(document).on('click', '#btnBackgroudImageChange', function (e) {
        e.stopImmediatePropagation();
        $(".form-dropzone-backgroundscreen").click();
    });

    $(document).on('click', '#btn_SaveNewKioskAreasDetail', function (e) {
        e.stopImmediatePropagation();
        var entity = new KioskAreaDetail();
        var list = [];
        var TransactionDetailID = $('#TransactionDetailID').val();

        entity.KioskAreaDetailID = $(this).data('entityid');
        entity.FileTypeID = $('#ddl_DAD_FileType').val();
        entity.FileTypeValueID = $('#hdnFileTypeValueID').val();
        entity.SizeID = $('#ddl_Add_Size').val();
        entity.BackColorID = $('#ddl_Add_BackColor').val();
        entity.FontColorID = $('#ddl_Add_FontColor').val();
        entity.IconID = $('#ddl_Add_Icon').val();
        entity.BackgroundImage = $('#txt_Add_BackGroundImage').data('url');
        entity.Seq = parseInt($('#txt_Add_SeqTag').val());
        entity.HRef = $('#txt_Add_Href').val();
        entity.IsWindow = $("#chkHrefVisualizationMode").is(":checked");
        entity.SourcePath = $('#txt_Add_SourcePath').data('url');
        entity.DataEffectID = $('#ddl_Add_DataEffect').val();
        entity.DataEffectDuration = $('#txt_Add_DataEffectDuration').val();
        entity.ParentKioskAreaDetailID = $('#ddl_ParentNode').val();
        entity.KioskAreaID = parseInt($('#ddl_Section').val());
        entity.BackgroundColor = $("#kiosk_area_detail_backgroundcolor").val();
        entity.FontColor = $("#kiosk_area_detail_fontcolor").val();

        $('#tb_AddKioskAreaDetailBody tr').each(function (index, row) {
            var entity = new GenericItem4Select();
            var txt_Language = getFindControl('txt_languagetext', row, row);

            entity.FieldKey = row.children[0].attributes['data-languageid'].value
            entity.RowNumber = index + 1;
            entity.FieldValue = (txt_Language.value == null) ? "" : txt_Language.value;

            list.push(entity);
        });

        var ElementVisibleID = $(".TypeOfNode:visible").attr("id");

        if (ElementVisibleID != null) {
            if ($("#" + ElementVisibleID).val() != "" && $("#" + ElementVisibleID).val() != null) {
                ShowProgressBar();
                $.post("/HR/KioskAdministration/KioskAreaDetail_Upsert", {
                    entity,
                    list,
                    TransactionDetailID
                }).done(function (data) {
                    notification("", data.ErrorMessage, data.notifyType);
                    if (data.ErrorCode == 0) {
                        $('#mo_KioskAreaDetailUpsert').modal('hide');
                        FillKioskAreaMasterTable();
                    }
                    HideProgressBar();
                })
            } else {
                notification("", LangResources.msg_MandatoryFileReference, "error");
            }

        } else {
            notification("", LangResources.msg_MandatoryNodeType, "error");
        }
    });

    //Kiosk Areas
    $(document).on('click', '.delete-kioskArea', function (e) {
        e.stopImmediatePropagation();
        var row = $(this).closest("tr");
        var KioskAreaID = this.attributes["data-kioskarea-id"].value;

        SetConfirmBoxAction(function () {
            e.stopImmediatePropagation();
            ShowProgressBar();
            $.post("/KioskAdministration/KioskArea_Delete", {
                KioskAreaID: KioskAreaID
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    row.remove();
                }
                HideProgressBar();
            });
        }, LangResources.msg_DeleteAreaConfirmMessage);
    });

    $(document).on('click', '.close-preview-video', function (e) {
        e.stopImmediatePropagation();
        var preview_video = $('#preview_video');
        preview_video.get(0).pause();
        $('#mo_PreviewVideo').modal('hide');
    });

    //Kiosk Area Details
    $(document).on('click', '.add-kioskareadetail', function (e) {
        e.stopImmediatePropagation();
        var ID = this.attributes["data-kioskarea-id"].value;

        ShowProgressBar();
        $.get('/HR/KioskAdministration/KioskAreaDetail_Init', { KioskAreaID: ID }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            $('#mo_KioskAreaDetailUpsert').modal('show');
            LoadDropzoneBackgroundImageKioskAreasDetailUpsert('.form-dropzone-backgroundimage');
            LoadDropzoneKioskAreasDetailFileType('.form-dropzone-filetype');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseSourcePath', 'form-dropzone-filetype');
            SetSelectPlugin();
            HideProgressBar();
            Setupminicolors();
        });

    });

    $(document).on('click', '.delete-kioskAreaDetail', function (e) {
        e.stopImmediatePropagation();

        var row = $(this).closest("tr");
        var KioskAreaDetailID = this.attributes["data-kioskareadetail-id"].value;

        SetConfirmBoxAction(function () {
            e.stopImmediatePropagation();
            ShowProgressBar();
            $.post("/KioskAdministration/KioskAreaDetail_Delete", {
                ID: KioskAreaDetailID
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    row.remove();
                }
                HideProgressBar();
            });
        }, LangResources.msg_DeleteAreaDetailConfirmMessage);

    });

    $(document).on('click', '.edit-kioskAreaDetail', function (e) {
        e.stopImmediatePropagation();

        var ID = this.attributes["data-kioskarea-id"].value;
        var KioskAreaDetailID = this.attributes["data-entityid"].value;

        ShowProgressBar();
        $.get('/HR/KioskAdministration/KioskAreaDetail_EditInit', { KioskAreaID: ID, KioskAreaDetailID: KioskAreaDetailID }
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            $('#mo_KioskAreaDetailUpsert').modal('show');
            LoadDropzoneBackgroundImageKioskAreasDetailUpsert('.form-dropzone-backgroundimage');
            RegisterButtonBrowseUrl('btn_BrowseBackGroundImage', 'form-dropzone-backgroundimage');
            SetSelectPlugin();
            HideProgressBar();
        });

    });

    $(document).on('click', '#btn_AddNodeSection', function (e) {
        e.stopImmediatePropagation();
        var KioskAreaDetailID = $(this).data('kioskareadetailid');
        ShowProgressBar();
        $.get('/HR/KioskAdministration/KioskArea_AddKioskAreaInit', {}
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            div_MPE.show();
            $('#div_KioskAreaUpsert_Seq').addClass('hidden');
            $('#btn_SaveNewKioskArea').data('kioskareadetailid', KioskAreaDetailID);
            $('#mo_KioskAreaUpsert').modal('show');
            SetSelectPlugin();
            HideProgressBar();
        });

    });

    $(document).on('click', '.delete-nodesection', function (e) {
        e.stopImmediatePropagation();
        var row = $(this).closest("tr");
        var ParentID = $(this).data("parentid");
        var KioskAreaID = $(this).data("kioskareaid");

        SetConfirmBoxAction(function () {
            e.stopImmediatePropagation();
            ShowProgressBar();

            $.post("/HR/KioskAdministration/KioskArea_Delete", {
                KioskAreaID
            }).done(function (data) {
                if (data.ErrorCode == 0) {
                    notification("", data.ErrorMessage, "success");
                    //actualizar el tbl_NodeSections  mo_ReorderSections
                    $.get('/HR/KioskAdministration/GetNodeSections', {
                        KioskAreaDetailID: ParentID
                    }
                    ).done(function (data) {
                        $('#div_table_nodesections').html(data.View);
                        $(".row-section").each(function (idx, elt) {
                            var newseq = idx + 1;
                            $(elt).children("td").first().text(newseq);
                            $(elt).attr("newseq", newseq);
                            var pk = $(elt).data("entityid");
                            var name = 'seq'
                            //llamada ajax a actualizar seq
                            $.post('/HR/KioskAdministration/UpdateQuickEditable',
                                { name, pk, value: newseq }
                            ).done(function (data) {
                                console.log(data.ErrorMessage);
                            });
                        });
                        //actualizar plugins
                        SetupSectionSortable();
                        SetXEditablePlugin();
                    });

                } else {
                    notification("", data.ErrorMessage, "error");
                }
                HideProgressBar();
            });
        }, LangResources.msg_DeleteAreaConfirmMessage);
    });

    $(document).on('click', '.carrusel-media-play-video', function (e) {
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

        $('#mo_PreviewVideo').modal('show');

    });

    $(document).on('change', '#ddl_ParentNode', function (e) {
        e.stopImmediatePropagation();
        var $ddl = $('#ddl_Section');

        ShowProgressBar();
        var ParentID = $('#ddl_ParentNode option:selected').val();
        $.get('/HR/KioskAdministration/GetParentNodeSections', { ParentID }
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

    function SetNodeTypeSizes() {

        var ddl_DAD_FileType = $("#ddl_DAD_FileType");
        var divBoxFileTypeLink = $('#divBoxFileTypeLink');
        var divBoxFileTypeMediaOrFile = $('#divBoxFileTypeMediaOrFile');
        var divBoxFileTypeGalery = $('.divBoxFileTypeGalery');
        var hdnFileTypeValueID = $('#hdnFileTypeValueID');
        var hdnFileExtensions = $('#hdnFileExtensions');

        if (ddl_DAD_FileType != null) {

            divBoxFileTypeLink.addClass("hidden");
            divBoxFileTypeMediaOrFile.addClass("hidden");
            divBoxFileTypeGalery.addClass("hidden");
            $("#divVisualizationMode").addClass("hidden");

            $("#div_ddl_DAD_FileType").removeClass("col-sm-4").addClass("col-sm-6");
            $("#divBoxFileTypeLink").removeClass("col-sm-4").addClass("col-sm-6");
            $("#divVisualizationMode").removeClass("col-sm-4").addClass("col-sm-6");


            var valueid = ddl_DAD_FileType.find(':selected').data('valueid');
            var validextension = ddl_DAD_FileType.find(':selected').data('valid-extension');

            $('#txt_Add_SourcePath').val('');
            $('#txt_Add_SourcePath').attr("data-url", "");

            hdnFileTypeValueID.val(valueid);
            hdnFileExtensions.val(validextension);
            if (ddl_DAD_FileType.val() != '') {
                if (valueid != null) {
                    if (valueid.toUpperCase() == 'L') {
                        divBoxFileTypeLink.removeClass("hidden");
                        $("#div_ddl_DAD_FileType").removeClass("col-sm-6").addClass("col-sm-4");
                        $("#divBoxFileTypeLink").removeClass("col-sm-6").addClass("col-sm-4");
                        $("#divVisualizationMode").removeClass("col-sm-6").addClass("col-sm-4");
                        $("#divVisualizationMode").removeClass("hidden");
                    } else if (valueid.toUpperCase() == 'G') {
                        divBoxFileTypeMediaOrFile.removeClass("hidden");
                        divBoxFileTypeGalery.removeClass("hidden");
                    } else {
                        divBoxFileTypeMediaOrFile.removeClass("hidden");
                    }
                }
            }
        }

    }

    $(document).on('change', '#ddl_DAD_FileType', function (e) {
        e.stopImmediatePropagation();
        SetNodeTypeSizes();
    });

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
            KioskAreaID: ID,
            list: list
        };

        ShowProgressBar();
        var QuickTranslateUpdateRequest = CallWebMethodPOST("/HR/KioskAdministration/KioskArea_QuickTranslateUpdate", QuickTranslateUpdateParams);

        QuickTranslateUpdateRequest.success(function (data) {
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, "success");
                $('#mo_CultureList').modal('hide');
                //si es reorder actualizar el listado
                if (updatenodesection) {
                    //actualizar el tbl_NodeSections  mo_ReorderSections
                    $.get('/HR/KioskAdministration/GetNodeSections', {
                        KioskAreaDetailID: ParentID
                    }
                    ).done(function (data) {
                        $('#div_table_nodesections').html(data.View);
                        //drag and drop
                        SetupSectionSortable();
                        SetXEditablePlugin();
                        HideProgressBar();
                    });
                }
                FillKioskAreaMasterTable();
            } else {
                notification("", data.ErrorMessage, "error");
            }
            HideProgressBar();
        });

    });

    //General Settings
    Dropzone.autoDiscover = false;
    $(document).on('click', '#btn_GralSetting', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/HR/KioskAdministration/KioskArea_GeneralSettings').done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");
            var div_MainKioskManagement = $('#div_MainKioskManagement');
            $(".max-length").maxlength();

            $("#div_MPE").css("display", "block");
            $("#div_MainKioskManagement").css("display", "none");

            $(".panel-collapse").on("click", function (e) {
                e.stopImmediatePropagation();
                panel_collapse($(this).parents(".panel"));
                $(this).parents(".dropdown").removeClass("open");
                return false;
            });

            $('#mo_GeneralSettings').modal('show');
            RegisterCarouselVideoSortable();
            RecalculateCarouselMediaSeq();
            HideProgressBar();
            div_MainKioskManagement.hide();

            SetupOnlyNumbers();

            LoadDropzoneCustom('.form-dropzone-background', function () {
                $.get("/Attachments/GetImage", {
                    ReferenceID: $('#ReferenceID').val(),
                    AttachmentType: $('#AttachmentType').val()
                }
                ).done(function (data) {
                    if (data.ErrorCode == 0) {
                        $("#div_dropzone_upload_img").empty();
                        $("#div_dropzone_upload_img").data("imageurl", data.ImgSrc);
                        $("#div_dropzone_upload_img").prepend('<a href="../..' + data.ImgSrc + '" target="_blank"><img src="../..' + data.ImgSrc + '"  style="width:100%" /></a>');
                        //$(".page-container").css("height", "100%")

                        $("#alert_background_image").css("display", "block");

                    } else {
                        notification("", data.ErrorMessage, data.notifyType);
                    }
                });
            }, "image/png, image/jpg, image/gif, image/jpeg");

            LoadDropzoneCustom('.form-dropzone-carouselmedia', function () {
                ShowProgressBar();
                $.get("/HR/KioskAdministration/GetCarruselMediaTemp", {
                    KioskCarouselMediaID: $("#KioskCarouselMediaID").val(),
                    TempAttachmentID: $('#ReferenceID').val()
                }).done(function (data) {
                    if (data.ErrorCode == 0) {
                        $("#div_boxGeneralSettingTable").empty();
                        $("#div_boxGeneralSettingTable").html(data.View);

                        //recalcular la seq en la tabla
                        RecalculateCarouselMediaSeq();
                        RegisterCarouselVideoSortable();

                    } else {
                        notification("", data.ErrorMessage, data.notifyType);
                    }
                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                }).always(function () {
                    HideProgressBar();
                });

            }, "video/mp4, image/png, image/jpg, image/gif, image/jpeg");
        });

    });

    $(document).on('click', '#btn_SaveGeneralSettings', function (e) {
        e.stopImmediatePropagation();
        var MediaInCarousel = [];
        var NewReferenceID = $("#KioskCarouselMediaID").val();

        $(".text-info").each(function () {
            MediaInCarousel.push($(this)[0].innerHTML);
        });

        ShowProgressBar();

        //var TheresImage = $("#div_dropzone_upload_img").find("img").attr("src"));

        $.post("/HR/KioskAdministration/KioskArea_UpdateGeneralSettings", {
            ImageURL: $("#div_dropzone_upload_img").data("imageurl"),
            SessionTime: $('#txt_SessionTime').val(),
            ReferenceID: $('#ReferenceID').val(),
            MediaInCarousel: MediaInCarousel,
            CarouselTransitionTime: $("#txt_CarouselTransitionTime").val()
        }).done(function (data) {
            HideProgressBar();
            if (data.ErrorCode == 0) {
                notification("", LangResources.msg_KioskConfigurationSaved, "success");

                if (data.KioskMediaID > 0) {
                    NewReferenceID = data.KioskMediaID;
                }

                //Se mueven los archivos temporales a la carpeta de carousel media
                $.get('/HR/KioskAdministration/ReplaceAttachments', {
                    ReferenceID: $('#ReferenceID').val(),
                    AttachmentType: "TEMPID",
                    NewReferenceID: NewReferenceID,
                    NewAttachmentType: "KIOSKCAROUSELMEDIA"
                }).done(function () {
                    $("#btn_BackToIndex").click();
                });
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });
    });

    $(document).on("click", "#btn_BackToIndex", function () {
        $("#div_MPE").css("display", "none");
        $("#div_MainKioskManagement").css("display", "block");
    });

    $(document).on('click', '.carrusel-media-delete', function (e) {
        e.stopImmediatePropagation();

        var row = $(this).closest("tr");
        var FileID = $(this).data('fileid');

        SetConfirmBoxAction(function () {
            e.stopImmediatePropagation();
            ShowProgressBar();
            $.get("/Attachments/Delete", {
                FileId: FileID,
                AttachmentType: "TEMPID"
            }).done(function (data) {
                notification("", data.ErrorMessage, data.notifyType);
                if (data.ErrorCode == 0) {
                    row.remove();
                }
                RecalculateCarouselMediaSeq();
                HideProgressBar();
            });
        }, LangResources.msg_ConfirmCarouselMediaDelete);

    });

    $(document).on('click', '.carrusel-media-download', function (e) {
        e.stopImmediatePropagation();

        var row = $(this).closest("tr");
        var FileID = row.data("pk");

        ShowProgressBar();
        $.get("/Attachments/Download", {
            FileId: FileID,
            AttachmentType: "TEMPID"
        }).done(function (data) {
            eval(data.JSCorefunction);
            HideProgressBar();
        });
    });

    $(document).on("change", "#tbl_attachments tbody", function () {
        alert("asd");
    });

    //Section
    $(document).on('click', '#btn_AddKiosk', function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/HR/KioskAdministration/KioskArea_AddKioskAreaInit', {}
        ).done(function (data) {
            var div_MPE = $('#div_MPE');
            div_MPE.html('');
            div_MPE.html(data);
            div_MPE.removeClass("hidden");

            $('#mo_KioskAreaUpsert').modal('show');
            SetSelectPlugin();
            HideProgressBar();
        });

    });

    $(document).on('click', '#btn_SaveNewKioskArea', function (e) {
        e.stopImmediatePropagation();
        var ddl_Add_Size = $('#ddl_Add_Size');
        var txt_Add_SeqTag = $('#txt_Add_SeqTag');
        var SizeID = 0;
        var Sequence = 0;
        var list = [];
        var ParentID = 0;
        ParentID = $(this).data('kioskareadetailid');
        $('#tb_AddKioskAreaBody').find('tr').each(function (index, element) {
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

        $.post("/HR/KioskAdministration/KioskArea_Upsert", {
            SizeID,
            Sequence,
            list,
            ParentID
        }).done(function (data) {
            if (data.ErrorCode == 0) {
                notification("", data.ErrorMessage, "success");
                $('#mo_KioskAreaUpsert').modal('hide');
                FillKioskAreaMasterTable();
                if (ParentID !== 0) {
                    //actualizar el tbl_NodeSections  mo_ReorderSections
                    $.get('/HR/KioskAdministration/GetNodeSections', {
                        KioskAreaDetailID: ParentID
                    }).done(function (data) {
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



        //AddKioskAreaParamsRequest.success(function (data) {
        //    if (data.ErrorCode == 0) {
        //        notification("", data.ErrorMessage, "success");
        //        $('#mo_KioskAreaUpsert').modal('hide');
        //        FillKioskAreaMasterTable();
        //        if (ParentID !== 0) {
        //            //actualizar el tbl_NodeSections  mo_ReorderSections
        //            $.get('/HR/KioskAdministration/GetNodeSections', {
        //                KioskAreaDetailID: ParentID
        //            }
        //            ).done(function (data) {
        //                $('#div_table_nodesections').html(data.View);
        //                //drag and drop
        //                SetupSectionSortable();
        //                SetXEditablePlugin();
        //                HideProgressBar();
        //            });
        //        }

        //    } else {
        //        notification("", data.ErrorMessage, "error");
        //    }
        //    HideProgressBar();
        //});

    });

    $(document).on("click", "#kiosk-background-delete", function () {
        SetConfirmBoxAction(function () {
            $("#div_dropzone_upload_img").empty();
            $("#div_dropzone_upload_img").append('<img src="" style="height:200px" />');

            $("#div_dropzone_upload_img").data("imageurl", "Nothing");
            $("#alert_background_image").css("display", "none");
        }, LangResources.msg_ConfirmRemoveBackgroundKiosk);
    });

    RegisterPluginDataTable(50);
    Setupminicolors();
    SetupNodeSortable();
    SetSelectPlugin();
    SetupOnlyNumbers();
    SetXEditablePlugin();

}
