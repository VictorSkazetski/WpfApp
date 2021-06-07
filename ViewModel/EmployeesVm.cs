using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfAppDB.DataAccessLayer;
using WpfAppDB.Model;

namespace WpfAppDB.ViewModel
{
    class EmployeesVm : ViewModelBase
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        private object selectedViewModel { get; set; }

        public object SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                OnPropertyChanged();
            }

        }

        public ICommand DropRow { get; set; }

        public ICommand UpdateEmployee { get; set; }

        private async void DropTableRow(object obj)
        {

            int id = ((Employee)obj).ID;

            Employees.Remove(Employees
                .First(emp => emp.ID == id));

            await DataController.ExecutQuery($"delete from employees where Id = {id}");

        }


        private async Task LoadData()
        {

            var data = await DataController.GetAllData();

            foreach (var emp in data)
            {
                Employees.Add(emp);
            }

        }


        private void GetViewAddEmployee_Upd(object emp)
        {
            SelectedViewModel = new AddEmployeeVm(true, (Employee)emp);
        }


        public EmployeesVm()
        {
            LoadData();

            DropRow = new RelayCommand(DropTableRow);
            UpdateEmployee = new RelayCommand(GetViewAddEmployee_Upd);
        }
    }
}
