
$(".tooltipclass").tooltip();
var baseUrl = window.baseUrl;

var genderOptions = '<option value="0">-- Select option --</option><option value="1">Male</option><option value="2">Female</option>';
var titleOptions = '<option value="0">-- Select option --</option><option value="1">Mr.</option><option value="2">Mrs.</option><option value="3">Miss</option><option value="4">Chief</option><option value="5">Rotr.</option>';
var cListIdds = [];
var customListDataCahe = [];
var dataLoaded = false;

$(function ()
{
    $("#Gender").html(genderOptions);
    $("#Title").html(titleOptions);

    $(".editBtn").click(function ()
    {
        var itemId = $(this).data("edit-id");
        if (itemId < 1)
        {
            alert('Invalid selection');
            return;
        }
        getBaseData(itemId);
    });
    
});

function processBaseData()
{
    window.baseDataScope.Email = $("#Email").val();
    window.baseDataScope.ProjectPrimaryCode = $("#baseDataTemplate #ProjectPrimaryCode").val();
    var confirmProjectPrimaryCode = $("#baseDataTemplate #confirmProjectPrimaryCode").val();
    window.baseDataScope.MobileNumber = $("#MobileNumber").val();
    window.baseDataScope.Surname = $("#Surname").val();
    window.baseDataScope.Gender = $("#Gender").val();
    window.baseDataScope.Title = $("#Title").val();
    window.baseDataScope.CuntryCode = $("#CuntryCode").val();
    window.baseDataScope.MiddleName = $("#MiddleName").val();
    window.baseDataScope.Firstname = $("#Firstname").val();
    window.baseDataScope.MiddleName = $("#MiddleName").val();
    window.baseDataScope.ValidIdNumber = $("#ValidIdNumber").val();
    
    if (window.baseDataScope.Email.length < 1) {
        alert('Please provide Email');
        return;
    }

    if (window.baseDataScope.ProjectPrimaryCode.length < 1) {
        alert('Please provide Project Primary');
        return;
    }
    if (window.baseDataScope.MobileNumber.length < 1) {
        alert('Please provide Mobile Number');
        return;
    }

    if (window.baseDataScope.Surname.length < 1) {
        alert("Please provide Surname");
        return;
    }

    if (window.baseDataScope.Firstname.length < 1) {
        alert("Please provide First Name");
        return;
    }

    if (window.baseDataScope.Gender.length < 1) {
        alert('Please select Gender');
        return;
    }

    if (window.baseDataScope.ProjectPrimaryCode !== confirmProjectPrimaryCode)
    {
        alert('The Project Primary Codes do not match');
        return;
    }

    if (window.baseDataScope.DOB.length < 1)
    {
        alert('Please provide Date Of Birthday');
        return;
    }

    var url = '';

    if (window.baseDataScope.EnrollmentId.length < 1)
    {
      url = baseUrl + '/BaseData/EnrollBaseData';
    }
    else
    {
        url = baseUrl + '/BaseData/UpdateBaseData';
    }

    $.ajax({
        url: url,
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ basedata: window.baseDataScope }),
        type: 'POST',
        beforeSend: function () { window.showUiBusy(); },
        success: function (response)
        {
            window.hideUiBusy();
            alert(response.Message);

            if (!response || response.Code < 1)
            {
                return;
            }

            window.baseDataScope.EnrollmentId = response.EnrollmentId;
            InitialiseBaseDataUi(response.EnrollmentId);
        }
    });
}

$(document).ready(function ()
{
    window.baseDataScope =
    {
        TableId: 0,
        Email: '',
        MobileNumber: '',
        Surname: '',
        Firstname: '',
        Gender: '',
        DOB: '',
        EnrollmentId: '',
        EnrollmentDate: '',
        Title: '',
        CuntryCode: '',
        ProjectPrimaryCode: window.generateUid().toUpperCase(),
        ValidIdNumber: '',
        TestColumn: '',
        ProjectCode: ''
    };
    getBaseDataByEmail();
   
});

//____________________________________________ Custom Data ____________________________________________________

//(response.EnrollmentId, tblList, baseDataTemplate, customDataView);
//(itemId, 'tblList', 'customDataView', 'baseDataTemplate', false)
var totalTabs = 0;

