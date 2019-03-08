using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace MyBot.Modules
{
    public class Question
    {
        private string _content;
        private string _howToRepeat;
        private readonly string _time = DateTime.Now.ToLongDateString();
        private readonly Random _rng = new Random();
        //private readonly string _idPath = @"questionIDs.txt";
        private readonly string _questionPath = @"questions.txt";
        //private List<long> _idS;
        //private long[] _parsedIDs;
        public long Id;
        public bool solved = false;


        public Question(string content, string howToRepeat, long id = 0)
        {
            _content = content;
            _howToRepeat = howToRepeat;

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
                    writer.WriteLine($"Question id: {Id}\t Question: \"{_content}\"\tSolved: {solved}\tTime: {_time}");
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(_questionPath))
                {
                    writer.WriteLine($"Question id: {Id}\t Question: \"{_content}\"\tSolved: {solved}\tTime: {_time}");
                }
            }

            Console.WriteLine("Written question");
        }


        void WriteSql()
        {

        }

        
    }
}