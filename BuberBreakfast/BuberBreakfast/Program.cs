using BuberBreakfast.Services.Breakfasts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
{
  builder.Services.AddControllers();
  builder.Services.AddScoped<IBreakfastService, BreakfastService>();
}

var app = builder.Build();
{
  app.UseExceptionHandler("/error");
  app.UseHttpsRedirection();
  app.MapControllers();
  app.Run();
}
