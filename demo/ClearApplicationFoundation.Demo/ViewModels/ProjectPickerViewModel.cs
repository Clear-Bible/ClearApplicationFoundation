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
    public class ProjectPickerViewModel:Screen, IWorkflowStepViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMediator _mediator;
        private string? _title;

        public Direction Direction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ProjectPickerViewModel()
        {
        }

        public ProjectPickerViewModel(IEventAggregator eventAggregator, ILogger<HomeViewModel> logger, IMediator mediator)
        {
            _eventAggregator = eventAggregator;
            _mediator = mediator;
            logger.LogDebug("HomeViewModel ctor called!");
        }

        protected async Task OnInitializeAsync(CancellationToken cancellationToken)
        {

            var tabs = IoC.GetAll<ITab>();
        }

        public async void CreateNewProject()
        {
            await TryCloseAsync(true);
        }
        

        public async Task MoveForwards()
        {
            Direction = Direction.Forwards;
            await MoveForwardsAction();
            await TryCloseAsync();
        }
        public async Task MoveBackwards()
        {
            Direction = Direction.Backwards;
            await TryCloseAsync();
        }

        public async Task MoveForwardsAction()
        {
            await Task.CompletedTask;
        }

        public Task MoveBackwardsAction()
        {
            throw new NotImplementedException();
        }
    }
}
