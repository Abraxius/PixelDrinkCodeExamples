using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;

namespace Utils { 
    public class RandomUtils
    {
        static Random _R = new Random();
        private static readonly object syncLock = new object();

        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return _R.Next(min, max);
            }
        }

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            lock (syncLock)
            { // synchronize
                return (T)v.GetValue(_R.Next(v.Length));
            }
        }
    }
}