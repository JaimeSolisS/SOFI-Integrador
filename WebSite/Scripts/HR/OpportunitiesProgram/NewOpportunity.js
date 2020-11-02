// =============================================================================================================================
//  Version: 20200211
//  Author:  Luis Hernandez
//  Created Date: 12 Febrero 2020
//  Description:  Scripts para el programa oportunidades
//  Modifications: 
// =============================================================================================================================
function RecalculateOpportunitySeq() {
    var start = 1;
    $('.opportunity-media-row').each(function () {
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

function RegisterOpportunitySortable() {
    $(".sortable-table tbody").sortable({
        stop: function (event, ui) {
            RecalculateOpportunitySeq();
        }
    });
}
function NewOpportunity(LangResources) {

    var FilesToDelete = [];
    OpportunityProgramDescriptionText();
    //Funciones
    RegisterOpportunitySortable();
    function SaveOpportunity(actionName) {
        ShowProgressBar();
        var Responsibles = [];
        var AttachmentType = "TEMPID";
        var NewAttachmentType = "OPPORTUNITYPROGRAMMEDIA";
        var NewReferenceID = 0;
        var ReferenceID = $('#ReferenceID').val();
        var DescriptionTypeID = $('input[type=radio][name=PublicationType]:checked').data('descriptiontypeid');

        $(".responsibles_row  td:first-child").each(function () {
            var Responsible = {
                ID: $(this).text()
            }
            Responsibles.push(Responsible);
        });

        $.post("/OpportunitiesProgram/" + actionName, {
            OpportunityProgramID: $("#NewEdit_OpportunityProgramID").val(),
            Name: $("#txt_OpportunityProgramName").val(),
            Description: $(".note-editable").html().toString().replace(/</g, "&lt;").replace(/>/g, "&gt;"),
            Responsibles: Responsibles,
            DescriptionTypeID: DescriptionTypeID,
            DepartmentID: $("#ddl_DepartmentsList option:selected").val(),
            ShiftID: $("#ddl_ShiftsList option:selected").val(),
            GradeID: $("#ddl_GradesList option:selected").val(),
            DdlFacilityID: $("#ddl_FacilitiesList option:selected").val(),
            ExpirationDate: $("#txt_OpportunityExpirationDate").val(),
            NotificationTypeID: $("#NewEdit_NotificationTypeID").val(),
            FilesToDelete: FilesToDelete.join(',')
        }).done(function (data) {
            if ($("#NewEdit_OpportunityProgramID").val() == 0) {
                NewReferenceID = data.ID;
            } else {
                NewReferenceID = $("#NewEdit_OpportunityProgramID").val();
            }

            notification("", data.ErrorMessage, data.notifyType);
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
                        $('.opportunity-media-row').each(function () {
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
                        window.location = "/HR/OpportunitiesProgram/Index";
                        var start = 1;


                    } else {
                        document.getElementById('btn_SaveNewOpportunity').disabled = false;
                        notification("", data.ErrorMessage, "error");
                        HideProgressBar();
                    }

                }).fail(function (xhr, textStatus, error) {
                    notification("", error.message, "error");
                    document.getElementById('btn_SaveNewOpportunity').disabled = false;
                    HideProgressBar();
                }).always(function () {

                });
            } else {
                document.getElementById('btn_SaveNewOpportunity').disabled = false;
                HideProgressBar();
            }

        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
            document.getElementById('btn_SaveNewOpportunity').disabled = false;
            HideProgressBar();
        }).always(function () {

        });
    }


    function OpportunityProgramDescriptionText() {
        if ($("#Text").is(":checked")) {
            var datos = $("#OpportunityProgramDescription").val().toString().replace(/&lt;/g, "<").replace(/&gt;/g, ">");
            $('.note-editable').append(datos);
        }
    }
    function ChangeDescriptionType(DescriptionType) {
        if (DescriptionType == "Text") {
            $("#rdio_option_free_text").css("display", "block");
            $("#rdio_option_img").css("display", "none");
        } else {
            $("#rdio_option_free_text").css("display", "none");
            $("#rdio_option_img").css("display", "block");
        }
    }
    function DescriptionTypeInit() {
        if ($("#NewEdit_NotificationTypeID").val() != 0) {
            ChangeDescriptionType($('input[type=radio][name=PublicationType]:checked').attr("id"));
        } else {
            ChangeDescriptionType("Text");
            $("input[type=radio][name=PublicationType][id=Text]").prop("checked", true);
        }
    }

    function InitializeDatePicker() {
        var CultureID = $("#CultureID").val();
        var region = $('.SelectCulture_' + CultureID).data("langabreviation");
        $("#txt_OpportunityExpirationDate").datepicker({
            format: 'yyyy-mm-dd',
            language: region
        });
    }
    function LoadDropzoneOpportunityProgram(form_selector, update_function, type_extension) {
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
                var index = $('.opportunity-media-row').length + 1;
                $("#tbl_attachments").append(
                    '<tr class="font-size-sm opportunity-media-row" data-sequence="' + index + '" data-fileid="' + response.ID + '">' +
                    '<td class="center w-100">' +
                    index +
                    "</td>" +
                    '<td class="text-info">' +
                    response.filesnames[0] +
                    "</td>" +
                    '<td class="pull-right">' +
                    '<button class="btn btn-info opportunity-program-media-download2" data-filename="' + response.filesnames[0] + '" data-url="' + response.filespath[0] + '" data-fileid="' + response.ID + '" title="Play"><span class="glyphicon glyphicon-play"></span></button>' +
                    ' <button class="btn btn-danger opportunity-program-media-delete" data-fileid="0" title="Remover elemento multimedia"><span class="glyphicon glyphicon-trash"></span></button>' +
                    "</td>" +
                    "</tr>"
                );
                RegisterOpportunitySortable();
            }
        });
    }
    function AutoCompleteDrop() {
        ShowProgressBar();

        $.get("/KioskRequestAdministrator/FilterResponsible", {
            SearchUserInfo: ""
        }).done(function (data) {
            $("#txt_AssignResponsible").empty();
            $.each(data.ResponsiblesList, function (k, v) {
                $("#txt_AssignResponsible").append("<option id=" + v.ID + " value=" + v.ID + " data-name=" + v.FullName + " data-email=" + v.eMail + " data-dateadded=" + v.DateAddedFormat + " data-department=" + v.DepartmentName + " data-employeenumber= " + v.EmployeeNumber + ">" + v.FullName + "</option>");
            });
            $("#txt_AssignResponsible").selectpicker("refresh");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });

    }


    function pregunta_Validar_Campos(id) {
        var t = document.getElementById("table_Responsibles");
        var trs = t.getElementsByTagName("tr");
        var tds = null;
        var secID = [];
        for (var i = 1; i < trs.length; i++) {
            tds = trs[i].getElementsByTagName("td");
            secID.push(tds[0].innerText);
        }
        var search = secID.indexOf(id);
        if (search > -1) {
            notification("", "No puedes repetir el usuario asignado", "error");  //cambia esta linea para poner una traduccion

            return false;
        } else {
            return true;
        }
    }
    //Inicialización
    $("select").selectpicker();
    //$("#btn_SaveEditedOpportunity")[0].title = "Guardar Cambios" //Rolando del futuro agrega por favor la traduccion
    InitializeDatePicker();
    $(".summernote").summernote();
    $(".max-length").maxlength();
    DescriptionTypeInit();
    AutoCompleteDrop();
    //TemporalitySeq();
    LoadDropzoneOpportunityProgram('.form-dropzone', function () {
        var ReferenceID = $('#ReferenceID').val();
        var AttachmentType = $('#AttachmentType').val();
        $.get("/Attachments/Get",
            { ReferenceID, AttachmentType }
        ).done(function (data) {

        });
    }, "video/mp4, image/png, image/jpg, image/gif, image/jpeg");

    //Eventos
    $("input[type=radio][name=PublicationType]").change(function (e) {
        ChangeDescriptionType($('input[type=radio][name=PublicationType]:checked').attr("id"));
    });

    $(document).on("click", "#btn_CancelNewOportunity", function (e) {
        ShowProgressBar();
    });

    $(document).on("keyup", "#txt_search_by_text", function (e) {
        ShowProgressBar();
        $.get("/KioskRequestAdministrator/FilterResponsible", {
            SearchUserInfo: $("#txt_search_by_text").val()
        }).done(function (data) {
            $("#datalist-responsibles").empty();
            $.each(data.ResponsiblesList, function (k, v) {
                var DepartmentNameTemp = v.DepartmentName;
                if (DepartmentNameTemp == null) {
                    DepartmentNameTemp = "N/A";
                }
                $('#datalist-responsibles').append('<option data-department="' + DepartmentNameTemp + '" data-id="' + v.ID + '" data-email="' + v.eMail + '" data-dateadded="' + v.DateAddedFormat + '">' + v.FullName + '</option>');
            });
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on("click", "#btn_AddResponsible", function (e) {
        e.stopImmediatePropagation();
        var selectedIndexc = document.getElementById('txt_AssignResponsible').options.selectedIndex;
        var name = document.getElementById('txt_AssignResponsible').options[selectedIndexc].dataset.name;
        var id = document.getElementById('txt_AssignResponsible').options[selectedIndexc].value;
        var email = document.getElementById('txt_AssignResponsible').options[selectedIndexc].dataset.email;
        var date = document.getElementById('txt_AssignResponsible').options[selectedIndexc].dataset.dateadded;
        var department = document.getElementById('txt_AssignResponsible').options[selectedIndexc].dataset.department;
        var employeeNumber = document.getElementById('txt_AssignResponsible').options[selectedIndexc].dataset.employeenumber;
        if (employeeNumber == "null") {
            employeeNumber = "";
        }
        if (department == "null") {
            department = "";
        }
        if (name != null && id != null && id != 0) {
            if (pregunta_Validar_Campos(id)) {
                $("#table_Responsibles").append(
                    "<tr class='responsibles_row'>" +
                    '<td style="display:none;">' +
                    id +
                    "</td>" +
                    "<td>" +
                    employeeNumber +
                    "</td>" +
                    "<td>" +
                    name +
                    "</td>" +
                    "<td>" +
                    email +
                    "</td>" +
                    "<td>" +
                    department +
                    "</td>" +
                    "<td>" +
                    date +
                    "</td>" +
                    "<td>" +
                    $("#NewEdit_CurrentUserName").val() +
                    '<td><button class="btn btn-danger delete-opportunity-responsible"><span class="glyphicon glyphicon-trash"></span></button></td>' +
                    "</td>" +
                    "</tr>"
                );
            }
        }
    });

    $(document).on("click", "#btn_SaveNewOpportunity", function (e) {
        e.stopImmediatePropagation();
        SaveOpportunity("SaveNewOpportunity");
        document.getElementById('btn_SaveNewOpportunity').disabled = true;
    });

    $(document).on("click", "#btn_SaveEditedOpportunity", function (e) {
        e.stopImmediatePropagation();
        SaveOpportunity("Update")
    });

    $(document).on("click", ".delete-opportunity-responsible", function (e) {
        e.stopImmediatePropagation();
        $(this).closest("tr").remove();
    });

    $(document).on("click", ".opportunity-program-media-delete", function (e) {
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

    $(document).on('click', '.opportunity-program-media-download2', function (e) {
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

    $(document).on('click', '.opportunity-program-media-download', function (e) {
        e.stopImmediatePropagation();

        var row = $(this).closest("tr");
        var FileID = row.data("fileid");

        ShowProgressBar();
        $.get("/Attachments/Download", {
            FileId: FileID,
            AttachmentType: "OPPORTUNITYPROGRAMMEDIA"
        }).done(function (data) {
            eval(data.JSCorefunction);
            HideProgressBar();
        });
    });

    $(document).on('click', '.opportunity-program-play-video', function (e) {
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

    $(document).on('click', '.close-preview-video', function (e) {
        e.stopImmediatePropagation();
        var preview_video = $('#preview_video');
        preview_video.get(0).pause();
        $('#mo_PreviewVideo').modal('hide');
    });

    $("#ddl_FacilitiesList").change(function () {
        ShowProgressBar();
        $.get('/HR/OpportunitiesProgram/GetShiftList', {
            Facility: $("#ddl_FacilitiesList option:selected").val()
        }).done(function (data) {
            $("#ddl_ShiftsList").empty();
            $.each(data, function (k, v) {
                $("#ddl_ShiftsList").append('<option value="' + v.value + '">' + v.text + '</option>');
            });
            $("#ddl_ShiftsList").selectpicker("refresh");
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });


    //BEGIN - dd new user account
    $("#txt_AssignResponsible").change(function (e) {
        var option = $("#txt_AssignResponsible option:selected");
        if (option.val() == 0 && option.data("name") == '--') {
            ShowProgressBar();
            $.get("/KioskRequestAdministrator/GetAddUserModal").done(function (data) {
                $("#div_Modal").html(data);
                $(".select").selectpicker();
                $("#mo_AddNewUser").modal("show");
                $('#txt_AssignResponsible').val(0);
                $('#txt_AssignResponsible').selectpicker("refresh");

            }).fail(function (xhr, textStatus, error) {
                notification("", error.message, "error");
            }).always(function () {
                HideProgressBar();
            });
        }
    });

    $(document).on("click", "#btnSearchUser", function (e) {
        e.stopImmediatePropagation();
        ShowProgressBar();
        $.get('/HR/KioskRequestAdministrator/SearchADUsers', {
            UserText: $('#txtSearchUser').val()
        }).done(function (data) {
            $("#div_ResponsiblesResultTable").html(data.View);
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    $(document).on('click', '.addUserAccount', function (e) {
        e.stopImmediatePropagation();
        var info = $(this);
        var User = {
            UserAccountID: info.data("useraccountid"),
            FirstName: info.data("firstname"),
            eMail: info.data("email")
        }

        ShowProgressBar();
        $.post('/HR/KioskRequestAdministrator/AddResponsibleAccount', {
            User: User
        }).done(function (data) {
            notification("", data.ErrorMessage, data.notifyType);

            if (data.ErrorCode == 0) {
                LoadResponsibles(data.ID);
            }
        }).fail(function (xhr, textStatus, error) {
            notification("", error.message, "error");
        }).always(function () {
            HideProgressBar();
        });
    });

    function LoadResponsibles(IDToSelect) {
        $.get("/KioskRequestAdministrator/GetRequestResponsibles", {
            UserText: null
        }).done(function (data) {
            $("#txt_AssignResponsible").empty();
            $.each(data.RequestResponsiblesList, function (k, v) {
                $("#txt_AssignResponsible").append('<option value="' + v.ID + '" data-name="' + v.FullName + '" ' + 
                    'data-email="' + v.eMail + '" data-dateadded="' + v.DateAddedFormat + '" data-department="' + v.DepartmentName + '"'  + 
                    'data-employeenumber="' + v.EmployeeNumber + '">' + v.FullName + '</option > ')
            });

            if (IDToSelect != null) {
                $("#txt_AssignResponsible").val(IDToSelect);
                $("#txt_AssignResponsible").selectpicker("refresh");
            }
            $("#mo_AddNewUser").modal("toggle");
        });
    }

    //END - Add new user account



}
