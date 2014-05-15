

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
            color: '78A1FF'
        });

    }

    function showMessage(data) {
        $("#added").html(data).show();
    }
    function clearMessage() {
        $('#added').html("").hide();
    }
    function clearSearch() {
        $("#search").val("");
    }
    function clearMessagendSearch() {
        clearMessage();
        clearSearch();
    }
 
    function dialogClose() {
        $('#dialog').dialog('close');
    }
    function openPopup() {
        $('#dialog').dialog('open');
        // $('#dialog').parent().appendTo($('form:first'));
      
        return false;
    }

    function fireAjax(id) {

        $("#" + id + ".submit").click();

    }
 
    function selectAjax() {
        $("#selectSubmit").click();
    }
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#profile')
                    .attr('src', e.target.result)
                    .width(200)
                    .height(200);
            };

            reader.readAsDataURL(input.files[0]);
        }
    }

window.onload = function() {
    createDialog();

        $("#profile").on('click', function () {
            $('#file').click();
        });
        $("#" + id + ".link").on('click',function() {
            $("#" + id + ".link").addClass("active");
        })

}