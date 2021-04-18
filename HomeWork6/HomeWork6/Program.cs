using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork6
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = GetN();

            if (n > 0 && n <= 1_000_000_000)
            {
                Console.WriteLine("Программа разбивает числа от 1 до N на группы, " +
                                  "при этом числа в каждой отдельно взятой группе не делятся друг на друга.");

                Console.WriteLine($"Промежуток: от 1 до {n}");

                string answer;

                do
                {
                    Console.WriteLine("Действия:\n" + 
                                      "1 - Рассчитать количество групп?\n" + 
                                      "2 - Получить группы и записать их на диск?\n" + 
                                      "Ответ (1 или 2):");
                    answer = Console.ReadLine();
                } while (answer != "1" && answer != "2");

                if (answer == "1")
                {
                    Console.WriteLine($"Количество групп: {GetNumOfGroup(n)}");
                }
                else
                {
                    DateTime startWriteGroup = DateTime.Now;

                    WriteGroup(n);

                    TimeSpan deltaTime = DateTime.Now - startWriteGroup;

                    Console.WriteLine($"Время формирования групп: {deltaTime.Minutes} минуты {deltaTime.Seconds} секунды");

                    do
                    {
                        Console.WriteLine("Поместить файл с группами в архив?:\n" +
                                          "1 - Да\n" +
                                          "2 - Нет\n" +
                                          "Ответ (1 или 2):");
                        answer = Console.ReadLine();
                    } while (answer != "1" && answer != "2");

                    if (answer == "1")
                    {
                        FileCompress("Groups.txt");
                    }
                }
            }
            else
            {
                if (n == 0)
                {
                    Console.WriteLine("Ошибка! Число в файле нет или не может быть прочитано!");
                }

                if (n < 0 || n > 1_000_000_000)
                {
                    Console.WriteLine("Ошибка! Число выходит за рамки заданного диапазона!");
                }
            }
        }

        /// <summary>
        /// Возращает число N из файла N.txt, который находится в корневой папке
        /// </summary>
        /// <returns>Возращает число N. Возращает 0, если данного числа там нет, а также если оно выходит за рамки заданного диапазона или не может быть прочитано</returns>
        public static int GetN()
        {
            int n = 0;

            string path = "N.txt";

            if (File.Exists(path))
            {
                Int32.TryParse(File.ReadAllText(path), out n);
            }

            return n;
        }

        /// <summary>
        /// Возращает кол-во групп на которые можно разбить промежуток от 1 до n
        /// </summary>
        /// <param name="n">Промежуток от 1 до n</param>
        /// <returns></returns>
        public static int GetNumOfGroup (int n)
        {
            int group = 0;

            while (Math.Pow(2, group) <= n)
            {
                group++;
            }

            return group;
        }

        /// <summary>
        /// Разбивает числа от 1 до N на группы, при этом числа в каждой отдельно взятой группе не делятся друг на друга.
        /// После получения групп производиться запись на диск.
        /// </summary>
        /// <param name="n"></param>
        public static void WriteGroup (int n)
        {
            using (StreamWriter sw = new StreamWriter("Groups.txt"))
            {
                int beginGroup = 1;
                int length = 2;
                int groups = GetNumOfGroup(n);

                for (int power = 0; power < groups; power++)
                {
                    sw.Write($"Группа {power + 1}: ");

                    if (beginGroup == 1)
                    {
                        sw.Write($"{1} ");
                    }
                    else
                    {
                        length *= 2;

                        length = length <= n ? length : n + 1;

                        for (int j = beginGroup; j < length; j++)
                        {
                            sw.Write($"{j} ");
                        }
                    }

                    sw.WriteLine();

                    Console.WriteLine($"Группа {power + 1} завершена!");

                    beginGroup *= 2;
                }
            }
        }

        /// <summary>
        /// Заархивирует файл по заданному пути
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public static void FileCompress(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream ss = new FileStream(path, FileMode.Open))
                {
                    using (FileStream ts = File.Create("Groups.zip"))
                    {
                        using (GZipStream cs = new GZipStream(ts, CompressionMode.Compress))
                        {
                            ss.CopyTo(cs);
                            Console.WriteLine($"Сжатие файла {path} завершено! Было: {ss.Length} стало: {ts.Length}");
                        }
                    }
                }
            }
        }
    }
}
