using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Interfaces;
using TaskService.Domain.Common;

namespace TaskService.Infras.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskDbContext _context;
        private readonly IMediator _mediator;
        private readonly IPublisher _publisher; 
        private readonly ILogger<UnitOfWork> _logger;
        public UnitOfWork(
           TaskDbContext context,
           IPublisher publisher, 
           ILogger<UnitOfWork> logger) 
        {
            _context = context;
            _publisher = publisher;
            _logger = logger;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Save changes để có Id
            var result = await _context.SaveChangesAsync(cancellationToken);
            //// 2. Publish domain events từ tất cả entities
            //await PublishDomainEvents(cancellationToken);

            return result;
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await SaveChangesAsync(cancellationToken);
            return result > 0;
        }


        //private async Task PublishDomainEvents(CancellationToken cancellationToken)
        //{
        //    // Lấy tất cả entities có domain events
        //    var entitiesWithEvents = _context.ChangeTracker
        //        .Entries<EntityBase>()
        //        .Where(e => e.Entity.DomainEvents?.Any() == true)
        //        .Select(e => e.Entity)
        //        .ToList();

        //    if (!entitiesWithEvents.Any())
        //        return;

        //    _logger.LogInformation(
        //        "Found {Count} entities with domain events to publish",
        //        entitiesWithEvents.Count);

        //    // Collect tất cả events
        //    var domainEvents = entitiesWithEvents
        //        .SelectMany(e => e.DomainEvents)
        //        .ToList();

        //    // Clear events từ từng entity
        //    entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        //    // Publish events
        //    foreach (var domainEvent in domainEvents)
        //    {
        //        await _publisher.Publish(domainEvent, cancellationToken);
        //        _logger.LogDebug(
        //            "Published domain event: {EventType} (EventId={EventId})",
        //            domainEvent.GetType().Name,
        //            domainEvent.EventId);
        //    }


        //}
       
        
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
