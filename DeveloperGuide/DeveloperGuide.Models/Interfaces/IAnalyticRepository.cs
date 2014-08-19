using DGuide.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGuide.DAL.Interfaces
{
    public interface IAnalyticRepository : IDisposable
    {
        IEnumerable<Analytic> GetAnalytics();
        Analytic GetAnalyticByID(int analyticId);
        void InsertAnalytic(Analytic analytic);
        void DeleteAnalytic(int analyticId);
        void UpdateAnalytic(Analytic analytic);
        void Save();
    }
}
