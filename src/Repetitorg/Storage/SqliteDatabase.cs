using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite
{
    class SqliteDatabase : IDatabase
    {
        public void Initialize(string pathToDbFile)
        {
            initialized = true;
            this.pathToDbFile = pathToDbFile;
            storagesByType = new Dictionary<Type, object>();
            storagesByType.Add(typeof(Client),  new ClientSqliteStorage());
            storagesByType.Add(typeof(Lesson),  new LessonSqliteStorage());
            storagesByType.Add(typeof(Order),   new OrderSqliteStorage());
            storagesByType.Add(typeof(Payment), new PaymentSqliteStorage());
            storagesByType.Add(typeof(Project), new ProjectSqliteStorage());
            storagesByType.Add(typeof(Student), new StudentSqliteStorage());
            storagesByType.Add(typeof(Task),    new TaskSqliteStorage());

            CreateDatabaseIfNotExist();
            LoadDataToStorages();
        }
        public IStorage<T> Entities<T>()
        {
            if(!initialized)
                throw new InvalidOperationException(
                    "Database is not initialized"
                );
            var key = typeof(T);
            if (storagesByType.ContainsKey(key))
                return (IStorage<T>)storagesByType[key];
            throw new InvalidOperationException("Unknown entity type");
        }

        private Dictionary<Type, Object> storagesByType;
        private string pathToDbFile;
        private bool initialized;

        private void CreateDatabaseIfNotExist()
        {
            CreateClientsTable();
            CreateLessonTable();
            CreateOrderTable();
            CreatePaymentTable();
            CreateProjectTable();
            CreateStudentTable();
            CreateTaskTable();
        }
        private void CreateClientsTable()
        {
            throw new NotImplementedException();
        }
        private void CreateLessonTable()
        {
            throw new NotImplementedException();
        }
        private void CreateOrderTable()
        {
            throw new NotImplementedException();
        }
        private void CreatePaymentTable()
        {
            throw new NotImplementedException();
        }
        private void CreateProjectTable()
        {
            throw new NotImplementedException();
        }
        private void CreateStudentTable()
        {
            throw new NotImplementedException();
        }
        private void CreateTaskTable()
        {
            throw new NotImplementedException();
        }

        private void LoadDataToStorages()
        {
            throw new NotImplementedException();
        }
    }
}
