using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace backend2
{
    
    class Program
    {
        /// <summary>
        /// Generate demo database with hardcoded random values
        /// </summary>
        static void DemoInit()
        {
            using (var ctx = new Model1())
            {
                List<String> Names = new List<string>() { "Михаил", "Виталий","Петр","Антон","Вадим","Валерий" };
                List<String> Surnames = new List<string>() { "Иванов", "Петров","Сидоров","Лукьяненко" };
                List<String> Passwords = new List<string>() { "cjhjrnsc", "zxj6tpmz","yd40gece","yekb9fyf" };//y
                //List<String> Logins = new List<string>() { "user1", "" };//autoincrement in demo code
                DateTime start = DateTime.Now;

                //ctx.init();
                var rnd=new Random();//4
                var generated=new List<User>();
                for (int i = 1; i < 31; i++)
                {
                    generated.Add(
                        new User()
                        {
                            Age= rnd.Next(18,100),
                            Login = "user"+i,
                            Name=Names[rnd.Next(0,Names.Count)],
                            Passport = new Passport() { Number = rnd.Next(0, 9999), Serial = rnd.Next(0, 999999) },
                            Password = Passwords[rnd.Next(0, Passwords.Count)],
                            Sex = sex.Male,//equal 0
                            Surname = Surnames[rnd.Next(0, Surnames.Count)]
                        }
                        );
                }


                ctx.Users.AddRange(generated);
                ctx.SaveChanges();

                Console.WriteLine("New demo database created in (millis) " +
                                  (DateTime.Now - start).TotalMilliseconds);
            }

        }

        static void Main(string[] args)
        {
//            Console.WriteLine(args[0]);
//            Console.ReadKey();
            if (args.Length >= 1)
            {
                if (args[0] == "--help")
                {
                    throw new Exception("No help supplied yet :)");
                }

                if (args[0] == "--create-with-demo")
                    DemoInit();
                if (args[0] == "--drop-database")
                    Console.WriteLine((new Model1()).Database.Delete() ? "Successfully done" : "Done with error (no database?)");

                if (args[0] == "--create-database")
                {
                    DateTime start = DateTime.Now;
                    Console.WriteLine((new Model1()).Database.CreateIfNotExists()
                        ? "New database created in (millis) " + (DateTime.Now - start).TotalMilliseconds.ToString()
                        : "Already exist");

                }

            }
        }
    }
}
