using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// https://github.com/Deci1337/BIVT-25-Lab-9/blob/main/Program.cs
namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var students = new Student[]
            {
                new Student("a", "b", new int[,] {{ 1, 2 }, {4, 5 }, { 5, 2} }),
                new Student("A", "B", new int[,] {{ 2, 3 }, {4, 4 }, { 5, 3} }),
                new Student("C", "c", new int[,] {{ 4, 5 }, {4, 5 }, { 5, 5} })
            };
            foreach (var s in students)
            {
                Console.WriteLine(s[0]); // вызов индексатора
                Console.WriteLine(s[0, 1]); // вызов второго индексатора матрицы
                Console.WriteLine(s[0, 1] = 5); // тут делаем set по индексатору
            }

            string str = "I am a good student";
            string str2 = "Not! A am!";
            str = "Not! I am!";
            str2 = str;
            Console.WriteLine(str2);
            str2 = str.ToLower();
            Console.WriteLine(str2);
            str2 = str.Replace("am", "you"); // StringComparison.InvariantCultureIgnoreCase как третий параметр чтобы игнорировать яз
            int a = str.IndexOf('a'); // поиск индекса символа в строке
            var strings = str.Split(new char[] { '.', '!', '?' }); // разбив по этим символам. по дефолту сплитует по пробелу.
            var strings20 = str.Split(new char[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries); // метод убирает пустые строчки и всякие вхождения, чтобы было ок

            //16-ый, 1st

            foreach (var s in "crsghu crhfaf krsdahcebr suihcreiog sjfhruj crfhafor hfrafhrf rhfrkdf hrf")
            {
                bool IsLetter = Char.IsLetter(s);
                bool IsDigit = Char.IsDigit(s);
                bool IsSpaceTabNewLine = Char.IsSeparator(s);
                bool IsPunctuation = Char.IsPunctuation(s);
            }

            string output = $"New \ntext on\r each \r\n line{Environment.NewLine} ez!";

            //str2 = str.Substring(4, 3);
            Console.WriteLine(str2);



            StringBuilder sb = new StringBuilder();
            sb.Append("jhsdfsf");
            sb.Remove(1, 3);
            // преобразование динамического массива символов в строку
            sb.ToString();

            Regex regex = new Regex("[/d+]"); // регулярка, не рекомендуется но интересно. ищет по паттернам


        }
    }
    public class Student
    {
        string _name;
        string _surname;

        int[,] _marks;
        int[] _averageMarks;

        public int[,] Marks => (int[,])_marks.Clone();

        // Объявление индексатора в классе
        public double this[int idx]
        {
            get { return AverageMarks[idx]; }
        }

        // Объявим ещё один индексатор
        public int this[int i, int j]
        {
            get { return Marks[i, j]; }
            set { _marks[i, j] = value; }
        }

        //public char this[int id]
        //{
        //  get { return _name[id]; }
        //}


        public double[] AverageMarks
        {
            get
            {
                if (_marks == null || _marks.Length == 0 || _marks.GetLength(1) == 0)
                {
                    return null;
                }
                var average = new double[_marks.GetLength(0)];
                for (int i = 0; i < average.GetLength(0); i++)
                {
                    for (int j = 0; j < _marks.GetLength(1); j++)
                    {
                        average[i] += _marks[i, j] / _marks.GetLength(1);
                    }
                }
                return average;
            }
        }

        public Student(string name, string surname, int[,] marks = null)
        {
            _name = name;
            _surname = surname;
            if (marks != null)
            {
                _marks = (int[,])marks.Clone();
            }
        }

        public override string ToString()
        {
           var output =  _name + ' ' + _surname;
            for (int i = 0; i < _marks.GetLength(0); i++)
            {
                for(int j = 0; j < _marks.GetLength(1); j++)
                {
                    output += _marks[i, j] + " ";
                }
                output = output.TrimEnd(); // убераем пробел с конца
                output += Environment.NewLine;
            }
            return output;
        }

    }
}
