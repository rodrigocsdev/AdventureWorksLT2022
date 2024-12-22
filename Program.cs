using AdventureWorksLT2022.Repositories;
using AdventureWorksLT2022.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:44438") // URL do frontend Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddSingleton(new Connection(connectionString: builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();    

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
