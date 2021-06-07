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

        public static void CreateNew(string fullName)
        {
            students.Add(new Student(fullName));
        }
        public static void Clear()
        {
            students.Clear();
        }


        private Student(string fullName)
        {
            this.fullName = fullName;
        }

        static Student()
        {
            students = new List<Student>();
        }

        private static List<Student> students;

        private string fullName;
    }

}
