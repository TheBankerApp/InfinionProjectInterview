using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinionInterviewProject.Infrastructure.Seed
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using global::InfinionInterviewProject.Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;

    namespace InfinionInterviewProject.Infrastructure.Seed
    {
        public static class StateLgaSeeder
        {
            public static async Task SeedAsync(AppDbContext db, HttpClient http)
            {
                if (db.States.Any() && db.Lgas.Any()) return;
                try
                {
                    var json = await http.GetStringAsync("https://nga-states-lga.onrender.com/fetch");
                    var states = JsonSerializer.Deserialize<string[]>(json);
                    int id = 1;
                    foreach (var stateName in states)
                    {
                        db.States.Add(new StateSeed {Name = stateName });

                        var lgaJson = await http.GetStringAsync("https://nga-states-lga.onrender.com/?state=" + stateName);
                        var lgas = JsonSerializer.Deserialize<string[]>(lgaJson);
                        
                        foreach (var lgaName in lgas)
                        {
                            db.Lgas.Add(new LgaSeed { Name = lgaName, State = stateName });
                        }
                        await db.SaveChangesAsync();
                    }
                    //db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT States ON;");
                    //db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT States OFF;");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Seeding failed: " + ex.Message);
                }
            }
        }
    }

}
