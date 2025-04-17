using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(MyContext context) : base(context)
        {
        }

        public async Task DeleteAsync(Campaign entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Campaign>> GetActiveCampaignsAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<bool> IsCampaignAvailableAsync(int campaignId)
        {
            return await _dbSet.AnyAsync(c => c.Id == campaignId && c.IsActive);
        }
    }
}
