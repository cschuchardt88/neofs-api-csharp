// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: session/service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace NeoFS.API.Session {

  /// <summary>Holder for reflection information generated from session/service.proto</summary>
  public static partial class ServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for session/service.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVzZXNzaW9uL3NlcnZpY2UucHJvdG8SB3Nlc3Npb24aE3Nlc3Npb24vdHlw",
            "ZXMucHJvdG8aEnNlcnZpY2UvbWV0YS5wcm90bxoUc2VydmljZS92ZXJpZnku",
            "cHJvdG8aLWdpdGh1Yi5jb20vZ29nby9wcm90b2J1Zi9nb2dvcHJvdG8vZ29n",
            "by5wcm90byLOAQoNQ3JlYXRlUmVxdWVzdBIeCgRJbml0GAEgASgLMg4uc2Vz",
            "c2lvbi5Ub2tlbkgAEiAKBlNpZ25lZBgCIAEoCzIOLnNlc3Npb24uVG9rZW5I",
            "ABIyCgRNZXRhGGIgASgLMhouc2VydmljZS5SZXF1ZXN0TWV0YUhlYWRlckII",
            "0N4fAcjeHwASPAoGVmVyaWZ5GGMgASgLMiIuc2VydmljZS5SZXF1ZXN0VmVy",
            "aWZpY2F0aW9uSGVhZGVyQgjQ3h8ByN4fAEIJCgdNZXNzYWdlImEKDkNyZWF0",
            "ZVJlc3BvbnNlEiIKCFVuc2lnbmVkGAEgASgLMg4uc2Vzc2lvbi5Ub2tlbkgA",
            "EiAKBlJlc3VsdBgCIAEoCzIOLnNlc3Npb24uVG9rZW5IAEIJCgdNZXNzYWdl",
            "MkgKB1Nlc3Npb24SPQoGQ3JlYXRlEhYuc2Vzc2lvbi5DcmVhdGVSZXF1ZXN0",
            "Ghcuc2Vzc2lvbi5DcmVhdGVSZXNwb25zZSgBMAFCQ1opZ2l0aHViLmNvbS9u",
            "c3BjYy1kZXYvbmVvZnMtYXBpLWdvL3Nlc3Npb26qAhFOZW9GUy5BUEkuU2Vz",
            "c2lvbtjiHgFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::NeoFS.API.Session.TypesReflection.Descriptor, global::NeoFS.API.Service.MetaReflection.Descriptor, global::NeoFS.API.Service.VerifyReflection.Descriptor, global::Gogoproto.GogoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::NeoFS.API.Session.CreateRequest), global::NeoFS.API.Session.CreateRequest.Parser, new[]{ "Init", "Signed", "Meta", "Verify" }, new[]{ "Message" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::NeoFS.API.Session.CreateResponse), global::NeoFS.API.Session.CreateResponse.Parser, new[]{ "Unsigned", "Result" }, new[]{ "Message" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class CreateRequest : pb::IMessage<CreateRequest> {
    private static readonly pb::MessageParser<CreateRequest> _parser = new pb::MessageParser<CreateRequest>(() => new CreateRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CreateRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::NeoFS.API.Session.ServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateRequest(CreateRequest other) : this() {
      meta_ = other.meta_ != null ? other.meta_.Clone() : null;
      verify_ = other.verify_ != null ? other.verify_.Clone() : null;
      switch (other.MessageCase) {
        case MessageOneofCase.Init:
          Init = other.Init.Clone();
          break;
        case MessageOneofCase.Signed:
          Signed = other.Signed.Clone();
          break;
      }

      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateRequest Clone() {
      return new CreateRequest(this);
    }

    /// <summary>Field number for the "Init" field.</summary>
    public const int InitFieldNumber = 1;
    /// <summary>
    /// Init is a message to initialize session opening. Carry:
    /// owner of manipulation object;
    /// ID of manipulation object;
    /// token lifetime bounds.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::NeoFS.API.Session.Token Init {
      get { return messageCase_ == MessageOneofCase.Init ? (global::NeoFS.API.Session.Token) message_ : null; }
      set {
        message_ = value;
        messageCase_ = value == null ? MessageOneofCase.None : MessageOneofCase.Init;
      }
    }

    /// <summary>Field number for the "Signed" field.</summary>
    public const int SignedFieldNumber = 2;
    /// <summary>
    /// Signed Init message response (Unsigned) from server with user private key
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::NeoFS.API.Session.Token Signed {
      get { return messageCase_ == MessageOneofCase.Signed ? (global::NeoFS.API.Session.Token) message_ : null; }
      set {
        message_ = value;
        messageCase_ = value == null ? MessageOneofCase.None : MessageOneofCase.Signed;
      }
    }

    /// <summary>Field number for the "Meta" field.</summary>
    public const int MetaFieldNumber = 98;
    private global::NeoFS.API.Service.RequestMetaHeader meta_;
    /// <summary>
    /// RequestMetaHeader contains information about request meta headers (should be embedded into message)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::NeoFS.API.Service.RequestMetaHeader Meta {
      get { return meta_; }
      set {
        meta_ = value;
      }
    }

    /// <summary>Field number for the "Verify" field.</summary>
    public const int VerifyFieldNumber = 99;
    private global::NeoFS.API.Service.RequestVerificationHeader verify_;
    /// <summary>
    /// RequestVerificationHeader is a set of signatures of every NeoFS Node that processed request (should be embedded into message)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::NeoFS.API.Service.RequestVerificationHeader Verify {
      get { return verify_; }
      set {
        verify_ = value;
      }
    }

    private object message_;
    /// <summary>Enum of possible cases for the "Message" oneof.</summary>
    public enum MessageOneofCase {
      None = 0,
      Init = 1,
      Signed = 2,
    }
    private MessageOneofCase messageCase_ = MessageOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MessageOneofCase MessageCase {
      get { return messageCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMessage() {
      messageCase_ = MessageOneofCase.None;
      message_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CreateRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CreateRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Init, other.Init)) return false;
      if (!object.Equals(Signed, other.Signed)) return false;
      if (!object.Equals(Meta, other.Meta)) return false;
      if (!object.Equals(Verify, other.Verify)) return false;
      if (MessageCase != other.MessageCase) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (messageCase_ == MessageOneofCase.Init) hash ^= Init.GetHashCode();
      if (messageCase_ == MessageOneofCase.Signed) hash ^= Signed.GetHashCode();
      if (meta_ != null) hash ^= Meta.GetHashCode();
      if (verify_ != null) hash ^= Verify.GetHashCode();
      hash ^= (int) messageCase_;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (messageCase_ == MessageOneofCase.Init) {
        output.WriteRawTag(10);
        output.WriteMessage(Init);
      }
      if (messageCase_ == MessageOneofCase.Signed) {
        output.WriteRawTag(18);
        output.WriteMessage(Signed);
      }
      if (meta_ != null) {
        output.WriteRawTag(146, 6);
        output.WriteMessage(Meta);
      }
      if (verify_ != null) {
        output.WriteRawTag(154, 6);
        output.WriteMessage(Verify);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (messageCase_ == MessageOneofCase.Init) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Init);
      }
      if (messageCase_ == MessageOneofCase.Signed) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Signed);
      }
      if (meta_ != null) {
        size += 2 + pb::CodedOutputStream.ComputeMessageSize(Meta);
      }
      if (verify_ != null) {
        size += 2 + pb::CodedOutputStream.ComputeMessageSize(Verify);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CreateRequest other) {
      if (other == null) {
        return;
      }
      if (other.meta_ != null) {
        if (meta_ == null) {
          Meta = new global::NeoFS.API.Service.RequestMetaHeader();
        }
        Meta.MergeFrom(other.Meta);
      }
      if (other.verify_ != null) {
        if (verify_ == null) {
          Verify = new global::NeoFS.API.Service.RequestVerificationHeader();
        }
        Verify.MergeFrom(other.Verify);
      }
      switch (other.MessageCase) {
        case MessageOneofCase.Init:
          if (Init == null) {
            Init = new global::NeoFS.API.Session.Token();
          }
          Init.MergeFrom(other.Init);
          break;
        case MessageOneofCase.Signed:
          if (Signed == null) {
            Signed = new global::NeoFS.API.Session.Token();
          }
          Signed.MergeFrom(other.Signed);
          break;
      }

      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            global::NeoFS.API.Session.Token subBuilder = new global::NeoFS.API.Session.Token();
            if (messageCase_ == MessageOneofCase.Init) {
              subBuilder.MergeFrom(Init);
            }
            input.ReadMessage(subBuilder);
            Init = subBuilder;
            break;
          }
          case 18: {
            global::NeoFS.API.Session.Token subBuilder = new global::NeoFS.API.Session.Token();
            if (messageCase_ == MessageOneofCase.Signed) {
              subBuilder.MergeFrom(Signed);
            }
            input.ReadMessage(subBuilder);
            Signed = subBuilder;
            break;
          }
          case 786: {
            if (meta_ == null) {
              Meta = new global::NeoFS.API.Service.RequestMetaHeader();
            }
            input.ReadMessage(Meta);
            break;
          }
          case 794: {
            if (verify_ == null) {
              Verify = new global::NeoFS.API.Service.RequestVerificationHeader();
            }
            input.ReadMessage(Verify);
            break;
          }
        }
      }
    }

  }

  public sealed partial class CreateResponse : pb::IMessage<CreateResponse> {
    private static readonly pb::MessageParser<CreateResponse> _parser = new pb::MessageParser<CreateResponse>(() => new CreateResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CreateResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::NeoFS.API.Session.ServiceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateResponse(CreateResponse other) : this() {
      switch (other.MessageCase) {
        case MessageOneofCase.Unsigned:
          Unsigned = other.Unsigned.Clone();
          break;
        case MessageOneofCase.Result:
          Result = other.Result.Clone();
          break;
      }

      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateResponse Clone() {
      return new CreateResponse(this);
    }

    /// <summary>Field number for the "Unsigned" field.</summary>
    public const int UnsignedFieldNumber = 1;
    /// <summary>
    /// Unsigned token with token ID and session public key generated on server side
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::NeoFS.API.Session.Token Unsigned {
      get { return messageCase_ == MessageOneofCase.Unsigned ? (global::NeoFS.API.Session.Token) message_ : null; }
      set {
        message_ = value;
        messageCase_ = value == null ? MessageOneofCase.None : MessageOneofCase.Unsigned;
      }
    }

    /// <summary>Field number for the "Result" field.</summary>
    public const int ResultFieldNumber = 2;
    /// <summary>
    /// Result is a resulting token which can be used for object placing through an trusted intermediary
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::NeoFS.API.Session.Token Result {
      get { return messageCase_ == MessageOneofCase.Result ? (global::NeoFS.API.Session.Token) message_ : null; }
      set {
        message_ = value;
        messageCase_ = value == null ? MessageOneofCase.None : MessageOneofCase.Result;
      }
    }

    private object message_;
    /// <summary>Enum of possible cases for the "Message" oneof.</summary>
    public enum MessageOneofCase {
      None = 0,
      Unsigned = 1,
      Result = 2,
    }
    private MessageOneofCase messageCase_ = MessageOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MessageOneofCase MessageCase {
      get { return messageCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMessage() {
      messageCase_ = MessageOneofCase.None;
      message_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CreateResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CreateResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Unsigned, other.Unsigned)) return false;
      if (!object.Equals(Result, other.Result)) return false;
      if (MessageCase != other.MessageCase) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (messageCase_ == MessageOneofCase.Unsigned) hash ^= Unsigned.GetHashCode();
      if (messageCase_ == MessageOneofCase.Result) hash ^= Result.GetHashCode();
      hash ^= (int) messageCase_;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (messageCase_ == MessageOneofCase.Unsigned) {
        output.WriteRawTag(10);
        output.WriteMessage(Unsigned);
      }
      if (messageCase_ == MessageOneofCase.Result) {
        output.WriteRawTag(18);
        output.WriteMessage(Result);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (messageCase_ == MessageOneofCase.Unsigned) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Unsigned);
      }
      if (messageCase_ == MessageOneofCase.Result) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Result);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CreateResponse other) {
      if (other == null) {
        return;
      }
      switch (other.MessageCase) {
        case MessageOneofCase.Unsigned:
          if (Unsigned == null) {
            Unsigned = new global::NeoFS.API.Session.Token();
          }
          Unsigned.MergeFrom(other.Unsigned);
          break;
        case MessageOneofCase.Result:
          if (Result == null) {
            Result = new global::NeoFS.API.Session.Token();
          }
          Result.MergeFrom(other.Result);
          break;
      }

      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            global::NeoFS.API.Session.Token subBuilder = new global::NeoFS.API.Session.Token();
            if (messageCase_ == MessageOneofCase.Unsigned) {
              subBuilder.MergeFrom(Unsigned);
            }
            input.ReadMessage(subBuilder);
            Unsigned = subBuilder;
            break;
          }
          case 18: {
            global::NeoFS.API.Session.Token subBuilder = new global::NeoFS.API.Session.Token();
            if (messageCase_ == MessageOneofCase.Result) {
              subBuilder.MergeFrom(Result);
            }
            input.ReadMessage(subBuilder);
            Result = subBuilder;
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
