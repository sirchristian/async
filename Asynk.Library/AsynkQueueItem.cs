using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Asynk.Library
{
    /// <summary>
    /// Represents an item that can go onto an AsynkQueue
    /// </summary>
    [DataContract(Name="ASynkQueueItem", Namespace=Constants.WebSite), Serializable]
    public class AsynkQueueItem
    {
        /// <summary>
        /// The full name of the assembly the method that will be run asynchronously
        /// is defined in
        /// </summary>
        [DataMember]
        public string AssemblyName { get; set; }

        /// <summary>
        /// The type name of the type where the method is declared
        /// </summary>
        [DataMember]
        public string FullTypeName { get; set; } 

        /// <summary>
        /// The method name of the method that will be run asynchronously
        /// </summary>
        [DataMember]
        public string MethodName { get; set; }

        /// <summary>
        /// Object that the method can get run against. Can be null for
        /// static methods
        /// </summary>
        [DataMember]
        public string Obj { get; set; }

        /// <summary>
        /// A dictionary of argument that the method needs
        /// </summary>
        [DataMember]
        public List<string> Arguments { get; set; }

        /// <summary>
        /// A counter variable that can be incremented when the method is called.
        /// To increment the variable call <see cref="Called"/>
        /// </summary>
        [DataMember]
        public int TimesCalled 
        {
            get { return _timesCalled; }
            private set { _timesCalled = 0; }
        }
        private int _timesCalled = 0;

        /// <summary>
        /// Call when this items should increment the times called method
        /// </summary>
        public void Called()
        {
            _timesCalled++;
        }

        /// <summary>
        /// The Id of the item in the queue (automatically generated)
        /// </summary>
        [DataMember]
        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();
                }

                return _id;
            }
            private set { _id = value; }
        }
        private Guid _id = Guid.Empty;

        #region Equality Operators
        /// <summary>
        /// Overrides the == operator and compares the Ids of the items
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(AsynkQueueItem x, AsynkQueueItem y)
        {
            // if both operands are null, return true
            if ((x as object) == null && (y as object) == null)
                return true;

            // if one operand is null, return false
            if ((x as object) == null || (y as object) == null)
                return false;

            // return the id comparison
            return (x.Id == y.Id);
        }

        /// <summary>
        /// Overrides the != operator
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(AsynkQueueItem x, AsynkQueueItem y)
        {
            // if both operands are null, return false
            if ((x as object) == null && (y as object) == null)
                return false;

            // if one operand is null, return true
            if ((x as object) == null || (y as object) == null)
                return true;

            return (x.Id != y.Id);
        }

        /// <summary>
        /// True if object equals the current object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AsynkQueueItem objectToCompare = obj as AsynkQueueItem;
            if (objectToCompare == null)
                return false;

            return (this == objectToCompare);
        }

        /// <summary>
        /// Returns the hashcode for the instance of this Id
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// Comparer for an AsynkQueueItem
    /// </summary>
    public class AsynkQueueEqualityComparer : IEqualityComparer<AsynkQueueItem>
    {
        #region IEqualityComparer<AsynkQueueItem> Members

        /// <summary>
        /// True if the Ids are the same for the queue item
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(AsynkQueueItem x, AsynkQueueItem y)
        {
            return (x == y);
        }

        /// <summary>
        /// Gets the HashCode (using hashcode off Id)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(AsynkQueueItem obj)
        {
            if (obj == null)
                return Guid.Empty.GetHashCode();
            else
                return obj.GetHashCode();
        }

        #endregion
    }
}
