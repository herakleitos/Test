using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo
{
    [Table("T_DEMO")]
    public class T_DEMO
    {
        [Key]
        public string NAME { get; set; }

        public string DESC { get; set; }

    }
}
