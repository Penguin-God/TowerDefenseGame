﻿using ColorRandomApi.Data;
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

            

            Skills skills = new Skills()
            {
                Owner = Gunal,
                SkillsInven = { new Skill() {SkillName = "태극", SkillExp = 11, SkillLevel = 3 }, new Skill() { SkillName = "네크로맨서", SkillExp = 10, SkillLevel = 2} },
                
            };

            Skills skills2 = new Skills()
            {
                Owner = Gunal,
                SkillsInven = { new Skill() { SkillName = "태극2", SkillExp = 11, SkillLevel = 3 }, new Skill() { SkillName = "네크로맨서2", SkillExp = 10, SkillLevel = 2 } },

            };

            Skills skills3 = new Skills()
            {
                Owner = Gunal,
                SkillsInven = { new Skill() { SkillName = "태극3", SkillExp = 11, SkillLevel = 3 }, new Skill() { SkillName = "네크로맨서3", SkillExp = 10, SkillLevel = 2 } },

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
                        Console.WriteLine($"{skills.Owner},   {skills.SkillsInven}");

                    }

                }
            }
        }



    }
}