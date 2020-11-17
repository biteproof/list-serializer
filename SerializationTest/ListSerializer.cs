using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using SerializationTest.Models;
using System.Text.Json;

namespace SerializationTest
{
    public class ListSerializer : IListSerializer
    {
        public async Task Serialize(ListNode head, Stream s)
        {
            var beforeSerialization = s.Position;
            var formatter = new BinaryFormatter();
            try
            {
                var options = new JsonSerializerOptions 
                {
                };

                await JsonSerializer.SerializeAsync(s, head, typeof(ListNode), options);

                s.Position = beforeSerialization;
                var test = await JsonSerializer.DeserializeAsync(s, typeof(ListNode), options);


            }
            catch
            { // return stream back to it's original state
                s.Position = beforeSerialization;
                s.SetLength(s.Position);
                throw;
            }
        }

        public async Task<ListNode> Deserialize(Stream s)
        {
            var formatter = new BinaryFormatter();
            s.Position = 0;
            try
            {
                return (ListNode) formatter.Deserialize(s);
            }
            catch (Exception)
            {
                throw new ArgumentException("Stream has invalid data");
            }
        }

        public async Task<ListNode> DeepCopy(ListNode head)
        {
            await using var stream = new MemoryStream();
            var formatter = new BinaryFormatter
            {
                Context = new StreamingContext(StreamingContextStates.Clone),
            };
            
            formatter.Serialize(stream, head);
            stream.Position = 0;
            
            return (ListNode) formatter.Deserialize(stream);
        }
    }
}