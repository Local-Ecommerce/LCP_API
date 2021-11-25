using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.LocalZone
{
    public class LocalZoneRequest
    {
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
    }

    public enum LocalZoneStatus
    {
        ERROR = -1,
        SUCCESS = 0,
        LOCALZONE_NOT_FOUND = 4001,
        DELETED_LOCALZONE = 4002,
        UNVERIFIED_CREATE_LOCALZONE = 4003,
        UNVERIFIED_UPDATE_LOCALZONE = 4004
    }
}
