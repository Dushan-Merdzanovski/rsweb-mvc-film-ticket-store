using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MVCFilmTicketStore.Areas.Identity.Data;
using MVCFilmTicketStore.Models;

namespace MVCFilmTicketStore.Data
{
    public class MVCFilmTicketStoreContext : IdentityDbContext<MVCFilmTicketStoreUser>
    {
        public MVCFilmTicketStoreContext(DbContextOptions<MVCFilmTicketStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Film> Film { get; set; } = default!;

        public DbSet<Director>? Director { get; set; }

        public DbSet<Actor>? Actor { get; set; }

        public DbSet<Genre>? Genre { get; set; }

        public DbSet<Projection>? Projection { get; set; }

        public DbSet<Ticket>? Ticket { get; set; }

        public DbSet<Theater>? Theater { get; set; }

        public DbSet<Seat>? Seat { get; set; }

        public DbSet<Review>? Review { get; set; }

        public DbSet<ActorFilm>? ActorFilm { get; set; }

        public DbSet<FilmGenre>? FilmGenre { get; set; }

        public DbSet<TicketSeat>? TicketSeat { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            // M:1 Film/Director
            builder.Entity<Film>()
               .HasOne<Director>(a => a.Director)
               .WithMany(a => a.Films)
               .HasForeignKey(p => p.DirectorId);

            // M:N Film/Actor
            builder.Entity<ActorFilm>()
                .HasOne<Film>(a => a.Film)
                .WithMany(a => a.ActorFilms) // a.BookGenres are "films"
                .HasForeignKey(p => p.FilmId);

            builder.Entity<ActorFilm>()
                .HasOne<Actor>(p => p.Actor)
                .WithMany(p => p.ActorFilms) // a.ActorFilms are "actors"
                .HasForeignKey(p => p.ActorId);

            // 1:M Film/Review
            builder.Entity<Review>()
                .HasOne<Film>(p => p.Film)
                .WithMany(p => p.Reviews)
                .HasForeignKey(p => p.FilmId);

            // M:N Film/Genre
            builder.Entity<FilmGenre>()
                .HasOne<Film>(a => a.Film)
                .WithMany(a => a.FilmGenres) // a.FilmGenres are "genres"
                .HasForeignKey(p => p.FilmId);

            builder.Entity<FilmGenre>()
                .HasOne<Genre>(p => p.Genre)
                .WithMany(p => p.FilmGenres) // a.FilmGenres are "films"
                .HasForeignKey(p => p.GenreId);

            // 1:M Film/Projection
            builder.Entity<Projection>()
               .HasOne<Film>(a => a.Film)
               .WithMany(a => a.Projections)
               .HasForeignKey(p => p.FilmId);

            // 1:M Projection/Theater
            builder.Entity<Projection>()
               .HasOne<Theater>(a => a.Theater)
               .WithMany(a => a.Projections)
               .HasForeignKey(p => p.TheaterId);

            // 1:M Projection/Ticket
            builder.Entity<Ticket>()
                .HasOne<Projection>(p => p.Projection)
                .WithMany(p => p.Tickets)
                .HasForeignKey(p => p.ProjectionId);

            // M:N Ticket/Seat
            builder.Entity<TicketSeat>()
                .HasOne<Ticket>(a => a.Ticket)
                .WithMany(a => a.TicketSeats) // a.TicketSeats are "tickets"
                .HasForeignKey(p => p.TicketId);

            builder.Entity<TicketSeat>()
                .HasOne<Seat>(p => p.Seat)
                .WithMany(p => p.TicketSeats) // a.TicketSeats are "seats"
                .HasForeignKey(p => p.SeatId);

            // OnDelete(DeleteBehavior.NoAction);
        }*/
    }
}