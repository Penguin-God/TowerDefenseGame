﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserName { get; set; }
        public List<Skill> skills { get; set; }
        public DateTime Date { get; set; }
    }

    public class Skill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int SkillExp { get; set; }
        public Player Owner { get; set; }
    }

}