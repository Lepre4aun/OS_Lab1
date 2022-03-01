using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
namespace OSfirst
{
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Task 1 :");
            Console.ReadLine();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Name : {drive.Name}");
                Console.WriteLine($"Type : {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Disk size : {drive.TotalSize}");
                    Console.WriteLine($"Free Space : {drive.TotalFreeSpace}");
                    Console.WriteLine($"Lable : {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }

            Console.Write("Task 2 :");
            Console.Read();
            // создаем каталог для файла
            string path = @"C:\LAB_1";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
                Console.WriteLine("Folder created successfully");
            }
            else
            {
                Console.WriteLine("The folder already exists");
            }
            Console.Read();
            Console.WriteLine("Enter a line to write to the file :");
            string text = Console.ReadLine();
            using (FileStream fstream = new FileStream($@"{path}\trial.txt", FileMode.Append))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                await fstream.WriteAsync(array, 0, array.Length);
            }
            using (FileStream fstream = File.OpenRead($@"{path}\trial.txt"))
            {
                byte[] array = new byte[fstream.Length];
                await fstream.ReadAsync(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Text from the file : {textFromFile}");
            }
            File.Delete($@"{path}\trial.txt");
            Console.WriteLine("File trial.txt deleted");
            Console.WriteLine();

            Console.Write("Task 3 :");
            Console.Read();
            using (FileStream fstream = new FileStream($@"{path}\user.json", FileMode.OpenOrCreate))
            {
                Person Ember = new Person() { Name = "Ember", Age = 23 };
                Person Joan = new Person() { Name = "Joan", Age = 25 };
                await JsonSerializer.SerializeAsync<Person>(fstream, Ember);
                Console.WriteLine("The file has been created and already contains data");
            }
            using (FileStream fstream = File.OpenRead($@"{path}\user.json"))
            {
                Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fstream);
                Console.WriteLine($"Name : {restoredPerson.Name}  Age : {restoredPerson.Age}");
            }
            File.Delete($@"{path}\user.json");
            Console.WriteLine("The user file.json deleted");
            Console.WriteLine();
            Console.Read();

            Console.Write("Task 4 :");
            Console.Read();
            Console.WriteLine();
            Console.Read();
            XDocument xdoc = new XDocument(new XElement("people",
                new XElement("person",
                    new XAttribute("name", "Frank-Valter"),
                    new XAttribute("name", "Franky"),
                    new XElement("company", "Bosch"),
                    new XElement("age", 66)),
                new XElement("person",
                    new XAttribute("name", "Mun Chge In"),
                    new XAttribute("name", "Mumchik"),
                    new XElement("company", "Naver"),
                    new XElement("age", 69))));
            xdoc.Save($@"{path}\people.xml");
            Console.WriteLine("people.xml created");
            Console.WriteLine("Enter a name to add to the file :");
            string tempname = Console.ReadLine();
            Console.WriteLine("Enter the company to add to the file :");
            string tempcompany = Console.ReadLine();
            Console.WriteLine("Enter the age to add to the file :");
            string tempage = Console.ReadLine();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load($@"{path}\people.xml");
            XmlElement? xRoot = xDoc.DocumentElement;
            XmlElement personElem = xDoc.CreateElement("people");
            XmlAttribute nameAttr = xDoc.CreateAttribute("name");
            XmlElement companyElem = xDoc.CreateElement("company");
            XmlElement ageElem = xDoc.CreateElement("age");
            XmlText nameText = xDoc.CreateTextNode(tempname);
            XmlText companyText = xDoc.CreateTextNode(tempcompany);
            XmlText ageText = xDoc.CreateTextNode(tempage);
            nameAttr.AppendChild(nameText);
            companyElem.AppendChild(companyText);
            ageElem.AppendChild(ageText);
            personElem.Attributes.Append(nameAttr);
            personElem.AppendChild(companyElem);
            personElem.AppendChild(ageElem);
            xRoot?.AppendChild(personElem);
            xDoc.Save($@"{path}\people.xml");
            Console.WriteLine("people.xml edited\n");
            XmlDocument xxDoc = new XmlDocument();
            xDoc.Load($@"{path}\people.xml");
            XmlElement xxRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("name");
                    if (attr != null)
                        Console.WriteLine(attr.Value);
                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "company")
                    {
                        Console.WriteLine($"Company : {childnode.InnerText}");
                    }
                    if (childnode.Name == "age")
                    {
                        Console.WriteLine($"Age : {childnode.InnerText}");
                    }
                }
                Console.WriteLine();
            }
            File.Delete($@"{path}\people.xml");
            Console.WriteLine("File people.xml deleted");
            Console.WriteLine();

            Console.Write("Task 5 :");
            Console.Read();
            Console.WriteLine();
            string somepath = @"C:\LAB_1\zip";
            DirectoryInfo dirInfoo = new DirectoryInfo(somepath);
            if (!dirInfoo.Exists)
            {
                dirInfoo.Create();
            }
            Console.Read();
            using (FileStream fstream = new FileStream($@"{path}\keks.txt", FileMode.CreateNew)) { }
            string sourceFolder = @"C:\LAB_1\zip\";
            string zipFile = @"C:\LAB_1\zip.zip";
            ZipFile.CreateFromDirectory(sourceFolder, zipFile);
            Console.WriteLine($"The {sourceFolder} folder has been created and converted to an archive {zipFile}");
            Console.Read();
            using (ZipArchive zipArchive = ZipFile.Open(zipFile, ZipArchiveMode.Update))
            {
                zipArchive.CreateEntryFromFile(@"C:\LAB_1\lectures.txt", "lectures.txt");
                zipArchive.CreateEntryFromFile(@"C:\LAB_1\practices.txt", "practices.txt");
            }
            Console.Read();
            Console.Write($"lectures.txt added to the archive {zipFile}\n");
            Console.Write($"practices.txt added to the archive {zipFile}\n");
            Console.Read();
            ZipFile.ExtractToDirectory(zipFile, sourceFolder);
            Console.WriteLine($"Archive {zipFile} unpacked to {sourceFolder}");
            Console.WriteLine();
            Console.Read();
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
            Directory.Delete(sourceFolder, true);
            Console.WriteLine("Files from Task 5 deleted");
            Console.Read();
            Directory.Delete(path, true);
            Console.WriteLine("LAB_1 has been deleted from your computer");
        }
    }
}