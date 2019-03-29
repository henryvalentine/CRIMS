

function tableManager(baseUrl, tableDirective, tableOptions, edit, custom, bio, addButtonLabel) {

    var columnOptions =
        [
            {
                "sName": tableOptions.itemId,
                "bSearchable": false,
                "bSortable": false
            }
            
    ];
    
    $.each(tableOptions.columnHeaders, function (i, e)
    {
        columnOptions.push({ 'sName': e });
    });
                     
        var jTable = tableDirective.dataTable({
            dom: '<"row"<"#topContainer.col-md-12"<"col-md-4"l><"col-md-4"f><"#newItemLnk.col-md-4">>>rt<"#bttmContainer.row"<"col-md-12"<"col-md-4"><"col-md-8"p>>>',
            "bServerSide": true,
            sAjaxSource: tableOptions.sourceUrl,
            "bProcessing": true,
            "bPaginate": true,
            "language": {
                "lengthMenu": 'Items per Page<select id="pgLenghtInfo">' +
                    '<option value="10">10</option>' +
                    '<option value="20">20</option>' +
                    '<option value="30">30</option>' +
                    '<option value="40">40</option>' +
                    '<option value="50">50</option>' +
                    '<option value="100">100</option>' +
                    '</select><br/>'
            },
            "sPaginationType": "full_numbers",
            aoColumns: columnOptions,
            fnRowCallback: function (nRow, aData, iDisplayIndex)
            {
                var oSettings = jTable.fnSettings();
                $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);

                var editStr = '<a data-edit-id="' + aData[0] + '" onclick="' + edit + '(\'' + aData[0] + '\')" class="editBtn" title="Edit" id="' + aData[0] + '" style="cursor: pointer"><img src="'+baseUrl+'/Content/img/edit.png" /></a>';
                var custDataStr = '<a title="Custom Data"  onclick="' + custom + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" id="' + aData[0] + '" style="cursor: pointer" class="dataBtn"><img src="'+baseUrl+'/Content/img/details.png" /></a>';
                //var bioStr = '<a class="bioBtn"  onclick="' + bio + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" title="Capture Biodata" id="trf' + aData[0] + '" style="cursor: pointer"><img src="'+baseUrl+'/Content/img/info.png" /></a>';
                var template = '<td style="width: 5%">' + editStr + '&nbsp;&nbsp;' + custDataStr + '</td>'; // + '&nbsp;' + bioStr
                var ttd = $('td:last', nRow);
                ttd.after(template);
                return nRow;
            }
        });
    
        var tth = '<div><a class="btn btn-primary btn_add" style="width: auto; float: right; text-align:right"><i class="fa fa-file 2x"> </i>&nbsp;' + addButtonLabel + '</a></div>';
        $('#newItemLnk').append(tth);
        $('.dataTables_filter input').addClass('form-control').attr('type', 'text').css({ 'width': '80%' });
        $('.dataTables_length select').addClass('form-control');
        return jTable;
}

