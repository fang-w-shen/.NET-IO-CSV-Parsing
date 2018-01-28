using System;
using System.Collections.Generic;
using System.Text;

namespace Treehouse
{
    class Student : IComparable<Student>
    {
        public string Name { get; set; }
        public int GradeLevel { get; set; }
        public int CompareTo(Student that)
        {
            int result = this.Name.CompareTo(that.Name);
            if (result == 0)
            {
                result = this.GradeLevel.CompareTo(that.GradeLevel);

            }
            return result;
        }
        public override int GetHashCode()
        {
            var n = Name.GetHashCode() + GradeLevel.GetHashCode();
            return n.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            
            if (!(obj is Student))
            {
                return false;
            }
            Student that = obj as Student;
            return this.Name == that.Name && this.GradeLevel == that.GradeLevel;
        }
    }
}
