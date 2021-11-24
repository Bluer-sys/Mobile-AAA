using System.Collections.Generic;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public class PersistentProgressWatchersService : IPersistentProgressWatchersService
    {
        public List<ISavedProgressReader> ProgressReaders { get; set; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; set; } = new List<ISavedProgress>();
        
        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
    }
}