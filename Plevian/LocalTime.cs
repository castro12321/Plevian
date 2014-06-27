﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    public class LocalTime
    {
        /// <summary>
        /// Represents virtual seconds since game start
        /// </summary>
        public ulong seconds { get; private set; }

        public LocalTime(ulong hours, ulong minutes, ulong seconds)
            : this(hours*60*60 + minutes*60 + seconds)
        {
        }

        public LocalTime(ulong seconds)
        {
            this.seconds = seconds;
        }

        public static LocalTime operator +(LocalTime lh, LocalTime rh)
        {
            return new LocalTime(lh.seconds + rh.seconds);
        }
    }
}