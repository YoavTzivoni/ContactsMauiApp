using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMauiApp.Model
{
	public class Contact
	{		
		private static int contactId = 0;
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public DateTime dateOfBirth { get; set; }
		public string ImgPath { get; set; } = "face.png";
		public Contact() { Id = ++contactId; }
	}
}
