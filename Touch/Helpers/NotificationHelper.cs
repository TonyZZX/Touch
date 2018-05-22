#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace Touch.Helpers
{
    /// <summary>
    ///     Help to set up property changed event with data bindings.
    /// </summary>
    public class NotificationHelper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // SetField (Name, value); // where there is a data member
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string property = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(property);
            return true;
        }

        // SetField(()=> somewhere.Name = value; somewhere.Name, value) 
        // Advanced case where you rely on another property
        protected bool SetProperty<T>(T currentValue, T newValue, Action doSet,
            [CallerMemberName] string property = null)
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue)) return false;
            doSet.Invoke();
            RaisePropertyChanged(property);
            return true;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    /// <summary>
    ///     Help to set up property changed event with data bindings.
    /// </summary>
    /// <typeparam name="T">Property type</typeparam>
    public class NotificationHelper<T> : NotificationHelper where T : class, new()
    {
        protected T This;

        public NotificationHelper(T thing = null)
        {
            This = thing ?? new T();
        }

        public static implicit operator T(NotificationHelper<T> thing)
        {
            return thing.This;
        }
    }
}