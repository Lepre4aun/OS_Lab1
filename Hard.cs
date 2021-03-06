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
            Console.Write("Task 1:");
            Console.ReadLine();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Объем диска: {drive.TotalSize}");
                    Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                    Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
            }
            Console.Write("Task 2:");
            Console.Read();
            // создаем каталог для файла
            string path = @"C:\LAB1";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
                Console.WriteLine("Папка успешно создана");
            }
            else
            {
                Console.WriteLine("Папка уже существует");
            }
            Console.Read();
            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            // запись в файл
            using (FileStream fstream = new FileStream($@"{path}\test.txt", FileMode.Append))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                // асинхронная запись массива байтов в файл
                await fstream.WriteAsync(array, 0, array.Length);
                //Console.WriteLine("Текст записан в файл");
            }

            // чтение из файла
            using (FileStream fstream = File.OpenRead($@"{path}\test.txt"))
            {
                byte[] array = new byte[fstream.Length];
                // асинхронное чтение файла
                await fstream.ReadAsync(array, 0, array.Length);

                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }
            Console.WriteLine("Удалить файлы: 1 - да, 0 - нет");
            string Check1 = Console.ReadLine();
            if (Check1 == "1")
            {
                File.Delete($@"{path}\test.txt");
                Console.WriteLine("Файл test.txt удалён");
                Console.WriteLine();
            }

            Console.Write("Task 3:");
            Console.Read();
            using (FileStream fstream = new FileStream($@"{path}\user.json", FileMode.OpenOrCreate))
            {
                Person Egor = new Person() { Name = "Viva", Age = 27 };
                await JsonSerializer.SerializeAsync<Person>(fstream, Egor);
                Console.WriteLine("Файл был создан и уже содержит данные");
            }
            using (FileStream fstream = File.OpenRead($@"{path}\user.json"))
            {
                Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fstream);
                Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
            }
            Console.WriteLine("Удалить файлы: 1 - да, 0 - нет");
            string Check2 = Console.ReadLine();
            if (Check2 == "1")
            {
                File.Delete($@"{path}\user.json");
                Console.WriteLine("Файл user.json удалён");
                Console.WriteLine();
            }
            Console.Read();
            Console.Write("Task 4:");
            Console.Read();
            Console.WriteLine();
            Console.Read();
            XDocument xdoc = new XDocument(new XElement("people",
                new XElement("person",
                    new XAttribute("name", "Bogdan"),
                    new XElement("company", "Shaurma Moscow"),
                    new XElement("age", 36)),
                new XElement("person",
                    new XAttribute("name", "Matvei"),
                    new XElement("company", "Simps Artillerists Offline"),
                    new XElement("age", 17))));
            xdoc.Save($@"{path}\people.xml");
            Console.WriteLine("people.xml created");

            Console.WriteLine("Введите имя для добавления в файл:");
            string tempname = Console.ReadLine();
            Console.WriteLine("Введите компанию для добавления в файл:");
            string tempcompany = Console.ReadLine();
            Console.WriteLine("Введите возраст для добавления в файл:");
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
                        Console.WriteLine($"Компания: {childnode.InnerText}");
                    }
                    if (childnode.Name == "age")
                    {
                        Console.WriteLine($"Возраст: {childnode.InnerText}");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Удалить файлы: 1 - да, 0 - нет");
            string Check3 = Console.ReadLine();
            if (Check3 == "1")
            {
                File.Delete($@"{path}\people.xml");
                Console.WriteLine("Файл people.xml удалён");
                Console.WriteLine();
            }

            Console.Write("Task 5:");
            Console.Read();
            Console.WriteLine();
            string somepath = @"C:\LAB1\zip";
            DirectoryInfo dirInfoo = new DirectoryInfo(somepath);
            if (!dirInfoo.Exists)
            {
                dirInfoo.Create();
            }
            Console.Read();
            using (FileStream fstream = new FileStream($@"{path}\Schrek.txt", FileMode.CreateNew)) { }
            string sourceFolder = @"C:\LAB1\zip\";
            string zipFile = @"C:\LAB1\zip.zip";
            ZipFile.CreateFromDirectory(sourceFolder, zipFile);
            Console.WriteLine($"Папка {sourceFolder} создана и конвертирована в архив {zipFile}");
            Console.Read();
            using (ZipArchive zipArchive = ZipFile.Open(zipFile, ZipArchiveMode.Update))
            {
                zipArchive.CreateEntryFromFile(@"C:\LAB1\Schrek.txt", "Schrek.txt");
            }
            Console.Read();
            Console.Write($"Schrek.txt добавлен в архив {zipFile}\n");
            Console.Read();
            ZipFile.ExtractToDirectory(zipFile, sourceFolder);
            Console.WriteLine($"Архив {zipFile} распакован в папку {sourceFolder}");
            Console.WriteLine();
            Console.Read();

            Console.WriteLine("Удалить файлы: 1 - да, 0 - нет");
            string Check4 = Console.ReadLine();
            if (Check4 == "1")
            {

                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(sourceFolder, true);
                Console.WriteLine("Файлы из Task удалены");
                Console.Read();
            }
            Console.WriteLine("Удалить файлы: 1 - да, 0 - нет");
            string Check5 = Console.ReadLine();
            if (Check5 == "1")
            {
                Directory.Delete(path, true);
                Console.WriteLine("OS1 удалена с вашего компьютера :D");
            }
            Console.WriteLine();
        }
    }
}
