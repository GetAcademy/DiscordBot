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
        private string _content;
        private string _howToRepeat;
        private readonly string _time = DateTime.Now.ToLongDateString();
        private readonly Random _rng = new Random();

        public ulong UserId;
        //private readonly string _idPath = @"questionIDs.txt";
        private readonly string _questionPath = @"questions.csv";
        //private List<long> _idS;
        //private long[] _parsedIDs;
        public long Id;
        public bool Solved;


        public Question(ulong userId, string content, string howToRepeat, long id = 0, string time = null, bool solved=false)
        {
            UserId = userId;
            _content = content;
            _howToRepeat = howToRepeat;
            Solved = solved;
            if (time != null)
            {
                _time = time;
            }

            if (id == 0)
            {
                Id = _rng.Next(100000000, 999999999);
            }
            else
            {
                Id = id;
            }

        }

        public void WriteToFile()
        {
            if (!File.Exists(_questionPath))
            {
                using (StreamWriter writer = File.CreateText(_questionPath)) // Get all used IDs from file and assign new id to question
                {
                    writer.WriteLine($"{Id},{_content},{_howToRepeat},{Solved},{_time},{UserId}\n");
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(_questionPath))
                {
                    writer.WriteLine($"{Id},{_content},{_howToRepeat},{Solved},{_time}, {UserId}\n");
                }
            }

            Console.WriteLine("Written question");
        }

        void WriteSql()
        {

        }

    }
}