function genericTableManager(baseUrl, tableDirective, tableOptions, edit, custom, bio, newButtonLabel) {

    var columnOptions = [{
        "sName": tableOptions.itemId,
        "bSearchable": false,
        "bSortable": false
    }];

    $.each(tableOptions.columnHeaders, function (i, e) {
        columnOptions.push({ 'sName': e });
    });

    var jTable = tableDirective.dataTable({
        dom: '<"row"<"#topContainer.col-md-12"<"col-md-4"l><"col-md-4"f><"#newItemLnk.col-md-4">>>rt<"#bttmContainer.row"<"col-md-12"<"col-md-4"><"col-md-8"p>>>',
        "bServerSide": true,
        sAjaxSource: tableOptions.sourceUrl,
        "bProcessing": true,
        "bPaginate": true,
        "language": {
            "lengthMenu": 'Items per Page<select id="pgLenghtInfo">' +
                '<option value="10">10</option>' +
                '<option value="20">20</option>' +
                '<option value="30">30</option>' +
                '<option value="40">40</option>' +
                '<option value="50">50</option>' +
                '<option value="100">100</option>' +
                '</select><br/>'
        },
        "sPaginationType": "full_numbers",
        aoColumns: columnOptions,
        fnRowCallback: function (nRow, aData, iDisplayIndex) {
            var oSettings = jTable.fnSettings();
            $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);

            var bioStr = '', editStr = '', custDataStr = '';

            if (edit !== undefined && edit !== null && edit.length > 0) {
                editStr = '<a data-edit-id="' + aData[0] + '" onclick="' + edit + '(\'' + aData[0] + '\')" class="editBtn" title="Edit" id="' + aData[0] + '" style="cursor: pointer"><img src="'+baseUrl+'/Content/img/edit.png" /></a>';
            }
            if (custom !== undefined && custom !== null && custom.length > 0) {
                custDataStr = '<a title="Custom Data"  onclick="' + custom + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" id="' + aData[0] + '" style="cursor: pointer" class="dataBtn"><img src="'+baseUrl+'/Content/img/details.png" /></a>';
            }

            if (bio !== undefined && bio !== null && bio.length > 0) {
                bioStr = '<a class="bioBtn"  onclick="' + bio + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" title="Capture Biodata" id="trf' + aData[0] + '" style="cursor: pointer"><img src="'+baseUrl+'/Content/img/info.png" /></a>';
            }

            var template = '<td style="width: 5%">' + editStr + '&nbsp;' + custDataStr + '&nbsp;' + bioStr + '</td>';
            var ttd = $('td:last', nRow);
            ttd.after(template);
            return nRow;
        }
    });

    if (newButtonLabel !== undefined && newButtonLabel !== null && newButtonLabel.length > 0)
    {
        var tth = '<div><a class="btn btn-primary btn_add" style="width: auto; float: right; text-align:right"><i class="fa fa-file 2x"> </i>&nbsp;' + newButtonLabel + '</a></div>';
        $('#newItemLnk').append(tth);
    }
    $('.dataTables_filter input').addClass('form-control').attr('type', 'text').css({ 'width': '80%' });
    $('.dataTables_length select').addClass('form-control');
    return jTable;
}

function aprovalsTableManager(baseUrl, tableDirective, tableOptions, edit, custom, history, newButtonLabel) {

    var columnOptions = [{
        "sName": tableOptions.itemId,
        "bSearchable": false,
        "bSortable": false
    }];

    $.each(tableOptions.columnHeaders, function (i, e) {
        columnOptions.push({ 'sName': e });
    });

    var jTable = tableDirective.dataTable({
        dom: '<"row"<"#topContainer.col-md-12"<"col-md-4"l><"col-md-4"f><"#newItemLnk.col-md-4">>>rt<"#bttmContainer.row"<"col-md-12"<"col-md-4"><"col-md-8"p>>>',
        "bServerSide": true,
        sAjaxSource: tableOptions.sourceUrl,
        "bProcessing": true,
        "bPaginate": true,
        "language": {
            "lengthMenu": 'Items per Page<select id="pgLenghtInfo">' +
                '<option value="10">10</option>' +
                '<option value="20">20</option>' +
                '<option value="30">30</option>' +
                '<option value="40">40</option>' +
                '<option value="50">50</option>' +
                '<option value="100">100</option>' +
                '</select><br/>'
        },
        "sPaginationType": "full_numbers",
        aoColumns: columnOptions,
        fnRowCallback: function (nRow, aData, iDisplayIndex) {
            var oSettings = jTable.fnSettings();
            $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);

            var bioStr = '', editStr = '', custDataStr = '';

            if (edit !== undefined && edit !== null && edit.length > 0) {
                editStr = '<a data-edit-id="' + aData[0] + '" onclick="' + edit + '(\'' + aData[0] + '\')" class="editBtn" title="Edit" id="' + aData[0] + '" style="cursor: pointer"><img src="'+baseUrl+'/Content/img/edit.png" /></a>';
            }
            if (custom !== undefined && custom !== null && custom.length > 0) {
                custDataStr = '<a title="Custom Data"  onclick="' + custom + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" id="' + aData[0] + '" style="cursor: pointer" class="dataBtn"><img src="'+baseUrl+'/Content/img/details.png" /></a>';
            }

            if (bio !== undefined && bio !== null && bio.length > 0) {
                bioStr = '<a class="bioBtn"  onclick="' + bio + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" title="View Approval History" id="trf' + aData[0] + '" style="cursor: pointer"><img src="'+baseUrl+'/Content/img/info.png" /></a>';
            }

            var template = '<td style="width: 5%">' + editStr + '&nbsp;' + custDataStr + '&nbsp;' + bioStr + '</td>';
            var ttd = $('td:last', nRow);
            ttd.after(template);
            return nRow;
        }
    });

    if (newButtonLabel !== undefined && newButtonLabel !== null && newButtonLabel.length > 0) {
        var tth = '<div><a class="btn btn-primary btn_add" style="width: auto; float: right; text-align:right"><i class="fa fa-file 2x"> </i>&nbsp;' + newButtonLabel + '</a></div>';
        $('#newItemLnk').append(tth);
    }
    $('.dataTables_filter input').addClass('form-control').attr('type', 'text').css({ 'width': '80%' });
    $('.dataTables_length select').addClass('form-control');
    return jTable;
}