function InitialiseBaseDataUi(itemId)
{
    clearBiometricControls();
    $.ajax({
        url: baseUrl + '/ProjectCustomData/GetCustomDataGroupFields?baseDataId=' + itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () { window.showUiBusy(); },
        success: function (data)
        {
            window.hideUiBusy();
            if (!data || data.TableId < 1 || data.CustomGroupViewModels.length < 1)
            {
                alert('Custom Data information processing could not be initiated. Please try again or contact Support team');
                return;
            }

            $('#EnrollmentId').html(data.EnrollmentId);
            $('#iProjectPrimaryCode').html(data.ProjectPrimaryCode);
            $('#Name').html(data.Name);
            
            var accordionUlHeader = $('.autoTabs');
            var tabsParent = $('.customTabs');

            accordionUlHeader.html('');
            tabsParent.html('');

            var customGroups = data.CustomGroupViewModels;
            var length = 1;
            
            $.each(customGroups, function (i, o)
            {
                var li = '<li role="presentation"><a id="grp_' + length + '" href="#tab_' + length + '" role="tab" data-toggle="tab">' + o.GroupName + '</a></li>';
                totalTabs++;
                accordionUlHeader.append(li);

                var tabpanel = '<div role="tabpanel" class="tab-pane fade" id="tab_' + length + '" style="margin-top: 30px"><div class="row">';
                var customFields = o.CustomFieldViewModels;
                
                if (customFields !== undefined && customFields !== null && customFields.length > 0)
                {
                    $.each(customFields, function (j, f)
                    {
                        var req = f.Required === 1 || f.Required === '1' ? 'true' : 'false';
                        var req2 = f.Required === 1 || f.Required === '1' ? '*' : '';

                        if (f.CustomFieldType.FieldTypeName === 'Text')
                        {
                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label><input data-edit-id="" data-required="' + req + '" size="' + f.CustomFieldSize + '" type="text" class="form-control" id="' + f.CustomFieldId + '" /></div>';
                        }

                        else if (f.CustomFieldType.FieldTypeName === 'Number')
                        {
                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label><input data-edit-id="" data-required="' + req + '" size="' + f.CustomFieldSize + '" type="number" class="form-control" id="' + f.CustomFieldId + '" /></div>';
                        }

                        else if (f.CustomFieldType.FieldTypeName === 'Date') {
                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label><input data-edit-id="" data-required="' + req + '" size="' + f.CustomFieldSize + '" type="text" class="form-control date" id="' + f.CustomFieldId + '" /></div>';
                        }

                        if (f.CustomFieldType.FieldTypeName === 'List' && f.CustomList !== undefined && f.CustomList !== null) {
                            var qq = f.CustomList;

                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label>';

                            if (qq.HasChildren === true) {
                                tabpanel += '<select data-edit-id="" style="text-transform: capitalize" data-required="' + req + '" data-has-child="true" data-required="' + f.Required + '" data-pList="' + qq.ParentListId + '" data-list="' + qq.CustomListId + '" onchange="checkChildList(this.id)" class="form-control" id="' + f.CustomFieldId + '"><option value="0">-- Select option --</option>';
                            }
                            else {
                                tabpanel += '<select data-edit-id="" style="text-transform: capitalize" data-required="' + req + '" data-has-child="false" data-required="' + f.Required + '" data-list="' + qq.CustomListId + '" class="form-control" id="' + f.CustomFieldId + '"><option value="0">-- Select option --</option>';
                            }

                            cListIdds.push({ FieldId: f.CustomFieldId, ListId: qq.CustomListId, hasChildren: qq.HasChildren });

                            tabpanel += '</select></div>';
                        }
                    });
                }

                tabpanel += '</div><br/>';
                tabpanel += '<div class="row"><div class="col-md-12">' + '<button type="button" class="btn btn-primary" onclick="saveThis(' + length + ')">Save</button></div></div>';
                tabpanel += '<br/></div>';

                tabsParent.append(tabpanel);
                length++;
            });
            
            if (data.FormPath !== undefined && data.FormPath !== null && data.FormPath.length > 0)
            {
                $("#fileDownload").attr('href', '/BaseData/DownloadContentFromFolder2?path=' + data.FormPath).fadeIn();
                $("#fileNotAvailable").hide();
                
            }
            else
            {
                $("#fileDownload").attr('href', '').hide();
                $("#fileNotAvailable").show();
            }

            $('.autoTabs li:first').addClass('active');

            $('.customTabs #tab_1').addClass('in active');

            var dateControls = $('.date');

            if (dateControls !== undefined && dateControls !== null && dateControls.length > 0)
            {
                $(".date").each(function ()
                {
                    $(this).datetimepicker({
                        format: 'Y-m-d',
                        timepicker: false,
                        onSelectDate: function (dp, $input)
                        {
                            //Assign to the appropriate Field

                            //window['Tbl_' + $(o).id][$(o).data('prop')] = dp;
                        }
                    });
                });
            }
            if (window.baseDataScope.ApprovalStatus === 2) {
                $('select').attr('readonly', 'readonly').attr('disabled', 'disabled');
                $('.btn-primary').attr('disabled', 'disabled').css('display', 'none');
                $('.btn-info').attr('disabled', 'disabled').css('display', 'none');
                $('input:not(button)').attr('readonly', 'readonly').attr('disabled', 'disabled');
                $('textarea').attr('readonly', 'readonly').attr('disabled', 'disabled');
            }

            getCustomListDatas(data.EnrollmentId);

            $("select").select2({
                allowClear: false
            });

            $("#custDtBtn").fadeIn();
        }
    });
}

