using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackagram.Models
{
    public class Value
    {
        public string Message { get; set; }
        public string AdditionalMessage { get; set; }
        public Value()
        {
            Message = AdditionalMessage = string.Empty;
        }
        public Value(string message, string additionalMessage = "")
        {
            Message = message;
            AdditionalMessage = additionalMessage;
        }
    }
}