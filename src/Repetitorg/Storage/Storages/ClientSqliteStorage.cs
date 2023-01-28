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
            var phoneNumber = entity.PersonData.PhoneNumber;
            var personData = entity.PersonData;
            long personDataId = -1;
            long? phoneNumberId = null;
            if (phoneNumber != null)
                phoneNumberId = InsertPhoneNumber(phoneNumber);
            personDataId = InsertPersonData(personData, phoneNumberId);

            return InsertClient(entity, personDataId);
        }

        private long InsertPhoneNumber(PhoneNumber phoneNumber)
        {
            return InsertInto(
                       "PhoneNumber",
                       new string[] {
                           "countryCode",
                           "operatorCode",
                           "number"
                       },
                       new object[]
                       {
                           phoneNumber.CountryCode,
                           phoneNumber.OperatorCode,
                           phoneNumber.Number
                       },
                       pathToDb
                   );
        }
        private long InsertPersonData(
            Person personData, long? phoneNumberId
        )
        {
            return InsertInto(
                       "PersonData",
                       new string[] {
                           "firstName",
                           "lastName",
                           "patronymic",
                           "phoneNumberId"
                       },
                       new object[]
                       {
                           personData.FullName.FirstName,
                           personData.FullName.LastName,
                           personData.FullName.Patronymic,
                           phoneNumberId
                       },
                       pathToDb
                   );
        }
        private long InsertClient(Client client, long personDataId)
        {
            return InsertInto(
                       "Client",
                       new string[] {
                           "balabceInKopeks",
                           "personDataId"
                       },
                       new object[]
                       {
                           client.BalanceInKopeks,
                           personDataId
                       },
                       pathToDb
                   );
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
                var phoneNumber = personData.PhoneNumberId.HasValue ?
                    phoneNumberEntities[
                        personData.PhoneNumberId.Value
                    ] : null;

                clients.Add(
                    clientEntity.Id,
                    Client.CreateLoaded(
                        clientEntity.Id,
                        clientEntity.BalanceInKopeks,
                        new FullName (
                            personData.FirstName,
                            personData.LastName,
                            personData.Patronymic
                        ),
                        phoneNumber != null ?
                        new PhoneNumber (
                            phoneNumber.CountryCode,
                            phoneNumber.OperatorCode,
                            phoneNumber.Number
                        ) : null
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
            var personData = new PersonDataEntity
                {
                    Id = personDataReader.GetInt64(0),
                    FirstName = personDataReader.GetString(1),
                    LastName = personDataReader.GetString(2),
                    Patronymic = personDataReader.GetString(3),
                    PhoneNumberId = null
                };
            if (!personDataReader.IsDBNull(4))
                personData.PhoneNumberId = personDataReader.GetInt64(4);
            return personData;
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
            ClientEntity oldClient = 
                ReadEntity("Client", pathToDb, BuildClientEntity, entity.Id);
            PersonDataEntity oldPersonData = 
                ReadEntity(
                    "PersonDAta", 
                    pathToDb, 
                    BuildPersonDataEntity, 
                    oldClient.PersonDataId
                );
            PhoneNumberEntity oldPhoneNumber = null;
            if (oldPersonData.PhoneNumberId.HasValue)
            {
                oldPhoneNumber =
                    ReadEntity(
                        "PhoneNumber",
                        pathToDb,
                        BuildPhoneNumberEntity,
                        oldPersonData.PhoneNumberId.Value
                    );
            }
        }
    }
}