function rejectionsTableManager(baseUrl, tableDirective, tableOptions, edit, custom, history, newButtonLabel) {

    var columnOptions = [{
        "sName": tableOptions.itemId,
        "bSearchable": false,
        "bSortable": false
    }];

    $.each(tableOptions.columnHeaders, function (i, e) {
        columnOptions.push({ 'sName': e });
    });

    var jTable = tableDirective.dataTable({
        dom: '<"row"<"#topContainer.col-md-12"<"col-md-4"l><"col-md-4"f><"#newItemLnk.col-md-4">>>rt<"#bttmContainer.row"<"col-md-12"<"col-md-4"><"col-md-8"p>>>',
        "bServerSide": true,
        sAjaxSource: tableOptions.sourceUrl,
        "bProcessing": true,
        "bPaginate": true,
        "language": {
            "lengthMenu": 'Items per Page<select id="pgLenghtInfo">' +
                '<option value="10">10</option>' +
                '<option value="20">20</option>' +
                '<option value="30">30</option>' +
                '<option value="40">40</option>' +
                '<option value="50">50</option>' +
                '<option value="100">100</option>' +
                '</select><br/>'
        },
        "sPaginationType": "full_numbers",
        aoColumns: columnOptions,
        fnRowCallback: function (nRow, aData, iDisplayIndex) {
            var oSettings = jTable.fnSettings();
            $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);

            var bioStr = '', editStr = '', custDataStr = '';

            if (edit !== undefined && edit !== null && edit.length > 0) {
                editStr = '<a data-edit-id="' + aData[0] + '" onclick="' + edit + '(\'' + aData[0] + '\')" class="editBtn" title="Edit" id="' + aData[0] + '" style="cursor: pointer"><img src="'+baseUrl+'/Content/img/edit.png" /></a>';
            }
            if (custom !== undefined && custom !== null && custom.length > 0) {
                custDataStr = '<a title="Custom Data"  onclick="' + custom + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" id="' + aData[0] + '" style="cursor: pointer" class="dataBtn"><img src="'+baseUrl+'/Content/img/details.png" /></a>';
            }

            if (bio !== undefined && bio !== null && bio.length > 0) {
                bioStr = '<a class="bioBtn"  onclick="' + bio + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" title="Pull out of Rejection" id="trf' + aData[0] + '" style="cursor: pointer"><img src="'+baseUrl+'/Content/img/info.png" /></a>';
            }

            var template = '<td style="width: 5%">' + editStr + '&nbsp;' + custDataStr + '&nbsp;' + bioStr + '</td>';
            var ttd = $('td:last', nRow);
            ttd.after(template);
            return nRow;
        }
    });

    if (newButtonLabel !== undefined && newButtonLabel !== null && newButtonLabel.length > 0) {
        var tth = '<div><a class="btn btn-primary btn_add" style="width: auto; float: right; text-align:right"><i class="fa fa-file 2x"> </i>&nbsp;' + newButtonLabel + '</a></div>';
        $('#newItemLnk').append(tth);
    }
    $('.dataTables_filter input').addClass('form-control').attr('type', 'text').css({ 'width': '80%' });
    $('.dataTables_length select').addClass('form-control');
    return jTable;
}

