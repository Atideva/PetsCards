 

 
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Misc
{
    public static class Extensions 
    {
        public static T With<T>(this T self, Action<T> set)
        {
            set.Invoke(self);
            return self;
        }
 
        public static T With<T>(this T self, Action<T> apply, Func<bool> when)
        {
            if (when())
                apply?.Invoke(self);
 
            return self;
        }
 
        public static T With<T>(this T self, Action<T> apply, bool when)
        {
            if (when)
                apply?.Invoke(self);
 
            return self;
        }
     
        
        public static List<T> Shuffle<T>(List<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var temp = list[i];
                var randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }

            return list;
        }
    }
}
