using Microsoft.EntityFrameworkCore;

namespace SimpleTodoManager
{
    public static class TodoController
    {
        public static void RegisterTodoEndpoints(this WebApplication app) {

            app.MapGet("/listTodos/All", ListAllTodos);
            app.MapGet("/listTodos/byStatus", ListByStatus);
            app.MapPost("/listTodos/byDate", ListByDate);
            app.MapPost("/createTodo", CreateTodo);
            app.MapPut("/updateTodo", UpdateTodo);
            app.MapDelete("/deleteTodo", DeleteTodo);
        }

        static async Task<IResult> ListAllTodos(TodoDb db) {
            return TypedResults.Ok(await db.Todos.ToArrayAsync());
        }

        static async Task<IResult> ListByStatus(Status status, TodoDb db) {
            return TypedResults.Ok(await db.Todos.Where(t => t.status.Equals(status)).ToListAsync());
        }

        static async Task<IResult> ListByDate(DateOnly endDate, TodoDb db) {
            return TypedResults.Ok(await db.Todos.Where(t => t.endDate.Equals(endDate)).ToListAsync());
        }

        static async Task<IResult> CreateTodo(Todo todo, TodoDb db) {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/todoitems/{todo.Id}", todo);
        }

        static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db) {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return Results.NotFound();

            todo.title = inputTodo.title;
            todo.description = inputTodo.description;
            todo.endDate = inputTodo.endDate;
            todo.status = inputTodo.status;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteTodo(int id, TodoDb db) {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}
