using Microsoft.EntityFrameworkCore;
using Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/todoitems", async (Todo todo, TodoDbContext db) => 
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});


app.MapPut("/todoitems/{id}", async (int id, Todo todo, TodoDbContext todoDbContext) => 
{
    var isId = await todoDbContext.Todos.FindAsync(id);

    if (isId is null) return Results.NotFound();

    isId.Name = todo.Name;
    isId.IsCompleted = todo.IsCompleted;

    await todoDbContext.SaveChangesAsync();

    return Results.NoContent();
    
});


app.MapDelete("/todoitems/{id}", async (int id, TodoDbContext todoDbContext) => 
{

    if (await todoDbContext.Todos.FindAsync(id) is Todo todo)
    {
        todoDbContext.Remove(todo);
        await todoDbContext.SaveChangesAsync();

        return Results.Ok(todo);
    }

    return Results.NotFound();
    
});

app.MapGet("/todoitems", async (TodoDbContext todoDbContext) =>
    await todoDbContext.Todos.ToListAsync());


app.Map("/todoitems/complete", async (TodoDbContext todoDbContext) => 
    await todoDbContext.Todos.Where(t => t.IsCompleted).ToListAsync());


app.Map("/todoitems/{id}", async (int id, TodoDbContext todoDbContext) => 
    await todoDbContext.Todos.FindAsync(id)
    is Todo todo 
    ? Results.Ok(todo)
    : Results.NotFound());


app.Run();