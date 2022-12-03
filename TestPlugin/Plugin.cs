using PluginBase;

namespace TestPlugin
{
    public class Plugin : IPlugin
    {
        public string Name => "Test plugin";

        public string Description => "Test description";

        public void Execute()
        {
            Console.WriteLine("It works");
        }
    }
}