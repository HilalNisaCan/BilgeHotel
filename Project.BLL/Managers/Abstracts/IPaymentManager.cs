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
    /// Ödeme işlemlerini yöneten arayüz.
    /// </summary>
    public interface IPaymentManager : IManager<PaymentDto, Payment>
    {


        /// <summary>
        /// Yeni ödeme oluşturur ve ID’yi döner.
        /// </summary>
        Task<int> CreateAndReturnIdAsync(PaymentDto dto);

        /// <summary>
        /// Yeni ödeme oluşturur (ID dönmez).
        /// </summary>
        Task<bool> AddAsync(PaymentDto dto);



    }
}
