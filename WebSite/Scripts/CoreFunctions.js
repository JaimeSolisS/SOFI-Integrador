
$(document).ajaxError(function (xhr, props) {
    if (props.status === 401) {
        //una notificacion que espere hasta que se le de OK para redireccionar
        //var test = notification("", props.statusText, "warning");
        //var test = messageOK("", props.statusText, "warning","OK");
        //alert(props.statusText);
        console.log("CoreFunctions ajaxError: " + props.statusText);
        Lobibox.alert("warning",
            {
                title: "Alert",
                msg: props.statusText,
                buttons: {
                    ok: {
                        'class': 'btn btn-default',
                        text: "OK",
                        closeOnClick: true
                    }
                },
                callback: function (lobibox, type) {
                    if (type === 'OK') {
                        //location.href = '/User/Login';
                        location.reload();
                    } else {
                        //location.href = '/User/Login';
                        location.reload();
                    }
                }
            });

        //location.reload();
    }
});

function messageOK(_title, _message, _type, _btnOKText) {
    Lobibox.alert(_type,
        {
            title: _title,
            msg: _message,
            buttons: {
                ok: {
                    'class': 'btn btn-default',
                    text: _btnOKText,
                    closeOnClick: true
                }
            }
        });
}

function notification(_title, _message, _type) {
    //reference: http://themifycloud.com/demos/templates/joli/ui-alerts-popups.html
    //noty({ text: 'Successful action', layout: 'topRight', type: 'success' });
    //noty({ text: 'There was an error', layout: 'topRight', type: 'error' });
    noty({
        text: _message,
        layout: 'topRight',
        type: _type,
        /*Integración de delay*/
        timeout: 4000
    });
}


//funcion que solo permite introducir numeros separado por .
function SetupOnlyDecimal() {
    //$(".onlydecimals").on("keypress keyup blur", function (event) {
    $(document).on('keypress keyup', '.onlydecimals', function (e) {
        e.stopImmediatePropagation();

        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
}

//solo permite numeros
function SetupOnlyNumbers() {
    //$(".onlynumbers").on("keypress keyup blur", function (event) {
    $(document).on('keypress keyup', '.onlynumbers', function (e) {
        e.stopImmediatePropagation();

        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if (event.which < 48 || event.which > 57) {
            event.preventDefault();
        }
    });
}

//funcion para ver imagenes en un modal de resources
function ViewResourceImage(ID, src, FileName, CreatedBy, CreatedOn, Orientation) {
    try {
        $("#img_Resource").attr("src", src);
        $('#sR_FileName').text(FileName);
        $('#sR_CreatedBy').text(CreatedBy);
        $('#sR_CreatedOn').text(CreatedOn);
        $("#img_Resource").removeClass();
        $("#img_Resource").addClass("img-responsive img-centered ");
        $("#img_Resource").addClass(Orientation);
        $('#m_ImageResource').modal('show');
    }
    catch (err) {
        // console.log('ERROR' + err.message);
    }
    finally { return false; }
}

//descargar un archivo basado en la ruta
function DownloadFile(src) {
    try {
        $('<iframe src="' + src + '"></iframe>').appendTo('body').hide();
    } catch (e) {
        // console.log('ERROR' + err.message);
    }
    finally { return false; }
}

//abre un archivo de descarga en una nueva ventana
function OpenResourceFile(src) {
    try {
        window.open(src, '_blank', 'fullscreen=yes');
    } catch (err) {
        // console.log('ERROR' + err.message);
    }
    finally { return false; }
}

//abre un archivo de descarga en una nueva ventana
function OpenFocusResourceFile(src) {
    try {
        var win = window.open(src, '_blank');
        win.focus();
    } catch (err) {
        // console.log('ERROR' + err.message);
    }
    finally { return false; }
}

//funcion generica para plugin Dropzone, se le pasa la funcion que actualiza la tabla de attachments
function LoadDropzone(form_selector, update_function) {
    $(form_selector).dropzone({
        addRemoveLinks: true,
        createImageThumbnails: false,
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
        }
    });
}
 
//funcion generica para plugin Dropzone, esta opcion posee la funcionalidad de escoger las extenciones validas que puedes cargar en el dropzone
//      El formato del parametro type_extension debe ser  "tipo_de_archivo/extencion", si el parametro es nulo, aceptara todos los archivos
//              Ejemplos:   "image/png, video/mp4, application/pdf"
//
//  Link de tipos mime: https://developer.mozilla.org/es/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Lista_completa_de_tipos_MIME
function LoadDropzoneCustom(form_selector, update_function, type_extension) {
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
        }
    });
}

