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
        public string PositionName { get; set; }
        public decimal PositionSalary { get; set; }
        public int PositionMaxNumber { get; set; }
        public Department PositionDepartment { get; set; }

        //свойства для сотрудников
        public string UserName { get; set; }
        public string UserSurName { get; set; }
        public string UserPhone { get; set; }
        public Position UserPosition { get; set; }

        //свойства для выделенных элементов
        public TabItem SelectedTabItem { get; set; }
        public User SelectedUser { get; set; }
        public Position SelectedPosition { get; set; }
        public Department SelectedDepartment { get; set; }


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
        private RelayCommand addNewPosition;
        public RelayCommand AddNewPosition
        {
            get
            {
                return addNewPosition ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string resultStr = "";
                    if (PositionName == null || PositionName.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(window, "NameBlock");
                    }
                    if (PositionSalary == 0)
                    {
                        SetRedBlockControll(window, "SalaryBlock");
                    }
                    if (PositionMaxNumber == 0)
                    {
                        SetRedBlockControll(window, "SalaryBlock");
                    }
                    if (PositionDepartment == null)
                    {
                        MessageBox.Show("Укажите отдел");
                    }
                    else
                    {
                        resultStr = DataWorker.CreatePosition(PositionName, PositionSalary, PositionMaxNumber, PositionDepartment);
                        UpdateAllDataView();
                        ShowMessageToUser(resultStr);
                        SetNullValuesToProperties();
                        window.Close();
                    }
                });
            }
        }

        private RelayCommand addNewUser;
        public RelayCommand AddNewUser
        {
            get
            {
                return addNewUser ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string resultStr = "";
                    if (UserName == null || UserName.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(window, "NameBlock");
                    }
                    if (UserSurName == null || UserName.Replace(" ", "").Length == 0)
                    {
                        SetRedBlockControll(window, "SurNameBlock");
                    }
                    //if (UserPhone == null || UserPhone.Replace(" ", "").Length == 0)
                    //{
                    //    SetRedBlockControll(window, "PhoneBlock");
                    //}
                    if (UserPosition == null)
                    {
                        MessageBox.Show("Укажите позицию");
                    }
                    else
                    {
                        resultStr = DataWorker.CreateUser(UserName, UserSurName, UserPhone, UserPosition);
                        UpdateAllDataView();
                        ShowMessageToUser(resultStr);
                        SetNullValuesToProperties();
                        window.Close();
                    }
                });
            }
        }

        #endregion

        private RelayCommand deleteItem;
        public RelayCommand DeleteItem
        {
            get
            {
                return deleteItem ?? new RelayCommand(obj =>
                {
                    string resultStr = "Ничего не выбрано";
                    //если сотрудник
                    if (SelectedTabItem.Name == "UsersTab" && SelectedUser != null)
                    {
                        resultStr = DataWorker.DeleteUser(SelectedUser);
                        UpdateAllDataView();
                    }
                    //если позиция
                    if (SelectedTabItem.Name == "PositionsTab" && SelectedPosition != null)
                    {
                        resultStr = DataWorker.DeletePosition(SelectedPosition);
                        UpdateAllDataView();
                    }
                    //если отдел
                    if (SelectedTabItem.Name == "DepartmentsTab" && SelectedDepartment != null)
                    {
                        resultStr = DataWorker.DeleteDepartment(SelectedDepartment);
                        UpdateAllDataView();
                    }
                    //обновление
                    SetNullValuesToProperties();
                    ShowMessageToUser(resultStr);
                });
            }
        }
        #region EDIT COMMANDS
        private RelayCommand editUser;
        public RelayCommand EditUser
        {
            get
            {
                return editUser ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string resultStr = "Не выбран сотрудник";
                    string noPositionStr = "Не выбрана новая должность";
                    if (SelectedUser != null)
                    {
                        if(UserPosition != null)
                        {
                            resultStr = DataWorker.EditUser(SelectedUser, UserName, UserSurName, UserPhone, UserPosition);
                            UpdateAllDataView();
                            SetNullValuesToProperties();
                            ShowMessageToUser(resultStr);
                            window.Close();
                        }
                        else ShowMessageToUser(noPositionStr);
                    }
                    else ShowMessageToUser(resultStr);
                });
            }
        }
        private RelayCommand editPosition;
        public RelayCommand EditPosition
        {
            get
            {
                return editPosition ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string resultStr = "Не выбрана позиция";
                    string noDepartmentStr = "Не выбрана новый отдел";
                    if (SelectedPosition != null)
                    {
                        if (PositionDepartment != null)
                        {
                            resultStr = DataWorker.EditPosition(SelectedPosition, PositionName, PositionSalary, PositionMaxNumber, PositionDepartment);
                            UpdateAllDataView();
                            SetNullValuesToProperties();
                            ShowMessageToUser(resultStr);
                            window.Close();
                        }
                        else ShowMessageToUser(noDepartmentStr);
                    }
                    else ShowMessageToUser(resultStr);
                });
            }
        }
        private RelayCommand editDepartment;
        public RelayCommand EditDepartment
        {
            get
            {
                return editDepartment ?? new RelayCommand(obj =>
                {
                    Window window = obj as Window;
                    string resultStr = "Не выбран отдел";
                    if (SelectedDepartment != null)
                    {
                            resultStr = DataWorker.EditDepartment(SelectedDepartment, DepartmentName);
                            UpdateAllDataView();
                            SetNullValuesToProperties();
                            ShowMessageToUser(resultStr);
                            window.Close();
                    }
                    else ShowMessageToUser(resultStr);
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
        private RelayCommand openEditItemWnd;
        public RelayCommand OpenEditItemWnd
        {
            get
            {
                return openEditItemWnd ?? new RelayCommand(obj =>
                {
                    string resultStr = "Ничего не выбрано";
                    //если сотрудник
                    if (SelectedTabItem.Name == "UsersTab" && SelectedUser != null)
                    {
                        OpenEditUserWindowMethod();
                    }
                    //если позиция
                    if (SelectedTabItem.Name == "PositionsTab" && SelectedPosition != null)
                    {
                        OpenEditPositionWindowMethod();
                    }
                    //если отдел
                    if (SelectedTabItem.Name == "DepartmentsTab" && SelectedDepartment != null) 
                    {
                        OpenEditDepartmentWindowMethod();
                    }
                });
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
        private void OpenEditDepartmentWindowMethod()
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
            UserName = null;
            UserSurName = null;
            UserPhone = null;
            UserPosition = null;
            //для позиции
            PositionName = null;
            PositionSalary = 0;
            PositionMaxNumber = 0;
            PositionDepartment = null;
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
