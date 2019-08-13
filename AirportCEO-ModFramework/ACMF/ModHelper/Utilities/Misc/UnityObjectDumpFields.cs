using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ACMF.ModHelper.Utilities.Misc
{
    public static class UnityObjectDumpFields
    {
        private static readonly string DUMPS_FOLDER_NAME = "Dumps";

        public static void DumpFields(this GameObject mb, bool printUnityComponents = true)
        {
            string path = Path.Combine(ACMF.ACMFFolderLocation, DUMPS_FOLDER_NAME);
            Directory.CreateDirectory(path);
            string fileLocation = Path.Combine(path, $"{mb.name}.txt");
            TextWriter stream = new StreamWriter(fileLocation, false);

            stream.WriteLine($"Generated From Gameobject {mb.name} at {DateTime.Now.ToString()}");
            foreach (Component c in mb.GetComponentsInChildren<Component>())
            {
                if (printUnityComponents == false && IsUnityComponent(c.GetType()))
                    continue;

                stream.WriteLine($"Component {c.name} || Type {c.GetType()} || Parent {c.transform.parent?.name}");
                DumpComponentToStream(c, stream);
                stream.WriteLine(" ");
            }

            stream.Close();
        }

        private static bool IsUnityComponent(Type t)
        {
            return t == typeof(Transform) || t == typeof(Light) || t == typeof(SpriteRenderer) || t == typeof(RectTransform)
                || t == typeof(ParticleSystem) || t == typeof(ParticleSystemRenderer);
        }

        private static void DumpComponentToStream(Component c, TextWriter stream)
        {
            // Airport CEO defined MonoBehaviours can use reflection to print out the information.
            // Unity components are special edge cases where reflection returns 0 fields.
            if (c is Transform)
                DumpComponentToStream((Transform)c, stream);
            else if (c is Light)
                DumpComponentToStream((Light)c, stream);
            else if (c is SpriteRenderer)
                DumpComponentToStream((SpriteRenderer)c, stream);
            else
                DumpComponentToStreamFallback(c, stream);
        }

        private static void DumpComponentToStreamFallback(Component c, TextWriter stream)
        {
            List<string> fieldNames = c.GetType().GetFields().Select(field => field.Name).ToList();
            List<object> fieldValues = c.GetType().GetFields().Select(field => field.GetValue(c)).ToList();
            for (int i = 0; i < fieldNames.Count; i++)
            {
                stream.WriteLine($"{fieldNames[i]} : {fieldValues[i]}");
            }
        }

        private static void DumpComponentToStream(Transform t, TextWriter stream)
        {
            stream.WriteLine($"position: {t.position}");
            stream.WriteLine($"localPosition: {t.localPosition}");
            stream.WriteLine($"rotation: {t.rotation}");
            stream.WriteLine($"localRotation: {t.localRotation}");
            stream.WriteLine($"eulerAngles: {t.eulerAngles}");
            stream.WriteLine($"localEulerAngles: {t.localEulerAngles}");
            stream.WriteLine($"lossyScale: {t.lossyScale}");
            stream.WriteLine($"localScale: {t.localScale}");
        }

        private static void DumpComponentToStream(Light t, TextWriter stream)
        {
            stream.WriteLine($"type: {t.type}");
            stream.WriteLine($"color: {t.color}");
            stream.WriteLine($"colorTemperature: {t.colorTemperature}");
            stream.WriteLine($"intensity: {t.intensity}");
            stream.WriteLine($"bounceIntensity: {t.bounceIntensity}");
            stream.WriteLine($"renderMode: {t.renderMode}");
            stream.WriteLine($"range: {t.range}");
            stream.WriteLine($"spotAngle: {t.spotAngle}");
        }

        private static void DumpComponentToStream(SpriteRenderer t, TextWriter stream)
        {
            stream.WriteLine($"color: {t.color}");
            stream.WriteLine($"flipX: {t.flipX}");
            stream.WriteLine($"flipY: {t.flipY}");
            stream.WriteLine($"material: {t.material.name}");
            stream.WriteLine($"drawMode: {t.drawMode}");
            stream.WriteLine($"sortingLayerID: {t.sortingLayerID}");
            stream.WriteLine($"sortingLayerName: {t.sortingLayerName}");
            stream.WriteLine($"sortingOrder: {t.sortingOrder}");
            stream.WriteLine($"maskInteraction: {t.maskInteraction}");
            stream.WriteLine($"Sprite: {t.sprite?.name}");
            stream.WriteLine($"Texture: {t.sprite?.texture?.name}");
            stream.WriteLine($"associatedAlphaSplitTexture: {t.sprite?.associatedAlphaSplitTexture}");
            stream.WriteLine($"border: {t.sprite?.border}");
            stream.WriteLine($"bounds: {t.sprite?.bounds}");
            stream.WriteLine($"packed: {t.sprite?.packed}");
            stream.WriteLine($"packingMode: {t.sprite?.packingMode}");
            stream.WriteLine($"packingRotation: {t.sprite?.packingRotation}");
            stream.WriteLine($"pivot: {t.sprite?.pivot}");
            stream.WriteLine($"pixelsPerUnit: {t.sprite?.pixelsPerUnit}");
            stream.WriteLine($"rect: {t.sprite?.rect}");
            stream.WriteLine($"textureRect: {t.sprite?.textureRect}");
            stream.WriteLine($"textureRectOffset: {t.sprite?.textureRectOffset}");
        }
    }
}
