using ExampleServiceNov2018.Command.Model;
using ExampleServiceNov2018.Command.Persistense;
using ExampleServiceNov2018.CommandStack;
using ExampleServiceNov2018.Query.Model;
using ExampleServiceNov2018.Query.Persistense;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlStreamStore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ExampleServiceNov2018.Api
{
    public class Startup
    {
        private const string _connectionString =
            @"Data Source=HBH-PC\SQLEXPRESS;Initial Catalog=TestDB;Trusted_Connection=True;";

        private static readonly MsSqlStreamStoreSettings _msSqlStreamStoreSettings
            = new MsSqlStreamStoreSettings(_connectionString);

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //Register infrastructure
            SetupStreamStore(services);
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
            services.AddScoped<ITodoReadService, TodoReadService>();

            //Register projections and readservices
            services.AddSingleton(
                new ReadConnection(_connectionString)); //using the same sql db as write side (for now)            
            services.AddSingleton<IHostedService, TodoReadServiceHost>();

            //Register mediator
            services.AddMediatR();
            services.AddMediatR(typeof(TodoCommandHandler).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();
        }

        private void SetupStreamStore(IServiceCollection services)
        {
            var streamStore = new MsSqlStreamStore(_msSqlStreamStoreSettings);
            services.AddSingleton<IStreamStore>(x => streamStore);
            var schema = streamStore.CheckSchema().GetAwaiter().GetResult();
            if (!schema.IsMatch())
                streamStore.CreateSchema();
        }
    }
}