// Funcion para configurar el link para poder bajar un archivo
function SetDownloadAttachments(download_selector) {
    $(document).on('click', download_selector, function (e) {
        e.stopImmediatePropagation();
        var entityid = $(this).data("entityid");
        var AttachmentType = $(this).data("attachmenttype");
        $.get("/Attachments/Download",
            { FileId: entityid, AttachmentType: AttachmentType }
        ).done(function (data) {
            if (data.ErrorCode === 0) {
                eval(data.JSCorefunction);
            } else {
                notification("", data.ErrorMessage, "error");
            }
        });
    });
}

// Funcion para actualizar el tipo de archivos del adjunto y se realice el UPDATE en la tabla correspondiente
function SetupUpdateTypeAttachment(filetype_selector) {
    $(document).on('change', filetype_selector, function (e) {
        e.stopImmediatePropagation();
        var fileid = $(this).parent().data("entityid");
        var AttachmentType = $(this).parent().data("attachmenttype");
        var filetypeid = $(this).val();
        $.post("/Attachments/Update",
            { FileId: fileid, AttachmentType: AttachmentType, FileTypeId: filetypeid })
            .done(function (data) {
                if (data.ErrorCode !== 0) {
                    notification("", data.ErrorMessage, data.notifyType);
                }
            });
    });
}

// Funcion para realizar el borrado de un archivo adjunto
function SetDeleteAttachments(delete_selector, LangResources, update_function) {
    $(document).on('click', delete_selector, function (e) {
        e.stopImmediatePropagation();
        var entityid = $(this).data("entityid");
        var AttachmentType = $(this).data("attachmenttype");
        Lobibox.confirm({
            title: LangResources.lbl_Delete,
            msg: LangResources.ntf_DeleteAttachment,
            buttons: {
                yes: {
                    'class': 'btn btn-success',
                    text: LangResources.btn_ConfirmYES,
                    closeOnClick: true
                },
                no: {
                    'class': 'btn btn-danger',
                    text: LangResources.btn_ConfirmNO,
                    closeOnClick: true
                }
            },
            callback: function (lobibox, type) {
                if (type === 'yes') {
                    $('.loading-process-div').show();
                    $.get("/Attachments/Delete",
                        { FileId: entityid, AttachmentType: AttachmentType }
                    ).done(function (data) {
                        if (data.ErrorCode === 0) {
                            update_function();
                            //eval(update_function);
                        } else {
                            notification("", data.ErrorMessage, "error");
                        }
                    }).always(function () {
                        $('.loading-process-div').hide();
                    });

                }
            }
        });
    });
}


// G.Sánchez (25 Julio 2018). Remarcar en amarillo una caja de texto cuando obtiene el foco
function _OnFocus(txt) {
    txt.style.backgroundColor = "yellow";
}

// G.Sánchez (25 Julio 2018). Colocar fondo blanco una caja de texto cuando pierde el foco
function _OnLeave(txt) {
    txt.style.backgroundColor = "white";
}

