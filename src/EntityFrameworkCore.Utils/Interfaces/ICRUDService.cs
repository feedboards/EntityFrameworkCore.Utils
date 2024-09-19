using Utils.EF.DTOs.Request;
using Utils.EF.DTOs.Response;

namespace Utils.EF.Interfaces
{
    public interface ICRUDService<TId, TResponse, TRequest>
        where TResponse : class
        where TRequest : class
    {
        Task<List<TResponse>> GetAllAsync();
        Task<TResponse> GetByIdAsync(TId id);

        Task<TResponse> InsertAsync(TRequest obj);
        Task<List<TResponse>> InsertAllAsync(List<TRequest> obj);

        Task<TResponse> UpdateAsync(TRequest obj);
        Task<List<TResponse>> UpdateAsync(List<TRequest> obj);

        Task<DeleteResponseDto<TId>> DeleteAsync(TId obj);
        Task<List<DeleteResponseDto<TId>>> DeleteAsync(List<TId> obj);

        Task<DeleteResponseDto<TId>> SoftDeleteAsync(TId id);
        Task<List<DeleteResponseDto<TId>>> SoftDeleteAsync(List<TId> id);

        Task<dynamic?> ExecuteSqlAsync(ExecuteSqlRequestDto obj);
    }

    public interface ICRUDService<TResponse, TRequest> : ICRUDService<Guid, TResponse, TRequest>
        where TResponse : class
        where TRequest : class
    {
    }
}
