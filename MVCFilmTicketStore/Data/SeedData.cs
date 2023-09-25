using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCFilmTicketStore.Areas.Identity.Data;
using MVCFilmTicketStore.DataTypes.Enums;
using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.Data
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<MVCFilmTicketStoreUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            MVCFilmTicketStoreUser user = await UserManager.FindByEmailAsync("admin@mvcbookstore.com");
            if (user == null)
            {
                var User = new MVCFilmTicketStoreUser();
                User.Email = "admin@mvcbookstore.com";
                User.UserName = "admin@mvcbookstore.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            var roleCheck2 = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck2) { roleResult = await RoleManager.CreateAsync(new IdentityRole("User")); }
            MVCFilmTicketStoreUser user2 = await UserManager.FindByEmailAsync("defuser@mvcbookstore.com");
            if (user2 == null)
            {
                var User = new MVCFilmTicketStoreUser();
                User.Email = "defuser@mvcbookstore.com";
                User.UserName = "defuser@mvcbookstore.com";
                string userPWD = "User123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(User, "User");
                }
            }
        }

        private static List<Seat> SeedSeats()
        {
            List<Seat> seats = new List<Seat>();
            int maxRows = 16;
            int maxColumns = 17;

            for (int row = 1; row <= maxRows; row++)
            {
                for (int column = 1; column <= maxColumns; column++)
                {
                    seats.Add(new Seat
                    {
                        Row = row,
                        Column = column
                    });
                }
            }

            return seats;
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MVCFilmTicketStoreContext(
                serviceProvider.GetRequiredService<DbContextOptions<MVCFilmTicketStoreContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Film.Any() || context.Ticket.Any() || context.Seat.Any() || context.Review.Any() || context.Projection.Any() || context.Actor.Any() || context.Director.Any() || context.Genre.Any() || context.Theater.Any())
                {
                    return;   // DB has been seeded
                }

                context.Actor.AddRange(
                    new Actor
                    {   /*Id = 1, */
                        FirstName = "Margot",
                        LastName = "Robbie",
                        BirthDate = DateTime.Parse("1990-07-02"),
                        Country = "Australia",
                        ProfilePictureUrl = "margot.png",
                        Gender = Gender.Female
                    },
                    new Actor
                    {   /*Id = 2, */
                        FirstName = "Sandra",
                        LastName = "Bullock",
                        BirthDate = DateTime.Parse("1964-07-26"),
                        Country = "United States",
                        ProfilePictureUrl = "sandra.png",
                        Gender = Gender.Female
                    },
                    new Actor
                    {   /*Id = 3, */
                        FirstName = "Channing",
                        LastName = "Tatum",
                        BirthDate = DateTime.Parse("1980-04-26"),
                        Country = "United States",
                        ProfilePictureUrl = "channing.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 4, */
                        FirstName = "Ezra",
                        LastName = "Miller",
                        BirthDate = DateTime.Parse("1992-09-30"),
                        Country = "United States",
                        ProfilePictureUrl = "ezra.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 5, */
                        FirstName = "Robert",
                        LastName = "Pattinson",
                        BirthDate = DateTime.Parse("1986-05-13"),
                        Country = "United Kingdom",
                        ProfilePictureUrl = "robert.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 6, */
                        FirstName = "Harrison",
                        LastName = "Ford",
                        BirthDate = DateTime.Parse("1942-07-13"),
                        Country = "United States",
                        ProfilePictureUrl = "harrison.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 7, */
                        FirstName = "Chris",
                        LastName = "Hemsworth",
                        BirthDate = DateTime.Parse("1983-08-11"),
                        Country = "Australia",
                        ProfilePictureUrl = "hemsworth.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 8, */
                        FirstName = "Natalie",
                        LastName = "Portman",
                        BirthDate = DateTime.Parse("1981-06-09"),
                        Country = "Israel",
                        ProfilePictureUrl = "natalie.png",
                        Gender = Gender.Female
                    },
                    new Actor
                    {   /*Id = 9, */
                        FirstName = "Tom",
                        LastName = "Cruise",
                        BirthDate = DateTime.Parse("1962-07-03"),
                        Country = "United States",
                        ProfilePictureUrl = "tom.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 10, */
                        FirstName = "Keanu",
                        LastName = "Reeves",
                        BirthDate = DateTime.Parse("1964-09-02"),
                        Country = "Lebanon",
                        ProfilePictureUrl = "keanu.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 11, */
                        FirstName = "Cillian",
                        LastName = "Murphy",
                        BirthDate = DateTime.Parse("1976-05-25"),
                        Country = "Ireland",
                        ProfilePictureUrl = "cillian.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 12, */
                        FirstName = "Ryan",
                        LastName = "Gosling",
                        BirthDate = DateTime.Parse("1990-07-02"),
                        Country = "United States",
                        ProfilePictureUrl = "ryan.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 13, */
                        FirstName = "Timothée",
                        LastName = "Chalamet",
                        BirthDate = DateTime.Parse("1995-12-27"),
                        Country = "United States",
                        ProfilePictureUrl = "timothee.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 14, */
                        FirstName = "Chris",
                        LastName = "Pratt",
                        BirthDate = DateTime.Parse("1979-06-21"),
                        Country = "United States",
                        ProfilePictureUrl = "pratt.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 15, */
                        FirstName = "Zendaya",
                        LastName = "Marie",
                        BirthDate = DateTime.Parse("1996-09-01"),
                        Country = "United States",
                        ProfilePictureUrl = "zendaya.png",
                        Gender = Gender.Female
                    },
                    new Actor
                    {   /*Id = 16, */
                        FirstName = "Sam",
                        LastName = "Worthington",
                        BirthDate = DateTime.Parse("1986-09-01"),
                        Country = "Australia",
                        ProfilePictureUrl = "sam.png",
                        Gender = Gender.Male
                    },
                    new Actor
                    {   /*Id = 17, */
                        FirstName = "Michael",
                        LastName = "B.Jordan",
                        BirthDate = DateTime.Parse("1986-09-01"),
                        Country = "United States",
                        ProfilePictureUrl = "michael.png",
                        Gender = Gender.Male
                    }
                );
                context.SaveChanges();

                context.Genre.AddRange(
                    new Genre { /*Id = 1, */GenreName = "Fantasy" },
                    new Genre { /*Id = 2, */GenreName = "Comedy" },
                    new Genre { /*Id = 3, */GenreName = "Sci-Fi" },
                    new Genre { /*Id = 4, */GenreName = "Romantic" },
                    new Genre { /*Id = 5, */GenreName = "Action" },
                    new Genre { /*Id = 6, */GenreName = "Tragedy" },
                    new Genre { /*Id = 7, */GenreName = "History" },
                    new Genre { /*Id = 8, */GenreName = "Science" },
                    new Genre { /*Id = 9, */GenreName = "Novel" },
                    new Genre { /*Id = 10, */GenreName = "Fiction" },
                    new Genre { /*Id = 11, */GenreName = "Political" }
                );
                context.SaveChanges();

                context.Director.AddRange(
                    new Director
                    {   /*Id = 1, */
                        FirstName = "Christopher",
                        LastName = "Nolan",
                        BirthDate = DateTime.Parse("1970-07-30"),
                        Country = "United Kingdom",
                        ProfilePictureUrl = "nolan.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 2, */
                        FirstName = "Aaron",
                        LastName = "Nee",
                        BirthDate = DateTime.Parse("1983-03-19"),
                        Country = "United States",
                        ProfilePictureUrl = "nee.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 3, */
                        FirstName = "Denis",
                        LastName = "Villeneuve",
                        BirthDate = DateTime.Parse("1967-10-03"),
                        Country = "Canada",
                        ProfilePictureUrl = "villeneuve.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 4, */
                        FirstName = "Colin",
                        LastName = "Trevorrow",
                        BirthDate = DateTime.Parse("1976-09-13"),
                        Country = "United States",
                        ProfilePictureUrl = "trevorrow.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 5, */
                        FirstName = "Andy",
                        LastName = "Muschietti",
                        BirthDate = DateTime.Parse("1973-08-26"),
                        Country = "Argentina",
                        ProfilePictureUrl = "muschietti.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 6, */
                        FirstName = "David",
                        LastName = "Yates",
                        BirthDate = DateTime.Parse("1963-10-08"),
                        Country = "United Kingdom",
                        ProfilePictureUrl = "yates.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 7, */
                        FirstName = "James",
                        LastName = "Cameron",
                        BirthDate = DateTime.Parse("1954-08-16"),
                        Country = "Canada",
                        ProfilePictureUrl = "cameron.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 8, */
                        FirstName = "Ryan",
                        LastName = "Coogler",
                        BirthDate = DateTime.Parse("1986-05-23"),
                        Country = "United States",
                        ProfilePictureUrl = "coogler.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 9, */
                        FirstName = "Christopher",
                        LastName = "McQuarrie",
                        BirthDate = DateTime.Parse("1968-10-27"),
                        Country = "United States",
                        ProfilePictureUrl = "mcquarrie.png",
                        Gender = Gender.Male
                    },
                    new Director
                    {   /*Id = 10, */
                        FirstName = "Lana",
                        LastName = "Wachowski",
                        BirthDate = DateTime.Parse("1965-06-21"),
                        Country = "United States",
                        ProfilePictureUrl = "wachowski.png",
                        Gender = Gender.Female
                    },
                    new Director
                    {   /*Id = 11, */
                        FirstName = "Greta",
                        LastName = "Gerwig",
                        BirthDate = DateTime.Parse("1983-08-04"),
                        Country = "United States",
                        ProfilePictureUrl = "gerwig.png",
                        Gender = Gender.Female
                    },
                    new Director
                    {   /*Id = 12, */
                        FirstName = "Taika",
                        LastName = "Waiti",
                        BirthDate = DateTime.Parse("1983-08-04"),
                        Country = "United States",
                        ProfilePictureUrl = "waiti.png",
                        Gender = Gender.Male
                    }
                );
                context.SaveChanges();

                context.Film.AddRange(
                    new Film
                    {
                        Title = "Oppenheimer",
                        Decription = "This is the newest film from Christopher Nolan, about the development of the Atomic Bomb in WW2.",
                        FilmDuration = new TimeSpan(3, 0, 0),
                        ReleaseDate = DateTime.Parse("2023-7-21"),
                        Poster = "oppenheimer.png",
                        DirectorId = 1
                    },
                    new Film
                    {
                        Title = "The Lost City",
                        Decription = "A comedic adventure film starring Sandra Bullock and Channing Tatum.",
                        FilmDuration = new TimeSpan(2, 15, 0),
                        ReleaseDate = DateTime.Parse("2023-08-18"),
                        Poster = "lostcity.png",
                        DirectorId = 2
                    },
                    new Film
                    {
                        Title = "Dune: Part Two",
                        Decription = "The second installment in the epic sci-fi saga directed by Denis Villeneuve.",
                        FilmDuration = new TimeSpan(2, 45, 0),
                        ReleaseDate = DateTime.Parse("2023-09-15"),
                        Poster = "dune2.png",
                        DirectorId = 3
                    },
                    new Film
                    {
                        Title = "Jurassic World: Dominion",
                        Decription = "The next chapter in the Jurassic Park franchise featuring dinosaurs and adventure.",
                        FilmDuration = new TimeSpan(2, 20, 0),
                        ReleaseDate = DateTime.Parse("2023-06-09"),
                        Poster = "jurassicworld.png",
                        DirectorId = 4
                    },
                    new Film
                    {
                        Title = "The Flash",
                        Decription = "A superhero film featuring the DC Comics character The Flash.",
                        FilmDuration = new TimeSpan(2, 5, 0),
                        ReleaseDate = DateTime.Parse("2023-07-07"),
                        Poster = "flash.png",
                        DirectorId = 5
                    },
                    new Film
                    {
                        Title = "Fantastic Beasts: The Secrets of Dumbledore",
                        Decription = "The next installment in the Fantastic Beasts series set in the Wizarding World of Harry Potter.",
                        FilmDuration = new TimeSpan(2, 25, 0),
                        ReleaseDate = DateTime.Parse("2022-11-15"),
                        Poster = "fantasticbeasts.png",
                        DirectorId = 6
                    },
                    new Film
                    {
                        Title = "Avatar: The Way of Water",
                        Decription = "The long-awaited sequel to James Cameron's groundbreaking sci-fi film, Avatar.",
                        FilmDuration = new TimeSpan(2, 50, 0),
                        ReleaseDate = DateTime.Parse("2023-12-22"),
                        Poster = "avatar.png",
                        DirectorId = 7
                    },
                    new Film
                    {
                        Title = "Black Panther: Wakanda Forever",
                        Decription = "The continuation of the Black Panther story in the Marvel Cinematic Universe.",
                        FilmDuration = new TimeSpan(2, 10, 0),
                        ReleaseDate = DateTime.Parse("2022-10-06"),
                        Poster = "blackpanther.png",
                        DirectorId = 8
                    },
                    new Film
                    {
                        Title = "Mission: Impossible – Dead Reckoning Part One",
                        Decription = "Another action-packed mission for Ethan Hunt and the IMF team.",
                        FilmDuration = new TimeSpan(2, 30, 0),
                        ReleaseDate = DateTime.Parse("2023-09-09"),
                        Poster = "missionimpossible.png",
                        DirectorId = 9
                    },
                    new Film
                    {
                        Title = "The Matrix 4: Resurrections",
                        Decription = "Neo returns in the fourth installment of the Matrix series, directed by Lana Wachowski.",
                        FilmDuration = new TimeSpan(2, 15, 0),
                        ReleaseDate = DateTime.Parse("2021-12-20"),
                        Poster = "matrix4.png",
                        DirectorId = 10
                    },
                    new Film
                    {
                        Title = "Barbie",
                        Decription = "Join Barbie in an exciting adventure as she explores new worlds and discovers the power of friendship.",
                        FilmDuration = new TimeSpan(1, 45, 0),
                        ReleaseDate = DateTime.Parse("2023-07-21"),
                        Poster = "barbie.png",
                        DirectorId = 11
                    },
                    new Film
                    {
                        Title = "Thor: Love And Thunder",
                        Decription = "Thor embarks on a journey unlike anything he's ever faced -- a quest for inner peace. However, his retirement gets interrupted by Gorr the God Butcher, a galactic killer who seeks the extinction of the gods.",
                        FilmDuration = new TimeSpan(1, 45, 0),
                        ReleaseDate = DateTime.Parse("2022-07-06"),
                        Poster = "thor.png",
                        DirectorId = 12
                    }
                );
                context.SaveChanges();

                context.ActorFilm.AddRange(
                    new ActorFilm { FilmId = 1, ActorId = 11 },
                    new ActorFilm { FilmId = 2, ActorId = 2 },
                    new ActorFilm { FilmId = 2, ActorId = 3 },
                    new ActorFilm { FilmId = 3, ActorId = 13 },
                    new ActorFilm { FilmId = 3, ActorId = 15 },
                    new ActorFilm { FilmId = 4, ActorId = 14 },
                    new ActorFilm { FilmId = 5, ActorId = 4 },
                    new ActorFilm { FilmId = 6, ActorId = 4 },
                    new ActorFilm { FilmId = 7, ActorId = 16 },
                    new ActorFilm { FilmId = 8, ActorId = 17 },
                    new ActorFilm { FilmId = 9, ActorId = 9 },
                    new ActorFilm { FilmId = 10, ActorId = 10 },
                    new ActorFilm { FilmId = 11, ActorId = 1 },
                    new ActorFilm { FilmId = 11, ActorId = 12 },
                    new ActorFilm { FilmId = 12, ActorId = 7 },
                    new ActorFilm { FilmId = 12, ActorId = 8 }
                );

                context.Review.AddRange(
                     new Review
                     {
                         /*Id = 1, */
                         FilmId = 1,
                         AppUser = "John Smith",
                         Comment = "The film is excellent! I loved every minute of it!",
                         Rating = 10
                     },
                     new Review
                     {
                         /*Id = 2, */
                         FilmId = 7,
                         AppUser = "John Smith",
                         Comment = "I found it very boring and dull... It's more fun watching paint dry.",
                         Rating = 2
                     },
                     new Review
                     {
                         /*Id = 3, */
                         FilmId = 4,
                         AppUser = "James Hetfield",
                         Comment = "The film is excellent! I loved every minute of it!",
                         Rating = 8
                     },
                     new Review
                     {
                         /*Id = 4, */
                         FilmId = 1,
                         AppUser = "Tom James",
                         Comment = "Great book, Eddard Stark was my favourite character, but we saw how that ended...The book is excellent! I loved every page of it!",
                         Rating = 10
                     },
                     new Review
                     {
                         /*Id = 5, */
                         FilmId = 2,
                         AppUser = "Tom James",
                         Comment = "Robb Stark is my favourite character, hope he wins the throne! The book is excellent! I loved every page of it!",
                         Rating = 9
                     },
                     new Review
                     {
                         /*Id = 6, */
                         FilmId = 3,
                         AppUser = "Tom James",
                         Comment = "Amazing sequel to clash! Arya is cool...The book is excellent! I loved every page of it!",
                         Rating = 10
                     },
                     new Review
                     {
                         /*Id = 7, */
                         FilmId = 4,
                         AppUser = "James Hetfield",
                         Comment = "The book is excellent! I loved every page of it!",
                         Rating = 8
                     },
                     new Review
                     {
                         /*Id = 8, */
                         FilmId = 5,
                         AppUser = "James Hetfield",
                         Comment = "Book is meh, i would recommend only if you don't have anything better to do.",
                         Rating = 6
                     },
                     new Review
                     {
                         /*Id = 9, */
                         FilmId = 4,
                         AppUser = "James Hetfield",
                         Comment = "The book is excellent! I loved every page of it!",
                         Rating = 8
                     },
                     new Review
                     {
                         /*Id = 10, */
                         FilmId = 1,
                         AppUser = "John Smith",
                         Comment = "The book is excellent! I loved every page of it!",
                         Rating = 10
                     },
                     new Review
                     {
                         /*Id = 11, */
                         FilmId = 2,
                         AppUser = "Tom James",
                         Comment = "I found it very boring and dull... It's more fun watching paint dry.",
                         Rating = 2
                     },
                     new Review
                     {
                         /*Id = 12, */
                         FilmId = 4,
                         AppUser = "John Smith",
                         Comment = "I found it very boring and dull... It's more fun watching paint dry.",
                         Rating = 4
                     },
                     new Review
                     {
                         /*Id = 13, */
                         FilmId = 5,
                         AppUser = "John Smith",
                         Comment = "I found it very boring and dull... It's more fun watching paint dry.",
                         Rating = 1
                     },
                     new Review
                     {
                         /*Id = 14, */
                         FilmId = 8,
                         AppUser = "John Smith",
                         Comment = "I found it very boring and dull... It's more fun watching paint dry.",
                         Rating = 3
                     }
                );
                context.SaveChanges();

                context.Theater.AddRange(
                    new Theater
                    {
                        /*Id = 1, */
                        Name = "Theater 1",
                        AllColumns = 15,
                        AllRows = 15,
                        AllSeatsNum = 225
                    },
                    new Theater
                    {
                        /*Id = 2, */
                        Name = "Theater 2",
                        AllColumns = 17,
                        AllRows = 10,
                        AllSeatsNum = 170
                    },
                    new Theater
                    {
                        /*Id = 3, */
                        Name = "Theater 3",
                        AllColumns = 10,
                        AllRows = 20,
                        AllSeatsNum = 200
                    },
                    new Theater
                    {
                        /*Id = 4, */
                        Name = "Theater 4",
                        AllColumns = 12,
                        AllRows = 13,
                        AllSeatsNum = 156
                    },
                    new Theater
                    {
                        /*Id = 5, */
                        Name = "Theater 5",
                        AllColumns = 16,
                        AllRows = 16,
                        AllSeatsNum = 256
                    },
                    new Theater
                    {
                        /*Id = 6, */
                        Name = "Theater 6",
                        AllColumns = 13,
                        AllRows = 13,
                        AllSeatsNum = 169
                    },
                    new Theater
                    {
                        /*Id = 7, */
                        Name = "Theater 7",
                        AllColumns = 15,
                        AllRows = 15,
                        AllSeatsNum = 225
                    }
                );
                context.SaveChanges();


                context.Projection.AddRange(
                     new Projection { /*Id = 1, */ FreeSeatsNum = 223, Is3D = true, Price = 6.99f, ProjectionTime = DateTime.Parse("2023/09/21 20:00:00"), FilmId = 11, TheaterId = 1 }, //225
                     new Projection { /*Id = 2, */ FreeSeatsNum = 170, Is3D = false, Price = 4.99f, ProjectionTime = DateTime.Parse("2023/09/21 18:00:00"), FilmId = 11, TheaterId = 2 },
                     new Projection { /*Id = 3, */ FreeSeatsNum = 199, Is3D = false, Price = 4.99f, ProjectionTime = DateTime.Parse("2023/09/21 22:00:00"), FilmId = 11, TheaterId = 3 },//200
                     new Projection { /*Id = 4, */ FreeSeatsNum = 154, Is3D = true, Price = 6.99f, ProjectionTime = DateTime.Parse("2023/09/21 17:00:00"), FilmId = 1, TheaterId = 4 },//156
                     new Projection { /*Id = 5, */ FreeSeatsNum = 256, Is3D = false, Price = 4.99f, ProjectionTime = DateTime.Parse("2023/09/21 20:00:00"), FilmId = 2, TheaterId = 5 },
                     new Projection { /*Id = 6, */ FreeSeatsNum = 169, Is3D = true, Price = 6.99f, ProjectionTime = DateTime.Parse("2023/09/22 21:00:00"), FilmId = 3, TheaterId = 6 },//167
                     new Projection { /*Id = 7, */ FreeSeatsNum = 223, Is3D = false, Price = 4.99f, ProjectionTime = DateTime.Parse("2023/09/22 19:00:00"), FilmId = 4, TheaterId = 7 },//225
                     new Projection { /*Id = 8, */ FreeSeatsNum = 225, Is3D = true, Price = 6.99f, ProjectionTime = DateTime.Parse("2023/09/22 19:00:00"), FilmId = 5, TheaterId = 1 }
                     );
                context.SaveChanges();

                context.Ticket.AddRange(
                    new Ticket { /*Id = 1, */ AppUser = "johnsmith@gmail.com", BoughtTime = DateTime.Now, IsValid = true, ProjectionId = 1 }, // 2 tixs
                    new Ticket { /*Id = 2, */ AppUser = "jameshetfield@gmail.com", BoughtTime = DateTime.Parse("2023/09/18 20:30:00"), IsValid = true, ProjectionId = 3 }, // 1 tix
                    new Ticket { /*Id = 3, */ AppUser = "tomjames@gmail.com", BoughtTime = DateTime.Parse("2023/09/17 20:30:00"), IsValid = true, ProjectionId = 4 }, // 2 tixs
                    new Ticket { /*Id = 4, */ AppUser = "elenamer@gmail.com", BoughtTime = DateTime.Parse("2023/09/18 20:30:00"), IsValid = true, ProjectionId = 6 }, // 2 tixs
                    new Ticket { /*Id = 5, */ AppUser = "johnsmith@gmail.com", BoughtTime = DateTime.Parse("2023/09/18 20:30:00"), IsValid = true, ProjectionId = 7 } // 2 tixs
                     /*                  , new Ticket { *//*Id = 1, *//* AppUser = "johnsmith@gmail.com", BoughtTime = DateTime.Now, IsValid = true, ProjectionId = 1 },
                                         new Ticket { *//*Id = 1, *//* AppUser = "johnsmith@gmail.com", BoughtTime = DateTime.Now, IsValid = true, ProjectionId = 1 },
                                         new Ticket { *//*Id = 1, *//* AppUser = "johnsmith@gmail.com", BoughtTime = DateTime.Now, IsValid = true, ProjectionId = 1 },*/
                     );
                context.SaveChanges();

                List<Seat> seats = SeedSeats();
                context.Seat.AddRange(seats);
                context.SaveChanges();

                context.TicketSeat.AddRange(
                    new TicketSeat { /*Id = 1, */ TicketId = 1, SeatId = 1 },
                    new TicketSeat { /*Id = 2, */ TicketId = 1, SeatId = 2 },
                    new TicketSeat { /*Id = 3, */ TicketId = 2, SeatId = 3 },
                    new TicketSeat { /*Id = 4, */ TicketId = 3, SeatId = 4 },
                    new TicketSeat { /*Id = 5, */ TicketId = 3, SeatId = 5 },
                    new TicketSeat { /*Id = 6, */ TicketId = 4, SeatId = 6 },
                    new TicketSeat { /*Id = 7, */ TicketId = 4, SeatId = 7 },
                    new TicketSeat { /*Id = 8, */ TicketId = 5, SeatId = 8 },
                    new TicketSeat { /*Id = 9, */ TicketId = 5, SeatId = 9 }
                    );
                context.SaveChanges();

                context.FilmGenre.AddRange(
                    new FilmGenre { FilmId = 1, GenreId = 1 },
                    new FilmGenre { FilmId = 1, GenreId = 9 },
                    new FilmGenre { FilmId = 2, GenreId = 10 },
                    new FilmGenre { FilmId = 2, GenreId = 11 },
                    new FilmGenre { FilmId = 3, GenreId = 1 },
                    new FilmGenre { FilmId = 3, GenreId = 9 },
                    new FilmGenre { FilmId = 4, GenreId = 10 },
                    new FilmGenre { FilmId = 4, GenreId = 11 },
                    new FilmGenre { FilmId = 5, GenreId = 1 },
                    new FilmGenre { FilmId = 5, GenreId = 9 },
                    new FilmGenre { FilmId = 6, GenreId = 10 },
                    new FilmGenre { FilmId = 6, GenreId = 11 },
                    new FilmGenre { FilmId = 7, GenreId = 3 },
                    new FilmGenre { FilmId = 7, GenreId = 5 },
                    new FilmGenre { FilmId = 8, GenreId = 10 },
                    new FilmGenre { FilmId = 8, GenreId = 3 },
                    new FilmGenre { FilmId = 9, GenreId = 5 },
                    new FilmGenre { FilmId = 9, GenreId = 10 },
                    new FilmGenre { FilmId = 10, GenreId = 3 },
                    new FilmGenre { FilmId = 10, GenreId = 5 },
                    new FilmGenre { FilmId = 11, GenreId = 10 },
                    new FilmGenre { FilmId = 11, GenreId = 4 }
                );
                context.SaveChanges();
            }
        }
    }
}
