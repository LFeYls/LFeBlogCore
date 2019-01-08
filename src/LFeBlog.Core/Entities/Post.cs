using System;
using LFeBlog.Core.Interfaces;

namespace LFeBlog.Core.Entities
{
    public class Post:Entity
    {
        
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}