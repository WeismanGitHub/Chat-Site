using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDB>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/items", async (TodoDB db) => 
    await db.Todos.ToListAsync());

app.MapGet("/items/complete", async (TodoDB db) => 
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/items/{id}", async (int id, TodoDB db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/items", async (Todo todo, TodoDB db) => {
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.ID}", todo);
});

app.MapPut("/items/{id}", async (int id, Todo inputTodo, TodoDB db) => {
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int id, TodoDB db) => {
    if (await db.Todos.FindAsync(id) is Todo todo) {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();