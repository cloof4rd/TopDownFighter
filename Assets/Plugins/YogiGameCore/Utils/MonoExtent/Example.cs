// using System.Collections;
// using UnityEngine;
// using YogiGameCore.Utils.MonoExtent.SearchableEnum;
//
// namespace YogiGameCore.Utils.MonoExtent
// {
//     /// <summary>
//     /// 自定义特性举例
//     /// </summary>
//     public class Example : MonoBehaviour
//     {
//         public Color TestColor;
//
//         [Button]
//         public void SetScale()
//         {
//             // transform.localScale = new Vector3 (scale, scale, scale);
//         }
//
//         [Button]
//         public void SetScaleX(float scale = 5.0f)
//         {
//             transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);
//         }
//
//         [Button]
//         public void FloatIntDefault(float floatAsInt = 5)
//         {
//             Debug.Log("floatAsInt " + floatAsInt);
//         }
//
//         [Button]
//         public void EmptyMethod()
//         {
//         }
//
//         [Button]
//         public void PrintStuff(float floatVal, int intVal, string stringVal, bool boolVal)
//         {
//             Debug.Log("floatVal " + floatVal);
//             Debug.Log("intVal " + intVal);
//             Debug.Log("stringVal " + stringVal);
//             Debug.Log("boolVal " + boolVal);
//         }
//
//         [Button]
//         public void SetMaterialColor(Color color)
//         {
//             GetComponent<MeshRenderer>().sharedMaterial.color = color;
//         }
//
//         [Button]
//         public IEnumerator CountTo(int max = 6)
//         {
//             int current = 0;
//             while (current < max)
//             {
//                 Debug.Log(current++);
//                 yield return new WaitForSeconds(1.0f);
//             }
//         }
//
//         [Button]
//         public string ConvertToHex(Color color)
//         {
//             return "#" + ColorToHex(color);
//         }
//
//         [Button]
//         public void PrintNameOf(GameObject go)
//         {
//             Debug.Log(go.name);
//         }
//
//         //TAKEN FROM http://wiki.unity3d.com/index.php?title=HexConverter
//
//         // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
//         string ColorToHex(Color32 color)
//         {
//             string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
//             return hex;
//         }
//
//         Color HexToColor(string hex)
//         {
//             byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
//             byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
//             byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
//             return new Color32(r, g, b, 255);
//         }
//
//         [Button("test")]
//         public IEnumerator RunClock(int s)
//         {
//             int n = 0;
//             while (n < 3)
//             {
//                 Debug.Log(n % 2 == 0 ? "Tick" : "Tack");
//                 yield return new WaitForSeconds(1.0f);
//                 n++;
//             }
//         }
//         
//         
//         [Tooltip("The finest enum browsing experience one can have.")]
//         [SearchableEnum]
//         public KeyCode AwesomeKeyCode;
//     }
// }