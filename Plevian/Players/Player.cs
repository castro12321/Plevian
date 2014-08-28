﻿using Plevian.Messages;
using Plevian.Villages;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Plevian.Players
{
    public class Player
    {
        public readonly string name;
        public readonly Color color;

        public ObservableCollection<Village> villages = new ObservableCollection<Village>();
        public ObservableCollection<Message> messages = new ObservableCollection<Message>();

        public Village Capital
        {
            get
            {
                return villages[0];
            }
        }

        // TODO: remove
        public ObservableCollection<Village> Villages
        {
            get
            {
                return villages;
            }
        }

        public Player(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        public void CaptureVillage(Village village)
        {
            if(village.Owner != null)
                village.Owner.removeVillage(village);
            village.Owner = this;
            village.turnBackAllOrders();
            addVillage(village);
        }

        public void addVillage(Village village)
        {
            villages.Add(village);
        }

        public void removeVillage(Village village)
        {
            if (!villages.Contains(village))
               throw new Exception("Removing not existing village");

            villages.Remove(village);
        }

        public void SendMessage(Message message)
        {
            messages.Add(message);
        }

        public void SendMessage(String sender, String title, String content)
        {
            SendMessage(new Message(sender, title, content, DateTime.Now));
        }

        public void DeleteMessage(Message message)
        {
            messages.Remove(message);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
