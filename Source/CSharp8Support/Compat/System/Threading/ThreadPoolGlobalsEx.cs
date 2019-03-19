using System.Runtime.CompilerServices;

namespace System.Threading
{
    public static class ThreadPoolGlobalsEx
    {
        internal static readonly Action<object> s_invokeAsyncStateMachineBox = state =>
                                                                               {
                                                                                   if (!(state is IAsyncStateMachineBox box))
                                                                                   {
                                                                                       ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.state);
                                                                                       return;
                                                                                   }

                                                                                   box.MoveNext();
                                                                               };

    }
}