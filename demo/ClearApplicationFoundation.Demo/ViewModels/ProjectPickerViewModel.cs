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
    public class ProjectPickerViewModel : WorkflowStepViewModel
    {
       
        public ProjectPickerViewModel(IEventAggregator eventAggregator, ILogger<HomeViewModel> logger, IMediator mediator, INavigationService navigationService) : base(eventAggregator, navigationService, logger, mediator)
        {
          
        }

        protected async Task OnInitializeAsync(CancellationToken cancellationToken)
        {

           CanMoveForwards = true;
           CanMoveBackwards = true;
           EnableControls = true;
        }
    }
}
