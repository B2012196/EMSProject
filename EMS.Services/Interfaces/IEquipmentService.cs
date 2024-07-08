using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IEquipmentService
    {
        Task<List<Equipment>> GetAllAsync();
        Task<List<EquipmentStatusDto>> GetAllByStatusIdAsync();
        Task<Equipment> GetByIdAsync(int id);
        Task<Equipment> GetByNameAsync(string name);
        Task<Equipment> GetBySeriAsync(string seri);
        Task<List<Equipment>> GetByLocationAsync(string locationname);
        Task<List<Equipment>> GetByModelAsync(string modelName);
        Task<Equipment> CreateAsync(EquipmentDto equipmentDto);
        Task <bool> UpdateAsync(EquipmentDtoUser equipmentDtoUser);
        Task <bool> DeleteAsync(int id);

    }
}
