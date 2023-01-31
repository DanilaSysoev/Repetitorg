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
        private NoteBufferSqliteStorage noteStorage;

        public ClientSqliteStorage(NoteBufferSqliteStorage noteStorage)
        {
            clients = new Dictionary<long, Client>();
            this.noteStorage = noteStorage;
        }

        public long Add(Client entity)
        {
            var phoneNumber = entity.PersonData.PhoneNumber;
            var personData = entity.PersonData;
            var note = entity.Note;
            long? phoneNumberId = null;
            long? noteId = null;
            if (phoneNumber != null)
                phoneNumberId = InsertPhoneNumber(phoneNumber);
            if (note != "")
                noteId = InsertNote(note, pathToDb);
            long personDataId = InsertPersonData(personData, phoneNumberId, noteId);

            long clientId = InsertClient(entity, personDataId);
            clients.Add(clientId, entity);
            return clientId;
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
            Person personData, long? phoneNumberId, long? noteId
        )
        {
            return InsertInto(
                       "PersonData",
                       new string[] {
                           "firstName",
                           "lastName",
                           "patronymic",
                           "phoneNumberId",
                           "noteId"
                       },
                       new object[]
                       {
                           personData.FullName.FirstName,
                           personData.FullName.LastName,
                           personData.FullName.Patronymic,
                           phoneNumberId,
                           noteId
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
                var note = noteStorage.GetNote(clientEntity.NoteId);

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
                        ) : null,
                        note != null ?
                        note.Text : ""
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
                CountryCode = phoneNumberReader.GetString(1),
                OperatorCode = phoneNumberReader.GetString(2),
                Number = phoneNumberReader.GetString(3)
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
            var entity = new ClientEntity
            {
                Id = personDataReader.GetInt64(0),
                BalanceInKopeks = personDataReader.GetInt64(1),
                PersonDataId = personDataReader.GetInt64(2)
            };
            if(!personDataReader.IsDBNull(3))
                entity.NoteId = personDataReader.GetInt64(3); 
            return entity;
        }

        public void Remove(Client entity)
        {
            ClientEntity clientEntity;
            PersonDataEntity personData;
            PhoneNumberEntity phoneNumber;
            NoteEntity note;
            ReadClientLinkedEntities(
                entity,
                out clientEntity,
                out personData,
                out phoneNumber,
                out note
            );

            RemoveEntity(clientEntity.Id, "Client", pathToDb);
            RemoveEntity(personData.Id, "PersonData", pathToDb);
            if(phoneNumber != null)
                RemoveEntity(phoneNumber.Id, "PhoneNumber", pathToDb);
            if(note != null)
                RemoveEntity(note.Id, "Note", pathToDb);

            clients.Remove(entity.Id);
        }

        public void Update(Client entity)
        {
            ClientEntity oldClient;
            PersonDataEntity oldPersonData;
            PhoneNumberEntity oldPhoneNumber;
            NoteEntity oldNote;
            ReadClientLinkedEntities(
                entity, 
                out oldClient,
                out oldPersonData, 
                out oldPhoneNumber,
                out oldNote
            );

            UpdateClientEntity(entity, oldClient);
            UpdatePersonData(entity.PersonData, oldPersonData);
            UpdatePhoneNumber(oldPersonData, entity.PersonData.PhoneNumber, oldPhoneNumber);
            UpdateNote(entity, entity.Note, oldNote);
            clients[entity.Id] = entity;
        }

        private void ReadClientLinkedEntities(
            Client client,
            out ClientEntity clientEntity,
            out PersonDataEntity personData,
            out PhoneNumberEntity phoneNumber,
            out NoteEntity note
        )
        {
            clientEntity = ReadEntity("Client", pathToDb, BuildClientEntity, client.Id);
            personData = ReadEntity(
                "PersonData",
                pathToDb,
                BuildPersonDataEntity,
                clientEntity.PersonDataId
            );
            phoneNumber = null;
            if (personData.PhoneNumberId.HasValue)
            {
                phoneNumber =
                    ReadEntity(
                        "PhoneNumber",
                        pathToDb,
                        BuildPhoneNumberEntity,
                        personData.PhoneNumberId.Value
                    );
            }
            note = noteStorage.GetNote(clientEntity.NoteId);
        }

        private void UpdateNote(
            Client client, string note, NoteEntity oldNote
        )
        {
            if (oldNote == null && note == "")
                return;
            if (oldNote == null)
                InsertNewNoteAndUpdateNoteId(client, note);
            else if(note == "")
                RemoveEntity(oldNote.Id, "Note", pathToDb);
            else
                UpdateNoteData(note, oldNote);
        }
        private void UpdateNoteData(string note, NoteEntity oldNote)
        {
            if (note.Equals(oldNote.Text))
                return;
            UpdateSet(
                oldNote.Id,
                "Note",
                new string[] { "noteText" },
                new object[] { note },
                pathToDb
            );
        }
        private void InsertNewNoteAndUpdateNoteId(Client client, string note)
        {
            long noteId = InsertInto(
                "Note",
                new string[] { "noteText" },
                new object[] { note },
                pathToDb
            );
            UpdateSet(
                client.Id,
                "Client",
                new string[] { "noteId" },
                new object[] { noteId },
                pathToDb
            );
        }

        private void UpdatePhoneNumber(
            PersonDataEntity personData, 
            PhoneNumber phoneNumber, 
            PhoneNumberEntity oldPhoneNumber
        )
        {
            if (oldPhoneNumber == null && phoneNumber == null)
                return;
            if (oldPhoneNumber == null)
                InsertNewPhoneNumberAndUpdatePhoneNumberId(
                    personData, phoneNumber
                );
            else if (phoneNumber == null)
                RemoveEntity(oldPhoneNumber.Id, "PhoneNumber", pathToDb);
            else
                UpdatePhoneNumberData(phoneNumber, oldPhoneNumber);
        }
        private void UpdatePhoneNumberData(
            PhoneNumber phoneNumber, 
            PhoneNumberEntity oldPhoneNumber
        )
        {
            if (phoneNumber.CountryCode == oldPhoneNumber.CountryCode &&
                phoneNumber.OperatorCode == oldPhoneNumber.OperatorCode &&
                phoneNumber.Number == oldPhoneNumber.Number)
                return;
            UpdateSet(
                oldPhoneNumber.Id,
                "PhoneNumber",
                new string[] { "countryCode", "operatorCode", "number" },
                new object[] {
                    phoneNumber.CountryCode,
                    phoneNumber.OperatorCode,
                    phoneNumber.Number
                },
                pathToDb
            );
        }
        private void InsertNewPhoneNumberAndUpdatePhoneNumberId(
            PersonDataEntity personData, PhoneNumber phoneNumber
        )
        {
            long phoneId = InsertInto(
                "PhoneNumber",
                new string[] { "countryCode", "operatorCode", "number" },
                new object[] {
                    phoneNumber.CountryCode,
                    phoneNumber.OperatorCode,
                    phoneNumber.Number
                },
                pathToDb
            );
            UpdateSet(
                personData.Id,
                "PersonData",
                new string[] { "phoneNumberId" },
                new object[] { phoneId },
                pathToDb
            );
        }

        private void UpdatePersonData(
            Person personData, PersonDataEntity oldPersonData
        )
        {
            if (personData.FullName.FirstName.Equals(oldPersonData.FirstName) &&
                personData.FullName.LastName.Equals(oldPersonData.LastName) &&
                personData.FullName.Patronymic.Equals(oldPersonData.Patronymic))
                return;
            UpdateSet(
                oldPersonData.Id,
                "PersonData",
                new string[] { "firstName", "lastName", "patronymic" },
                new object[] {
                    personData.FullName.FirstName,
                    personData.FullName.LastName,
                    personData.FullName.Patronymic
                },
                pathToDb
            );
        }

        private void UpdateClientEntity(
            Client client, ClientEntity oldClient
        )
        {
            if (oldClient.BalanceInKopeks == client.BalanceInKopeks)
                return;
            UpdateSet(
                oldClient.Id,
                "Client",
                new string[] { "balabceInKopeks" },
                new object[] {
                    client.BalanceInKopeks
                },
                pathToDb
            );
        }
    }
}
