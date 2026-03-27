using System.Runtime.Serialization;

namespace Library.SoapApi.Models
{
    [DataContract]
    public class Author
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Surname { get; set; }
    }
}