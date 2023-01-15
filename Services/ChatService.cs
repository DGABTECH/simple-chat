using SimpleChat.Models;

namespace SimpleChat.Services
{
    public class ChatService : IChatService
    {
        private readonly List<ChatRoom> _chatRooms = new();

        public async Task<Guid> CreateChatRoom(string connectionID)
        {
            var newChatRoom = new ChatRoom
            {
                RoomGuid = Guid.NewGuid(),
                ConnectionID = connectionID,
                Sender = string.Empty,
                Receiver = string.Empty
            };
            _chatRooms.Add(newChatRoom);

            return await Task.FromResult(newChatRoom.RoomGuid);
        }

        public Task SetChatRoom(Guid roomGuid, string sender, string receiver)
        {
            var relatedChatRoom = _chatRooms.FirstOrDefault(cr => cr.RoomGuid == roomGuid);
            if (relatedChatRoom == null)
            {
                throw new NullReferenceException(nameof(relatedChatRoom));
            }

            relatedChatRoom.Sender = sender;
            relatedChatRoom.Receiver = receiver;

            return Task.CompletedTask;
        }

        public async Task<Guid> GetSenderChatRoomGuidBySenderConnectionID(string connectionID)
        {
            var relatedChatRoom = _chatRooms.FirstOrDefault(cr => cr.ConnectionID == connectionID);
            if (relatedChatRoom == null)
            {
                throw new NullReferenceException(nameof(relatedChatRoom));
            }

            return await Task.FromResult(relatedChatRoom.RoomGuid);
        }

        public async Task<Guid> GetReceiverChatRoomGuidBySenderConnectionID(string connectionID)
        {
            var senderChatRoom = _chatRooms.FirstOrDefault(cr => cr.ConnectionID == connectionID);
            if (senderChatRoom == null)
            {
                throw new NullReferenceException(nameof(senderChatRoom));
            }

            var receiver = senderChatRoom.Receiver;
            var sender = senderChatRoom.Sender;

            var receiverChatRoom = _chatRooms.FirstOrDefault(cr => cr.Sender == receiver && cr.Receiver == sender);
            if (receiverChatRoom == null)
            {
                throw new NullReferenceException(nameof(receiverChatRoom));
            }

            return await Task.FromResult(receiverChatRoom.RoomGuid);
        }
    }
}
