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


    class Program
    {

        static void Main(string[] args)
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

            Console.ReadLine();
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
    }
}
