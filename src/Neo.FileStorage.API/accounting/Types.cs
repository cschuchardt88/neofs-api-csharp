// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: accounting/types.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Neo.FileStorage.API.Accounting
{

    /// <summary>Holder for reflection information generated from accounting/types.proto</summary>
    public static partial class TypesReflection
    {

        #region Descriptor
        /// <summary>File descriptor for accounting/types.proto</summary>
        public static pbr::FileDescriptor Descriptor
        {
            get { return descriptor; }
        }
        private static pbr::FileDescriptor descriptor;

        static TypesReflection()
        {
            byte[] descriptorData = global::System.Convert.FromBase64String(
                string.Concat(
                  "ChZhY2NvdW50aW5nL3R5cGVzLnByb3RvEhRuZW8uZnMudjIuYWNjb3VudGlu",
                  "ZyI9CgdEZWNpbWFsEhQKBXZhbHVlGAEgASgDUgV2YWx1ZRIcCglwcmVjaXNp",
                  "b24YAiABKA1SCXByZWNpc2lvbkJhWj9naXRodWIuY29tL25zcGNjLWRldi9u",
                  "ZW9mcy1hcGktZ28vdjIvYWNjb3VudGluZy9ncnBjO2FjY291bnRpbmeqAh1O",
                  "ZW8uRmlsZVN5c3RlbS5BUEkuQWNjb3VudGluZ2IGcHJvdG8z"));
            descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
                new pbr::FileDescriptor[] { },
                new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Neo.FileStorage.API.Accounting.Decimal), global::Neo.FileStorage.API.Accounting.Decimal.Parser, new[]{ "Value", "Precision" }, null, null, null, null)
                }));
        }
        #endregion

    }
    #region Messages
    /// <summary>
    /// Standard floating point data type can't be used in NeoFS due to inexactness
    /// of the result when doing lots of small number operations. To solve the lost
    /// precision issue, special `Decimal` format is used for monetary computations.
    ///
    /// Please see [The General Decimal Arithmetic
    /// Specification](http://speleotrove.com/decimal/) for detailed problem
    /// description.
    /// </summary>
    public sealed partial class Decimal : pb::IMessage<Decimal>
    {
        private static readonly pb::MessageParser<Decimal> _parser = new pb::MessageParser<Decimal>(() => new Decimal());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pb::MessageParser<Decimal> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pbr::MessageDescriptor Descriptor
        {
            get { return global::Neo.FileStorage.API.Accounting.TypesReflection.Descriptor.MessageTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        pbr::MessageDescriptor pb::IMessage.Descriptor
        {
            get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Decimal()
        {
            OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Decimal(Decimal other) : this()
        {
            value_ = other.value_;
            precision_ = other.precision_;
            _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Decimal Clone()
        {
            return new Decimal(this);
        }

        /// <summary>Field number for the "value" field.</summary>
        public const int ValueFieldNumber = 1;
        private long value_;
        /// <summary>
        /// Number in smallest Token fractions.
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public long Value
        {
            get { return value_; }
            set
            {
                value_ = value;
            }
        }

        /// <summary>Field number for the "precision" field.</summary>
        public const int PrecisionFieldNumber = 2;
        private uint precision_;
        /// <summary>
        /// Precision value indicating how many smallest fractions can be in one
        /// integer.
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public uint Precision
        {
            get { return precision_; }
            set
            {
                precision_ = value;
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override bool Equals(object other)
        {
            return Equals(other as Decimal);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public bool Equals(Decimal other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (Value != other.Value) return false;
            if (Precision != other.Precision) return false;
            return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override int GetHashCode()
        {
            int hash = 1;
            if (Value != 0L) hash ^= Value.GetHashCode();
            if (Precision != 0) hash ^= Precision.GetHashCode();
            if (_unknownFields != null)
            {
                hash ^= _unknownFields.GetHashCode();
            }
            return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override string ToString()
        {
            return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void WriteTo(pb::CodedOutputStream output)
        {
            if (Value != 0L)
            {
                output.WriteRawTag(8);
                output.WriteInt64(Value);
            }
            if (Precision != 0)
            {
                output.WriteRawTag(16);
                output.WriteUInt32(Precision);
            }
            if (_unknownFields != null)
            {
                _unknownFields.WriteTo(output);
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int CalculateSize()
        {
            int size = 0;
            if (Value != 0L)
            {
                size += 1 + pb::CodedOutputStream.ComputeInt64Size(Value);
            }
            if (Precision != 0)
            {
                size += 1 + pb::CodedOutputStream.ComputeUInt32Size(Precision);
            }
            if (_unknownFields != null)
            {
                size += _unknownFields.CalculateSize();
            }
            return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(Decimal other)
        {
            if (other == null)
            {
                return;
            }
            if (other.Value != 0L)
            {
                Value = other.Value;
            }
            if (other.Precision != 0)
            {
                Precision = other.Precision;
            }
            _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(pb::CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                        break;
                    case 8:
                        {
                            Value = input.ReadInt64();
                            break;
                        }
                    case 16:
                        {
                            Precision = input.ReadUInt32();
                            break;
                        }
                }
            }
        }

    }

    #endregion

}

#endregion Designer generated code
