using BLL.Dtos.RefreshToken;
using BLL.Dtos.Resident;
using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BLL.Dtos.Account {
	[Serializable]
	public class ExtendAccountResponse : AccountResponse {
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public Collection<ResidentResponse> Residents { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public Collection<ExtendRefreshTokenDto> RefreshTokens { get; set; }
	}
}
