
  /* jQuery UI Datepicker */

  $(function() {
    $( "#fromDate" ).datepicker({
      defaultDate: "+1w",
      changeMonth: true,
      numberOfMonths: 3,
      onClose: function( selectedDate ) {
        $( "#toDate" ).datepicker( "option", "minDate", selectedDate );
      }
    });
    $( "#toDate" ).datepicker({
      defaultDate: "+1w",
      changeMonth: true,
      numberOfMonths: 3,
      onClose: function( selectedDate ) {
        $( "#fromDate" ).datepicker( "option", "maxDate", selectedDate );
      }
    });
  });

  $(function() {

      $( "#datepicker_multiple_months" ).datepicker({
        numberOfMonths: 3,
        showButtonPanel: true
      });

  });

  $(function() {
      $( ".datepicker" ).datepicker();
  });