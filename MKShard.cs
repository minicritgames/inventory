using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
 
namespace Minikit.Inventory
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    [MKShardHidden]
    public abstract class MKShard
    {
        [JsonProperty(Order = -100)] public List<MKTag> tags = new();


        public virtual string GetDebugPrintString() { return "";  }

        public void OnAddedToItem(MKItem _item)
        {
        }

        public void OnRemovedFromItem(MKItem _item)
        {
        }
    }

    [MKShard(shardType = "Text")]
    public class MKShard_Text : MKShard
    {
        [JsonProperty] public string text;


        public override string GetDebugPrintString()
        {
            return text;
        }
    }

    [MKShard(shardType = "Float")]
    public class MKShard_NumberFloat : MKShard
    {
        [JsonProperty] public float number;


        public override string GetDebugPrintString()
        {
            return number.ToString();
        }
    }

    [MKShard(shardType = "Int")]
    public class MKShard_NumberInt : MKShard
    {
        [JsonProperty] public int number;


        public override string GetDebugPrintString()
        {
            return number.ToString();
        }
    }

    [MKShard(shardType = "Double")]
    public class MKShard_NumberDouble : MKShard
    {
        [JsonProperty] public double number;


        public override string GetDebugPrintString()
        {
            return number.ToString();
        }
    }

    [MKShard(shardType = "Bool")]
    public class MKShard_Bool : MKShard
    {
        [JsonProperty] public bool value;


        public override string GetDebugPrintString()
        {
            return value.ToString();
        }
    }

    [MKShard(shardType = "Vector2")]
    public class MKShard_Vector2 : MKShard
    {
        [JsonProperty] public Vector2 vector2;


        public override string GetDebugPrintString()
        {
            return vector2.ToString();
        }
    }

    [MKShard(shardType = "Vector3")]
    public class MKShard_Vector3 : MKShard
    {
        [JsonProperty] public Vector3 vector3;


        public override string GetDebugPrintString()
        {
            return vector3.ToString();
        }
    }

    [MKShard(shardType = "Rect")]
    public class MKShard_Rect : MKShard
    {
        [JsonProperty] public Rect rect;


        public override string GetDebugPrintString()
        {
            return rect.ToString();
        }
    }

    [MKShard(shardType = "Color")]
    public class MKShard_Color : MKShard
    {
        [JsonProperty] public Color color;


        public override string GetDebugPrintString()
        {
            return color.ToString();
        }
    }

    [MKShard(shardType = "Tag")]
    public class MKShard_Tag : MKShard
    {
        [JsonProperty] public MKTag abilityTag;
        
        
        public override string GetDebugPrintString()
        {
            return abilityTag.ToString();
        }
    }

    [MKShard(shardType = "Sprite")]
    public class MKShard_Sprite : MKShard
    {
        [JsonProperty] public Sprite sprite;
        
        
        public override string GetDebugPrintString()
        {
            return sprite.ToString();
        }
    }

    [MKShard(shardType = "Box")]
    public class MKShard_Box : MKShard
    {
        [JsonProperty] public Vector3 size;
        [JsonProperty] public Vector3 offset;
        [JsonProperty] public Vector3 rotation;
        
        
        public override string GetDebugPrintString()
        {
            return $"Box (Size={size.ToString()}, Offset={offset.ToString()}, Rotation={rotation.ToString()})";
        }
    }
} // Minikit.Inventory namespace
