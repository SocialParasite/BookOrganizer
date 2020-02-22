using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookOrganizer.Domain.Services
{
    public interface IDomainService<T> where T: class
    {
        IRepository<T> Repository { get; set; }
    }
}
