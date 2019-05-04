    $(function () {
        $("#loaderbody").addClass("hide");

        $(document).bind("ajaxStart", function () {
            $("#loaderbody").removeClass("hide");
        }).bind("ajaxStop", function () {
            $("#loaderbody").addClass("hide");
        });
    });
    
    function ShowImagePreview(imageUploader, previewImage) {
        if (imageUploader.files && imageUploader.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $(previewImage).attr('src', e.target.result);
            }
            reader.readAsDataURL(imageUploader.files[0]);
        }
    }

    function jQueryAjaxPost(form) {
        $.validator.unobtrusive.parse(form);
        if ($(form).valid()) {
            var ajaxConfig = {
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                // contentType: false, // for file uploader
                // processData: false, // for file uploader
                success: function (response) {
                    if (response.success) {
                        $("#firstTab").html(response.html);
                        refreshAddNewTab($(form).attr("data-resetUrl"), true);
                        // success message
                        $.notify(response.message, "success");
                        // load DataTable in ajax request
                        if (typeof activatejQueryTable != undefined && $.isFunction(activatejQueryTable))
                            activatejQueryTable();
                    } else {
                        // error message
                        $.notify(response.message, "error");
                    }

                }
            }
            if ($(form).attr('enctype') == "multipart/form-data") {
                ajaxConfig["contentType"] = false; // for file uploader
                ajaxConfig["processData"] = false; // for file uploader
            }
            $.ajax(ajaxConfig);
        }
        return false;
    }
    function refreshAddNewTab(resetUrl, showViewTab) {
        $.ajax({
            type: 'GET',
            url: resetUrl,
            success: function (response) {
                $("#secondTab").html(response);
                $("ul.nav.nav-tabs a:eq(1)").html("Add New");
                if (showViewTab)
                    $("ul.nav.nav-tabs a:eq(0)").tab("show");
            }
        });
    }
    
    function Edit(url) {
        
            //alert("hi");
            $.ajax({
                type: 'GET',
                url: url,
                success: function (response) {
                    $("#secondTab").html(response);
                    $("ul.nav.nav-tabs a:eq(1)").html("Edit");
                    $("ul.nav.nav-tabs a:eq(1)").tab("show");
                }
            });
    }

    function Delete(url) {
        if (confirm("Are You Sure you want to delete the record? ") == true) {
           /* $.ajax({
                type: 'POST',
                url: url,
                success: function (response) {
                    if (response.success) {
                        $("#firstTab").html(response.html);
                        $.notify(response.message, "warn");
                        // load DataTable in ajax request
                        if (typeof activatejQueryTable != undefined && $.isFunction(activatejQueryTable))
                            activatejQueryTable();
                    } else {
                        $.notify(response.message, "error");
                    }
                }
            });*/
            $.ajax({
                type: "POST",
                url: '/UserController/Delete',
                data: JSON.stringify({ id: id }), //use id here
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function () {
                     alert("Data has been deleted.");
                    LoadData();
                },
                error: function () {
                    alert("Error while deleting data");
                }
            });
        }
    }
        


