using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackagram.Models
{
    public class PersonalAnsweredQuestion
    {
        public int ID { get; set; }
        public string Excercise { get; set; }
        public int QuestionNumber { get; set; }
        public string Answer { get; set; }
        public string QuestionText { get; set; }
        public int Points { get; set; }
        public string Hint { get; set; }
        public bool Done { get; set; }

        public PersonalAnsweredQuestion(int Id, string excercise, int QuestionNumber, string answer, string questionText, int points, string hint, bool isDone)
        {
            this.Excercise = excercise;
            this.Answer = answer;
            this.Points = points;
            this.QuestionNumber = QuestionNumber;
            this.QuestionText = questionText;
            this.Hint = hint;
            this.Done = isDone;
            this.ID = Id;
        }
        public PersonalAnsweredQuestion()
        {
            this.Excercise = this.Answer = this.QuestionText = this.Hint = string.Empty;
            this.Points = this.QuestionNumber = 0;
            this.Done = false;
        }
    }
}