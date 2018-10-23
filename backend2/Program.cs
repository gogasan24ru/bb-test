using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;

namespace backend2
{
    /// <summary>
    /// Dummy global vars
    /// </summary>

    class Program
    {
        private static Logger logger;




        private static void UMR_listener(object sender,UnknownMessageReceivedEventArgs a) {
            logger.Log("Unknown message received by Service host: "+"\n"+
                a.Message+" ("+a+")");
        }

        private static void Fault_listener(object sender,EventArgs a)
        {
            throw new Exception("Fault_listener called");
        }

        /// <summary>
        /// Start service
        /// </summary>
        /// <returns>Service entity. You should stop it, when shutting down app</returns>
        private static ServiceHost  RunService()
        {
            var t = RunCustomHost();
            t.UnknownMessageReceived += UMR_listener;
            t.Faulted += Fault_listener;
            return t;
//            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
//            smb.HttpGetEnabled = true;
//            //            smb.HttpGetBinding=new BasicHttpBinding();
////            ServiceHost host = new ServiceHost(typeof(UsersManagementService), new Uri("http://localhost:8000/UsersManagementService"));
//            ServiceHost host = new ServiceHost(smb, new Uri("http://localhost:8000/UsersManagementService"));
//            host.Open();

//            host.AddServiceEndpoint(typeof(IUsersManagementService), new WSHttpBinding(), "");
//            //            Console.WriteLine("The service is ready.");
//            //            Console.WriteLine("Press <ENTER> to terminate service.");
//            //            Console.WriteLine();
//            //            Console.ReadLine();
//            //            host.Close();
//            return host;
        }

        /// <summary>
        /// Generate demo database with hardcoded random values
        /// </summary>
        private static void DemoInit()
        {
            using (var ctx = new Model1())
            {
                List<String> Names = new List<string>() { "Михаил", "Виталий","Петр","Антон","Вадим","Валерий" };
                List<String> Surnames = new List<string>() { "Иванов", "Петров","Сидоров","Лукьяненко" };
                //List<String> Passwords = new List<string>() { "cjhjrnsc", "zxj6tpmz", "yd40gece", "yekb9fyf" };//y
                List<String> Passwords = new List<string>() { "abc123"};//y
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
                            Passport = new Passport() { Number = rnd.Next(0, 999999), Serial = rnd.Next(0, 9999) },
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
            ServiceHost host = null;
            //            events = new List<Event>();
            //            events.Add(new Event("Application start."));
            logger= new Logger();
            GlobalVar.GlobalLogger = logger;
            Trace.AutoFlush = true;
            Trace.Listeners.Add(
                new ConsoleTraceListener() { TraceOutputOptions = TraceOptions.Timestamp | TraceOptions.Callstack }
            );
            logger.Log("Application stated.");


//            System.Diagnostics.XmlWriterTraceListener tracer= new XmlWriterTraceListener();


            var arguments = args.ToList();
            try
            {
                //                var ComplicatedArgument = "--complicated-argument";
                //                if (arguments.Contains(ComplicatedArgument))
                //                {
                //                    var a = arguments[arguments.IndexOf(ComplicatedArgument) + 1];
                //                    int b; 
                //                    int.TryParse(arguments[arguments.IndexOf(ComplicatedArgument) + 2],out b);
                //                    ComplicatedMethod(a, b);
                //                }
                if (arguments.Contains("--custom-server"))
                {
                    throw new NotImplementedException();
                }
                if (arguments.Contains("--enable-tracing"))
                {
                   // Trace.Listeners.Add(new XmlWriterTraceListener("c:\\logs\\Traces.svclog", "nope"));
                    throw new NotImplementedException();
                }
                if (arguments.Contains("--help"))
                {
                    Console.WriteLine("--help - this help");
                    Console.WriteLine("--create-with-demo - create database with demo data");
                    Console.WriteLine("--drop-database - drop database is exist");
                    Console.WriteLine("--create-database - create database with empty tables");
                    Console.WriteLine("--run-service - run service and create database with empty tables if not exist");
                }
                if (arguments.Contains("--create-with-demo"))
                {
                    CreateDatabase();
                    DemoInit();
                }
                if (arguments.Contains("--drop-database"))
                {
                    logger.Log((new Model1()).Database.Delete()
                        ? "Successfully done"
                        : "Done with error (no database?)");
                }
                if (arguments.Contains("--create-database"))
                {
                    CreateDatabase();
                }
                if (arguments.Contains("--run-service"))
                {
                    CreateDatabase();
                    host = RunService();
                    if (host != null) logger.Log("service start, " + host.BaseAddresses[0]);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentException(e.Message);
            }


            if(host!=null)//Service started. wait until shutdown with ^c maybe.
                while (true)
                {
                    logger.HandleEvents();
                }




            logger.HandleEvents();
        }

        private static ServiceHost RunCustomHost()
        {
            ServiceHost host;
            try
            {
                ServiceHost svcHost = new ServiceHost(typeof(UsersManagementService),
                    new Uri("http://localhost:8000/UsersManagementService"));
                //svcHost.SingletonInstance.
                ServiceMetadataBehavior smb = svcHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (smb == null)
                    smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                svcHost.Description.Behaviors.Add(smb);
                svcHost.AddServiceEndpoint(
                    ServiceMetadataBehavior.MexContractName,
                    MetadataExchangeBindings.CreateMexHttpBinding(),
                    "mex"
                );
                
                svcHost.AddServiceEndpoint(typeof(IUsersManagementService), new WSHttpBinding(), "");
                svcHost.Open();
//                if (svcHost != null) Log("Service start, " + svcHost.BaseAddresses[0]);
                return svcHost;
            }
            catch (Exception e)
            {
                logger.HandleEvents();
                throw;
            }

        }

        private static void CreateDatabase()
        {
            DateTime start = DateTime.Now;
            logger.Log((new Model1()).Database.CreateIfNotExists()
                ? "New database created in (millis) " + (DateTime.Now - start).TotalMilliseconds.ToString()
                : "Database already exist");
        }


    }

    
}
