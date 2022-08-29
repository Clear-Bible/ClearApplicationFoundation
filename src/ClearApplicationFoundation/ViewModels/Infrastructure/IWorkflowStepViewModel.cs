using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearApplicationFoundation.ViewModels.Infrastructure
{
    public interface IWorkflowStepViewModel
    {
        Direction Direction { get; set; }
        Task MoveForwards();
        Task MoveBackwards();
        Task MoveForwardsAction();
        Task MoveBackwardsAction();
    }
}
