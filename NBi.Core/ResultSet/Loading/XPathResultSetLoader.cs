﻿using NBi.Core.Query;
using NBi.Core.Xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Loading
{
    class XPathResultSetLoader : IResultSetLoader
    {
        private readonly XPathEngine xpath;

        public XPathResultSetLoader(XPathEngine xpath)
        {
            this.xpath = xpath;
        }

        public virtual ResultSet Execute()
        {
            var objects = xpath.Execute();

            var helper = new ObjectsToRowsHelper();
            var rows = helper.Execute(objects);

            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }
    }
}
