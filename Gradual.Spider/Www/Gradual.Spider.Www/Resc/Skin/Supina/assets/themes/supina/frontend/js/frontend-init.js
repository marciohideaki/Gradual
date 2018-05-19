new WOW().init();

jQuery(document).ready(function($) {

    /* Sticky bars */

    $(function() {

        $('.sticky-nav').hcSticky({
            top: 50
        });

    });

    /* Main nav */

    $(function() {

      $('.main-nav').superfish({
          animation:     {opacity:'show'},
          animationOut:  {opacity:'hide'},
      });

    });

    /* Responsive nav */

    $(function() {

        $('#responsive-menu').click(function(){
            $('.main-header ul.main-nav').toggle();
        });

    });

});