function getCustomListDatas(enrollmentId) {
    if (cListIdds.length < 1) {
        alert('An error was encountered on the page. Please refresh the page and try again or contact Support team');
        return;
    }

    if (dataLoaded === true) {
        buildCustomListDatas(enrollmentId);
        return;
    }
    customListDataCahe = [];
    $.each(cListIdds, function (i, c) {
        $.ajax({
            url: baseUrl + '/ProjectCustomData/GetCustomListDatas?listId=' + c.ListId,
            contentType: "application/json",
            dataType: 'json',
            type: 'GET',
            success: function (response) {
                if (response && response.length > 0) {
                    var option = '<option value="">-- Select option --</option>';
                    $.each(response, function (j, u) {
                        option += '<option value="' + u.CustomListDataId + '">' + u.ListDataName + '</option>';
                        customListDataCahe.push(u);
                    });

                    $('#' + c.FieldId).html(option);
                }
            }
        });
    });

    if (customListDataCahe.length > 0) {
        dataLoaded = true;
    }

    getUserDatas(enrollmentId);
}

function buildCustomListDatas(enrollmentId) {
    if (customListDataCahe.length < 1 || cListIdds.length < 1) {
        alert('An error was encountered on the page. Please refresh the page and try again or contact Support team');
        return;
    }

    $.each(cListIdds, function (i, c) {
        var dataList = customListDataCahe.filter(function (l) {
            return l.CustomListId === c.ListId;
        });

        if (dataList && dataList.length > 0) {
            var option = '<option value="">-- Select option --</option>';
            $.each(dataList, function (j, u) {
                option += '<option value="' + u.CustomListDataId + '">' + u.ListDataName + '</option>';
            });
            $('#' + c.FieldId).html(option);
        }
    });
    getUserDatas(enrollmentId);
}

