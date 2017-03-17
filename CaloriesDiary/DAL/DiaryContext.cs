using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CaloriesDiary.DAL
{
	public class DiaryContext : DbContext
	{

		public DiaryContext() : base("CaloriesDiaryEntities")
		{
		}

		public DbSet<Food> Foods { get; set; }
		public DbSet<DiaryEntry> DiaryEntris { get; set; }
		public DbSet<Measure> Measures { get; set; }
		public DbSet<Diary> Diaries { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}