using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary1
{
    public static class GlobalVar
    {
        /// <summary>
        /// Used in data validating. Because no https :(
        /// </summary>
        public const string ClientSecret = "ImMegaClientSecret";
        public const string ServerSecret = "ImMegaServerSecret";
    }

    [DataContract]
    [Serializable()]
    public class Returnable
    {

        [DataMember]
        public UInt32 timestamp { get; set; }
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
        public string ToJson() //TODO check behavior 
        {
            return JsonConvert.SerializeObject(this);
        }

        public Returnable()
        {
        }

        public Returnable(string sd) : this(false, sd) { }
        public Returnable(List<User> ld) : this(false, null, ld) { }


        public Returnable(bool booleanData = true, string stringData = null, List<User> listData = null)
        {
            timestamp = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Boolean = booleanData;
            StringData = stringData;
            UserList = listData;
            CheckSum = Convert.ToBase64String(MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    timestamp +
                    (Boolean ? "true" : "false") +
                    (StringData ?? "null") +
                    ((UserList == null) ? "null" : UserList.ToArray().ToString()) +
                    GlobalVar.ServerSecret
                )
            ));//TODO confirm correct behavior, add data
               //            int a = 1;

        }

        #endregion

        [OperationContract]
        public bool CheckSumOk()
        {
            return Convert.ToBase64String(MD5.Create().ComputeHash(
                Encoding.UTF8.GetBytes(
                    timestamp +
                    (Boolean ? "true" : "false") +
                    (StringData ?? "null") +
                    ((UserList == null) ? "null" : UserList.ToArray().ToString()) +
                    GlobalVar.ServerSecret
                )
            )).Equals(CheckSum);
        }

        public dynamic ExtractData(out Type t)//TODO remove
        {
            if (StringData != null)
            {
                t = StringData.GetType();
                return StringData;
            }

            if (UserList != null)
            {
                t = UserList.GetType();
                return UserList;
            }
            t = Boolean.GetType();
            return Boolean;
        }
    }

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

        public string sSerial => $"{Serial:0000}";
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
