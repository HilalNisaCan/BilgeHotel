using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Döviz kuru işlemlerini yöneten manager arayüzüdür.
    /// </summary>
    public interface IExchangeRateManager : IManager<ExchangeRateDto, ExchangeRate>
    {

        /// <summary>
        /// ID üzerinden döviz kuru kaydını siler.
        /// (Kontrol yapılmadan doğrudan silme işlemi uygulanır)
        /// </summary>
        /// Geliştirme durumu ileride ekleniceği için şimdilik manaagerda eklendi 
        ///  ileride "sadece bugünkü kurlar silinebilir" gibi kurallar eklenicek
        Task<bool> DeleteAsync(int id);


    }
}
