

    function createDialog() {
        // Dialog

        $('#dialog').dialog({
            autoOpen: false,
            width: 'auto',
            closeOnEscape: true,
            resizable: false,
            draggable: true,
            modal: true,
            height: 'auto',
            title: 'Pas contact aan',
            color: '78A1FF'
        });

    }


 
    function openPopup() {
        $('#dialog').dialog('open');
        // $('#dialog').parent().appendTo($('form:first'));
      
        return false;
    }

window.onload = function() {
    createDialog();
}