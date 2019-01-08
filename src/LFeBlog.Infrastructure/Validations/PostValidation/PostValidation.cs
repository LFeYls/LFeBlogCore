using FluentValidation;
using LFeBlog.Infrastructure.EntityDtos.PostDtos;

namespace LFeBlog.Infrastructure.Validations.PostValidation
{
    public class PostValidation:AbstractValidator<PostDto>
    {
        public PostValidation()
        {

            RuleFor(c => c.Title.Trim())
                .NotNull()
                .WithName("标题")
                .WithMessage("{PropertyName}不能为空")
                .MaximumLength(50)
                .WithMessage("{PropertyName}最大长度为{MaxLength}");
        }
    }
}