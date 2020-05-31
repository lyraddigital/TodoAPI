using TodoAPI.Services.Core.Contracts;

namespace TodoAPI.Services.Foundation.Mapping
{
    public class Mapper : IMapper
    {
        private readonly AutoMapper.IMapper _autoMapper;

        public Mapper(AutoMapper.IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public U Map<T, U>(T from)
            where T : class
            where U : class
        {
            return _autoMapper.Map<T, U>(from);
        }
    }
}
