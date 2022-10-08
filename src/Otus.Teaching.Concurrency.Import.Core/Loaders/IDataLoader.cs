using System;

namespace Otus.Teaching.Concurrency.Import.Core.Loaders
{
    public interface IDataLoader
    {
        event Action<string> DisplayMessage;

        void LoadData();
    }
}