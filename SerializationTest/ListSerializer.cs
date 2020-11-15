using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using SerializationTest.Models;

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
                formatter.Serialize(s,head);
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