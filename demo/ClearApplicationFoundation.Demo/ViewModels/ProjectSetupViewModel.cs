using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using ClearApplicationFoundation.Demo.Features.Title;
using ClearApplicationFoundation.Framework;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.Demo.ViewModels
{
    public class ProjectSetupViewModel :Screen, IWorkflowStepViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMediator _mediator;
        private string? _title;
        

        public ProjectSetupViewModel()
        {
        }

        public ProjectSetupViewModel(IEventAggregator eventAggregator, ILogger<HomeViewModel> logger, IMediator mediator)
        {
            _eventAggregator = eventAggregator;
            _mediator = mediator;
            logger.LogDebug("HomeViewModel ctor called!");
        }

        public string? Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Direction Direction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task MoveBackwards()
        {
            throw new NotImplementedException();
        }

        public Task MoveBackwardsAction()
        {
            throw new NotImplementedException();
        }

        public Task MoveForwards()
        {
            throw new NotImplementedException();
        }

        public Task MoveForwardsAction()
        {
            throw new NotImplementedException();
        }

        protected async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
           
            var tabs = IoC.GetAll<ITab>();

           

        }
    }
}