function getBaseDataByEmail()
{
    window.customDataList = [];
    $.ajax({
        url: baseUrl + '/BaseData/GetBaseDataByEmail',
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function (){window.showUiBusy();},
        success: function (response)
        {
            $.ajax({
                url: baseUrl + '/BaseData/GetProject',
                contentType: "application/json",
                dataType: 'json',
                type: 'GET',
                beforeSend: function () {
                    window.showUiBusy();
                },
                success: function (response)
                {
                    window.hideUiBusy();
                    if (!response || response.TableId < 1)
                    {
                        alert('Project information could not be retrieved. Please try again or contact Support team');
                        return;
                    }

                    window.baseDataScope.ProjectCode = response.ProjectCode;
                }
            });

            window.hideUiBusy();
            console.log()

            if (!response || response.EnrollmentId === null || response.EnrollmentId.length < 1)
            {
                if (response.Email !== null || response.Email.length > 0)
                {
                    $("#Email").val(response.Email);
                    $("#MobileNumber").val(response.MobileNumber);
                    $("#Surname").val(response.Surname);
                    $("#Firstname").val(response.Firstname);
                    $("#Gender").html(genderOptions).val(response.Gender);
                    return;
                }
            }

            window.baseDataScope = response;

            if (window.baseDataScope.ApprovalStatus === 2)
            {
                $('select').attr('readonly', 'readonly').attr('disabled', 'disabled');
                $('.btn-primary').attr('disabled', 'disabled').css('display', 'none');
                $('.btn-info').attr('disabled', 'disabled').css('display', 'none');
                $('input:not(button)').attr('readonly', 'readonly').attr('disabled', 'disabled');
                $('textarea').attr('readonly', 'readonly').attr('disabled', 'disabled');
            }

            $("#myModalLabel").html('Update Primary Information');
            $("#EnrollmentId").val(response.EnrollmentId);
            $("#ProjectPrimaryCode").val(response.ProjectPrimaryCode);
            $("#confirmProjectPrimaryCode").val(response.ProjectPrimaryCode);
            $("#Email").val(response.Email);
            $("#MobileNumber").val(response.MobileNumber);
            $("#Surname").val(response.Surname);
            $("#CuntryCode").val(response.CuntryCode);
            $("#MiddleName").val(response.MiddleName);
            $("#Firstname").val(response.Firstname);
            $("#MiddleName").val(response.MiddleName);
            $("#ValidIdNumber").val(response.ValidIdNumber);
            $("#Gender").html(genderOptions).val(response.Gender);
            $("#Title").html(titleOptions).val(response.Title);

            var dateVar = moment(response.DOB).format("DD/MM/YYYY");
            $("#DOB").datetimepicker({
                format: 'd/m/Y', maxDate: new Date() - 1, timepicker: false,
                onSelectDate: function (dp, $input) {
                    response.DOB = dp;
                }
            }).val(dateVar);
            window.baseDataScope.EnrollmentId = response.EnrollmentId;
            $("#custDtBtn").fadeIn();
            InitialiseBaseDataUi(response.EnrollmentId);
        }
    });
}
function goToCustomData()
{
    $('#baseDataTemplate').hide();
    $('#customDataView').fadeIn();
}
function goToBaseData()
{
    $('#customDataView').hide();
    $('#baseDataTemplate').fadeIn();
}
function checkChildList(controlId, crimsCustomData) {
    var tt = $('#' + controlId);
    var value = tt.val();
    if (value === undefined || value === null || value === '0' || value === 'null' || value.length < 1) {
        return;
    }

    $.ajax({
        url: baseUrl + '/ProjectCustomData/GetCustomListDatasByParentList?parentListId=' + tt.data('list') + '&parentNode=' + value + '&parentCustomFieldId=' + controlId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () {
            window.showUiBusy();
        },
        success: function (cList) {
            window.hideUiBusy();
            if (!cList || !cList.CustomListDatas || cList.CustomListDatas.length < 1) {
                alert('Some Further important information information could not be retrieved. Please try again or contact Support team');
                return;
            }

            var req = cList.Required === 1 || cList.Required === '1' ? 'true' : 'false';
            var req2 = cList.Required === 1 || cList.Required === '1' ? '*' : '';
            window.customDataList.push(cList);
            if ($('#' + cList.CustomFieldId).length) {
                var options = '<option value="0">-- Select option --</option>';
                cList.CustomListDatas.forEach(function (c, i) {
                    options += '<option value="' + c.CustomListDataId + '">' + c.ListDataName + '</option>';
                });

                $('#' + cList.CustomFieldId).html(options);
            }
            else {
                var newFormGroup = '<div style="display: none; margin-bottom: 10px" class="col-md-6" id="frmGrp_' + cList.CustomFieldId + '"><label>' + cList.CustomFieldName + ' ' + req2 + '</label>';

                var qq = cList;
                if (qq.HasChildren === true) {
                    newFormGroup += '<select data-edit-id="" data-pFId="' + controlId + '" data-required="' + req + '" data-has-child="true" data-required="' + cList.Required + '" data-list="' + qq.CustomListId + '" onchange="checkChildList(this.id)" class="form-control" id="' + cList.CustomFieldId + '"><option value="0">-- Select option --</option>';
                }
                else {
                    newFormGroup += '<select data-edit-id="" data-pFId="' + controlId + '" data-required="' + req + '" data-has-child="false" data-list="' + qq.CustomListId + '" class="form-control" id="' + cList.CustomFieldId + '"><option value="0">-- Select option --</option>';
                }

                qq.CustomListDatas.forEach(function (c, i) {
                    newFormGroup += '<option value="' + c.CustomListDataId + '">' + c.ListDataName + '</option>';
                });

                newFormGroup += '</select></div>';

                $('#frmGrp_' + tt.attr('id')).after(newFormGroup);

                $('#frmGrp_' + cList.CustomFieldId).fadeIn();
            }

            $("select").select2({
                allowClear: false
            });

            //Check if this child List already has a data previously selected
            setChildListValue(cList.CustomFieldId, crimsCustomData);

        }
    });
}
function saveThis(tabId)
{
    var tab = $('#tab_' + tabId);
    if (tab === undefined || tab === null)
    {
        alert('An unknown error was encountered on this page. Please refresh the page and try again');
        return;
    }

    var selectList = $('#tab_' + tabId + ' select');
    var textList = $('#tab_' + tabId + ' input:not(button)');
    var textAreaList = $('#tab_' + tabId + ' textarea');

    if ((!selectList || selectList.length < 1) && (!textList || textList.length < 1) && (!textAreaList || textAreaList.length < 1)) {
        alert('An unknown error was encountered on this page. Please refresh the page and try again');
        return;
    }

    var customDatas = [];

    if (selectList.length > 0) {
        for (var i = 0; i < selectList.length; i++) {
            var customDataControl = $(selectList[i]);
            var required = customDataControl.data('required');
            var customData = customDataControl.val();

            if (required === true || required === 'true') {
                if (customData === undefined || customData === null || customData === '0' || customData === '') {
                    alert('Please provide/select all required fields and try again');
                    return;
                }
            }

            customDatas.push({
                CustomDataId: customDataControl.data('edit-id'),
                CustomFieldId: customDataControl.attr('id'),
                EnrollmentId: window.baseDataScope.EnrollmentId,
                CrimsCustomData: customData,
                CustomListId: customDataControl.data('list'),
                ProjectSIteId: $('#ProjectPrimaryCode').html()
            });
        }
    }

    if (textList.length > 0) {
        for (var j = 0; j < textList.length; j++) {
            var textDataControl = $(textList[j]);
            var tetxtRequired = textDataControl.data('required');
            var textData = textDataControl.val();

            if (tetxtRequired === true || tetxtRequired === 'true') {
                if (textData === undefined || textData === null || textData.length < 1 || textData === '') {
                    alert('Please provide/select all required fields and try again');
                    return;
                }
            }

            customDatas.push({
                CustomDataId: textDataControl.data('edit-id'),
                CustomFieldId: textDataControl.attr('id'),
                EnrollmentId: window.baseDataScope.EnrollmentId,
                CrimsCustomData: textData,
                CustomListId: null,
                ProjectSIteId: $('#ProjectPrimaryCode').html()
            });
        }
    }

    if (textAreaList.length > 0) {
        for (var k = 0; k < textAreaList.length; k++) {
            var textAreaDataControl = $(textAreaList[k]);
            var textAreaRequired = textAreaDataControl.data('required');
            var textAreaData = textAreaDataControl.val();

            if (textAreaRequired === true || textAreaRequired === 'true') {
                if (textAreaData === undefined || textAreaData === null || textAreaData.length < 1 || textAreaData === '') {
                    alert('Please provide/select all required fields and try again');
                    return;
                }
            }

            customDatas.push({
                CustomDataId: textAreaDataControl.data('edit-id'),
                CustomFieldId: textAreaDataControl.attr('id'),
                EnrollmentId: window.baseDataScope.EnrollmentId,
                CrimsCustomData: textAreaData,
                CustomListId: null,
                ProjectSIteId: $('#ProjectPrimaryCode').html()
            });
        }
    }

    if (customDatas.length < 1) {
        alert('An unknown error was encountered on this page. Please refresh the page and try again');
        return;
    }

    $.ajax({
        url: baseUrl + '/CustomData/ProcessCustomData',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ customDataList: customDatas }),
        type: 'POST',
        beforeSend: function () { window.showUiBusy(); },
        success: function (response)
        {
            window.hideUiBusy();

            alert(response.Message);

            if (!response || response.Code < 1)
            {
                return;
            }

            //do this to refresh only the processed section of the UI controls
            //To mark them as edits for subsequent actions
            $.each(response.CustomDataList, function (i, c)
            {
                $('#' + c.CustomFieldId).attr('data-edit-id', c.CustomDataId);
            });
           
            if (parseInt(tabId) < totalTabs)
            {
                var nextTabRef = tabId + 1;
                $('#grp_' + nextTabRef).trigger('click');
                $('#grp_' + nextTabRef).addClass('active');
                $('#tab_' + nextTabRef).addClass('in active');
               
            }
            else
            {
                $('#grp_1').trigger('click');
                $('#tab_1').addClass('in active');

                $('#grp_1').addClass('active');
                $('#tab_1').addClass('in active');
            }

            $('#grp_' + tabId).removeClass('active');
            $('#tab_' + tabId).removeClass('in active');
        }
    });
}
function getUserDatas(enrollmentId) {
    $.ajax({
        url: baseUrl + '/CustomData/GetUserCustomDatas?enrollmentId=' + enrollmentId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        success: function (response)
        {
            window.hideUiBusy();
            if (!response || response.length < 1)
            {
                return;
            }

            window.customDataList = response;
            $('#tagDiv').fadeIn();
            populateCustomDataUi(response);
        }
    });
}
function populateCustomDataUi(data) {
    if (!data || data.length < 1) {
        return;
    }

    getBioElements();

    $.each(data, function (i, o) {
        var dataControl = $('#' + o.CustomFieldId);
        if (dataControl !== undefined && dataControl !== null && dataControl.length > 0) {
            //Set the previously selected data and set 
            //the CustomDataId to data-edit-id for Editing

            if (o.CustomListId === undefined || o.CustomListId === null || o.CustomListId.length < 1) {
                dataControl.val(o.CrimsCustomData).attr('data-edit-id', o.CustomDataId);
            }
            else {
                if (o.ParentListId === undefined || o.ParentListId === null || o.ParentListId.length < 1) {
                    dataControl.val(o.CrimsCustomData).attr('data-edit-id', o.CustomDataId);

                    if (o.HasChildren === true) {
                        checkChildList(o.CustomFieldId, o.ChildCrimsCustomData);
                    }
                }
            }
        }


    });
}
function setChildListValue(listFieldId, crimsCustomData) {
    if (window.customDataList === undefined || window.customDataList === null || window.customDataList.length < 1 || listFieldId === undefined || listFieldId === null || listFieldId.length < 1) //window.customDataList === undefined || window.customDataList === null || window.customDataList.length < 1 ||
    {
        return;
    }
    var dataControl = $('#' + listFieldId);

    if (dataControl !== undefined && dataControl !== null) {
        var data = window.customDataList.filter(function (o) {
            return o.CustomFieldId === listFieldId;
        });
        if (crimsCustomData !== undefined && crimsCustomData !== null && crimsCustomData.length > 0) {
            data = data.filter(function (o) {
                return o.CrimsCustomData === crimsCustomData;
            });
            if (data.length > 0) {
                dataControl.val(crimsCustomData).attr('data-edit-id', data[0].CustomDataId);
                if (data[0].HasChildren === true) {
                    checkChildList(listFieldId);
                }
            }
        } else {
            var childData = window.customDataList.filter(function (q) {
                return q.ParentListId === data[0].ParentListId;
            });

            if (childData.length > 0) {
                dataControl.val(childData[0].CrimsCustomData).attr('data-edit-id', childData[0].CustomDataId);
                if (childData[0].HasChildren === true) {
                    checkChildList(listFieldId);
                }
            }
        }

    }
    else {
        alert('Control not found : ' + listFieldId);
    }
}
$('#DOB').datetimepicker({
    format: 'd/m/Y',
    maxDate: new Date() - 1,
    timepicker: false,
    onSelectDate: function (dp, $input) {
        window.baseDataScope.DOB = dp;
    }
});
$(document).ready($('#dataForm').on('change', function (e)
{
    var dataFile = e.target.files[0];
    if (dataFile !== undefined && dataFile !== null && dataFile.size > 0)
    {
        if (window.FormData !== undefined)
        {
            var data = new FormData();
            data.append(dataFile.name, dataFile);

            $.ajax({
                type: "POST",
                url: baseUrl + '/BaseData/UploadFile?enrolmentId=' + window.itemId + '&oldPath=' + window.baseDataScope.FormPath,
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
                    window.baseDataScope.FormPath = response.FilePath;
                },
                error: function (xhr) {
                    var err = JSON.parse(xhr.responseText).Message;
                    alert(err);
                }
            });
        }
        else {
            alert("This browser doesn't support HTML5 file uploads!");
        }
    }
}));
function getBioElements() {
    $.ajax({
        url: baseUrl + '/Enrollment/GetBiometricData?enrollmentId=' + window.EnrollmentId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        success: function (response)
        {
            window.hideUiBusy();
            if (!response || response.PhotoPath === undefined || response.PhotoPath === null|| response.PhotoPath.length < 1)
            {
                return;
            }
            
            $('#photo').attr('src', response.Photo);
            $('#lLittle').attr('src', baseUrl + response.LeftLittle);
            $('#lRing').attr('src', baseUrl + response.LeftRing);
            $('#lMiddle').attr('src', baseUrl + response.LeftMiddle);
            $('#lIndex').attr('src', baseUrl + response.LeftIndex);
            $('#lThumb').attr('src', baseUrl + response.LeftThumb);
            $('#rThumb').attr('src', baseUrl + response.RightThumb);
            $('#rIndex').attr('src', baseUrl + response.RightIndex);
            $('#rMiddle').attr('src', baseUrl + response.RightMiddle);
            $('#rRing').attr('src', baseUrl + response.RightRing);
            $('#rLittle').attr('src', baseUrl + response.RightLittle);
            $('#signature').attr('src', response.Signature);

            $('#bioDiv').fadeIn();

        }
    });
}
function previewForm()
{
    var itemId = window.baseDataScope.EnrollmentId;
    if (!itemId || itemId.length < 1)
    {
        alert('Action cannot be completed now. Please try again later.');
        return;
    }
    
    $("#pDwnLink").fadeIn();
    window.customDataList = [];
    window.itemId = itemId;

    $.ajax({
        url: baseUrl + '/Approval/GetCustomDataGroupAndFields?baseDataId=' + itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () { window.showUiBusy(); },
        success: function (response)
        {
            window.hideUiBusy();
            if (!response || response.length < 1)
            {
                alert('The requested process failed. Please try again or contact Support team');
                return;
            }

            InitialisePreviewUi(response);
        }
    });

   
}
function InitialisePreviewUi(data)
{
    console.log(data);
    if (!data || data.CustomGroupViewModels.length < 1)
    {
        alert('An unknown error was encountered. Please refresh the page and try again.');
        return;
    }

    window.EnrollmentId = data.EnrollmentId;
    $('#iProjectPrimaryCode').html(data.ProjectPrimaryCode);
    $('#pName').html(data.Name);

    if (data.FormPath !== undefined && data.FormPath !== null && data.FormPath.length > 0) {
        $("#pFileDownload").attr('href', '/BaseData/DownloadContentFromFolder2?path=' + data.FormPath).fadeIn();
        $("#pFileNotAvailable").hide();

    }
    else {
        $("#pFileDownload").attr('href', '').hide();
        $("#pFileNotAvailable").show();
    }

    var tabsParent = $('.appDiv');

    getBioElements();

    tabsParent.html('');

    var customGroups = data.CustomGroupViewModels;
    var length = 1;
    var parentLists = [];

    $.each(customGroups, function (i, o) {
       
        var tabpanel = '<div class="col-md-12" id="appTab_' + length + '" style="margin-top: 3px"><div class="row"><div class="col-md-12"><h4 role="presentation">' + o.GroupName + '</h4></div>';
        var customFields = o.CustomFieldViewModels;
        if (customFields !== undefined && customFields !== null && customFields.length > 0) {
            $.each(customFields, function (j, f) {
                if (f.CustomListId !== undefined && f.CustomListId !== null && f.CustomListId.length > 0 && f.CustomListId !== 'null' && f.CustomListId !== '0') {
                    tabpanel += '<div  id="appFrmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' </label> : <h4 id="' + f.CustomFieldId + '">' + f.CustomListDataName + '</h4></div>';
                    if (f.HasChildren === true) {
                        parentLists.push({ ParentListId: f.CustomListId, ParentNode: f.CrimsCustomData, CustomDataId: f.CustomDataId, PrecedingField: f.CustomFieldId });
                    }
                }
                else {
                    if (f.CrimsCustomData !== undefined && f.CrimsCustomData !== null && f.CrimsCustomData.length > 0 && f.CrimsCustomData !== 'null') {
                        tabpanel += '<div  id="appFrmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' </label> : <h4 id="' + f.CustomFieldId + '">' + f.CrimsCustomData + '</h4></div>';
                    }
                }
            });
        }

        tabpanel += '</div><br/>';
        tabpanel += '<br/></div>';

        tabsParent.append(tabpanel);
        length++;
    });

    if (parentLists.length > 0)
    {
        checkChildPreviewList(parentLists);
    }

    $("#baseDataTemplate").hide();
    $('#preview').fadeIn();

}
function checkChildPreviewList(parentList) {
    if (parentList === undefined || parentList === null || parentList.length < 1) {
        return;
    }

    $.ajax({
        url: baseUrl + '/Approval/GetCustomListApprovalDataByParentList',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ parentModels: parentList }),
        type: 'POST',
        beforeSend: function () {
            window.showUiBusy();
        },
        success: function (customFields) {
            window.hideUiBusy();
            if (!customFields || customFields.length < 1) {
                alert('Some Further important information information could not be retrieved. Please try again or contact Support team');
                return;
            }

            parentList = [];

            $.each(customFields, function (j, f) {
                var tabpanel = '';

                if (f.CustomListId !== undefined && f.CustomListId !== null && f.CustomListId.length > 0 && f.CustomListId !== 'null') {
                    tabpanel += '<div  id="appFrmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' </label> : <h4 id="' + f.CustomFieldId + '">' + f.CustomListDataName + '</h4></div>';

                    if (f.HasChildren === true) {
                        parentList.push({ ParentListId: f.CustomListId, ParentNode: f.CrimsCustomData, CustomDataId: f.CustomDataId, PrecedingField: f.CustomFieldId });
                    }
                }
                $('#appFrmGrp_' + f.PrecedingField).after(tabpanel);

                $('#appFrmGrp_' + f.PrecedingField).fadeIn();
            });

            if (parentList.length > 0) {
                checkChildPreviewList(parentList);
            }

        }
    });
}
function closePreviewView()
{
    $('#preview').hide();
    $('#baseDataTemplate').fadeIn();

}

