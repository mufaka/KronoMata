using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KronoMata.Model.Stats
{
    /// <summary>
    /// TableStat provides a generic means to encapsulate information
    /// about data in a particular table.
    /// </summary>
    [Serializable]
    public class TableStat
    {
        /// <summary>
        /// The name of the table for this TableStat.
        /// </summary>
        public string TableName { get; set; } = String.Empty;

        /// <summary>
        /// The total amount of records in the table.
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// The Date and Time of the oldest record in the table.
        /// </summary>
        public DateTime? OldestRecord { get; set; }

        /// <summary>
        /// The Date and Time of the newest record in the table.
        /// </summary>
        public DateTime? NewestRecord { get; set; }
    }
}
