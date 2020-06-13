using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo
{
    class RerunableTaskSequence
    {
        Action[] ActionSequence = null;
        Func< bool>[] ActionValidationSequence = null;
        string[] ActionNameSequence = null;
        bool ForceRun = false;
        public RerunableTaskSequence(Action[] actionSequence, Func<bool>[] actionValidationSequence, bool forceRun = false, string[] actionNameSequence=null)
        {
            Sanity.Requires(actionSequence != null, "Action sequence is null.");
            Sanity.Requires(actionValidationSequence != null, "Action validation is null.");
            Sanity.Requires(actionSequence.Length == actionValidationSequence.Length, "The length of action sequence has to be the same as action validation sequence.");
            Sanity.Requires(actionNameSequence == null || actionSequence.Length == actionNameSequence.Length, "The length of action sequence has to be the same as action name sequence");
            ActionSequence = actionSequence.ToArray();
            ActionValidationSequence = actionValidationSequence.ToArray();
            ActionNameSequence = actionNameSequence ?? actionNameSequence.ToArray();
            ForceRun = forceRun;
        }
        public void Run()
        {
            for(int i = 0; i < ActionSequence.Length; i++)
            {
                string stepName = ActionNameSequence == null ? (i + 1).ToString() : ActionNameSequence[i];
                Logger.WriteLine($"Start to validate step {stepName}...");
                if (ActionValidationSequence[i]()&&!ForceRun)
                {
                    Logger.WriteLine($"Step {stepName} has completed, force run is disabled. Skip step {stepName}.");
                    continue;
                }
                if (ForceRun)
                    Logger.WriteLine($"Force run is enabled. Run step {stepName}.");
                else
                    Logger.WriteLine($"Step {stepName} is not complete. Run step {stepName}.");
                ActionSequence[i].Invoke();
            }
        }
    }
}
