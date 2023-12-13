using System.Linq;
using System.Collections;
using System.Linq.Expressions;
using AutoMapper;

namespace TestASP.API.Extensions
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

        public static IEnumerable<TDestination> SelectMap<TDestination>(this IEnumerable<object> items, IMapperBase mapper)
        {
            return items.Select(item => mapper.Map<TDestination>(item));
        }

        public static List<TDestination> SelectMapList<TDestination>(this IEnumerable<object> items, IMapperBase mapper)
        {
            return items.SelectMap<TDestination>(mapper).ToList();
        }
    }
}

