$( function() {

	var $container = $('.isotope').isotope({
		itemSelector: '.item',
		layoutMode: 'masonry',
		masonry: {
		  columnWidth: '.col-lg-2'
		},
	});

	$('#filters').on( 'click', 'a', function( event ) {
		var filterValue = $(this).attr('data-filter-value');

		$('#filters a').removeClass('active');

		$(this).addClass('active');

		$container.isotope({ filter: filterValue });
	});
  
});

$(function(){

    $('#portfolio-grid').mixitup({
    	showOnLoad: 'hover_2'
    });

    $('.controls .filter').click(function(){
        var portfolio_title = $('a', this).html();
        $('.portfolio-category').html(portfolio_title);
    });

});