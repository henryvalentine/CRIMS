var baseUrl = '/crimsWeb';
//var baseUrl = '';

var mainTabListParent = '';
var mainTabParent = '';
var genderOptions = '<option value="0">-- Select option --</option><option value="1">Male</option><option value="2">Female</option>';
var titleOptions =
    '<option value="0">-- Select option --</option><option value="1">Mr.</option><option value="2">Mrs.</option><option value="3">Master</option><option value="4">Miss</option><option value="5">Chief</option>' +
        '<option value="6">Chief Mrs</option><option value="7">Knight</option><option value="6">Lady</option><option value="8">Dr</option><option value="9">Dr Mrs</option><option value="10">Barr</option>' +
        + '<option value="11">SAN</option><option value="12">Engr</option><option value="13">Rotr</option>';


var jtable = {};

$(function ()
{
    $.ajax({
        url: baseUrl + '/BaseData/GetProject',
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () {
            window.showUiBusy();
        },
        success: function (response) {
            window.hideUiBusy();
            if (!response || response.TableId < 1) {
                alert('Project information could not be retrieved. Please try again or contact Support team');
                return;
            }

            window.userRole = response.UserRole;
            window.projectCode = response.ProjectCode;

            //Initialise JQuery DataTable for Approved Enrollments
            if (/Approval/.test(window.location.href))
            {
                var approvalTable = $('#approvalTable');
                if (approvalTable !== undefined && approvalTable !== null) {
                    var approvedTableOptions = {};
                    approvedTableOptions.sourceUrl = baseUrl + "/BaseData/GetApprovedEnrollments";
                    approvedTableOptions.itemId = 'EnrollmentId';
                    approvedTableOptions.columnHeaders = ['ProjectPrimaryCode', 'Surname', 'Firstname', 'BiometricStatus', 'EnrollmentOfficer', 'EnrollmentDateStr'];
                    var customStr = window.userRole.length > 0 && window.userRole !== 'Enrollment_Officer' ? 'customA' : null;
                    var appTable = aprovalsTableManager(baseUrl, approvalTable, approvedTableOptions, null, customStr, 'approvalHistory');
                    appTable.removeAttr('width').attr('width', 'auto');
                    jtable = appTable;
                }
            }
        }
    });

    //Initialise JQuery DataTable for Fresh Enrollments
    if (/BaseData/.test(window.location.href))
    {
        var enrolleeTable = $('#enrolleeTable');
        if (enrolleeTable !== undefined && enrolleeTable !== null)
        {
            var tableOptions = {};
            tableOptions.sourceUrl = baseUrl + "/BaseData/GetEnrollments";
            tableOptions.itemId = 'EnrollmentId';
            tableOptions.columnHeaders = [
                'ProjectPrimaryCode', 'Surname', 'Firstname', 'BiometricStatus', 'EnrollmentOfficer',
                'EnrollmentDateStr'
            ];
            var ttc = enrolleeTableManager(baseUrl, enrolleeTable, tableOptions, 'edit', 'custom', 'bio', 'New Enrollment');
            ttc.removeAttr('width').attr('width', 'auto');
            jtable = ttc;
        }
    }
    
    //Initialise JQuery DataTable for Rejected Enrollments
    if (/Rejections/.test(window.location.href))
    {
        var rejectionTable = $('#rejectionTable');
        if (rejectionTable !== undefined && rejectionTable !== null)
        {
            var rejectionTableOptions = {};
            rejectionTableOptions.sourceUrl = baseUrl + "/BaseData/GetRejectedEnrollments";
            rejectionTableOptions.itemId = 'EnrollmentId';
            rejectionTableOptions.columnHeaders = ['ProjectPrimaryCode', 'Surname', 'Firstname', 'BiometricStatus', 'EnrollmentOfficer', 'EnrollmentDateStr'];
            var rejTable = rejectionsTableManager(baseUrl, rejectionTable, rejectionTableOptions, 'customA', 'approvalHistory');
            rejTable.removeAttr('width').attr('width', 'auto');
            jtable = rejTable;
        }
    }
    $(".btn_add").click(function ()
    {
        if (window.projectCode === undefined || window.projectCode === null || window.projectCode.length < 1)
        {
            alert('An error was encountered. Pleasse contact our support team');
            return;
        }

        $("#myModalLabel").html('Capture Basedata');
        $("#Gender").html(genderOptions);
        $("#Title").html(titleOptions);

        $("#Email").val('');
        $("#baseDataTemplate #ProjectPrimaryCode").val('');
        $("#baseDataTemplate #confirmProjectPrimaryCode").val('');
        $("#MobileNumber").val('');
        $("#Surname").val('');
        $("#Gender").val('');
        $("#Title").val('');
        $("#CuntryCode").val('');
        $("#DOB").val('');
        $("#MiddleName").val('');
        $("#Firstname").val('');
        $("#MiddleName").val('');
        $("#ValidIdNumber").val('');
        
        window.baseDataScope =
         {
            TableId : 0,
            ProjectPrimaryCode: '',
            Email: '',
            MobileNumber: '',
            Surname: '',
            Gender: '',
            Title: '',
            DOB: '',
            CuntryCode: '',
            MiddleName: '',
            Firstname: '',
            ValidIdNumber: '',
            ProjectCode: window.projectCode
        }

        $('.tblList').hide();
        $('#customDataView').hide();
        $("#baseDataTemplate").fadeIn();

    });
});
function edit(itemId)
{
    if (!itemId || itemId.length < 1) 
    {
        alert('Invalid selection');
        return;
    }

    getBaseData(itemId, 'tblList', 'baseDataTemplate', false);
}
function custom(itemId) {
    if (!itemId || itemId.length < 1) {
        alert('Invalid selection');
        return;
    }

    //Use this to hold all retrieved CustomData
    window.customDataList = [];
    $("#dwnLink").hide();
    $("#uplink").fadeIn();
    window.itemId = itemId;
    mainTabListParent = '.autoTabs';
    mainTabParent = '.customTabs';
    $("#approvalSection").hide();
    InitialiseBaseDataUi(itemId, 'tblList', 'customDataView');
}
function customA(itemId)
{
    if (!itemId || itemId.length < 1)
    {
        alert('Invalid selection');
        return;
    }

    //Use this to hold all retrieved CustomData
    window.customDataList = [];
    $("#dwnLink").hide();
    $("#uplink").fadeIn();
    window.itemId = itemId;
    mainTabListParent = '.autoTabs';
    mainTabParent = '.customTabs';
    getBaseData(itemId, 'tblList', 'baseDataTemplate', true);
}
function approvalHistory(itemId)
{
    if (!itemId || itemId.length < 1)
    {
        alert('Invalid selection');
        return;
    }
     
    window.itemId = itemId;
    $.ajax({
        url: baseUrl + '/Approval/GetApprovalHistories?enrollmentId=' + itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () { window.showUiBusy(); },
        success: function (response)
        {
            window.hideUiBusy();
            if (!response || response.TableId < 1)
            {
                alert('The requested process failed. Please try again or contact Support team');
                return;
            }

            InitialiseDataApprovalHistoryUi(response);
        }
    });

}
function deleteEnrollment(itemId)
{
    if (!itemId || itemId < 1) {
        alert('Invalid selection');
        return;
    }

    window.showUiBusy();

    $.ajax({
        url: baseUrl + '/BaseData/DeleteBaseData?baseDataTableId=' + itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'POST',
        beforeSend: function () {
            if (!confirm('Do you want to delete this item?')) {
                return;
            }
        },
        success: function (response) {
            window.hideUiBusy();
            alert(response.Message);
            if (!response || response.Code < 1) {
                return;
            }
            location.reload();
        }
    });
}
function pullOutOfRejection(itemId)
{
    if (!itemId || itemId < 1) 
    {
        alert('Invalid selection');
        return;
    }
    
    var xhr = $.ajax({
    url: baseUrl + '/BaseData/PullEnrollmentFromRejection?enrollmentId=' + itemId,
    contentType: "application/json",
    dataType: 'json',
    type: 'POST',
    beforeSend: function ()
    {
        if (!confirm('Do you want to Pull this Enrollment Out Of Rejection?'))
        {
            xhr.abort();
            return;
        }
        window.showUiBusy();
    },
    success: function (response) {
        window.hideUiBusy();
        alert(response.Message);
        if (!response || response.Code < 1)
        {
            return;
        }
        jtable.fnClearTable();
        closeBaseDataView();
    }
});
}
function editApproval(itemId)
{
    if (!itemId || itemId.length < 1)
    {
        alert('Invalid selection');
        return;
    }
         
    window.itemId = itemId;
    mainTabListParent = '.approvalAutoTabList';
    mainTabParent = '.approvalTabsParent';
    getBaseData(itemId, 'approvalTblList', 'baseDataTemplate', true);

    $("#uplink").hide();
    $("#dwnLink").show();
}
function sync(itemId)
{
    if (!itemId || itemId.length < 1)
    {
        alert('Invalid selection');
        return;
    }
    $.ajax({
        url: '/BaseData/SyncEnrollmentRecordFromDb?enrollmentId=' + itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () { window.showUiBusy(); },
        success: function (response) {
            window.hideUiBusy();
            if (!response || response.length < 1) {
                alert(response.Message);
                return;
            }

        }
    });
}

