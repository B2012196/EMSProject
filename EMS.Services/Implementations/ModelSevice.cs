using EMS.Data;
using EMS.Data.Entities;
using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class ModelService : IModelService
    {
        private readonly MyDbContext _context;

        public ModelService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Model>> GetAllAsync()
        {
            return await _context.Models.ToListAsync();
        }

        public async Task<Model> GetByIdAsync(int id)
        {
            var model = await _context.Models.SingleOrDefaultAsync(m => m.Id == id);
            if (model != null)
            {
                return model;
            }
            else return null;
        }

        public async Task<Model> GetByNameAsync(string name)
        {
            var model = await _context.Models.SingleOrDefaultAsync(m => m.Name == name);
            if (model != null)
            {
                return model;
            }
            else return null;
        }

        public async Task<Model> CreateAsync(ModelDto modelDto)
        {
            try
            {
                if (string.IsNullOrEmpty(modelDto.Name))
                {
                    return null;
                }
                var model = new Model
                {
                    Name = modelDto.Name,
                };
                _context.Add(model);
                await _context.SaveChangesAsync();
                return model;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var model = await _context.Models.SingleOrDefaultAsync(m => m.Id == id);
                if (model != null)
                {
                    _context.Models.Remove(model);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ModelDtoUser modelDtoUser)
        {
            try
            {
                var model = await _context.Models.SingleOrDefaultAsync(m => m.Id == modelDtoUser.Id);
                if (model != null)
                {
                    model.Name = modelDtoUser.Name;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
