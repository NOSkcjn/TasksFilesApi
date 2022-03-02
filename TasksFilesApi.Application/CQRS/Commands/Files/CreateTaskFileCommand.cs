using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Domain.Entities;
using System.Linq;
using TasksFilesApi.Services.Interfaces;

namespace TasksFilesApi.Application.CQRS.Commands.Files
{
    public class CreateTaskFileCommand : IRequest<bool>
    {
        public int TaskId { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }

    public class CreateTaskFileCommandHandler : IRequestHandler<CreateTaskFileCommand, bool>
    {
        private readonly IMainContext _context;
        private readonly IStorageService _storage;

        public CreateTaskFileCommandHandler(IMainContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<bool> Handle(CreateTaskFileCommand command, CancellationToken cancellationToken)
        {
            var task = _context.Tasks.Where(x => x.Id == command.TaskId).FirstOrDefault();

            var fileNames = _context.Files.Select(x => x.Name);
            if (fileNames.Contains(command.Name))
                return false;

            var file = new ServiceFile();
            file.Name = command.Name;
            var guid = await _storage.SaveAsync(command.Data);
            file.ExtGuid = guid;
            file.ContentType = command.ContentType;
            file.Task = task;

            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

