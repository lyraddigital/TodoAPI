namespace TodoAPI.Services.Core.Contracts
{
    public interface IMapper
    {
        U Map<T, U>(T from)
            where T : class
            where U : class;
    }
}
