﻿using Identity.IdentityPolicy;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<AppUser, IdentityRole>().
AddEntityFrameworkStores<AppIdentityDbContext>().
AddDefaultTokenProviders();

builder.Services.AddDbContext<AppIdentityDbContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
    );

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
});

builder.Services.ConfigureApplicationCookie(
        opts =>
        {
            //默认登录页面
            opts.LoginPath = "/Account/Login";
            opts.AccessDeniedPath = "/Account/AccessDenied";
            //设置Cookie名称
            opts.Cookie.Name = ".AspNetCore.Identity.Application";
            //设置Cookie超时时间
            opts.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            //设置滑动时间
            opts.SlidingExpiration = true;
        }
    );

// Add services to the container.
builder.Services.AddControllersWithViews();
#region 自定义密码、用户名、邮箱策略
//builder.Services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
//builder.Services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();