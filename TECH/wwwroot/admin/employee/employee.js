(function ($) {
    var self = this;
    self.IsUpdate = false;  
    self.UserAvatar = {};
    self.Employee = {
        EmployeeCode: "",
        PositionCode: null,
        RoomCode: null,
        Avatar: "",
        FullName: "",
        SDT: "",
        Email: "",
        CMND: "",
        Password:"",
        LevelAcademic: null,
        Gender: null,
        YearOfBirth: null,
        Address: "",
        IsDeleted: null,
        Basicsalary:0
    }
    self.Search = {
        Name: "",
        IsSearch: null,
        PositionCode: "",
        Status:0,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
    self.ChucVu = [];
    self.PhongBan = [];
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
                var editButton = "";
                if (item.IsDeleted !=true) {
                    editButton = "<button  class=\"btn btn-primary custom-button\" onClick=\'Update(\"" + item.EmployeeCode + "\")\'><i  class=\"bi bi-pencil-square\"></i></button>" +
                        "<button  class=\"btn btn-danger custom-button\" onClick=\'Deleted(\"" + item.EmployeeCode + "\")\'><i  class=\"bi bi-trash\"></i></button>";
                }
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                if (item.Avatar != null && item.Avatar !="") {
                    html += "<td> <img src=/avartar/" + item.Avatar + " class=\"item-image\" /></td>";
                } else {
                    html += "<td> <img src=../../../public/images/avatar-mac-dinh.jpg class=\"item-image\" /></td>";
                }
                html += "<td>" + item.FullName + "</td>";
                html += "<td>" + item.EmployeeCode + "</td>";
                html += "<td>" + item.YearOfBirthStr + "</td>";
                //html += "<td>" + item.GenderStr + "</td>";
                html += "<td>" + item.PositionCodeStr + "</td>";
                html += "<td>" + item.RoomCodeStr + "</td>";
                html += "<td>" + item.IsDeletedStr + "</td>";                     
                html += "<td style=\"text-align: center;\">" +                    
                    editButton
                    +"</td>";                
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
            $("#titleModal").text("Cập nhật nhân viên");
            $(".btn-submit-format").text("Cập nhật");
            self.GetById(id, self.RenderHtmlByObject);
            self.Employee.EmployeeCode = id;
            $('#userModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Employee/GetById',
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
            tedu.confirm('Bạn có chắc muốn xóa nhân viên này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Employee/Delete",
                    data: { id: id },
                    beforeSend: function () {
                    },
                    success: function () {
                        tedu.notify('Đã xóa thành công', 'success');
                        self.GetDataPaging(true);
                        window.location.reload();
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
            url: '/Employee/GetAllPaging',
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
            url: '/Employee/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                EmployeeModelView: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    if (self.UserAvatar != null && self.UserAvatar != "" && response.id != null && response.id !="") {
                        self.UploadFileImage(response.id);
                    }
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
            url: '/Employee/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                EmployeeModelView: userView
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
                FullName: {
                    required: true,
                },
                SDT: {
                    required: true,
                },
                Email: {
                    required: true,
                },
                CMND: {
                    required: true,
                },
                LevelAcademic: {
                    required: true,
                },
                Gender: {
                    required: true,
                },
                Address: {
                    required: true,
                },
                PositionCode: {
                    required: true,
                },
                RoomCode: {
                    required: true,
                },
                YearOfBirth: {
                    required: true,
                },
                Password: {
                    required: true,
                },
                UserName: {
                    required: true,
                },
                confirm_password: {
                    required: true,
                    equalTo: "#Password"
                },
                Basicsalary: {
                    required: true,
                },
            },
            messages:
            {
                FullName: {
                    required: "Bạn chưa nhập tên nhân viên",
                },
                SDT: {
                    required: "Bạn chưa nhập số điện thoại",
                },
                Email: {
                    required: "Bạn chưa nhập email",
                },
                CMND: {
                    required: "Bạn chưa nhập CMND",
                },
                LevelAcademic: {
                    required: "Bạn chưa chọn trình độ",
                },
                Gender: {
                    required: "Bạn chưa chọn giới tính",
                },
                Address: {
                    required: "Bạn chưa nhập địa chỉ",
                },
                PositionCode: {
                    required: "Bạn chưa chọn vị trí",
                },
                RoomCode: {
                    required: "Bạn chưa chọn phòng ban",
                },
                YearOfBirth: {
                    required: "Bạn chưa nhập ngày sinh",
                },
                Password: {
                    required: "Bạn chưa nhập mật khẩu",
                },
                UserName: {
                    required: "Bạn chưa nhập tên đăng nhập",
                },
                confirm_password: {
                    required: "Vui lòng nhập lại mật khẩu",
                    equalTo: "Mật khẩu không đúng.",
                },
                Basicsalary: {
                    required: "Lương cơ bản không được để trống",
                },
            },
            submitHandler: function (form) {   
                self.GetValue();
               
                if (self.IsUpdate) {
                    self.UpdateUser(self.Employee);
                    if (self.UserAvatar != null && self.UserAvatar != "" && self.Employee.EmployeeCode !="") {
                        self.UploadFileImage(self.Employee.EmployeeCode);
                    }
                }
                else {
                    self.AddUser(self.Employee);
                }
            }
        });
    }

    self.GetValue = function () {
        self.Employee.FullName = $("#FullName").val();
        self.Employee.SDT = $("#SDT").val();
        self.Employee.Email = $("#Email").val();  
        self.Employee.CMND = $("#CMND").val();
        self.Employee.LevelAcademic = $("#LevelAcademic").val();
        self.Employee.Gender = $('input[name="Gender"]:checked').val();
        self.Employee.Address = $("#Address").val();
        self.Employee.PositionCode = $("#PositionCode").val();
        self.Employee.RoomCode = $("#RoomCode").val();   
        self.Employee.Password = $("#Password").val();   
        self.Employee.UserName = $("#UserName").val();   
        self.Employee.YearOfBirth = $.datepicker.formatDate("yy-mm-dd", $("#YearOfBirth").datepicker("getDate"));
        self.Employee.Basicsalary = $("#Basicsalary").val();    
    }

    self.RenderHtmlByObject = function (view) {       
        $("#FullName").val(view.FullName);          
        $("#SDT").val(view.SDT);
        $("#Email").val(view.Email);
        $("#LevelAcademic").val(view.LevelAcademic);
        //$("#Gender").val(view.Gender);
        if (view.Gender == 1) {
            $('#Male').prop('checked', true);
        } else if (view.Gender == 2) {
            $('#Female').prop('checked', true);
        }
        $("#Address").val(view.Address);
        $("#PositionCode").val(view.PositionCode);
        $("#RoomCode").val(view.RoomCode);
        $("#Password").val(view.Password);
        $("#UserName").val(view.UserName);
        $("#confirm_password").val(view.Password);
        $("#Basicsalary").val(view.Basicsalary);
        $("#CMND").val(view.CMND);
        if (view.Avatar != null && view.Avatar !="") {
            $(".avatar-img").attr("src", "/avartar/" + view.Avatar);
        }
       
        $("#YearOfBirth").datepicker("setDate", new Date(view.YearOfBirthStr));
    }
    self.GetAllChucVu = function () {
        $.ajax({
            url: '/Position/GetPositionChucVu',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Data != null) {
                    self.ChucVu = response.Data;
                    var html = "<option value=''>Chọn chức vụ</option>";
                    var htmlSearch = "<option value=''>Chọn tất cả</option>";
                    $.each(response.Data, function (key, item) {
                        html += "<option value=" + item.Id + ">" + item.PositionName + "</option>";
                        htmlSearch += "<option value=" + item.Id + ">" + item.PositionName + "</option>";
                    })
                    $("#PositionCode").html(html);
                    $("#select-right-chu-vu").html(htmlSearch);
                }
            }
        })
    }
    self.GetAllPhongBan = function () {
        $.ajax({
            url: '/Department/GetAllDepartment',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.Data != null) {
                    self.ChucVu = response.Data;
                    var html = "<option value=''>Chọn chức vụ</option>";
                    $.each(response.Data, function (key, item) {
                        html += "<option value=" + item.Id + ">" + item.RoomName + "</option>";
                    })
                    $("#RoomCode").html(html);
                }
            }
        })
    }
    self.UploadFileImage = function(id) {
        var dataImage = new FormData();
        dataImage.append(id, self.UserAvatar);

        $.ajax({
            url: '/Employee/UploadImageAvartar',
            type: 'POST',
            contentType: false,
            processData: false,
            data: dataImage,
            beforeSend: function () {
                
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                //if (response.success) {
                //    self.GetDataPaging(true);
                //    $('#_addUpdate').modal('hide');
                //}
            }
        })
    }

    $(document).ready(function () {
        self.GetAllPhongBan();
        self.GetAllChucVu();
        self.GetDataPaging();
        self.ValidateUser();    

        $("#Avatar").change(function () {
            var files = $(this).prop('files')[0];

            var t = files.type.split('/').pop().toLowerCase();

            if (t != "jpeg" && t != "jpg" && t != "png" && t != "gif") {
                alert('Vui lòng chọn một tập tin hình ảnh hợp lệ!');
                return false;
            }

            if (files.size > 2048000) {
                alert('Kích thước tải lên tối đa chỉ 2Mb');
                return false;
            }

            var img = new Image();
            img.src = URL.createObjectURL(files);
            img.onload = function () {
                CheckWidthHeight(this.width, this.height);
            }
            var CheckWidthHeight = function (w, h) {
                if (w <= 300 && h <= 300) {
                    alert("Ảnh tối thiểu 300 x 300 px");
                }
                else {
                    $(".avatar-img").attr("src", img.src);
                    self.UserAvatar = files;
                }
            }

        })


        $('#YearOfBirth').datepicker({
            changeMonth: true,
            changeYear: true,          
            showButtonPanel: true,
            yearRange: "1970:2040",
            dateFormat: 'dd/mm/yy',
        });

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
            $("#titleModal").text("Thêm mới dự án");
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;  
            $(".avatar-img").attr("src", "/public/images/avatar-mac-dinh.jpg");
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });
        $('#select-right-chu-vu').on('change', function () {
            $('input.form-search').val("");
            self.Search.PositionCode = $(this).val();
            if (self.Search.PositionCode != "") {
                self.Search.IsSearch = true;
            } else {
                self.Search.IsSearch = false;
            }
            //self.Search.IsSearch = true;
            self.GetDataPaging(true);
        });
        $('#select-right-tinh-trang').on('change', function () {
            $('input.form-search').val("");
            self.Search.Status = $(this).val();
            if (self.Search.Status != "") {
                self.Search.IsSearch = true;
            } else {
                self.Search.IsSearch = false;
            }
            //self.Search.IsSearch = true;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.Search.Name = $(this).val();
            //self.Search.IsSearch = true;
            if (self.Search.Name != "") {
                self.Search.IsSearch = true;
            } else {
                self.Search.IsSearch = false;
            }
            
            self.GetDataPaging(true);
        });
       
    })
})(jQuery);