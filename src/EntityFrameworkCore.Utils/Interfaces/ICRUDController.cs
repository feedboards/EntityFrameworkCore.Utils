using EntityFrameworkCore.Utils.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Utils.Interfaces
{
    public interface ICRUDController<TId, TRequestDto>
        where TRequestDto : class
    {
        Task<IActionResult> GetAllAsync();
        Task<IActionResult> GetByIdAsync(TId id);

        Task<IActionResult> InsertAsync([FromBody] TRequestDto obj);
        Task<IActionResult> InsertAllAsync([FromBody] List<TRequestDto> obj);


        Task<IActionResult> UpdateAsync([FromBody] TRequestDto obj);
        Task<IActionResult> UpdateAsync([FromBody] List<TRequestDto> obj);

        Task<IActionResult> DeleteAsync(TId obj);
        Task<IActionResult> DeleteAsync([FromBody] List<TId> obj);

        Task<IActionResult> SoftDeleteAsync(TId id);
        Task<IActionResult> SoftDeleteAsync([FromBody] List<TId> id);

        Task<IActionResult> ExecuteSqlAsync([FromBody] ExecuteSqlRequestDto obj);
    }

    public interface ICRUDController<T> : ICRUDController<Guid, T>
        where T : class
    {
    }
}
