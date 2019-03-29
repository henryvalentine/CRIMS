var baseUrl = window.baseUrl;

var customListDataScope =
           {
               TableId: 0,
               ListDataName: '',
               CustomListId: '',
               CustomListDataId: ''
           };


var isEdit = false;
var selectedOptionId = 0;

$('#CustomListId').val(0);
$('#ListDataName').val('');
        
var tagInputs = [];
var listOptions = '<option value="0">-- Select option --</option>';

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
        window.showUiBusy();

        $.ajax({
            url: baseUrl + '/CustomListData/GetCustomListData?id=' + itemId,
            contentType: "application/json",
            dataType: 'json',
            type: 'GET',
            success: function (response) {
                window.hideUiBusy();
                if (!response || response.TableId < 1) {
                    alert('Custom List information could not be retrieved. Please try again or contact Support team');
                    return;
                }

                $("#process_modal_body").load(baseUrl + '/CustomListData/GetView', function () {
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

        window.showUiBusy();

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
                window.hideUiBusy();
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
                    listOptions += '<option value="' + list.CustomListId + '">' + list.CustomListName + '</option>';
                });

                $('#CustomListId').html(listOptions);
            }
        }
    });
}
        
function addCustomListData()
{
    if (isEdit === true)
    {
        var dataToEdit = $('#CustomListDataId  #' + selectedOptionId);
        dataToEdit.text($('#ListDataName').val());
        selectedOptionId = 0;
        isEdit = false;
        $('#ListDataName').val('');
        $('#btnAddData').text('Add List Data');
        return;
    }

    var cList = $('#CustomListId option:selected');
    var dataLength = $('#CustomListId option').length + 1;
    var data = $('#ListDataName').val();
    if (!cList || !cList.val() || cList.val().length < 1 || cList.val() === "0")
    {
        alert('Please select a Custom List');
        return;
    }

    if (!data || data.length < 1)
    {
        alert('Please provide a Custom List Data');
        return;
    }

    $('#CustomListDataId').append('<option data-list-id="0" id="' + dataLength + '" value="' + cList.val() + '">' + data + '</option>');

    $('#ListDataName').val('');
}

function deleteSelected()
{
    var cListDataIds = [];
    var cListData = [];

    $('#CustomListDataId option:selected').each(function ()
    {
        var dataId = $(this).data('data-list-id');
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
        beforeSend: function () { window.showUiBusy(); },
        success: function (response)
        {
            window.hideUiBusy();

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
        url: baseUrl + '/CustomListData/GetCustomListDataByCustomList?customListId=' + customListId,
        contentType: "application/json",
        dataType: 'json',
        type: 'GET',
        beforeSend: function () { window.showUiBusy(); },
        success: function (response)
        {
            window.hideUiBusy();
            $('#CustomListDataId').html('');
            var dataLength = response.length;
            if (!response || response.length < 1)
            {
                return;
            }

            var sr = '';

            response.forEach(function (o, i)
            {
                sr += '<option data-list-id="' + o.CustomListDataId + '" id="' + dataLength + '" value="' + o.CustomListId + '">' + o.ListDataName + '</option>';
                dataLength++;
            });

            if (sr.length > 0)
            {
                $('#CustomListDataId').html(sr);
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
            CustomListId: $(this).val(),
            CustomListDataId: $(this).data('list-id')
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
        beforeSend: function () { window.showUiBusy(); },
        success: function (response)
        {
            window.hideUiBusy();

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
       
$('#CustomListDataId').on('dblclick', 'option:selected', function (e) {
    var t = e.target;

    if (t)
    {
        isEdit = true;
        selectedOptionId = $(t).attr('id');
        $('#btnAddData').text('Update List Data');
        $('#ListDataName').val($(t).text());
        $('#CustomListId').val($(t).val());
    }
            
});