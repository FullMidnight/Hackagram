﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackagram.Models
{
    public class AnsweredQuestion
    {
        public int ID { get; set; }
        public string email { get; set; }
        public string excercise { get; set; }
        public int questionNumber { get; set; }

        public AnsweredQuestion(string excercise, string email, int QuestionNumber)
        {
            this.excercise = excercise;
            this.email = email;
            this.questionNumber = QuestionNumber;
        }
        public AnsweredQuestion()
        {
            this.excercise = this.email = string.Empty;
            this.questionNumber = 0;
        }
    }
}