function allTableManager(baseUrl, tableDirective, tableOptions, addFunc, edit, deleteFunc, addButtonLabel)
{
    var columnOptions =
        [
            {
                "sName": tableOptions.itemId,
                "bSearchable": false,
                "bSortable": false
            }

        ];

    $.each(tableOptions.columnHeaders, function (i, e)
    {
        columnOptions.push({ 'sName': e });
    });

    var jTable = tableDirective.dataTable({
        dom: '<"row"<"#topContainer.col-md-12"<"col-md-4"l><"col-md-4"f><"#newItemLnk.col-md-4">>>rt<"#bttmContainer.row"<"col-md-12"<"col-md-4"><"col-md-8"p>>>',
        "bServerSide": true,
        sAjaxSource: tableOptions.sourceUrl,
        "bProcessing": true,
        "bPaginate": true,
        "language": {
            "lengthMenu": 'Items per Page<select id="pgLenghtInfo">' +
                '<option value="10">10</option>' +
                '<option value="20">20</option>' +
                '<option value="30">30</option>' +
                '<option value="40">40</option>' +
                '<option value="50">50</option>' +
                '<option value="100">100</option>' +
                '</select><br/>'
        },
        "sPaginationType": "full_numbers",
        aoColumns: columnOptions,
        fnRowCallback: function (nRow, aData, iDisplayIndex) 
        {
            var editStr = '', custDataStr = '';
            var oSettings = jTable.fnSettings();
            $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);

            if (edit !== undefined && edit !== null && edit.length > 0)
            {
                editStr = '<a  data-target="#new_modal" data-edit-id="' + aData[0] + '" onclick="' + edit + '(\'' + aData[0] + '\')" class="editBtn" title="Edit" id="' + aData[0] + '" style="cursor: pointer" data-toggle="modal"><img src="'+baseUrl+'/Content/img/edit.png" /></a>';
            }
            if (deleteFunc !== undefined && deleteFunc !== null && deleteFunc.length > 0)
            {
                custDataStr = '<a title="Custom Data"  onclick="' + deleteFunc + '(\'' + aData[0] + '\')" data-edit-id="' + aData[0] + '" id="' + aData[0] + '" style="cursor: pointer" class="dataBtn"><img src="'+baseUrl+'/Content/img/delete.png" /></a>';
            }
            
            var template = '<td style="width: 5%">' + editStr + '&nbsp;&nbsp;' + custDataStr + '</td>';
            var ttd = $('td:last', nRow);
            ttd.after(template);
            return nRow;
        }
    });
    
    if (addFunc !== undefined && addFunc !== null && addFunc.length > 0)
    {
        var tth = '<div><a data-target="#new_modal" onclick="' + addFunc + '()"  role="button"  class="btn btn-primary btn_add" style="width: auto; float: right; text-align:right" data-toggle="modal"><i class="fa fa-file 2x"></i>' + addButtonLabel + '</a></div>';
        $('#newItemLnk').append(tth);
    }
    
    $('.dataTables_filter input').addClass('form-control').attr('type', 'text').css({ 'width': '80%' });
    $('.dataTables_length select').addClass('form-control');
    return jTable;
}



