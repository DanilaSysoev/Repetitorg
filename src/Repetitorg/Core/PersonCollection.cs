﻿using Repetitorg.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repetitorg.Core
{
    public class PersonsCollection<T> : Person where T : Person
    {
        internal PersonsCollection(string fullName, string phoneNumber)
            : base(fullName, phoneNumber)
        { }


        protected static List<T> entities;

        public static int Count
        {
            get
            {
                return entities.Count;
            }
        }
        public static T CreateNew(string fullName, string phoneNumber = "")
        {
            var nc = new NullChecker();

            new NullChecker().
                Add(fullName, "Can not create client with NULL name").
                Add(phoneNumber, "Can not create client with NULL phone number").
                Check();

            object[] objects = { fullName, phoneNumber };
            var entity = typeof(T).GetConstructors(
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance
            )[0].Invoke(objects) as T;

            if (entities.Contains(entity))
                throw new InvalidOperationException(
                    "Creation clients with same names and phone numbers is impossible"
                );

            entities.Add(entity);
            return entity;
        }
        public static IList<T> GetAll()
        {
            return new List<T>(entities);
        }
        public static IList<T> FilterByName(string condition)
        {
            return
                (from entity in entities
                 where entity.FullName.ToLower().Contains(condition.ToLower())
                 select entity).ToList();
        }
        public static void Clear()
        {
            entities.Clear();
        }
        static PersonsCollection()
        {
            entities = new List<T>();
        }
    }
}
