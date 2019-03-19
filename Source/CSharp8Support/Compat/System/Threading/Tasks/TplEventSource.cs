// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Diagnostics.Tracing;
using Internal.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    internal sealed class TplEventSource 
    {
 
        public static readonly TplEventSource Log = new TplEventSource();

        /// <summary>Configured behavior of a task wait operation.</summary>
        public enum TaskWaitBehavior : int
        {
            /// <summary>A synchronous wait.</summary>
            Synchronous = 1,
            /// <summary>An asynchronous await.</summary>
            Asynchronous = 2
        }

        /// <summary>ETW tasks that have start/stop events.</summary>
        public class Tasks // this name is important for EventSource
        {
            /// <summary>A parallel loop.</summary>
            public const EventTask Loop = (EventTask)1;
            /// <summary>A parallel invoke.</summary>
            public const EventTask Invoke = (EventTask)2;
            /// <summary>Executing a Task.</summary>
            public const EventTask TaskExecute = (EventTask)3;
            /// <summary>Waiting on a Task.</summary>
            public const EventTask TaskWait = (EventTask)4;
            /// <summary>A fork/join task within a loop or invoke.</summary>
            public const EventTask ForkJoin = (EventTask)5;
            /// <summary>A task is scheduled to execute.</summary>
            public const EventTask TaskScheduled = (EventTask)6;
            /// <summary>An await task continuation is scheduled to execute.</summary>
            public const EventTask AwaitTaskContinuationScheduled = (EventTask)7;

            /// <summary>AsyncCausalityFunctionality.</summary>
            public const EventTask TraceOperation = (EventTask)8;
            public const EventTask TraceSynchronousWork = (EventTask)9;
        }

        public class Keywords // thisname is important for EventSource
        {
            /// <summary>
            /// Only the most basic information about the workings of the task library
            /// This sets activity IDS and logs when tasks are schedules (or waits begin)
            /// But are otherwise silent
            /// </summary>
            public const EventKeywords TaskTransfer = (EventKeywords)1;
            /// <summary>
            /// TaskTranser events plus events when tasks start and stop 
            /// </summary>
            public const EventKeywords Tasks = (EventKeywords)2;
            /// <summary>
            /// Events associted with the higher level parallel APIs
            /// </summary>
            public const EventKeywords Parallel = (EventKeywords)4;
            /// <summary>
            /// These are relatively verbose events that effectively just redirect
            /// the windows AsyncCausalityTracer to ETW
            /// </summary>
            public const EventKeywords AsyncCausalityOperation = (EventKeywords)8;
            public const EventKeywords AsyncCausalityRelation = (EventKeywords)0x10;
            public const EventKeywords AsyncCausalitySynchronousWork = (EventKeywords)0x20;

            /// <summary>
            /// Emit the stops as well as the schedule/start events
            /// </summary>
            public const EventKeywords TaskStops = (EventKeywords)0x40;

            /// <summary>
            /// TasksFlowActivityIds indicate that activity ID flow from one task
            /// to any task created by it. 
            /// </summary>
            public const EventKeywords TasksFlowActivityIds = (EventKeywords)0x80;

            /// <summary>
            /// Events related to the happenings of async methods.
            /// </summary>
            public const EventKeywords AsyncMethod = (EventKeywords)0x100;

            /// <summary>
            /// TasksSetActivityIds will cause the task operations to set Activity Ids 
            /// This option is incompatible with TasksFlowActivityIds flow is ignored
            /// if that keyword is set.   This option is likely to be removed in the future
            /// </summary>
            public const EventKeywords TasksSetActivityIds = (EventKeywords)0x10000;

            /// <summary>
            /// Relatively Verbose logging meant for debugging the Task library itself. Will probably be removed in the future
            /// </summary>
            public const EventKeywords Debug = (EventKeywords)0x20000;
            /// <summary>
            /// Relatively Verbose logging meant for debugging the Task library itself.  Will probably be removed in the future
            /// </summary>
            public const EventKeywords DebugActivityId = (EventKeywords)0x40000;
        }

       

        //-----------------------------------------------------------------------------------
        //        
        // Task Events
        //

        // These are all verbose events, so we need to call IsEnabled(EventLevel.Verbose, ALL_KEYWORDS) 
        // call. However since the IsEnabled(l,k) call is more expensive than IsEnabled(), we only want 
        // to incur this cost when instrumentation is enabled. So the Task codepaths that call these
        // event functions still do the check for IsEnabled()

        #region TaskScheduled
        /// <summary>
        /// Fired when a task is queued to a TaskScheduler.
        /// </summary>
        /// <param name="OriginatingTaskSchedulerID">The scheduler ID.</param>
        /// <param name="OriginatingTaskID">The task ID.</param>
        /// <param name="TaskID">The task ID.</param>
        /// <param name="CreatingTaskID">The task ID</param>
        /// <param name="TaskCreationOptions">The options used to create the task.</param>
           public void TaskScheduled(
            int OriginatingTaskSchedulerID, int OriginatingTaskID,  // PFX_COMMON_EVENT_HEADER
            int TaskID, int CreatingTaskID, int TaskCreationOptions, int appDomain = -1)
        {
        
        }
        #endregion

        #region TaskStarted
        /// <summary>
        /// Fired just before a task actually starts executing.
        /// </summary>
        /// <param name="OriginatingTaskSchedulerID">The scheduler ID.</param>
        /// <param name="OriginatingTaskID">The task ID.</param>
        /// <param name="TaskID">The task ID.</param>
        public void TaskStarted(
            int OriginatingTaskSchedulerID, int OriginatingTaskID,  // PFX_COMMON_EVENT_HEADER
            int TaskID)
        {
        }
        #endregion

        #region TaskCompleted
        /// <summary>
        /// Fired right after a task finished executing.
        /// </summary>
        /// <param name="OriginatingTaskSchedulerID">The scheduler ID.</param>
        /// <param name="OriginatingTaskID">The task ID.</param>
        /// <param name="TaskID">The task ID.</param>
        /// <param name="IsExceptional">Whether the task completed due to an error.</param>
        public void TaskCompleted(
            int OriginatingTaskSchedulerID, int OriginatingTaskID,  // PFX_COMMON_EVENT_HEADER
            int TaskID, bool IsExceptional)
        {
          
        }
        #endregion

        #region TaskWaitBegin
        /// <summary>
        /// Fired when starting to wait for a taks's completion explicitly or implicitly.
        /// </summary>
        /// <param name="OriginatingTaskSchedulerID">The scheduler ID.</param>
        /// <param name="OriginatingTaskID">The task ID.</param>
        /// <param name="TaskID">The task ID.</param>
        /// <param name="Behavior">Configured behavior for the wait.</param>
        /// <param name="ContinueWithTaskID">
        /// If known, if 'TaskID' has a 'continueWith' task, mention give its ID here.  
        /// 0 means unknown.   This allows better visualization of the common sequential chaining case.
        /// </param>
        public void TaskWaitBegin(
            int OriginatingTaskSchedulerID, int OriginatingTaskID,  // PFX_COMMON_EVENT_HEADER
            int TaskID, TaskWaitBehavior Behavior, int ContinueWithTaskID)
        {
        
        }
        #endregion

        /// <summary>
        /// Fired when the wait for a tasks completion returns.
        /// </summary>
        /// <param name="OriginatingTaskSchedulerID">The scheduler ID.</param>
        /// <param name="OriginatingTaskID">The task ID.</param>
        /// <param name="TaskID">The task ID.</param>
        public void TaskWaitEnd(
            int OriginatingTaskSchedulerID, int OriginatingTaskID,  // PFX_COMMON_EVENT_HEADER
            int TaskID)
        {
         
        }

        /// <summary>
        /// Fired when the work (method) associated with a TaskWaitEnd completes
        /// </summary>
        /// <param name="TaskID">The task ID.</param>
        public void TaskWaitContinuationComplete(int TaskID)
        {
        }

        /// <summary>
        /// Fired when the work (method) associated with a TaskWaitEnd completes
        /// </summary>
        /// <param name="TaskID">The task ID.</param>
        public void TaskWaitContinuationStarted(int TaskID)
        {
        }

        /// <summary>
        /// Fired when the an asynchronous continuation for a task is scheduled
        /// </summary>
        /// <param name="OriginatingTaskSchedulerID">The scheduler ID.</param>
        /// <param name="OriginatingTaskID">The task ID.</param>
        public void AwaitTaskContinuationScheduled(
            int OriginatingTaskSchedulerID, int OriginatingTaskID,  // PFX_COMMON_EVENT_HEADER
            int ContinuwWithTaskId)
        {

        }

        public void TraceOperationBegin(int TaskID, string OperationName, long RelatedContext)
        {
        }

        public void TraceOperationRelation(int TaskID, CausalityRelation Relation)
        {
        }

        public void TraceOperationEnd(int TaskID, AsyncCausalityStatus Status)
        {
        }

        public void TraceSynchronousWorkBegin(int TaskID, CausalitySynchronousWork Work)
        {
        }

        public void TraceSynchronousWorkEnd(CausalitySynchronousWork Work)
        {
        }

        public unsafe void RunningContinuation(int TaskID, object Object) { RunningContinuation(TaskID, (long)*((void**)Unsafe.AsPointer(ref Object))); }

        private void RunningContinuation(int TaskID, long Object)
        {
        }

        public unsafe void RunningContinuationList(int TaskID, int Index, object Object) { RunningContinuationList(TaskID, Index, (long)*((void**)Unsafe.AsPointer(ref Object))); }

        public void RunningContinuationList(int TaskID, int Index, long Object)
        {
        }

        public void DebugFacilityMessage(string Facility, string Message)
        {

        }

        public void DebugFacilityMessage1(string Facility, string Message, string Value1)
        {
            
        }

        public void SetActivityId(Guid NewId)
        {
        }

        public void NewID(int TaskID)
        {
        }

        public void IncompleteAsyncMethod(IAsyncStateMachineBox stateMachineBox)
        {
        }

        private void IncompleteAsyncMethod(string stateMachineDescription)
        {
        }

        /// <summary>
        /// Activity IDs are GUIDS but task IDS are integers (and are not unique across appdomains
        /// This routine creates a process wide unique GUID given a task ID
        /// </summary>
        internal static Guid CreateGuidForTaskID(int taskID)
        {
            return Guid.NewGuid();
        }
    }
}
