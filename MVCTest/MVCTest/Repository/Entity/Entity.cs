using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCTest.Repository
{
    public class Entity
    {
        [Key]
        public string Id { get; set; }
        public Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}