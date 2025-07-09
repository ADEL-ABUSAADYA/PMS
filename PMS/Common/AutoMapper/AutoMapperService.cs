using AutoMapper;

namespace PMS.Common.AutoMapper;

public static class AutoMapperService
{
    public static IMapper _mapper;

    public static IQueryable<T> ProjectTo<T>(this IQueryable<object> source)
    {
        return _mapper.ProjectTo<T>(source);
    }
    public static T MapFirstOrDefault<T>(this IQueryable<object> source)
    {
        return _mapper.ProjectTo<T>(source).FirstOrDefault();
    }

    public static T Map<T>(this object source)
    {
        return _mapper.Map<T>(source);
    }
}