﻿using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IOrderRepository:IRepository<Order>
    {
        Task<List<Order>> GetOrdersByUserIdAsync(int userId); // Kullanıcının yaptığı siparişleri getir
        Task<List<Order>> GetOrdersByReservationIdAsync(int reservationId); // Rezervasyona bağlı siparişleri getir
       
    }
}

