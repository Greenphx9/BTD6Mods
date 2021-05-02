using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using Harmony;
using Il2CppSystem;
using Il2CppSystem.IO;
using MelonLoader;
using minicustomtowers;
using minicustomtowers.Resources;
using TMPro;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using Color = UnityEngine.Color;
using DateTime = System.DateTime;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;

namespace SixthTiers.Tasks
{
    public class Assets
    {
        private static AssetBundle _shader = null;

        public static AssetBundle shaderBundle
        {
            get
            {
                if (_shader == null)
                    _shader = AssetBundle.LoadFromMemory(Images.Shader);
                return _shader;
            }
        }

        [HarmonyPatch(typeof(Factory), nameof(Factory.FindAndSetupPrototypeAsync))]
        public class DisplayFactory
        {
            public static Dictionary<string, UnityDisplayNode> protos = new();

            [HarmonyPrefix]
            public static bool Prefix(Factory __instance, string objectId, Action<UnityDisplayNode> onComplete)
            {
                if (!protos.ContainsKey(objectId))
                {
                    if (objectId.Equals("FlaminShuriken"))
                    {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("bd9321403a5a6894c8cd923490317562",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++)
                                {
                                    if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<SpriteRenderer>())
                                    {
                                        var smr = nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                        smr.sprite = SpriteBuilder.createProjectile(CacheBuilder.Get(objectId));
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("Bombjitsu"))
                    {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("9cd388b906451874abb35c8608c1d6ed",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++)
                                {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>())
                                    {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        var assets = shaderBundle.LoadAllAssets();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }

                }

                if (protos.ContainsKey(objectId))
                {
                    onComplete.Invoke(protos[objectId]);
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ResourceLoader), nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public record ResourceLoader_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference, Image image)
            {
                if (reference != null)
                {
                    var bitmap = Images.ResourceManager.GetObject(reference.guidRef) as byte[];
                    if (bitmap != null)
                    {
                        var texture = new Texture2D(0, 0);
                        ImageConversion.LoadImage(texture, bitmap);
                        image.canvasRenderer.SetTexture(texture);
                        image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(), 10.2f);
                    }
                    else
                    {
                        var b = Images.ResourceManager.GetObject(reference.guidRef);
                        if (b != null)
                        {
                            var bm = (byte[])new ImageConverter().ConvertTo(b, typeof(byte[]));
                            var texture = new Texture2D(0, 0);
                            ImageConversion.LoadImage(texture, bm);
                            image.canvasRenderer.SetTexture(texture);
                            image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(), 10.2f);
                        }
                    }
                }
            }
        }
    }
}