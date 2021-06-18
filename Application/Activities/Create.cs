using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>> {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly ILogger<Handler> _logger;

            public Handler(DataContext context, IUserAccessor userAccessor, ILogger<Handler> logger)
            {
                _context = context;
                _userAccessor = userAccessor;
                _logger = logger;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                var attendee = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true
                    
                };
                _logger.LogError(JsonSerializer.Serialize(request.Activity));

                _logger.LogError(JsonSerializer.Serialize(attendee));

                request.Activity.Attendees.Add(attendee);
//                _logger.LogError(JsonSerializer.Serialize(attendee));
               _context.Activities.Add(request.Activity);
                
                var result = await _context.SaveChangesAsync() > 0;
                if (!result)
                    return Result<Unit>.Failure("Failed to create activity");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
