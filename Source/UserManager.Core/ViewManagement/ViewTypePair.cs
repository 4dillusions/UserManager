using System;
using System.Windows;

namespace UserManager.Core.ViewManagement
{
    public class ViewTypePair
    {
        public Type ViewType { get; private set; }
        public Type ViewModelType { get; private set; }
        public FrameworkElement CachedView { get; set; }

        public ViewTypePair(Type viewType, Type viewModelType)
        {
            this.ViewType = viewType;
            this.ViewModelType = viewModelType;
        } 
    }
}
