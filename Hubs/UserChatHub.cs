using Microsoft.AspNetCore.SignalR;
using SimpleChat.Services;

namespace SimpleChat.Hubs
{
    public class UserChatHub : Hub
    {
        private readonly IChatService _chatService;

        public UserChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var connectionID = Context.ConnectionId;
            var chatRoomGuid = await _chatService.CreateChatRoom(connectionID);
            string chatRoomName = chatRoomGuid.ToString();
            await Groups.AddToGroupAsync(connectionID, chatRoomName);

            await base.OnConnectedAsync();
        }

        public async Task SetChatRoomAsync(string senderName, string receicerName)
        {
            var connectionID = Context.ConnectionId;
            var chatRoomGuid = await _chatService.GetSenderChatRoomGuidBySenderConnectionID(connectionID);
            await _chatService.SetChatRoom(chatRoomGuid, senderName, receicerName);
        }

        public async Task SendMessageToUserAsync(string sender, string message)
        {
            var connectionID = Context.ConnectionId;
            var senderChatRoomGuid = await _chatService.GetSenderChatRoomGuidBySenderConnectionID(connectionID);
            var receiverChatRoomGuid = await _chatService.GetReceiverChatRoomGuidBySenderConnectionID(connectionID);
            string senderChatRoomName = senderChatRoomGuid.ToString();
            string receiverChatRoomName = receiverChatRoomGuid.ToString();
            await Clients.Groups(senderChatRoomName, receiverChatRoomName).SendAsync("receiveMessage", sender, message, DateTime.Now.ToString("HH:mm"));
        }
    }
}
