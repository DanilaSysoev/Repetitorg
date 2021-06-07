﻿using System;
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

        public static Student CreateNew(string fullName)
        {
            new NullChecker().Add(fullName, "Name of the student can't be null").Check();

            Student student = new Student(fullName);
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
                return "";
            }
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
