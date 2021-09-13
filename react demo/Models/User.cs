using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace react_demo.Models
{
    public class User: BaseModel
    {
        [Required]
        [StringLength(100)]
        public string AuthId { get; set; }

        public ICollection<Todo> Todos { get; set; }
        
    }

    public interface IUserRepository : IRepository<User>
    {
        public Task<User> FindByAuthId(string authId);
        public Task<User> FindOrCreateByAuthId(string authId);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(DbContext context) : base(context)
        {

        }
        
        private TodoDataContext TodoContext => Context as TodoDataContext;
        public async Task<User> FindByAuthId(string authId)
        {
            return await TodoContext.Users.SingleOrDefaultAsync(u => u.AuthId == authId);
        }

        public async Task<User> FindOrCreateByAuthId(string authId)
        {
            var user = await FindByAuthId(authId);
            if (user == null)
            {
                user = new User()
                {
                    AuthId = authId
                };

                await Add(user);
            }

            return user;
        }
    }
}