using MongoDB.Driver;
using FastTechFoodsDLQProcessor.Data;
using FastTechFoodsDLQProcessor.Models;
using System;
using System.Threading.Tasks;

namespace FastTechFoodsDLQProcessor.Services
{
    public class DLQMessageService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMongoCollection<DLQMessage> _collection;

        public DLQMessageService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _collection = _dbContext.GetCollection<DLQMessage>("DLQMessages");
        }

        public async Task SaveMessageAsync(string message)
        {
            var dlqMessage = new DLQMessage
            {
                Message = message,
                ProcessedAt = DateTime.UtcNow
            };
            await _collection.InsertOneAsync(dlqMessage);
        }
    }
}
