using Pavas.Patterns.Context.DependencyInjection;
using Pavas.Patterns.UnitOfWork.Contracts;
using Pavas.Patterns.UnitOfWork.DependencyInjection;
using UnitOfWork.Example;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingletonContext(new TenantContext { TenantId = "MyTenant2" });
builder.Services.AddUnitOfWork<UnitOfWorkContext, UnitOfWorkConfigurator>(ServiceLifetime.Singleton);

var app = builder.Build();

var unitOfWork = app.Services.GetRequiredService<IUnitOfWork>();
var repository = await unitOfWork.GetRepositoryAsync<MyEntity, RepositoryConfigurator>();
var entities = await repository.GetAllAsync();

Console.WriteLine(entities);

await unitOfWork.SaveChangesAsync();

app.Run();