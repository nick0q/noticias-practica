using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using noticias.Models;
using noticias.Data;

namespace noticias.Service
{
    public class FeedbackService
    {

        private readonly ILogger<FeedbackService> _logger;
        private readonly ApplicationDbContext _context;

        public FeedbackService(ILogger<FeedbackService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<Feedback?> GetById(int id)
        {
            _logger.LogInformation("Fetching product with ID {0} from the database.", id);
            if (_context.DbSetFeedback == null)
                return null;
            var post = await _context.DbSetFeedback.FindAsync(id);
            if (post == null)
                return null;
            _logger.LogInformation("Fetched product with ID {0} from the database.", id);
            return post;
        }

        public async Task<bool> Add(Feedback feedback)
        {
            _logger.LogInformation("Adding a new user to the database.");
            if (_context.DbSetFeedback == null)
                return false;
            await _context.DbSetFeedback.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<Feedback>?> GetAll()
        {
            _logger.LogInformation("Fetching all users from the database.");
            if (_context.DbSetFeedback == null)
                return null;
            var posts = await _context.DbSetFeedback.ToListAsync();
            return posts;
        }

    }
}