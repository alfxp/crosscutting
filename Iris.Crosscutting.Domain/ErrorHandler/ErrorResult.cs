using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Iris.Crosscutting.Domain.ErrorHandler
{
    public class ErrorResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool Success { get { return false; } }

        public IEnumerable<string> Errors { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Url { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}