﻿using EntityFrameworkCore.Utils.DTOs.Request;
using EntityFrameworkCore.Utils.DTOs.Response;

namespace EntityFrameworkCore.Utils.Interfaces
{
    public interface ICRUDService<TId, TResponse, TRequest>
        where TResponse : class
        where TRequest : class
    {
        Task<List<TResponse>> GetAsync();
        Task<TResponse> GetByIdAsync(TId id);

        Task<TResponse> InsertAsync(TRequest obj);
        Task<List<TResponse>> InsertAsync(List<TRequest> obj);

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
