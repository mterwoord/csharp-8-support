using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    public static class TaskAwaiterEx
    {
        internal static void UnsafeOnCompletedInternal(Task task, IAsyncStateMachineBox stateMachineBox, bool continueOnCapturedContext)
        {
            //Debug.Assert(stateMachineBox != null);

            // If TaskWait* ETW events are enabled, trace a beginning event for this await
            // and set up an ending event to be traced when the asynchronous await completes.
            //if (TplEventSource.Log.IsEnabled() || Task.s_asyncDebuggingEnabled)
            //{
            //    task.SetContinuationForAwait(OutputWaitEtwEvents(task, stateMachineBox.MoveNextAction), continueOnCapturedContext, flowExecutionContext: false);
            //}
            //else
            {
                TaskEx.UnsafeSetContinuationForAwait(task, stateMachineBox, continueOnCapturedContext);
            }
        }


    }
}