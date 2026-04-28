using BooksIO2026.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksIO2026.Data
{
    public class BooksDbContext : DbContext
    {
        /*DbSet para cada entidad que se quiera mapear a la base de datos,
        en este caso solo Author pero se pueden agregar mas DbSet para otras entidades como Book, Publisher, etc.*/
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book> Books { get; set; }

        //sobreescribimos el metodo OnConfiguring para configurar la conexion a la base de datos, en este caso sql server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Cadena de conexion para sql server, se puede cambiar por otra base de datos como mysql o postgresql,
            //pero se debe instalar el paquete correspondiente y cambiar el provider en el OnConfiguring
            //UseSqlServer proviene del paquete Microsoft.EntityFrameworkCore.SqlServer por lo tanto hay que instalar en nuget
            optionsBuilder.UseSqlServer(@"Data Source=.; 
                                          Initial Catalog=BooksIO2026; 
                                          Integrated Security=true; 
                                          Trusted_Connection=True; 
                                          TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //es aplicar la configuracion de forma manual usado para casos donde se quiera aplicar una config especifica
            //modelBuilder.ApplyConfiguration(new AuthoEntityTypeConfiguration());

            //aplica configuraciones de forma automatica, busca todas las clases que implementen IEntityTypeConfiguration y las aplica automaticamente,
            //no es necesario hacer referencia a cada una de ellas de forma manual
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooksDbContext).Assembly);
        }
    }
}
/*Posteriormente procedemos a realizar la migracion en la CONSOLA DEL ADMINISTRADOR DE PAQUETES NUGET 
  (pero antes debemos instalar el paquete Microsoft.EntityFrameworkCore.Tools para poder realizar migraciones)
    add-Migration MigracionInicial
    update-Database
    Automaticamente nos creara la base de datos y las cosas que hemos creado en codigo codefirst
*/

/*
 * Tambien podemos agregar registros sin necesidad de usar la base de datos o escribir codigo previamente, para esop realizamos lo siguiente
 * add-migration AddNewRecordsToAuthorsTable
 * nos generara una nueva migracion en blanco y usando el migrationBuilder.Sql() podemos escribir codigo SQL para agregar registros a la base de datos, 
 * en este caso agregamos 4 autores a la tabla Authors
 * una vez hecho ingresamos el comando update-database para aplicar la migracion y agregar los registros a la base de datos
 */