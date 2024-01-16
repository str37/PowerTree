using CommunityToolkit.Mvvm.ComponentModel;
using PowerTree.Maui.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {

        //protected readonly ILogicLayer _logicLayer;
        protected readonly IUnitOfWork _unitOfWork;
        //protected readonly INavigationService _navigationService;

        public BaseViewModel( IUnitOfWork unitOfWork)/*INavigationService navigationService,*/
        {
            _unitOfWork = unitOfWork;
            //_navigationService = navigationService;

            //_logicLayer = logicLayer;
        }
    }
}
