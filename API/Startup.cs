using API.Utils;
using Logica.Alunos;
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
            services.AddTransient<UnitOfWork>();

            services.AddTransient<ICommandHandler<EditarInformacoesPessoaisCommand>, EditarInformacoesPessoaisCommandHandler>();
            services.AddTransient<ICommandHandler<RegistrarAlunoCommand>, RegistrarAlunoCommandHandler>();
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
