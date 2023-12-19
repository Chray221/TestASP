using System.Linq;
using System.Collections;
using System.Linq.Expressions;
using AutoMapper;
using Newtonsoft.Json;

namespace TestASP.Web.Extensions
{
	public static class MappingExtensions
	{
		public static IMappingExpression<TSource,TDestination> IgnoreMember<TSource, TDestination, TMember>(
			this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination,TMember>> expression)
		{
			return mappingExpression.ForMember(expression, map => map.Ignore());
        }

        public static IMappingExpression<TSource, TDestination> SetIgnoredMember<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, TMember>> ignoreMemberExpression,
            Action<TSource,TDestination,IRuntimeMapper> map)
        {
            return mappingExpression
                    .ForMember(ignoreMemberExpression, map => map.Ignore())
                    .AfterMap((src, dest, context) =>
                    {
                        map.Invoke(src, dest, context.Mapper);
                    });
        }

        public static IMappingExpression<TSource, TDestination> ForMemberMap<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, TMember>> ignoreMemberExpression,
            Func<TSource,TMember> mapFrom)
        {
                   //mappingExpression.ForMember(ignoreMemberExpression, map => map.MapFrom(src => src.Id));
            return mappingExpression.ForMember(ignoreMemberExpression, map => map.MapFrom(src => mapFrom.Invoke(src)));
        }

        

        public static IEnumerable<TDestination> SelectMap<TDestination>(this IEnumerable<object> items, IMapperBase mapper)
        {
            return items.Select(item => mapper.Map<TDestination>(item));
        }

        public static List<TDestination> SelectMapList<TDestination>(this IEnumerable<object> items, IMapperBase mapper)
        {
            return items.SelectMap<TDestination>(mapper).ToList();
        }

        public static T Clone<T>(this T item)
        {
            string serialize = JsonConvert.SerializeObject(item);
            return JsonConvert.DeserializeObject<T>(serialize);
        }
    }
}

