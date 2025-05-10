using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{/// <summary>
 /// Misafir ziyaret kayıtlarıyla ilgili işlemleri tanımlar.
 /// </summary>
    public interface IGuestVisitLogManager : IManager<GuestVisitLogDto, GuestVisitLog>
    {
        /// <summary>
        /// Yeni misafir ziyaret kaydı ekler.
        /// </summary>
        Task AddAsync(GuestVisitLogDto dto);
    }
}