//cgarcia 22-sep-2018  Remarcar en un color cuando un input obtiene el foco
function SetActiveInputStyle(input_selector, input_type) {
    //el input_selector, es la clase que deben de tener los inputs, para que aplique
    //el input type es para el tipo de input, en caso necesario se puede customizar el selector para un tipo en particular
    $(document).on('focus', input_selector, function (e) {
        e.stopImmediatePropagation();
        switch (input_type) {
            case 'text':
                $(this).addClass('active-input-focus-style');
                break;
            case 'chosen':
                $(this).find('a').addClass('active-input-focus-style');
                break;
            default:
                $(this).addClass('active-input-focus-style');
                break;
        }
    });
    $(document).on('blur', input_selector, function (e) {
        e.stopImmediatePropagation();
        switch (input_type) {
            case 'text':
                $(this).removeClass('active-input-focus-style');
                break;
            case 'chosen':
                $(this).find('a').removeClass('active-input-focus-style');
                break;
            default:
                $(this).removeClass('active-input-focus-style');
                break;
        }
    });
}

function CallWebMethodPOST(url, parameters) {

    return $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(parameters),
        contentType: 'application/json;',
        dataType: 'json'
    });
}

function RegisterPluginDataTable(pageSize) {
    $(".datatable").dataTable({
        "language": {
            "url": "/Base/DataTableLang"
        },
        "pageLength": pageSize,
        responsive: true
    });
}

//cgarcia 2-may-2019  Funcione generica para mostrar un confirm tipo delete
function SetConfirmBoxAction(AcceptFunction, DeleteMsg) {
    if (DeleteMsg !== '') {
        $('#title_ConfirmDelete_Common_Tag').val(decodeURI(DeleteMsg));
    }

    var confirmBox = new ConfirmBox();
    confirmBox.yesTag = $('#Yes_Common_Tag').val();
    confirmBox.noTag = $('#Cancel_Common_Tag').val();
    confirmBox.title = $('#Warning_Common_Tag').val();
    confirmBox.msg = $('#title_ConfirmDelete_Common_Tag').val();
    confirmBox.showMsg('warning');

    confirmBox.onAccept = AcceptFunction;
}

function SetAlertConfirmBoxAction(AcceptFunction, Message, Title) {
    if (Message !== '') {
        $('#title_ConfirmDelete_Common_Tag').val(decodeURI(Message));
    }

    var confirmBox = new ConfirmBox();
    confirmBox.yesTag = $('#Yes_Common_Tag').val();
    confirmBox.noTag = $('#Cancel_Common_Tag').val();
    confirmBox.title = Title; //$('#Warning_Common_Tag').val();
    confirmBox.msg = $('#title_ConfirmDelete_Common_Tag').val();
    confirmBox.showMsg('warning');

    $(".mb-container").css("background-color", "#cccc00");

    confirmBox.onAccept = AcceptFunction;
}

function SetAlertConfirmCustomBoxAction(AcceptFunction, Message, Title, MessageType) {
    if (Message !== '') {
        $('#title_ConfirmDelete_Common_Tag').val(decodeURI(Message));
    }

    var confirmBox = new ConfirmBox();
    confirmBox.yesTag = $('#Yes_Common_Tag').val();
    confirmBox.noTag = $('#Cancel_Common_Tag').val();
    confirmBox.title = Title;
    confirmBox.msg = $('#title_ConfirmDelete_Common_Tag').val();
    confirmBox.showMsg("warning");

    if (MessageType == "info") {
        $(".mb-container").css("background-color", "#2271b3");
        $('#confirmbx_yes').removeClass("btn-danger");
        $('#confirmbx_yes').addClass("btn-info");
    }
    if (MessageType == "Success") {
        $(".mb-container").css("background-color", "#95b75d");
        $('#confirmbx_yes').removeClass("btn-danger");
        $('#confirmbx_yes').addClass("btn-info");
    }

    confirmBox.onAccept = AcceptFunction;
}


/*Entity Data Info*/
ConfirmBox = function () {
    this.title = '';
    this.yesTag = '';
    this.noTag = '';
    this.msg = '';
};


