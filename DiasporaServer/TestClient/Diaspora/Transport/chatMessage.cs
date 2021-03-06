// automatically generated by the FlatBuffers compiler, do not modify

namespace Diaspora.Transport
{

using System;
using FlatBuffers;

public enum MessageType : sbyte
{
 Chat = 0,
 Move = 1,
};

public struct ChatMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static ChatMessage GetRootAsChatMessage(ByteBuffer _bb) { return GetRootAsChatMessage(_bb, new ChatMessage()); }
  public static ChatMessage GetRootAsChatMessage(ByteBuffer _bb, ChatMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public ChatMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public Header? MHeader { get { int o = __p.__offset(4); return o != 0 ? (Header?)(new Header()).__assign(__p.__indirect(o + __p.bb_pos), __p.bb) : null; } }
  public string Name { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(6); }
  public string Message { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetMessageBytes() { return __p.__vector_as_arraysegment(8); }

  public static Offset<ChatMessage> CreateChatMessage(FlatBufferBuilder builder,
      Offset<Header> mHeaderOffset = default(Offset<Header>),
      StringOffset nameOffset = default(StringOffset),
      StringOffset messageOffset = default(StringOffset)) {
    builder.StartObject(3);
    ChatMessage.AddMessage(builder, messageOffset);
    ChatMessage.AddName(builder, nameOffset);
    ChatMessage.AddMHeader(builder, mHeaderOffset);
    return ChatMessage.EndChatMessage(builder);
  }

  public static void StartChatMessage(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddMHeader(FlatBufferBuilder builder, Offset<Header> mHeaderOffset) { builder.AddOffset(0, mHeaderOffset.Value, 0); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(1, nameOffset.Value, 0); }
  public static void AddMessage(FlatBufferBuilder builder, StringOffset messageOffset) { builder.AddOffset(2, messageOffset.Value, 0); }
  public static Offset<ChatMessage> EndChatMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<ChatMessage>(o);
  }
  public static void FinishChatMessageBuffer(FlatBufferBuilder builder, Offset<ChatMessage> offset) { builder.Finish(offset.Value); }
};

public struct Header : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Header GetRootAsHeader(ByteBuffer _bb) { return GetRootAsHeader(_bb, new Header()); }
  public static Header GetRootAsHeader(ByteBuffer _bb, Header obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Header __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string InterestID { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetInterestIDBytes() { return __p.__vector_as_arraysegment(4); }
  public string RegionID { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetRegionIDBytes() { return __p.__vector_as_arraysegment(6); }
  public MessageType MType { get { int o = __p.__offset(8); return o != 0 ? (MessageType)__p.bb.GetSbyte(o + __p.bb_pos) : MessageType.Chat; } }

  public static Offset<Header> CreateHeader(FlatBufferBuilder builder,
      StringOffset InterestIDOffset = default(StringOffset),
      StringOffset RegionIDOffset = default(StringOffset),
      MessageType mType = MessageType.Chat) {
    builder.StartObject(3);
    Header.AddRegionID(builder, RegionIDOffset);
    Header.AddInterestID(builder, InterestIDOffset);
    Header.AddMType(builder, mType);
    return Header.EndHeader(builder);
  }

  public static void StartHeader(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddInterestID(FlatBufferBuilder builder, StringOffset InterestIDOffset) { builder.AddOffset(0, InterestIDOffset.Value, 0); }
  public static void AddRegionID(FlatBufferBuilder builder, StringOffset RegionIDOffset) { builder.AddOffset(1, RegionIDOffset.Value, 0); }
  public static void AddMType(FlatBufferBuilder builder, MessageType mType) { builder.AddSbyte(2, (sbyte)mType, 0); }
  public static Offset<Header> EndHeader(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Header>(o);
  }
};


}
