using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz_Master.Models
{
    public class Question
    {
        public string question { get; set; }

        public string[] answers { get; set; }

        public int correctAnswer;
    }
}
