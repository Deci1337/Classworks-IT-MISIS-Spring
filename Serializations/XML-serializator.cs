using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Serialization;
using System.Xml.Serialization;
using System.Net.Http.Headers;

namespace Serialize
{
    public class MyXMLSerializator : MySerializator
    {
        public new void Serialize()
        {
            // нужно знать точный тип заранее
            var ser = new XmlSerializer(typeof(DTOStudent[]));

            _path = Path.Combine(_desktopPath, "example.xml");

            using (var fs = new StreamWriter(_path))
            {
                // Конвертируем студентов в DTO перед записью в XML
                var dtoObjects = new List<DTOStudent>(_students.Count);

                foreach (var student in _students)
                {
                    dtoObjects.Add(new DTOStudent(student));
                }

                ser.Serialize(fs, dtoObjects.ToArray());
            }

            _students = null;
        }

        public new void Deserialize()
        {
            var ser = new XmlSerializer(typeof(DTOStudent[]));

            using (var fs = new StreamReader(_path))
            {
                // Десериализуем XML обратно в массив DTO, затем восстанавливаем в студентов
                var objects = ser.Deserialize(fs) as DTOStudent[];

                _students = new List<Student>();

                foreach (var obj in objects)
                {
                    _students.Add(obj.GetStudent());
                }
            }

            Console.WriteLine();

            foreach (var student in _students)
            {
                Console.WriteLine($"{student.Id} {student.Name} has marks: " +
                    $"{string.Join(" ", student.Subjects.Select(x => x.FinalMark))}");
            }
        }

        public class DTOStudent
        {
            public int Id { get; private set; }
            public string Name { get; private set; }
            public string Surname { get; private set; }
            public DTOSubject[] Subjects { get; private set; }

            public DTOStudent()
            {
            }

            public DTOStudent(Student student)
            {
                Id = student.Id;
                Name = student.Name;
                Surname = student.Surname;

                var dtoObjects = new List<DTOSubject>(student.Subjects.Length);

                foreach (var subject in student.Subjects)
                {
                    dtoObjects.Add(new DTOSubject(subject));
                }

                Subjects = dtoObjects.ToArray();
            }

            public Student GetStudent()
            {
                var subjects = new Subject[Subjects.Length];

                for (int i = 0; i < subjects.Length; i++)
                {
                    subjects[i] = Subjects[i].GetSubject();
                }

                return new Student(Id, Name, Surname, subjects);
            }
        }

        // XmlInclude нужен чтоб сериализатор знал о производном тпие DTOCourse при обработке DTOSubject..
        // пишем в []
        [XmlInclude(typeof(DTOCourse))]
        public class DTOSubject
        {
            [XmlElement(ElementName = "Subject")]
            public string Name { get; set; }

            public string TypeName { get; set; }
            public int[] Marks { get; set; }
            public int Duration;

            public DTOSubject()
            {
            }

            public DTOSubject(Subject subject)
            {
                Name = subject.Name;
                Marks = subject.Marks;

                if (subject is Course course)
                {
                    Duration = course.Duration;
                }
            }

            public virtual Subject GetSubject()
            {
                return new Subject(Name, Marks);
            }
        }

        public class DTOCourse
        {
            public string Name { get; set; }
            public int[] Marks { get; set; }
            public int Duration { get; set; }

            public DTOCourse(){}

            public DTOCourse(Course subject)
            {
                Name = subject.Name;
                Duration = subject.Duration;
                Marks = subject.Marks;
            }
        }
    }
}
