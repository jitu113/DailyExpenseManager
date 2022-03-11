var app = angular.module('myApp', ['angular.filter']);

app.filter("jsDate", function () {
    return function (x) {
        return new Date(parseInt(x.substr(6)));
    };
});
app.controller('myCtrl', function ($scope, $http) {
    //variable declaration
    var ItemTypeId;
    var item;
    $scope.item = '';
    $scope.FromDate = new Date();
    $scope.ToDate = new Date();
    $scope.Totol = 0;
    $scope.TransactionCreate = false;
    $scope.lblAlrt = '';
    var LstOfExpensDetails = [];
    $scope.LstOfExpensDetails = LstOfExpensDetails;
 
    $scope.SearchItemPost = function (isInitialLoad) {
        $(".myTableHead").css('display', 'none');
        $('#SearchModal').modal('hide');
        var req = {
            method: 'POST',
            url: IndexJsUrl,
            headers: {
                'Content-Type': 'application/json'
            },
            data: { item: $scope.KeyWords, itemTypeId: $('#itemTypeSerch').val(), fromDate: $('#fromdateid').val(), Todate: $('#todateid').val(), UserId: $("#userId").val() }
        }
        $http(req).then(
            function (response) {
                $(".myTableContainer").empty();
        
                if (response.data.status == 200) {
                    $scope.LstOfExpensDetails = [];
                    $scope.Totol = 0;
                    $scope.LstOfExpensDetails = response.data;

                    $.each(response.data.lstExpenseDtls, function (index, Element) {
                        $(".myTableContainer").append('<div class="card a" id="div_' + index + '"><h4 style=""><span class="CardHead">' + forMateDate(Element.Date) + '</span></h4><table id="expenseTbl_' + index + '" style="" class="expenseTbl">' +
                            '<thead"><tr><th  class="TheadDate" hidden="hidden">Date</th><th style="">Description</th><th>UnitPrice</th><th>Quantity</th><th>Totol</th><th>ItemType</th><th>Actions</th><th style="border-left:none"></th></tr></thead></table></div>'

                        );
                        $.each(Element.Items, function (index1, Element1) {
                            $("#expenseTbl_" + index).append('<tr class="listInput_' + Element1.EntryId + '"><td hidden="hidden" class="tdDate"> <input type="text" value="' + forMateDate(Element1.Date) + '" name="datefield" class="txtList txtListLg" disabled/></td>' +
                                '<td> <input type="text" value="' + Element1.Particular + '" class="txtList txtListLg  txtList_' + Element1.EntryId + '" name="particular" style="border:none" disabled/></td>' +
                                '<td>  <input type="text" value="' + Element1.UnitPrice + '" name="unitprice" class="txtList txtListsm  txtList_' + Element1.EntryId + '" disabled/></td>' +
                                '<td> <input type="text" value="' + Element1.Quantity + '" name="quantity" class="txtList txtListsm txtList_' + Element1.EntryId + '" disabled/></td>' +
                                '<td><input type="text" value="' + Element1.Totol + '"class="txtList txtList_' + Element1.EntryId + '" name="total" disabled /></td>' +
                                '<td> <input type="text" name="itemType" value="' + Element1.ItemType + '"  class="txtList  txtList_' + Element1.EntryId + '" disabled/></td>' +
                                '<td style="border-left:1px solid #ddd"><input type="button" onclick="edit(' + Element1.EntryId + ')" value="Edit" id="btnEdit_' + Element1.EntryId + '" style="background-color:MediumSeaGreen;width:60px;right:2px" name="edit" class="button"/></td>' +
                                 '<td><input type="button" onclick="showDelte(' + Element1.EntryId + ')" value="Delete" id="btnDelete_' + Element1.EntryId + '" style="background-color:red;width:60px;" name="delete" class="button" /></td ></tr >' 
                            )

                        })
                        $("#div_" + index).append('<div class="row"><div class="col-3 col-md-2 sumrow" >Total</div><div class="col-2 col-md-1 sumrowval">' + Element.Sum + '</div></div>')



                    })
                    if (response.data.lstExpenseDtls.length > 0) {
                    
                        $("#resultDiv").css('display', 'none');
                        $("#grandTtlDiv").css('display', 'block');
                        document.getElementById("grndttl").innerHTML = response.data.lstExpenseDtls[0].GrandTotol;
                    }
                    else {
                        
                       
                    }
                }

                else {
                   
                    if (isInitialLoad) {

                        $("#resultDiv").text(' No expense on current Date');

                    }
                    else {
                        $("#resultDiv").text(' No expense found');
                    }
                    $("#grandTtlDiv").css('display', 'none');
                    $("#resultDiv").css('display', 'block');
                   

                }


            }, function (error) {

                $("#resultDiv").text('Somthing went wrong!');
                $("#grandTtlDiv").css('display', 'none');
                $("#resultDiv").css('display', 'block');

            });

    }
    deleteFun = function () {

        var req = {
            method: 'POST',
            
            url: DeleteUrl + '?id=' + $scope.EntryId + "&userid=" + $("#userId").val(),
        }
        $http(req).then(
            function (response) {
                if (response.data.status == 200) {
                    $scope.EntryId = 0;
                    $('#DeleteMsg').text('Delted Succsessfully');
                    $('#btnDeleteOk').val('Ok');
                    $('#btnDeleteOk').removeAttr('onclick');
                    $('#btnDeleteOk').on('click', function () { $('#DeleteModal').modal('hide'); })
                }
                else {
                    $('#DeleteMsg').text('Unable to delete item.Please Retry!');
                   
                }
            }, function (error) {
                $('#DeleteMsg').text('Sorry something went wrong.Please Retry!');
               

            });

    }
    showDelte = function (EntryId) {
        $('#DeleteMsg').text('Are you sure you want to delete');
        $('#btnDeleteOk').off('click').on('click', deleteFun)
        //$('#btnDeleteOk').unbind('click',);
        //$("#btnDeleteOk").click(deleteFun);
        $scope.EntryId = EntryId;
        $('#DeleteModal').modal('show');
    }

    Update = function (entryId, date, particular, quantity, unitPrice, totol, remarks, itemTypeId) {
        var req = {
            method: 'POST',
            url: CreateUrl,
            headers: {
                'Content-Type': 'application/json'
            },
            data: {
                EntryId: entryId, Date: date, Particular: particular, Quantity: quantity,
                UnitPrice: unitPrice, Totol: totol, Remarks: remarks, ItemTypeId: itemTypeId,
                UserId: $("#userId").val()
            }
        }
        $http(req).then(
            function (response) {
                var msg = '';

                if (response.data.status == 200) {
                    $('#ResultMsg').val('Updated Successfully!');
                    $('.txtList_' + entryId).prop('disabled', true);
                    $("#btnEdit_" + entryId).css({ 'border-bottom': 'none' });
                    $('.txtList_' + entryId).prop('disabled', true);
                    $(".listInput_" + entryId).find('input, select').each(function () {
                        //$(this).prop('disabled', true);

                        $(this).css({ 'border': 0, 'border-left': '1px solid #ddd' });
                        if ($(this).attr('name') == 'datefield') {

                            var dat = $(this).val();
                            var par = $(this).parent();
                            $(this).parent().empty();
                            par.append('<input type="text" value="' + forMateDate(dat) + '" name="datefield" class="txtList" disabled/>');
                        }

                        if ($(this).attr('name') == 'edit') {
                            $("#btnEdit_" + entryId).val('Edit');

                            $(this).css({ 'border-bottom': 'none' });
                        }
                        if ($(this).attr('name') == 'itemType') {
                            var ItemType = $(this).val();
                            var text = $("#itemType_" + entryId + " option:selected").text();
                            var par = $(this).parent();
                            $(this).parent().empty();
                            par.append('<input type="text" name="itemType" value="' + text + '"  class="txtList  txtList_' + entryId + '" disabled/></td>')
                        }


                    });

                }
                else {

                    $('#ResultMsg').val('Failed to update.Please try again later!');

                }

            }, function (error) {

                $('#ResultMsg').val('Failed to update.Please try again later!');



            });
        $('#ResultModal').modal('show');
    }

   

    $scope.CreateTransaction = function () {

        var req = {
            method: 'POST',
            url: CreateUrl,
            headers: {
                'Content-Type': 'application/json'
            },
            data: {
                EntryId: $scope.Entryid, Date: $('#dateid').val(), Particular: $scope.particular, Quantity: $('#quantity').val(),
                UnitPrice: $scope.unitPrice, Totol: $scope.total, Remarks: $scope.Remarks, ItemTypeId: $('#itemType').val(),
                UserId: $("#userId").val()
            }
        }
        $http(req).then(
            function (response) {
                var msg = '';

               
                 if (response.data.status == 201) {
                    $scope.SearchItemPost(false);
                    msg = "Added Successfully!";

                    $("#btnClrAndAdd").val('Clear and add new');
                }
                else {
                    msg = 'Somthing went wrong!';
                    $("#btnClrAndAdd").prop('value', 'Retry');
                }
                $("#msgbox").css('color', 'green');
                $("#msgbox").text(msg);
                $("#BeforeCreation1").attr('hidden', 'true');
                $('#AfterCreation1').removeAttr('hidden');
            }, function (error) {
                $("#btnClrAndAdd").val('Retry');
                msg = 'Somthing went wrong!';
             
                $("#BeforeCreation1").attr('hidden', 'true');
                $('#AfterCreation1').removeAttr('hidden');
                   $("#msgbox").text(msg);
                   $("#msgbox").css('color', 'red');

            });

        //   $('#AlterBox').modal('show');
    }
    $scope.GetCurrentDetails = function () {
       
        var req = {
            method: 'GET',
            url: CurrentDetailsUrl,
        }
        $http(req).then(
            function (response) {
                if (response.status == 200) {
                    $('#expectedIncome').html(response.data.ExpectedIncome);
                    $('#currentIncome').html(response.data.CurrentIncome);

                    var incomeLeft = response.data.ExpectedIncome - response.data.CurrentIncome;
                    $('#incomeLeft').html(incomeLeft);

                    $('#expectedExpense').html(response.data.ExpectedExpense);
                    $('#currentExpense').html(response.data.CurrentExpenses);

                    var expenseLeft = response.data.ExpectedExpense - response.data.CurrentExpenses;
                    $('#expenseLeft').html(expenseLeft);
                    var savings = response.data.CurrentIncome - response.data.CurrentExpenses;
                    $('#savings').html(savings);
                }
                else {

                    $("#CurrentDetailsErr").text("Somthing went wrong!");
                    $("#CurrentDetailsErr").show();
                }
            }, function (error) {

                $("#CurrentDetailsErr").text("Somthing went wrong!");
                $("#CurrentDetailsErr").show();

            });

    }
    $scope.Clear = function () {
        if ($("#btnClrAndAdd").val() == 'Retry') {
            $scope.CreateTransaction();
        }
        else {

            clear();
        }

    }
    clear = function () {


        $("#AfterCreation1").attr('hidden', 'true');
        $('#BeforeCreation1').removeAttr('hidden');
        $scope.particular = '';
        $scope.unitPrice = ''; $scope.total = ''; $scope.Remarks = '';
        $scope.EntryId = 0;
        $("#quantity").val(1);
        $("#itemType").val(1);
    }

    ShowLogin = function () {

        $("#btnLogin").val('Login');

        $("#confirmPassword").attr('hidden', 'none');
        $("#LoginDiv").css('display', 'none');
        $("#RegisTerDiv").css('display', 'block');
        
        $('#loginForm').attr('action', '/home/UserLogin');
        
    }
    ShowRegister = function () {

        $("#btnLogin").val('Register');

        $("#confirmPassword").removeAttr('hidden');
        $("#LoginDiv").css('display', 'block');
        $("#RegisTerDiv").css('display', 'none');

        $('#loginForm').attr('action', '/home/Register');

    }
    $(document).ready(
       
        function () {
            if ($("#userId").val() == '0' || $("#userId").val()=="") {

                alert('The data you are seeing are dummy data entred by Guset Users!');
                $scope.SearchItemPost(true);
                $scope.GetCurrentDetails();
            }
            else {
             

                $scope.SearchItemPost(true);
                $scope.GetCurrentDetails();
            }

        }
    )
    CalculateTotol = function () {

        if ($scope.unitPrice != NaN || $scope.unitPrice != undefined) {
            $scope.total = $scope.unitPrice * $("#quantity").val();
            $("#totalJq").val($scope.total);
        }
    }
    $('#myModal').on('shown.bs.modal', function (e) {
        $("#dateid").val(GetCurrentDate());
    })
    $('#DeleteModal').on('hidden.bs.modal', function (e) {
        $scope.SearchItemPost(false);
    })
    $('#myModal').on('hidden.bs.modal', function (e) {
        clear();
    })
    edit = function (EntryId) {
    
        var remarks, date, particular, itemTypeId, unitPrice, quantity, totol, remarks;
        if ($("#btnEdit_" + EntryId).val() == 'Update') {


            $(".listInput_" + EntryId).find('input, select').each(function () {

                if ($(this).attr('name') == 'datefield') {
                    date = $(this).val();

                }
                else if ($(this).attr('name') == 'particular') {
                    particular = $(this).val();
                }
                else if ($(this).attr('name') == 'itemType') {
                    itemTypeId = $(this).val();
                }
                else if ($(this).attr('name') == 'unitprice') {
                    unitPrice = $(this).val();

                }
                else if ($(this).attr('name') == 'quantity') {
                    quantity = $(this).val();

                }
                else if ($(this).attr('name') == 'total') {
                    totol = $(this).val();

                }
                else if ($(this).attr('name') == 'remarks') {
                    remarks = $(this).val();
                }

            });
            Update(EntryId, date, particular, quantity, unitPrice, totol, remarks, itemTypeId);

        }
        else {
            $(".listInput_" + EntryId).find('input, select').each(function () {
                $(this).prop('disabled', false);

                $(this).css({ 'border': 0, 'border-left': '2px solid #989898' });
                //enable to to edit Date
                //if ($(this).attr('name') == 'datefield') {

                //    var dat = $(this).val();
                //    var par = $(this).parent();
                //    $(this).parent().empty();
                //    par.append('<input type="date" name="datefield" id="dateid_' + EntryId + '"/>');
                //    $(".tdDate").prop('hidden', false);
                //    $("#dateid_" + EntryId).val(revertDate(dat));
                //    $(".TheadDate").prop('hidden', false);
                //}

                if ($(this).attr('name') == 'edit') {
                    $("#btnEdit_" + EntryId).val('Update');

                    $(this).css({ 'border-bottom': '2px solid #007bff' });
                }
                if ($(this).attr('name') == 'itemType') {
                    var type = $(this).val();
                    var par = $(this).parent();
                    $(this).parent().empty();
                    par.append('<select name="itemType" id="itemType_' + EntryId + '" style="font-family:Consolas">' +
                        '<option value=1>Food</option>' +
                        '<option value=2>Shopping</option>' +
                        '<option value=3>Grocery</option>' +
                        '<option value=4>Bill</option>' +
                        '<option value=5>Travel</option>' +
                        '<option value=6>Investment</option>' +
                        '<option value=7>Other</option>'+
                        '</select>');
                }
               

            });
        }
    };
});
