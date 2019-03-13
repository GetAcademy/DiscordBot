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
            Console.WriteLine("Existing Question Loaded");
            if (!File.Exists(_filename))
            {
                File.CreateText(_filename);
            }
            List<Question> questions = new List<Question>();
            foreach (var line in File.ReadAllLines(_filename))
            {
                
                var values = line.Split(',');
                if (values.Length < 5)
                {
                    continue;
                }
                var content = values[1];
                var time = values[4];
                var howToRepeat = values[2];
                long.TryParse(values[0], out var id);
                bool.TryParse(values[3], out var solved);
                ulong.TryParse(values[5], out var userId);
                ulong.TryParse(values[6], out var assigned);
                questions.Add(new Question(userId, content, howToRepeat, id, time, solved, assigned));
            }
            return questions;
        }
    }

}