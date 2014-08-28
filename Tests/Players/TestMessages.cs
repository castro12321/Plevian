using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;
using Plevian.Messages;
using Plevian.Players;
using Plevian.Villages;
using SFML.Graphics;
using System;

namespace Tests.Players
{
    [TestClass]
    public class TestMessages : TestWithTime
    {
        Player player = new Player("", SFML.Graphics.Color.White);

        [TestMethod]
        public void MessageSendingAndDeleting()
        {
            Assert.AreEqual(0, player.messages.Count);

            player.SendMessage("sender1", "title1", "content1");
            player.SendMessage("sender2", "title2", "content2");
            player.SendMessage("sender3", "title3", "content3");
            Assert.AreEqual(3, player.messages.Count);

            Message message = new Message("sender4", "topic4", "message4", DateTime.Now);
            player.SendMessage(message);
            Assert.AreEqual(4, player.messages.Count);

            player.DeleteMessage(message);
            Assert.AreEqual(3, player.messages.Count);
        }

    }
}
