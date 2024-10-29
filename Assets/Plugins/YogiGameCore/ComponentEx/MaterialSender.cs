using UnityEngine;
using YogiGameCore.Utils;
using YogiGameCore.Utils.MonoExtent;

namespace YogiGameCore.ComponentEx
{
    public class MaterialSender : MonoBehaviour
    {
        private Material[] _allMat;
        public void Awake()
        {
            var renderer = this.GetComponent<Renderer>();
            var rendererMaterials = renderer.materials;
            _allMat = rendererMaterials;
        }

        [Button]
        public void SetColor(string key,Color value)
        {
            _allMat.ForEach(x=>
            {
                x.SetColor(key,value);
            });
        }
        [Button]
        public void SetFloat(string key,float value)
        {
            _allMat.ForEach(x=>
            {
                x.SetFloat(key,value);
            });
        }
        [Button]
        public void SetVector(string key,Vector4 value)
        {
            _allMat.ForEach(x=>
            {
                x.SetVector(key,value);
            });
        }
    }
}
