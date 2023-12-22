using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PostDb>(opt => opt.UseInMemoryDatabase("Posts"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(
  opt => opt.AddPolicy("CorsPolicy", 
    policy => {
      // TODO: Allowed origin should be https://forum-io.fly.dev, but variable doesnt show up in Fly.io docker container
      var allowedOrigin = Environment.GetEnvironmentVariable("ASPNETCORE_ALLOWED_ORIGIN");
      if (allowedOrigin is string) policy.WithOrigins(allowedOrigin).AllowAnyHeader().AllowAnyMethod();
    }
));
var app = builder.Build();

app.UseCors("CorsPolicy");
app.Use( async(context, next) => {
  var allowedOrigin = Environment.GetEnvironmentVariable("ASPNETCORE_ALLOWED_ORIGIN");
  Console.WriteLine($"Allowed origin: {allowedOrigin}");
  if (allowedOrigin is string && context.Request.Headers.Referer.ToString().Contains(allowedOrigin)) await next();
  else {
    context.Response.Clear();
    context.Response.StatusCode = 403;
    await context.Response.WriteAsync("Forbidden");
  }
});

app.MapGet("/posts", async (PostDb db) => {
  return await db.Posts.Include(post => post.Comments).ToListAsync();
});
app.MapGet("/posts/{id}", async (int id, PostDb db) => {
  return await db.Posts.Include(post => post.Comments).FirstOrDefaultAsync(post => post.Id == id)
    is Post post ? Results.Ok(post) : Results.NotFound();
  
});

app.MapPost("/posts", async (Post post, PostDb db) =>
{
    db.Posts.Add(post);
    await db.SaveChangesAsync();

    return Results.Created($"/posts/{post.Id}", post.Id);
});


app.MapPost("/posts/{id}/comments", async (Comment comment, PostDb db) =>
{
    db.Comments.Add(comment);
    await db.SaveChangesAsync();

    return Results.Created($"/posts/{comment.Id}/comments", comment);
});

app.Run();
