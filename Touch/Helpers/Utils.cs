#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace Touch.Helpers
{
    internal static class Utils
    {
        public static async Task<IEnumerable<TOut>> WaitAllTasksAsync<TIn, TOut>(IEnumerable<TIn> input,
            Func<TIn, Task<TOut>> func)
        {
            var tasks = input.Select(func).ToList();
            // Wait for all tasks in parallel
            await Task.WhenAll(tasks);
            return tasks.Select(task => task.Result);
        }
    }
}