function bio(itemId)
{
    if (!itemId || itemId.length < 1)
    {
        alert('Invalid selection');
        return;
    }

    if (typeof callbackObj != 'undefined' && typeof callbackObj != null)
    {
        callbackObj.findRecord(itemId);
    }
}
function syncForm(itemId) {
    //var itemId = $(this).data("edit-id");
    if (!itemId || itemId.length < 1) {
        alert('Invalid selection');
        return;
    }
    $.ajax({
        url: '/BaseData/SyncEnrollmentDataForms?enrollmentId=' + itemId + '&projectCode=' + window.projectCode,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () { window.showUiBusy(); },
        success: function (response) {
            window.hideUiBusy();
            if (!response || response.length < 1) {
                alert(response.Message);
                return;
            }

        }
    });

}
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
    window.baseDataScope.ValidIdNumber = $("#ValidIdNumber").val();
    window.baseDataScope.DOB = $("#DOB").val();
   
    //if (window.baseDataScope.Email.length < 1)
    //{
    //    alert('Please provide Email');
    //    return;
    //}

    //var email = window.baseDataScope.Email;
   
    //if (email.indexOf('@') < 0) {
    //    alert('Please provide a valid Email');
    //    return;
    //}

    //if (email.indexOf('.') < 1)
    //{
    //    alert('Please provide a valid Email');
    //    return;
    //}

    //var splits = email.split('.');
    //var last = splits[splits.length - 1];
    //if (last.length < 1) {
    //    alert('Please provide a valid Email');
    //    return;
    //}

    //var trimed = email.replace(/\D/g, "");
    //var reg = '^[a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,15})$';
    //var matchEmail = reg.match(trimed);
    //if (!matchEmail)
    //{
    //    alert("Please provide a valid Email");
    //    return;
    //}

    //window.baseDataScope.Email = trimed;

    if (window.baseDataScope.ProjectPrimaryCode.length < 1)
    {
        alert('Please provide Project Primary');
        return;
    }

    if (window.baseDataScope.MobileNumber.length < 1)
    {
        alert('Please provide Mobile Number');
        return;
    }
    
    //var phoneRe = '^0[7-9][0-4][2-9]\d{7}$';
    //var digits = window.baseDataScope.MobileNumber.replace(/\D/g, "");
    ////var test = value.match(/\d/g);
    //var match = phoneRe.match(digits);
    //if (!match)
    //{
    //    alert("Please provide a valid Mobile phone Number");
    //    return;
    //}

    if (window.baseDataScope.Surname.length < 1) {
        alert("Please provide Surname");
        return;
    }

    var nameRegex = "^[A-Za-z\s]{2,40}$";
    var nameTrim = window.baseDataScope.Surname.replace(/\D/g, "");
    var nameMatch = nameRegex.match(nameTrim);
    if (!nameMatch) {
        alert("Invalid Surname");
        return;
    }
    if (window.baseDataScope.Firstname.length < 1) {
        alert("Please provide First Name");
        return;
    }

    var firstNameRegex = "^[A-Za-z\s]{2,40}$";
    var firstNameTrim = window.baseDataScope.Firstname.replace(/\D/g, "");
    var firstNameMatch = firstNameRegex.match(firstNameTrim);
    if (!firstNameMatch) {
        alert("Invalid First Name");
        return;
    }

    if (window.baseDataScope.MiddleName !== undefined && window.baseDataScope.MiddleName !== null && window.baseDataScope.MiddleName.trim().length > 0)
    {
        var middleNameRegex = "^[A-Za-z\s]{2,40}$";
        var middleNameTrim = window.baseDataScope.Firstname.replace(/\D/g, "");
        var middleNameMatch = middleNameRegex.match(middleNameTrim);
        if (!middleNameMatch) {
            alert("Invalid First Middle Name");
            return;
        }
    }

    if (window.baseDataScope.Gender.length < 1)
    {
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
    if (window.baseDataScope.TableId < 1)
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

            if (!response || response.Code < 1) {
                return;
            }

            jtable.fnClearTable();
            closeBaseDataView();
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
        TestColumn: ''
    };
});
function retrieveEnrollment()
{
    clearControls();
    var itemId = $('#txtEnrollment').val();
    if (!itemId || itemId.length < 1)
    {
        alert('Please provide Enrollment Id');
        return;
    }
   
    $.ajax({
        url: baseUrl + '/BaseData/GetEnrollmentInForApproval?enrollmentId=' + itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend : function (){window.showUiBusy();},
        success: function (response)
        {
            window.hideUiBusy();
            if (!response || response.EnrollmentId === null || response.EnrollmentId.length < 1)
            {
                alert('No new record is yet ready for approval');
                return;
            }

            continueRetrieval(response);
        }
    });

}
function GetNextEnrollment()
{
    $('#txtEnrollment').val('');
    clearControls();
   $.ajax({
        url: baseUrl + '/BaseData/GetNextEnrollmentInForApproval',
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend : function (){window.showUiBusy();},
        success: function (response)
        {
            window.hideUiBusy();
            if (!response || response.EnrollmentId === null || response.EnrollmentId.length < 1)
            {
                alert('No new record is yet ready for approval');
                return;
            }

            continueRetrieval(response);
        }
    });

}
function continueRetrieval(response)
{
    mainTabListParent = '.autoTabs';
    mainTabParent = '.customTabs';
    InitialiseBaseDataUi(response.EnrollmentId, null, null);

    window.baseDataScope = response;
    window.itemId = response.EnrollmentId;

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
    $("#enrolledBy").html(response.EnrollmentOfficer);
    $("#enrollmentDate").html(response.EnrollmentDateStr);
    //$("#BiometricStatus").html(response.BiometricStatus);
    
    $("#baseDataTemplate").fadeIn();
    
    var dateVar = moment(response.DOB).format("DD/MM/YYYY");
    $("#DOB").datetimepicker({
        format: 'd/m/Y', maxDate: new Date() - 1, timepicker: false,
        onSelectDate: function (dp, $input) {
            response.DOB = dp;
        }
    }).val(dateVar);

    if (response.FormPath !== undefined && response.FormPath !== null && response.FormPath.length > 0)
    {
        $("#fileDownload").attr('href', baseUrl + '/BaseData/DownloadContentFromFolder2?path=' + response.FormPath).fadeIn();
        $("#fileNotAvailable").hide();
        $("#dwnLink").fadeIn();
    }
    else
    {
        $("#fileDownload").attr('href', '').hide();
        $("#fileNotAvailable").show();
        $("#uplink").fadeIn();
    }
}
function CheckInToApprove() 
{
    if (window.itemId === undefined || window.itemId === null || window.itemId.length < 1)
    {
        alert('An error was encountered. Please try again later or contact the support team');
        return;
    }
    
   var xhr = $.ajax({
        url: baseUrl + '/BaseData/CheckEnrollmentInForApproval?enrollmentId=' + window.itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'POST',
        beforeSend: function() 
        {
            var status = confirm('Do you want to check-in this enrollment to Approve it?');
            if (status === false) {
                xhr.abort();
                return;
            }
            window.showUiBusy();
        },
        success: function (response)
        {
            window.hideUiBusy();
           
            if (!response || response.length < 1)
            {
                alert(response.Message);
                return;
            }

            $('#chekInDiv').hide();

            var str = '<div class="col-md-12"><label>'
                      + 'Leave a comment below for the Enrollment Officer if this record will be rejected with exception'
                      + '</label><textarea class="form-control" id="approvalComment" style="width: 1022px; height: 153px;"></textarea>'
                      + '</div><br /><div class="col-md-12"><div class="col-md-5">'
                      + '<button class="btn btn-danger" onclick="approveData(3)">Reject</button></div>'
                      + '<div class="col-md-3"><button class="btn btn-primary" onclick="approveData(2)">Approve</button>'
                      + '</div></div>';
            $('#fullDetails').html(str);
            $('#fullDetails').fadeIn();
        }
    });
}
function PullOutOfRejection()
{
    var itemId = window.baseDataScope.EnrollmentId;
    if (!itemId || itemId.length < 1)
    {
        alert('Invalid selection');
        return;
    }
    var xhr = $.ajax({
        url: baseUrl + '/BaseData/PullEnrollmentFromRejection?enrollmentId=' + itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'POST',
        beforeSend: function ()
        {
            var status = confirm('Are sure you want to Pull this Enrollment out of Rejection?');
            if (status === false)
            {
                xhr.abort();
                return;
            }
            window.showUiBusy();
        },
        success: function (response)
        {
            window.hideUiBusy();
            alert(response.Message);
            if (response.Code < 1)
            {
                return;
            }

            jtable.fnClearTable();
            closeBaseDataView();
        }
    });

}
function clearControls()
{
    window.customDataList = [];
    window.baseDataScope = {};
    window.itemId = '';
    $("#baseDataTemplate").hide();
    $('#fullDetails').hide().html('');
    $(".customTabs").val('');
    $(".autoTabs").val('');
    $("#EnrollmentId").val('');
    $("#ProjectPrimaryCode").val('');
    $("#confirmProjectPrimaryCode").val('');
    $("#Email").val('');
    $("#MobileNumber").val('');
    $("#Surname").val('');
    $("#CuntryCode").val('');
    $("#MiddleName").val('');
    $("#Firstname").val('');
    $("#MiddleName").val('');
    $("#ValidIdNumber").val('');
    $("#Gender").val('');
    $("#Title").val();
    $("#enrolledBy").html('');
    $("#enrollmentDate").html('');
    $("#baseDataTemplate").hide();
    $("#fileDownload").attr('href', '').hide();
    $("#fileNotAvailable").show();
    $("#DOB").val('');

}

