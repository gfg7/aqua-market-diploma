namespace AquaServer.Interfaces.Services
{
    public interface IPasswordService
    {
        bool Compare(string hash, string salt, string input);

        (string, string) Generate(string pass = null);
    }
}
