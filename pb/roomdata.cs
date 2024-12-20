// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: RoomData.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace RS.WechatFerry.pb {

  /// <summary>Holder for reflection information generated from RoomData.proto</summary>
  public static partial class RoomDataReflection {

    #region Descriptor
    /// <summary>File descriptor for RoomData.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static RoomDataReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5Sb29tRGF0YS5wcm90bxIHd2NmZXJyeSL3AQoIUm9vbURhdGESLQoHbWVt",
            "YmVycxgBIAMoCzIcLndjZmVycnkuUm9vbURhdGEuUm9vbU1lbWJlchIPCgdm",
            "aWVsZF8yGAIgASgFEg8KB2ZpZWxkXzMYAyABKAUSDwoHZmllbGRfNBgEIAEo",
            "BRIVCg1yb29tX2NhcGFjaXR5GAUgASgFEg8KB2ZpZWxkXzYYBiABKAUSEwoH",
            "ZmllbGRfNxgHIAEoA0ICMAESEwoHZmllbGRfOBgIIAEoA0ICMAEaNwoKUm9v",
            "bU1lbWJlchIMCgR3eGlkGAEgASgJEgwKBG5hbWUYAiABKAkSDQoFc3RhdGUY",
            "AyABKAVCDFoKLi87d2NmZXJyeWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::RS.WechatFerry.pb.RoomData), 
                                          global::RS.WechatFerry.pb.RoomData.Parser, 
                                          new[]{ "Members", "Field2", "Field3", "Field4", "RoomCapacity", "Field6", "Field7", "Field8" }, 
                                          null, 
                                          null, 
                                          null, 
                                          new pbr::GeneratedClrTypeInfo[] { new pbr::GeneratedClrTypeInfo(typeof(global :: RS.WechatFerry.pb.RoomData.Types.RoomMember), 
                                                                            global::RS.WechatFerry.pb.RoomData.Types.RoomMember.Parser, 
                                                                            new[]{ "Wxid", "Name", "State" }, 
                                                                            null, 
                                                                            null,
                                                                            null, 
                                                                            null)
                                          })
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class RoomData : pb::IMessage<RoomData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RoomData> _parser = new pb::MessageParser<RoomData>(() => new RoomData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<RoomData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::RS.WechatFerry.pb.RoomDataReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RoomData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RoomData(RoomData other) : this() {
      members_ = other.members_.Clone();
      field2_ = other.field2_;
      field3_ = other.field3_;
      field4_ = other.field4_;
      roomCapacity_ = other.roomCapacity_;
      field6_ = other.field6_;
      field7_ = other.field7_;
      field8_ = other.field8_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public RoomData Clone() {
      return new RoomData(this);
    }

    /// <summary>Field number for the "members" field.</summary>
    public const int MembersFieldNumber = 1;
    private static readonly pb::FieldCodec<global::RS.WechatFerry.pb.RoomData.Types.RoomMember> _repeated_members_codec
        = pb::FieldCodec.ForMessage(10, global::RS.WechatFerry.pb.RoomData.Types.RoomMember.Parser);
    private readonly pbc::RepeatedField<global::RS.WechatFerry.pb.RoomData.Types.RoomMember> members_ = new pbc::RepeatedField<global::RS.WechatFerry.pb.RoomData.Types.RoomMember>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::RS.WechatFerry.pb.RoomData.Types.RoomMember> Members {
      get { return members_; }
    }

    /// <summary>Field number for the "field_2" field.</summary>
    public const int Field2FieldNumber = 2;
    private int field2_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Field2 {
      get { return field2_; }
      set {
        field2_ = value;
      }
    }

    /// <summary>Field number for the "field_3" field.</summary>
    public const int Field3FieldNumber = 3;
    private int field3_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Field3 {
      get { return field3_; }
      set {
        field3_ = value;
      }
    }

    /// <summary>Field number for the "field_4" field.</summary>
    public const int Field4FieldNumber = 4;
    private int field4_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Field4 {
      get { return field4_; }
      set {
        field4_ = value;
      }
    }

    /// <summary>Field number for the "room_capacity" field.</summary>
    public const int RoomCapacityFieldNumber = 5;
    private int roomCapacity_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int RoomCapacity {
      get { return roomCapacity_; }
      set {
        roomCapacity_ = value;
      }
    }

    /// <summary>Field number for the "field_6" field.</summary>
    public const int Field6FieldNumber = 6;
    private int field6_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Field6 {
      get { return field6_; }
      set {
        field6_ = value;
      }
    }

    /// <summary>Field number for the "field_7" field.</summary>
    public const int Field7FieldNumber = 7;
    private long field7_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long Field7 {
      get { return field7_; }
      set {
        field7_ = value;
      }
    }

    /// <summary>Field number for the "field_8" field.</summary>
    public const int Field8FieldNumber = 8;
    private long field8_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public long Field8 {
      get { return field8_; }
      set {
        field8_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as RoomData);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(RoomData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!members_.Equals(other.members_)) return false;
      if (Field2 != other.Field2) return false;
      if (Field3 != other.Field3) return false;
      if (Field4 != other.Field4) return false;
      if (RoomCapacity != other.RoomCapacity) return false;
      if (Field6 != other.Field6) return false;
      if (Field7 != other.Field7) return false;
      if (Field8 != other.Field8) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= members_.GetHashCode();
      if (Field2 != 0) hash ^= Field2.GetHashCode();
      if (Field3 != 0) hash ^= Field3.GetHashCode();
      if (Field4 != 0) hash ^= Field4.GetHashCode();
      if (RoomCapacity != 0) hash ^= RoomCapacity.GetHashCode();
      if (Field6 != 0) hash ^= Field6.GetHashCode();
      if (Field7 != 0L) hash ^= Field7.GetHashCode();
      if (Field8 != 0L) hash ^= Field8.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      members_.WriteTo(output, _repeated_members_codec);
      if (Field2 != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Field2);
      }
      if (Field3 != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Field3);
      }
      if (Field4 != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(Field4);
      }
      if (RoomCapacity != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(RoomCapacity);
      }
      if (Field6 != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(Field6);
      }
      if (Field7 != 0L) {
        output.WriteRawTag(56);
        output.WriteInt64(Field7);
      }
      if (Field8 != 0L) {
        output.WriteRawTag(64);
        output.WriteInt64(Field8);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      members_.WriteTo(ref output, _repeated_members_codec);
      if (Field2 != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Field2);
      }
      if (Field3 != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Field3);
      }
      if (Field4 != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(Field4);
      }
      if (RoomCapacity != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(RoomCapacity);
      }
      if (Field6 != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(Field6);
      }
      if (Field7 != 0L) {
        output.WriteRawTag(56);
        output.WriteInt64(Field7);
      }
      if (Field8 != 0L) {
        output.WriteRawTag(64);
        output.WriteInt64(Field8);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      size += members_.CalculateSize(_repeated_members_codec);
      if (Field2 != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Field2);
      }
      if (Field3 != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Field3);
      }
      if (Field4 != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Field4);
      }
      if (RoomCapacity != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoomCapacity);
      }
      if (Field6 != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Field6);
      }
      if (Field7 != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Field7);
      }
      if (Field8 != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Field8);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(RoomData other) {
      if (other == null) {
        return;
      }
      members_.Add(other.members_);
      if (other.Field2 != 0) {
        Field2 = other.Field2;
      }
      if (other.Field3 != 0) {
        Field3 = other.Field3;
      }
      if (other.Field4 != 0) {
        Field4 = other.Field4;
      }
      if (other.RoomCapacity != 0) {
        RoomCapacity = other.RoomCapacity;
      }
      if (other.Field6 != 0) {
        Field6 = other.Field6;
      }
      if (other.Field7 != 0L) {
        Field7 = other.Field7;
      }
      if (other.Field8 != 0L) {
        Field8 = other.Field8;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            members_.AddEntriesFrom(input, _repeated_members_codec);
            break;
          }
          case 16: {
            Field2 = input.ReadInt32();
            break;
          }
          case 24: {
            Field3 = input.ReadInt32();
            break;
          }
          case 32: {
            Field4 = input.ReadInt32();
            break;
          }
          case 40: {
            RoomCapacity = input.ReadInt32();
            break;
          }
          case 48: {
            Field6 = input.ReadInt32();
            break;
          }
          case 56: {
            Field7 = input.ReadInt64();
            break;
          }
          case 64: {
            Field8 = input.ReadInt64();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            members_.AddEntriesFrom(ref input, _repeated_members_codec);
            break;
          }
          case 16: {
            Field2 = input.ReadInt32();
            break;
          }
          case 24: {
            Field3 = input.ReadInt32();
            break;
          }
          case 32: {
            Field4 = input.ReadInt32();
            break;
          }
          case 40: {
            RoomCapacity = input.ReadInt32();
            break;
          }
          case 48: {
            Field6 = input.ReadInt32();
            break;
          }
          case 56: {
            Field7 = input.ReadInt64();
            break;
          }
          case 64: {
            Field8 = input.ReadInt64();
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the RoomData message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static partial class Types {
      [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
      public sealed partial class RoomMember : pb::IMessage<RoomMember>
      #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
          , pb::IBufferMessage
      #endif
      {
        private static readonly pb::MessageParser<RoomMember> _parser = new pb::MessageParser<RoomMember>(() => new RoomMember());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public static pb::MessageParser<RoomMember> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public static pbr::MessageDescriptor Descriptor {
          get { return global::RS.WechatFerry.pb.RoomData.Descriptor.NestedTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public RoomMember() {
          OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public RoomMember(RoomMember other) : this() {
          wxid_ = other.wxid_;
          name_ = other.name_;
          state_ = other.state_;
          _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public RoomMember Clone() {
          return new RoomMember(this);
        }

        /// <summary>Field number for the "wxid" field.</summary>
        public const int WxidFieldNumber = 1;
        private string wxid_ = "";
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public string Wxid {
          get { return wxid_; }
          set {
            wxid_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
          }
        }

        /// <summary>Field number for the "name" field.</summary>
        public const int NameFieldNumber = 2;
        private string name_ = "";
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public string Name {
          get { return name_; }
          set {
            name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
          }
        }

        /// <summary>Field number for the "state" field.</summary>
        public const int StateFieldNumber = 3;
        private int state_;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public int State {
          get { return state_; }
          set {
            state_ = value;
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public override bool Equals(object other) {
          return Equals(other as RoomMember);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public bool Equals(RoomMember other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          if (Wxid != other.Wxid) return false;
          if (Name != other.Name) return false;
          if (State != other.State) return false;
          return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public override int GetHashCode() {
          int hash = 1;
          if (Wxid.Length != 0) hash ^= Wxid.GetHashCode();
          if (Name.Length != 0) hash ^= Name.GetHashCode();
          if (State != 0) hash ^= State.GetHashCode();
          if (_unknownFields != null) {
            hash ^= _unknownFields.GetHashCode();
          }
          return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public override string ToString() {
          return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public void WriteTo(pb::CodedOutputStream output) {
        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
          output.WriteRawMessage(this);
        #else
          if (Wxid.Length != 0) {
            output.WriteRawTag(10);
            output.WriteString(Wxid);
          }
          if (Name.Length != 0) {
            output.WriteRawTag(18);
            output.WriteString(Name);
          }
          if (State != 0) {
            output.WriteRawTag(24);
            output.WriteInt32(State);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(output);
          }
        #endif
        }

        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
          if (Wxid.Length != 0) {
            output.WriteRawTag(10);
            output.WriteString(Wxid);
          }
          if (Name.Length != 0) {
            output.WriteRawTag(18);
            output.WriteString(Name);
          }
          if (State != 0) {
            output.WriteRawTag(24);
            output.WriteInt32(State);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(ref output);
          }
        }
        #endif

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public int CalculateSize() {
          int size = 0;
          if (Wxid.Length != 0) {
            size += 1 + pb::CodedOutputStream.ComputeStringSize(Wxid);
          }
          if (Name.Length != 0) {
            size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
          }
          if (State != 0) {
            size += 1 + pb::CodedOutputStream.ComputeInt32Size(State);
          }
          if (_unknownFields != null) {
            size += _unknownFields.CalculateSize();
          }
          return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public void MergeFrom(RoomMember other) {
          if (other == null) {
            return;
          }
          if (other.Wxid.Length != 0) {
            Wxid = other.Wxid;
          }
          if (other.Name.Length != 0) {
            Name = other.Name;
          }
          if (other.State != 0) {
            State = other.State;
          }
          _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        public void MergeFrom(pb::CodedInputStream input) {
        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
          input.ReadRawMessage(this);
        #else
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                break;
              case 10: {
                Wxid = input.ReadString();
                break;
              }
              case 18: {
                Name = input.ReadString();
                break;
              }
              case 24: {
                State = input.ReadInt32();
                break;
              }
            }
          }
        #endif
        }

        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
        void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
                break;
              case 10: {
                Wxid = input.ReadString();
                break;
              }
              case 18: {
                Name = input.ReadString();
                break;
              }
              case 24: {
                State = input.ReadInt32();
                break;
              }
            }
          }
        }
        #endif

      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
