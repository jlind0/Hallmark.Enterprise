using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HallData.ApplicationViews;
using HallData.Globalization;

namespace HallData.Security
{
	public class SecurityUser : IHasKey<Guid>
	{
		public Guid UserGuid { get; set; }
		[Required]
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Guid OrganizationGuid { get; set; }
		public Cultures Culture { get; set; }
		public Guid Key
		{
			get
			{
				return this.UserGuid;
			}
			set
			{
				this.UserGuid = value;
			}
		}
	}
	
}
