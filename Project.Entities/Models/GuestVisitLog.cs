using Project.Entities.Enums;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class GuestVisitLog:BaseEntity,IIdentifiablePerson
    {
        public int CustomerId { get; set; }         // Misafiri getiren müşteri (rezervasyon sahibi)
        public int RoomId { get; set; }             // Hangi odada kaldı

        public string FirstName { get; set; } = null!; //Misafirin İsmi
        public string LastName { get; set; } = null!; //Misafirin Soyadı
        public string IdentityNumber { get; set; } = null!; // Misafirin Tc Kimlik Numarası
        public string PhoneNumber { get; set; } = null!;    

        public string? GuestNationality { get; set; }     // 💡 Uyruk (örn: Turkey, Germany)
        public DateTime BirthDate { get; set; }          // 💡 Yaş kontrolü ve valilik bildirimi için

        public DateTime EntryDate { get; set; }     // Giriş tarihi
        public DateTime? ExitDate { get; set; }     // Çıkış tarihi (null olabilir)

        public GuestVisitStatus GuestVisitStatus { get; set; }    // Misafirin oda kullanım durumu

        //relational properties
        public virtual Customer Customer { get; set; } = null!; // Misafir eden müşteri bilgisi
        public virtual Room Room { get; set; } = null!;

    }
}
