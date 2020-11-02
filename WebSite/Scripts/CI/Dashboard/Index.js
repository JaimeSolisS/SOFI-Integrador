// =============================================================================================================================
//  Version: 20190914
//  Author:  Guillermo Sánchez
//  Created Date: Viernes 15 de Marzo de 2019
//  Description: Contiene funciones JS para la página de Index del Dashboard de mejora continua
//  Modifications: 
//  F.Vera 03 Junio 2019. Agregar integración para carousel de videos
//  G.Sánchez (14 Sept 2019). Integrar manejo de videos de Screen Saver con los del carrusel de videos
//  
// =============================================================================================================================

function IndexInit(Resources) {

    //set el slider con previews de videos
    function SetCarouselVideoNav() {
        //para cada video cargado de crea una img del seg 5, para usar como preview
        $('.carousel-video').find('video').each(function (i) {
            var $vid = $(this);
            var video = $vid.get(0);
            video.currentTime = 5;
            video.onloadeddata = function () {
                //Captura de la img del video
                var canvas = document.createElement("canvas");
                canvas.width = 200;
                canvas.height = 125;
                canvas.getContext('2d').drawImage(video, 0, 0, canvas.width, canvas.height);
                //actualizacion del src del img
                var img = document.getElementById("imgpreview_" + $vid.attr("id"));
                img.src = canvas.toDataURL();
                //console.log("preview created for video " + i);
                //se actualiza el carusel, debido a la llamada async
                $('.carousel-video-nav').slick('refresh');
                video.currentTime = 0;

            };
        });
  
        //plugin de carousel con los preview
        $('.carousel-video-nav').slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            asNavFor: '.carousel-video',
            dots: true,
            centerMode: true,
            focusOnSelect: true
        });
    }

    var mousetimeout;
    var screensaver_active = false;
    var idletime = 5;

    var fn_CloseWindowAfter;
    var closeWindowAfterTimer_Active = false;
    var closewindow_afterTimer = 0;

    function openFullscreen(elem) {
        if (elem != undefined) {
            //elem.webkitRequestFullscreen();
            //if (elem.requestFullscreen) {
            //    elem.requestFullscreen();
            //} else
            if (elem.mozRequestFullScreen) { /* Firefox */
                elem.mozRequestFullScreen();
            } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
                elem.webkitRequestFullscreen();
            } else if (elem.msRequestFullscreen) { /* IE/Edge */
                elem.msRequestFullscreen();
            }
        }
    }

    idletime = Resources.ScreenSaverInterval;
    closewindow_afterTimer = Resources.CloseWindowAfterIterval;

    $('#v_screensaver').get(0).load();

    $('#mo_Welcome').modal({
        backdrop: 'static',
        keyboard: false
    });

    $('body').on('mousemove', function () {
        set_DashboardTimers();
        $('#if_link').css('pointer-events', 'auto');
        $('#pdf_file').css('pointer-events', 'auto');
    });

    $('body').on('touchmove', function () {
        set_DashboardTimers();
        $('#if_link').css('pointer-events', 'auto');
        $('#pdf_file').css('pointer-events', 'auto');
    });

    //desactiva cursor, mouse over en iframe, cada 1/2seg, para que funcione set_DashboardTimers
    setInterval(function () {
        //console.log('desactiva cursor del iframe...');
        $('#if_link').css('pointer-events', 'none');
        //console.log('desactiva cursor del pdf...');
        $('#pdf_file').css('pointer-events', 'none');
    }, 20000);

    // Cerrar panel de detalle
    $('.containerpanel-remove').on('click', function () {
        CleanControlsFullDetail();
        PreviewDocument(false);

        closeWindowAfterTimer_Active = false;
        clearTimeout(fn_CloseWindowAfter);
        fn_CloseWindowAfter = null;

        $('#pnl_Detail').addClass('hidden');
        $('#a_fullscreen').click();

        registerPluginCarouselVideo();
        // computePostionDashboardVideo();
        start_CarouselVideo();
    });

    $(document).on('click', '.CI_PDFFile', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var sourcepath = $(this).data('sourcepath');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        ShowFullDetail(Name, Footer);

        $("#pdf_file").attr("src", sourcepath);
        $("#pdf_file").removeClass('hidden');
        //desactiva cursor, mouse over de inicio 
        $('#pdf_file').css('pointer-events', 'none');
    });

    $(document).on('click', '.CI_Link', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var href = $(this).data('href');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        ShowFullDetail(Name, Footer);

        $("#div_link").html('<iframe is="x-frame-bypass" src="' + href + '" style="width:100%;height:100%;" />');

        $("#div_link").removeClass('hidden');
        //desactiva cursor, mouse over de inicio 
        $('#if_link').css('pointer-events', 'none');
    });

    $(document).on('click', '.CI_Video', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var sourcepath = $(this).data('sourcepath');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        ShowFullDetail(Name, Footer);
        SetVideo(sourcepath);

    });

    $(document).on('click', '.CI_Image', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var sourcepath = $(this).data('sourcepath');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        ShowFullDetail(Name, Footer);

        $("#i_detail").attr("src", sourcepath);
        $("#i_detail").removeClass('hidden');
    });

    $(document).on('click', '.CI_Gallery', function (e) {
        e.stopImmediatePropagation();

        var FilterID = $(this).data('filterid');
        var Name = $(this).data('name');
        var Footer = $(this).data('footer');
        var sourcepath = $(this).data('sourcepath');

        if (FilterID > 0) {
            ResetDashboardAreas(Name, FilterID);
            return;
        }

        // Mostrar area de detalle
        ShowFullDetail(Name, Footer);

        // TODO: Colocar todo el contenido dinamico
        //$('#div_Gallery').html('');
        $("#div_Gallery").removeClass('hidden');
        SetGallerySettings(sourcepath);
    });

    $(document).on('click', '#btn_PlayScreenSaverVideo', function (e) {
        e.stopImmediatePropagation();
        $('#v_screensaver').get(0).play();
    });

    $('#welcome-close').on('click', function () {
        var elem = document.documentElement;
        openFullscreen(elem);
        registerPluginCarouselVideo();
        computePostionDashboardVideo();
    });

    $('#refresh-dashboard').on('click', function () {
        location.reload();
    });

    var carouselvideo = $('.carousel-video');
    carouselvideo.slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        autoplay: true,
        lazyLoad: 'ondemand',
        autoplaySpeed: 3000,
        fade: true,
        asNavFor: '.carousel-video-nav'
    });

    SetCarouselVideoNav();

    function set_DashboardTimers() {
        //console.log('set_DashboardTimers....');
        clearTimeout(mousetimeout);
        mousetimeout = null;

        if (screensaver_active) {
            stop_screensaver();
        }

        clearTimeout();//reset timers
        mousetimeout = setTimeout(function () {
            show_screensaver();
        }, 1000 * idletime); // 5 secs	

        try {
            //console.log('eliminado:' + fn_CloseWindowAfter);
            clearTimeout(fn_CloseWindowAfter);
            fn_CloseWindowAfter = null;

            // Mapear funcion que cerrara ventana maximizada en caso que este abierta
            if (closeWindowAfterTimer_Active) {
                fn_CloseWindowAfter = setTimeout(function () {
                    closeWindowAfterTimer_Active = false;
                    $('.containerpanel-remove').click();
                    //console.log('fin: ' + fn_CloseWindowAfter);
                }, 1000 * closewindow_afterTimer); // 5 secs
                //console.log('generado:' + fn_CloseWindowAfter);
            }

        } catch (err) {
            console.log(err.message);
        }
    }

    function registerPluginCarouselVideo() {
        // Slider Dashboard Video
        // ref : https://stackoverflow.com/questions/31521763/slick-js-and-html5-video-autoplay-and-pause-on-video
        var carouselvideo = $('.carousel-video');
        carouselvideo.on('init', function (slick) {
            //var selector = '#slideVideo' + (currentSlide + 1);
            carouselvideo.data('slide', 1);
            carouselvideo.slick('slickPause');
            //$(selector).get(0).play();
            //$(selector).bind("ended", function () {
            //    carouselvideo.slick('slickPlay');

            //});
        });

        carouselvideo.on('afterChange', function (event, slick, currentSlide) {
            // G.Sánchez (14 Sept 2019). Registar el numero de video actual que esta corriendo
            //$('#hd_CurrentSlideVideSeq').val(currentSlide + 1);

            var i = currentSlide + 1;
            var selector = '#slideVideo' + i;
            //console.log(selector);
            carouselvideo.slick('slickPause');
            carouselvideo.data('slide', i);

            $(selector).get(0).play();
            //openFullscreen(document.getElementById('slideVideo' + i));
            $(selector).off();
            $(selector).bind("ended", function () {
                carouselvideo.slick('slickPlay');
                //this.webkitExitFullscreen(); // Deprecated
                //if (document.exitFullscreen) {
                //    document.exitFullscreen(); // Standard
                //} else if (document.webkitExitFullscreen) {
                //    document.webkitExitFullscreen(); // Blink
                //} else if (document.mozCancelFullScreen) {
                //    document.mozCancelFullScreen(); // Gecko
                //} else if (document.msExitFullscreen) {
                //    document.msExitFullscreen(); // Old IE
                //}
            });
        });

        carouselvideo.on('beforeChange', function (slick, currentSlide, nextSlide) {
            var i = nextSlide + 1;
            var selector = '#slideVideo' + i;
            $(selector).get(0).pause();
            $(selector).off();
        });
    }

    function computePostionDashboardVideo() {
        var div_boxPanelRight = $('#div_boxPanelRight');
        var div_pluginClock = $('#div_pluginClock');
        var carouselvideo = $('.carousel-video');
        var carosuelVideoContainer = $('#div_boxCarouselVideoContainer');

        var videoHeight = carosuelVideoContainer.height(); // Video Height
        var mainPanelHeight = div_boxPanelRight.height(); // 100%
        var div_pluginClockHeight = div_pluginClock.height();
        var h = parseInt(mainPanelHeight - videoHeight - div_pluginClockHeight);
        var result = Math.abs(parseInt(h / 2));
        //carosuelVideoContainer.css({ "margin-top": result.toString() + "px" });
    }

    function CleanControlsFullDetail() {
        $("#pdf_file").addClass('hidden');
        $("#pdf_file").attr("src", '');
        $("#div_link").addClass('hidden');
        $("#div_link").html('');
        $("#div_video").addClass('hidden');
        $('#v_video').get(0).pause();
        $("#i_detail").addClass('hidden');
        $("#i_detail").attr("src", '');
        //$('#div_Gallery').html('');
        $("#div_Gallery").addClass('hidden');
        try {
            $('.SliderFade').slick('unslick');
        }
        catch (e) {
            console.log('No sick available');
        }
    }

    function ShowFullDetail(Name, Footer) {
        // Ocultar todos los controles
        CleanControlsFullDetail();
        PreviewDocument(true);

        // Colocar los valores
        $('#h3_Name').text(Name);
        $('#s_Footer').text(Footer);
        $('#pnl_Detail').removeClass('hidden');
        $('#a_fullscreen').click();
        stop_CarouselVideo();
        closeWindowAfterTimer_Active = true;

        CounterAdd(); // Anexar contador de visitas
    }

    function SetVideo(sourcepath) {
        var videoID = 'v_video';
        var sourceID = 's_video';
        var newposter = 'media/video-poster2.jpg';

        $('#' + videoID).get(0).pause();
        $('#' + sourceID).attr('src', sourcepath);

        $('#' + videoID).get(0).load();
        //$('#'+videoID).attr('poster', newposter); //Change video poster
        $('#' + videoID).get(0).play();

        $("#div_video").removeClass('hidden');
    }

    function SetGallerySettings(sourcepath) {

        $.get("/CI/Dashboard/GetGalleryDetail",
            {
                SourcePath: sourcepath
            })
            .done(function (data) {
                $(".SliderFade").html(data);

                $(".SliderFade").slick({
                    centerMode: true,
                    centerPadding: '60px',
                    slidesToShow: 1,
                    responsive: [
                        {
                            breakpoint: 768,
                            settings: {
                                arrows: false,
                                centerMode: true,
                                centerPadding: '80px',
                                slidesToShow: 1
                            }
                        },
                        {
                            breakpoint: 480,
                            settings: {
                                arrows: false,
                                centerMode: true,
                                centerPadding: '80px',
                                slidesToShow: 1
                            }
                        }
                    ]
                });

                //$('.SliderFade').slick({
                //    autoplay: false,
                //    pauseOnFocus: true,
                //    pauseOnHover: true,
                //    autoplaySpeed: 500,
                //    dots: false,
                //    //prevArrow: true,
                //    //nextArrow: true,
                //    slidesToShow: 1,
                //    //slidesToScroll: 3,
                //    infinite: true,
                //    speed: 500,
                //    fade: true,
                //    draggable:true,
                //    useTransform: false,
                //    useCSS: false,
                //    cssEase: 'linear'
                //});
                $('.image-full-detail').height($('#body_pnl_Detail').height());
            });
    }

    function CounterAdd() {
        $.post("/CI/Dashboard/CounterAdd", {})
            .done(function (data) {
                var lbl_Visits = $('#lbl_Visits').html();
                $('#s_TotalVisits').html(data.TotalVisits + ' ' + lbl_Visits);
            });
    }

    // #region SCREENSAVER
    function start_CloseWindowAfter() {

    }

    function show_screensaver() {
        $('#div_screensavervideo').fadeIn();
        screensaver_active = true;
        PreviewDocument(true);
        $('#div_screensavervideo').removeClass('hidden');
        //openFullscreenVideo();
        $("#v_screensaver").show();
        $("#v_screensaver").css('position', 'absolute')
            .css('height', '100%').css('width', '100%')
            .css('margin', 0).css('margin-top', '0%')
            .css('top', '0').css('left', '0')
            .css('float', 'left').css('z-index', 600)
            .css('background', 'black');
        $('#v_screensaver').show();

        // G.Sánchez (14 Sept 2019). Usar videos del carrusel no uno fijo (El carrusel tiene indice 0)
        //$('#v_screensaver').get(0).play(); 
        var seq = carouselvideo.slick('slickCurrentSlide') + 1; //$('#hd_CurrentSlideVideSeq').val();
        StartScreenSaverVideo(seq);

        stop_CarouselVideo();
    }

    function stop_screensaver() {
        $('#v_screensaver').get(0).pause();
        $('#v_screensaver').hide();
        $('#div_screensavervideo').fadeOut();
        screensaver_active = false;
        PreviewDocument(false);
        $('#div_screensavervideo').addClass('hidden');
        start_CarouselVideo();
        //$(".BackImage").css("background-image", "url(http://localhost:58056//Files/CI/Dashboard/BackgroundImages/white-wallpaper.jpg)");
    }

    function getRandomColor() {
        var letters = '0123456789ABCDEF'.split('');
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[Math.round(Math.random() * 15)];
        }
        return color;
    }

    function screensaver_animation() {
        if (screensaver_active) {
            $('#screensaver').animate(
                { backgroundColor: getRandomColor() },
                400,
                screensaver_animation);
        }
    }

    function PreviewDocument(IsPreview) {
        var div_boxPanelRight = $('#div_boxPanelRight');
        var div_boxPanelLeft = $('#div_boxPanelLeft');

        if (IsPreview) {
            div_boxPanelLeft.removeClass("split");
            div_boxPanelLeft.removeClass("left");

            div_boxPanelRight.removeClass("split");
            div_boxPanelRight.removeClass("right");
            div_boxPanelRight.addClass("hidden");
        } else {
            div_boxPanelLeft.addClass("split");
            div_boxPanelLeft.addClass("left");

            div_boxPanelRight.addClass("split");
            div_boxPanelRight.addClass("right");
            div_boxPanelRight.removeClass("hidden");

        }
    }
    // #endregion
}

