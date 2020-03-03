using System;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Лаб2
{
    /// <summary>
    /// Класс для решения задачи.</summary>
    /// <remarks>
    ///Создать абстрактный класс Source с методами, позволяющими вывести на экран информацию об издании, а также определить
    ///является ли данное издание искомым. Создать производные классы: 
    ///Book(свойства: название, фамилия автора, год издания, издательство),
    ///Paper(свойства: название, фамилия автора, название журнала, его номер и год издания), 
    ///EResources(свойства: название, фамилия автора, ссылка, аннотация) со своими методами вывода информации на экран.
    ///Создать каталог(массив) из n изданий, вывести полную информацию из каталога, а также организовать поиск изданий по фамилии автора.

    ///Значение n и характеристики ресурсов берутся из файла input.txt. </remarks>
    ///
    ///Пример корректного ввода данных из файла:
    ///n=34
    ///lastname=A
    ///Book: A,1,1934,a1
    ///Paper: Aa,1,NM,12-23,1935
    ///EResources: Aaa,1,https://docs.microsoft.com,lala

    public class Program
    {
        /// <summary>
        /// Абстрактный класс Source
        /// </summary>
        /// <remarks>
        /// От него будут наследоваться производные классы: Book, Paper, EResources
        /// </remarks>
        [XmlInclude(typeof(Book))]
        [XmlInclude(typeof(Paper))]
        [XmlInclude(typeof(EResources))]
        abstract public class Source {
            public string Name { get; set; }
            public string Author_LN { get; set; }
            public Source() { }
            public Source(string name, string author) {
                Name = name;
                Author_LN = author;
            }

            /// <summary>
            /// Абстрактный метод для возвращения информации об источнике информации
            /// </summary>
            public abstract string GetInfo();

            /// <summary>
            /// Абстрактный метод для проверки искомого издания
            /// </summary>
            /// <param name="s"> Экземпляр данного класса</param>
            /// <returns>
            /// Возвращает true/false - исколмлое ли это издание?
            /// </returns>
            public bool IsEqual(Source s) {
                if ((this.Name == s.Name) && (this.Author_LN == s.Author_LN))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Класс Book
        /// </summary>
        /// <remarks>
        /// Наследуется от Source
        /// </remarks>
        [Serializable]
        public class Book : Source {
            public int Year { get; set; }
            public string Publisher { get; set; }

            public Book() //: base()
            { }
            public Book (string name, string author, int year, string publisher): base(name, author) {
                Year = year;
                Publisher = publisher;
            }
            public override string GetInfo() {
                return Name + ", " + Author_LN + ", " + Publisher  + ", " + Year;
            }

            public bool IsEqual(Book b)
            {
                if ((this.Name==b.Name) && (this.Author_LN==b.Author_LN) &&
                    (this.Year==b.Year) && (this.Publisher==b.Publisher))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Класс Paper
        /// </summary>
        /// <remarks>
        /// Наследуется от Source
        /// </remarks>
        public class Paper : Source
        {
            public string Magazine_Name { get; set; }
            public int Year { get; set; }
            public string Number { get; set; }
            public Paper() { }
            public Paper (string name, string author, string magName, string number, int year) : base(name, author)
            {
                Year = year;
                Number = number;
                Magazine_Name = magName;
            }
            public override string GetInfo()
            {
                return Name + ", " + Author_LN + ", " + Magazine_Name + ", " + Number + ", " + Year;
            }

            public bool IsEqual(Paper p)
            {
                if ((this.Name == p.Name) && (this.Author_LN == p.Author_LN) &&
                   (this.Year == p.Year) && (this.Number == p.Number) && (this.Magazine_Name == p.Magazine_Name))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Класс EResources
        /// </summary>
        /// <remarks>
        /// Наследуется от Source
        /// </remarks>
        public class EResources : Source
        {
            public string Link { get; set; }
            public string Annotation { get; set; }
            public EResources() { }

            public EResources(string name, string author, string link, string annotation) : base(name, author)
            {
                Link = link;
                Annotation = annotation;
            }
            public override string GetInfo()
            {
                return Name + ", " + Author_LN + ", " + Link + ", " + Annotation;
            }

            public bool IsEqual(EResources er)
            {
                if ((this.Name == er.Name) && (this.Author_LN == er.Author_LN) &&
                   (this.Link == er.Link) && (this.Annotation == er.Annotation))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Точка входа для приложения.
        /// </summary>
        /// <remarks>
        /// Читает входной файл и выводит каталог ресурсов, также организовывает поиск изданий по фамилии автора.
        /// </remarks>
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(System.IO.File.CreateText("Trace.txt")));//new TextWriterTraceListener(Console.Out)
            Trace.AutoFlush = true;
            Trace.Indent();
            Trace.WriteLine("Entering Main");
            
            string lastname = "";
            int n;
            int ind = 0;
            Source[] sources;

            Trace.Indent();
            Trace.WriteLine("Trying to read data from input");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.txt");
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    
                    line = sr.ReadLine();
                    char ch = '=';
                    int indexOfChar = line.IndexOf(ch)+1;
                    if (indexOfChar == 0)
                    {
                        throw new ArgumentException("некорректная информация об n");
                    }
                    n = Int32.Parse(line.Substring(indexOfChar).Trim(' '));
                    sources = new Source[n];
                    Trace.Indent();
                    Trace.WriteLine("n found successfully");

                    line = sr.ReadLine();
                    ch = '=';
                    indexOfChar = line.IndexOf(ch)+1;//0
                    if (indexOfChar == 0)
                    {
                        throw new ArgumentException("некорректная информация о lastname");
                    }
                    lastname = line.Substring(indexOfChar).Trim(' ');
                    Trace.WriteLine("lastname found successfully");

                    Trace.Indent();
                    Trace.WriteLine("Trying to read other information");
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("Book:"))
                        {
                            ch = ':';
                            indexOfChar = line.IndexOf(ch)+1;
                            line = line.Substring(indexOfChar);
                            String[] elem = line.Split(new char[] { ',',' ' }, StringSplitOptions.RemoveEmptyEntries);
                            sources[ind] = new Book(elem[0], elem[1], Int32.Parse(elem[2]), elem[3]);
                            ind++;
                        }
                        if (line.Contains("Paper:"))
                        {
                            ch = ':';
                            indexOfChar = line.IndexOf(ch)+1;
                            line = line.Substring(indexOfChar);
                            String[] elem = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            sources[ind] = new Paper(elem[0], elem[1], elem[2], elem[3], Int32.Parse(elem[4]));
                            ind++;
                        }
                        if (line.Contains("EResources:"))
                        {
                            ch = ':';
                            indexOfChar = line.IndexOf(ch)+1;
                            line = line.Substring(indexOfChar);
                            String[] elem = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            sources[ind] = new EResources(elem[0], elem[1], elem[2], elem[3]);
                            ind++;
                        }
                    }
                    Trace.WriteLine("information found successfully");
                    Trace.Unindent();
                    Trace.WriteLineIf(ind !=n, "n и количество источников не совпадает");

                    if (ind != n) {
                        throw new IndexOutOfRangeException("n и количество источников не совпадает");
                    }
             
                    //if (stInp == "")
                    //{
                    //    throw new ArgumentException("Вы ничего не ввели.");//ArgumentException, ArgumentNullException
                    //}
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(@"Файл input.txt не удалось найти");
                Console.ReadKey();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return;
            }
            Trace.Unindent();
            Trace.WriteLine("information was successfully extracted from the input file");


            Console.WriteLine("Информация о всех изданиях:");
            foreach (var p in sources)
            {
                Console.WriteLine(p.GetInfo());
            }

            Console.WriteLine("Поиск по фамилии:");
            foreach (var p in sources)
            {
                if (p.Author_LN == lastname)
                {
                    Console.WriteLine(p.GetInfo());
                }
            }


            
            Book b1=new Book("Ск2","ХМ",1224,"изд2" );
            Paper p1 = new Paper("p1", "A1", "Mag1", "12-12-121", 1234);
            EResources er1 = new EResources("er1", "A1", "link1", "Ann1");


            Trace.WriteLine("Started serialization");


            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(Source[]));
            //XmlSerializer formatter1 = new XmlSerializer(typeof(Book));
            //XmlSerializer formatter2 = new XmlSerializer(typeof(Paper));
            //XmlSerializer formatter3 = new XmlSerializer(typeof(EResources));
            try
            {
                using (FileStream fs = new FileStream("Sources.xml", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, sources);
                    //formatter1.Serialize(fs, b1);
                    //formatter2.Serialize(fs, p1);
                    //formatter1.Serialize(fs, er1);
                    Console.WriteLine("Сериализация");
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }


            // десериализация
            try
            {
                using (FileStream fs = new FileStream("Sources.xml", FileMode.OpenOrCreate))
                {
                    Source[] news = (Source[])formatter.Deserialize(fs);

                    //Book newb = (Book)formatter1.Deserialize(fs);
                    //Paper newp = (Paper)formatter2.Deserialize(fs);
                    //EResources newer = (EResources)formatter3.Deserialize(fs);

                    Console.WriteLine("Десериализация");
                    foreach (var p in news)
                    {
                        Console.WriteLine(p.GetInfo());
                    }
                    //Console.WriteLine($"Имя: {newb.Name} --- Автор: {newb.Author_LN} --- Год издания: {newb.Year} --- Издательство: {newb.Publisher}");
                    //Console.WriteLine($"Имя: {newp.Name} --- Автор: {newp.Author_LN} --- Журнал: {newp.Magazine_Name} --- Номер журнала: {newp.Number} --- Год издания: {newp.Year}");
                    //Console.WriteLine($"Имя: {newer.Name} --- Автор: {newer.Author_LN} --- Ссылка: {newer.Link} --- Аннотация: {newer.Annotation}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Trace.WriteLine("Serialization was successfully");
            Trace.Unindent();

            Console.WriteLine("Проверка на искомое издание:");
            Book b2 = new Book("Ск", "ХМ", 1224, "изд");
            Console.WriteLine(b1.IsEqual(b2));
    
            Paper p2 = new Paper("p1", "A1", "Mag1", "12-12-121", 1234);
            Console.WriteLine(p1.IsEqual(p2));
   
            EResources er2 = new EResources("er2", "A1", "link2", "Ann2");
            Console.WriteLine(er2.IsEqual(er1));

            //List<Source> list = new List<Source>();
            //list.Add(new Book("Book","Test",2005,"Star"));
            //list.Add(new Paper("Paper","Test","Super", "12", 2012));
            //list.Add(new EResources("Internet", "Test", "http", "Annotation"));

            //foreach (var p in list)
            //{
            //    Console.WriteLine(p.GetInfo());
            //}

            ////Поиск по фамилии
            //string lastname2 = "Test";
            //foreach (var p in list.FindAll(item => item.Author_LN == lastname2))
            //{
            //    Console.WriteLine(p.GetInfo());
            //}

            Trace.WriteLine("Exiting Main");
            Trace.Unindent();
            Trace.Flush();

            Console.ReadKey();
        }
    }
}
