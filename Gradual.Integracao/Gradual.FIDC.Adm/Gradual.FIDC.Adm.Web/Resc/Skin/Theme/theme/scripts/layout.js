$(function()
{
    /* Layout Options */
    var layout = $.cookie('layout') ? $.cookie('layout') : 'fixed';
    
    /*
    if (layout == 'fixed' && !$('.container-fluid:first').is('.fixed'))
        $('.container-fluid:first').addClass('fixed');
    
    if (layout == 'fluid' && $('.container-fluid:first').is('.fixed'))
        $('.container-fluid:first').removeClass('fixed');

    $('#footer [data-toggle="layout"]').not('[data-layout="'+layout+'"]').parent().removeClass('active');
    $('#footer [data-toggle="layout"][data-layout="'+layout+'"]').parent().addClass('active');
    
    $('#footer [data-toggle="layout"]').click(function()
    {
        if ($(this).parent().is('.active'))
            return;
        
        $('#footer [data-toggle="layout"]').not(this).parent().removeClass('active');
        $(this).parent().addClass('active');
        
        if ($(this).attr('data-layout') == 'fixed')
            $('.container-fluid:first').addClass('fixed');
        else
            $('.container-fluid:first').removeClass('fixed');
            
        $.cookie('layout', $(this).attr('data-layout'));
        
        if (typeof masonryGallery != 'undefined') 
            masonryGallery();
            
    });
    
    */


    /* Menu Options */
    var menuPosition = $.cookie('menuPosition') ? $.cookie('menuPosition') : 'left-menu';
    
    if (menuPosition == 'right-menu' && !$('.container-fluid:first').is('.right-menu'))
        $('.container-fluid:first').addClass('right-menu');
    
    if (menuPosition == 'right-menu' && $('.container-fluid:first').is('.left-menu'))
        $('.container-fluid:first').removeClass('left-menu');
    
    $('#footer [data-toggle="menuPosition"]').not('[data-menuPosition="'+menuPosition+'"]').parent().removeClass('active');
    $('#footer [data-toggle="menuPosition"][data-menuPosition="'+menuPosition+'"]').parent().addClass('active');
    
    $('#footer [data-toggle="menuPosition"]').click(function()
    {
        if ($(this).parent().is('.active'))
            return;
        
        $('#footer [data-toggle="menuPosition"]').not(this).parent().removeClass('active');
        $(this).parent().addClass('active');
        
        if ($(this).attr('data-menuPosition') == 'right-menu')
            $('.container-fluid:first').addClass('right-menu');
        
        if ($(this).attr('data-menuPosition') == 'right-menu' && $('.container-fluid:first').is('.left-menu'))
            $('.container-fluid:first').removeClass('left-menu');
        
        if ($(this).attr('data-menuPosition') == 'left-menu' && $('.container-fluid:first').is('.right-menu'))
            $('.container-fluid:first').removeClass('right-menu');
            
        $.cookie('menuPosition', $(this).attr('data-menuPosition'));
    });
});