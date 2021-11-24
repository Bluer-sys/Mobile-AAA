using System.Collections.Generic;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public interface IPersistentProgressWatchersService : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        void CleanUp();
    }
}