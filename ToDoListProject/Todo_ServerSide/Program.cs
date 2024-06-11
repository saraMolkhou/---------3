using TodoApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
// using System.Web.Http;

var builder = WebApplication.CreateBuilder(args);//מופע של האפליקציה 

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
                     new MySqlServerVersion(new Version(8, 0, 36))));// חיבור למסד נתונים 

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });// מגדיר מסמך של סוואגר בשביל להתחבר לממשק של סוואגר ויזואלי 
});
builder.Services.AddControllers();// מגדיר לאפליקציה לגשת למטודות לפי בקשות ה HTTP

builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

}));// מוסיף קורס שזה מאפשר לנו שהקליינט יוכל לגשת לסרבר גם אם הם רצים בפורטים שונים

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

app.UseHttpsRedirection();//  מ HTTP ל HTTPS גישה מאובטחת

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CodeTimeSwaggerDemo v1");
        c.RoutePrefix = "swagger";
    });
}
app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapGet("/items", async (ToDoDbContext dbContext) =>
{
    // console.WriteLine("i am in the get method in the api")
    var items = await dbContext.Items.ToListAsync();
    return Results.Ok(items);
});

// POST a new item
app.MapPost("/items", async (Item item, ToDoDbContext dbContext) =>
{
        Console.WriteLine("im in the post method");
        await dbContext.Items.AddAsync(item);
        await dbContext.SaveChangesAsync();
        return Results.Ok(item);
});

// PUT method to update an existing item
app.MapPut("/items/{id}", async (int id, Item updatedItem, ToDoDbContext dbContext) =>
{
        // console.WriteLine("i am in the put method in the api")
    var item = await dbContext.Items.FindAsync(id);
    if (item == null)
        return Results.NotFound();

    item.Name = updatedItem.Name;
    item.IsComplete = updatedItem.IsComplete;

    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

// DELETE method to delete an existing item
app.MapDelete("/items/{id}", async (int id, ToDoDbContext dbContext) =>
{
    Console.WriteLine("i am in the delete method in the api");
    var item = await dbContext.Items.FindAsync(id);
    if (item == null)
        return Results.NotFound();

    dbContext.Items.Remove(item);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
