using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IReplacementRecordService
    {
        Task<List<ReplacementRecord>> GetAllAsync();
        Task<ReplacementRecord> GetByIdAsync(int id);
        Task<ReplacementRecord> CreateAsync(ReplacementRecordDto replacementRecordDto);
    }
}
