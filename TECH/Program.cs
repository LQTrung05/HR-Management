using Microsoft.EntityFrameworkCore;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
});
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddDbContext<DataBaseEntityContext>(options =>
{
    // Đọc chuỗi kết nối
    string connectstring = builder.Configuration.GetConnectionString("AppDbContext");
    options.UseSqlServer(connectstring);
});
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(EFUnitOfWork));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IBonusPunishRepository, BonusPunishRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeProjectRepository, EmployeeProjectRepository>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();

builder.Services.AddScoped<IBonusPunishService, BonusPunishService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
//builder.Services.AddScoped<IBonusPunishService, BonusPunishService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
//builder.Services.AddScoped<IEmployeeProjectService, EmployeeProjectService>();
//builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEmployeeProjectService, EmployeeProjectService>();


//builder.Services.AddMemoryCache();

// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Quantri",
      pattern: "/quan-ly-he-thong",
      defaults: new { controller = "Home", action = "Index" });
    endpoints.MapControllerRoute(
       name: "Phong",
       pattern: "/quan-ly-phong-ban",
       defaults: new { controller = "Department", action = "Index" });

    endpoints.MapControllerRoute(
      name: "ChucVu",
      pattern: "/quan-ly-chuc-vu",
      defaults: new { controller = "Position", action = "Index" });


    endpoints.MapControllerRoute(
      name: "DuAn",
      pattern: "/quan-ly-du-an",
      defaults: new { controller = "Project", action = "Index" });

    endpoints.MapControllerRoute(
       name: "NhanVien",
       pattern: "/quan-ly-nhan-vien",
       defaults: new { controller = "Employee", action = "Index" });

    endpoints.MapControllerRoute(
       name: "userlogin",
       pattern: "/dang-nhap",
       defaults: new { controller = "Users", action = "Login" });
    endpoints.MapControllerRoute(
      name: "Luong",
      pattern: "/quan-ly-luong",
      defaults: new { controller = "Employee", action = "Luong" });

    endpoints.MapControllerRoute(
      name: "UserDetail",     
      pattern: "/thong-tin-ca-nhan",
      defaults: new { controller = "Users", action = "ViewDetail" });

    endpoints.MapControllerRoute(
      name: "ChangePass",
      pattern: "/doi-mat-khau",
      defaults: new { controller = "Users", action = "ChangePassWord" });
    endpoints.MapControllerRoute(
name: "DangXuat",
pattern: "/dang-xuat",
defaults: new { controller = "Users", action = "LogOut" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Users}/{action=Login}/{id?}");


});


//app.MapControllerRoute(

//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
