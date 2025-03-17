using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMB.Core.Notifications
{
    /// <summary>
    /// Represents the possible actions for notifications related to entity state changes.
    /// This enum is used to indicate whether an entity was created, updated, or deleted.
    /// </summary>
    public enum NotificationAction
    {
        Created =1,
        Updated = 2,
        Deleted = 3
    }
}
