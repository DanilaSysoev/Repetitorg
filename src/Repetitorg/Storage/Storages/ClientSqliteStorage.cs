using Microsoft.Data.Sqlite;
using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.DatabaseRawEntities;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.SQLite.Storages
{
    class ClientSqliteStorage : SqliteLoadable, IStorage<Client>
    {
        public long Add(Client entity)
        {
            throw new NotImplementedException();
        }

        public IList<Client> Filter(Predicate<Client> predicate)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Client> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Load(string pathToDb)
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDb))
            )
            {
                connection.Open();
                var phoneNumberEntities = ReadEntities(
                    "PhoneNumber", connection, BuildPhoneNumberEntity
                );
                var personDataEntities = ReadEntities(
                    "PersonData", connection, BuildPersonDataEntity
                );
                var clientEntities = ReadEntities(
                    "Client", connection, BuildClientEntity
                );

                CreateAndLinkObjects(
                    phoneNumberEntities,
                    personDataEntities,
                    clientEntities
                );

                connection.Close();
            }
        }

        private void CreateAndLinkObjects(
            List<PhoneNumberEntity> phoneNumberEntities, 
            List<PersonDataEntity> personDataEntities,
            List<ClientEntity> clientEntities
        )
        {
        }

        private static PhoneNumberEntity
            BuildPhoneNumberEntity(SqliteDataReader phoneNumberReader)
        {
            return new PhoneNumberEntity
            {
                Id = phoneNumberReader.GetInt64(0),
                CountryCode = phoneNumberReader.GetInt32(1),
                OperatorCode = phoneNumberReader.GetInt32(2),
                Number = phoneNumberReader.GetInt64(3)
            };
        }
        private static PersonDataEntity 
            BuildPersonDataEntity(SqliteDataReader personDataReader)
        {
            return new PersonDataEntity
            {
                Id = personDataReader.GetInt64(0),
                FirstName = personDataReader.GetString(1),
                LastName = personDataReader.GetString(2),
                Patronymic = personDataReader.GetString(3),
                PhoneNumberId = personDataReader.GetInt64(4)
            };
        }
        private static ClientEntity
            BuildClientEntity(SqliteDataReader personDataReader)
        {
            return new ClientEntity
            {
                Id = personDataReader.GetInt64(0),
                BalanceInCopex = personDataReader.GetInt64(1),
                PersonDataId = personDataReader.GetInt64(2),
            };
        }

        public void Remove(Client entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Client entity)
        {
            throw new NotImplementedException();
        }
    }
}
