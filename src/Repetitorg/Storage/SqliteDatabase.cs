using Microsoft.Data.Sqlite;
using Repetitorg.Core;
using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace Storage.SQLite
{
    public class SqliteDatabase : IDatabase
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
            if (!File.Exists(pathToDbFile))
            {
                CreateClientsTable();
                CreateStudentTable();
                CreateOrderTable();
                CreateLessonTable();
                CreatePaymentTable();
                CreateProjectTable();
                CreateTaskTable();
            }
        }
        private void CreateClientsTable()
        {
            using (var connection = 
                new SqliteConnection(string.Format("Data Source={0}", pathToDbFile))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS " +
                    "PhoneNumber(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "phoneCountryCode INTEGER NOT NULL, " +
                        "phoneOperatorCode INTEGER NOT NULL, " +
                        "phoneNumber INTEGER NOT NULL, " +
                        "UNIQUE (phoneCountryCode, phoneOperatorCode, phoneNumber));\n" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Client(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "balabceInCopex INTEGER DEFAULT 0, " +
                        "firstName TEXT NOT NULL, " +
                        "lastName TEXT, " +
                        "patronymic TEXT, " +
                        "phoneNumberId INTEGER UNIQUE, " +
                        "FOREIGN KEY (phoneNumberId) REFERENCES PhoneNumber (id) ON DELETE SET NULL)";
                command.ExecuteNonQuery();
            }
        }
        private void CreateStudentTable()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDbFile))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS " +
                    "Student(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "firstName TEXT NOT NULL, " +
                        "lastName TEXT, " +
                        "patronymic TEXT, " +
                        "phoneNumberId INTEGER UNIQUE, " +
                        "clientId INTEGER, " +
                        "FOREIGN KEY (phoneNumberId) REFERENCES PhoneNumber (id) ON DELETE SET NULL," +
                        "FOREIGN KEY (clientId) REFERENCES Client (id) ON DELETE RESTRICT)";
                command.ExecuteNonQuery();
            }
        }
        private void CreateOrderTable()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDbFile))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS " +
                    "StudyOrder(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "name TEXT NOT NULL);\n" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "StudentOrderCosts(" +
                        "studentId INTEGER NOT NULL, " +
                        "orderId INTEGER NOT NULL, " +
                        "costInCopexPerHour INTEGER CHECK (costInCopexPerHour >= 0), " +
                        "PRIMARY KEY (studentId, orderId), " +
                        "FOREIGN KEY(studentId) REFERENCES Student (id) ON DELETE RESTRICT, " +
                        "FOREIGN KEY(orderId) REFERENCES StudyOrder (id) ON DELETE RESTRICT)";
                command.ExecuteNonQuery();
            }
        }
        private void CreateLessonTable()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDbFile))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS " +
                    "LessonStatus(" +
                        "id INTEGER PRIMARY KEY, " +
                        "status TEXT NOT NULL);\n" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Lesson(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "dateTime TEXT NOT NULL, " +
                        "lengthInMinutes INTEGER NOT NULL CHECK (lengthInMinutes > 0), " +
                        "orderId INTEGER NOT NULL, " +
                        "statusId INTEGER NOT NULL, " +
                        "movedOnId INTEGER, " +
                        "movedFromId INTEGER, " +
                        "FOREIGN KEY(orderId) REFERENCES StudyOrder (id) ON DELETE RESTRICT, " +
                        "FOREIGN KEY(statusId) REFERENCES LessonStatus (id) ON DELETE RESTRICT, " +
                        "FOREIGN KEY(movedOnId) REFERENCES Lesson (id) ON DELETE RESTRICT, " +
                        "FOREIGN KEY(movedFromId) REFERENCES Lesson (id) ON DELETE RESTRICT);\n";
                var statuses = Enum.GetNames(typeof(LessonStatus));
                for (int i = 0; i < statuses.Length; ++i)
                {
                    command.CommandText +=
                        string.Format(
                            "INSERT INTO LessonStatus (id, status) VALUES ({0}, '{1}');\n",
                            i,
                            statuses[i]
                        );
                }
                command.ExecuteNonQuery();
            }
        }
        private void CreatePaymentTable()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDbFile))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS " +
                    "PaymentDocument(" +
                        "id INTEGER PRIMARY KEY, " +
                        "documentType TEXT NOT NULL);\n" +
                    "CREATE TABLE IF NOT EXISTS " +
                    "Payment(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "date TEXT NOT NULL, " +
                        "summInCopex INTEGER NOT NULL CHECK (summInCopex > 0), " +
                        "documentTypeId INTEGER NOT NULL, " +
                        "documentNumber INTEGER NOT NULL, " +
                        "clientId INTEGER NOT NULL, " +
                        "FOREIGN KEY(documentTypeId) REFERENCES PaymentDocument (id) ON DELETE RESTRICT, " +
                        "FOREIGN KEY(clientId) REFERENCES Client (id) ON DELETE RESTRICT);\n";
                var docTypes = Enum.GetNames(typeof(PaymentDocumentType));
                for (int i = 0; i < docTypes.Length; ++i)
                {
                    command.CommandText +=
                        string.Format(
                            "INSERT INTO PaymentDocument (id, documentType) VALUES ({0}, '{1}');\n",
                            i,
                            docTypes[i]
                        );
                }
                command.ExecuteNonQuery();
            }
        }
        private void CreateProjectTable()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDbFile))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS " +
                    "Project(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "name TEXT NOT NULL, " +
                        "completed INTEGER NOT NULL);\n";                    
                command.ExecuteNonQuery();
            }
        }
        private void CreateTaskTable()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDbFile))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS " +
                    "Task(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "name TEXT NOT NULL, " +
                        "date TEXT NOT NULL, " +
                        "completed INTEGER NOT NULL, " +
                        "projectId INTEGER NOT NULL, " +
                        "FOREIGN KEY (projectId) REFERENCES Project (id) ON DELETE CASCADE);\n";
                command.ExecuteNonQuery();
            }
        }

        private void LoadDataToStorages()
        {
            throw new NotImplementedException();
        }
    }
}
