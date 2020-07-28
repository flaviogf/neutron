namespace Neutron.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
