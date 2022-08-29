using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClearApplicationFoundation.ViewModels.Infrastructure
{
    public abstract  class WorkflowShellViewModel : Conductor<IWorkflowStepViewModel>.Collection.OneActive
    {
        protected ILogger? Logger { get; set; }
        public List<IWorkflowStepViewModel>? Steps { get; set; }
        protected IEventAggregator? EventAggregator { get; set; }
        protected INavigationService? NavigationService { get; set; }
        protected IMediator? Mediator { get; set; }
        protected ILifetimeScope? LifetimeScope { get; set; }


        private FlowDirection _windowFlowDirection = FlowDirection.LeftToRight;
        public FlowDirection WindowFlowDirection
        {
            get => _windowFlowDirection;
            set => Set(ref _windowFlowDirection, value, nameof(WindowFlowDirection));
        }

        private bool _isLastWorkflowStep;
        public bool IsLastWorkflowStep
        {
            get => _isLastWorkflowStep;
            set => Set(ref _isLastWorkflowStep, value, nameof(IsLastWorkflowStep));
        }

        private bool _enableControls;
        public bool EnableControls
        {
            get => _enableControls;
            set
            {
                Logger!.LogInformation($"WorkflowShellViewModel - Setting EnableControls to {value} at {DateTime.Now:HH:mm:ss.fff}");
                Set(ref _enableControls, value);
            }
        }

        protected WorkflowShellViewModel(ILifetimeScope? lifetimeScope)
        {
            LifetimeScope = lifetimeScope;
        }
        protected WorkflowShellViewModel(IMediator? mediator,  ILogger? logger, INavigationService? navigationService, IEventAggregator? eventAggregator, ILifetimeScope? lifetimeScope)
        {
            Mediator = mediator;
            Logger = logger;
            NavigationService = navigationService;
            EventAggregator = eventAggregator;
            LifetimeScope = lifetimeScope;
            Steps = new List<IWorkflowStepViewModel>();

            //WindowFlowDirection = ProjectManager.CurrentLanguageFlowDirection;

        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            EventAggregator.SubscribeOnUIThread(this);
            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            EventAggregator!.Unsubscribe(this);
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public override Task ActivateItemAsync(IWorkflowStepViewModel item, CancellationToken cancellationToken = new CancellationToken())
        {
            // TODO:  do whatever needs to be done when a new workflow step has been activated.
            return base.ActivateItemAsync(item, cancellationToken);
        }

        protected IWorkflowStepViewModel? CurrentStep { get; set; }
        protected override IWorkflowStepViewModel DetermineNextItemToActivate(IList<IWorkflowStepViewModel> list, int lastIndex)
        {
            var current = list[lastIndex];

            var currentIndex = Steps!.IndexOf(current);
            IWorkflowStepViewModel? next;
            switch (current.Direction)
            {
                case Direction.Forwards:
                    next = currentIndex < Steps.Count - 1 ? Steps[++currentIndex] : current;
                    break;
                case Direction.Backwards:
                    next = currentIndex > 0 ? Steps[--currentIndex] : current;
                    break;
                default:
                    return current;
            }

            IsLastWorkflowStep = currentIndex == Steps.Count - 1;

            CurrentStep = next;
            return next;
        }
    }
}
