using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Apassos.Common
{
    public class AcessosException : Exception
    {
        public AcessosException()
            : base() { }

        public AcessosException(string message)
            : base(message) { }

        public AcessosException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public AcessosException(string message, AcessosException innerException)
            : base(message, innerException) { }

        public AcessosException(string format, AcessosException innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected AcessosException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}