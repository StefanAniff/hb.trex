using System;
using System.Collections.Generic;
using Trex.Core.Interfaces;
using Trex.ServiceContracts;

namespace Trex.Core.Services
{
    public interface IEntitySearchService
    {
        IEnumerable<IEntity> Result { get; }
        void Search(string searchString);
        event EventHandler SearchCompleted;
    }
}