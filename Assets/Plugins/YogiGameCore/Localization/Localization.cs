using System.Collections.Generic;
using FullSerializer;

namespace YogiGameCore.Localization
{
    public class Localization<T> : ILocalization<T>
    {
        [fsProperty] private Dictionary<string, T> Data { get; set; }

        public Localization()
        {
            Data = new Dictionary<string, T>();
        }

        public Dictionary<string, T> GetAll()
        {
            return this.Data;
        }

        public T TryGet(string key, T defaultValue)
        {
            return this.Data.TryGetValue(key, out T value) ? value : defaultValue;
        }

        public T GetOrAdd(string key, T defaultValue)
        {
            if (!this.Data.TryGetValue(key, out T value))
            {
                this.Data.Add(key, defaultValue);
                value = defaultValue;
            }

            return value;
        }

        public void TryAddOrModify(string key, T value)
        {
            if (this.Data.TryAdd(key, value))
                return;
            this.Data[key] = value;
        }

        public bool TryRemove(string key)
        {
            return this.Data.Remove(key);
        }

        public static Localization<T> LoadFromJson(string data)
        {
            return JsonHelper.Deserialize<Localization<T>>(data);
        }

        public string ParseToJson()
        {
            return JsonHelper.Serialize(this);
        }
    }
}