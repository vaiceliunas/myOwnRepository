using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Idp4.Models.Entities
{
    public class PersistedGrantModel
    {
        //
        // Summary:
        //     Gets or sets the key.
        //
        // Value:
        //     The key.
        [Key]
        public string Key { get; set; }
        //
        // Summary:
        //     Gets the type.
        //
        // Value:
        //     The type.
        public string Type { get; set; }
        //
        // Summary:
        //     Gets the subject identifier.
        //
        // Value:
        //     The subject identifier.
        public string SubjectId { get; set; }
        //
        // Summary:
        //     Gets the session identifier.
        //
        // Value:
        //     The session identifier.
        public string SessionId { get; set; }
        //
        // Summary:
        //     Gets the client identifier.
        //
        // Value:
        //     The client identifier.
        public string ClientId { get; set; }
        //
        // Summary:
        //     Gets the description the user assigned to the device being authorized.
        //
        // Value:
        //     The description.
        public string Description { get; set; }
        //
        // Summary:
        //     Gets or sets the creation time.
        //
        // Value:
        //     The creation time.
        public DateTime CreationTime { get; set; }
        //
        // Summary:
        //     Gets or sets the expiration.
        //
        // Value:
        //     The expiration.
        public DateTime? Expiration { get; set; }
        //
        // Summary:
        //     Gets or sets the consumed time.
        //
        // Value:
        //     The consumed time.
        public DateTime? ConsumedTime { get; set; }
        //
        // Summary:
        //     Gets or sets the data.
        //
        // Value:
        //     The data.
        public string Data { get; set; }
    }
}
