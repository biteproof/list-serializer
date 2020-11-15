using System;
using SerializationTest.Models;

namespace SerializationTest
{
    /// <summary>
    /// Realization of simple double linked list
    /// </summary>
    public class DoubleLinkedList
    {
        public int Length { get; set; }

        public ListNode Init(string payload)
        {
            Length++;
            return new ListNode{ Data = payload };
        } 
        
        public ListNode Add(ListNode prevNode, string payload)
        {
            var newNode = new ListNode
            {
                Previous = prevNode,
                Next = prevNode.Next,
                Data = payload
            };

            prevNode.Next = newNode;
            Length++;
            
            return newNode;
        }
    }
}