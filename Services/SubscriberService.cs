using Crito.Contexts;
using Crito.Models;

namespace Crito.Services
{
    public class SubscriberService
    {
        private readonly DataContext _context;

        public SubscriberService(DataContext context)
        {
            _context = context;
        }

        public async Task AddSubscriberAsync(NewsletterForm form)
        {
            var email = _context.Subscribers.FirstOrDefault(x => x.Email == form.Email);

            if(email == null)
            {
                _context.Subscribers.Add(new SubscriberEntity { Email = form.Email });
                await _context.SaveChangesAsync();
            }

        }
    }
}
