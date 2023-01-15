using System.Globalization;

namespace SimpleChat.Services
{
    public interface IChatService
    {
        Task<Guid> CreateChatRoom(string connectionID);

        Task SetChatRoom(Guid roomGuid, string sender, string receiver);

        Task<Guid> GetSenderChatRoomGuidBySenderConnectionID(string connectionID);

        Task<Guid> GetReceiverChatRoomGuidBySenderConnectionID(string connectionID);
    }
}
