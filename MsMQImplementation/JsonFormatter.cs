using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Messaging;

namespace Data
{
    public sealed class JsonFormatter : SingletonBase<JsonFormatter>, IMessageFormatter
    {
        private readonly JsonSerializerSettings _serializerSettings;

        private readonly JsonSerializerSettings _defaultSerializerSettings =
          new JsonSerializerSettings
          {
              TypeNameHandling = TypeNameHandling.Objects
          };

        public Encoding Encoding { get; set; }

        private JsonFormatter()
        {
            Encoding = Encoding.UTF8;
            _serializerSettings = _defaultSerializerSettings;
        }

        public bool CanRead(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            var stream = message.BodyStream;

            return stream != null
                && stream.CanRead
                && stream.Length > 0;
        }

        public object Clone()
        {
            return Instance;
        }

        public object Read(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (CanRead(message) == false)
                return null;

            using (var reader = new StreamReader(message.BodyStream, Encoding))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject(json, _serializerSettings);
            }
        }

        public void Write(Message message, object obj)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (obj == null)
                throw new ArgumentNullException("obj");

            string json = JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings);

            message.BodyStream = new MemoryStream(Encoding.GetBytes(json));

            //Need to reset the body type, in case the same message
            //is reused by some other formatter.
            message.BodyType = 0;
        }
    }
}