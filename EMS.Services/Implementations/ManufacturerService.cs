using EMS.Data;
using EMS.Data.Entities;
using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EMS.Services.Implementations
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly MyDbContext _context;

        public ManufacturerService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Manufacturer>> GetAllAsync()
        {
            return await _context.Manufacturers.ToListAsync();
        }

        public async Task<List<Manufacturer>> GetByAddressAsync(string address)
        {
            var Manufacturer = await _context.Manufacturers.Where(m => m.Address == address).ToListAsync();
            if (Manufacturer != null)
            {
                return Manufacturer;
            }
            else return null;
        }

        public async Task<Manufacturer> GetByIdAsync(int id)
        {
            var Manufacturer = await _context.Manufacturers.SingleOrDefaultAsync(m => m.Id == id);
            if (Manufacturer != null)
            {
                return Manufacturer;
            }
            else return null;
        }

        public async Task<Manufacturer> GetByNameAsync(string name)
        {
            var Manufacturer = await _context.Manufacturers.SingleOrDefaultAsync(m => m.Name == name);
            if (Manufacturer != null)
            {
                return Manufacturer;
            }
            else return null;
        }

        public async Task<Manufacturer> GetByPhoneAsync(string phone)
        {
            var Manufacturer = await _context.Manufacturers.SingleOrDefaultAsync(m => m.Phone == phone);
            if (Manufacturer != null)
            {
                return Manufacturer;
            }
            else return null;
        }
        public async Task<Manufacturer> CreateAsync(ManufacturerDto manufacturerDto)
        {
            try
            {
                if (manufacturerDto == null || string.IsNullOrEmpty(manufacturerDto.Name) ||
                    string.IsNullOrEmpty(manufacturerDto.Address) || string.IsNullOrEmpty(manufacturerDto.Phone))
                    return null;

                var manufacturer = new Manufacturer
                {
                    Name = manufacturerDto.Name,
                    Address = manufacturerDto.Address,
                    Phone = manufacturerDto.Phone,
                };

                _context.Add(manufacturer);
                await _context.SaveChangesAsync();

                return manufacturer;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var manufacturer = await _context.Manufacturers.SingleOrDefaultAsync(e => e.Id == id);
            if (manufacturer != null)
            {
                _context.Remove(manufacturer);
                await _context.SaveChangesAsync();
                return true;
            }
            else return false;
        }

        

        public async Task<bool> UpdateAsync(ManufacturerDtoUser manufacturerDtoUser)
        {
            var manufacturer = await _context.Manufacturers.SingleOrDefaultAsync(e => e.Id == manufacturerDtoUser.Id);
            if (manufacturer != null)
            {
                manufacturer.Name = manufacturerDtoUser.Name;
                manufacturer.Address = manufacturerDtoUser.Address;
                manufacturer.Phone = manufacturerDtoUser.Phone;

                _context.Update(manufacturer);
                await _context.SaveChangesAsync();
                return true;
            }
            else return false;
        }
    }
}
