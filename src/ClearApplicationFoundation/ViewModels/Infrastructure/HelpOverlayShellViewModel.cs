using Autofac;
using Caliburn.Micro;
using ClearApplicationFoundation.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ClearApplicationFoundation.ViewModels.Infrastructure
{

    public abstract class HelpOverlayShellViewModel: Conductor<IWorkflowStepViewModel>.Collection.OneActive
    {
        protected ILogger? Logger { get; set; }

        protected ILifetimeScope? LifetimeScope { get; set; }

        protected IWorkflowStepViewModel? CurrentStep { get; set; }

        public List<IWorkflowStepViewModel>? Steps { get; set; }

        public bool IsVisible
        {
            get => isVisible_;
            set => Set(ref isVisible_, value);
        }

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
        private bool isVisible_;


        public bool EnableControls
        {
            get => _enableControls;
            set
            {
                Logger!.LogInformation($"WorkflowShellViewModel - Setting EnableControls to {value} at {DateTime.Now:HH:mm:ss.fff}");
                Set(ref _enableControls, value);
            }
        }

        protected HelpOverlayShellViewModel()
        {
            // allows view models to be used in design-time mode.
        }

        protected HelpOverlayShellViewModel(ILogger? logger, ILifetimeScope? lifetimeScope)
        {

            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            LifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope)); ;
            Steps = new List<IWorkflowStepViewModel>();
        }

     
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
