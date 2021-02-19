using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAnalysis
{
    static class FileManager
    {
        public static void ReadFromFile(string filename, ref BlockingCollection<string> words)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
                {
                    string word = String.Empty;

                    while (sr.Peek() != -1)
                    {
                        char ch = (char)sr.Read();
                        if (Char.IsLetter(ch) && !Char.IsWhiteSpace(ch))
                        {
                            word += ch;
                        }
                        if (Char.IsPunctuation(ch) || Char.IsWhiteSpace(ch))
                        {
                            if (word != String.Empty)
                            {
                                if (Console.KeyAvailable)
                                {
                                    words.Add(word);
                                    word = String.Empty;
                                    words.CompleteAdding();
                                    sr.Close();
                                    break;
                                }
                                else
                                {
                                    words.Add(word);
                                    word = String.Empty;
                                }
                            }
                        };

                    }
                    if (word != String.Empty)
                        words.Add(word);

                    words.CompleteAdding();
                    sr.Close();
                }
            }
            catch
            {
                Console.WriteLine($"Ошибка при чтении файла: {filename}");
            }
        }
    }
}
