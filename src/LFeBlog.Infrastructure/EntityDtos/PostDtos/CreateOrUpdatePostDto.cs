namespace LFeBlog.Infrastructure.EntityDtos.PostDtos
{
    public class CreateOrUpdatePostDto
    {
        
        public int? Id { get; set; }
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public string Remark { get; set; }
    }
}