ConfirmBox.prototype.showMsg = function (notificationType) {
    /*default style*/
    var confirmBox = new ConfirmBoxControl();
    confirmBox.init(this);
    confirmBox.id.attr("class", "message-box animated fadeIn");
    confirmBox.btnNo.attr("class", "btn btn-lg mb-control-yes");
    confirmBox.btnNo.attr("class", "btn btn-lg mb-control-close");
    confirmBox.id.addClass(confirmBox.notificationTypeClass(notificationType));
    confirmBox.faIcon.addClass(confirmBox.faIconCss(notificationType));
    confirmBox.btnNo.addClass(confirmBox.btnNoCss);
    confirmBox.btnYes.addClass(confirmBox.btnYesCss);
    confirmBox.id.addClass('open');
};

//Control 
ConfirmBoxControl = function () {
    this.id = $('#confirmbx_generic');
    this.title = $('#confirmbx_title');
    this.faIcon = $('#confirmbx_icon');
    this.msg = $('#confirmbx_msg');
    this.yesTag = $('#confirmbx_yesTag');
    this.noTag = $('#confirmbx_noTag');
    this.btnYes = $('#confirmbx_yes');
    this.btnNo = $('#confirmbx_no');
    this.btnNoCss = "btn-default";
    this.btnYesCss = "btn-danger";
    this.notificationTypeClass = function (notificationType) {
        var notifyClass = '';
        if (notificationType.toLowerCase() === 'success') {
            notifyClass = 'message-box-success';
        } else if (notificationType.toLowerCase() === 'warning') {
            notifyClass = 'message-box-danger';
        }
        return notifyClass;
    };
    this.faIconCss = function (notificationType) {
        var faIcon = '';
        if (notificationType.toLowerCase() === 'success') {
            notifyClass = 'fa fa-check';
        } else if (notificationType.toLowerCase() === 'warning') {
            notifyClass = 'fa fa-times';
        }
        return notifyClass;
    };

};

//Set Data
ConfirmBoxControl.prototype.init = function (confirmBoxData) {
    /*default style*/
    this.yesTag.text(confirmBoxData.yesTag);
    this.noTag.text(confirmBoxData.noTag);
    this.title.text(confirmBoxData.title);
    this.msg.text(confirmBoxData.msg);
    $('#confirmbx_yes').off();
    $('#confirmbx_yes').on('click', function (e) {
        e.stopImmediatePropagation();
        var confirmBox = new ConfirmBoxControl();
        confirmBox.id.removeClass('open');
        // TODO : Verificar si la funcion existe
        confirmBoxData.onAccept();
    });

};

function ShowProgressBar() {
    $('.loading-process-div').show();
    //$('#loading-process-div').css("display", "block");
}

function HideProgressBar() {
    $('.loading-process-div').hide();
    //$('#loading-process-div').css("display", "none");
}

function getFindControl(control, sender, deeprow) {
    var row = deeprow;
    var result = undefined;

    //Fetch all controls in GridView Row.
    var controls = row.getElementsByTagName("*");

    //Loop through the fetched controls.
    for (var i = 0; i < controls.length; i++) {

        //Find the TextBox Quantity control.
        if (controls[i].id.indexOf(control) !== -1) {
            result = controls[i];
            break;
        }
    }

    return result;
}

