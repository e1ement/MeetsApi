using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IValueRepository
    {
        Task<IEnumerable<ValueEntity>> GetAllValuesAsync();
        Task<ValueEntity> GetValueByIdAsync(Guid id);
        Task CreateValueAsync(ValueEntity value);
        Task UpdateValueAsync(ValueEntity dbValue, ValueEntity value);
        Task DeleteValueAsync(ValueEntity value);
    }
}
