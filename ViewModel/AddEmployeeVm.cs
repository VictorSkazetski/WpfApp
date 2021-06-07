using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WpfAppDB.DataAccessLayer;
using WpfAppDB.Model;

namespace WpfAppDB.ViewModel
{
    class AddEmployeeVm : ViewModelBase, IDataErrorInfo
    {
        private int emplId;

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private string surname;
        public string Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged();
            }
        }

        private string middleName;
        public string MiddleName
        {
            get => middleName;
            set
            {
                middleName = value;
                OnPropertyChanged();
            }
        }

        private DateTime birthday;
        public DateTime Birthday
        {
            get => birthday;
            set
            {
                birthday = value;
                OnPropertyChanged();
            }
        }

        private string department;
        public string Department
        {
            get => department;
            set
            {
                department = value;
                OnPropertyChanged();
            }
        }

        private bool flag;

        public bool AddChangeFlag
        {
            get => flag;
            set
            {
                flag = value;
                OnPropertyChanged();
            }
        }

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

        public bool ContainsError { get; set; }
        public string Error => string.Empty;

        public ICommand AddEmployee { get; set; }
        public ICommand ChangeEmployee { get; set; }
        

        private async void AddTableRow(object _)
        {
            if (ContainsError)
            {
                MessageBox.Show("Форма содержит ошибки");
            }
            else
            {
                await DataController.ExecutQuery($"insert EMPLOYEES(NAME, SURNAME, MIDDLE_NAME, BIRTHDAY, DEPARTMENT)" +
                                                 $" values('{Name}', '{Surname}', '{MiddleName}', convert(datetime, '{Birthday.ToShortDateString()}', 104), '{Department}')");

                SelectedViewModel = new EmployeesVm();
            }
        }


        private async void UpdateTableRow(object _)
        {
            if (ContainsError)
            {
                MessageBox.Show("Форма содержит ошибки");
            }
            else
            {
                await DataController.ExecutQuery($"update EMPLOYEES set NAME = '{Name}', " +
                                                 $"SURNAME = '{Surname}', " +
                                                 $"MIDDLE_NAME = '{MiddleName}', " +
                                                 $"BIRTHDAY = convert(datetime,'{Birthday.ToShortDateString()}', 104), " +
                                                 $"DEPARTMENT = '{Department}'" +
                                                 $"where Id = {emplId}");

                SelectedViewModel = new EmployeesVm();
            }
        }

        private string FieldsCheck(string field)
        {

            if (string.IsNullOrWhiteSpace(field) || string.IsNullOrEmpty(field))
                return "Поле не должно быть пустое";
            if (field?.Length > 50)
                return "Поле может содержать только 50 символов";
            if (!Regex.IsMatch(field, @"^[a-zA-Zа-яA-Я]+$"))
                return "Поле может содержать только алфавитные символы";

            return string.Empty;
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;

                switch (columnName)
                {
                    case nameof(Name):
                        error = FieldsCheck(Name);
                        break;
                    case nameof(Surname):
                        error = FieldsCheck(Surname);
                        break;
                    case nameof(MiddleName):
                        error = FieldsCheck(MiddleName);
                        break;
                    case nameof(Department):
                        error = FieldsCheck(Department);
                        break;

                }

                if (!string.IsNullOrEmpty(error)) ContainsError = true;
                else ContainsError = false;

                return error;
            }
        }


        public AddEmployeeVm()
        {
            Birthday = DateTime.Now;

            AddEmployee = new RelayCommand(AddTableRow);
        }


        public AddEmployeeVm(bool flag, Employee emp)
        {
            AddChangeFlag = flag;

            emplId = emp.ID;
            Name = emp.Name;
            Surname = emp.Surname;
            MiddleName = emp.MiddleName;
            Birthday = emp.Birthday;
            Department = emp.Department;

            ChangeEmployee = new RelayCommand(UpdateTableRow);
        }
    }
}
