using FluentValidation;
using LFeBlog.Infrastructure.EntityDtos.PostDtos;

namespace LFeBlog.Infrastructure.Validations.PostValidation
{
    public class CreateOrUpdatePostDtoValidator<T>:AbstractValidator<T> where T: CreateOrUpdatePostDto
    {
        public CreateOrUpdatePostDtoValidator()
        {

            RuleFor(c => c.Title)
                .NotNull()
                .WithName("标题")
                .WithMessage("required|{PropertyName}是必填的")
                .MaximumLength(50)
                .WithMessage("maxlength|{PropertyName}的最大长度是{MaxLength}");


            RuleFor(c => c.Body)
                .NotNull()
                .WithName("正文")
                .WithMessage("required| {PropertyName}是必填的")
                .MinimumLength(20)
                .WithMessage("maxlength|{PropertyName}的最小长度是{MinLength}");
        }
    }
}