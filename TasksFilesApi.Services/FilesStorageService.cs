using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using TasksFilesApi.Services.Interfaces;

namespace TasksFilesApi.Services
{
    public class FilesStorageService : IStorageService
    {
        private const string EmptyGuidError = "Guid can not be empty";
        private const string GuidNotFoundErrorMessage = "File with '{0}' not found.";
        private const string DateTimeFormat = "yyyy_MM_dd-HH_mm_ss";
        private const string ArchiveName = "files-{0}.zip";
        private const string FileFormat = ".data";

        private readonly IConfiguration Configuration;

        public FilesStorageService(IConfiguration configuration)
        {
            Configuration = configuration;

            InitDirectory();
        }

        private void InitDirectory()
        {
            if (!Directory.Exists(Configuration["FilesPath"]))
                Directory.CreateDirectory(Configuration["FilesPath"]);
        }

        public async Task<byte[]> GetAsync(Guid key)
        {
            if (key == Guid.Empty)
                throw new Exception(EmptyGuidError);

            var file = GetFileName(key);

            if (File.Exists(file))
                return await File.ReadAllBytesAsync(file);

            throw new Exception(string.Format(GuidNotFoundErrorMessage, key));
        }

        public async Task<Guid> SaveAsync(byte[] data)
        {
            if (data == null)
                return Guid.Empty;

            var key = Guid.NewGuid();
            var file = GetFileName(key);

            await File.WriteAllBytesAsync(file, data);
            return key;
        }

        public void Delete(Guid key)
        {
            if (key == Guid.Empty)
                throw new Exception(EmptyGuidError);

            var file = GetFileName(key);
            if (File.Exists(file))
            {
                File.Delete(file);
                return;
            }

            throw new Exception(string.Format(GuidNotFoundErrorMessage, key));
        }

        private string GetFileName(Guid key)
        {
            return Path.Combine(Configuration["FilesPath"], key.ToString() + FileFormat);
        }

        public async Task<Tuple<string, byte[]>> CreateArchiveAsync(IDictionary<string, Guid> files)
        {
            if (files?.Count == 0)
                return null;

            var zipName = string.Format(ArchiveName, DateTime.Now.ToString(DateTimeFormat));

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    foreach (var file in files)
                        using (var entryStream = archive.CreateEntry(file.Key).Open())
                        using (var fileStream = new MemoryStream(await GetAsync(file.Value)))
                            await fileStream.CopyToAsync(entryStream);

                return new Tuple<string, byte[]>(zipName, memoryStream.ToArray());
            }
        }
    }
}
