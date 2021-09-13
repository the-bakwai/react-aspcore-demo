using System;

namespace react_demo.Models
{
    public class BaseModel
    {
        public long Id { get; set; }

        public bool Active { get; set; }
        
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; } = DateTime.Now;
    }
}