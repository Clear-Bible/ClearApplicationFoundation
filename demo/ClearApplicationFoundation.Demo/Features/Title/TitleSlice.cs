using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.Demo.Features.Title
{
    internal record TitleQuery : IRequest<string>
    {
    }

    internal class TitleQueryHandler : IRequestHandler<TitleQuery, string>
    {
        private readonly ILogger<TitleQueryHandler> _logger;

        public TitleQueryHandler(ILogger<TitleQueryHandler> logger)
        {
            _logger = logger;
        }
        public Task<string> Handle(TitleQuery request, CancellationToken cancellationToken)
        {
            var title = "Hello Foundation";
            _logger.LogDebug($"Fetch title: {title}");
            return Task.FromResult(title);
        }
    }


}
