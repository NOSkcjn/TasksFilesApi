using System;
using System.Collections.Generic;

namespace TasksFilesApi.Domain.Entities
{
    public class ServiceTask: BaseEntity
    {
        public DateTime Date { get; set; }

        public string Name { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<ServiceFile> Files { get; set; }
    }
}
