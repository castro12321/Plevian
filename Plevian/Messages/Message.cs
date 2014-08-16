using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Plevian.Messages
{
    public class Message
    {
        public String Sender {get; set;}
        public String Topic { get; set; }
        public readonly String message;
        public DateTime Date { get; set; }

        public Message(String sender, String topic, String message, DateTime date)
        {
            this.Sender = sender;
            this.Topic = topic;
            this.message = message;
            this.Date = date;
        }
    }
}
