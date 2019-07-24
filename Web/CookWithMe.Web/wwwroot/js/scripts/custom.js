/* ----------------- Start Document ----------------- */
(function($){
    "use strict";

    $(document).ready(function(){



  /*----------------------------------------------------*/
  /*  Navigation
  /*----------------------------------------------------*/

    function menumobile(){
        var winWidth = $(window).width();
        if( winWidth < 973 ) {
            $('#navigation').removeClass('menu');
            $('#navigation li').removeClass('dropdown');
            $('#navigation').superfish('destroy');
        } else {
            $('#navigation').addClass('menu');
            $('#navigation').superfish({
                delay:       300,                               // one second delay on mouseout
                animation:   {opacity:'show', height:'show'},   // fade-in and slide-down animation
                speed:       200,                               // animation speed
                speedOut:    50                                 // out animation speed
            });
        }
    }

    $(window).resize(function (){
        menumobile();
    });
    menumobile();


  /*----------------------------------------------------*/
  /*  Royal Slider
  /*----------------------------------------------------*/

    // Homepage Slider

    var rsi = $('#homeSlider').royalSlider({

        controlNavigation:'thumbnails',
        imageScaleMode: 'fill',
        arrowsNav: false,
        arrowsNavHideOnTouch: true,
        slidesSpacing: 0,

        fullscreen: false,
        loop: false,

        thumbs: {
          firstMargin: false,
          paddingBottom: 0,
          spacing: 0
      },

      numImagesToPreload: 3,
      thumbsFirstMargin: false,

      keyboardNavEnabled: true,
      navigateByClick: false,
      fadeinLoadedSlide: true,
      transitionType: 'fade',

      block: {
        fadeEffect: true,
        moveEffect: 'top',
        delay: 0
    }

    }).data('royalSlider');
    $('#slider-next').click(function() {
        rsi.next();
    });
    $('#slider-prev').click(function() {
        rsi.prev();
    });


    // Recipe Slider
    $(".recipeSlider").royalSlider({

        imageScalePadding: 0,
        keyboardNavEnabled: true,

        navigateByClick: false,
        fadeinLoadedSlide: true,
        arrowsNavAutoHide: false,

        imageScaleMode: 'fill',
        arrowsNav: true,
        slidesSpacing: 0,

    });


    // Product Slider
    $(".productSlider").royalSlider({

      autoScaleSlider: true,
      autoHeight: false,
      autoScaleSliderWidth: 580,
      autoScaleSliderHeight: 500,

      loop: false,
      slidesSpacing: 0,

      imageScaleMode: 'fill',
      imageAlignCenter:false,

      navigateByClick: false,
      numImagesToPreload:2,

    });


    // Alternative Homepage Slider
    $('#homeSliderAlt').royalSlider({
      arrowsNav: false,
      fadeinLoadedSlide: true,
      controlNavigationSpacing: 0,
      controlNavigation: 'thumbnails',

      thumbs: {
        autoCenter: false,
        fitInViewport: true,
        orientation: 'vertical',
        spacing: 0,
        paddingBottom: 0
      },
      keyboardNavEnabled: true,
      imageScaleMode: 'fill',
      imageAlignCenter:true,
      slidesSpacing: 0,
      loop: false,
      loopRewind: true,
      numImagesToPreload: 3,
      autoScaleSlider: true,
      autoScaleSliderWidth: 1180,
      autoScaleSliderHeight: 500,


    });



    /*----------------------------------------------------*/
    /*  Image Height Fix
    /*----------------------------------------------------*/
    function fixliststylebox(){
        var winWidth = $(window).width();
        if(winWidth > 973) {
           $( ".list-style .recipe-box-content" ).each(function() {
            var recipeHeight = $(this).outerHeight();
            $(this).parent().find('.thumbnail-holder').height(recipeHeight);
        });
       } else {
        $('.thumbnail-holder').css({
            height: 'auto'
        });
       }
    }


    $(window).load(function(){
        fixliststylebox();

    });
    $(window).resize(function (){
        fixliststylebox();

    });



    /*----------------------------------------------------*/
    /*  Tooltips
    /*----------------------------------------------------*/
    $(".tooltip.top").tipTip({
          defaultPosition: "top"
    });

    $(".tooltip.bottom").tipTip({
          defaultPosition: "bottom"
     });

     $(".tooltip.left").tipTip({
           defaultPosition: "left"
     });

    $(".tooltip.right").tipTip({
          defaultPosition: "right"
    });



  /*----------------------------------------------------*/
  /*  Product Quantity
  /*----------------------------------------------------*/
    var thisrowfield;
    $('.qtyplus').click(function(e){
      e.preventDefault();
      thisrowfield = $(this).parent().parent().parent().find('.qty');

      var currentVal = parseInt(thisrowfield.val());
      if (!isNaN(currentVal)) {
        thisrowfield.val(currentVal + 1);
      } else {
        thisrowfield.val(0);
      }
    });

    $(".qtyminus").click(function(e) {
      e.preventDefault();
      thisrowfield = $(this).parent().parent().parent().find('.qty');
      var currentVal = parseInt(thisrowfield.val());
      if (!isNaN(currentVal) && currentVal > 0) {
        thisrowfield.val(currentVal - 1);
      } else {
        thisrowfield.val(0);
      }
    });



  /*----------------------------------------------------*/
  /*  Mobile Navigation
  /*----------------------------------------------------*/

  var navigation = responsiveNav(".nav-collapse", {
        animate: true,                    // Boolean: Use CSS3 transitions, true or false
        transition: 284,                  // Integer: Speed of the transition, in milliseconds
        label: "Menu",                    // String: Label for the navigation toggle
        insert: "before",                 // String: Insert the toggle before or after the navigation
        customToggle: "",                 // Selector: Specify the ID of a custom toggle
        closeOnNavClick: false,           // Boolean: Close the navigation when one of the links are clicked
        openPos: "relative",              // String: Position of the opened nav, relative or static
        navClass: "nav-collapse",         // String: Default CSS class. If changed, you need to edit the CSS too!
        navActiveClass: "js-nav-active",  // String: Class that is added to <html> element when nav is active
        jsClass: "js",                    // String: 'JS enabled' class which is added to <html> element
        init: function(){},               // Function: Init callback
        open: function(){},               // Function: Open callback
        close: function(){}               // Function: Close callback
    });



    /*----------------------------------------------------*/
    /*  Isotope
    /*----------------------------------------------------*/

    $(window).load(function(){
        var $container = $('.isotope');
        $container.isotope({ itemSelector: '.recipe-box, .isotope-box', layoutMode: 'masonry' });
    });



    /*----------------------------------------------------*/
    /*  Contact Form
    /*----------------------------------------------------*/
    $("#contactform .submit").click(function(e) {


      e.preventDefault();
      var user_name       = $('input[name=name]').val();
      var user_email      = $('input[name=email]').val();
      var user_comment    = $('textarea[name=comment]').val();

      //simple validation at client's end
      //we simply change border color to red if empty field using .css()
      var proceed = true;
      if(user_name===""){
          $('input[name=name]').addClass('error');
            proceed = false;
          }
          if(user_email===""){
            $('input[name=email]').addClass('error');
            proceed = false;
          }
          if(user_comment==="") {
            $('textarea[name=comment]').addClass('error');
            proceed = false;
          }

          //everything looks good! proceed...
          if(proceed) {
            $('.hide').fadeIn();
            $("#contactform .submit").fadeOut();
              //data to be sent to server
              var post_data = {'userName':user_name, 'userEmail':user_email, 'userComment':user_comment};

              //Ajax post data to server
              $.post('contact.php', post_data, function(response){
                var output;
                //load json data from server and output comment
                if(response.type == 'error')
                  {
                    output = '<div class="error">'+response.text+'</div>';
                    $('.hide').fadeOut();
                    $("#contactform .submit").fadeIn();
                  } else {

                    output = '<div class="success">'+response.text+'</div>';
                    //reset values in all input fields
                    $('#contact div input').val('');
                    $('#contact textarea').val('');
                    $('.hide').fadeOut();
                    $("#contactform .submit").fadeIn().attr("disabled", "disabled").css({'backgroundColor':'#c0c0c0', 'cursor': 'default' });
                  }

                  $("#result").hide().html(output).slideDown();
                }, 'json');
            }
      });

    //reset previously set border colors and hide all comment on .keyup()
    $("#contactform input, #contactform textarea").keyup(function() {
      $("#contactform input, #contactform textarea").removeClass('error');
      $("#result").slideUp();
    });



    /*----------------------------------------------------*/
    /*  Advanced Search
    /*----------------------------------------------------*/

    $('.adv-search-btn').click(function(event) {
        var slideDuration = 200;

        if($(this).hasClass('opened')){
            $('.adv-search-btn').removeClass('active');
           $(this).removeClass('opened').find('i').addClass('fa-caret-down').removeClass('fa-caret-up');
           $('.extra-search-options').stop(true, true).fadeOut({ duration: slideDuration, queue: false }).slideUp(slideDuration);
        } else {
            $('.adv-search-btn').addClass('active');
            $(this).addClass('opened').find('i').addClass('fa-caret-up').removeClass('fa-caret-down');
            $('.extra-search-options').removeClass('closed').stop(true, true).fadeIn({ duration: slideDuration, queue: false }).css('display', 'none').slideDown(slideDuration);
        }
        event.preventDefault();
    });

    var config = {
      '.chosen-select'           : {disable_search_threshold: 10, width:"100%"},
      '.chosen-select-deselect'  : {allow_single_deselect:true, width:"100%"},
      '.chosen-select-no-single' : {disable_search_threshold:10, width:"100%"},
      '.chosen-select-no-results': {no_results_text:'Oops, nothing found!'},
      '.chosen-select-width'     : {width:"95%"}
    };
    for (var selector in config) {
      $(selector).chosen(config[selector]);
    }



    /*----------------------------------------------------*/
    /*  Tabs
    /*----------------------------------------------------*/

    var $tabsNav    = $('.tabs-nav'),
     $tabsNavLis = $tabsNav.children('li');
    // $tabContent = $('.tab-content');

    $tabsNav.each(function() {
     var $this = $(this);

        $this.next().children('.tab-content').stop(true,true).hide()
        .first().show();

        $this.children('li').first().addClass('active').stop(true,true).show();
     });

    $tabsNavLis.on('click', function(e) {
         var $this = $(this);

         $this.siblings().removeClass('active').end()
        .addClass('active');

        $this.parent().next().children('.tab-content').stop(true,true).hide()
        .siblings( $this.find('a').attr('href') ).fadeIn();

        e.preventDefault();
    });



    /*----------------------------------------------------*/
    /*  Filter by Price
    /*----------------------------------------------------*/

    $( "#slider-range" ).slider({
      range: true,
      min: 0,
      max: 50,
      values: [ 0, 50 ],
      slide: function( event, ui ) {
        event = event;
        $( "#amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
      }
    });
    $( "#amount" ).val( "$" + $( "#slider-range" ).slider( "values", 0 ) +
      " - $" + $( "#slider-range" ).slider( "values", 1 ) );



    /*----------------------------------------------------*/
    /*  Accordions
    /*----------------------------------------------------*/

    var $accor = $('.accordion');

     $accor.each(function() {
        $(this).addClass('ui-accordion ui-widget ui-helper-reset');
        $(this).find('h3').addClass('ui-accordion-header ui-helper-reset ui-state-default ui-accordion-icons ui-corner-all');
        $(this).find('div').addClass('ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom');
        $(this).find("div").hide().first().show();
        $(this).find("h3").first().removeClass('ui-accordion-header-active ui-state-active ui-corner-top').addClass('ui-accordion-header-active ui-state-active ui-corner-top');
        $(this).find("span").first().addClass('ui-accordion-icon-active');
    });

    var $trigger = $accor.find('h3');

    $trigger.on('click', function(e) {
        var location = $(this).parent();

        if( $(this).next().is(':hidden') ) {
            var $triggerloc = $('h3',location);
            $triggerloc.removeClass('ui-accordion-header-active ui-state-active ui-corner-top').next().slideUp(300);
            $triggerloc.find('span').removeClass('ui-accordion-icon-active');
            $(this).find('span').addClass('ui-accordion-icon-active');
            $(this).addClass('ui-accordion-header-active ui-state-active ui-corner-top').next().slideDown(300);
        }
         e.preventDefault();
    });



    /*----------------------------------------------------*/
    /*  Magnific Popup
    /*----------------------------------------------------*/

      $('body').magnificPopup({
        type: 'image',
        delegate: 'a.mfp-gallery',

        fixedContentPos: true,
        fixedBgPos: true,

        overflowY: 'auto',

        closeBtnInside: true,
        preloader: true,

        removalDelay: 0,
        mainClass: 'mfp-fade',

        gallery:{enabled:true},

        callbacks: {
          buildControls: function() {
            console.log('inside'); this.contentContainer.append(this.arrowLeft.add(this.arrowRight));
          }

        }
      });


      $('.popup-with-zoom-anim').magnificPopup({
        type: 'inline',

        fixedContentPos: false,
        fixedBgPos: true,

        overflowY: 'auto',

        closeBtnInside: true,
        preloader: false,

        midClick: true,
        removalDelay: 300,
        mainClass: 'my-mfp-zoom-in'
      });


      $('.mfp-image').magnificPopup({
        type: 'image',
        closeOnContentClick: true,
        mainClass: 'mfp-fade',
        image: {
          verticalFit: true
        }
      });


      $('.popup-youtube, .popup-vimeo, .popup-gmaps').magnificPopup({
        disableOn: 700,
        type: 'iframe',
        mainClass: 'mfp-fade',
        removalDelay: 160,
        preloader: false,

        fixedContentPos: false
      });



    /*----------------------------------------------------*/
    /*  Toggles
    /*----------------------------------------------------*/
    $(".toggle-container").hide();
    $(".trigger").toggle(function(){
          $(this).addClass("active");
    }, function () {
        $(this).removeClass("active");
    });
    $(".trigger").click(function(){
         $(this).next(".toggle-container").slideToggle();
    });

     $(".trigger.opened").toggle(function(){
         $(this).removeClass("active");
     }, function () {
         $(this).addClass("active");
     });

    $(".trigger.opened").addClass("active").next(".toggle-container").show();



 // ------------------ End Document ------------------ //
});

})(this.jQuery);


