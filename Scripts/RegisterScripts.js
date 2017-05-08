var selectOpt = $('.dropdown');

function remove(element) {
    var amountEntries = $('.dropdown').size() + $('.dropdownclone').size();
    if (amountEntries <= 1) {
        return;
    }
    if (amountEntries === 2) {
        $('.deleteButton').hide();
    }
    $(element).closest(".dropdown, .dropdownclone").fadeOut('blind', function () {
        if ($(this).is(".dropdown")) {
            $(".dropdownclone:first").toggleClass("dropdown dropdownclone");
        }
        $(this).remove();
    });
}

function addNew() {
    var dropdownclone, inputCote, amountEntries = $('.dropdown').size() + $('.dropdownclone').size();


    dropdownclone = $('.dropdown').clone();

    //test
    var dropdown = $('.dropdown');
    var $dropdownSelects = dropdown.find('select');

    //Forcer le reset sur le clone de l'input
    var $inputCote = dropdownclone.find('input').val("");
    dropdownclone.attr("class", 'dropdownclone');
    dropdownclone.hide().appendTo(".clonecontainer").fadeIn(1000);
}
