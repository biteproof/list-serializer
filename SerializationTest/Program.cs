using System;
using System.IO;
using System.Threading.Tasks;
using SerializationTest.Models;

namespace SerializationTest
{
    class Program
    {
        private static async Task Main()
        {
            var rootNode = CreateLinkedList();
            
            var serializer = new ListSerializer();
            
            using var stream = new MemoryStream();
            await serializer.Serialize(rootNode, stream);

            var deserializedNode = serializer.Deserialize(stream).GetAwaiter().GetResult();
            // user code
            Console.ReadLine();
        }

        private static ListNode CreateLinkedList()
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