using System;
using System.Collections.Generic;
using System.IO;

namespace MyBot.Modules
{
    public class LoadData
    {
        private static string _filename = @"questions.csv";
        public LoadData()
        {
            
        }

        public static List<Question> ReadQuestions()
        {
            if (!File.Exists(_filename))
            {
                File.CreateText(_filename);
            }
            List<Question> questions = new List<Question>();
            foreach (var line in File.ReadAllLines(_filename))
            {
                Console.WriteLine("new line! " + line);
                var values = line.Split(',');
                if (values.Length < 5)
                {
                    continue;
                }
                var content = values[0];
                var time = values[3];
                var howToRepeat = values[1];
                long.TryParse(values[2], out var id);
                bool.TryParse(values[4], out var solved);
                ulong.TryParse(values[5], out var userId);
                
                questions.Add(new Question(userId, content, howToRepeat, id, time, solved));
            }
            return questions;
        }
    }

}