/*

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); 

*/

using EvergreenResort.Api.Data;
using EvergreenResort.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurazione Servizi
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=Evergreen.db"));

var app = builder.Build();

// 2. Configurazione Pipeline (Middleware)
app.UseDefaultFiles();  // Fondamentale per caricare index.html in automatico
app.UseStaticFiles();   // Permette l'accesso a CSS, JS e immagini
app.MapControllers();   // Collega i tuoi controller API

// 3. Inizializzazione Database (Seed Data)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated(); // Crea fisicamente il file Evergreen.db

    if (!db.Camere.Any())
    {
        db.Camere.AddRange(
            // --- SUITE ---
            new Camera { 
                Nome = "Pine Valley Suite", 
                Numero = "101", 
                Prezzo = 250.00m, 
                Stato = "Libera", 
                Tipologia = "Suite",
                ImmagineUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=800", 
                Descrizione = "Immersa tra i pini secolari, con balcone privato affacciato sul green." 
            },
            new Camera { 
                Nome = "Royal Garden Suite", 
                Numero = "102", 
                Prezzo = 320.00m, 
                Stato = "In Pulizia", 
                Tipologia = "Suite",
                ImmagineUrl = "https://images.unsplash.com/photo-1582719508461-905c673771fd?w=800", 
                Descrizione = "Lusso sfrenato con giardino privato e vasca idromassaggio." 
            },
            new Camera { 
                Nome = "Ocean Breeze Suite", 
                Numero = "103", 
                Prezzo = 450.00m, 
                Stato = "Libera", 
                Tipologia = "Suite",
                ImmagineUrl = "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?w=800", 
                Descrizione = "La massima esperienza di lusso con vista mare a 180 gradi e servizio maggiordomo." 
            },
             new Camera { 
                Nome = "Golden Sunset Suite", 
                Numero = "104", 
                Prezzo = 380.00m, 
                Stato = "Libera", 
                Tipologia = "Suite",
                ImmagineUrl = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=800", 
                Descrizione = "Suite esposta a ovest per godere di tramonti indimenticabili dal terrazzo privato." 
            },
            new Camera { 
                Nome = "Presidential Golf Suite", 
                Numero = "105", 
                Prezzo = 550.00m, 
                Stato = "Libera", 
                Tipologia = "Suite",
                ImmagineUrl = "https://images.unsplash.com/photo-1590490360182-c33d57733427?w=800", 
                Descrizione = "Il top del resort: 120mq, due camere, salotto e vista diretta sul campo da golf." 
            },
            new Camera { 
                Nome = "Wellness Spa Suite", 
                Numero = "106", 
                Prezzo = 400.00m, 
                Stato = "Manutenzione", 
                Tipologia = "Suite",
                ImmagineUrl = "https://images.unsplash.com/photo-1531835551805-16d864c8d311?w=800", 
                Descrizione = "Suite dedicata al benessere con sauna privata e accesso diretto alla SPA." 
            },
            new Camera { 
                Nome = "Honeymoon Haven", 
                Numero = "107", 
                Prezzo = 360.00m, 
                Stato = "Occupata", 
                Tipologia = "Suite",
                ImmagineUrl = "https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?w=800", 
                Descrizione = "Romantica e appartata, con letto a baldacchino e champagne all'arrivo." 
            },

            // --- DELUXE ---
            new Camera { 
                Nome = "Emerald Fairway Deluxe", 
                Numero = "202", 
                Prezzo = 180.00m, 
                Stato = "Libera", 
                Tipologia = "Deluxe",
                ImmagineUrl = "https://images.unsplash.com/photo-1566665797739-1674de7a421a?w=800", 
                Descrizione = "Vista panoramica sulla buca 18 e arredamento in legno di rovere." 
            },
            new Camera { 
                Nome = "Lagoon View Deluxe", 
                Numero = "203", 
                Prezzo = 210.00m, 
                Stato = "Occupata", 
                Tipologia = "Deluxe",
                ImmagineUrl = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800", 
                Descrizione = "Affacciata sul laghetto del resort, offre un'atmosfera di pace assoluta." 
            },
            new Camera { 
                Nome = "Mountain Retreat Deluxe", 
                Numero = "204", 
                Prezzo = 195.00m, 
                Stato = "Libera", 
                Tipologia = "Deluxe",
                ImmagineUrl = "https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?w=800", 
                Descrizione = "Design alpino-chic con camino in pietra e vista sulle montagne circostanti." 
            },
            new Camera { 
                Nome = "Botanic Garden Deluxe", 
                Numero = "205", 
                Prezzo = 185.00m, 
                Stato = "Libera", 
                Tipologia = "Deluxe",
                ImmagineUrl = "https://images.unsplash.com/photo-1598928506311-c55ded91a20c?w=800", 
                Descrizione = "Accesso diretto al giardino botanico, ideale per gli amanti della natura." 
            },
            new Camera { 
                Nome = "Oak Wood Deluxe", 
                Numero = "206", 
                Prezzo = 190.00m, 
                Stato = "Libera", 
                Tipologia = "Deluxe",
                ImmagineUrl = "https://images.unsplash.com/photo-1505691938895-1758d7feb511?w=800", 
                Descrizione = "Arredata interamente con legni pregiati locali, calda e accogliente." 
            },
            new Camera { 
                Nome = "Sunrise Terrace Deluxe", 
                Numero = "207", 
                Prezzo = 200.00m, 
                Stato = "Occupata", 
                Tipologia = "Deluxe",
                ImmagineUrl = "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=800", 
                Descrizione = "Ampio terrazzo esposto a est per godersi l'alba con la colazione in camera." 
            },
            new Camera { 
                Nome = "Poolside Deluxe", 
                Numero = "208", 
                Prezzo = 205.00m, 
                Stato = "Libera", 
                Tipologia = "Deluxe",
                ImmagineUrl = "https://images.unsplash.com/photo-1571003123894-1f0594d2b5d9?w=800", 
                Descrizione = "A due passi dalla piscina infinity, perfetta per chi ama nuotare." 
            },

            // --- STANDARD ---
            new Camera { 
                Nome = "Willow Creek Room", 
                Numero = "303", 
                Prezzo = 135.00m, 
                Stato = "Occupata", 
                Tipologia = "Standard",
                ImmagineUrl = "https://images.unsplash.com/photo-1611892440504-42a792e24d32?w=800", 
                Descrizione = "Tranquilla e spaziosa, ideale per chi cerca il massimo relax." 
            },
            new Camera { 
                Nome = "Sunset View Standard", 
                Numero = "304", 
                Prezzo = 140.00m, 
                Stato = "Manutenzione", 
                Tipologia = "Standard",
                ImmagineUrl = "https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=800", 
                Descrizione = "Camera accogliente con vista mozzafiato sul tramonto." 
            },
            new Camera { 
                Nome = "Cozy Corner Standard", 
                Numero = "305", 
                Prezzo = 120.00m, 
                Stato = "Libera", 
                Tipologia = "Standard",
                ImmagineUrl = "https://images.unsplash.com/photo-1505691938895-1758d7feb511?w=800", 
                Descrizione = "Piccola ma confortevole, perfetta per soggiorni brevi e viaggiatori singoli." 
            },
             new Camera { 
                Nome = "Family Comfort Standard", 
                Numero = "306", 
                Prezzo = 150.00m, 
                Stato = "Libera", 
                Tipologia = "Standard",
                ImmagineUrl = "https://images.unsplash.com/photo-1596436889106-be35e843f974?w=800", 
                Descrizione = "Spazi ottimizzati per famiglie, con due letti queen size e area giochi." 
            },
            new Camera { 
                Nome = "Green Starter Room", 
                Numero = "307", 
                Prezzo = 115.00m, 
                Stato = "Libera", 
                Tipologia = "Standard",
                ImmagineUrl = "https://images.unsplash.com/photo-1590073242678-70ee3fc28e8e?w=800", 
                Descrizione = "Essenziale e moderna, ottima come punto d'appoggio per i golfisti." 
            },
            new Camera { 
                Nome = "Hiker's Base", 
                Numero = "308", 
                Prezzo = 110.00m, 
                Stato = "Libera", 
                Tipologia = "Standard",
                ImmagineUrl = "https://images.unsplash.com/photo-1512916194211-3f2b7f5f7de3?w=800", 
                Descrizione = "Vicino ai sentieri, pratica e funzionale per gli amanti del trekking." 
            },
            new Camera { 
                Nome = "Garden Patch Room", 
                Numero = "309", 
                Prezzo = 125.00m, 
                Stato = "In Pulizia", 
                Tipologia = "Standard",
                ImmagineUrl = "https://images.unsplash.com/photo-1507652313519-d4e9174996dd?w=800", 
                Descrizione = "Piano terra con piccolo patio privato immerso nel verde." 
            }
        );
        db.SaveChanges();
    }
}

app.Run();