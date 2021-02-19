using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrequencyAnalysis
{
    class TripletDictionary
    {
        private readonly int threadCount = Environment.ProcessorCount;                           // количество потоков
        private readonly int tripletSize = 3;                                                    // длина n-плета
        private readonly int countMaxElement = 10;                                               // количество макимально встречающихся триплетов
        private readonly string filename;                                                        // абсолютный путь к файлу
        private BlockingCollection<string> words = new BlockingCollection<string>();             // коллекция слов из файла
        private Dictionary<string, int> TripletDict = new Dictionary<string, int>();             // словарь n-плетов
        private static readonly object locker = new object();

        public TripletDictionary(string filename)
        {
            this.filename = filename;
        }

        //Разделенеие на потоки, загрузка файла
        public void AnalisStart()
        {
            Thread[] analisThreads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                analisThreads[i] = new Thread(new ThreadStart(TextAnalis));
                analisThreads[i].Start();
            }

            FileManager.ReadFromFile(filename,ref words);

            foreach (var t in analisThreads)
                t.Join();
        }

        //Анализ слов в потоках
        public void TextAnalis()
        {
            foreach (string s in words.GetConsumingEnumerable())
                WordAnalis(s);
        }

        //Вытаскиваем n-плеты из слова
        private void WordAnalis(string text)
        {
            string word = text;
            string triplet = string.Empty;
            for (int i = 0; i <= word.Length - tripletSize; i++)
            {
                for (int j = i; j < i + tripletSize; j++)
                    triplet += word[j];
                lock (locker)
                {
                    if (TripletDict.ContainsKey(triplet))
                        TripletDict[triplet] += 1;
                    else
                        TripletDict.Add(triplet, 0);
                }
                triplet = string.Empty;
            }
        }

        //Выводит на экран в строку список из countMaxElement триплетов
        public string WriteMaxTriplets()
        {
            List<string> maxTripletList = new List<string>();
            for (int i = 0; i < countMaxElement; i++)
            {
                if (TripletDict.Count > 0)
                {
                    string maxElement = TripletDict.First(x => x.Value == TripletDict.Values.Max()).Key;
                    maxTripletList.Add(maxElement);
                    TripletDict.Remove(maxElement);
                }
            }

            return string.Join(", ", maxTripletList.Select(x => x).ToList());
        }
    }
}
