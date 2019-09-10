using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class ValueRepository : RepositoryBase<ValueEntity>, IValueRepository
    {
        public ValueRepository(RepositoryContext context) : base(context)
        {

        }

        public async Task<IEnumerable<ValueEntity>> GetAllValuesAsync()
        {
            return await FindAll().OrderBy(o => o.Id).ToListAsync();
        }

        public async Task<ValueEntity> GetValueByIdAsync(Guid id)
        {
            return await FindByCondition(o => o.Id.Equals(id))
                .DefaultIfEmpty(new ValueEntity())
                .SingleAsync();
        }

        public async Task CreateValueAsync(ValueEntity value)
        {
            Create(value);
            await SaveAsync();
        }

        public async Task UpdateValueAsync(ValueEntity dbValue, ValueEntity value)
        {
            dbValue.Map(value);
            Update(dbValue);
            await SaveAsync();
        }

        public async Task DeleteValueAsync(ValueEntity value)
        {
            Delete(value);
            await SaveAsync();
        }
    }
}
