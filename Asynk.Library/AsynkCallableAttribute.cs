using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asynk.Library
{
    /// <summary>
    /// Used to decorate methods that can be called in an 
    /// asynchronous manner
    /// </summary>
    /// <example>
    /// [AsynkCallable]
    /// public void foo(string arg) {}
    /// </example>
    [AttributeUsage(AttributeTargets.Method)]
    public class AsynkCallableAttribute : Attribute
    {
        /// <summary>
        /// Default constructor for the AsynkCallableAttribute
        /// </summary>
        public AsynkCallableAttribute() : base() 
        {
            _category = null;
        }

        /// <summary>
        /// Constructs a AsynkCallable Attribute with an optional category
        /// </summary>
        /// <param name="category"></param>
        public AsynkCallableAttribute(string category)
            : base()
        {
            _category = category;
        }

        private string _category;
    }
}
