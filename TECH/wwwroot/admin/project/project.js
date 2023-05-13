(function ($) {
    var self = this;
    self.IsUpdate = false;  
    self.Project = {
        Id: null,
        ProjectName: "",
        StartDate: null,
        EndDate: null,
        Status: 0,
        Note:""
    }
    self.Search = {
        name: "",
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
    self.MaAddThanhVien = 0,
    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
        };
    self.ThanhVienProjectExist = [];
    self.ThanhVienPhongExist = [];
    self.ThanhViens = [];
    self.MaPhongAddThanhVien = 0;
    self.Files = {};
    self.ThanhVienPhong = [];

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var htmlThanhVien = "";
                var editHtml = "";
                var statusCutom = item.StatusStr;
                if (item.IsDeleted != true) {
                    if (item.Status == 1) {
                        htmlThanhVien = "<button class='btn btn-primary custom-button' onClick='AddThanhVien(" + item.Id + ")' type='button'><i  class=\"fa-solid fa-users\"></i></button>";
                    }
                    editHtml = "<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id + ")\"><i  class=\"bi bi-pencil-square\"></i></button>" + htmlThanhVien +
                        "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.Id + ")\"><i  class=\"bi bi-trash\"></i></button>";
                } else {
                    statusCutom = "Đã xóa";
                }
                
                //else {
                //    htmlThanhVien = "<p class='custom-p' style='height:35px'></p>";
                //}
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + item.ProjectName + "</td>";
                html += "<td>" + item.StartDateStr + "</td>";
                html += "<td>" + item.EndDateStr + "</td>";
                html += "<td>" + statusCutom + "</td>";
                html += "<td>" + item.Note + "</td>";                
                html += "<td style=\"text-align: center;\">" + editHtml + "</td>";                
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
    // thêm thành viên start
    self.InitThanhVien = function () {
        self.GetAllEmployee();
        $(".btn-create-row-thanhvien").click(function () {
            $('#thanhvien table > tbody  > tr').each(function (index, tr) {
                // lấy id dịch vụ
                //var thanhvienphong = {
                //    EmployeeCode: 0,
                //    ProjectCode: self.MaPhongAddThanhVien
                //};
                var maThanhVien = $(tr).find('.dichvuselect').val();
                //if (maThanhVien != "") {
                //    thanhvienphong.EmployeeCode = maThanhVien;
                //}
                if (self.ThanhVienPhongExist != null && self.ThanhVienPhongExist.length > 0) {
                    if (maThanhVien != "" && self.ThanhVienPhongExist.indexOf(maThanhVien) == -1) {
                        self.ThanhVienPhongExist.push(maThanhVien);
                    }
                } else {
                    self.ThanhVienPhongExist.push(maThanhVien);
                }
               
            });
            var html = self.AddRowThanhVienHtml();
            $("#thanhvien #tblData").append(html);
            $(".no-data").hide();

        });
        $("#form-submit-thanhvien").on("submit", function (e) {
            e.preventDefault();

            $('#thanhvien table > tbody  > tr').each(function (index, tr) {
                // lấy id dịch vụ
                var thanhvienphong = {
                    EmployeeCode: 0,
                    ProjectCode: self.MaPhongAddThanhVien
                };
                var maThanhVien = $(tr).find('.dichvuselect').val();
                if (maThanhVien != "") {
                    thanhvienphong.EmployeeCode = maThanhVien;
                }

                self.ThanhVienPhong.push(thanhvienphong);
            });

            self.SaveThanhVienPhong(self.ThanhVienPhong, self.MaPhongAddThanhVien);
        })
    }
    self.SaveThanhVienPhong = function (dichVuPhong, maPhong) {
        if (dichVuPhong != null && dichVuPhong.length > 0) {
            var indexOf0 = dichVuPhong.findIndex(p => p.EmployeeCode == 0);
            if (indexOf0 >= 0) {
                dichVuPhong.splice(indexOf0,1);
            }
        }
        $.ajax({
            url: '/Project/AddThanhVienProject',
            type: 'POST',
            dataType: 'json',
            data: {
                ThanhVienProjectModelViews: dichVuPhong,
                maProject: maPhong
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('thêm thanh viên thành công', 'success');
                    $('#thanhvien').modal('hide');
                    self.ThanhVienProjectExist = [];
                    self.ThanhVienPhongExist = [];
                    setTimeout(function () {
                        window.location.reload();
                    },2000);
                   
                } else {
                    tedu.notify('thêm thanh viên không thành công', 'error');
                    $('#thanhvien').modal('hide');
                }
            }
        })
    }
    self.AddThanhVien = function (id) {
        $('#thanhvien').modal('show');
        self.MaPhongAddThanhVien = id;
        self.GetThanhVienPhongForPhongId(id);
    }

    self.GetAllEmployee = function () {
        $.ajax({
            url: '/Employee/GetAll',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                self.ThanhViens = response.Data;
            }
        });
    };

    self.GetThanhVienByIdOrAll = function (id, isRenderAPI) {
        var html = "";
        if (isRenderAPI) {
            html = "<select class=\"form-select colors dichvuselect\" disabled onchange='GetKhachHangSelect(this)'><option value=\"\"> Chọn Thành Viên </option>";
        }
        else {
            html = "<select class=\"form-select colors dichvuselect\" onchange='GetKhachHangSelect(this)' ><option value=\"\"> Chọn Thành Viên </option>";
        }
        if (self.ThanhViens != null && self.ThanhViens.length > 0) {
            for (var i = 0; i < self.ThanhViens.length; i++) {
                var item = self.ThanhViens[i];
                if (item.EmployeeCode == id) {
                    html += "<option value=\"" + item.EmployeeCode + "\" selected>" + (item.FullName != "" ? item.FullName : "") + "</option>";
                }
                else if (self.ThanhVienPhongExist.indexOf(item.EmployeeCode) == -1) {
                    html += "<option value=\"" + item.EmployeeCode + "\">" + (item.FullName != "" ? item.FullName : "") + "</option>";
                }
            }
        }
        html += "</select>";
        return html;
    }
    self.GetKhachHangSelect = function (tag) {
        var dichVuSelected = $(tag).val();
        var tagParent = $(tag).parent().parent();
        if (dichVuSelected != "") {
            if (self.ThanhViens != null && self.ThanhViens.length > 0) {
                var dichvu = self.ThanhViens.find(d => d.EmployeeCode == dichVuSelected);
                if (dichvu != null) {
                    $(tagParent.find('.sdt')).text(dichvu.SDT);
                    $(tagParent.find('.gioitinh')).text(dichvu.GenderStr);
                    $(tagParent.find('.diachi')).text(dichvu.Email);
                    //if (dichvu.LoaiDV == 4) {
                    //    $(tagParent.find('.soluongtheodichvu')).append('<input type=\"number\" class=\"form-control soluong\" min=\"0\" required>');
                    //} else {
                    //    $(tagParent.find('.soluongtheodichvu')).html('');
                    //}
                }
            }
        } else {
            $(tagParent.find('.dongia')).text("");
        }
    }
    self.AddRowThanhVienHtml = function () {
        var index = 0;
        var html = "<tr class=\"new\">";
        html += "<td>" + (++index) + "</td>";
        html += "<td>" + self.GetThanhVienByIdOrAll(0) + "</td>";
        html += "<td class ='sdt'></td>";
        html += "<td class='gioitinh'></td>";
        html += "<td class='diachi'></td>";
        //html += "<td></td>";
        html += "<td style=\"text-align: center;\">" +
            "<button  class=\"btn btn-danger custom-button\" onClick=\"DeletedHtml(this)\"><i  class=\"bi bi-trash custom-icon\"></i></button>" +
            "</td>";
        html += "</tr>";
        return html;
    }
    self.DeletedHtml = function (tag) {
        $(tag).closest(".new").remove();
        var employeeCode = $(tag).closest(".new").find('.dichvuselect').val();
        if (employeeCode != "") {
            if (self.ThanhVienPhongExist != null && self.ThanhVienPhongExist.length >0) {
                var indexOf = self.ThanhVienPhongExist.findIndex(p => p == employeeCode);
                if (indexOf >= 0) {
                    self.ThanhVienPhongExist.splice(indexOf, 1);
                }   
            }
                     
        }
    }
    self.GetThanhVienPhongForPhongId = function (id) {
        $.ajax({
            url: '/Project/GetThanhVienProjectByProjectId',
            type: 'GET',
            data: {
                id: id
            },
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                self.ThanhVienRenderTableHtml(response.Data);
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        self.ThanhVienPhongExist.push(item.EmployeeCode);
                    }
                }
                console.log(self.ThanhVienPhongExist);
            }
        });
    };

    self.ThanhVienRenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                var htmlEdit = "";
                //if (i != 0) {
                    htmlEdit = 
                        "<button type=\"button\" class=\"btn btn-danger custom-button\" onClick=\"DeletedThanhVien(" + item.Id + ")\"><i  class=\"bi bi-trash custom-icon\"></i></button>";
                //}
                html += "<tr data-quantity=" + item.Id + " class=" + item.Id + ">";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + self.GetThanhVienByIdOrAll(item.Employee.EmployeeCode, true) + "</td>";
                html += "<td class ='sdt'>" + item.Employee.SDT + "</td>";
                html += "<td class ='gioitinh'>" + item.Employee.GenderStr + "</td>";
                html += "<td class ='diachi'>" + item.Employee.Email + "</td>";
                html += "<td style=\"text-align: center;\">" + htmlEdit + "</td>";
                html += "</tr>";
            }
        }
        $("#thanhvien #tblData").html(html);
        $('#thanhvien').modal('show');
    };
    self.UpdateThanhVien = function (id) {
        if (id > 0) {
            /*self.ListUpdateQuantity.push(id);*/
            var classNameSelect = "." + id.toString() + " select";

            var classNameInput = "." + id.toString() + " .totalimport" + " , " + "." + id.toString() + " .soluong";
            var className = classNameSelect + "," + classNameInput;
            $(className).removeAttr('disabled');
        }
    }
    self.DeletedThanhVien = function (id) {
        if (id > 0) {
            var className = "." + id.toString();
            var employeeCode = $(className + " .dichvuselect").val();
            if (employeeCode != "") {
                if (self.ThanhVienPhongExist != null && self.ThanhVienPhongExist.length > 0) {
                    var indexOf = self.ThanhVienPhongExist.findIndex(p => p == employeeCode);
                    if (indexOf >= 0) {
                        self.ThanhVienPhongExist.splice(indexOf, 1);
                    }
                }
            }
            $(className).remove();
        }
    }
    // thêm thành viên end




    self.Update = function (id) {
        if (id != null && id != "") {
            $("#titleModal").text("Cập nhật");
            $(".btn-submit-format").text("Cập nhật");
            self.GetById(id, self.RenderHtmlByObject);
            self.Project.Id = id;
            $('#userModal').modal('show');
            $(".project-status").show();
            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Project/GetById',
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
            tedu.confirm('Bạn có chắc muốn xóa dự án này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Project/Delete",
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
        self.ThanhVienProjectExist = [];
        self.ThanhVienPhongExist = [];
        $.ajax({
            url: '/Project/GetAllPaging',
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
            url: '/Project/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                ProjectModelView: userView
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
            url: '/Project/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                ProjectModelView: userView
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
                ProjectName: {
                    required: true,
                },
                StartDate: {
                    required: true,
                },
                EndDate: {
                    required: true,
                },
                Status: {
                    required: true,
                }
            },
            messages:
            {
                ProjectName: {
                    required: "Tên dự án không được để trống",
                },
                StartDate: {
                    required: "Ngày bắt đầu không được để trống",
                },
                EndDate: {
                    required: "Ngày kết thúc không được để trống",
                },
                Status: {
                    required: "Vui lòng chọn trạng thái",
                }
            },
            submitHandler: function (form) {   
                self.GetValue();
                var startTimeConvert = Date.parse(self.Project.StartDate);
                var endTimeConvert = Date.parse(self.Project.EndDate);
                if (endTimeConvert < startTimeConvert) {
                    alert("Vui lòng chọn ngày kết thúc lớn hơn ngày bắt đầu");
                    /*window.location.reload();*/
                    return;
                }
                if (self.IsUpdate) {
                    self.UpdateUser(self.Project);
                }
                else {
                    self.AddUser(self.Project);
                }
            }
        });
    }

    self.GetValue = function () {
        self.Project.ProjectName = $("#ProjectName").val();
       /* self.Position.StartDate = $("#StartDate").val();*/
        //self.Position.EndDate = $("#EndDate").val();
        self.Project.Status = $("#Status").val();
        self.Project.Note = $("#Note").val();    
        self.Project.StartDate = $.datepicker.formatDate("yy-mm-dd", $("#StartDate").datepicker("getDate"));
        self.Project.EndDate = $.datepicker.formatDate("yy-mm-dd", $("#EndDate").datepicker("getDate"));
    }

    self.RenderHtmlByObject = function (view) {       
        $("#ProjectName").val(view.ProjectName);          
        $("#StartDate").val(view.StartDate);
        $("#EndDate").val(view.EndDate);
        $("#Status").val(view.Status);
        $("#Note").val(view.Note);
        $("#StartDate").datepicker("setDate", new Date(view.StartDateStr));
        $("#EndDate").datepicker("setDate", new Date(view.EndDateStr));
    }

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();    
        self.InitThanhVien();
        $('#EndDate,#StartDate').datepicker({
            changeMonth: true,
            changeYear: true,          
            showButtonPanel: true,
            dateFormat: 'dd/mm/yy',
        });
        $(".project-status").hide();

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
            $("#titleModal").text("Thêm mới dự án");
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;  
            $(".project-status").hide();
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