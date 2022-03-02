using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TasksFilesApi.Services.Interfaces;

namespace TasksFilesApi.Application.CQRS.Queries.Files
{
    public class GetFileByIdQuery : IRequest<FileData>
    {
        public int Id { get; set; }

        public GetFileByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQuery, FileData>
    {
        private readonly IMainContext _context;
        private readonly IStorageService _storage;

        public GetFileByIdQueryHandler(IMainContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<FileData> Handle(GetFileByIdQuery query, CancellationToken cancellationToken)
        {
            var file = await _context.Files.FirstOrDefaultAsync(x => x.Id == query.Id);
            if (file == null)
                return null;

            var data = await _storage.GetAsync(file.ExtGuid);
            return new FileData { ContentType = file.ContentType, Name = file.Name, Data = data };
        }
    }
}
