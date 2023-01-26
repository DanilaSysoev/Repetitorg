﻿using Microsoft.Data.Sqlite;
using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.Storages;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace Storage.SQLite
{
    public class SqliteDatabase : IDatabase
    {
        private ClientSqliteStorage clientStorage;
        private StudentSqliteStorage studentStorage;
        private OrderSqliteStorage orderStorage;
        private LessonSqliteStorage lessonStorage;
        private PaymentDocumentTypeSqliteStorage paymentDocumentTypeStorage;
        private PaymentSqliteStorage paymentStorage;
        private ProjectSqliteStorage projectStorage;
        private TaskSqliteStorage taskStorage;

        public void Initialize(string pathToDbFile)
        {
            initialized = true;
            this.pathToDbFile = pathToDbFile;
            CreateStorages();
            CreateDatabaseIfNotExist();
            LoadData();
            SetupStorages();
        }

        private void CreateStorages()
        {            
            clientStorage = new ClientSqliteStorage();
            studentStorage = new StudentSqliteStorage();
            orderStorage = new OrderSqliteStorage();
            lessonStorage = new LessonSqliteStorage();
            paymentDocumentTypeStorage = new PaymentDocumentTypeSqliteStorage();
            paymentStorage = new PaymentSqliteStorage();
            projectStorage = new ProjectSqliteStorage();
            taskStorage = new TaskSqliteStorage();

            storagesByType = new Dictionary<Type, ILoadable>();
            storagesByType.Add(typeof(Client), clientStorage);
            storagesByType.Add(typeof(Student), studentStorage);
            storagesByType.Add(typeof(Order), orderStorage);
            storagesByType.Add(typeof(Lesson), lessonStorage);
            storagesByType.Add(typeof(PaymentDocumentType), paymentDocumentTypeStorage);
            storagesByType.Add(typeof(Payment), paymentStorage);
            storagesByType.Add(typeof(Project), projectStorage);
            storagesByType.Add(typeof(Task), taskStorage);
        }

        private void SetupStorages()
        {
            Client.SetupStorage(clientStorage);
            Student.SetupStorage(studentStorage);
            Order.SetupStorage(orderStorage);
            Lesson.SetupStorage(lessonStorage);
            PaymentDocumentType.SetupStorage(paymentDocumentTypeStorage);
            Payment.SetupStorage(paymentStorage);
            Project.SetupStorage(projectStorage);
            Task.SetupStorage(taskStorage);
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

        private Dictionary<Type, ILoadable> storagesByType;
        private string pathToDbFile;
        private bool initialized;

        private void CreateDatabaseIfNotExist()
        {
            if (!File.Exists(pathToDbFile))
            {
                CreatePhoneNumberTable();
                CreatePersonDataTable();
                CreateClientsTable();
                CreateStudentTable();
                CreateOrderTable();
                CreateLessonTable();
                CreatePaymentDocumentTypeTable();
                CreatePaymentTable();
                CreateProjectTable();
                CreateTaskTable();
            }
        }
        private void CreatePhoneNumberTable()
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
                        "UNIQUE (phoneCountryCode, phoneOperatorCode, phoneNumber));";
                command.ExecuteNonQuery();
            }
        }
        private void CreatePersonDataTable()
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDbFile))
            )
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "CREATE TABLE IF NOT EXISTS " +
                    "PersonData(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "firstName TEXT NOT NULL, " +
                        "lastName TEXT, " +
                        "patronymic TEXT, " +
                        "phoneNumberId INTEGER UNIQUE NOT NULL, " +
                        "FOREIGN KEY (phoneNumberId) REFERENCES PhoneNumber (id) ON DELETE SET NULL)";
                command.ExecuteNonQuery();
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
                    "Client(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "balabceInCopex INTEGER DEFAULT 0, " +
                        "personDataId INTEGER UNIQUE NOT NULL, " +
                        "FOREIGN KEY (personDataId) REFERENCES PersonData (id) ON DELETE RESTRICT)";
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
                        "personDataId INTEGER UNIQUE NOT NULL, " +
                        "clientId INTEGER NOT NULL, " +
                        "FOREIGN KEY (personDataId) REFERENCES PersonData (id) ON DELETE RESTRICT," +
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
        private void CreatePaymentDocumentTypeTable()
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
                        "documentType TEXT NOT NULL);" +                    
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
                    "Payment(" +
                        "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "date TEXT NOT NULL, " +
                        "summInCopex INTEGER NOT NULL CHECK (summInCopex > 0), " +
                        "documentTypeId INTEGER NOT NULL, " +
                        "documentId TEXT NOT NULL, " +
                        "clientId INTEGER NOT NULL, " +
                        "FOREIGN KEY(documentTypeId) REFERENCES PaymentDocument (id) ON DELETE RESTRICT, " +
                        "FOREIGN KEY(clientId) REFERENCES Client (id) ON DELETE RESTRICT);";
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

        private void LoadData()
        {
            clientStorage.Load(pathToDbFile);
            studentStorage.Load(pathToDbFile);
            orderStorage.Load(pathToDbFile);
            lessonStorage.Load(pathToDbFile);
            paymentDocumentTypeStorage.Load(pathToDbFile);
            paymentStorage.Load(pathToDbFile);
            projectStorage.Load(pathToDbFile);
            taskStorage.Load(pathToDbFile);
        }
    }
}
