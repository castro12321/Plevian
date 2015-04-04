using Plevian.Messages;
using Plevian.Players;
using Plevian.Units;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Plevian.Messages
{
    public partial class MessageWindow : Window
    {
        public MessageWindow(Message message)
        {
            InitializeComponent();
            messageBlock.Text = message.message;
            this.Title = message.Topic.Content.ToString();
        }
    }
}
