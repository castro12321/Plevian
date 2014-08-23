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
        public Label Sender {get; set;}
        public Label Topic { get; set; }
        public readonly String message;
        public Label Date { get; set; }

        public Message(String sender, String topic, String message, DateTime date)
        {
            this.Sender = new Label();
            this.Sender.Content = sender;

            this.Topic = new Label();
            this.Topic.Content = topic;

            this.message = message;

            this.Date = new Label();
            this.Date.Content = date;
        }

        public override string ToString()
        {
            return Topic.Content.ToString();
        }
    }
}
