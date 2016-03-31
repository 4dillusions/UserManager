using UserManager.Core.Common;
using UserManager.Core.Factory;
using UserManager.ViewModel.ViewManagement;

namespace UserManager.ViewModel
{
    public class MainViewModel : NotificationObject
    {
        private ViewType viewType;

        public ViewType ViewType
        {
            get { return viewType; }

            set
            {
                viewType = value;
                NotifyPropertyChanged("ViewType");
            }
        }

        public MainViewModel()
        {
            Ioc<MainViewModel>.Register<MainViewModel>(() => this);
            Ioc<MainViewModel>.Create();
        }
    }
}