function NextScreenVideo() {
    var carouselvideo = $('.carousel-video');

    // El carrusel inicia en indice 0
    var seq = carouselvideo.slick('slickCurrentSlide') + 2; //parseInt($('#hd_CurrentSlideVideSeq').val()) + 1;
    StartScreenSaverVideo(seq);

    // Mover tambien el carrusel al siguiente video y pausarlo (al establecer el siguiente se hacce play solo)
    carouselvideo.slick('slickNext');
    setTimeout(function () { stop_CarouselVideo(); }, 500);
}

function StartScreenSaverVideo(seq) {
    $.post("/CI/Dashboard/GetPathVideo", {
        Seq: seq
    })
        .done(function (data) {
            //$('#hd_CurrentSlideVideSeq').val(seq);
            var sourcepath = data.VideoPath;
            var videoID = 'v_screensaver';
            var sourceID = 'sv_screensaver';

            $('#' + videoID).get(0).pause();
            $('#' + sourceID).attr('src', sourcepath);

            $('#' + videoID).get(0).load();
            $('#' + videoID).get(0).play();
        });
}

function start_CarouselVideo() {
    var carouselvideo = $('.carousel-video');
    carouselvideo.slick('slickPlay');
}

function stop_CarouselVideo() {
    var carouselvideo = $('.carousel-video');
    var selector = '#slideVideo' + carouselvideo.data('slide');
    $(selector).get(0).pause();
    carouselvideo.slick('slickPause');
}

function ResetDashboardAreas(Name, ParentID) {
    // Obtener el filtro.
    // Colocar el nuevo filtro
    var title = $('#s_filterText').text();
    if (title.length > 0 && Name.length > 0) {
        $('#s_filterText').text(title + ' -> ' + Name);
    }
    else {
        $('#s_filterText').text(Name);
    }

    // Mostrar u ocultar div de alert
    if (ParentID !== 0) {
        $('#div_FilterAlert').removeClass('hidden');
    }
    else {
        $('#div_FilterAlert').addClass('hidden');
    }

    // Recargar zona de Dashboard Areas
    $.get("/CI/Dashboard/GetAreas"
        , { ParentID: ParentID })
        .done(function (data) {
            $('#div_DashboardAreas').html('');
            $('#div_DashboardAreas').html(data);
        });
}

function CloseFilterAlert() {
    ResetDashboardAreas('', 0);
    return false;
}