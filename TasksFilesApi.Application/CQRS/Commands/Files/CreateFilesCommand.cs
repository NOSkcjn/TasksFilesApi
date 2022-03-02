using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Services.Interfaces;
using TasksFilesApi.Domain.Entities;

namespace TasksFilesApi.Application.CQRS.Commands.Files
{
    public class FileInfo
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }

    public class CreateFilesCommand : IRequest<bool>
    {
        public IEnumerable<FileInfo> Files { get; set; }
    }

    public class CreateFileCommandHandler : IRequestHandler<CreateFilesCommand, bool>
    {
        private readonly IMainContext _context;
        private readonly IStorageService _storage;

        public CreateFileCommandHandler(IMainContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<bool> Handle(CreateFilesCommand command, CancellationToken cancellationToken)
        {
            var fileNames = _context.Files.Select(x => x.Name);
            var commandFileNames = command.Files.Select(x => x.Name);
            if (commandFileNames.Intersect(fileNames).Any())
                return false;

            var files = await Task.WhenAll(command.Files.Select(async x => await CreateFile(x)));

            _context.Files.AddRange(files);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<ServiceFile> CreateFile(FileInfo file)
        {
            var result = new ServiceFile();
            result.Name = file.Name;
            var guid = await _storage.SaveAsync(file.Data);
            result.ExtGuid = guid;
            result.ContentType = file.ContentType;

            return result;
        }
    }

}
