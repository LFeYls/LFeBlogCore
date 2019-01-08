using System.ComponentModel.DataAnnotations;

namespace LFeBlog.Core.Interfaces
{
    public abstract class Entity:IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}