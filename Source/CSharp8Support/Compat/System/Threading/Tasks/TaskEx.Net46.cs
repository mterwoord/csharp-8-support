#if NET46
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    public static partial class TaskEx
    {
        public static Task CompletedTask => Task.CompletedTask;

        public static Task FromCanceled(CancellationToken token)
        {
            return Task.FromCanceled(token);
        }

        public static Task<T> FromCanceled<T>(CancellationToken token)
        {
            return Task.FromCanceled<T>(token);
        }

        public static Task FromException(Exception exception)
        {
            return Task.FromException(exception);
        }

        public static Task<T> FromException<T>(Exception exception)
        {
            return Task.FromException<T>(exception);
        }
    }
}
#endif