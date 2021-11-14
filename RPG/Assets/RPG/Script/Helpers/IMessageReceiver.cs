using System;

namespace RPG
{
    public enum MessageType
      {
         DAMAGED,
         DEAD      
      }
    public interface IMessageReceiver
    {
       void OneReceiveMessage(MessageType type);
    
    }

}
