using System.Runtime.Serialization;

namespace Library.SoapApi.Models
{
    [DataContract]
    public class Book
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public int AuthorId { get; set; }

        [DataMember]
        public int PublishingCompanyId { get; set; }

        [DataMember]
        public int PublishingYear { get; set; }
    }
}