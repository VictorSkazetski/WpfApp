using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppDB.DataAccessLayer;
using WpfAppDB.Model;

namespace WpfAppDB.ViewModel
{
    class MainVm : ViewModelBase
    {
        public ICommand Employees { get; set; }
        public ICommand AddEmployee { get; set; }

        private object selectedViewModel;

        public object SelectedViewModel
        {
            get => selectedViewModel;
            set 
            {
                selectedViewModel = value; 
                OnPropertyChanged();
            }

        }

        
        private void GetViewEmployees(object _)
        {
            SelectedViewModel = new EmployeesVm();
        }


        private void GetViewAddEmployee(object obj)
        {
            SelectedViewModel = new AddEmployeeVm();
        }


        public MainVm()
        {
            SelectedViewModel = new EmployeesVm();

            Employees = new RelayCommand(GetViewEmployees);
            AddEmployee = new RelayCommand(GetViewAddEmployee);
           
        }
    }
}
