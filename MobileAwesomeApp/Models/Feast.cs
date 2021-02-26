using System.Collections.Generic;
using MongoDB.Bson;

namespace MobileAwesomeApp.Models
{
    public class Feast
    {
        private ISet<User> _participants = new HashSet<User>();

        public ObjectId Id { get; private set; }
        public string FeastKey { get; set; }
        public User Creator { get; set; }

        public ISet<User> Participants
        {
            get => _participants;
            private set => _participants = value ?? new HashSet<User>();
        }

        public void AddParticipant(User user)
        {
            _participants.Add(user);
        }
    }
}
