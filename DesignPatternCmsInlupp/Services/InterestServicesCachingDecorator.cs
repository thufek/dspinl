using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DesignPatternCmsInlupp.Services
{
    public class InterestServicesCachingDecorator : IInterestService
    {
        private readonly IInterestService _decoratedInterestService;
        private decimal? _cache;
        private const int ExpirationInHours = 24;
        private DateTime LastUpdated;

        public InterestServicesCachingDecorator(InterestService interestService)
        {
            _decoratedInterestService = interestService;
        }
        public decimal GetRiksbankensBaseRate()
        {
            if(LastUpdated.AddHours(ExpirationInHours) < DateTime.Now) ///Vet ej om detta ens stämmer, tror inte... Kommer ej ihåg hur man skulle göra här då ditt exempel ej finns på kurswebben eller git  
            {
                _cache = null;
            }
            if (_cache == null)
            {
                _cache = _decoratedInterestService.GetRiksbankensBaseRate();
                LastUpdated = DateTime.Now;
            }
            return _cache.GetValueOrDefault();
        }
    }
}