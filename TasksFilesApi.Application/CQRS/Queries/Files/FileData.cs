namespace TasksFilesApi.Application.CQRS.Queries.Files
{
    public class FileData
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }
}
