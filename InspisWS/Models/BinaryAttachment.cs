using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace InspisWS
{
    public class BinaryAttachment
    {
        /// <summary>
        /// Puvodni nazev souboru
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Stream
        /// </summary>
        public byte[] BinaryData { get; set; }

        /// <summary>
        /// MIME type
        /// </summary>
        public string ContentType { get; set; }
    }
}