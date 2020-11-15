using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SerializationTest.Models;
using Xunit;

namespace SerializationTest.Test
{
    public class ListSerializerTest
    {
        [Fact]
        private void CheckSerializeAndDeserialize()
        {
            var rootNode = CreateLinkedList();
            
            var serializer = new ListSerializer();
            
            using var stream = new MemoryStream();
            serializer.Serialize(rootNode, stream);

            var deserializedNode = (serializer.Deserialize(stream)).GetAwaiter().GetResult();

            while (deserializedNode.Next != null)
            {
                Assert.Equal(rootNode.Data, deserializedNode.Data);
                Assert.Equal(rootNode.Random?.Data, deserializedNode.Random?.Data);
                rootNode = rootNode.Next;
                deserializedNode = deserializedNode.Next;
            }
        }

        [Fact]
        private void CheckDeepCopy()
        {
            var rootNode = CreateLinkedList();
            
            var serializer = new ListSerializer();
            var nodeCopy = (serializer.DeepCopy(rootNode)).GetAwaiter().GetResult();

            while (nodeCopy.Next != null)
            {
                Assert.Equal(rootNode.Data, nodeCopy.Data);
                Assert.Equal(rootNode.Random?.Data, nodeCopy.Random?.Data);
                rootNode = rootNode.Next;
                nodeCopy = nodeCopy.Next;
            }
        }
        
        [Fact]
        private void CheckDeserializeException()
        {
            var serializer = new ListSerializer();

            var rootNode = CreateLinkedList();
            
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(rootNode, stream);
                 
                using var anotherStream = new MemoryStream();
                var formatter = new BinaryFormatter();
                 
                formatter.Serialize(anotherStream, new [] {"some","data", "123"});
                Assert.Throws<ArgumentException>(() => (serializer.Deserialize(anotherStream)).GetAwaiter().GetResult());
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