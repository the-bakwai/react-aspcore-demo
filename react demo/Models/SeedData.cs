using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace react_demo.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            var ctx = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<TodoDataContext>();

            if (ctx.Database.GetPendingMigrations().Any())
            {
                ctx.Database.Migrate();
            }

        }
    }
}