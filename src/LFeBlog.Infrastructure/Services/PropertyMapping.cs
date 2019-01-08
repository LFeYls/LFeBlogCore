using System.Collections.Generic;
using AutoMapper;
using LFeBlog.Core.Interfaces;

namespace LFeBlog.Infrastructure.Services
{
    public class PropertyMapping<TSource,TDestination>:IPropertyMapping
    where TSource:IEntity
    {
        public Dictionary<string,List<MappedProperty>> MappingDictionary { get; set; }

        protected PropertyMapping(Dictionary<string,List<MappedProperty>> mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
            
            mappingDictionary[nameof(IEntity.Id)]=new List<MappedProperty>
            {
                new MappedProperty{Name = nameof(IEntity.Id),Revert = true}
            };

        }
        



    }
}