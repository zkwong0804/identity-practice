using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityPractice.Models
{
    public class FoodDbContext : IdentityDbContext<FoodUser, FoodRole, Guid>
    {
        public FoodDbContext(DbContextOptions opt) : base(opt) { }
    }

    public class FoodUser : IdentityUser<Guid> { }
    public class FoodRole : IdentityRole<Guid> { }
}
