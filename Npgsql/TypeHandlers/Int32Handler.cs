﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql.Messages;

namespace Npgsql.TypeHandlers
{
    internal class Int32Handler : SimpleTypeHandler
    {
        static readonly string[] _pgNames = { "int4" };
        internal override string[] PgNames { get { return _pgNames; } }
        internal override bool SupportsBinaryRead { get { return true; } }
        internal override Type FieldType { get { return typeof(int); } }

        internal override void ReadText(NpgsqlBufferedStream buf, int len, FieldDescription field, NpgsqlValue output)
        {
            output.SetTo(Int32.Parse(buf.ReadString(len)));
        }

        internal override void ReadBinary(NpgsqlBufferedStream buf, int len, FieldDescription field, NpgsqlValue output)
        {
            output.SetTo(buf.ReadInt32());
        }
    }
}