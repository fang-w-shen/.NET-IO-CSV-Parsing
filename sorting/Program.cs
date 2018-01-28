using System;
using System.Collections.Generic;
using System.Text;

namespace Treehouse
{
    class Program
    {
        static void Main()
        {
            HashSet<Student> students = new HashSet<Student>
            {
                new Student() {Name = "Sally", GradeLevel = 3},
                new Student() {Name = "Bob", GradeLevel = 3},
                new Student() {Name = "Sally", GradeLevel = 2}
            };
            Student joe = new Student() { Name = "Joe", GradeLevel = 2 };
            students.Add(joe);
            Student duplicatejoe = new Student() { Name = "Joe", GradeLevel = 2 };
            Console.WriteLine(joe.Equals(duplicatejoe));
            foreach (Student student in students)
            {
                Console.WriteLine(string.Format("{0} is in grade {1}", student.Name, student.GradeLevel));
            }

            if (students.Contains(duplicatejoe))
            {
                Console.WriteLine("does");
         
            }
            else
            {
                Console.WriteLine("does not");
            }
            //students.Sort();
            //Student b = students[0];
            //int index = students.IndexOf(b);
            //if(index < 0)
            //{
            //    students.Insert(~index, b);
            //}
            //Console.WriteLine(index);

            //SchoolRoll schoolRoll = new SchoolRoll();
            // schoolRoll.AddStudents(students);
            // because <List> was changed to <IEnumerable> now dont have removeat sort or addrange methods
            //schoolRoll.Students.RemoveAt(0);
            //schoolRoll.Students.Sort();
            //schoolRoll.Students.AddRange(students);

        }
    }
}
