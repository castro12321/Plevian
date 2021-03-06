﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Plevian;

namespace Tests
{
    class FakeGameTime : GameTime
    {
        public FakeGameTime(int time)
            : base(time)
        {
        }

        public void addFakeTime(int seconds)
        {
            Type typeInQuestion = typeof(GameTime);
            FieldInfo field = typeInQuestion.GetField("lastSystemTime", BindingFlags.NonPublic | BindingFlags.Static);
            ulong oldValue = (ulong)field.GetValue(this);
            field.SetValue(this, oldValue - (ulong)seconds);
        }
    }
}
