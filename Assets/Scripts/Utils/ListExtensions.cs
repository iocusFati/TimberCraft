using System;
using System.Collections.Generic;

namespace Utils
{
    public static class ListExtensions
    {
        private static readonly Random _random = new Random();
        
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new InvalidOperationException("The list is empty.");
            
            int randomIndex = _random.Next(0, list.Count);
            
            return list[randomIndex];
        }
    }
}