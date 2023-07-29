using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedData.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Skill> Skills { get; set; }
    }


}