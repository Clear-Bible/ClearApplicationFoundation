using Caliburn.Micro;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;

using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ClearApplicationFoundation.Demo.DependencyInjectionTest;
using ClearApplicationFoundation.Demo.Features.Title;
using ClearApplicationFoundation.Framework;
using ClearApplicationFoundation.ViewModels.Infrastructure;
using MediatR;

namespace ClearApplicationFoundation.Demo.ViewModels
{
    public class HomeViewModel : ApplicationConductorOneActive<ITab>, IMainWindowViewModel
    {
       

        public HomeViewModel()
        {
            
        }

        public HomeViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ILogger<HomeViewModel> logger, IMediator mediator, IServiceProvider serviceProvider, ILifetimeScope lifetimeScope): base(navigationService, logger, eventAggregator, mediator, lifetimeScope)
        {
            logger.LogDebug("HomeViewModel ctor called!");
            var testService = serviceProvider.GetService<TestService>();

            var testService2 = lifetimeScope.Resolve<TestService>();

        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            Title = await Mediator.Send(new TitleQuery(), cancellationToken);
            
            var tabs = IoC.GetAll<ITab>();

            foreach (var tab in tabs)
            {
                await ActivateItemAsync(tab, cancellationToken);
            }

            await ActivateItemAsync(Items[0], cancellationToken);

           
        }
    }
}
