using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo
{
    [Table("TbGTRemovePeoInfo")]
    public class TbGTRemovePeoInfo
    {
        public int ID { get; set; }

        public string ProjectNum { get; set; }

        public string ProjectName { get; set; }

        public string Name { get; set; }

        public string PersonCarID { get; set; }

        public string AccountNature { get; set; }

        public string OwnerRelation { get; set; }

        public string IsOnlyChild { get; set; }

        public string IsHalfHouse { get; set; }

        public string IsHaveDisability { get; set; }

        public string ResidenceAddress { get; set; }

        public string Remarks { get; set; }

        public string OwnerName { get; set; }


    }
}
