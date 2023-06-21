using ManageStaffDBApp.Model;
using ManageStaffDBApp.View;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace ManageStaffDBApp.ViewModel
{
    internal class DataManageVM : INotifyPropertyChanged
    {
        // Все отделы
        private List<Department> allDepartments = DataWorker.GetAllDepartments();
        public List<Department> AllDepartments
        { 
            get { return allDepartments; }
            set 
            { 
                allDepartments = value;
                NotifyPropertyChanged("AllDepartments:");
            }
        }
        // Все позиции
        private List<Position> allPositions= DataWorker.GetAllPositions();
        public List<Position> AllPositions
        {
            get { return allPositions; }
            set
            {
                allPositions = value;
                NotifyPropertyChanged("AllPositions:");
            }
        }
        // Все сотрудники
        private List<User> allUsers = DataWorker.GetAllUsers();
        public List<User> AllUsers
        {
            get { return allUsers; }
            set
            {
                allUsers = value;
                NotifyPropertyChanged("AllUsers:");
            }
        }

        //свойства для отдела
        public string DepartmentName { get; set; }
        //свойства для позиций

        //свойства для сотрудников

        #region COMMANDS TO ADD
        private RelayCommand addNewDepartment;
        public RelayCommand AddNewDepartment
        {
            get
            {
                return addNewDepartment ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string resultStr = "";
                    if (DepartmentName == null || DepartmentName.Replace(" ","").Length == 0)
                    {
                        SetRedBlockControll(window, "NameBlock");
                    }
                    else
                    {
                        resultStr = DataWorker.CreateDepartment(DepartmentName);
                        UpdateAllDataView();
                        ShowMessageToUser(resultStr);
                        SetNullValuesToProperties();
                        window.Close();
                    }
                });
            }
        }


        #endregion

        // Команды открытия окон
        #region COMMAND TO OPEN WINDOW
        private RelayCommand openAddNewDepartmentWindow;
        public RelayCommand OpenAddNewDepartmentWindow
        {
            get
            {
                return openAddNewDepartmentWindow ?? new RelayCommand(obj =>
                {
                    OpenAddDepartmentWindowMethod();
                }
                );
            }
        }
        private RelayCommand openAddNewPositionWindow;
        public RelayCommand OpenAddNewPositionWindow
        {
            get
            {
                return openAddNewPositionWindow ?? new RelayCommand(obj =>
                {
                    OpenAddPositionWindowMethod();
                }
                );
            }
        }
        private RelayCommand openAddNewUserWindow;
        public RelayCommand OpenAddNewUserWindow
        {
            get
            {
                return openAddNewUserWindow ?? new RelayCommand(obj =>
                {
                    OpenAddUserWindowMethod();
                }
                );
            }
        }
        #endregion
        // Методы открытия окон
        #region METHODS TO OPEN WINDOW
        private void OpenAddDepartmentWindowMethod()
        {
            AddNewDepartmentWindow newDepartmentWindow = new AddNewDepartmentWindow();
            SetCenterPositionAndOpen(newDepartmentWindow);
        }
        private void OpenAddPositionWindowMethod()
        {
            AddNewPositionWindow newPositionWindow = new AddNewPositionWindow();
            SetCenterPositionAndOpen(newPositionWindow);
        }
        private void OpenAddUserWindowMethod()
        {
            AddNewUserWindow newUserWindow = new AddNewUserWindow();
            SetCenterPositionAndOpen(newUserWindow);
        }
        #endregion
        // Окна редактирования
        #region METHODS TO EDIT WINDOW
        private void OpeEditDepartmentWindowMethod()
        {
            EditDepartmentWindow editDepartmentWindow = new EditDepartmentWindow();
            SetCenterPositionAndOpen(editDepartmentWindow);
        }
        private void OpenEditPositionWindowMethod()
        {
            EditPositionWindow editPositionWindow = new EditPositionWindow();
            SetCenterPositionAndOpen(editPositionWindow);
        }
        private void OpenEditUserWindowMethod()
        {
            EditUserWindow editUserWindow = new EditUserWindow();
            SetCenterPositionAndOpen(editUserWindow);
        }
        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }
        #endregion

        private void SetRedBlockControll(Window window, string blockName)
        {
            Control block = window.FindName(blockName) as Control;
            block.BorderBrush = Brushes.Red;
        }
        #region UPDATE VIEWS
        private void SetNullValuesToProperties()
        {
            //для пользователя
            //для позиции
            //для отдела
            DepartmentName = null;
        }
        private void UpdateAllDataView()
        {
            UpdateAllDepartmentsView();
            UpdateAllPositionsView();
            UpdateAllUsersView();
        }
        private void UpdateAllDepartmentsView()
        {
            AllDepartments = DataWorker.GetAllDepartments();
            MainWindow.AllDepartmentsView.ItemsSource = null;
            MainWindow.AllDepartmentsView.Items.Clear();
            MainWindow.AllDepartmentsView.ItemsSource = AllDepartments;
            MainWindow.AllDepartmentsView.Items.Refresh();
        }
        private void UpdateAllPositionsView()
        {
            AllPositions = DataWorker.GetAllPositions();
            MainWindow.AllPositionsView.ItemsSource = null;
            MainWindow.AllPositionsView.Items.Clear();
            MainWindow.AllPositionsView.ItemsSource = AllPositions;
            MainWindow.AllPositionsView.Items.Refresh();
        }
        private void UpdateAllUsersView()
        {
            AllUsers = DataWorker.GetAllUsers();
            MainWindow.AllUsersView.ItemsSource = null;
            MainWindow.AllUsersView.Items.Clear();
            MainWindow.AllUsersView.ItemsSource = AllUsers;
            MainWindow.AllUsersView.Items.Refresh();
        }
        #endregion
        private void ShowMessageToUser(string message)
        {
            MessageView messageView = new MessageView(message);
            SetCenterPositionAndOpen(messageView);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
