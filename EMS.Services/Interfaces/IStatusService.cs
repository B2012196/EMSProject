using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IStatusService
    {
        Task<List<Status>> GetAllAsync();
        Task<Status> GetByIdAsync(int id);
        Task<Status> CreateAsync(StatusDto statusDto);
        Task<bool> DeleteAsync(int id);
    }
}
