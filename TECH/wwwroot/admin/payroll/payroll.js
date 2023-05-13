(function ($) {
    var self = this;
    self.IsUpdate = false;  
    self.Payroll = {
        Id: null,
        Basicsalary: 0,
        SalaryKPI:0,
        EmployeeCode:"",
        Note:""
    }
    self.Search = {
        Name: "",
        PositionCode: "",
        IsSearch: null,
        Status: 0,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
    self.ThanhVienPhong = [];
    self.BonusPunishId = 0;
    self.EmployeeCode = "";
    self.ThanhViens = [];
    self.MaPhongAddThanhVien = 0;
    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };

    self.RenderTableHtml = function (data) {
        //var html = "";
        //if (data != "" && data.length > 0) {
        //    var index = 0;
        //    for (var i = 0; i < data.length; i++) {
        //        var item = data[i];
        //        var thuong = "";
        //        var phat = "";
        //        if (item.BonusPunish != null && item.BonusPunish.length > 0) {
        //            thuong = item.TotalBonusStr != null && item.TotalBonusStr != "" ? item.TotalBonusStr : "";
        //            phat = item.TotalPunishStr != null && item.TotalPunishStr != "" ? item.TotalPunishStr : "";
        //        }
        //        html += "<tr>";
        //        html += "<td>" + (++index) + "</td>";
        //        html += "<td>" + item.EmployeeCode + "</td>";
        //        html += "<td>" + item.Employee.FullName + "</td>";
        //        html += "<td>" + item.Coefficient + "</td>";
        //        html += "<td>" + item.BasicsalaryStr + "</td>";
        //        html += "<td>" + item.SalaryKPIStr + "</td>";
        //        html += "<td>" + thuong + "</td>";
        //        html += "<td>" + phat + "</td>";
        //        html += "<td>" + item.TotalPenaltyBonusStr + "</td>";
        //        //html += "<td>" + item.Note + "</td>";

        //        html += "<td style=\"text-align: center;\">" +
        //            //"<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id + ")\"><i  class=\"bi bi-pencil-square\"></i></button>" +
        //            "<button  class=\"btn btn-primary custom-button\" onClick=\"AddThuongPhat(" + item.Id + ")\"><i  class=\"fa-solid fa-file-invoice-dollar\"></i></button>" +
        //            "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.Id +")\"><i  class=\"bi bi-trash\"></i></button>" +
        //            "</td>";
        //        html += "</tr>";
        //    }
        //}
        //else {
        //    html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        //}
        //$("#tblData").html(html);
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                //var editButton = "";
                //if (item.IsDeleted != true) {
                //    editButton = "<button  class=\"btn btn-primary custom-button\" onClick=\'Update(\"" + item.EmployeeCode + "\")\'><i  class=\"bi bi-pencil-square\"></i></button>" +
                //        "<button  class=\"btn btn-danger custom-button\" onClick=\'Deleted(\"" + item.EmployeeCode + "\")\'><i  class=\"bi bi-trash\"></i></button>";
                //}
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";               
                html += "<td>" + item.FullName + "</td>";
                html += "<td>" + item.RoomCodeStr + "</td>";
                html += "<td>" + item.PositionCodeStr + "</td>";
                html += "<td>" + item.Coefficient + "</td>";
                html += "<td>" + (item.Basicsalary != null ? item.Basicsalary:"") + "</td>";
                html += "<td>" + (item.TotalBonusStr != null ? item.TotalBonusStr :"")+ "</td>";
                html += "<td>" + (item.TotalPunishStr != null ? item.TotalPunishStr:"") + "</td>";
                html += "<td>" + (item.TotalPenaltyBonusStr != null ? item.TotalPenaltyBonusStr:"") + "</td>";
                //html += "<td>" + item.GenderStr + "</td>";
                //html += "<td>" + item.PositionCodeStr + "</td>";
                //html += "<td>" + item.RoomCodeStr + "</td>";
                //html += "<td>" + item.IsDeletedStr + "</td>";
                        html += "<td style=\"text-align: center;\">" +
                            //"<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id + ")\"><i  class=\"bi bi-pencil-square\"></i></button>" +
                            "<button  class=\"btn btn-primary custom-button\" onClick=\'AddThuongPhat(\"" + item.EmployeeCode + "\")\'><i  class=\"fa-solid fa-file-invoice-dollar\"></i></button>" +
                            "<button  class=\"btn btn-danger custom-button\" onClick=\'Deleted(\"" + item.EmployeeCode + "\")\'><i  class=\"bi bi-trash\"></i></button>" +
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
            $("#titleModal").text("Cập nhật lương");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.Payroll.Id = id;
            $('#userModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Payroll/GetById',
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
                        self.Id = id;
                        
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
            tedu.confirm('Bạn có chắc muốn xóa này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Employee/DeleteThuongPhat",
                    data: { employeeCode: id },
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
            url: '/Payroll/GetAllPaging',
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
            url: '/Payroll/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                PayrollModelView: userView
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
            url: '/Payroll/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                PayrollModelView: userView
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
                  /*  tedu.notify('Tên dịch vụ đã tồn tại', 'error');*/
                }
               
            }
        })
    }

    self.ValidateUser = function () {                
        $("#form-submit").validate({
            rules:
            {
                Basicsalary: {
                    required: true,
                },
                EmployeeCode: {
                    required: true,
                }
            },
            messages:
            {
                Basicsalary: {
                    required: "Lương cơ bản không được để trống",
                },
                EmployeeCode: {
                    required: "Bạn chưa chọn nhân viên",
                }
            },
            submitHandler: function (form) {   
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.Payroll);
                }
                else {
                    self.AddUser(self.Payroll);
                }
            }
        });
    }

    self.GetValue = function () {
        self.Payroll.Basicsalary = $("#Basicsalary").val();
        self.Payroll.SalaryKPI = $("#SalaryKPI").val();     
        self.Payroll.EmployeeCode = $("#EmployeeCode").val();     
        self.Payroll.Note = $("#Note").val();     
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
                if (response.Data != null && response.Data.length > 0) {
                    var html = "<select class=\"form-select colors dichvuselect\"><option value=\"\"> Chọn nhân Viên </option>";                   
                    if (self.ThanhViens != null && self.ThanhViens.length > 0) {
                        for (var i = 0; i < response.Data.length; i++) {
                            var item = response.Data[i];
                            html += "<option value=\"" + item.EmployeeCode + "\">" + (item.FullName != "" ? item.FullName : "") + "</option>";
                        }
                    }
                    html += "</select>";
                    $("#EmployeeCode").html(html);
                    $(".EmployeeCodeSelected").html(html);
                }
            }
        });
    };

    self.RenderHtmlByObject = function (view) {
        $("#Basicsalary").val(view.Basicsalary);
        $("#SalaryKPI").val(view.SalaryKPI); 
        $("#EmployeeCode").val(view.EmployeeCode);
        $("#Note").val(view.Note); 
    }
    // thưởng phạt start

    // thêm thành viên start
    self.InitThanhVien = function () {
       /* self.GetAllEmployee();*/
        $(".btn-create-row-thanhvien").click(function () {
            var html = self.AddRowThanhVienHtml();
            $("#thuongphatshow #tblDataThuongPhat").append(html);
            $(".no-data").hide();
        });
        $("#form-submit-thanhvien").on("submit", function (e) {
            e.preventDefault();

            $('#thuongphatshow table > tbody  > tr').each(function (index, tr) {
                // lấy id dịch vụ
                var thanhvienphong = {
                    RewardType: 0,
                    RewardTypeName: "",
                    PenaltyBonus: "",
                    Note:"",
                    EmployeeCode: self.EmployeeCode
                };
               
                thanhvienphong.RewardType = $(tr).find('.RewardType').val();
                thanhvienphong.RewardTypeName = $(tr).find('.RewardTypeName').val();
                thanhvienphong.PenaltyBonus = $(tr).find('.PenaltyBonus').val();
                thanhvienphong.Note = $(tr).find('.Note').val();

                self.ThanhVienPhong.push(thanhvienphong);
            });

            self.SaveThanhVienPhong(self.ThanhVienPhong, self.EmployeeCode);
        })
    }
    self.SaveThanhVienPhong = function (dichVuPhong, employeeCode) {
        $.ajax({
            url: '/Employee/AddTThuongPhat',
            type: 'POST',
            dataType: 'json',
            data: {
                BonusPunishModelViews: dichVuPhong,
                employeeCode: employeeCode
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật thành công', 'success');
                    $('#thuongphatshow').modal('hide');
                    window.location.reload();
                }
            }
        })
    }
    self.AddThuongPhat = function (id) {
        $('#thuongphatshow').modal('show');
        self.EmployeeCode = id;
        self.GetThanhVienPhongForPhongId(id);
    }

    //self.GetAllEmployee = function () {
    //    $.ajax({
    //        url: '/Employee/GetAll',
    //        type: 'GET',
    //        dataType: 'json',
    //        beforeSend: function () {
    //        },
    //        complete: function () {
    //        },
    //        success: function (response) {
    //            self.ThanhViens = response.Data;
    //        }
    //    });
    //};

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

    self.RenderLoaiThuongPhat = function (id) {
        var html = "";
        html = "<select class=\"form-select colors custom-input RewardType \"" + (id > 0 ?"disabled":"")+" ><option value=\"\" id='RewardType'> Chọn Loại </option>";      
        html += "<option value='1' " + (id == 1 ? "selected" :"")+">Thưởng</option>";
        html += "<option value='2' " + (id == 2 ? "selected" : "") +">Phạt</option>";        
        html += "</select>";
        return html;
    }

    self.AddRowThanhVienHtml = function () {
        var index = 0;
        var html = "<tr class=\"new\">";
        html += "<td>" + (++index) + "</td>";
        html += "<td>" + self.RenderLoaiThuongPhat(0) + "</td>";
        html += "<td id ='RewardTypeName'><input type=\"text\" class=\"form-control RewardTypeName\" required></td>";
        html += "<td id='PenaltyBonus'><input type=\"text\" class=\"form-control PenaltyBonus\" required></td>";
        html += "<td id='Note'><input type=\"text\" class=\"form-control Note\"></td>";       
        //html += "<td></td>";
        html += "<td style=\"text-align: center;\">" +
            "<button  class=\"btn btn-danger custom-button\" onClick=\"DeletedHtml(this)\"><i  class=\"bi bi-trash custom-icon\"></i></button>" +
            "</td>";
        html += "</tr>";
        return html;
    }
    self.DeletedHtml = function (tag) {
        $(tag).closest(".new").remove();
    }
    self.GetThanhVienPhongForPhongId = function (id) {
        $.ajax({
            url: '/Employee/GetThuongPhatEmployee',
            type: 'GET',
            data: {
                employeeCode: id
            },
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                self.ThanhVienRenderTableHtml(response.Data);
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
                htmlEdit = "<button type=\"button\" class=\"btn btn-primary custom-button\" onClick=\"UpdateThanhVien(" + item.Id + ")\"><i  class=\"bi bi-pencil-square custom-icon\"></i></button>" +
                    "<button type=\"button\" class=\"btn btn-danger custom-button\" onClick=\"DeletedThanhVien(" + item.Id + ")\"><i  class=\"bi bi-trash custom-icon\"></i></button>";
                html += "<tr data-quantity=" + item.Id + " class=" + item.Id + ">";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + self.RenderLoaiThuongPhat(item.RewardType) + "</td>";
                html += "<td id ='RewardTypeName'><input type=\"text\" class=\"form-control custom-input RewardTypeName\" value='" + item.RewardTypeName +"' disabled /></td>";
                html += "<td id='PenaltyBonus'><input type=\"text\" class=\"form-control custom-input PenaltyBonus\" value='" + item.PenaltyBonus +"' disabled /></td>";
                html += "<td id='Note'><input type=\"text\" class=\"form-control custom-input Note\" value='" + item.Note +"' disabled /></td>";    
                html += "<td style=\"text-align: center;\">" + htmlEdit + "</td>";
                html += "</tr>";
            }
        }
        $("#thuongphatshow #tblDataThuongPhat").html(html);
        $('#thuongphatshow').modal('show');
    };
    self.UpdateThanhVien = function (id) {
        if (id > 0) {
            /*self.ListUpdateQuantity.push(id);*/
            var classNameSelect = "." + id.toString() + " select";

            var classNameInput = "." + id.toString() + " input";
            var className = classNameSelect + "," + classNameInput;
            $(className).removeAttr('disabled');
        }
    }
    self.DeletedThanhVien = function (id) {
        if (id > 0) {
            var className = "." + id.toString();
            $(className).remove();
        }
    }
    // thêm thành viên end



    // thưởng phạt end

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();    
        self.GetAllEmployee();
        self.InitThanhVien();
        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
            $("#titleModal").text("Thêm mới phòng ban");
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;  
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });
        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            self.Search.Name = $(this).val();
            if (self.Search.Name != "") {
                self.Search.IsSearch = true;
            } else {
                self.Search.IsSearch = false;
            }
            //self.Search.IsSearch = true;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.Search.Name = $(this).val();
            self.GetDataPaging(true);
        });
       
    })
})(jQuery);