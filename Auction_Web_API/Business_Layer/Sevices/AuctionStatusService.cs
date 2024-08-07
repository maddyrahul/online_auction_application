using Data_Access_Layer.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Business_Layer.Sevices
{
    public class AuctionStatusService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30); // Check every 30 seconds

        public AuctionStatusService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

                    var activeAuctions = await dbContext.Auctions
                        .Where(a => !a.IsEnded && a.EndTime <= DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                    foreach (var auction in activeAuctions)
                    {
                        auction.IsEnded = true;
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
