using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackagram.Models
{
    public class Question
    {
        public int ID { get; set; }
        public string Excercise { get; set; }
        public int QuestionNumber { get; set; }
        public string Answer   { get; set; }
        public Question(string excercise, int exerciseNumber ,string answer)
        {
            this.Excercise = excercise;
            this.Answer = answer;
            this.QuestionNumber = exerciseNumber;
        }
    }
}