using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TasksFilesApi.Services.Interfaces
{
    public interface IStorageService
    {
        Task<byte[]> GetAsync(Guid key);
        Task<Guid> SaveAsync(byte[] data);
        void Delete(Guid key);
        Task<Tuple<string, byte[]>> CreateArchiveAsync(IDictionary<string, Guid> files);
    }
}
