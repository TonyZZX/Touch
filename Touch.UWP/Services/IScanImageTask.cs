#region

using System;
using System.Threading.Tasks;

#endregion

namespace Touch.Services
{
    public interface IScanImageTask
    {
        event EventHandler Running;

        event EventHandler ContentChanged;

        event EventHandler Completed;

        Task Start();
    }
}