using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackagram.Models
{
    public class Value
    {
        public string Message { get; set; }
        public int Number { get; set; }
        public string AdditionalMessage { get; set; }
        public string AnswerUsed { get; set; }
        public Value()
        {
            Message = AdditionalMessage = AnswerUsed = string.Empty;
            Number = 0;
        }
        public Value(string message, int number ,string additionalMessage = "", string questionAnswer = "")
        {
            Message = message;
            AdditionalMessage = additionalMessage;
            Number = number;
            AnswerUsed = questionAnswer;
        }
    }
}