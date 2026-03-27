using System.Runtime.Serialization;

namespace Library.SoapApi.Models
{
    [DataContract]
    public class PublishingCompany
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}