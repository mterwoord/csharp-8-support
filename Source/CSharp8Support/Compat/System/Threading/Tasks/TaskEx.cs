using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    public static partial class TaskEx
    {
        /// <summary>
        /// Sets a continuation onto the <see cref="System.Threading.Tasks.Task"/>.
        /// The continuation is scheduled to run in the current synchronization context is one exists, 
        /// otherwise in the current task scheduler.
        /// </summary>
        /// <param name="stateMachineBox">The action to invoke when the <see cref="System.Threading.Tasks.Task"/> has completed.</param>
        /// <param name="continueOnCapturedContext">
        /// true to attempt to marshal the continuation back to the original context captured; otherwise, false.
        /// </param>
        /// <exception cref="System.InvalidOperationException">The awaiter was not properly initialized.</exception>
        internal static void UnsafeSetContinuationForAwait(Task self, IAsyncStateMachineBox stateMachineBox, bool continueOnCapturedContext)
        {
            Debug.Assert(stateMachineBox != null);

            // This code path doesn't emit all expected TPL-related events, such as for continuations.
            // It's expected that all callers check whether events are enabled before calling this function,
            // and only call it if they're not, so we assert. However, as events can be dynamically turned
            // on and off, it's possible this assert could fire even when used correctly.  If it becomes
            // noisy, it can be deleted.
            //Debug.Assert(!TplEventSource.Log.IsEnabled());

            // If the caller wants to continue on the current context/scheduler and there is one,
            // fall back to using the state machine's delegate.
            if (continueOnCapturedContext)
            {
                SynchronizationContext syncCtx = SynchronizationContext.Current;
                if (syncCtx != null && syncCtx.GetType() != typeof(SynchronizationContext))
                {
                    var tc = NewSynchronizationContextAwaitTaskContinuation(syncCtx, stateMachineBox.MoveNextAction, flowExecutionContext: false);
                    if (!AddTaskContinuation(self, tc, addBeforeOthers: false))
                    {
                        RunContinuation(tc, self, canInlineContinuationTask: false);
                    }
                    return;
                }
                else
                {
                    TaskScheduler scheduler = TaskScheduler.Current;
                    if (scheduler != TaskScheduler.Default)
                    {
                        var tc = NewTaskSchedulerAwaitTaskContinuation(scheduler, stateMachineBox.MoveNextAction, flowExecutionContext: false);
                        if (!AddTaskContinuation(self, tc, addBeforeOthers: false))
                        {
                            RunContinuation(tc, self, canInlineContinuationTask: false);
                        }
                        return;
                    }
                }
            }

            // Otherwise, add the state machine box directly as the continuation.
            // If we're unable to because the task has already completed, queue it.
            if (!AddTaskContinuation(self, stateMachineBox, addBeforeOthers: false))
            {
                Debug.Assert(stateMachineBox is Task, "Every state machine box should derive from Task");
                ThreadPoolEx.UnsafeQueueUserWorkItemInternal(stateMachineBox, preferLocal: true);
            }
        }

        private static bool AddTaskContinuation(Task self, object tc, bool addBeforeOthers)
        {
            throw new NotImplementedException("via reflection");
        }

        private static object NewSynchronizationContextAwaitTaskContinuation(SynchronizationContext context, Action moveNextAction, bool flowExecutionContext)
        {
            throw new NotImplementedException("via reflection");
        }

        private static object NewTaskSchedulerAwaitTaskContinuation(TaskScheduler scheduler, Action moveNextAction, bool flowExecutionContext)
        {
            throw new NotImplementedException("via reflection");
        }

        private static void RunContinuation(object self, Task task, bool canInlineContinuationTask)
        {
            throw new NotImplementedException("via reflection");
        }
    }
}