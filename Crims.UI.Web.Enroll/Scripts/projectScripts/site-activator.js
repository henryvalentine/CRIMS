var baseUrl = window.baseUrl;

$(document).ready(function () {
    if (window.location.href.indexOf('ProjectExpired') > -1 && window.location.href.indexOf('true') > -1)
    {
        $('#siteExpired').fadeIn();
    }
    $('#license')
        .on('change',
            function (e)
            {
                var licenseFile = e.target.files[0];
                if (licenseFile !== undefined && licenseFile !== null && licenseFile.size > 0)
                {
                    if (window.FormData !== undefined)
                    {
                        var data = new FormData();
                        data.append(licenseFile.name, licenseFile);

                        $.ajax({
                            type: "POST",
                            url: baseUrl + '/SiteActivator/InstallLicense',
                            contentType: false,
                            dataType: 'json',
                            processData: false,
                            data: data,
                            beforeSend: function () { window.showUiBusy(); },
                            success: function (response)
                            {
                                window.hideUiBusy();
                                alert(response.Message);
                                if (response.Code < 1)
                                {
                                    return;
                                }
                                //window.location.href = baseUrl + "/Account/Setup";
                                window.location.href = baseUrl + "/Account/Login";
                            },
                            error: function (xhr)
                            {
                                var err = JSON.parse(xhr.responseText).Message;
                                alert(err);
                            }
                        });
                    }
                    else {
                        alert("This browser doesn't support HTML5 file uploads!");
                    }
                }
            });
});
