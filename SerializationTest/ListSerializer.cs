using System;
using System.IO;
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
            try
            {
                var options = new JsonSerializerOptions { IncludeFields = true };
                await JsonSerializer.SerializeAsync<ListNode>(s, head, options);
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
            s.Position = 0;
            try
            {
                var options = new JsonSerializerOptions { IncludeFields = true };
                return await JsonSerializer.DeserializeAsync<ListNode>(s, options);
            }
            catch (Exception)
            {
                throw new ArgumentException("Stream has invalid data");
            }
        }

        public async Task<ListNode> DeepCopy(ListNode head)
        {
            await using var stream = new MemoryStream();

            var options = new JsonSerializerOptions { IncludeFields = true };
            await JsonSerializer.SerializeAsync<ListNode>(stream, head, options);
            
            stream.Position = 0;
            return await JsonSerializer.DeserializeAsync<ListNode>(stream, options);
        }
    }
}