////////////////////////GRAFICAS GENERICAS///////////////////////////////
//lHernandez 5-sep-2019  Dibuja el html en el div dynamicCharts
function LoadGenericCharts(AreaValueID) {
    ShowProgressBar();
    $.get("/Administration/GenericCharts/GetChartInfoByArea", { AreaValueID }).done(function (data) {
        $("#dynamicCharts").html(data.View);
        $(".selectpicker").selectpicker();

        $(".dynamicChartDiv").each(function () {
            var panel = $(this);
            var GenericChartID = panel.data("genericchartid");
            var PanelID = panel.attr("id");
            //recorre los filtros para encontrar el valor defecto y dejarlo seleccionado, actualizar la grafica
            LoadGenericChartFilters(PanelID);

            //Esta funcion liga el evento para los cambios de filtros por grafica
            $(document).on("change", ".function_" + PanelID, function () {
                RefreshGenericChart(PanelID);
            });

        });
        //Esta funcion liga el evento click para el boton fullscren
        $(document).on("click", ".panel-fullscreen-genericchart", function () {
            var panel = $(this).parents(".panel");
            var panel_ratio = $(window).height() * 0.4 + "px";

            if (panel.hasClass("panel-fullscreened")) {//MINIMIZaR
                panel.find('.panel-title-box >h3').css("font-size", "100%");
                panel.find('.panel-title-box >span').css("font-size", "100%");

                panel.removeClass("panel-fullscreened").unwrap();
                panel.find(".panel-body").css("height", panel_ratio);
                panel.find(".chart-holder").css("height", panel_ratio);
                panel.find(".chart-holder").css("min-height", panel_ratio);
                panel.find(".panel-fullscreen .fa").removeClass("fa-compress").addClass("fa-expand");

                $(window).resize();

            } else {//MAXIMIZaR
                var full_screen_panel_ratio = 0.85; //30 normal

                panel.find(".panel-body,.chart-holder").height($(window).height() * full_screen_panel_ratio);

                panel.find('.panel-title-box >h3').css("font-size", "150%");
                panel.find('.panel-title-box >span').css("font-size", "120%");

                panel.addClass("panel-fullscreened").wrap('<div class="panel-fullscreen-wrap"></div>');
                panel.find(".panel-fullscreen .fa").removeClass("fa-expand").addClass("fa-compress");

                $(window).resize();

            }
            return false;
        });
        //Esta funcion liga el evento para el boton refresh del panel
        $(document).on("click", ".panel-refresh-genericchart", function () {
            var panel = $(this).parents(".panel");
            var PanelID = panel.attr("id");
            RefreshGenericChart(PanelID);
        });
        $(window).resize();
        HideProgressBar();
    });
}
//lHernandez 9-sep-2019 Funcion auxiliar para asignar el valor por defecto de los filtros que se guardo en la configuracion
function LoadGenericChartFilters(PanelID) {
    var panel = $("#" + PanelID);
    $("#" + PanelID).find(".dynamic_filter_chart").each(function () {
        var FilterID = $(this).data("valueid");
        var FilterName = $(this).attr("id");
        if (typeof FilterID !== "undefined") {
            //LoadFilterData(FilterID, FilterName, panel);
            var GenericChartID = panel.data("genericchartid");
            $.get("/Administration/GenericCharts/LoadDynamicFilterInfo", { FilterID, GenericChartID }).done(function (data) {
                $("#" + FilterName).empty();
                $.each(data.FilterElementsList, function () {
                    if (parseInt(data.DefaultValue) === $(this)[0].CatalogDetailID) {
                        $("#" + FilterName).append("<option selected value='" + $(this)[0].CatalogDetailID + "'>" + $(this)[0].DisplayText + "</option>");
                    } else {
                        $("#" + FilterName).append("<option value='" + $(this)[0].CatalogDetailID + "'>" + $(this)[0].DisplayText + "</option>");
                    }
                });
                $("#" + FilterName).selectpicker("refresh");
                RefreshGenericChart(PanelID);
            });
        }
    });
}

//cgarcia 19-sep-2019 funcion auxiliar para actualizar la info de la grafica en un panel, basado en los datos de los filtros
function RefreshGenericChart(PanelID) {
    var $panel = $("#" + PanelID);
    var GenericChartID = $panel.data("genericchartid");
    var FilterData = "";
    $(".function_" + PanelID).each(function () {
        if (typeof $(this).data("valueid") !== "undefined") {
            FilterData = FilterData + "," + $(this).data("valueid") + "=" + $(this).find("option:selected").val();
            var div_ID = "#" + $(this).closest(".dynamicChartDiv").attr("id");
            FilterData = FilterData.replace(",", "");
            $.get("/Administration/GenericCharts/GetFilteredData", { GenericChartID, FilterData }).done(function (data) {
                FormatGenericChartData(data.listData, data.listAxis, $panel);
            });
        }
    });
}

