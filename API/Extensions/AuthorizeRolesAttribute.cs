using Microsoft.AspNetCore.Authorization;

namespace API.Extensions {
	public class AuthorizeRolesAttribute : AuthorizeAttribute {
		public AuthorizeRolesAttribute(params string[] roles) : base() {
			Roles = string.Join(",", roles);
		}
	}
}
