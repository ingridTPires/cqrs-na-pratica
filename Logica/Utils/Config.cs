namespace Logica.Utils
{
    public sealed class Config
    {
        public Config(int numeroDeTentativasNoBanco)
        {
            NumeroDeTentativasNoBanco = numeroDeTentativasNoBanco;
        }
        public int NumeroDeTentativasNoBanco { get; }
    }

}
