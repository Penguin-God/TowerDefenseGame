using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedData.Models
{
    [Serializable]
    public class Skill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int SkillExp { get; set; }
        public int SkillLevel { get; set; }

        [ForeignKey("OwnerId")]
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public Player Owner { get; set; }
    }
}
