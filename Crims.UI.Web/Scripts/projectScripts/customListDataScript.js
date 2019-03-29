var customListDataScope =
           {
               TableId: 0,
               ListDataName: '',
               CustomListId: '',
               CustomListDataId: ''
           };

//var baseUrl = '';
var baseUrl = '/crims';

var isEdit = false;
var selectedOptionId = 0;

$('#CustomListId').val(0);
$('#ListDataName').val('');
        
var tagInputs = [];
var listOptions = '<option value="">-- Select option --</option>';

$(function ()
{

    getCustomLists();
            
    $(".editBtn").click(function () 
    {
        var itemId = $(this).data("edit-id");
        if (itemId < 1) 
        {
            alert('Invalid selection');
            return;
        }
        $('.showBusy').fadeIn();

        $.ajax({
            url: baseUrl + '/CustomListData/GetCustomListData?id=' + itemId,
            contentType: "application/json",
            dataType: 'json',
            type: 'GET',
            success: function (response) {
                $('.showBusy').fadeOut();
                if (!response || response.TableId < 1) {
                    alert('Custom List information could not be retrieved. Please try again or contact Support team');
                    return;
                }

                $("#process_modal_body").load(baseUrl + '/CustomListData/GetView', function ()
                {
                    customListDataScope = response;
                   
                    $("#CustomListId").val(customListDataScope.CustomListId);
                    $("#myModalLabel").html('Update Custom List Data');
                    $("#ListDataName").val(response.ListDataName);
                    $("#tableId").val(response.TableId);
                });
            }
        });

    });
    //Load the delete page
    $(".deleteBtn").click(function () {
        var itemId = $(this).data("delete-id");
        if (!itemId || itemId < 1) {
            alert('Invalid selection');
            return;
        }

        $('.showBusy').fadeIn();

        $.ajax({
            url: baseUrl + '/CustomListData/DeleteCustomListData?customListDataTableId=' + itemId,
            contentType: "application/json",
            dataType: 'json',
            type: 'POST',
            beforeSend: function () {
                if (!confirm('Do you want to delete this item?')) {
                    return;
                }
            },
            success: function (response) {
                $('.showBusy').fadeOut();
                alert(response.Message);
                if (!response || response.Code < 1)
                {
                    return;
                }
                location.reload();
            }
        });
    });
});

function getCustomLists()
{
    $.ajax({
        url: baseUrl + '/CustomListData/GetCustomLists',
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        success: function (response)
        {
            if (response.length > 0)
            {
                $.each(response, function (i, list)
                {
                    listOptions += '<option data-plist="' + list.ParentListId + '" value="' + list.CustomListId + '">' + list.CustomListName + '</option>';
                });

                $('#CustomListId').html(listOptions);
            }
        }
    });
}
        
function addCustomListData()
{
    var cList = $('#CustomListId');
    var dataLength = $('#CustomListId option').length + 1;
    var data = $('#ListDataName').val();
    if (!cList || cList.val().length < 1 || cList.val() === "0") {
        alert('Please select a Custom List');
        return;
    }

    if (!data || data.length < 1)
    {
        alert('Please provide a Custom List Data');
        return;
    }

    if (isEdit === true)
    {
        var dataToEdit = $('#CustomListDataId  #' + selectedOptionId);
        var replacement = '<option data-pnode="' + $('#ParentNodeId').val() + '" data-list-id="' + $('#CustomListId').val() + '" id="' + selectedOptionId + '" value="' + dataToEdit.val() + '">' + data + '</option>';
        dataToEdit.replaceWith(replacement);

        selectedOptionId = 0;
        isEdit = false;
        $('#ListDataName').val('');
        //$('#ParentNodeId').val('');
        //$('#CustomListId').val('');
        $('#btnAddData').text('Add List Data');
        return;
    }

    $('#CustomListDataId').append('<option data-pnode="' + $('#ParentNodeId').val() + '" data-list-id="' + cList.val() + '" id="' + dataLength + '" value="">' + data + '</option>');

    $('#ListDataName').val('');
}

function deleteSelected()
{
    var cListDataIds = [];
    var cListData = [];

    $('#CustomListDataId option:selected').each(function ()
    {
        var dataId = $(this).val();
        if (dataId && dataId !== "0" && dataId.length > 0)
        {
            cListDataIds.push(dataId);
        }
        cListData.push($(this).val());
    });

    if (cListData.length < 1)
    {
        alert('Requested operation could not be completed. Please try again later');
        return;
    }

    //if the CustomListDataIds is empty => all are newly added items
    if (cListDataIds.length < 1)
    {
        $('#CustomListDataId option:selected').remove();
        return;
    }

    $.ajax({
        url: baseUrl + '/CustomListData/DeleteCustomListDataList',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ customListDataIds: cListDataIds }),
        type: 'POST',
        beforeSend: function ()
        {
            if (!confirm('Are you sure you want to delete selected Item(s)?'))
            {
                return;
            }
            $('.showBusy').fadeIn();
        },
        success: function (response)
        {
            $('.showBusy').fadeOut();

            alert(response.Message);

            if (!response || response.Code < 1)
            {
                return;
            }

            $('#CustomListDataId option:selected').remove();
        }
    });
}

