using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;
using SerializationTest.Models;
using Xunit;

namespace SerializationTest.Test
{
    public class ListSerializerTest
    {
        [Fact]
        private async Task CheckSerializeAndDeserialize()
        {
            var rootNode = CreateLinkedList();
            
            var serializer = new ListSerializer();
            
            using var stream = new MemoryStream();
            await serializer.Serialize(rootNode, stream);

            var deserializedNode = await serializer.Deserialize(stream);

            while (deserializedNode.Next != null)
            {
                Assert.Equal(rootNode.Data, deserializedNode.Data);
                Assert.Equal(rootNode.Random?.Data, deserializedNode.Random?.Data);
                rootNode = rootNode.Next;
                deserializedNode = deserializedNode.Next;
            }
        }

        [Fact]
        private async Task CheckDeepCopy()
        {
            var rootNode = CreateLinkedList();
            
            var serializer = new ListSerializer();
            var nodeCopy = await serializer.DeepCopy(rootNode);

            while (nodeCopy.Next != null)
            {
                Assert.Equal(rootNode.Data, nodeCopy.Data);
                Assert.Equal(rootNode.Random?.Data, nodeCopy.Random?.Data);
                rootNode = rootNode.Next;
                nodeCopy = nodeCopy.Next;
            }
        }
        
        [Fact]
        private async Task CheckDeserializeException()
        {
            var serializer = new ListSerializer();

            var rootNode = CreateLinkedList();
            
            using (var stream = new MemoryStream())
            {
                await serializer.Serialize(rootNode, stream);
                 
                using var anotherStream = new MemoryStream();
                var options = new JsonSerializerOptions { IncludeFields = true };
                await JsonSerializer.SerializeAsync(anotherStream, new [] {"some","data", "123"}, typeof(string[]), options);
                 
                Assert.Throws<ArgumentException>(() => serializer.Deserialize(anotherStream).GetAwaiter().GetResult());
            }
        }

        private ListNode CreateLinkedList()
        {
            var dll = new DoubleLinkedList();
            
            var rootNode = dll.Init("data-0");
            var prevNode = rootNode;
            
            for (var i = 1; i < 10; i++)
            {
                prevNode = dll.Add(prevNode, $"data-{i}");
            }

            return rootNode;
        }
    }
}