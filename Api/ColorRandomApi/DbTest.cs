using ColorRandomApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMO_EFCore
{
    public class DbTest
    {
        // 초기화 시간이 좀 걸림
        public static void InitiakuzeDB(bool forceReset = false) // 초기화를 할지 말지
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // DB가 만들어져 있는지 체크
                if (!forceReset && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();


                CreateTestData(db);
                Console.WriteLine("DB INitialized");
            }
        }


        public static void CreateTestData(ApplicationDbContext db)
        {
            var Gunal = new Player() { Name = "Gunal" };
            var Faker = new Player() { Name = "Faker" };
            var Dohee = new Player() { Name = "Dohee" };

            

            Skill skills = new Skill()
            {
                Owner = Gunal,
                SkillName = "태극 1",
                SkillExp = 11,
                SkillLevel = 3
            };

            Skill skills2 = new Skill()
            {
                Owner = Gunal,
                SkillName = "태극 2",
                SkillExp = 13,
                SkillLevel = 34
            };

            Skill skills3 = new Skill()
            {
                Owner = Gunal,
                SkillName = "태극 3",
                SkillExp = 12,
                SkillLevel = 5
            };

            db.Skills.Add(skills);
            db.Skills.Add(skills2);
            db.Skills.Add(skills3);

            db.SaveChanges();
        }


        public static void ShowItems()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var skills in db.Skills.Include(i => i.Owner).IgnoreQueryFilters().ToList())
                {
                    
                    if (skills.Owner != null)
                    {
                        Console.WriteLine($"{skills.Owner},   {skills.SkillName}");

                    }

                }
            }
        }



    }
}