function getListData()
{
    var selected = $('#CustomListId');
    var customListId = selected.val();
    if (!customListId || customListId.length < 1 || customListId === "0" || customListId === 'a8d0b7aefc4342f7b566618576beaa9f' || selected.text() === 'Not Applicable') {
        $('#CustomListId').val('0');
        alert('Invalid selection');
        return;
    }
    
    $.ajax({
        url: baseUrl + '/CustomListData/GetCustomListDataByCustomList?customListId=' + customListId + '&parentCustomListId=' + $('#CustomListId option:selected').data('plist'),
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () { $('.showBusy').fadeIn(); },
        success: function (response)
        {
            $('.showBusy').fadeOut();
            $('#CustomListDataId').html('');
           
            if (!response)
            {
                return;
            }
            
            if (response.CustomListDatas.length > 0)
            {
                var sr = '';
                var dataLength = response.CustomListDatas.length;
                response.CustomListDatas.forEach(function (o, i)
                {
                    sr += '<option data-pnode="' + o.ParentNodeId + '" data-list-id="' + o.CustomListId + '" id="' + dataLength + '" value="' + o.CustomListDataId + '">' + o.ListDataName + '</option>';
                    dataLength++;
                });
                if (sr.length > 0)
                {
                    $('#CustomListDataId').html(sr);
                }
            }
            
            if (response.ParentListData.length > 0)
            {
                var sr2 = '<option value="">-- Select option --</option>';
                var l2 = response.ParentListData.length;
                response.ParentListData.forEach(function (o, i)
                {
                    sr2 += '<option data-pnode="' + o.ParentNodeId + '" data-list-id="' + o.CustomListId + '" id="' + l2 + '" value="' + o.CustomListDataId + '">' + o.ListDataName + '</option>';
                    l2++;
                });
                if (sr2.length > 0)
                {
                    $('#ParentNodeId').html(sr2);
                }
            }

            else
            {
                var srx = '<option value="">-- Select option --</option>';
                $('#ParentNodeId').html(srx);
            }
        }
    });
}
     
function processCustomListData()
{
          
    var cListData = [];
    $('#CustomListDataId option').each(function ()
    {
        cListData.push({
            TableId: 0,
            ListDataName: $(this).text(),
            CustomListId: $(this).data('list-id'),
            ParentNodeId: $(this).data('pnode'),
            CustomListDataId: $(this).val()
        });
    });

    if (cListData.length < 1)
    {
        alert('Custom List Data could not be processed. Please try again later');
        return;
    }

    $.ajax({
        url: baseUrl + '/CustomListData/ProcessCustomListData',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({ listDataList: cListData }),
        type: 'POST',
        beforeSend: function () { $('.showBusy').fadeIn(); },
        success: function (response)
        {
            $('.showBusy').fadeOut();

            alert(response.Message);

            if (!response || response.Code < 1)
            {
                return;
            }

            location.reload();
        }
    });
}

$("#ListDataName").keypress(function (event)
{
    if (event.keyCode === 13)
    {
        event.preventDefault();
        $("#btnAddData").trigger('click');
    }
});
       
$('#CustomListDataId').on('dblclick', 'option:selected', function (e)
{
    var t = e.target;
    if (t)
    {
        isEdit = true;
        selectedOptionId = $(t).attr('id');
        $('#btnAddData').text('Update List Data');
        $('#ListDataName').val($(t).text());
        $('#CustomListId').val($(t).data('list-id'));
        var pNode = $(t).data('pnode');
        if (pNode === undefined || pNode === null || pNode === '0' || pNode.length < 1)
        {
            $('#ParentNodeId').val("");
            
        }
        else
        {
            $('#ParentNodeId').val(pNode);
        }
    }
            
});

//____________________________ BULK UPPLOAD _________________________________

var bulkOutput = [];

function handleExcelCompleted(output)
{
    if (output === undefined || output === null)
    {
        alert('The File could not be processed. Please try again');
        return;
    }

    var cListId = $('#CustomListId').val();
    var pNodeVal = $('#ParentNodeId').val();
    var pNode = pNodeVal !== undefined && pNodeVal !== null && pNodeVal.length > 0 ? pNodeVal : "";
    var result = output.listdata;

    $.each(result, function (i, o)
    {
        bulkOutput.push({
            TableId: 0,
            ListDataName: o.Custom_List_Data_Name,
            ParentData : o.Parent_Data,
            CustomListId: cListId,
            ParentNodeId: pNode,
            CustomListDataId: ""
        });
    });

    if (bulkOutput.length < 1)
    {
        alert('Custom List Data could not be processed. Please try again later');
        return;
    }

    var groupedArr = createGroupedArray(bulkOutput, 30);
    
    if (groupedArr.length < 1)
    {
        alert('Custom List Data could not be processed. Please try again later');
        return;
    }

    $.each(groupedArr, function (i, o)
    {
        $.ajax({
            url: baseUrl + '/CustomListData/ProcessCustomListData',
            contentType: "application/json",
            dataType: 'json',
            data: JSON.stringify({ listDataList: o }),
            type: 'POST',
            beforeSend: function () { $('.showBusy').fadeIn(); },
            success: function (response)
            {
                if (response.Code < 1)
                {
                    alert(response.Message);
                }
                else
                {
                    if (i === groupedArr.length - 1)
                    {
                        $('.showBusy').fadeOut();
                        alert(response.Message);
                        location.reload();
                    }
                }
                
            }
        });
      
    });

};

var createGroupedArray = function (arr, chunkSize) {
    var groups = [], i;
    for (i = 0; i < arr.length; i += chunkSize)
    {
        groups.push(arr.slice(i, i + chunkSize));
    }
    return groups;
}


function bulkDataUpload(excel)
{
    var cList = $('#CustomListId');
    if (!cList || cList.val().length < 1 || cList.val() === "0")
    {
        alert('Please select a Custom List');
        return;
    }

    if (excel == null || excel.size < 1)
    {
        alert("ERROR: Please select a valid template for bulk upload");
        el.files = null;
        return;
    }

    readExcelToJson(excel, handleExcelCompleted);
};


