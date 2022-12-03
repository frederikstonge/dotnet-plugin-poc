namespace WebshopTests
{
    public interface IApplicationLifetime
    {
        CancellationTokenSource? CancellationTokenSource { get; }
        bool StartRequest { get; }

        void Restart();
        void Start();
    }
}