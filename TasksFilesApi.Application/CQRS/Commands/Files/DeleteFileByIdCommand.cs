using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Services.Interfaces;

namespace TasksFilesApi.Application.CQRS.Commands.Files
{
    public class DeleteFileByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteFileByIdCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteFileByIdCommandHandler : IRequestHandler<DeleteFileByIdCommand, bool>
    {
        private readonly IMainContext _context;
        private readonly IStorageService _storage;

        public DeleteFileByIdCommandHandler(IMainContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<bool> Handle(DeleteFileByIdCommand command, CancellationToken cancellationToken)
        {
            var file = _context.Files.Where(x => x.Id == command.Id).FirstOrDefault();
            if (file == null) 
                return false;

            _storage.Delete(file.ExtGuid);
            _context.Files.Remove(file);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
