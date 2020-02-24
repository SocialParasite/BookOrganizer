namespace BookOrganizer.Domain.Services
{
    public interface IDomainService<T> where T: class
    {
        IRepository<T> Repository { get;  }

        T CreateItem();
    }
}
