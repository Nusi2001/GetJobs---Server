using GetJobsBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("GetJobsDb"));
builder.Services.AddCors();

var app = builder.Build();

// Middleware
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

//  Serve Angular frontend
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

//  Fallback to index.html for Angular routing
app.MapFallbackToFile("index.html");

// Optional Seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Jobs.AddRange(
        new GetJobsBackend.Models.Job { Title = "Frontend Developer", Domain = "Web", Description = "Angular dev" },
        new GetJobsBackend.Models.Job { Title = "Data Scientist", Domain = "AI", Description = "ML Expert" }
    );
    context.SaveChanges();
}

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();