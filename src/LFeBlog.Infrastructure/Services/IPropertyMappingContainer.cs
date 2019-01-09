using LFeBlog.Core.Interfaces;

namespace LFeBlog.Infrastructure.Services
{
    public interface IPropertyMappingContainer
    {

        void Register<T>() where T : IPropertyMapping, new();

        IPropertyMapping Resolve<TSource, TDestination>() where TDestination : IEntity;

        bool ValidateMappingExistsFor<TSource, TDestination>(string fields) where TDestination : IEntity;

    }
}