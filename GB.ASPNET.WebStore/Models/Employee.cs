using System.Text;

namespace GB.ASPNET.WebStore.Models
{
    public class Employee
    {
        private int age;

        public int Id { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NamePaternal { get; set; }

        public string NameShort
        {
            get
            {
                var result = new StringBuilder(NameLast);

                if (NameFirst is { Length: > 0 })
                    _ = result.Append(' ').Append(NameFirst[0]).Append('.');
                if (NamePaternal is { Length: > 0 })
                    _ = result.Append(' ').Append(NamePaternal[0]).Append('.');

                return result.ToString();
            }
        }

        public int Age
        {
            get => age;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), value, "Значение возраста не может быть меньше нуля");
                age = value;
            }
        }

        //public Employee(string nameFirst, string nameLast, string namePaternal)
        //{
        //    NameFirst = nameFirst;
        //    NameLast = nameLast;
        //    NamePaternal = namePaternal;
        //}
        //public Employee(string nameFirst, string nameLast, string namePaternal, int age)
        //    => new Employee(nameFirst, nameLast, namePaternal) { Age = age };
    }
}
