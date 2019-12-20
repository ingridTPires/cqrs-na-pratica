using API.Utils;
using Logica.Alunos;
using Logica.Decorators;
using Logica.Models;
using Logica.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(new SessionFactory(Configuration["ConnectionString"]));

            var config = new Config(3);
            services.AddSingleton(config);

            //services.AddTransient<ICommandHandler<EditarInformacoesPessoaisCommand>, EditarInformacoesPessoaisCommandHandler>();
            services.AddTransient<ICommandHandler<EditarInformacoesPessoaisCommand>>(provider =>
            {
                return new AuditLoggingDecorator<EditarInformacoesPessoaisCommand>(new 
                    DatabaseRetryDecorator<EditarInformacoesPessoaisCommand>(new EditarInformacoesPessoaisCommandHandler
                    (provider.GetService<SessionFactory>()), provider.GetService<Config>()));
            });

            services.AddTransient<ICommandHandler<RegistrarAlunoCommand>, RegistrarAlunoCommandHandler>();
            services.AddTransient<ICommandHandler<DesregistrarAlunoCommand>, DesregistrarAlunoCommandHandler>();
            services.AddTransient<ICommandHandler<InscreverAlunoCommand>, InscreverAlunoCommandHandler>();
            services.AddTransient<ICommandHandler<TransferirAlunoCommand>, TransferirAlunoCommandHandler>();
            services.AddTransient<ICommandHandler<DesinscreverAlunoCommand>, DesinscreverAlunoCommandHandler>();

            services.AddTransient<IQueryHandler<RecuperarAlunosQuery, List<AlunoDto>>, RecuperarAlunosQueryHandler>();


            services.AddSingleton<Messages>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