function FormatGenericChartData(DataList, AxisList, panel) {
    $("#_div_canvas_" + panel.attr("id")).empty();

    if (DataList.length > 0) {
        var MaxValue = 0;
        var xArray = "";
        var data = [];

        $(DataList).each(function () {
            xArray += "," + $(this)[0].Field1;
        });

        var ChartEntities = [];
        for (var i = 1; i <= AxisList.length - 1; i++) {
            var AxisData = AxisList[i];
            var ChartType = AxisList[i].DataChartTypeName.toLowerCase();
            var ColorData = AxisList[i].AxisColor;

            data = [];
            if (i == 1) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field2);
                }
            } else if (i == 2) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field3);
                }
            }
            else if (i == 3) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field4);
                }
            } else if (i == 4) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field5);
                }
            } else if (i == 5) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field6);
                }
            } else if (i == 6) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field7);
                }
            } else if (i == 7) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field8);
                }
            } else if (i == 8) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field9);
                }
            } else if (i == 9) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field10);
                }
            } else if (i == 10) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field11);
                }
            } else if (i == 11) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field12);
                }
            } else if (i == 12) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field13);
                }
            } else if (i == 13) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field14);
                }
            } else if (i == 14) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field15);
                }
            } else if (i == 15) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field16);
                }
            } else if (i == 16) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field17);
                }
            } else if (i == 17) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field18);
                }
            } else if (i == 18) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field19);
                }
            } else if (i == 19) {
                for (var i2 = 0; i2 <= DataList.length - 1; i2++) {
                    data.push(DataList[i2].Field20);
                }
            }

            var newmax = 0;
            newmax = Math.max.apply(null, data);
            //calculo de valor maximo de las series
            if (parseFloat(newmax) > parseFloat(MaxValue)) {
                MaxValue = newmax;
            }

            var ChartEntity = {
                label: AxisData.AxisName,
                yAxisID: null,
                type: null,
                borderColor: 0,
                backgroundColor: null,
                borderWidht: 0,
                data: data,
                Format: AxisData.AxisFormat,
                showLine: AxisData.ShowLine,
                labelrotation: AxisData.DataLabelRotation,
                labelshow: AxisData.DataLabelShow,
                labelfontsize: AxisData.DataLabelFontSize,
                labelfontcolor: AxisData.DataLabelFontColor,
                labelfontbgcolor: AxisData.DataLabelFontBGColor
            };

            if (ChartType === "stacked") {
                ChartType = "bar";
            }
            ChartEntity.type = ChartType;

            if (ChartType === "line") {
                ChartEntity.yAxisID = "LineAxis";
                ChartEntity.borderColor = ColorData;
                ChartEntity.backgroundColor = "transparent";
            } else {
                ChartEntity.yAxisID = "BarAxis";
                ChartEntity.backgroundColor = ColorData;
            }


            ChartEntities.push(ChartEntity);
        }
        console.log('maxvalue es :' + MaxValue);
        xArray = xArray.replace(",", "");
        GenerateGenericChart(xArray, ChartType, ChartEntities, panel, MaxValue);
    }
}

