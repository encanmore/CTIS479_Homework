using Microsoft.EntityFrameworkCore;

namespace APP.Domain
{
    public class Db : DbContext
    {
        public Db(DbContextOptions options) : base(options)
        {

        }
    }
}
