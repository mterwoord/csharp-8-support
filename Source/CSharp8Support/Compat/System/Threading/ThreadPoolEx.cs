namespace System.Threading
{
    public static class ThreadPoolEx
    {
        public static void UnsafeQueueUserWorkItemInternal(object callback, bool preferLocal)
        {
            throw new NotImplementedException("via reflection");
        }
    }
}