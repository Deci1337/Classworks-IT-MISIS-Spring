using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab10
{
    internal class Serialization
    {
        string _desktopPath;
        string _path;
        List<Student> _students;
        public Serialization()
        {
            _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _path = Path.Combine(_desktopPath, "example28.json");


            _students = new List<Student>(3)
            {
                new Student("Petya", "Ivanov"),
                new Student("Fedor", "Lazarev"),
                new Student("Tatyana", "Smirnova")
            };

            _students[0].AddMarks("Math", new int[] { 1, 1, 2, 3, 5, 5, 3, 4, 5 });
            _students[1].AddMarks("Math", new int[] { 1, 1, 2, 3, 5, 5, 3, 4, 5 });
            _students[2].AddMarks("Math", new int[] { 1, 1, 2, 3, 5, 5, 3, 4, 5 });
            _students[0].AddMarks("Math", new int[] { 1, 1, 2, 3, 5, 5, 3, 4, 5 });
            _students[1].AddMarks("Math", new int[] { 1, 1, 2, 3, 5, 5, 3, 4, 5 });
        }
        public void Serialize(Student[] students)
        {
            var jsonString = JsonSerializer.Serialize(_students);

            File.WriteAllText(_path, jsonString);

            _students = null;

        }
        public void Deserialize()
        {
            Console.WriteLine();
            var jsonString = File.ReadAllText(_path);
            Console.WriteLine(jsonString);

            var obj = JsonSerializer.Deserialize<Student[]>(jsonString);
        }
    }
}