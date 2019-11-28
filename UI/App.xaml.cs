using UI.API;

namespace UI
{
    public sealed partial class App
    {
        public App()
        {
            ApiClient.Init("http://localhost:49500/api/alunos");
        }
    }
}
