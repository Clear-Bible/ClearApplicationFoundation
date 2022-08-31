using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.Demo.Features.Title;
using ClearApplicationFoundation.Framework;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.Demo.ViewModels
{
    public class ProjectSetupViewModel : WorkflowStepViewModel
    {

        public ProjectSetupViewModel(INavigationService navigationService, ILogger<HomeViewModel> logger, 
            IEventAggregator eventAggregator, IMediator mediator,  ILifetimeScope lifetimeScope) : 
            base(navigationService, logger, eventAggregator, mediator, lifetimeScope)
        {

        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {

            CanMoveForwards = true;
            CanMoveBackwards = true;
            EnableControls = true;
            await Task.CompletedTask;
        }

        public async Task Create()
        {
            var startupViewModel = Parent as StartupViewModel;
            startupViewModel?.Ok();
            await Task.CompletedTask;
        }
    }
}
