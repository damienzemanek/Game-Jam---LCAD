using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Extensions
{
    public static class EnumerateEX
    {

        public static float Rand(this Vector2 v)
        {
            return UnityEngine.Random.Range(v.x, v.y);
        }

        #region Privates

        #endregion
        public static T Rand<T>(this T[] ts)
        {
            return ts[UnityEngine.Random.Range(0, ts.Length)];
        }
        public static T Rand<T>(this T[] ts, T[] exclude)
        {
            var include = ts.Where(t => !exclude.Contains(t)).ToArray();
            if (include.Length <= 0) return default;
            return Rand(include);
        }
        public static T Rand<T>(this T[] ts, int min, int max)
        {
            return ts[UnityEngine.Random.Range(min, max)];
        }


        public static T Rand<T>(this List<T> ts)
        {
            return ts[UnityEngine.Random.Range(0, ts.Count)];
        }

        public static T Rand<T>(this List<T> ts, List<T> exclude)
        {
            var include = ts.Where(t => !exclude.Contains(t)).ToList();
            if (include.Count <= 0) return default;
            return Rand(include);
        }

        public static T Rand<T>(this List<T> ts, int min, int max)
        {
            return ts[UnityEngine.Random.Range(min, max)];
        }

        public static List<T> Swap<T>(this List<T> list, int first, int second)
        {
            if (first == second) return list;
            if (first < 0 || second < 0 || first >= list.Count || second >= list.Count) throw new ArgumentOutOfRangeException();

            T buffer = list[first];
            list[first] = list[second];
            list[second] = buffer;

            return list;
        }

        public static List<GameObject> SetAllActive(this List<GameObject> list, bool val)
        {
            for(int i = 0; i < list.Count; i++) list[i].SetActive(val);
            return list;
        }

        public static GameObject[] SetAllActive(this GameObject[] list, bool val)
        {
            for (int i = 0; i < list.Length; i++) list[i].SetActive(val);
            return list;
        }


        #region Methods

        #endregion

    }

}