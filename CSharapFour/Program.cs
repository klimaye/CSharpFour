using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Massive;
using Microsoft.Office.Interop.Excel;
using CSharapFour.Domain;

namespace CSharapFour
{
    class Fruit {}
    class Banana : Fruit {}
    class Apple : Fruit {}

    //only supports methods returning T or IEnumerable<T>
    interface ICoV<out T>
    {
        T GetAnInstanceOfT { get; }
    }

    //only supports methods taking in T
    interface IContra<in T>
    {
        void TakeInAnInstanceOfT(T item);
    }

    class Program
    {
        private static void coAndContraVariance()
        {
            //co variance
            ICoV<Apple> apple = null;
            ICoV<Fruit> fruit = apple;

            //co variance
            List<Banana> bananas = new List<Banana>();            
            //this works because IEnumerable<Fruit> is not malleable
            IEnumerable<Fruit> fruitBowl = bananas;
            //This does not work because List<Fruit> is malleable
            //List<Fruit> anotherBowl = bananas;

            //contra variance
            IContra<Fruit> anotherFruit = null;
            IContra<Banana> banana = anotherFruit;

        }

        static void Main(string[] args)
        {
            //object
            dynamic x = "hello";
            Console.WriteLine(x.Length);
            x = new int[] { 10, 20, 30 };
            Console.WriteLine(x.Length);

            ////you can do this but this is not the intended use.
            //dynamic x = 10;
            //dynamic y = 3.14;
            //dynamic z = "test";
            //dynamic k = true;
            //dynamic whoaWillThisWork = x * y + z + k;
            ////what will happen below?
            //dynamic whatAboutThis = x + y * z - k;

            //namedParameters();

            //optionalParameters();

            //Dynamics();

            //dynamicObjects();

            comExample();

            jsonWork();

            ironRubyInteract();

            Console.ReadLine();

            performanceTest();

            coAndContraVariance();

            massiveExample();

            //Base b = new Derived();
            //b.Foo( 4, 6);
        }

        private static void comExample()
        {
            var x1 = new Microsoft.Office.Interop.Excel.Application();
            x1.Visible = true;

            var workBooks = x1.Workbooks;
            var workBook = workBooks.Add(XlSheetType.xlWorksheet);
            var workSheet = (Worksheet)x1.ActiveSheet;
            //Console.WriteLine("{0}", x1.Cells.Count);
            //earlier way
            ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[1, 1]).Value2 = "Process Name";
            ((Microsoft.Office.Interop.Excel.Range)workSheet.Cells[1, 2]).Value2 = "Memory Usage";

            //new way
            workSheet.Cells[1, 1].Value2 = "Process Name";
            workSheet.Cells[1, 2].Value2 = "Memory Usage";

            var wordApp = new Microsoft.Office.Interop.Word.Application();
            wordApp.Visible = true;

            //earlier way
            object useDefaultValue = Type.Missing;

            wordApp.Documents.Add(ref useDefaultValue, ref useDefaultValue,
                ref useDefaultValue, ref useDefaultValue);

            var rng = wordApp.Selection.Range;
            rng.PasteSpecial(ref useDefaultValue,
                                            ref useDefaultValue,
                                            ref useDefaultValue,
                                            ref useDefaultValue,
                                            ref useDefaultValue,
                                            ref useDefaultValue,
                                            ref useDefaultValue);

            //new way
            wordApp.Documents.Add();


            // PasteSpecial has seven reference parameters, all of which are 
            // optional. This example uses named arguments to specify values 
            // for two of the parameters. Although these are reference 
            // parameters, you do not need to use the ref keyword, or to create 
            // variables to send in as arguments. You can send the values directly.
            rng.PasteSpecial();
        }

        private static void jsonWork()
        {

            //other uses on the web.
            var testObject = new { Name = "Dynamic", Age = 1, Popularity = "Rising" };
            var jsonRepresentation = testObject.ToJson();
            //Console.WriteLine("json created is : {0}", jsonRepresentation)
            var conversionOk = "{\"Name\":\"Dynamic\",\"Age\":1,\"Popularity\":\"Rising\"}" == jsonRepresentation;
            Console.WriteLine("json serialized ok = {0}", conversionOk);

            Console.Read();


            string jsonText = "{ xyz: 123, items: [ 10, 100, 1000 ] }";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            dynamic json = jss.Deserialize<dynamic>(jsonText);
            var items = json["items"];
            items[2] = 1020;

            Console.Read();
        }

        private static void massiveExample()
        {

            //var table = new Product();
            var table = new DynamicModel("default", tableName: "Production.Product", primaryKeyField: "ProductID");
            //grab all the products
            var products = table.All();
            Console.WriteLine("total number of products = {0}", products.Count());
            Console.ReadLine();

            //just grab from category 4. This uses named parameters
            var redColorProducts = table.All(where: "WHERE color=@0", args: "Red");
            Console.WriteLine("red color product count : {0}", redColorProducts.Count());
            Console.Read();

            dynamic prodTable =
                new DynamicModel(
                    "default",
                    tableName: "Production.Product",
                    primaryKeyField: "ProductID");

            var firstSilverProduct = prodTable.First(Color: "Silver");

            Console.WriteLine("first silver product id is {0}", firstSilverProduct.ProductID);

            Console.Read();
        }

        private static void performanceTest()
        {
            //Performance Vs Reflection
            var music = new Music("test.wav");
            var video = new Video("vid.wmv");

            PlayMedia(music);
            PlayMedia(video);

            Console.Read();
            Console.Read();
        }

