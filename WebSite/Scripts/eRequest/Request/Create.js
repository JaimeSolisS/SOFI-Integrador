// =============================================================================================================================
//  Version: 
//  Author:  
//  Created Date: 
//  Description: Contiene funciones JS para la página de Create de eRequest
//  Modifications: 

// =============================================================================================================================
function CreateInit(LangResources) {
    //Plugins
    $(".select").selectpicker({
        liveSearch: false
    });
    
    $('#CompanyName').maxlength();
    $('#Justification').maxlength();
    $("#FormatID").prop('disabled', true);


    var FilesToDelete = [""];

    LoadDropzoneRequest('.form-dropzone', function () {
        var ReferenceID = $('#ReferenceID').val();
        var AttachmentType = $('#AttachmentType').val();
        $.get("/Attachments/Get",
            { ReferenceID, AttachmentType }
        ).done(function (data) {

        });
    });

    $(".datetimepicker").datetimepicker({
        format: 'YYYY[-]MM[-]DD'
    });
    //$(document).on('click', '#btn_Add', function (e) {
    //    e.stopImmediatePropagation();
    //    var count = $("#table_Detail th").length;
    //    var Concept = $('#Concept').val();
    //    var Reference1 = $('#Reference1').val();
    //    var Reference2 = $('#Reference2').val();
    //    var Reference3 = $('#Reference3').val();
    //    var Reference4 = $('#Reference4').val();
    //    var Reference5 = $('#Reference5').val();
    //    var Reference = "";
    //    var tdTable = "";
    //    if (count - 1 >= 1) {
    //        tdTable = "<td class='concept'>" + Concept + "</td>"
    //        for (i = 2; i <= count - 1; i++) {
    //            Reference = '#Reference' + (i - 1);
    //            tdTable = tdTable + '<td class="reference' + (i - 1) + '">' + $(Reference).clone()[0].outerHTML + '</td>'
    //        }
    //    }

    //    $("#table_Detail").append(
    //        "<tr class='responsibles_row'>" +
    //        tdTable +
    //        '<td><button class="btn btn-danger delete-detail"><span class="glyphicon glyphicon-trash"></span></button></td>' +
    //        "</td>" +
    //        "</tr>"
    //    );

    //    $('#Concept').val('');
    //    $('#Reference1').val('');;
    //    $('#Reference2').val('');;
    //    $('#Reference3').val('');;
    //    $('#Reference4').val('');;
    //    $('#Reference5').val('');;
    //});

    $(document).on('click', '#btn_Add', function (e) {
        e.stopImmediatePropagation();
        var count = $("#table_Detail th").length;
        var Concept = $('#Concept').val();
        var Reference1 = $('#Reference1').val();
        var Reference2 = $('#Reference2').val();
        var Reference3 = $('#Reference3').val();
        var Reference4 = $('#Reference4').val();
        var Reference5 = $('#Reference5').val();
        var Reference = "";
        var tdTable = "";
        var tableid = $('#table_Detail tr').length;
        if (count - 1 >= 1) {

            tdTable = "<td class='concept'>" + $('#Concept').clone()[0].outerHTML.replace('display: none;', '').replace('Concept', 'Concept' + tableid)  + "</td>"

            for (i = 2; i <= count - 1; i++) {
                Reference = '#Reference' + (i - 1);
                ReferenceOldID = 'Reference' + (i - 1);
                ReferenceNewID = ReferenceOldID + tableid;
                tdTable = tdTable + '<td class="reference' + (i - 1) + '" id="ref' + (i - 1) + '' + tableid + '">' + $(Reference).clone()[0].outerHTML.replace('display: none;', '').replace(ReferenceOldID, ReferenceNewID) + '</td>'
            }
        }
       
        $("#table_Detail").append(
            "<tr class='responsibles_row' data-requestdetailid='0'>" +
            tdTable +
            '<td><button class="btn btn-danger delete-detail"><span class="glyphicon glyphicon-trash"></span></button></td>' +
            "</td>" +
            "</tr>"
        );

        $('#Concept' + tableid).val(Concept);
        $('#Reference1' + tableid).val(Reference1);
        $('#Reference2' + tableid).val(Reference2);
        $('#Reference3' + tableid).val(Reference3);
        $('#Reference4' + tableid).val(Reference4);
        $('#Reference5' + tableid).val(Reference5);
        if ($('#Concept').attr('type') === 'text') {
            $('#Concept').val('');
        } else {
            $('#Concept option').prop("selected", false).trigger("change");
        }
        if ($('#Reference1').attr('type') === 'text') {
            $('#Reference1').val('');
        } else {
            $('#Reference1 option').prop("selected", false).trigger("change");
        }
        if ($('#Reference2').attr('type') === 'text') {
            $('#Reference2').val('');
        } else {
            $('#Reference2 option').prop("selected", false).trigger("change");
        }
        if ($('#Reference3').attr('type') === 'text') {
            $('#Reference3').val('');
        } else {
            $('#Reference3 option').prop("selected", false).trigger("change");
        }
        if ($('#Reference4').attr('type') === 'text') {
            $('#Reference4').val('');
        } else {
            $('#Reference4 option').prop("selected", false).trigger("change");
        }
        if ($('#Reference5').attr('type') === 'text') {
            $('#Reference5').val('');
        } else {
            $('#Reference5 option').prop("selected", false).trigger("change");
        }
        $("#table_Detail").find("select").attr("title", LangResources.chsn_SelectOption);
        $("#table_Detail").find("select").selectpicker();
    });

    $('#btnCancel').on('click', function () {
        SetConfirmBoxAction(function () {
            ShowProgressBar();
            window.location.href = '/eRequest/Request/Index';
        }, LangResources.msg_CancelReqCreate);
    });

    $('#btnBack').on('click', function () {
        ShowProgressBar();
        window.location.href = '/eRequest/Request/Index';
    });

    $(document).on("click", ".delete-detail", function (e) {
        e.stopImmediatePropagation();
        $(this).closest("tr").remove();
    });


    $(document).on("click", ".format-media-delete", function (e) {
        e.stopImmediatePropagation();
        var row = $(this).closest("tr");
        FilesToDelete.push(row.data("fileid"));
        row.remove();
        var t = document.getElementById("tbl_attachments");
        var trs = t.getElementsByTagName("tr");
        var tds = null;
        var secID = [];
        for (var i = 1; i < trs.length; i++) {
            tds = trs[i].getElementsByTagName("td");
            tds[0].innerText = i;
        }
    });
    //$(document).on('click', '#btn_SaveRequest', function (e) {
    //    e.stopImmediatePropagation();
    //    ShowProgressBar();
    //    var Details = [];
    //    var Facility = $('#MaterialOwned').val();
    //    var RequestDate = $('#txt_RequestDate').val();
    //    var Depeartment = $('#DepeartmentID').val();
    //    var Format = $('#FormatID').val();
    //    var Concept = $('#Justification').val();
    //    var Specification = $('#Specification').val();

    //    $(".detail_row").each(function (index, item) {
    //        var entity = {};
    //        $(item).find('td').each(function (i, tag) {
    //            if ($(tag).hasClass('concept')) {
    //                entity.Concept = $(tag).val();
    //            } else {
    //                entity[('Reference' + i)] = $(tag).val();
    //            }
    //        });
    //        entity.RequestLine = index + 1;
    //        Details.push(entity);
    //    });


    //    $.post("/Request/CreateNewRequest", {
    //        CreateNewRequest: Details
    //    }).done(function (data) {

    //        notification("", data.ErrorMessage, data.notifyType);
    //        HideProgressBar();

    //        if (data.ErrorCode == 0) {
    //            //Se mueven los archivos temporales a la carpeta de carousel media
    //            window.location = "/eRequest/Request/Index";
    //        } else {
    //            document.getElementById('btn_SaveNewOpportunity').disabled = false;
    //            notification("", data.ErrorMessage, "error");
    //            HideProgressBar();
    //        }

    //    }).fail(function (xhr, textStatus, error) {
    //        notification("", error.message, "error");
    //        HideProgressBar();
    //    }).always(function () {

    //    });
    //});
    $(document).on('click', '.btn-create', function (e) {
        e.stopImmediatePropagation();
        SetConfirmBoxAction(function () {
            var Details = [];
            var Facility = $('#MaterialOwned').val();
            var RequestDate = $('#txt_RequestDate').val();
            var Depeartment = $('#DepeartmentID').val();
            var Format = $('#FormatID').val();
            var Concept = $('#Justification').val();
            var Specification = $('#Specification').val();
            var AttachmentType = "TEMPID";
            var NewAttachmentType = "EREQUESTMEDIA";
            var NewReferenceID = 0;
            var ReferenceID = $('#ReferenceID').val();
            $('.btn-create').attr("disabled", true);
            $(".responsibles_row").each(function (index, item) {
                var entity = {};
                $(item).find('td').each(function (i, tag) {
                    if ($(tag).hasClass('concept')) {
                        entity.Concept = $('#Concept' + (index + 1)).val();
                    } else {
                        entity[('Reference' + i)] = $('#Reference' + i + '' + (index + 1)).val();
                    }
                });
                entity.RequestLine = index + 1;
                Details.push(entity);
            });
            $.post("/Request/CreateNewRequest", {
                GenericDetail: Details,
                AdditionalFields: getCollectionAdditionalFields(),
                FacilityID: Facility,
                RequestDate: RequestDate,
                DepeartmentID: Depeartment,
                FormatID: Format,
                Concept: Concept,
                Specification: Specification
            }).done(function (data) {

                if ($("#NewEdit_RequestID").val() == 0) {
                    NewReferenceID = data.ID;
                } else {
                    NewReferenceID = $("#NewEdit_RequestID").val();
                }
                HideProgressBar();

                if (data.ErrorCode == 0) {
                    //Se mueven los archivos temporales a la carpeta de carousel media
                    ShowProgressBar();
                    $.post("/Attachments/Copy", {
                        ReferenceID,
                        AttachmentType,
                        NewReferenceID,
                        NewAttachmentType
                    }).done(function (data) {
                        if (data.ErrorCode == 0) {
                            var start = 1;
                            $('.request-media-row').each(function () {
                                var row = $(this);
                                $.post("/Attachments/Properties_QuickUpsert", {
                                    FileId: row.data("fileid"),
                                    PropertyName: "Seq",
                                    PropertyValue: start,
                                    PropertyTypeID: 0
                                }).done(function (dataQuick) {
                                    if (dataQuick.ErrorCode == 0) {

                                    }
                                });
                                row.find("td:first-child").text(start);
                                start++;
                            });
                            window.location = "/eRequest/Request/Index";
                            var start = 1;
                        } else {
                            $('.btn-create').attr("disabled", false);
                            notification("", data.ErrorMessage, "error");
                            HideProgressBar();
                        }

                    }).fail(function (xhr, textStatus, error) {
                        notification("", error.message, "error");
                        $('.btn-create').attr("disabled", false);
                        HideProgressBar();
                    }).always(function () {

                    });
                } else {
                    $('.btn-create').attr("disabled", false);
                    notification("", data.ErrorMessage, "error");
                    HideProgressBar();
                }

            }).fail(function (xhr, textStatus, error) {
                HideProgressBar();
                $('.btn-create').attr("disabled", false);
            }).always(function () {

            });

        }, LangResources.msg_CreateReqConfirmation);
    });

    $(document).on('click', '.btn-edit', function (e) {
        e.stopImmediatePropagation();
        var Details = [];
        var Facility = $('#MaterialOwned').val();
        var RequestDate = $('#txt_RequestDate').val();
        var Depeartment = $('#DepeartmentID').val();
        var Format = $('#FormatID').val();
        var Concept = $('#Justification').val();
        var Specification = $('#Specification').val();
        var AttachmentType = "TEMPID";
        var NewAttachmentType = "EREQUESTMEDIA";
        var NewReferenceID = 0;
        var ReferenceID = $('#ReferenceID').val();
        var RequestID = $('#NewEdit_RequestID').val();
        $('.btn-create').attr("disabled", true);
        $(".responsibles_row").each(function (index, item) {
            var entity = {};
            $(item).find('td').each(function (i, tag) {
                if ($(tag).hasClass('concept')) {
                    entity.Concept = $('#Concept' + (index + 1)).val();
                } else {
                    entity[('Reference' + i)] = $('#Reference' + i + '' + (index + 1)).val();
                }
            });
            entity.RequestGenericDetailID = $(item).data('requestdetailid');
            entity.RequestLine = index + 1;
            Details.push(entity);
        });
        $.post("/Request/EditRequest", {
            RequestID: RequestID,
            GenericDetail: Details,
            AdditionalFields: getCollectionAdditionalFields(),
            FilesToDelete: FilesToDelete.join(','),
            FacilityID: Facility,
            RequestDate: RequestDate,
            DepeartmentID: Depeartment,
            FormatID: Format,
            Concept: Concept,
            Specification: Specification
        }).done(function (data) {

            if ($("#NewEdit_RequestID").val() == 0) {
                NewReferenceID = data.ID;
            } else {
                NewReferenceID = $("#NewEdit_RequestID").val();
            }
            HideProgressBar();

            if (data.ErrorCode == 0) {
                //Se mueven los archivos temporales a la carpeta de carousel media
                ShowProgressBar();
                $.post("/Attachments/Copy", {
                    ReferenceID,
                    AttachmentType,
                    NewReferenceID,
                    NewAttachmentType
                }).done(function (data) {
                    if (data.ErrorCode == 0) {
                        var start = 1;
                        $('.request-media-row').each(function () {
                            var row = $(this);
                            $.post("/Attachments/Properties_QuickUpsert", {
                                FileId: row.data("fileid"),
                                PropertyName: "Seq",
                                PropertyValue: start,
                                PropertyTypeID: 0
                            }).done(function (dataQuick) {
                                if (dataQuick.ErrorCode == 0) {

                                }
                            });
                            row.find("td:first-child").text(start);
                            start++;
                        });
                        window.location = "/eRequest/Request/Index";
                        var start = 1;
                    } else {
                        $('.btn-create').attr("disabled", false);
                        notification("", data.ErrorMessage, "error");
                        HideProgressBar();
                    }

                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                    $('.btn-create').attr("disabled", false);
                    HideProgressBar();
                }).always(function () {

                });
            } else {
                $('.btn-create').attr("disabled", false);
                notification("", data.ErrorMessage, "error");
                HideProgressBar();
            }

        }).fail(function (xhr, textStatus, error) {
            HideProgressBar();
            $('.btn-create').attr("disabled", false);
        }).always(function () {

        });
    });

    $(document).on('click', '.format-media-download2', function (e) {
        e.stopImmediatePropagation();

        var FileID = $(this).data("fileid");

        ShowProgressBar();
        $.get("/Attachments/Download", {
            FileId: FileID,
            AttachmentType: "TEMP"
        }).done(function (data) {
            eval(data.JSCorefunction);
            HideProgressBar();
        });
    });

    $(document).on('click', '.format-media-download', function (e) {
        e.stopImmediatePropagation();

        var row = $(this).closest("tr");
        var FileID = row.data("fileid");

        ShowProgressBar();
        $.get("/Attachments/Download", {
            FileId: FileID,
            AttachmentType: "EREQUESTMEDIA"
        }).done(function (data) {
            eval(data.JSCorefunction);
            HideProgressBar();
        });
    });

}

