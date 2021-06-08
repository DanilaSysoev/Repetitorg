using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Student : Person
    {        
        public static int StudentsCount
        {
            get
            {
                return students.Count;
            }
        }

        public static Student CreateNew(string fullName, string phoneNumber = "")
        {
            new NullChecker().
                Add(fullName, "Name of the student can't be null").
                Add(phoneNumber, "Phone number of the student can't be null").
                Check();

            Student student = new Student(fullName, phoneNumber);

            if (students.Contains(student))
                throw new InvalidOperationException(
                    "Creation student with same names and phone numbers is impossible"
                );

            students.Add(student);

            return student;
        }
        public static void Clear()
        {
            students.Clear();
        }

        private Student(string fullName, string phoneNumber)
            : base(fullName, phoneNumber)
        {
        }

        static Student()
        {
            students = new List<Student>();
        }

        private static List<Student> students;
    }

}
