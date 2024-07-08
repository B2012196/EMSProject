using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IModelService
    {
        Task<List<Model>> GetAllAsync();
        Task<Model> GetByIdAsync(int id);
        Task<Model> GetByNameAsync(string name);
        Task<Model> CreateAsync(ModelDto modelDto);
        Task<bool> UpdateAsync(ModelDtoUser modelDtoUser);
        Task<bool> DeleteAsync(int id);
    }
}
