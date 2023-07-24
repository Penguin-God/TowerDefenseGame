using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedData.Models
{
    public class Skills
    {
        public int SkillsId { get; set; }
        public ICollection<Skill> SkillsInven { get; set; }

        [ForeignKey("OwnerId")]
        public int OwnerId { get; set; }
        public Player Owner { get; set; }
    }

    public class Skill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int SkillExp { get; set; }
        public int SkillLevel { get; set; }
        //public int OwnerId { get; set; }
        //public Player Owner { get; set; }
    }
}
