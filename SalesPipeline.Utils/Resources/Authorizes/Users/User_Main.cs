using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class User_Main : CommonModel
	{
        public UserCustom? User { get; set; }
        public List<User_Target_SaleCustom> ItemsTarget { get; set; } = new();
    }
}
