namespace System.Threading.Tasks
{
    public static class TaskCompletionSourceEx
    {
        public static void TrySetCanceled<T>(this TaskCompletionSource<T> self, CancellationToken cancellationToken)
        {
            self.TrySetCanceled();
        }
    }
}