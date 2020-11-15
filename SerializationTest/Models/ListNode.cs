﻿using System;

namespace SerializationTest.Models
{
    [Serializable]
    public  class ListNode
    {
        /// <summary>
        /// Ref to the previous node in the list, null for head
        /// </summary>
        public ListNode Previous;

        /// <summary>
        /// Ref to the next node in the list, null for tail
        /// </summary>
        public ListNode Next;

        /// <summary>
        /// Ref to the random node in the list, could be null
        /// </summary>
        public ListNode Random;

        /// <summary>
        /// Payload
        /// </summary>
        public string Data;
    }
}