using System;
using System.Runtime.Serialization;

namespace BLL.Dtos.Exception {
	public class EntityNotFoundException : System.Exception {
		private Type EntityType { get; }

		private object Info { get; set; }

		public EntityNotFoundException() : base("Entity not found !") {
		}

		public EntityNotFoundException(Type entityType, object id)
				: this(entityType, id, null) {
		}

		public EntityNotFoundException(Type entityType, object info, System.Exception innerException)
				: base($"{entityType.Name} containing the info that {info} is not found.", innerException) {
			EntityType = entityType;
			Info = info;
		}

		public EntityNotFoundException(string message)
				: base(message) {
		}

		public EntityNotFoundException(string message, System.Exception innerException)
				: base(message, innerException) {
		}
	}
}
