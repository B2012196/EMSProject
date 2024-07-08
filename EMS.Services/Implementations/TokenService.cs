using EMS.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class TokenService
    {
        private readonly MyDbContext _context;

        public TokenService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTokenRevoked(string tokenValue)
        {
            var token = await _context.Tokens.SingleOrDefaultAsync(t => t.TokenValue == tokenValue);
            return token?.IsRevoke ?? true;

        }
    }
}
