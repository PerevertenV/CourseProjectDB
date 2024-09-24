using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Services.IServices
{
	public interface IServiceBL
	{
		public IUserService User { get; }
		public IPurchaseService Purchase { get; }
	}
}
