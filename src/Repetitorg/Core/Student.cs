using System;
using System.Collections.Generic;
using System.Text;

namespace Repetitorg.Core
{
    public class Student
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
            students.Add(student);

            return student;
        }
        public static void Clear()
        {
            students.Clear();
        }


        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
        }

        private Student(string fullName, string phoneNumber)
        {
            this.fullName = fullName;
            this.phoneNumber = phoneNumber;
        }

        static Student()
        {
            students = new List<Student>();
        }

        private static List<Student> students;

        private string fullName;
        private string phoneNumber;
    }

}
