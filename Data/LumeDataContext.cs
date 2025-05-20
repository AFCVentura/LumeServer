using LumeServer.Data.Mappings.Movie;
using LumeServer.Data.Mappings.Question;
using LumeServer.Data.Mappings.User;
using LumeServer.Models.Movie;
using LumeServer.Models.Question;
using LumeServer.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LumeServer.Data
{
    public class LumeDataContext : IdentityDbContext<User>
    {
        #region DbSets dos modelos do filme
        // Filmes
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieKeyword> MovieKeywords { get; set; }
        public DbSet<MovieProductionCompany> MovieProductionCompanies { get; set; }
        public DbSet<MovieProductionCountry> MovieProductionCountries { get; set; }
        public DbSet<MovieSpokenLanguage> MovieSpokenLanguages { get; set; }
        public DbSet<ProductionCompany> ProductionCompanies { get; set; }
        public DbSet<ProductionCountry> ProductionCountries { get; set; }
        public DbSet<SpokenLanguage> SpokenLanguages { get; set; }
        // Usuários
        public DbSet<User> Users { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WatchedList> WatchedLists { get; set; }
        public DbSet<UserDailyProfile> UserDailyProfiles { get; set; }
        public DbSet<UserDailyProfileCluster> UserDailyProfileClusters { get; set; }
        public DbSet<UserGeneralProfileCluster> UserGeneralProfileClusters { get; set; }

        // Questões
        public DbSet<ExtraQuestion> ExtraQuestions { get; set; }
        public DbSet<ExtraAnswer> ExtraAnswers { get; set; }
        public DbSet<ExtraAnswerSpokenLanguage> ExtraAnswerSpokenLanguages { get; set; }
        public DbSet<ExtraAnswerProductionCountry> ExtraAnswerProductionCountries { get; set; }

        public DbSet<ThemeQuestion> ThemeQuestions { get; set; }
        public DbSet<ThemeAnswer> ThemeAnswers { get; set; }
        public DbSet<ThemeAnswerGenre> ThemeAnswerGenres { get; set; }
        public DbSet<ThemeAnswerKeyword> ThemeAnswerKeywords { get; set; }
        public DbSet<UserGeneralProfileProductionCountry> UserGeneralProfileProductionCountries { get; set; }
        public DbSet<UserGeneralProfileSpokenLanguage> UserGeneralProfileSpokenLanguages { get; set; }
        public DbSet<UserDailyProfileProductionCountry> UserDailyProfileProductionCountries { get; set; }
        public DbSet<UserDailyProfileSpokenLanguage> UserDailyProfileSpokenLanguages { get; set; }

        #endregion

        #region Mapeamentos dos modelos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Filmes
            modelBuilder.ApplyConfiguration(new ClusterMap());
            modelBuilder.ApplyConfiguration(new GenreMap());
            modelBuilder.ApplyConfiguration(new KeywordMap());
            modelBuilder.ApplyConfiguration(new MovieMap());
            modelBuilder.ApplyConfiguration(new MovieGenreMap());
            modelBuilder.ApplyConfiguration(new MovieKeywordMap());
            modelBuilder.ApplyConfiguration(new MovieProductionCompanyMap());
            modelBuilder.ApplyConfiguration(new MovieProductionCountryMap());
            modelBuilder.ApplyConfiguration(new MovieSpokenLanguageMap());
            modelBuilder.ApplyConfiguration(new ProductionCompanyMap());
            modelBuilder.ApplyConfiguration(new ProductionCountryMap());
            modelBuilder.ApplyConfiguration(new SpokenLanguageMap());


            // Usuários
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new WishListMap());
            modelBuilder.ApplyConfiguration(new WatchedListMap());
            modelBuilder.ApplyConfiguration(new UserDailyProfileMap());
            modelBuilder.ApplyConfiguration(new UserDailyProfileClusterMap());
            modelBuilder.ApplyConfiguration(new UserGeneralProfileClusterMap());
            modelBuilder.ApplyConfiguration(new UserGeneralProfileProductionCountryMap());
            modelBuilder.ApplyConfiguration(new UserGeneralProfileSpokenLanguageMap());
            modelBuilder.ApplyConfiguration(new UserDailyProfileProductionCountryMap());
            modelBuilder.ApplyConfiguration(new UserDailyProfileSpokenLanguageMap());

            // Questões
            modelBuilder.ApplyConfiguration(new ExtraQuestionMap());
            modelBuilder.ApplyConfiguration(new ExtraAnswerMap());
            modelBuilder.ApplyConfiguration(new ExtraAnswerSpokenLanguageMap());
            modelBuilder.ApplyConfiguration(new ExtraAnswerProductionCountryMap());

            modelBuilder.ApplyConfiguration(new ThemeQuestionMap());
            modelBuilder.ApplyConfiguration(new ThemeAnswerMap());
            modelBuilder.ApplyConfiguration(new ThemeAnswerGenreMap());
            modelBuilder.ApplyConfiguration(new ThemeAnswerKeywordMap());
        }
        #endregion

        public LumeDataContext(DbContextOptions<LumeDataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.LogTo(Console.WriteLine);
        }

    }
}
