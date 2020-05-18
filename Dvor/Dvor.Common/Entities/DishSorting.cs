using Dvor.Common.Enums;
using System.Collections.Generic;

namespace Dvor.Common.Entities
{
    public class DishSorting
    {
        public SortingMethod SortingMethod { get; set; }

        public IEnumerable<string> Allergies { get; set; }

        public bool NewOnly { get; set; }

        public string CategoryId { get; set; }
    }
}