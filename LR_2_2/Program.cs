using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml;
using XML;
namespace ConsoleApp2_2
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string input = "C:\\Users\\zaz\\source\\repos\\ConsoleApp2_2\\ConsoleApp2_2\\example.xml";
            string output = "C:\\Users\\zaz\\source\\repos\\ConsoleApp2_2\\ConsoleApp2_2\\example1.xml";
            // Устанавливаем кодировку консоли.
            // Нужно только если при использовании англоязычной Windows
            // на консоль вместо кириллицы выводятся знаки вопроса
            Console.OutputEncoding = Encoding.Unicode;
            // Читаем Xml файл.
            ReadXmlFile(input);
            // Ждем ввода пользователя.
            Console.ReadLine();
            // Создаем структуру данных.
            var catalog = new Catalog() // Корневой элемент
            {
                Phones = new List<Phone>() // Коллекция номеров телефонов.
        {
 new Phone() {Model = "Саша", Brand = "1grit" , Specs = "cool", Price = 1},
 new Phone() {Model = "Дима", Brand = "1grit", Specs = "nice", Price = 1},
 new Phone() {Model = "Рита", Brand = "1grit", Specs = "awful", Price = 978.99}
 }
            };
            // Пишем в файл.
            WriteXmlFile(output, catalog);
            // Сообщаем пользователю о завершении.
            Console.WriteLine("ОК");
            Console.ReadLine();
        }


        /*
         * Чтение XML файла на языке C#
         */


        /// <summary>
        /// Прочитать Xml файл.
        /// </summary>
        /// <param name="filename"> Путь к Xml файлу. </param>
        static void ReadXmlFile(string filename)
        {
            // Создаем экземпляр Xml документа.
            var doc = new XmlDocument();
            // Загружаем данные из файла.
            doc.Load(filename);
            // Получаем корневой элемент документа.
            var root = doc.DocumentElement;
            // Используем метод для рекурсивного обхода документа.
            PrintItem(root);
        }
        /// <summary>
        /// Метод для отображения содержимого xml элемента.
        /// </summary>
        /// <remarks>
        /// Получает элемент xml, отображает его имя, затем все атрибуты
        /// после этого переходит к зависимым элементам.
        /// Отображает зависимые элементы со смещением вправо от начала строки.
        /// </remarks>
        /// <param name="item"> Элемент Xml. </param>
        /// <param name="indent"> Количество отступов от начала строки. </param>
        static void PrintItem(XmlElement item, int indent = 0)
        {
            // Выводим имя самого элемента.
            // new string('\t', indent) - создает строку состоящую из indent табов.
            // Это нужно для смещения вправо.
            // Пробел справа нужен чтобы атрибуты не прилипали к имени.
            Console.Write($"{new string('\t', indent)}{item.LocalName} ");
            // Если у элемента есть атрибуты,
            // то выводим их поочередно, каждый в квадратных скобках.
            foreach (XmlAttribute attr in item.Attributes)
            {
                Console.Write($"[{attr.InnerText}]");
            }
            // Если у элемента есть зависимые элементы, то выводим.
            foreach (var child in item.ChildNodes)
            {
                if (child is XmlElement node)
                {
                    // Если зависимый элемент тоже элемент,
                    // то переходим на новую строку 
                    // и рекурсивно вызываем метод.
                    // Следующий элемент будет смещен на один отступ вправо.
                    Console.WriteLine();
                    PrintItem(node, indent + 1);
                }
                if (child is XmlText text)
                {
                    // Если зависимый элемент текст,
                    // то выводим его через тире.
                    Console.Write($"- {text.InnerText}");
                }
            }
        }


        /*
         * Теперь реализуем метод формирования файла на основе имеющихся данных
         */


        /// <summary>
        /// Сохранить данные в Xml файл.
        /// </summary>
        /// <param name="filename"> Путь к сохраняемому файлу. </param>
        /// <param name="catalog"> Сохраняемые данные. </param>
        static void WriteXmlFile(string filename, Catalog catalog)
        {
            // Создаем новый Xml документ.
            var doc = new XmlDocument();
            // Создаем Xml заголовок.
            var xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            // Добавляем заголовок перед корневым элементом.
            doc.AppendChild(xmlDeclaration);
            // Создаем Корневой элемент
            var root = doc.CreateElement("catalog");
            // Получаем все записи телефонной книги.
            foreach (var phone in catalog.Phones)
            {
                // Создаем элемент записи телефонной книги.
                var phoneNode = doc.CreateElement("phone");
                /*
                if (phone.Important)
                {
                    // Если установлен атрибут Важный в true,
                    // то создаем и добавляем атрибут к элементу записи телефонной книги
                    // Создаем атрибут и нужным именем.
                    var attribute = doc.CreateAttribute("group");
                    // Устанавливаем содержимое атрибута.
                    attribute.InnerText = "important";
                    // Добавляем атрибут к элементу.
                    phoneNode.Attributes.Append(attribute);
                }
                */
                // Создаем зависимые элементы.
                AddChildNode("Model", phone.Model, phoneNode, doc);
                AddChildNode("Brand", phone.Brand, phoneNode, doc);
                AddChildNode("Specs", phone.Specs, phoneNode, doc);
                AddChildNode("Price", phone.Price.ToString(), phoneNode, doc);
                // Добавляем запись телефонной книги в каталог.
                root.AppendChild(phoneNode);
            }
            // Добавляем новый корневой элемент в документ.
            doc.AppendChild(root);
            // Сохраняем документ.
            doc.Save(filename);
        }
        /// <summary>
        /// Добавить зависимый элемент с текстом.
        /// </summary>
        /// <param name="childName"> Имя дочернего элемента. </param>
        /// <param name="childText"> Текст, который будет внутри дочернего элемента. </param>
        /// <param name="parentNode"> Родительский элемент. </param>
        /// <param name="doc"> Xml документ. </param>
        static void AddChildNode(string childName, string childText, XmlElement parentNode,
        XmlDocument doc)
        {
            var child = doc.CreateElement(childName);
            child.InnerText = childText;
            parentNode.AppendChild(child);
        }
    }
}
