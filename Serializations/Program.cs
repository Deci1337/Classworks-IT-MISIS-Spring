//https://github.com/Deci1337/BIVT-25-Lab-10/blob/main/Program.cs
﻿using System.Text.Json.Serialization;

namespace Lab9
{
    public class Program
    {
        public static void Main()
        {
            Serialization serialization = new Serialization();
            //serialization.Serialize();
            //serialization.Deserialize();
        }
    }

    public class Student
    {
        public static int _counter = 0;
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public Subject[] Subjects { get; private set; }
        public Student(int id, string name, string surname, Subject[] subjects)
        {
            Id = _counter++;
            Name = name;
            Surname = surname;
            Subjects = subjects;

        }

        public void AddMarks(string subjectName, int[] marks)
        {
            for (int i = 0; i < Subjects.Length; i++)
            {
                if (Subjects[i].Name == subjectName)
                {
                    Subjects[i].AddMark(marks[i]);
                }
            }
        }


        public void AddMarks(Subject subject, int[] marks)
        {
            for (int i = 0; i < Subjects.Length; i++)
            {
                if (Subjects[i].Name == subject.Name)
                {
                    Subjects[i].AddMark(marks[i]);
                }
            }
        }
    }
    public class Subject
    {
        private List<int> _marks;
        public string Name { get; private set; }
        public int[] Marks => _marks.ToArray();
        public int FinalMark
        {
            get
            {
                if (Marks != null && Marks.Length > 0)
                {
                    return (int)Math.Round(_marks.Average());
                }
                return 0;
            }
        }
        [JsonConstructor]
        public Subject(string name)
        {
            Name = name;
            _marks = new List<int>();
            _marks.AddRange(Marks);
        }
        public void AddMark(int mark)
        {
            _marks.Add(mark);
        }
        public void AddMarks(int[] mark)
        {
            _marks.AddRange(mark);
        }
    }
}