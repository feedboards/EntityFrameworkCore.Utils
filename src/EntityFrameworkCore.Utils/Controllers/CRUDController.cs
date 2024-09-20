using EntityFrameworkCore.Utils.Constants;
using EntityFrameworkCore.Utils.DTOs.Request;
using EntityFrameworkCore.Utils.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Utils.Controllers
{
    public class CRUDController<TId, TResponseDto, TRequestDto> : Controller, ICRUDController<TId, TRequestDto>
        where TResponseDto : class
        where TRequestDto : class
    {
        private readonly ICRUDService<TId, TResponseDto, TRequestDto> _service;

        public CRUDController(ICRUDService<TId, TResponseDto, TRequestDto> service)
        {
            _service = service;
        }

        [HttpGet(ControllerRoutes.ROOT)]
        public virtual async Task<IActionResult> GetAsync()
        {
            return Ok(await _service.GetAsync());
        }

        [HttpPost(ControllerRoutes.ROOT)]
        public virtual async Task<IActionResult> InsertAsync([FromBody] TRequestDto obj)
        {
            return Ok(await _service.InsertAsync(obj));
        }

        [HttpPut(ControllerRoutes.ROOT)]
        public virtual async Task<IActionResult> UpdateAsync([FromBody] TRequestDto obj)
        {
            return Ok(await _service.UpdateAsync(obj));
        }

        [HttpPost(ControllerRoutes.SQL)]
        public virtual async Task<IActionResult> ExecuteSqlAsync([FromBody] ExecuteSqlRequestDto obj)
        {
            return Ok(await _service.ExecuteSqlAsync(obj));
        }

        [HttpGet(ControllerRoutes.ID)]
        public virtual async Task<IActionResult> GetByIdAsync(TId id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpDelete(ControllerRoutes.ID)]
        public virtual async Task<IActionResult> DeleteAsync(TId id)
        {
            return Ok(await _service.DeleteAsync(id));
        }

        [HttpDelete(ControllerRoutes.ID_SOFT_DELETE)]
        public virtual async Task<IActionResult> SoftDeleteAsync(TId id)
        {
            return Ok(await _service.SoftDeleteAsync(id));
        }

        [HttpPost(ControllerRoutes.ITEMS)]
        public virtual async Task<IActionResult> InsertAsync([FromBody] List<TRequestDto> obj)
        {
            return Ok(await _service.InsertAsync(obj));
        }

        [HttpDelete(ControllerRoutes.ITEMS)]
        public virtual async Task<IActionResult> DeleteAsync([FromBody] List<TId> ids)
        {
            return Ok(await _service.DeleteAsync(ids));
        }

        [HttpDelete(ControllerRoutes.ITEMS_SOFT_DELETE)]
        public virtual async Task<IActionResult> SoftDeleteAsync([FromBody] List<TId> id)
        {
            return Ok(await _service.SoftDeleteAsync(id));
        }

        [HttpPut(ControllerRoutes.ITEMS)]
        public virtual async Task<IActionResult> UpdateAsync([FromBody] List<TRequestDto> obj)
        {
            return Ok(await _service.UpdateAsync(obj));
        }
    }

    public class CRUDController<TResponseDto, TRequestDto> : CRUDController<Guid, TResponseDto, TRequestDto>, ICRUDController<Guid, TRequestDto>
        where TResponseDto : class
        where TRequestDto : class
    {
        public CRUDController(ICRUDService<Guid, TResponseDto, TRequestDto> service) : base(service)
        {
        }
    }
}