var qrPath = '';
function printTag()
{
    if (window.baseDataScope.EnrollmentId === undefined || window.baseDataScope.EnrollmentId === null || window.baseDataScope.EnrollmentId.length < 1)
    {
        alert('Please ensure you have provided all required information before proceeding');
        return;
    }

    $.ajax({
        url: baseUrl + '/BaseData/GenerateQrCode?enrolmentId=' + window.baseDataScope.EnrollmentId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function ()
        {
            window.showUiBusy();
        },
        success: function (response)
        {
            window.hideUiBusy();
            if (response === null)
            {
                alert('Process failed. Please contact the support team for help.');
                return;
            }

            qrPath = response;

            $('#qrCode').attr('src', response);
            var name = window.baseDataScope.Firstname  + ' ' + window.baseDataScope.Surname;
            $('#FullName').html(name);
            $('#PrimaryCode').html(window.baseDataScope.ProjectPrimaryCode);
            $('#tEmail').html(window.baseDataScope.Email);
          
            print();
        }
    });
}

function print()
{
    var printElement = $('#tagDivx');
    var contents = printElement.html();
    printElement.css('display', 'table');
    if (contents === undefined || contents === null || contents.length < 1)
    {
        alert('Nothing to print!');
        return false;
    }
    var url = parent.document.URL;
    var frame1 = document.createElement('iframe');
    frame1.src = url;
    frame1.name = "frame1";
    frame1.style.position = "absolute";
    frame1.style.top = "-1000000px";
    document.body.appendChild(frame1);
    var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
    frameDoc.document.open();
    frameDoc.document.write('<html><head><link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css"/>' +
        '<title>Chams _ Rims Registration Tag</title><br/><br/>');
    frameDoc.document.write('</head><body>');
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        document.body.removeChild(frame1);
    }, 500);

    printElement.css('display', 'none');

    //$.ajax({
    //    url: baseUrl + '/BaseData/DeleteFilex?path=' + qrPath,
    //    contentType: "application/json",
    //    dataType: 'json',
    //    type: 'GET',
    //    beforeSend: function ()
    //    {
    //        window.showUiBusy();
    //    },
    //    success: function (response)
    //    {
    //        window.hideUiBusy();
    //    }
    //});
    return true;
};