function SetupDataTable() {
    $(".datatable").dataTable({
        "language": {
            "url": "/Base/DataTableLang"
        },
        "pageLength": 100,
        responsive: true
    });
}
function getCollectionAdditionalFields() {
    var list = [];
    var div_boxAdditionalFieldSection = $('#div_boxAdditionalFieldSection');
    div_boxAdditionalFieldSection.find('.additional-field').each(
        function (index, control) {
            var entity = new AdditionalFields();
            if (control.type === 'text') {
                entity.FieldValue = control.value;
                entity.TableAdditionalFieldID = $(control).data('id');
                entity.TableAdditionalFieldValueID = $(control).data('valueid');
                list.push(entity);
            } else if (control.type === 'textarea') {
                entity.FieldValue = control.value;
                entity.TableAdditionalFieldID = $(control).data('id');
                entity.TableAdditionalFieldValueID = $(control).data('valueid');
                list.push(entity);
            } else if (control.type === 'select-one') {
                var selectd = $(control).find(':selected');
                entity.FieldValue = control.value;
                entity.TableAdditionalFieldID = $(control).data('id');
                entity.TableAdditionalFieldValueID = $(control).data('valueid');
                list.push(entity);
            } else if (control.type === 'checkbox') {
                entity.FieldValue = control.value;
                entity.TableAdditionalFieldID = $(control).data('id');
                entity.TableAdditionalFieldValueID = $(control).data('valueid');
                list.push(entity);
            }
        });
    return list;
}
function LoadDropzoneRequest(form_selector, update_function, type_extension) {
    $(form_selector).dropzone({
        addRemoveLinks: true,
        createImageThumbnails: false,
        acceptedFiles: type_extension,
        previewTemplate: '<div class="uploaded-image" style="display:none;"></div>',
        init: function () {
            this.on("complete", function (file) {
                if (this.getUploadingFiles().length === 0 && this.getQueuedFiles().length === 0) {
                    this.removeAllFiles(true);
                }
                var data = JSON.parse(file.xhr.responseText);
                notification(data.Title, data.ErrorMessage, data.notifyType);
                update_function();
            });
        },
        success: function (file, response) {
            console.log(file);
            console.log(response);
            var index = $('.request-media-row').length + 1;
            $("#tbl_attachments").append(
                '<tr class="font-size-sm request-media-row" data-sequence="' + index + '" data-fileid="' + response.ID + '">' +
                '<td class="center w-100">' +
                index +
                "</td>" +
                '<td class="text-info">' +
                response.filesnames[0] +
                "</td>" +
                '<td class="pull-right">' +
                '<button class="btn btn-info format-media-download2" data-filename="' + response.filesnames[0] + '" data-url="' + response.filespath[0] + '" data-fileid="' + response.ID + '" title="Play"><span class="glyphicon glyphicon-play"></span></button>' +
                ' <button class="btn btn-danger format-media-delete" data-fileid="0" title="Remover elemento multimedia"><span class="glyphicon glyphicon-trash"></span></button>' +
                "</td>" +
                "</tr>"
            );
            RegisterRequestSortable();
        }
    });

}

function RegisterRequestSortable() {
    $(".sortable-table tbody").sortable({
        stop: function (event, ui) {
            RecalculateRequestSeq();
        }
    });
}

function AdditionalFields() {
    this.TableAdditionalFieldID = 0;
    this.TableAdditionalFieldValueID = 0;
    this.FieldValue = '';
}

function RecalculateRequestSeq() {
    var start = 1;
    $('.request-media-row').each(function () {
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