function estimatesTableManager($scope, $compile, tableDirective, tableOptions, addButtonValue, prepareTemplateMethodName, retrieveRecordMethodName, getItemDetails, addBtnWidth) {
    var columnOptions = [{
        "sName": tableOptions.itemId,
        "bSearchable": false,
        "bSortable": false
    }];

    $.each(tableOptions.columnHeaders, function (i, e)
    {
        columnOptions.push({ 'sName': e });
    });

    var jTable = tableDirective.dataTable({
        dom: '<"row"<"#topContainer.col-md-12"<"col-md-4"l><"col-md-4"f><"#newItemLnk.col-md-4">>>rt<"#bttmContainer.row"<"col-md-12"<"col-md-4"><"col-md-8"p>>>',
        "bServerSide": true,
        sAjaxSource: tableOptions.sourceUrl,
        "bProcessing": true,
        "language": {
            "lengthMenu": 'Items per Page<select id="pgLenghtInfo">' +
                '<option value="10">10</option>' +
                '<option value="20">20</option>' +
                '<option value="30">30</option>' +
                '<option value="40">40</option>' +
                '<option value="50">50</option>' +
                '<option value="100">100</option>' +
                '</select><br/>'
        },
        "sPaginationType": "full_numbers",
        aoColumns: columnOptions,
        fnRowCallback: function (nRow, aData, iDisplayIndex)
        {
            var oSettings = jTable.fnSettings();
            $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);

            var details = '<a title="Details" style="cursor: pointer" ng-click = "' + getItemDetails + '(\''+aData[1]+'\',\''+aData[7]+'\')"><img src="/Content/images/details.png" /></a>';
            var editStr = '';
           
            if (aData[7] !== 'Approved' && aData[7] !== 'Processed')
            {
                editStr = '<a title="Edit" id="' + aData[0] + '" style="cursor: pointer" class="bankEdTx" ng-click = "' + retrieveRecordMethodName + '(' + aData[0] + ')"><img src="/Content/images/edit.png" /></a>';
            }

            var template = '<td style="width: 5%">' + details + '&nbsp;&nbsp;' + editStr + '</td>';

            var ttd = $('td:last', nRow);
            ttd.after($compile(template)($scope));
            return nRow;
        }
    });

    $('.dataTables_filter input').addClass('form-control').attr('type', 'text').css({ 'width': '80%' });
    $('.dataTables_length select').addClass('form-control');
    
    return jTable;
}
function transferNotesTableManager($scope, $compile, tableDirective, tableOptions, addButtonValue, prepareTemplateMethodName, retrieveRecordMethodName, getItemDetails, addBtnWidth) {
    var columnOptions = [{
        "sName": tableOptions.itemId,
        "bSearchable": false,
        "bSortable": false
    }];

    $.each(tableOptions.columnHeaders, function (i, e)
    {
        columnOptions.push({ 'sName': e });
    });

    var jTable = tableDirective.dataTable({
        dom: '<"row"<"#topContainer.col-md-12"<"col-md-4"l><"col-md-4"f><"#newItemLnk.col-md-4">>>rt<"#bttmContainer.row"<"col-md-12"<"col-md-4"><"col-md-8"p>>>',
        "bServerSide": true,
        sAjaxSource: tableOptions.sourceUrl,
        "bProcessing": true,
        "language": {
            "lengthMenu": 'Items per Page<select id="pgLenghtInfo">' +
                '<option value="10">10</option>' +
                '<option value="20">20</option>' +
                '<option value="30">30</option>' +
                '<option value="40">40</option>' +
                '<option value="50">50</option>' +
                '<option value="100">100</option>' +
                '</select><br/>'
        },
        "sPaginationType": "full_numbers",
        aoColumns: columnOptions,
        fnRowCallback: function (nRow, aData, iDisplayIndex)
        {
            var oSettings = jTable.fnSettings();
            $("td:first", nRow).html(oSettings._iDisplayStart + iDisplayIndex + 1);

            var details = '<a title="Details" style="cursor: pointer" ng-click = "' + getItemDetails + '(\'' + aData[1] + '\',\'' + aData[8] + '\')"><img src="/Content/images/details.png" /></a>';

            var editStr = '';

            //"TransferNoteNumber", "DateGeneratedStr", "SourceOutletName", "TargetOutletName", 'GeneratedBy', "TotalAmountStr", 'DateTransferdStr', 'StatusStr'

            if (aData[8] !== 'Completely Transfered')
            {
                editStr = '<a title="Edit" id="' + aData[0] + '" style="cursor: pointer" class="bankEdTx" ng-click = "' + retrieveRecordMethodName + '(' + aData[0] + ')"><img src="/Content/images/edit.png" /></a>';
            }

            var template = '<td style="width: 5%">' + details + '&nbsp;&nbsp;' + editStr + '</td>';

            var ttd = $('td:last', nRow);
            ttd.after($compile(template)($scope));
            return nRow;
        }
    });

    $('.dataTables_filter input').addClass('form-control').attr('type', 'text').css({ 'width': '80%' });
    $('.dataTables_length select').addClass('form-control');

    return jTable;
}


