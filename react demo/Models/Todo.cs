using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace react_demo.Models
{
    public class Todo: BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        public bool Completed { get; set; }
        public long UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public DateTime? DateCompleted { get; set; } = null;
    }

    public interface ITodoRepository: IRepository<Todo>
    {
        Task<List<Todo>> GetAllByUser(long id);
    }

    public class TodoRepository : Repository<Todo>, ITodoRepository
    {

        public TodoRepository(DbContext context) : base(context)
        {
        }

        public async Task<List<Todo>> GetAllByUser(long id)
        {
            return await TodoContext.Todos.Where(t => t.UserId == id).ToListAsync();
        }

        private TodoDataContext TodoContext => Context as TodoDataContext;
    }
}