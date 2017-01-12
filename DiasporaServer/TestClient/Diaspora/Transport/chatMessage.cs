// automatically generated by the FlatBuffers compiler, do not modify

namespace Diaspora.Transport
{

using System;
using FlatBuffers;

public struct ChatMessage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static ChatMessage GetRootAsChatMessage(ByteBuffer _bb) { return GetRootAsChatMessage(_bb, new ChatMessage()); }
  public static ChatMessage GetRootAsChatMessage(ByteBuffer _bb, ChatMessage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public ChatMessage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Name { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(4); }
  public string Message { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetMessageBytes() { return __p.__vector_as_arraysegment(6); }

  public static Offset<ChatMessage> CreateChatMessage(FlatBufferBuilder builder,
      StringOffset nameOffset = default(StringOffset),
      StringOffset messageOffset = default(StringOffset)) {
    builder.StartObject(2);
    ChatMessage.AddMessage(builder, messageOffset);
    ChatMessage.AddName(builder, nameOffset);
    return ChatMessage.EndChatMessage(builder);
  }

  public static void StartChatMessage(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(0, nameOffset.Value, 0); }
  public static void AddMessage(FlatBufferBuilder builder, StringOffset messageOffset) { builder.AddOffset(1, messageOffset.Value, 0); }
  public static Offset<ChatMessage> EndChatMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<ChatMessage>(o);
  }
  public static void FinishChatMessageBuffer(FlatBufferBuilder builder, Offset<ChatMessage> offset) { builder.Finish(offset.Value); }
};


}