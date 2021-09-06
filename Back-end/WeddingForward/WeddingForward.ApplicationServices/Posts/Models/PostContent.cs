using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WeddingForward.ApplicationServices.Posts.Models
{
    public abstract class PostContent
    {
        public string Id { get; set; }

        public abstract string ContentUrl { get; }

        public IReadOnlyList<string> Tagged { get; set; } = new List<string>();

        public abstract PostContentType ContentType { get; }

        public int Order { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PostContentType
    {
        [EnumMember(Value = "GraphImage")]
        Photo,
        [EnumMember(Value = "GraphVideo")]
        Video,
        [EnumMember(Value = "GraphSidecar")]
        Carousel
    }
}
