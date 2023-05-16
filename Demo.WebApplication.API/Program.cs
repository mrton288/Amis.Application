using Demo.WebApplication.BL.AccountBL;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.DepartmentBL;
using Demo.WebApplication.BL.EmployeeBL;
using Demo.WebApplication.BL.PayBL;
using Demo.WebApplication.BL.PayDetailBL;
using Demo.WebApplication.BL.SupplierBL;
using Demo.WepApplication.DL;
using Demo.WepApplication.DL.AccountDL;
using Demo.WepApplication.DL.BaseDL;
using Demo.WepApplication.DL.DepartmentDL;
using Demo.WepApplication.DL.EmployeeDL;
using Demo.WepApplication.DL.PayDetailDL;
using Demo.WepApplication.DL.PayDL;
using Demo.WepApplication.DL.SupplierDL;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// trả về JSON ParcalCase
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    //options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
}
);

// Add services to the container.

builder.Services.AddControllers();
//.AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition =
//System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection
builder.Services.AddScoped<Demo.WebApplication.BL.EmployeeBL.IPayDetailBL, EmployeeBL>();
builder.Services.AddScoped<Demo.WepApplication.DL.EmployeeDL.IPayDetailDL, EmployeeDL>();

builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();
builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();

builder.Services.AddScoped<IAccountBL, AccountBL>();
builder.Services.AddScoped<IAccountDL, AccountDL>();

builder.Services.AddScoped<IPayBL, PayBL>();
builder.Services.AddScoped<IPayDL, PayDL>();

builder.Services.AddScoped<Demo.WebApplication.BL.PayDetailBL.IPayDetailBL, PayDetailBL>();
builder.Services.AddScoped<Demo.WepApplication.DL.PayDetailDL.IPayDetailDL, PayDetailDL>();

builder.Services.AddScoped<ISupplierBL, SupplierBL>();
builder.Services.AddScoped<ISupplierDL, SupplierDL>();

builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));

DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("PostgreSql");

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
