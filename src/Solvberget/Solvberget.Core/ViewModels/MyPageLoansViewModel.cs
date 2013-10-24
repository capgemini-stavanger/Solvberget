using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Solvberget.Core.Services;
using Solvberget.Core.ViewModels.Base;
using Solvberget.Domain.DTO;

namespace Solvberget.Core.ViewModels
{
    public class MyPageLoansViewModel : BaseViewModel
    {
        public MyPageLoansViewModel()
        {
            var service = Mvx.Resolve<IUserInformationService>();
            if (service == null) throw new ArgumentNullException("service");

            Loans = service.GetUserLoans("id");
        }

        private List<Loan> _loans;
        public List<Loan> Loans
        {
            get { return _loans; }
            set { _loans = value; RaisePropertyChanged(() => Loans); }
        } 
    }
}
