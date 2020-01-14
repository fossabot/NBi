﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using NBi.Core.DataSerialization;
using NBi.Core.ResultSet;

namespace NBi.Core.DataSerialization.Flattening
{
    public abstract class PathFlattenizer : IDataSerializationFlattenizer
    {
        protected IEnumerable<IPathSelect> Selects { get; }
        protected string From { get; }

        protected PathFlattenizer(string from, IEnumerable<IPathSelect> selects)
            => (From, Selects) = (from, selects);

        public abstract IEnumerable<object> Execute(TextReader textReader);
    }
}
