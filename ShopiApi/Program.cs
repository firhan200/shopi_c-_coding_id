using ShopiApi.Repositories;

string myCORSKey = "shopiCORSKey";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myCORSKey,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5171");
                          policy.WithOrigins("http://localhost:5172");
                          policy.WithOrigins("http://localhost:5173");
                      });
});

builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(myCORSKey);

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
