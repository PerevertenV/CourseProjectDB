using CP.DataAccess.Repository;
using CP.DataAccess.Services.IServices;
using CP.DataAccess.ServicesBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Services
{
	public class ServiceBL: IServiceBL
	{
		public IUserService User { get; private set; }
		public IPurchaseService Purchase { get; private set; }

		public ServiceBL()
		{
			User = new UserService();
			Purchase = new PurchaseService();
		}
	}
}
