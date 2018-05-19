function layoutFormatter() {

    /* Layout Formatter */

    setTimeout(function () {

        var windowH = $(window).height();

        var headerH = $('#page-header').height();
        var navH = $('#page-nav').height();

        var sideH = windowH - headerH;
        var contH = windowH - headerH - navH;

        $('#page-sidebar').height(sideH);
        $('#page-sidebar-wrapper').height(sideH);
        $('#page-content').css('min-height', contH - 2);

    }, 499);

};

$(window).resize(function () {

    layoutFormatter();

});

/* Search input expander */
/*
$(function () {

    $('#sidebar-search input')
        .focus(function () {
            $(this).stop().animate({width: 200}, 'slow');
        })
        .blur(function () {
            $(this).stop().animate({width: 100}, 'slow');
        });

});*/

$(document).on('ready', function() {

    $("#page-sidebar-wrapper").niceScroll({
        horizrailenabled: false,
        cursorborder: "0",
        cursorwidth: "6px",
        cursorcolor: "#000000",
        zindex: "5555",
        autohidemode: true,
        bouncescroll: true,
        mousescrollstep: '40',
        scrollspeed: '100',
        background: "#2A2A2A",
        cursoropacitymax: "0.6",
        cursorborderradius: "0"
    });

    $("#page-sidebar-wrapper").getNiceScroll().resize();

});

$(document).ready(function () {

    layoutFormatter();

    /* Open responsive nav menu */

    $(function () {

        $('#responsive-open-menu').click(function () {
            $('#page-sidebar').toggle();
        });

    });

    $(function () {

        $('#sidebar-menu > ul').superclick({
            animation: {height: 'show'},
            animationOut: {height: 'hide'}
        });

    });

    $(function () {

        $('.main-nav > ul').superclick({
            animation: {height: 'show'},
            animationOut: {height: 'hide'}
        });

    });

    /* Colapse sidebar */

    $(function () {

        $('#collapse-sidebar').click(function () {
            $('#page-sidebar, #page-content-wrapper, #header-logo').removeClass('rm-transition');
            $('body').toggleClass('sidebar-collapsed');
            $('.glyph-icon', this).toggleClass('icon-chevron-right').toggleClass('icon-chevron-left');

        });

    });

});

function init_page_transitions() {

    var transitions = ['.pt-page-moveFromLeft', 'pt-page-moveFromRight', 'pt-page-moveFromTop', 'pt-page-moveFromBottom', 'pt-page-fade', 'pt-page-moveFromLeftFade', 'pt-page-moveFromRightFade', 'pt-page-moveFromTopFade', 'pt-page-moveFromBottomFade', 'pt-page-scaleUp', 'pt-page-scaleUpCenter', 'pt-page-flipInLeft', 'pt-page-flipInRight', 'pt-page-flipInBottom', 'pt-page-flipInTop', 'pt-page-rotatePullRight', 'pt-page-rotatePullLeft', 'pt-page-rotatePullTop', 'pt-page-rotatePullBottom', 'pt-page-rotateUnfoldLeft', 'pt-page-rotateUnfoldRight', 'pt-page-rotateUnfoldTop', 'pt-page-rotateUnfoldBottom'];
    for (var i in transitions) {
        var transition_name = transitions[i];
        if ($('.add-transition').hasClass(transition_name)) {

            $('.add-transition').addClass(transition_name + '-init page-transition');

            setTimeout(function () {
                $('.add-transition').removeClass(transition_name + ' ' + transition_name + '-init page-transition');
            }, 1200);
            return;
        }
    }

};

$(document).ready(function () {
    init_page_transitions();
});
