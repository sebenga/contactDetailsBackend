using MongoDB.Bson;

namespace contactDetails.Models
{
    public class Contact
    {
        public ObjectId Id { get; set; }
        public int ContactId { get; set; }
        public  string Name  { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Notes { get; set; }

    }
}
