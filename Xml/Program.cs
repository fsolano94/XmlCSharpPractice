using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Xml
{
    public class Date
    {
        public int Id { get; set; }
        public string Day { get; set; } = "Monday";
        public string Month { get; set; } = "Janurary";
        public string Year { get; set; } = DateTime.Now.Year.ToString();
    }

    public class Book
    {
        public string Author { get; set; }
        public string DatePublished { get; set; }
        public string Genre { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }


        public static List<Book> GetAllBooks()
        {
            return new List<Book>()
            {
                new Book(){Author  = "Robert C. Martin", DatePublished = "Aug. 1999", Genre = "Arch.", Title = "CleanCode", Rating = 10},
                new Book(){Author = "Martin Fowler", DatePublished = "Sept. 2000", Genre = "Arch.", Title = "Refactoring", Rating = 9 },
                new Book(){Author = "Gregor Hohpe", DatePublished = "Oct. 2010", Genre = "Design Patterns", Title = "Enterprise Software", Rating = 8},
                new Book(){Author = "Eric Evans", DatePublished = "Jan. 2017", Genre = "DDD", Title = "Tackling DDD", Rating = 7}
            };
        }
    }


    class Program
    {

        static void Main(string[] args)
        {
            PrintNameOf3OutOf7StatesInAddressesFileWithHighestPopulationInDescendingOrder();

            Console.ReadLine();
        }

        public static void Test_1()
        {

            string fileName = "abc.xml";

            List<Date> dates = new List<Date>();

            for (int i = 0; i < 5; i++)
            {
                dates.Add(new Date() { Id = i + 1 });
            }

            var dateAsXmlString = SerializeDateToXmlString(dates);

            Console.WriteLine("====================================");
            Console.WriteLine(dateAsXmlString);
            Console.WriteLine("====================================");


            Console.Write("Enter name of file to save xml: ");

            var filePathAndName = Console.ReadLine();

            Console.WriteLine("Saving Xml ...");

            SaveXmlToFile(dateAsXmlString, filePathAndName);

            Console.WriteLine($"Xml successfully saved at: {filePathAndName}.");

            ModifyDateElementWithSpecifiedId(fileName, 4, "Day", "SomeNewTestDay");

        }

        public static string SerializeDateToXmlString(List<Date> dates)
        {
            XmlRootAttribute root = new XmlRootAttribute("Dates");
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<Date>), root);
            XmlSerializerNamespaces xmlNamespace = new XmlSerializerNamespaces();

            xmlNamespace.Add("", "");
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww, new XmlWriterSettings() { OmitXmlDeclaration = true}))
                {
                    xsSubmit.Serialize(writer, dates, xmlNamespace);
                    xml = sww.ToString(); // Your XML
                }
            }
            return xml;
        }

        public static void SaveXmlToFile(string xmlStringToSave, string filePathAndNameOfFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStringToSave);
            // Save the document to a file and auto-indent the output.
            using (XmlTextWriter writer = new XmlTextWriter(filePathAndNameOfFile, null))
            {
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
            }
        }


        public static void ModifyDateElementWithSpecifiedId(string filePathAndNameOfXmlDocument, int idOfElement, string nameOfElementToModify, string newValueOfElement)
        {
            var xmlDocument = XDocument.Load(filePathAndNameOfXmlDocument);

            xmlDocument.Descendants("Date")
                .FirstOrDefault(element => element.Element("Id")?.Value == Convert.ToString(idOfElement))
                .SetElementValue(nameOfElementToModify, newValueOfElement);

            xmlDocument.Save(filePathAndNameOfXmlDocument);
        }

        public static void CreateXmlDocument(string nameOfRootElement, string nameOfChildElement, string comment, int childElementId, string fullPathToXmlFileThatWillBeCreated)
        {

            Console.WriteLine($"Creating file {fullPathToXmlFileThatWillBeCreated} ...");

            XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment(comment),
                new XElement(nameOfRootElement,
                new XElement(nameOfChildElement,
                    new XAttribute("id", childElementId)
                )));

            xmlDocument.Save(fullPathToXmlFileThatWillBeCreated);

            Console.WriteLine("File created successfully.");
        }

        public static void Test_CreateXmlFileOfAddresses()
        {
            Console.WriteLine("Creating xml file containing addresses ...");
            try
            {

                XDocument xmlDocumentOfAddresses = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("All addresses found."),
                    new XElement("Addresses",

                        new XElement("Address",
                            new XAttribute("id", 1),
                            new XElement("state", "NV"),
                            new XElement("city", "Reno"),
                            new XElement("zipcode", "89557"),
                            new XElement("population", 2900000)
                                    ),

                        new XElement("Address",
                            new XAttribute("id", 2),
                            new XElement("state", "CA"),
                            new XElement("city", "San Francisco"),
                            new XElement("zipcode", "94016"),
                            new XElement("population", 39540000)
                                    ),

                        new XElement("Address",
                            new XAttribute("id", 3),
                            new XElement("state", "MN"),
                            new XElement("city", "Saint Paul"),
                            new XElement("zipcode", "55101"),
                            new XElement("population", 5577000)
                                    ),

                        new XElement("Address",
                            new XAttribute("id", 4),
                            new XElement("state", "TX"),
                            new XElement("city", "Austin"),
                            new XElement("zipcode", "73301"),
                            new XElement("population", 28300000)
                                    ),
                        new XElement("Address",
                            new XAttribute("id", 5),
                            new XElement("state", "NE"),
                            new XElement("city", "Lincoln"),
                            new XElement("zipcode", "68501"),
                            new XElement("population", 1920000)
                                    ),

                        new XElement("Address",
                            new XAttribute("id", 6),
                            new XElement("state", "MI"),
                            new XElement("city", "Lansing"),
                            new XElement("zipcode", "48864"),
                            new XElement("population", 9962000)
                                    ),

                        new XElement("Address",
                            new XAttribute("id", 7),
                            new XElement("state", "MA"),
                            new XElement("city", "Boston"),
                            new XElement("zipcode", "02101"),
                            new XElement("population", 6860000)
                                    )
                    ));

                xmlDocumentOfAddresses.Save(@"C:\Users\fsola\Source\Repos\Xml\Xml\Addresses.xml");
                Console.WriteLine("Addresses.xml file created successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.ReadLine();
        }

        public static void PrintNameOf3OutOf7StatesInAddressesFileWithHighestPopulationInDescendingOrder()
        {
            
            var states = XDocument.Load(@"C:\Users\fsola\Source\Repos\Xml\Xml\Addresses.xml").Descendants("Address")
                         .OrderBy(address => Convert.ToUInt64(address.Element("population").Value)).Reverse().Take(3).Select(foundAddress => foundAddress.Element("state").Value);

            Console.WriteLine("3 of the 7 states from Addresses.xml file with the greatest population in descending order: ");

            foreach (var state in states)
            {
                Console.WriteLine(state);
            }

        }

        public static void Test_CreateXmlDocument()
        {
            string fileName = string.Empty;
            string rootElementName = string.Empty;
            string childElementName = string.Empty;
            string comment = string.Empty;
            int idOfChildElement = 0;
            
            Console.Write("Enter root element name: ");

            rootElementName = Console.ReadLine();

            Console.Write("Enter child element name: ");

            childElementName = Console.ReadLine();

            Console.Write("Enter id of child element: ");

            idOfChildElement = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter comment: ");

            comment = Console.ReadLine();

            Console.Write("Enter full path and name of file that will be created: ");

            fileName = Console.ReadLine();

            CreateXmlDocument(rootElementName, childElementName, comment, idOfChildElement, fileName);

        }

        public static void Test_CreateXmlDocumentFromListOfElements()
        {
            string fileName = string.Empty;

            Console.Write("Enter the file name and location where the list of books will be stored: ");

            fileName = Console.ReadLine();

            Console.WriteLine($"Now creating file {fileName}.");

            XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("This list of books was made up for learning how to make this file :)"),
                new XElement("Books",
                        from book in Book.GetAllBooks() select
                        new XElement("Book",
                            new XElement("Author", book.Author),
                            new XElement("DatePublished", book.DatePublished),
                            new XElement("Genre", book.Genre),
                            new XElement("Title", book.Title),
                            new XElement("Rating", book.Rating)
                            )
                ));

            xmlDocument.Save(fileName);

            Console.WriteLine($"Successfully created file {fileName}.");
        }

        public static void Test_QueryXmlFile()
        {
            var bookTitles = (from book in XDocument.Load(@"C:\listOfBooks.xml")
                             .Descendants("Book")
                             where (int)book.Element("Rating") > 9
                             orderby (int)book.Element("Rating") descending
                             select book.Element("Title").Value).ToList();


            for(int index = 1; index <= bookTitles.Count(); index++)
            {
                Console.WriteLine($"Title of book {index}: {bookTitles[index-1]}.");
            }

            
        }

    }
}
