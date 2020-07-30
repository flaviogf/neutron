namespace Neutron.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NeutronDbContext _context;

        public UnitOfWork(NeutronDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
