﻿namespace PluginBase
{
    public interface IPlugin
    {
        string Name { get; }

        string Description { get; }

        void Execute();
    }
}