using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public int SkillsId { get; set; }
        public Skills skills { get; set; }
        public DateTime Date { get; set; }
    }


}