using ColorRandomApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorRandomApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        ApplicationDbContext _context;

        public SkillsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public Skill AddSkill([FromBody] Skill skill)
        {
            _context.Skills.Add(skill);
            _context.SaveChanges();

            return skill;
        }

        [HttpGet]
        public List<Skill> GetSkills()
        {
            List<Skill> results = _context.Skills
                .OrderByDescending(item => item.OwnerId)
                .ToList();

            return results;
        }

        [HttpGet("{id}")]
        public Skill GetSkills(int id)
        {
            Skill result = _context.Skills
                .Where(item => item.OwnerId == id)
                .FirstOrDefault();

            return result;
        }

        [HttpPut]
        public bool UpdateSkill([FromBody] Skill skill)
        {
            var findResult = _context.Skills
                .Where(x => x.SkillId == skill.SkillId)
                .FirstOrDefault();

            if (findResult == null)
                return false;

            findResult.SkillName = skill.SkillName;
            findResult.SkillExp = skill.SkillExp;
            findResult.SkillLevel = skill.SkillLevel;
            _context.SaveChanges();

            return true;
        }


        //[HttpDelete]
        //public bool DeleteSkill(int id)
        //{
        //    var findResult = _context.Skills
        //        .Where(x => x.SkillId == id)
        //        .FirstOrDefault();

        //    if (findResult == null)
        //        return false;

        //    _context.Skills.Remove(findResult);
        //    _context.SaveChanges();

        //    return true;
        //}
    }
}
