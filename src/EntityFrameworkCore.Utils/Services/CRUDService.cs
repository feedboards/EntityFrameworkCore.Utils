using AutoMapper;
using EntityFrameworkCore.Utils.DTOs.Request;
using EntityFrameworkCore.Utils.DTOs.Response;
using EntityFrameworkCore.Utils.Enums;
using EntityFrameworkCore.Utils.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Utils.Services
{
    public class CRUDService<TId, TModel, TResponseDto, TRequestDto> : ICRUDService<TId, TResponseDto, TRequestDto>
        where TModel : class
        where TResponseDto : class
        where TRequestDto : class
    {
        protected readonly IMapper _mapper;
        protected readonly DbContext _context;
        protected readonly DbSet<TModel> _table;

        public CRUDService(IMapper mapper, DbContext context)
        {
            _mapper = mapper;
            _context = context;
            _table = context.Set<TModel>();
        }

        public virtual async Task<List<TResponseDto>> GetAsync()
        {
            var models = await _table.ToListAsync();

            return _mapper.Map<List<TResponseDto>>(models);
        }

        public virtual async Task<TResponseDto> GetByIdAsync(TId id)
        {
            var idProperty = typeof(TModel).GetProperty(nameof(ModelProperties.Id));

            if (idProperty == null || idProperty.PropertyType != typeof(Guid))
            {
                throw new InvalidOperationException("DTO does not have an 'Id' property of type 'Guid'.");
            }

            var parameter = Expression.Parameter(typeof(TModel), "x");
            var property = Expression.Property(parameter, idProperty);
            var constant = Expression.Constant(id);
            var equals = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<TModel, bool>>(equals, parameter);
            var model = await _table.Where(lambda).AsNoTracking().FirstOrDefaultAsync();

            return _mapper.Map<TResponseDto>(model);
        }

        public virtual async Task<TResponseDto> InsertAsync(TRequestDto obj)
        {
            var model = _mapper.Map<TModel>(_mapper.Map<TResponseDto>(obj));

            await _table.AddAsync(model);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResponseDto>(model);
        }

        public virtual async Task<List<TResponseDto>> InsertAsync(List<TRequestDto> obj)
        {
            var models = _mapper.Map<List<TModel>>(_mapper.Map<TResponseDto>(obj));

            await _table.AddRangeAsync(models);
            await _context.SaveChangesAsync();

            return _mapper.Map<List<TResponseDto>>(models);
        }

        public virtual async Task<TResponseDto> UpdateAsync(TRequestDto obj)
        {
            var model = _mapper.Map<TModel>(_mapper.Map<TResponseDto>(obj));

            _table.Attach(model);
            _context.Entry(model).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return _mapper.Map<TResponseDto>(model);
        }

        public virtual async Task<List<TResponseDto>> UpdateAsync(List<TRequestDto> obj)
        {
            var models = _mapper.Map<List<TModel>>(_mapper.Map<TResponseDto>(obj));

            models.ForEach(model =>
            {
                _table.Attach(model);
                _context.Entry(model).State = EntityState.Modified;
            });

            await _context.SaveChangesAsync();

            return _mapper.Map<List<TResponseDto>>(models);
        }

        public virtual async Task<DeleteResponseDto<TId>> DeleteAsync(TId id)
        {
            var dto = await GetByIdAsync(id);

            if (dto == null)
            {
                return new() { Id = id, IsDeleted = false };
            }

            _table.Remove(_mapper.Map<TModel>(dto));
            await _context.SaveChangesAsync();

            return new() { Id = id, IsDeleted = true };
        }

        public virtual async Task<List<DeleteResponseDto<TId>>> DeleteAsync(List<TId> ids)
        {
            var deleteResults = new List<DeleteResponseDto<TId>>();
            var modelsToRemove = new List<TModel>();

            foreach (var item in ids)
            {
                var dto = await GetByIdAsync(item);

                if (dto != null)
                {
                    modelsToRemove.Add(_mapper.Map<TModel>(_mapper.Map<TResponseDto>(dto)));
                    deleteResults.Add(new() { Id = item, IsDeleted = true });
                }
                else
                {
                    deleteResults.Add(new() { Id = item, IsDeleted = false });
                }
            }

            if (modelsToRemove.Any())
            {
                _table.RemoveRange(modelsToRemove);
                await _context.SaveChangesAsync();
            }

            return deleteResults;
        }

        //TODO need to test SoftDeleteAsync
        public virtual async Task<DeleteResponseDto<TId>> SoftDeleteAsync(TId id)
        {
            var idProperty = typeof(TResponseDto).GetProperty(nameof(ModelProperties.Id));
            var isDeleteProperty = typeof(TResponseDto).GetProperty(nameof(ModelProperties.IsDeleted));

            if (idProperty == null || idProperty.PropertyType != typeof(TId))
            {
                throw new InvalidOperationException("DTO does not have an 'Id' property of type 'TId'.");
            }

            if (isDeleteProperty == null || isDeleteProperty.PropertyType != typeof(bool))
            {
                throw new InvalidOperationException("DTO does not have an 'IsDeleted' property of type 'bool'.");
            }

            var dto = await GetByIdAsync(id);


            if (dto != null && isDeleteProperty != null && isDeleteProperty.CanWrite)
            {
                isDeleteProperty.SetValue(dto, true);

                await UpdateAsync(_mapper.Map<TRequestDto>(dto));

                return new() { Id = id, IsDeleted = true };
            }

            return new() { Id = id, IsDeleted = false };
        }

        //TODO need to test SoftDeleteAsync
        public virtual async Task<List<DeleteResponseDto<TId>>> SoftDeleteAsync(List<TId> ids)
        {
            var idProperty = typeof(TResponseDto).GetProperty(nameof(ModelProperties.Id));
            var isDeleteProperty = typeof(TResponseDto).GetProperty(nameof(ModelProperties.IsDeleted));

            if (idProperty == null || idProperty.PropertyType != typeof(TId))
            {
                throw new InvalidOperationException("DTO does not have an 'Id' property of type 'TId'.");
            }

            if (isDeleteProperty == null || isDeleteProperty.PropertyType != typeof(bool))
            {
                throw new InvalidOperationException("DTO does not have an 'IsDeleted' property of type 'bool'.");
            }

            var deleteResults = new List<DeleteResponseDto<TId>>();
            var dtos = new List<TResponseDto>();

            foreach (var id in ids)
            {
                var dto = await GetByIdAsync(id);

                if (dto != null && isDeleteProperty.CanWrite)
                {
                    isDeleteProperty.SetValue(dto, true);
                    dtos.Add(dto);
                    deleteResults.Add(new() { Id = id, IsDeleted = true });
                }
                else
                {
                    deleteResults.Add(new() { Id = id, IsDeleted = false });
                }
            }

            if (dtos.Any() && deleteResults.Any())
            {
                await UpdateAsync(_mapper.Map<List<TRequestDto>>(dtos));
            }

            return deleteResults;
        }

        public virtual async Task<dynamic?> ExecuteSqlAsync(ExecuteSqlRequestDto obj)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = obj.Sql;
            _context.Database.OpenConnection();

            using var result = await command.ExecuteReaderAsync();
            var results = new List<dynamic>();

            while (await result.ReadAsync())
            {
               var data = new ExpandoObject() as IDictionary<string, object>;
               for (var i = 0; i < result.FieldCount; i++)
               {
                   data.Add(result.GetName(i), result.IsDBNull(i) ? null : result.GetValue(i));
               }
               results.Add(data);
            }

            return results;
        }
    }

    public class CRUDService<TModel, TResponseDto, TRequestDto> :
        CRUDService<Guid, TModel, TResponseDto, TRequestDto>,
        ICRUDService<Guid, TResponseDto, TRequestDto>
        where TModel : class
        where TResponseDto : class
        where TRequestDto : class
    {
        public CRUDService(IMapper mapper, DbContext context) : base(mapper, context)
        {
        }
    }
}
