using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using UserManager.Core.ViewManagement;
using UserManager.ViewModel;
using UserManager.ViewModel.ViewManagement;

namespace UserManager.View.ViewManagement
{
    public class ViewTypeConverter : ConverterMarkupExtension<ViewTypeConverter>
    {
        private static Dictionary<ViewType, ViewTypePair> logics;

        public ViewTypeConverter()
        { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (logics == null)
            {
                logics = new Dictionary<ViewType, ViewTypePair>();

                logics.Add(ViewType.Login, new ViewTypePair(typeof(LoginView), typeof(LoginViewModel)));
                logics.Add(ViewType.UserList, new ViewTypePair(typeof(UserListView), typeof(UserListViewModel)));
                logics.Add(ViewType.User, new ViewTypePair(typeof(UserView), typeof(UserViewModel)));
            }

            return base.ProvideValue(serviceProvider);
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var logic = logics[ConverterHelper.GetValue<ViewType>(value)];

            //get instance
            //if (logic.CachedView != null)
                //return logic.CachedView;

            //create new instance
            logic.CachedView = null;

            logic.CachedView = (FrameworkElement)Activator.CreateInstance(logic.ViewType);
            if (logic.ViewModelType != null)
                logic.CachedView.DataContext = Activator.CreateInstance(logic.ViewModelType);

            return logic.CachedView;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    } 
}
