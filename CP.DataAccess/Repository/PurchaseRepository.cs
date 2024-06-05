﻿using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;
using CP.Models;

namespace CP.DataAccess.Repository
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        private ApplicationDbContext _db;
        public PurchaseRepository(ApplicationDbContext? db) : base(db)
        {
            _db = db;
        }

        public double CountMoneyToReturn(double DepositedMoney, double NeededMoney)
        {
            if (DepositedMoney < NeededMoney) return 0;
            else return Math.Round(DepositedMoney - NeededMoney, 2);
        }

        public double CountSumInUAH(double SumToChange, double PDVpercent, double Price)
        {
            return Math.Round(SumToChange * Price + SumToChange * Price * PDVpercent, 2);
        }

        public void Update(Purchase obj)
        {
            _db.Purchase.Update(obj);
        }
    }
}
