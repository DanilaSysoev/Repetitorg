using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core.Base
{
    public class StorageWrapper<T> : INote
    {
        public event Action NotesUpdated;

        public string Note { get; private set; }
        public StorageWrapper()
        {
            Note = "";
        }
        public void UpdateNote(string newNote)
        {
            CheckConditionsForUpdateNotes(newNote);
            Note = newNote;
            NotesUpdated?.Invoke();
        }
        private void CheckConditionsForUpdateNotes(string note)
        {
            new Checker()
                .AddNull(note, "Note can't be null")
                .Check();
        }

        public static int Count
        {
            get
            {
                return storage.GetAll().Count;
            }
        }

        public static void SetupStorage(IStorage<T> storage)
        {
            StorageWrapper<T>.storage = storage;
        }
        public static IReadOnlyList<T> GetAll()
        {
            return storage.GetAll();
        }
        public static void Remove(T entity)
        {
            new Checker().AddNull(entity, "Entity can't be null").Check();

            storage.Remove(entity);
        }
        public static IStorage<T> Storage
        {
            get
            {
                return storage; 
            }
        }



        protected static IStorage<T> storage;
    }
}
