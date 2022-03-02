using System;

namespace TasksFilesApi.Domain.Entities
{
    public class ServiceFile : BaseEntity
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public Guid ExtGuid { get; set; }

        public ServiceTask Task { get; set; }
    }
}
