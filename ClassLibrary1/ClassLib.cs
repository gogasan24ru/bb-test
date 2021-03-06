﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public enum LogLevel
    {
        NotClassified = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }
    public class Event
    {
        private bool displayed;
        private string message;
        private DateTime timestamp;

        public Event(string msg)
        {
            timestamp = DateTime.Now;
            displayed = false;
            message = msg;
        }

        public bool Displayed => displayed;

        /// <summary>
        /// custom override with self modifying
        /// </summary>
        /// <returns>DateTime : Message</returns>
        public override string ToString()
        {
            displayed = true;
            return timestamp + " : " + message; //NOTTODO: +severity or smthing. Ignoring: possible over-engineering 
        }
    }

    public  class Logger
    {
        private LogLevel MinLogLevel = 0;
        private List<Event> events = new List<Event>();

        public Logger(LogLevel mll=0)
        {
            MinLogLevel = mll;
        }
        public void Log(string msg, LogLevel lvl = 0)
        {
//            Trace.WriteLine(msg);
            if (lvl>=MinLogLevel)
                lock (events)
                    events.Add(new Event(msg));
        }
        public void HandleEvents()
        {
            Trace.Flush();
            lock (events)
                foreach (var a in events.FindAll(a => !a.Displayed))
                    Console.WriteLine(a.ToString());
        }
    }


    public static  class GlobalVar
    {
        public const string ClientSecret = "ImMegaClientSecret";
        public const string ServerSecret = "ImMegaServerSecret";

        public static Logger GlobalLogger;
        public static List<object> Misc;//dummy walkaround 
    }

    [DataContract]
    [Serializable()]
    public class Returnable
    {
        //        [IgnoreDataMember] private string key = "not set";
        //maybe add inheritance for duplex usage???

        [DataMember]
        public UInt32 timestamp { get; set; }
        [DataMember]
        public int IntData { get; set; }
        [DataMember]
        public string CheckSum { get; set; }
        [DataMember]
        public string Contains { get; set; }
        [DataMember]
        public bool Boolean { get; set; }

        [DataMember]
        public string StringData { get; set; }
        [DataMember]
        public List<User> UserList { get; set; }

        #region used functions
        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public Returnable()
        {
        }

        public Returnable(string sd) : this(true, sd) { }
        public Returnable(List<User> ld) : this(true, null, ld) { }
        public Returnable(int i) : this(true, null, null,i) { }


        public Returnable(bool booleanData = true, string stringData = null, List<User> listData = null, int intdata=int.MinValue)
        {
            timestamp = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Boolean = booleanData;
            StringData = stringData;
            UserList = listData;
            IntData = intdata;
            CheckSum = Convert.ToBase64String(MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    timestamp +
                    (Boolean ? "true" : "false") +
                    (StringData ?? "null") +
                    IntData +
                    ((UserList == null) ? "null" : UserList.ToArray().ToString()) +
                    GlobalVar.ServerSecret
                )
            ));

        }

        #endregion

//        [OperationContract]
        public bool CheckSumOk =>
        
             Convert.ToBase64String(MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    timestamp +
                    (Boolean ? "true" : "false") +
                    (StringData ?? "null") +
                    IntData +
                    ((UserList == null) ? "null" : UserList.ToArray().ToString()) +
                    GlobalVar.ServerSecret
                )
            )).Equals(CheckSum);

    }

    public enum sex
    {
        Male = 0 , Female = 1
    }

    public class Passport
    {

        [Index]
        [ScaffoldColumn(false)]
        public int PassportId { get; set; }

        [Required, Range(0, 9999)]
        [Display(Name = "Серия паспотра")]
        public int Serial { get; set; }

        [Required, Range(0, 999999)]
        [Display(Name="Номер паспотра")]
        public int Number { get; set; }



        [ScaffoldColumn(false)]
        public string sSerial => $"{Serial:0000}";

        [ScaffoldColumn(false)]
        public string sNumber => $"{Number:000000}";

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
        public override string ToString()
        {
            return "" + UserId + ";" + Sex + ";" + Age + ";" + Login + ";" + Name + ";" + Surname + ";" + Password +
                   ";" + Passport.ToString();
        }

        [Index]
        [ScaffoldColumn(false),Required]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Пол")]
        [ScaffoldColumn(false)] //required for drop down list. Any better solution? 
        public sex Sex { get; set; }

        [Required]
        [Display(Name = "Возраст")]
        [Range(18, int.MaxValue,ErrorMessage = "Age should be at least 18")]
        public int Age { get; set; } = 18;//ushort?

        [MaxLength(8, ErrorMessage = "Login too long"), MinLength(1,ErrorMessage = "Login too short")]
        [Index(IsClustered=false,IsUnique = true,Order=0)]//Should be
        [Required]
        [Display(Name = "Login")]
        [RegularExpression("^[A-Za-z0-9]{1,8}$", ErrorMessage = "Login length must be between 1 and 8 characters, only latin alphabet and digits are allowed")]
        public string Login { get; set; }

        //[MaxLength(30, ErrorMessage = "Name too long")]
        [RegularExpression("^[А-Яа-я]{0,30}$",ErrorMessage = "Name length must be les than 30 characters, cyrillic alphabet, only letters")]
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        //[MaxLength(30)]
        [RegularExpression("^[А-Яа-я]{0,30}$", ErrorMessage = "Surname length must be les than 30 characters, cyrillic alphabet, only letters")]
        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        //        [MaxLength(8), MinLength(4)]
        [Required]
        [Display(Name = "Пароль")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*[0-9]).{4,8}$", ErrorMessage = "Password length must be between 4 and 8 characters, only latin alphabet and at least one digit")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Паспортные данные")]
        public Passport Passport { get; set; }

        [ScaffoldColumn(false)]
        public bool ChangedFlag { get; set; } 
        [ScaffoldColumn(false)]
        public string SessionKey { get; set; }

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
