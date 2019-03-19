#if NET452

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    public static partial class TaskEx
    {
        public static Task CompletedTask => Task.FromResult(0);

        public static Task FromCanceled(CancellationToken token)
        {
            var task = new TaskCompletionSource<byte>();
            task.SetCanceled();
            return task.Task;
        }

        public static Task<T> FromCanceled<T>(CancellationToken token)
        {
            var task = new TaskCompletionSource<T>();
            task.SetCanceled();
            return task.Task;
        }

        public static Task FromException(Exception exception)
        {
            var task = new TaskCompletionSource<byte>();
            task.SetException(exception);
            return task.Task;
        }

        public static Task<T> FromException<T>(Exception exception)
        {
            var task = new TaskCompletionSource<T>();
            task.SetException(exception);
            return task.Task;
        }
    }
}
#endif