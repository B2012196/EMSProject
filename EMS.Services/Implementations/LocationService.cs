using EMS.Data;
using EMS.Data.Entities;
using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly MyDbContext _context;

        public LocationService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<List<Location>> GetAllAsync()
        {
            return await _context.Locations.ToListAsync();
        }
        public async Task<List<string>> GetAllDistinctnAsync()
        {
            return await _context.Locations.Select(l => l.Name).Distinct().ToListAsync();
        }


        public async Task<List<DepartmentEquipmentCountDto>> GetEquipmentCountByDepartmentAsync()
        {
            var result = await _context.Locations
            .GroupBy(l => l.Name)
            .Select(group => new DepartmentEquipmentCountDto
            {
                DepartmentName = group.Key,
                EquipmentCount = group.Sum(l => l.Equipments.Count) // Đếm số thiết bị của mỗi phòng ban chính
            })
            .ToListAsync();
            return result;
        }

        public async Task<Location> GetByIdAsync(int id)
        {
            var location = await _context.Locations.SingleOrDefaultAsync(l => l.Id == id);
            if (location != null)
            {
                return location;
            }
            else return null;
        }

        public async Task<List<Location>> GetByNameAsync(string name)
        {
            var locations = await _context.Locations.Where(l => l.Name == name).ToListAsync();

            if (locations == null || !locations.Any())
                return null;
            else return locations;
        }

        public async Task<int> GetDeviceCountByDepartment(string name)
        {
            //var deviceCount = await _context.Locations.Where(e => e.Name == name) // Assuming Location_Id is used to store department ID
            //.CountAsync();
            var deviceCount = await _context.Equipments
                                 .Include(e => e.Location)  // Include the related Model entity
                                 .Where(e => e.Location.Name == name)
                                 .CountAsync();
            if (deviceCount > 0)
                return deviceCount;
            else return 0;
        }

        public async Task<Location> CreateAsync(LocationDto locationDto)
        {
            try
            {
                var location = new Location
                {
                    Name = locationDto.Name,
                    Floor = locationDto.Floor,
                    RoomNumber = locationDto.RoomNumber,
                };
                _context.Add(location);
                await _context.SaveChangesAsync();
                return location;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(LocationDtoUser locationDtoUser)
        {
            var location = await _context.Locations.SingleOrDefaultAsync(l => l.Id  == locationDtoUser.Id);

            if (location != null)
            {
                location.Name = locationDtoUser.Name;
                location.Floor = locationDtoUser.Floor;
                location.RoomNumber = locationDtoUser.RoomNumber;

                _context.Update(location);
                await _context.SaveChangesAsync();
                return true;
            }
            else return false;

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var location = await _context.Locations.SingleOrDefaultAsync(l => l.Id == id);
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                return true;
            }
            else return false;
        }

        
    }
}
