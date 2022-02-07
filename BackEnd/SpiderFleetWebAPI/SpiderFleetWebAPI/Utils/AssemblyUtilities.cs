using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Utils
{
    /// <summary>
    /// Assembly Utilities Class
    /// </summary>
    public class AssemblyUtilities
    {
        /// <summary>
        /// Harcoded assembly title
        /// A static readonly property will throw an error if the actual assembly title does not match the harcoded one.
        /// The reason why there is a harcoded one is because the attributes where this value is used requiere a constan value.
        /// </summary>
        public const string Title = "Spider Fleet API";

        /// <summary>
        /// Assembly version used for display in the frontend
        /// </summary>
        /// <remarks>Make sure to update it under Properties > AssemblyInfo.cs</remarks>
        public const string Version = "1.0.0";

        /// <summary>
        /// Assembly title and version used for display in the frontend
        /// </summary>
        public const string TitleAndVersion = Title + " v" + Version;

        /// <summary>
        /// Short display of the Spider API environment
        /// </summary>
        public const string EnvironmentShortName = "DEV";

        /// <summary>
        /// Notes about the environment
        /// </summary>
        public const string EnvironmentNotes = "(DEV backend *** For testing purposes)";

        public const string Description = "REST Spider API para uso interno y aplicaciones cliente";

        /// <summary>
        /// Name of the team who built this web application
        /// </summary>
        public const string DeveloperTeamName = "Software Factory Spider";

        /// <summary>
        /// Email of the team who built this web application
        /// </summary>
        public const string DeveloperEmail = "contacto@spider.com.mx";

        /// <summary>
        /// Name of the company that owns the web application
        /// </summary>
        public const string CompanyName = "Spider Fleet";

        /// <summary>
        /// Mexican Spanish culture name
        /// </summary>
        public const string CultureName = "es-mx";

        /// <summary>
        /// Mexican Spanish culture LCID
        /// </summary>
        public const string CultureLCID = "2058";

        /// <summary>
        /// Mexican Spanish culture display name
        /// </summary>
        public const string CultureDisplayName = "Spanish (Mexico)";
    }
}