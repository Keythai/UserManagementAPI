using UserManagementAPI.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Register the exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Register the token validation middleware
app.UseMiddleware<TokenValidationMiddleware>();

// Register the request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

var userRoutes = app.MapGroup("/api/users");

userRoutes.MapGet("/", () =>
{
    try
    {
        return Results.Ok(User.GetAll());
    }
    catch (Exception)
    {
        return Results.Problem("An unexpected error occurred while retrieving users.");
    }
});

userRoutes.MapGet("/{id}", (int id) =>
{
    try
    {
        return User.Get(id) is User user ? Results.Ok(user) : Results.NotFound();
    }
    catch (Exception)
    {
        return Results.Problem("An unexpected error occurred while retrieving the user.");
    }
});

userRoutes.MapPost("/", (User user) =>
{
    Console.WriteLine($"Received user: {user}");
    try
    {
        var createdUser = User.Add(user);
        return Results.Created($"/api/users/{createdUser.Id}", createdUser);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error adding user: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An unexpected error occurred while adding the user: {ex.Message}");
        return Results.Problem("An unexpected error occurred while adding the user.");
    }
});

userRoutes.MapPut("/{id}", (int id, User inputUser) =>
{
    Console.WriteLine($"Received user: {inputUser}");
    try
    {
        return User.Update(id, inputUser) ? Results.NoContent() : Results.NotFound();
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error updating user: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An unexpected error occurred while updating the user: {ex.Message}");
        return Results.Problem("An unexpected error occurred while updating the user.");
    }
});

userRoutes.MapDelete("/{id}", (int id) =>
{
    try
    {
        return User.Delete(id) ? Results.NoContent() : Results.NotFound();
    }
    catch (Exception)
    {
        return Results.Problem("An unexpected error occurred while deleting the user.");
    }
});

userRoutes.MapGet("/error", () =>{
    // Deliberate error to trigger ExceptionHandlingMiddleware
    throw new Exception("Deliberate exception to test middleware");
});

User.Add(new User { Id = 1, Name = "Alice", Email = "alice@example.com" });
User.Add(new User { Id = 2, Name = "Bob", Email = "bob@example.com" });
User.Add(new User { Id = 3, Name = "Charlie", Email = "charlie@example.com" });
User.Add(new User { Id = 4, Name = "Diana", Email = "diana@example.com" });
User.Add(new User { Id = 5, Name = "Eve", Email = "eve@example.com" });

app.Run();