        static void PlayMedia<T>(T media)
        {
            var startTick = Environment.TickCount;
            for (int i = 0; i < 1000000; i++)
            {
                media.GetType().GetMethod("Play").Invoke(media, null);
            }
            var endTick = Environment.TickCount - startTick;
            Console.WriteLine("with Reflection time taken in sec: {0} ", endTick / 1000.0m);

            startTick = Environment.TickCount;
            for (int i = 0; i < 1000000; i++)
            {
                dynamic d = media;
                d.Play();
            }
            endTick = Environment.TickCount - startTick;
            Console.WriteLine("with dynamic time taken in sec: {0} ", endTick / 1000.0m);
        }

        private static void ironRubyInteract()
        {
            var engine = IronRuby.Ruby.CreateEngine();
            engine.Execute("puts 'ruby via c#'");
            var fileContent = File.ReadAllText("MyIronRuby.rb");
            engine.Execute(fileContent);
            Console.ReadLine();


            //engine.ExecuteFile("MyIronRuby.rb");
        }

        private static void SayHello(string name, string greeting = "hello")
        {
            Console.WriteLine("{0} {1}", greeting, name);
        }

        private static void proceduralMethod(string userName, bool doesBungeeJumping, int age, bool isSmoker, bool isMale, bool ownsHome, bool doesWaterSports)
        {
            
        }

        private static void Dynamics()
        {
            //Example 2 : XML

            //definition

            //earlier way
            var contactXML =
                new XElement("Contact",
                    new XElement("Name", "Patrick Hines"),
                    new XElement("Phone", "206-555-0144"),
                    new XElement("Address",
                        new XElement("Street1", "123 Main St"),
                        new XElement("City", "Mercer Island"),
                        new XElement("State", "WA"),
                        new XElement("Postal", "68042")
                    )
                );
            //dynamic way
            dynamic contact = new ExpandoObject();
            contact.Name = "Tom";
            contact.Phone = "318-841-1111";
            contact.Address = new ExpandoObject();
            contact.Address.City = "Shreveport";
            contact.Address.State = "LA";

            //use

            //earlier way
            Console.WriteLine((string)contactXML.Element("Address").Element("State"));

            //new way

            Console.WriteLine(contact.Address.State);

            Console.ReadLine(); 
           
            //lists

            //earlier
            XElement contactsXML =
                new XElement("Contacts",
                    new XElement("Contact",
                        new XElement("Name", "Sherlock Holmes"),
                        new XElement("Phone", "206-555-0144")
                    ),
                    new XElement("Contact",
                        new XElement("Name", "Poirot"),
                        new XElement("Phone", "206-555-0155")
                    )
                );

            //new way
            dynamic contacts = new List<dynamic>();

            contacts.Add(new ExpandoObject());
            contacts[0].Name = "Sherlock Holmes";
            contacts[0].Phone = "318-111-1111";

            contacts.Add(new ExpandoObject());
            contacts[1].Name = "Poirot";
            contacts[1].Phone = "555-555-5555";

            //use & query

            //earlier
            foreach (var c in contactsXML.Descendants("Name"))
                Console.WriteLine((string)c);

            //dynamic);
            foreach (var c in contacts)
                Console.WriteLine(c.Name);

            //linq over objects
            var phonesXML = from c in contactsXML.Elements("Contact")
                            where c.Element("Name").Value == "Poirot"
                            select c.Element("Phone").Value;

            Console.WriteLine("phoneXML = {0}", phonesXML.First());

            var phone = from c in (contacts as List<dynamic>)
                         where c.Name == "Poirot"
                         select c.Phone;
            Console.WriteLine("phone = {0}", phone.First());

            //what regular linq to xml does better?
            contactsXML.Elements("Contact").Elements("Phone").Remove();

            //Vs
            foreach (var person in contacts)
                ((IDictionary<String, Object>)person).Remove("Phone");

            //linq to xml also has Save() & Load() which have no equivalent yet
            Console.ReadLine();            
        }

        private static void dynamicObjects()
        {
            dynamic dict = new DynamicDictionary();
            //dynamic properties.
            dict.Foo = "Hey";
            dict.Bar = "There";

            Console.WriteLine(string.Format("{0} {1}", dict.Foo, dict.Bar));

            Console.ReadLine();
            //dynamic method
            Func<int, string, string, string> cycleMethod =
                                        (index, even, odd) => index % 2 == 0 ? even : odd;
            dict.Cycle = cycleMethod;

            Console.WriteLine(
                string.Format("dynamic method result = {0}", dict.Cycle(2, "even", "odd")));

            Console.ReadLine();
            
            //call methods on internal type
            dict.Remove("Foo");

            Console.WriteLine("Foo is Present = {0}", dict.ContainsKey("Foo"));

            Console.ReadLine();
        }

        private static void namedParameters()
        {
            //named parameters
            proceduralMethod("Tom", true, 30, true, true, false, true);

            proceduralMethod(
                "Tom",
                doesBungeeJumping: true,
                age: 30,
                isSmoker: true,
                isMale: true,
                ownsHome: false,
                doesWaterSports: true
                );

            Console.ReadLine();            
        }

        private static void optionalParameters()
        {
            //optional parameters
            SayHello("Harry Potter");
            SayHello(name: "Harry Potter", greeting: "Guten tag ");

            Console.ReadLine();            
        }
    }
}
