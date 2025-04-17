using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Tools
{  
    
    /// <summary>
   /// Kimlik verisi olan kişi nesneleri için yardımcı metotlar.
   /// </summary>
    public static class PersonExtensions
    {
        /// <summary>
        /// Ad ve soyad'ı birleştirerek tam ad döner.
        /// </summary>
        public static string GetFullName(this IIdentifiablePerson person)
        {
            return $"{person.FirstName} {person.LastName}";
        }
    }
}
