using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Services.Interfaces;

namespace TasksFilesApi.Application.CQRS.Commands.Files
{
    public class UpdateFileByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }

    public class UpdateFileByIdCommandHandler : IRequestHandler<UpdateFileByIdCommand, bool>
    {
        private readonly IMainContext _context;
        private readonly IStorageService _storage;

        public UpdateFileByIdCommandHandler(IMainContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<bool> Handle(UpdateFileByIdCommand command, CancellationToken cancellationToken)
        {
            if (_context.Files.Select(x => x.Name).Contains(command.Name))
                return false;

            var file = _context.Files.Where(x => x.Id == command.Id).FirstOrDefault();

            file.Name = command.Name;
            var newGuid = await _storage.SaveAsync(command.Data);
            _storage.Delete(file.ExtGuid);
            file.ExtGuid = newGuid;
            file.ContentType = command.ContentType;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
