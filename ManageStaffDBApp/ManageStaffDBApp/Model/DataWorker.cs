using ManageStaffDBApp.Model.Data;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ManageStaffDBApp.Model
{
    public static class DataWorker
    {
        // Получить все отделы
        public static List<Department> GetAllDepartments()
        {
            using(ApplicationContext db = new ApplicationContext()) 
            {
                return db.Departments.ToList();
            }
        }
        // Получить все позиции
        public static List<Position> GetAllPositions()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Positions.ToList();
            }
        }
        // Получить всех сотрудников
        public static List<User> GetAllUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Users.ToList();
            }
        }
        // Создать отдел
        public static string CreateDepartment(string name)
        {
            string result = "Отдел уже существует";
            using (ApplicationContext db = new ApplicationContext()) 
            {
                //Проверка на дубликат
                bool checkInExist = db.Departments.Any(x => x.Name == name);
                if (!checkInExist) 
                { 
                    Department newDepartment = new Department { Name = name };
                    db.Departments.Add(newDepartment);
                    db.SaveChanges();
                    result = "Отдел добавлен!";
                }
                return result;
            }
        }
        // Создать позицию
        public static string CreatePosition(string name, decimal salary, int maxNumber, Department departament)
        {
            string result = "Позиция уже существует";
            using (ApplicationContext db = new ApplicationContext())
            {
                //Проверка на дубликат
                bool checkInExist = db.Positions.Any(x => x.Name == name && x.Salary == salary);
                if (!checkInExist)
                {
                    Position newPosition = new Position 
                    { 
                        Name = name, 
                        Salary = salary, 
                        MaxNumber = maxNumber, 
                        DepartmentId = departament.Id 
                    };
                    db.Positions.Add(newPosition);
                    db.SaveChanges();
                    result = "Позиция добавлена!";
                }
                return result;
            }
        }
        // Создать сотрудника
        public static string CreateUser(string name, string surName, string phone, Position position)
        {
            string result = "Сотрудник уже существует";
            using (ApplicationContext db = new ApplicationContext())
            {
                //Проверка на дубликат
                bool checkInExist = db.Users.Any(x => x.Name == name && x.SurName == surName && x.Position == position);
                if (!checkInExist)
                {
                    User newUser = new User
                    {
                        Name = name,
                        SurName = surName,
                        Phone = phone,
                        PositionId = position.Id
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    result = "Сотрудник добавлен!";
                }
                return result;
            }
        }
        // Удалить отдел
        public static string DeleteDepartment(Department department)
        {
            string result = "Такого отдела не существует";
            using(ApplicationContext db = new ApplicationContext()) 
            {
                db.Departments.Remove(department);
                db.SaveChanges();
                result = $"Отдел {department.Name} удален";
            }
            return result;
        }
        // Удалить позицию
        public static string DeletePosition(Position position)
        {
            string result = "Такой позиции не существует";
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Positions.Remove(position);
                db.SaveChanges();
                result = $"Позиция {position.Name} удалена";
            }
            return result;
        }
        // Удалить сотрудника
        public static string DeleteUser(User user)
        {
            string result = "Такого сотрудника не существует";
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Users.Remove(user);
                db.SaveChanges();
                result = $"Сотрудник {user.Name} удален";
            }
            return result;
        }
        // Редактировать отдел
        public static string EditDepartment(Department oldDepartment, string newName)
        {
            string result = "Такого отдела не существует";
            using (ApplicationContext db = new ApplicationContext())
            {
                Department department = db.Departments.FirstOrDefault(x => x.Id == oldDepartment.Id);
                if (department != null)
                {
                    department.Name = newName;
                    db.SaveChanges();
                    result = $"Отдел {department.Name} изменен";
                }
            }
            return result;
        }
        // Редактировать позицию
        public static string EditPosition(Position oldPosition, string newName, decimal newSalary, int newMaxNumber, Department newDepartament)
        {
            string result = "Такой позиции не существует";
            using (ApplicationContext db = new ApplicationContext())
            {
                Position position = db.Positions.FirstOrDefault(x => x.Id == oldPosition.Id);
                if (position != null)
                {
                    position.Name = newName;
                    position.Salary = newSalary;
                    position.MaxNumber = newMaxNumber;
                    position.DepartmentId = newDepartament.Id;
                    db.SaveChanges();
                    result = $"Позиция {position.Name} изменена";
                }
            }
            return result;
        }
        // Редактировать сотрудника
        public static string EditUser(User oldUser, string newName, string newSurName, string newPhone, Position newPosition)
        {
            string result = "Такого сотрудника не существует";
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = db.Users.FirstOrDefault(x => x.Id == oldUser.Id);
                if (user != null)
                {
                    user.Name = newName;
                    user.SurName = newSurName;
                    user.Phone = newPhone;
                    user.PositionId = newPosition.Id;
                    db.SaveChanges();
                    result = $"Сотрудник {user.Name} изменен";
                }
            }
            return result;
        }
        //получение позиции по id позиции
        public static Position GetPositionById(int id)
        {
            using(ApplicationContext db = new ApplicationContext())

            {
                return db.Positions.FirstOrDefault(x => x.Id == id);
            }
        }
        //получение отдела по id отдела
        public static Department GetDepartmentById(int id)
        {
            using (ApplicationContext db = new ApplicationContext())

            {
                return db.Departments.FirstOrDefault(x => x.Id == id);
            }
        }
        //получение всех пользователей по id позиции
        public static List<User> GetAllUsersByPositionId(int id)
        {
            using (ApplicationContext db = new ApplicationContext())

            {
                return (from user in GetAllUsers() where user.PositionId == id select user).ToList();
            }
        }

        //получение всех позиций по id отдела
        public static List<Position> GetAllPositionsByDepartmentId(int id)
        {
            using (ApplicationContext db = new ApplicationContext())

            {
                return (from position in GetAllPositions() where position.DepartmentId == id select position).ToList();
            }
        }
    }
}
