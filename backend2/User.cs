using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend2
{
    public enum sex
    {
        Male = 0 , Female = 1
    }

    public class Passport
    {

        [Index]
        public int PassportId { get; set; }

        public int Serial { get; set; }

        public int Number { get; set; }

        public Passport(int serial, int number)
        {
            this.Serial = serial;
            this.Number = number;
        }
        public Passport()
        {
        }
    }

    public class User
    {

        [Index]
//        [Index(false, true, 0)]
        public int UserId { get; set; }
        public sex Sex { get; set; }
        public int Age { get; set; }//ushort?

        [MaxLength(8), MinLength(1)]
        [Index(IsClustered=false,IsUnique = true,Order=0)]//Should be
        public string Login { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string Surname { get; set; }

//        [MaxLength(8), MinLength(4)]
        public string Password { get; set; }
        public Passport Passport { get; set; }

        public bool IsAuthenticated { get; set; }

//        public User(sex sex, ushort age, string login, string name, string surename, string password, Passport passport)
//        {
//            Sex = sex;
//            Age = age;
//            Login = login ?? throw new ArgumentNullException(nameof(login));
//            Name = name ?? throw new ArgumentNullException(nameof(name));
//            Surname = surename ?? throw new ArgumentNullException(nameof(surename));
//            Password = password ?? throw new ArgumentNullException(nameof(password));
//            Passport = passport ?? throw new ArgumentNullException(nameof(passport));
//        }
//        public User(sex sex, ushort age, string login, string name, string surename, string password, ushort serial, UInt32 number)
//        {
//            Sex = sex;
//            Age = age;
//            Login = login ?? throw new ArgumentNullException(nameof(login));
//            Name = name ?? throw new ArgumentNullException(nameof(name));
//            Surname = surename ?? throw new ArgumentNullException(nameof(surename));
//            Password = password ?? throw new ArgumentNullException(nameof(password));
//            Passport = new Passport(serial, number);
//        }
    }


    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
//    public class DBusers : DbContext
//    {
//        public DBusers() : base()
//        {
//
//        }
//
//        public DbSet<User> Users { get; set; }
//        public DbSet<Passport> Passports { get; set; }
//
//
//    }
}
