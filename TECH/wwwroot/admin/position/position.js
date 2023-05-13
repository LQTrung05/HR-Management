(function ($) {
    var self = this;
    self.IsUpdate = false;  
    self.Position = {
        Id: null,
        PositionName: "",
        Coefficient:0,
        Note:""
    }
    self.Search = {
        name: "",
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }

    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };
    self.Files = {};

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + item.PositionName + "</td>";
                html += "<td>" + item.Coefficient + "</td>";
                html += "<td>" + item.Note + "</td>";
                     
                html += "<td style=\"text-align: center;\">" +                    
                    "<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id +")\"><i  class=\"bi bi-pencil-square\"></i></button>" +
                    "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.Id +")\"><i  class=\"bi bi-trash\"></i></button>" +
                    "</td>";                
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
    
    self.Update = function (id) {
        if (id != null && id != "") {
            $("#titleModal").text("Cập nhật chức vụ");
            $(".btn-submit-format").text("Cập nhật");
            self.GetById(id, self.RenderHtmlByObject);
            self.Position.Id = id;
            $('#userModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Position/GetById',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.Data != null) {
                        renderCallBack(response.Data);
                    }
                }
            })
        }
    }

    self.WrapPaging = function (recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / tedu.configs.pageSize);
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: '<<',
            prev: '<',
            next: '>',
            last: '>>',
            onPageClick: function (event, p) {
                tedu.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
    self.Deleted = function (id) {
        if (id != null && id != "") {
            tedu.confirm('Bạn có chắc muốn xóa chức vụ này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Position/Delete",
                    data: { id: id },
                    beforeSend: function () {
                    },
                    success: function () {
                        tedu.notify('Đã xóa thành công', 'success');
                        self.GetDataPaging(true);
                    },
                    error: function () {
                        tedu.notify('Has an error', 'error');
                    }
                });
            });
        }
    }

    self.GetDataPaging = function (isPageChanged) {
        self.Search.PageIndex = tedu.configs.pageIndex;
        self.Search.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Position/GetAllPaging',
            type: 'GET',
            data: self.Search,
            dataType: 'json',
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                self.RenderTableHtml(response.data.Results);
                $('#lblTotalRecords').text(response.data.RowCount);
                if (response.data.RowCount != null && response.data.RowCount > 0) {
                    self.WrapPaging(response.data.RowCount, function () {
                        GetDataPaging();
                    }, isPageChanged);
                }

            }
        })

    };       

    self.AddUser = function (userView) {
        $.ajax({
            url: '/Position/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                PositionModelView: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Thêm mới dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    $('#userModal').modal('hide');
                }
                else {
                    tedu.notify('Tên phòng đã tồn tại', 'error');
                }
            }
        })
    }


    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Position/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                PositionModelView: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    $('#userModal').modal('hide');
                } else {
                    tedu.notify('Tên dịch vụ đã tồn tại', 'error');
                }
               
            }
        })
    }

    self.ValidateUser = function () {                
        $("#form-submit").validate({
            rules:
            {
                PositionName: {
                    required: true,
                },
                Coefficient: {
                    required: true,
                }
            },
            messages:
            {
                PositionName: {
                    required: "Tên chức vụ không được để trống",
                },
                Coefficient: {
                    required: "Hệ số lương không được để trống",
                }
            },
            submitHandler: function (form) {   
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.Position);
                }
                else {
                    self.AddUser(self.Position);
                }
            }
        });
    }

    self.GetValue = function () {
        self.Position.PositionName = $("#PositionName").val();
        self.Position.Coefficient = $("#Coefficient").val();
        self.Position.Note = $("#Note").val();       
    }

    Set.SetValue = function () {
        $("#PositionName").val(self.Position.PositionName);
        $("#Coefficient").val(self.Position.Coefficient);
        $("#Note").val(self.Position.Note);       
    }

    self.RenderHtmlByObject = function (view) {       
        $("#Note").val(view.Note);    
        $("#PositionName").val(view.PositionName);
        $("#Coefficient").val(view.Coefficient);
    }


    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();    

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
            $("#titleModal").text("Thêm mới chức vụ");
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;  
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.Search.name = $(this).val();
            self.GetDataPaging(true);
        });
       
    })
})(jQuery);