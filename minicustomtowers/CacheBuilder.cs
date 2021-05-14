using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using MelonLoader;
using minicustomtowers.Resources;
using UnityEngine;

//This code has been modified from 1330 Studio's BTD6E Modules. Huge thanks to them for letting me use this code.

namespace minicustomtowers
{
    public class CacheBuilder
    {
        public static AssetStack<string> toBuild = new();
        private static readonly Dictionary<string, string> built = new();
        private static readonly Dictionary<string, byte[]> builtBytes = new();

        public static void Build()
        {
            while (toBuild.Count > 0)
            {
                var id = toBuild.Pop();
                if (Images.ResourceManager.GetObject(id) is not byte[] bitmap) break;
                //var v = id.Contains("center") ? 0.5f : 0f;
                built.Add(id, Convert.ToBase64String(bitmap));
            }
        }

        public static Texture2D Get(string key)
        {
            if (builtBytes.ContainsKey(key))
            {
                var text = LoadTextureFromBytes(builtBytes[key]);
                return text;
            }

                var bytes = Convert.FromBase64String(built[key]);
                builtBytes.Add(key, bytes);
                var textNew = LoadTextureFromBytes(Convert.FromBase64String(built[key]));
                return textNew;
        }

        public static void Flush(bool shouldFlushStack = true)
        {
            if (shouldFlushStack) toBuild.Clear();
            built.Clear();
        }

        private static Texture2D LoadTextureFromBytes(byte[] FileData)
        {
            Texture2D Tex2D = new Texture2D(2, 2);
            if (ImageConversion.LoadImage(Tex2D, FileData)) return Tex2D;

            return null;
        }
    }
}