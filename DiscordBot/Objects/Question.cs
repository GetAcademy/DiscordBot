using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.EC2.Model;
using Newtonsoft.Json.Serialization;

namespace MyBot.Modules
{
    public class Question
    {
        public string Content;
        public string HowToRepeat;
        public readonly string Time = DateTime.Now.ToLongDateString();
        private readonly Random _rng = new Random();
        public ulong AssignedTo;
        public ulong UserId;
        //private readonly string _idPath = @"questionIDs.txt";
        private readonly string _questionPath = @"questions.csv";
        //private List<long> _idS;
        //private long[] _parsedIDs;
        public long Id;
        public bool Solved;


        public Question(ulong userId, string content, string howToRepeat, long id = 0, string time = null, bool solved=false, ulong assigned = ulong.MinValue)
        {
            UserId = userId;
            Content = content;
            HowToRepeat = howToRepeat;
            Solved = solved;
            if (time != null)
            {
                Time = time;
            }

            AssignedTo = assigned;
            

            Id = id == 0 ? _rng.Next(100000000, 999999999) : id;

        }

        public void AddToFile()
        {
            if (!File.Exists(_questionPath))
            {
                using (StreamWriter writer = File.CreateText(_questionPath))
                {
                    writer.WriteLine($"{Id},{Content},{HowToRepeat},{Solved},{Time},{UserId},{AssignedTo}");
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(_questionPath))
                {
                    writer.WriteLine($"{Id},{Content},{HowToRepeat},{Solved},{Time},{UserId},{AssignedTo}");
                }
            }
        }

        public void WriteToFile()
        {
            if (!File.Exists(_questionPath))
            {
                using (StreamWriter writer = File.CreateText(_questionPath)) // Get all used IDs from file and assign new id to question
                {
                    writer.WriteLine($"{Id},{Content},{HowToRepeat},{Solved},{Time},{UserId},{AssignedTo}");
                }
            }
            else
            {
                var lines = File.ReadAllLines(_questionPath);
                var newLines = new string[lines.Length];
                var index = 0;
                foreach (var line in lines)
                {
                    long.TryParse(line.Split(',')[0], out var writtenId);
                    if (writtenId == Id)
                    {
                        newLines[index] = $"{Id},{Content},{HowToRepeat},{Solved},{Time},{UserId},{AssignedTo}";
                        
                    }
                    else
                    {
                        newLines[index] = line;
                    }

                    index++;
                }
                using (StreamWriter writer = File.CreateText(_questionPath))
                {
                    foreach (var line in newLines)
                    {
                        writer.WriteLine(line);
                    }
                }
            }

            Console.WriteLine("Written question");
        }

        void WriteSql()
        {

        }

    }
}