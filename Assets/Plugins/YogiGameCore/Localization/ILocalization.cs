using System.Collections.Generic;

namespace YogiGameCore.Localization
{
    public interface ILocalization<T>
    {
        public Dictionary<string, T> GetAll();
        public T TryGet(string key, T defaultValue);
        public T GetOrAdd(string key, T defaultValue);
        public void TryAddOrModify(string key, T value);
        // public  ILocalization<T> LoadFromJson(string data);
        public string ParseToJson();
    }
}