function setBackupControlDate($scope, minDate, maxDate) {
    $scope.today = function () {
        return new Date();
    };

    $scope.today();

    $scope.clear = function () {
        $scope.dt = null;
    };

    $scope.toggleMin = function () {
        $scope.minDate = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMax = function () {
        $scope.maxDate = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMin();
    $scope.toggleMax();

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    $scope.dateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.formats = ['dd/MM/yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
}

function setTime($scope, minDate, maxDate)
{
    $scope.today = function () {
        return new Date();
    };

    $scope.today();

    $scope.clear = function () {
        $scope.dt = null;
    };

    $scope.toggleMin = function () {
        $scope.minDate = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMax = function () {
        $scope.maxDate = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMin();
    $scope.toggleMax();

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    $scope.dateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.timeOptions =
     {
        readonlyInput: false,
        showMeridian: true
    };

    $scope.formats = ['HH:MM'];
    $scope.format = $scope.formats[0];
}

function setControlDate($scope, minDate, maxDate)
{
    $scope.today = function () {
        return new Date();
    };

    $scope.today();

    $scope.clear = function () {
        $scope.dt = null;
    };

    $scope.toggleMin = function () {
        $scope.minDate = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMax = function () {
        $scope.maxDate = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMin();
    $scope.toggleMax();

    $scope.open = function ($event)
    {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    $scope.dateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.formats = ['dd/MM/yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
}

function reportStartDate($scope, minDate, maxDate)
{
    $scope.today = function () {
        return new Date();
    };

    $scope.today();

    $scope.clear = function () {
        $scope.dt = null;
    };

    $scope.toggleMin = function () {
        $scope.minDate = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMax = function () {
        $scope.maxDate = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMin();
    $scope.toggleMax();

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    $scope.dateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.formats = ['dd/MM/yyyy HH:mm:ss Z', 'yyyy/MM/dd HH:mm:ss Z', 'dd.MM.yyyy HH:mm:ss Z', 'shortDate'];
    $scope.format = $scope.formats[0];
}

function setReportEndDate($scope, minDate, maxDate)
{
    $scope.today = function ()
    {
        return new Date();
    };

    $scope.today();

    $scope.clearEndDate = function () {
        $scope.expiryDate = null;
    };

    $scope.toggleEndMin = function () {
        $scope.minEndDate = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleEndMax = function () {
        $scope.maxEndDate = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleEndMin();
    $scope.toggleEndMax();

    $scope.openEnDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.endDateOpened = true;
    };

    $scope.endDateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.endDateFormats = ['dd/MM/yyyy HH:mm:ss Z', 'yyyy/MM/dd HH:mm:ss Z', 'dd.MM.yyyy HH:mm:ss Z', 'shortDate'];
    $scope.endDateformat = $scope.endDateFormats[0];
}

function setEndDate($scope, minDate, maxDate)
{
    $scope.today = function () {
        return new Date();
    };

    $scope.today();

    $scope.clearEndDate = function ()
    {
        $scope.expiryDate = null;
    };

    $scope.toggleEndMin = function ()
    {
        $scope.minEndDate = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleEndMax = function ()
    {
        $scope.maxEndDate = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleEndMin();
    $scope.toggleEndMax();

    $scope.openEnDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.endDateOpened = true;
    };

    $scope.endDateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.endDateFormats = 'dd/MM/yyyy';
    $scope.endDateformat = $scope.endDateFormats;
}

function setMaxDate($scope, minDate, maxDate)
{
    $scope.today = function ()
    {
        return new Date();
    };

    $scope.today();

    $scope.clearEndDate = function ()
    {
        $scope.expiryDate = null;
    };

    // Disable weekend selection
    //$scope.disabled = function (date, mode) {
    //    return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    //};

    $scope.toggleEndMin = function () {
        $scope.minEndDate = minDate;
    };

    $scope.toggleEndMax = function () {
        $scope.maxEndDate = maxDate;
    };

    $scope.toggleEndMin();
    $scope.toggleEndMax();

    $scope.openEnDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.endDateOpened = true;
    };

    $scope.endDateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.endDateFormats = ['dd/MM/yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.endDateformat = $scope.endDateFormats[0];
}

function setMaxDateWithWeekends($scope, minDate, maxDate)
{
    $scope.today = function ()
    {
        return new Date();
    };

    $scope.today();

    $scope.clearEndDatep = function () {
        $scope.expiryDatep = null;
    };

    $scope.toggleEndMinp = function () {
        $scope.minEndDatep = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleEndMaxp = function () {
        $scope.maxEndDatep = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleEndMinp();
    $scope.toggleEndMaxp();

    $scope.openEnDatep = function ($event)
    {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.endDateOpenedp = true;
    };

    $scope.endDateOptionsp =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.endDateFormatsp = 'dd/MM/yyyy';
    $scope.endDateformatp = $scope.endDateFormatsp;
}

function setExpiryDate($scope, minDate, maxDate)
{
    $scope.maxToday = function ()
    {
        return new Date();
    };

    $scope.maxToday();

    $scope.clearEndExpDate = function ()
    {
        $scope.expDate = null;
    };

    $scope.toggleMinExpDate = function ()
    {
        $scope.minExpDate = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMaxExpDate = function ()
    {
        $scope.maxExpDate = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleMinExpDate();
    $scope.toggleMaxExpDate();

    $scope.openExpDate = function ($event)
    {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.expDateOpened = true;
    };
    
    $scope.expDateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.expDateFormat = 'dd/MM/yyyy';
}

function setEndDateWithWeekends($scope, minDate, maxDate) {
    $scope.today = function () {
        return new Date();
    };

    $scope.today();

    $scope.clearEndDate = function () {
        $scope.expiryDate = null;
    };

    $scope.toggleEndMin = function () {
        $scope.minEndDate = minDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleEndMax = function () {
        $scope.maxEndDate = maxDate; //$scope.minDate ? null : new Date();
    };

    $scope.toggleEndMin();
    $scope.toggleEndMax();

    $scope.openEnDate = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.endDateOpened = true;
    };

    $scope.endDateOptions =
    {
        formatYear: 'yyyy',
        startingDay: 1
    };

    $scope.endDateFormats = 'dd/MM/yyyy';
    $scope.endDateformat = $scope.endDateFormats;
}