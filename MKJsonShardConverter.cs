using System;
using System.Linq;
using Minikit.Inventory.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Minikit.Inventory
{
    public class MKJsonShardConverter : MKJsonConverter
    {
        private const string shardTypeKey = "_type";
        
        
        public override void WriteJson(JsonWriter _writer, object _value, JsonSerializer _serializer)
        {
            if (_value == null)
            {
                _writer.WriteNull();
                return;
            }
            
            // Temporarily remove this converter to prevent re-entry
            MKJsonShardConverter jsonShardConverter = _serializer.Converters.OfType<MKJsonShardConverter>().FirstOrDefault();
            if (jsonShardConverter != null)
            {
                _serializer.Converters.Remove(jsonShardConverter);
            }
            
            JObject jo = JObject.FromObject(_value, _serializer);
            string shardTypeName = MKShardReflector.GetRegisteredShardTypeName(_value.GetType());
            if (shardTypeName != null)
            {
                jo.AddFirst(new JProperty(shardTypeKey, shardTypeName));
            }

            // Restore the converter
            if (jsonShardConverter != null)
            {
                _serializer.Converters.Add(jsonShardConverter);
            }
            
            jo.WriteTo(_writer);
        }

        public override object ReadJson(JsonReader _reader, Type _objectType, object _existingValue, JsonSerializer _serializer)
        {
            JObject jo = JObject.Load(_reader);
            string shardTypeString = jo[shardTypeKey]?.ToString();

            Type shardType = MKShardReflector.GetRegisteredShardType(shardTypeString);
            if (shardType == null)
            {
                throw new Exception($"Invalid shard type: {shardTypeString}");
            }

            if (Activator.CreateInstance(shardType) is not MKShard shard)
            {
                throw new Exception($"Failed to create instance of shard with type: {shardTypeString}");
            }
            
            _serializer.Populate(jo.CreateReader(), shard);
            return shard;
        }

        public override bool CanConvert(Type _objectType)
        {
            return typeof(MKShard).IsAssignableFrom(_objectType);
        }
    }
} // Minikit.Inventory namespace