//lHernandez 5-sep-2019 Funcion auxiliar que llena las gráficas dinamicas con sus datos en el div dynamicCharts
function GenerateGenericChart(xArray, ChartType, ChartEntities, panel, MaxValue) {
    var Canvas = "canvas_" + panel.attr("id");
    var DivCanvas = "_div_canvas_" + panel.attr("id");
    var $panel = $("#" + panel.attr("id"));
    $panel.append('<div class="panel-refresh-layer"><img src="../Content/img/loaders/default.gif"/></div>');
    $panel.find(".panel-refresh-layer").width($panel.width()).height($panel.height());
    $panel.addClass("panel-refreshing");

    var xValues = xArray.split(",");
    var series = ChartEntities;

    var ChartyAxes = [
        {
            id: 'LineAxis',
            type: 'linear',
            position: 'right',
            ticks: {
                fontSize: 12,
                max: parseFloat(MaxValue) * 1.16,
                beginAtZero: true
            },
            display: false
        },
        {
            id: 'BarAxis',
            type: 'linear',
            position: 'left',
            ticks: {
                fontSize: 12,
                max: parseFloat(MaxValue) * 1.16,
                beginAtZero: true
            },
            display: true
        }];
    if (ChartType !== "line") {
        ChartyAxes = [
            {
                id: 'BarAxis',
                type: 'linear',
                position: 'left',
                ticks: {
                    fontSize: 12,
                    max: parseFloat(MaxValue) * 1.16,
                    beginAtZero: true
                },
                display: true
            }];
    }

    Chart.defaults.global.legend.labels.usePointStyle = true;

    $(DivCanvas).empty();
    document.getElementById(DivCanvas).innerHTML = '&nbsp;';
    document.getElementById(DivCanvas).innerHTML = '<canvas id="' + Canvas + '" style="position: relative;margin: auto;height: 65vh;width: 80vw;"></canvas>';

    var ctx = document.getElementById(Canvas).getContext('2d');
    var GenericChart = new Chart(ctx, {
        plugins: [ChartDataLabels],
        type: 'bar',
        data: {
            labels: xValues,
            datasets: series
        },
        options: {
            tooltips: {
                enabled: true,
                mode: 'single'
            },
            responsive: true,
            plugins: {
                // Change options for ALL labels of THIS CHART
                datalabels: {
                    display: function (ctx) {
                        if (ctx.dataset.labelshow && ctx.dataset.data[ctx.dataIndex] > 0) {
                            return 'auto';
                        }
                        return false;
                    },
                    backgroundColor: function (ctx) {
                        return ctx.dataset.labelfontbgcolor;
                    },
                    clamp: true,
                    borderRadius: 4,
                    color: function (ctx) {
                        return ctx.dataset.labelfontcolor;
                    },
                    font: function (ctx) {
                        var width = ctx.chart.width;
                        var size;
                        size = Math.round(width / ctx.dataset.labelfontsize);

                        return {
                            size: size,
                            weight: 'bold'
                        };
                    },
                    formatter: function (value, ctx) {
                        value = GetFormatedData(value, ctx.dataset.Format);
                        return value;
                    },
                    anchor: function (ctx) {
                        var id = ctx.chart.ctx.canvas.id;
                        if (id == "canvas_Savings") {
                            return 'responsive';
                        }
                        return 'center';
                    },
                    rotation: function (ctx) {
                        return ctx.dataset.labelrotation;
                    }
                }
            },
            scales: {
                yAxes: ChartyAxes,
                xAxes: [{ stacked: true }]
            },
            legend: {
                display: true,
                position: 'bottom',
                labels: {
                    fontSize: 12
                }
            },
            animation: {
                onComplete: function (animation) {
                    //ESTA SECCION ES LA QUE AJUSTA EL TAMAÑO DE LA GRÁFICA PARA QUE SE ADAPTE CON LA 
                    //RESOLUCION DE LA PANTALLA(SE EJECUTA CUANDO SE TERMINA DE DIBUJAR LA GRÁFICA)
                    $(Canvas).height($(DivCanvas).height());

                }
            }
        }
    });


    $panel.find(".panel-refresh-layer").remove();
    $panel.removeClass("panel-refreshing");
}


/*
 * Esta funcion se encarga de darle formato a los datos que se van a mostrar en los labels de las graficas dinamicas, 
 * está basado en lo que configuro el usuario en la seccion de Administration/GenericChartsAdministration/New
 * para los ejes y
*/
function GetFormatedData(value, FormatType) {
    switch (FormatType) {
        case "Money":
            value = "$" + value.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            break;
        case "Percentage":
            value = value.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "%";
            break;
        case "Numeric":
            value = value.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            break;
        default:
            value;
    }

    return value;
}
////////////////////////GRAFICAS GENERICAS///////////////////////////////