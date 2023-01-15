namespace SimpleChat.Models
{
    public class ChatRoom
    {
        public Guid RoomGuid { get; set; }

        public required string ConnectionID { get; set; }

        public required string Sender { get; set; }

        public required string Receiver { get; set; }
    }
}
