namespace WebshopTests
{
    public class ApplicationLifetime : IApplicationLifetime
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _startRequest;

        public ApplicationLifetime()
        {
            _startRequest = true;
        }

        public bool StartRequest => _startRequest;

        public CancellationTokenSource? CancellationTokenSource => _cancellationTokenSource;

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _startRequest = false;
        }

        public void Restart()
        {
            _startRequest = true;
            _cancellationTokenSource?.Cancel();
        }
    }
}
