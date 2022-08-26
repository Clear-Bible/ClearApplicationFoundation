using Caliburn.Micro;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClearApplicationFoundation.Demo.Features.Title;
using ClearApplicationFoundation.Framework;
using MediatR;

namespace ClearApplicationFoundation.Demo.ViewModels
{
    public class HomeViewModel : Conductor<ITab>.Collection.OneActive, IMainWindow
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMediator _mediator;
        private string? _title;

        public string? Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public HomeViewModel()
        {
            
        }

        public HomeViewModel(IEventAggregator eventAggregator, ILogger<HomeViewModel> logger, IMediator mediator)
        {
            _eventAggregator = eventAggregator;
            _mediator = mediator;
            logger.LogDebug("HomeViewModel ctor called!");
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Title = await _mediator.Send(new TitleQuery(), cancellationToken);


            var tabs = IoC.GetAll<ITab>();

            foreach (var tab in tabs)
            {
                await ActivateItemAsync(tab, cancellationToken);
            }

            await ActivateItemAsync(Items[0], cancellationToken);

           
        }
    }
}
