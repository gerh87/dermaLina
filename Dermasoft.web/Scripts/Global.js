$(function () {
    scrollToTop();
    $(".dropdown-button").dropdown({
        inDuration: 0,
        hover: false,
        belowOrigin: true
    });
    
    $(".button-collapse").sideNav({
        menuWidth: 200
    });
    $('.modal-trigger').leanModal({
        dismissible: true,
        in_duration: 10, 
        out_duration: 10
    });
    $('ul.tabs').tabs();
    $('select').material_select();
    $('.materialboxed').materialbox();
    $('.collapsible').collapsible({
        accordion: false 
    });

});

jQuery('.date').datetimepicker({
    lang: 'es',
    timepicker: false,
    format: 'd/m/Y',
    startDate: '1990/01/01',
    onChangeDateTime: function (dp, $input) {
        alert($input.val())
    }
});

function scrollToTop()
{
    $(window).scroll(function(){
        if ($(this).scrollTop() > 100) {
            $('.scrollToTop').fadeIn();
        } else {
            $('.scrollToTop').fadeOut();
        }
    });
	
    //Click event to scroll to top
    $('.scrollToTop').click(function(){
        $('html, body').animate({scrollTop : 0},300);
        return false;
    });
}


function loadPopup(title, width) {
    $('#modal-popup').leanModal();
    //$("#poprender").dialog({
    //    title: title,
    //    width: width,
    //    modal: true,
    //    resizable: false
    //});
}

function loadPopin(title) {
    $('#modal-popup').leanModal();
}


function loadComplete(id, url, length, top) {
    $('#' + id).autocomplete({
        serviceUrl: url,
        transformResult: function (response) {
            return {
                suggestions: JSON.parse(response)
            };
        },
        minChars: length,
        lookupLimit: top
    });
}