//____________________________________________ Custom Data ____________________________________________________

var totalTabs = 0;
var cListIdds = [];
var customListDataCahe = [];
var dataLoaded = false;
function InitialiseBaseDataUi(itemId, tblList, customDataView)
{
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

            $('.customContainer').show();

            var accordionUlHeader = $(mainTabListParent);
            var tabsParent = $(mainTabParent);

            accordionUlHeader.html('');
            tabsParent.html('');

            var customGroups = data.CustomGroupViewModels;
            var length = 1;
            cListIdds = [];
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
                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label><input data-edit-id="" data-required="' + req + '" size="' + f.CustomFieldSize + '" type="text" data-type="text" class="form-control"  style="text-transform: capitalize" id="' + f.CustomFieldId + '" /></div>';
                        }

                        else if (f.CustomFieldType.FieldTypeName === 'Number')
                        {
                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label><input data-edit-id="" data-required="' + req + '" size="' + f.CustomFieldSize + '" type="number" data-type="number" class="form-control"  style="text-transform: capitalize" id="' + f.CustomFieldId + '" /></div>';
                        }

                        else if (f.CustomFieldType.FieldTypeName === 'Date') 
                        {
                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label><input data-edit-id="" data-required="' + req + '" size="' + f.CustomFieldSize + '" type="text"  data-type="date" class="form-control date"  style="text-transform: capitalize" id="' + f.CustomFieldId + '" /></div>';
                        }

                        else if (f.CustomFieldType.FieldTypeName === 'Email') 
                        {
                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label><input data-edit-id="" data-required="' + req + '" size="' + f.CustomFieldSize + '" type="email"  data-type="email" class="form-control date"  style="text-transform: capitalize" id="' + f.CustomFieldId + '" /></div>';
                        }

                        if (f.CustomFieldType.FieldTypeName === 'List' && f.CustomList !== undefined && f.CustomList !== null)
                        {
                            var qq = f.CustomList;

                            tabpanel += '<div  id="frmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' ' + req2 + '</label>';
                            
                            if (qq.HasChildren === true) 
                            {
                                tabpanel += '<select data-edit-id="" style="text-transform: capitalize" data-required="' + req + '" data-has-child="true" data-pList="' + qq.ParentListId + '" data-list="' + qq.CustomListId + '" onchange="checkChildList(this.id)" class="selectpicker form-control" id="' + f.CustomFieldId + '"><option value="0">-- Select option --</option>';
                            }
                            else
                            {
                                tabpanel += '<select data-edit-id="" style="text-transform: capitalize" data-required="' + req + '" data-has-child="false" data-required="' + f.Required + '" data-list="' + qq.CustomListId + '" class="selectpicker form-control" id="' + f.CustomFieldId + '"><option value="0">-- Select option --</option>';
                            }
                           
                            tabpanel += '</select></div>';
                            cListIdds.push({ FieldId: f.CustomFieldId, ListId: qq.CustomListId, hasChildren: qq.HasChildren });
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
                $("#fileDownload").attr('href', baseUrl + '/BaseData/DownloadContentFromFolder2?path=' + data.FormPath).fadeIn();
                $("#fileNotAvailable").hide();
                
            }
            else
            {
                $("#fileDownload").attr('href', '').hide();
                $("#fileNotAvailable").show();
            }

            $(mainTabListParent + ' li:first').addClass('active');

            $(mainTabParent + ' #tab_1').addClass('in active');

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
            window.EnrollmentId = data.EnrollmentId;
            
            //$("select").select2({
            //    allowClear: false
            //});

            $('.' + tblList).hide();
            $('#' + customDataView).fadeIn();
            getCustomListDatas(data.EnrollmentId);
        }
    });
}
function getCustomListDatas(enrollmentId)
{
    if (cListIdds.length < 1)
    {
        alert('An error was encountered on the page. Please refresh the page and try again or contact Support team');
        return;
    }

    if (dataLoaded === true)
    {
        buildCustomListDatas(enrollmentId);
        return;
    }
    customListDataCahe = [];
    $.each(cListIdds, function (i, c)
    {
        $.ajax({
            url: baseUrl + '/ProjectCustomData/GetCustomListDatas?listId=' + c.ListId,
            contentType: "application/json",
            dataType: 'json',
            type: 'GET',
            success: function (response)
            {
                if (response && response.length > 0)
                {
                    var option = '<option value="">-- Select option --</option>';
                    $.each(response, function (j, u)
                    {
                        option += '<option id="' + u.CustomListDataId + '" value="' + u.CustomListDataId + '">' + u.ListDataName + '</option>';
                        customListDataCahe.push(u);
                    });

                    $('#' + c.FieldId).html(option);
                }
            }
        });
    });

    if (customListDataCahe.length > 0)
    {
        dataLoaded = true;
    }
    getUserDatas(enrollmentId);
}
function buildCustomListDatas(enrollmentId)
{
    if (customListDataCahe.length < 1 || cListIdds.length < 1)
    {
        alert('An error was encountered on the page. Please refresh the page and try again or contact Support team');
        return;
    }

    $.each(cListIdds, function (i, c)
    {
        var dataList = customListDataCahe.filter(function (l)
        {
            return l.CustomListId === c.ListId;
        });

        if (dataList && dataList.length > 0)
        {
            var option = '<option value="">-- Select option --</option>';
            $.each(dataList, function (j, u)
            {
                option += '<option id="' + u.CustomListDataId + '" value="' + u.CustomListDataId + '">' + u.ListDataName + '</option>';
            });
            $('#' + c.FieldId).html(option);
        }
    });
    
    getUserDatas(enrollmentId);
}
function checkChildList(controlId, crimsCustomData)
{
    var tt = $('#' + controlId);
    var value = tt.val();
    if (value === undefined || value === null || value === '0' || value === 'null' || value.length < 1)
    {
        return;
    }

    $.ajax({
        url: baseUrl + '/ProjectCustomData/GetCustomListDatasByParentList?parentListId=' + tt.data('list') + '&parentNode=' + value + '&parentCustomFieldId=' + controlId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function ()
        {
            window.showUiBusy();
        },
        success: function (cList)
        {
            window.hideUiBusy();
            if (!cList || !cList.CustomListDatas || cList.CustomListDatas.length < 1)
            {
                //alert('Some Further important information information could not be retrieved. Please try again or contact Support team');
                return;
            }

            var req = cList.Required === 1 || cList.Required === '1' ? 'true' : 'false';
            var req2 = cList.Required === 1 || cList.Required === '1' ? '*' : '';
            window.customDataList.push(cList);
            if ($('#' + cList.CustomFieldId).length)
            {
                var options = '<option value="0">-- Select option --</option>';
                cList.CustomListDatas.forEach(function (c, i)
                {
                    options += '<option id="' + c.CustomListDataId + '" value="' + c.CustomListDataId + '">' + c.ListDataName + '</option>';
                });

                $('#' + cList.CustomFieldId).html(options);
            }
            else
            {
                var newFormGroup = '<div style="display: none; margin-bottom: 10px" class="col-md-6" id="frmGrp_' + cList.CustomFieldId + '"><label>' + cList.CustomFieldName + ' ' + req2 + '</label>';

                var qq = cList;
                if (qq.HasChildren === true)
                {
                    newFormGroup += '<select data-edit-id="" data-pFId="' + controlId + '" data-required="' + req + '" data-has-child="true" data-required="' + cList.Required + '" data-list="' + qq.CustomListId + '" onchange="checkChildList(this.id)" class="form-control" id="' + cList.CustomFieldId + '"><option value="0">-- Select option --</option>';
                }
                else
                {
                    newFormGroup += '<select data-edit-id="" data-pFId="' + controlId + '" data-required="' + req + '" data-has-child="false" data-list="' + qq.CustomListId + '" class="form-control" id="' + cList.CustomFieldId + '"><option value="0">-- Select option --</option>';
                }

                qq.CustomListDatas.forEach(function (c, i) {
                    newFormGroup += '<option id="' + c.CustomListDataId + '" value="' + c.CustomListDataId + '">' + c.ListDataName + '</option>';
                });

                newFormGroup += '</select></div>';

                $('#frmGrp_' + tt.attr('id')).after(newFormGroup);

                //$("#" + cList.CustomFieldId).select2({
                //    allowClear: false
                //});

                $('#frmGrp_' + cList.CustomFieldId).fadeIn();
            }
           
            //Check if this child List already has a data previously selected
            setChildListValue(cList.CustomFieldId, crimsCustomData);
            
        }
    });
}
function setChildListValue(listFieldId, crimsCustomData)
{
    if (window.customDataList === undefined || window.customDataList === null || window.customDataList.length < 1 || listFieldId === undefined || listFieldId === null || listFieldId.length < 1) //window.customDataList === undefined || window.customDataList === null || window.customDataList.length < 1 ||
    {
        return;
    }
    var dataControl = $('#' + listFieldId);

    if (dataControl !== undefined && dataControl !== null)
    {
        var data = window.customDataList.filter(function (o)
        {
            return o.CustomFieldId === listFieldId;
        });
        if (crimsCustomData !== undefined && crimsCustomData !== null && crimsCustomData.length > 0)
        {
            data = data.filter(function (o)
            {
                return o.CrimsCustomData === crimsCustomData;
            });
            if (data.length > 0)
            {
                dataControl.val(crimsCustomData).attr('data-edit-id', data[0].CustomDataId);
                if (data[0].HasChildren === true)
                {
                    checkChildList(listFieldId);
                }
            }
        } else {
            var childData = window.customDataList.filter(function (q)
            {
                return q.ParentListId === data[0].ParentListId;
            });

            if (childData.length > 0)
            {
                dataControl.val(childData[0].CrimsCustomData).attr('data-edit-id', childData[0].CustomDataId);
                if(childData[0].HasChildren === true)
                {
                    checkChildList(listFieldId);
                }
            }
        }
        
    }
    else
    {
        alert('Control not found : ' + listFieldId);
    }
}
function getBaseData(itemId, tblList, customDataView, getUiData)
{
    window.showUiBusy();
    
    $.ajax({
    url: baseUrl + '/BaseData/GetEnrollmentInfo?enrollmentId=' + itemId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        success: function (response) {
            window.hideUiBusy();
            if (!response || response.TableId < 1) {
                alert('Base Data information could not be retrieved. Please try again or contact Support team');
                return;
            }

            if (getUiData === true)
            {
                InitialiseBaseDataUi(response.EnrollmentId, tblList, customDataView);
            }

            window.baseDataScope = response;
           
            $("#EnrollmentId").html(response.EnrollmentId);
            $("#sEnrollmentId").html(response.EnrollmentId);
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
            $("#enrolledBy").html(response.EnrollmentOfficer);
            $("#enrollmentDate").html(response.EnrollmentDateStr);
            
            var dateVar = moment(response.DOB).format("DD/MM/YYYY");
            $("#DOB").datetimepicker({
                format: 'd/m/Y', maxDate: new Date() - 1, timepicker: false,
                onSelectDate: function (dp, $input)
                {
                    response.DOB = dp;
                }
            }).val(dateVar);
            
            if (tblList !== undefined && tblList !== null && tblList.length > 0)
            {
                $('.' + tblList).hide();
            }
           
            if (customDataView !== undefined && customDataView !== null && customDataView.length > 0)
            {
                $("#" + customDataView).fadeIn();
            }
        }
    });
}
function closeCustomDataView() {
    $('#customDataView').hide();
    $('.tblList').fadeIn();
    $('.autoTabs').html('');
    $('.customTabs').html('');
}
function closeApprovalView()
{
    $('#approvalView').hide();
    $('.tblList').fadeIn();

    $('.approvalAutoTabs').html('');
    $('.approvalTabsParent').html('');
}
function closeApprovalHistoryView()
{
    $('#historyView').hide();
    $('.approvalTblList').fadeIn();
}
function closeBiodataView() {
    $('#baseDataTemplate').hide();
    $('.approvalTblList').fadeIn();
    $('#baseContainer').addClass('active');
    $('#custContainer').removeClass('active');
     
}
function closeBaseDataView() {
    $('#baseDataTemplate').hide();
    $('.tblList').fadeIn();
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
    
    //data-type="date"
    //data-type="number"
    //data-type="text"
    //data-type="email"

    var customDatas = [];

    if (selectList.length > 0)
    {
        for (var i = 0; i < selectList.length; i++)
        {
            var customDataControl = $(selectList[i]);
            var required = customDataControl.data('required');
            var haschild = customDataControl.data('has-child');
            var customData = customDataControl.val();
            
            if (required === true || required === 'true')
            {
                if (customData === undefined || customData === null || customData === '0' || customData === '')
                {
                    alert('Please provide/select all required fields and try again');
                    return;
                }
            }

            var cData =
             {
                CustomDataId: customDataControl.data('edit-id'),
                CustomFieldId: customDataControl.attr('id'),
                EnrollmentId: $('#EnrollmentId').html(),
                CrimsCustomData: customData,
                CustomListId: customDataControl.data('list'),
                ProjectSIteId: $('#ProjectPrimaryCode').html(),
                IsRequired: required
            };

            if (haschild !== undefined && haschild !== null && (haschild === 'true' || haschild === true))
            {
                var childControl = $('[data-pFId="' + customDataControl.attr('id') + '"]');
                if (childControl !== undefined && childControl !== null && childControl.length > 0)
                {
                    var childValue = childControl.val();
                    if (childValue !== undefined && childValue !== null && childValue !== '0')
                    {
                        cData.ChildCrimsCustomData = childValue;
                    }
                }
            }
           customDatas.push(cData);
        }
    }

    if (textList.length > 0) {
        for (var j = 0; j < textList.length; j++) 
        {
            var textDataControl = $(textList[j]);
            var tetxtRequired = textDataControl.data('required');
            var textData = textDataControl.val().trim();
            var type = textDataControl.data('type');
            var size = textDataControl.attr('size');

            if (tetxtRequired === true || tetxtRequired === 'true') {
                if (textData === undefined || textData === null || textData.length < 1 || textData === '') {
                    alert('Please provide/select all required fields and try again');
                    return;
                }
                if (type === 'email') 
                {
                    var emailRegex = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                    var trimed = textData.replace(/\D/g, "");
                    //var test = value.match(/\d/g);
                    var matchMail = emailRegex.match(trimed);
                    if (!matchMail) 
                    {
                        alert("Please provide a valid Email");
                        return;
                    }
                }

                if (type === 'number') 
                {
                    var trimedNum = textData.replace(/\D/g, "");
                    //var test = value.match(/\d/g);
                    var regex=/^[0-9]+$/;
                    if (!trimedNum.match(regex))
                    {
                        alert("Please ensure valid input");
                        return;
                    }
                }
            }

            customDatas.push({
                CustomDataId: textDataControl.data('edit-id'),
                CustomFieldId: textDataControl.attr('id'),
                EnrollmentId: $('#EnrollmentId').html(),
                CrimsCustomData: textData,
                CustomListId: null,
                ProjectSIteId: $('#ProjectPrimaryCode').html(),
                IsRequired: tetxtRequired
            });
        }
    }

    if (textAreaList.length > 0) {
        for (var k = 0; k < textAreaList.length; k++) {
            var textAreaDataControl = $(textAreaList[k]);
            var textAreaRequired = textAreaDataControl.data('required');
            var textAreaData = textAreaDataControl.val().trim();

            if (textAreaRequired === true || textAreaRequired === 'true') {
                if (textAreaData === undefined || textAreaData === null || textAreaData.length < 1 || textAreaData === '') {
                    alert('Please provide/select all required fields and try again');
                    return;
                }
            }

            customDatas.push({
                CustomDataId: textAreaDataControl.data('edit-id'),
                CustomFieldId: textAreaDataControl.attr('id'),
                EnrollmentId: $('#EnrollmentId').html(),
                CrimsCustomData: textAreaData,
                CustomListId: null,
                ProjectSIteId: $('#ProjectPrimaryCode').html(),
                IsRequired: textAreaRequired
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
function getUserDatas(enrollmentId)
{
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
            populateCustomDataUi(response);
        }
    });
}
function populateCustomDataUi(data)
{
    if (!data || data.length < 1) 
    {
        return;
    }

    getBioElements();
    
    $.each(data, function (i, o)
    {
        var dataControl = $('#' + o.CustomFieldId);
        if (dataControl !== undefined && dataControl !== null && dataControl.length > 0)
        {
            //Set the previously selected data and set 
            //the CustomDataId to data-edit-id for Editing

            if (o.CustomListId === undefined || o.CustomListId === null || o.CustomListId.length < 1)
            {
                dataControl.val(o.CrimsCustomData).attr('data-edit-id', o.CustomDataId);
            }
            else
            {
                if (o.ParentListId === undefined || o.ParentListId === null || o.ParentListId.length < 1)
                {
                    dataControl.val(o.CrimsCustomData).attr('data-edit-id', o.CustomDataId);
                    //$('#' + dataControl.attr('id') + ' option#' + o.CrimsCustomData).attr('selected', 'selected');
                  
                    if (o.HasChildren === true)
                    {
                      checkChildList(o.CustomFieldId, o.ChildCrimsCustomData);
                    }
                }
            }
        }
        

    });
}
function getBioElements()
{
    $.ajax({
        url: baseUrl + '/Enrollment/GetBiometricData?enrollmentId=' + window.EnrollmentId,
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

            $('#photo').attr('src', response.Photo);
            $('#lLittle').attr('src', response.LeftLittle);
            $('#lRing').attr('src', response.LeftRing);
            $('#lMiddle').attr('src', response.LeftMiddle);
            $('#lIndex').attr('src', response.LeftIndex);
            $('#lThumb').attr('src', response.LeftThumb);
            $('#rThumb').attr('src', response.RightThumb);
            $('#rIndex').attr('src', response.RightIndex);
            $('#rMiddle').attr('src', response.RightMiddle);
            $('#rRing').attr('src', response.RightRing);
            $('#rLittle').attr('src', response.RightLittle);
            $('#signature').attr('src', response.Signature);
           
            $('#bioDiv').fadeIn();
            
        }
    });
}
function hexToBase64(str)
{
    return 'data:image/jpeg;base64,' + btoa(String.fromCharCode.apply(null, str.replace(/\r|\n/g, "").replace(/([\da-fA-F]{2}) ?/g, "0x$1 ").replace(/ +$/, "").split(" ")));
}
$('#DOB').datetimepicker({
    format: 'd/m/Y',
    maxDate: new Date() - 1,
    timepicker: false,
    onSelectDate: function (dp, $input) {
        window.baseDataScope.DOB = dp;
    }
});
$(document).ready(
    function ()
    {
        //Upload DataForm
        $('#dataForm')
            .on('change',
                function(e) {
                    var dataFile = e.target.files[0];
                    if (dataFile !== undefined && dataFile !== null && dataFile.size > 0) {
                        if (window.FormData !== undefined) {
                            var data = new FormData();
                            data.append(dataFile.name, dataFile);

                            $.ajax({
                                type: "POST",
                                url: baseUrl +
                                    '/BaseData/UploadFile?enrollmentId=' +
                                    window.itemId +
                                    '&projectCode=' +
                                    window.projectCode,
                                contentType: false,
                                dataType: 'json',
                                processData: false,
                                data: data,
                                beforeSend: function() { window.showUiBusy(); },
                                success: function(response) {
                                    window.hideUiBusy();
                                    alert(response.Message);
                                    if (response.Code < 1) {
                                        return;
                                    }
                                    window.baseDataScope.FormPath = response.FilePath;
                                },
                                error: function(xhr) {
                                    var err = JSON.parse(xhr.responseText).Message;
                                    alert(err);
                                }
                            });
                        } else {
                            alert("This browser doesn't support HTML5 file uploads!");
                        }
                    }

                });

        //Data Import
        $('#importFile')
            .on('change',
                function(e) {
                    var licenseFile = e.target.files[0];
                    if (licenseFile !== undefined && licenseFile !== null && licenseFile.size > 0) {
                        if (window.FormData !== undefined) {
                            var data = new FormData();
                            data.append(licenseFile.name, licenseFile);

                            $.ajax({
                                type: "POST",
                                url: baseUrl + '/BaseData/ImportData',
                                contentType: false,
                                dataType: 'json',
                                processData: false,
                                data: data,
                                beforeSend: function() { window.showUiBusy(); },
                                success: function(response) {
                                    window.hideUiBusy();
                                    alert(response.Message);
                                },
                                error: function(xhr) {
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

        $('#newLicense')
        .on('change',
            function (e) {
                var licenseFile = e.target.files[0];
                if (licenseFile !== undefined && licenseFile !== null && licenseFile.size > 0)
                {
                    if (window.FormData !== undefined) {
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
                            success: function (response) {
                                window.hideUiBusy();
                                alert(response.Message);
                                if (response.Code < 1) {
                                    return;
                                }
                                window.location.href = baseUrl + "/Home/ProjectOptions";
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
            });
    });

//_________________________________________________________ Approvals _______________________________________________

function InitialiseDataApprovalUi(data)
{
    if (!data || data.CustomGroupViewModels.length < 1)
    {
        alert('An unknown error was encountered. Please refresh the page and try again.');
        return;
    }

    window.EnrollmentId = data.EnrollmentId;
    $('#EnrollmentId').html(data.EnrollmentId);
    $('#iProjectPrimaryCode').html(data.ProjectPrimaryCode);
    $('#Name').html(data.Name);

    if (data.FormPath !== undefined && data.FormPath !== null && data.FormPath.length > 0)
    {
        $("#fileDownload").attr('href', baseUrl + '/BaseData/DownloadContentFromFolder2?path=' + data.FormPath).fadeIn();
        $("#fileNotAvailable").hide();

    }
    else
    {
        $("#fileDownload").attr('href', '').hide();
        $("#fileNotAvailable").show();
    }

    //$('#approvalEnrollmentId').html(data.EnrollmentId);
    //$('#approvalProjectPrimaryCode').html(data.ProjectPrimaryCode);
    //$('#approvalName').html(data.Name);

    //var accordionUlHeader = $('.approvalAutoTabs');
    var tabsParent = $('.appDiv');

    getBioElements();

    //accordionUlHeader.html('');
    tabsParent.html('');

    var customGroups = data.CustomGroupViewModels;
    var length = 1;
    var parentLists = [];

    $.each(customGroups, function (i, o)
    {
        //var li = '';
        //accordionUlHeader.append(li);

        var tabpanel = '<div class="col-md-12" id="appTab_' + length + '" style="margin-top: 3px"><div class="row"><div class="col-md-12"><h4 role="presentation">' + o.GroupName + '</h4></div>';
        var customFields = o.CustomFieldViewModels;
        if (customFields !== undefined && customFields !== null && customFields.length > 0)
        {
            $.each(customFields, function (j, f)
            {
                if (f.CustomListId !== undefined && f.CustomListId !== null && f.CustomListId.length > 0 && f.CustomListId !== 'null' && f.CustomListId !== '0')
                {
                    tabpanel += '<div  id="appFrmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' </label> : <h4 id="' + f.CustomFieldId + '">' + f.CustomListDataName + '</h4></div>';
                    if (f.HasChildren === true)
                    {
                        parentLists.push({ ParentListId: f.CustomListId, ParentNode: f.CrimsCustomData, CustomDataId: f.CustomDataId, PrecedingField: f.CustomFieldId });
                    }
                }
                else
                {
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

    $('.approvalAutoTabs li:first').addClass('active');

    $('div[id^="appTab_"]:first').addClass('in active');

    if (parentLists.length > 0)
    {
        checkChildApprovalList(parentLists);
    }
    $('#chekInDiv').show();
    $('.tblList').hide();
    $("#baseDataTemplate").hide();
    $('#customContainer').hide();
    $('#approvalView').fadeIn(); 
    $('#customDataView').fadeIn();
    
}
function checkChildApprovalList(parentList)
{
    if (parentList === undefined || parentList === null || parentList.length < 1)
    {
        return;
    }
    
    $.ajax({
        url: baseUrl + '/Approval/GetCustomListApprovalDataByParentList',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({parentModels: parentList }),
        type: 'POST',
        beforeSend: function ()
        {
            window.showUiBusy();
        },
        success: function (customFields)
        {
            window.hideUiBusy();
            if (!customFields || customFields.length < 1)
            {
                alert('Some Further important information information could not be retrieved. Please try again or contact Support team');
                return;
            }

            parentList = [];

            $.each(customFields, function (j, f)
            {
                var tabpanel = '';
               
                if (f.CustomListId !== undefined && f.CustomListId !== null && f.CustomListId.length > 0 && f.CustomListId !== 'null')
                {
                    tabpanel += '<div  id="appFrmGrp_' + f.CustomFieldId + '" class="col-md-6" style="margin-bottom: 10px"><label>' + f.CustomFieldName + ' </label> : <h4 id="' + f.CustomFieldId + '">' + f.CustomListDataName + '</h4></div>';

                    if (f.HasChildren === true)
                    {
                        parentList.push({ ParentListId: f.CustomListId, ParentNode: f.CrimsCustomData, CustomDataId: f.CustomDataId, PrecedingField: f.CustomFieldId });
                    }
                }
                $('#appFrmGrp_' + f.PrecedingField).after(tabpanel);

                $('#appFrmGrp_' + f.PrecedingField).fadeIn();
            });

            if (parentList.length > 0)
            {
                checkChildApprovalList(parentList);
            }
            
        }
    });
}
function InitialiseDataApprovalHistoryUi(data)
{
    console.log(data);
    if (!data || data.ApprovalHistories.length < 1)
    {
        alert('An unknown error was encountered. Please refresh the page and try again.');
        return;
    }
      
     $('#commentEnrollmentId').html(data.EnrollmentId);
     $('#commentProjectPrimaryCode').html(data.ProjectPrimaryCode);
    var middleName = data.MiddleName !== undefined && data.MiddleName !== null ? data.MiddleName + ' ' : '';
    var name = data.Surname + ' ' + middleName + data.Firstname;
    $('#commentName').html(name);

     var approvalHistoryUl = $('.historyTabs');

    approvalHistoryUl.html('');

    var histories = data.ApprovalHistories;
    var historyli = '';
    $.each(histories, function (i, o)
    {
        historyli += '<li><div class="row" style="margin-bottom: 11px"><div class="col-sm-3"><label>Date :</label></div><div class="col-sm-8"><label>' + o.DateProcessedStr + '</label>' + '</div></div><div class="row" style="margin-bottom: 11px"><div class="col-sm-3"><label>Processed by :</label></div>' + '<div class="col-sm-8"><label>' + o.ProcessedByName + '</label></div></div><div class="row" style="margin-bottom: 11px">' + '<div class="col-sm-3"><label>Status :</label></div><div class="col-sm-8"><label style="color: #008000">' + o.ApprovalStatusStr + '</label></div>' + '</div><div class="row" style="margin-bottom: 11px"><div class="col-md-12"><label>Comment :</label>' + '<p>' + o.Comment + '</p></div></div></li>';

    });

    approvalHistoryUl.append(historyli);
    
    $('.approvalTblList').hide();
    $("#biodataTemplate").hide();
    //$('#customDataView').hide();
    $('#historyView').fadeIn();
}
function approveData(indicator)
{
    var payLoad = {
        TableId: 0,
        ApprovalId: '',
        Comment: $('#approvalComment').val(),
        EnrollmentId: window.EnrollmentId,
        DateProcessed: '',
        ProcessedById: '',
        ApprovalStatus: indicator
    };
    
    $.ajax({
    url: baseUrl + '/Approval/ProcessApproval',
        contentType: "application/json",
        dataType: 'json',
        type: 'POST',
        data: JSON.stringify({ approval: payLoad }),
        beforeSend: function () { window.showUiBusy(); },
        success: function (response)
        {
            window.hideUiBusy();
            alert(response.Message);
            if (!response || response.length < 1)
            {
                return;
            }

            $('#txtEnrollment').val('');
            jtable.fnClearTable();
            $('.tblList').show();
            clearControls();
        }
    });
}