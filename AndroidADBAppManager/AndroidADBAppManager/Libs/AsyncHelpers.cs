﻿#nullable enable

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AndroidADBAppManager.Libs
{
    internal static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RunSync(this Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            ExclusiveSynchronizationContext synch = new();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? RunSync<T>(this Func<Task<T?>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T? ret = default(T);
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task.Invoke();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        [DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? RunSync<T>(this Task<T> task)
        {
            var ret = RunSync(() => task!);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception? InnerException { get; set; }
            readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object?>> items =
                new Queue<Tuple<SendOrPostCallback, object?>>();

            public override void Send(SendOrPostCallback d, object? state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object? state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop() => Post(_ => done = true, null);

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object?>? task = null;
                    lock (items)
                    {
                        if (items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            //throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                            throw InnerException;
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
}
