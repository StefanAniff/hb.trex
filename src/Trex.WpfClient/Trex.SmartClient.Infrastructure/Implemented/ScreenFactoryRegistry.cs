using System;
using System.Collections.Generic;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class ScreenFactoryRegistry : IScreenFactoryRegistry
    {
        private readonly Dictionary<Guid , IScreenFactory> _screenFactoryDictionary;

        public ScreenFactoryRegistry()
        {
            _screenFactoryDictionary = new Dictionary<Guid, IScreenFactory>();
        }

        public void RegisterFactory(Guid guid, IScreenFactory screenFactory)
        {
            if (!_screenFactoryDictionary.ContainsKey(guid))
                _screenFactoryDictionary.Add(guid, screenFactory);
        }

        public IScreenFactory GetFactory(Guid guid)
        {
            if (_screenFactoryDictionary.ContainsKey(guid))
                return _screenFactoryDictionary[guid];
            else
            {
                return null;
            }
        }

        public int Count
        {
            get { return _screenFactoryDictionary.Count; }
        }
    }
}