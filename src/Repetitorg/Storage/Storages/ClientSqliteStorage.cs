using Microsoft.Data.Sqlite;
using Repetitorg.Core;
using Repetitorg.Core.Base;
using Storage.SQLite.DatabaseRawEntities;
using Storage.SQLite.Storages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storage.SQLite.Storages
{
    class ClientSqliteStorage : SqliteLoadable, IStorage<Client>
    {
        private Dictionary<long, Client> clients;
        private string pathToDb;

        public ClientSqliteStorage()
        {
            clients = new Dictionary<long, Client>();
        }

        public long Add(Client entity)
        {
            return -1;
        }

        public IList<Client> Filter(Predicate<Client> predicate)
        {
            return FilterByPredicate(clients.Values, predicate);
        }

        public IReadOnlyList<Client> GetAll()
        {
            return new List<Client>(clients.Values);
        }

        public override void Load(string pathToDb)
        {
            using (var connection =
                new SqliteConnection(string.Format("Data Source={0}", pathToDb))
            )
            {
                connection.Open();
                var phoneNumberEntities = ReadEntitiesToDict(
                    "PhoneNumber", connection, BuildPhoneNumberEntity
                );
                var personDataEntities = ReadEntitiesToDict(
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

            this.pathToDb = pathToDb;
        }

        private void CreateAndLinkObjects(
            Dictionary<long, PhoneNumberEntity> phoneNumberEntities, 
            Dictionary<long, PersonDataEntity> personDataEntities,
            List<ClientEntity> clientEntities
        )
        {
            clients = new Dictionary<long, Client>();
            foreach(var clientEntity in clientEntities)
            {
                var personData = personDataEntities[clientEntity.PersonDataId];
                var phoneNumber = phoneNumberEntities[
                    personDataEntities[clientEntity.PersonDataId].PhoneNumberId
                ];
                clients.Add(
                    clientEntity.Id,
                    Client.CreateLoaded(
                        clientEntity.Id,
                        clientEntity.BalanceInKopeks,
                        new FullName
                        {
                            FirstName = personData.FirstName,
                            LastName = personData.LastName,
                            Patronymic = personData.Patronymic,
                        },
                        new PhoneNumber
                        {
                            CountryCode = phoneNumber.CountryCode,
                            OperatorCode = phoneNumber.OperatorCode,
                            Number = phoneNumber.Number
                        }
                    )
                );
            }
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
                BalanceInKopeks = personDataReader.GetInt64(1),
                PersonDataId = personDataReader.GetInt64(2),
            };
        }

        public void Remove(Client entity)
        {
            RemoveEntity(entity, "Client", pathToDb);
        }

        public void Update(Client entity)
        {
            throw new NotImplementedException();
        }
    }
}
