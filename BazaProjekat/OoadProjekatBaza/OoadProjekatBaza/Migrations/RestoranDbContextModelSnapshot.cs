using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations.Infrastructure;
using OoadProjekatBaza.RestoranBaza.Models;

namespace OoadProjekatBazaMigrations
{
    [ContextType(typeof(RestoranDbContext))]
    partial class RestoranDbContextModelSnapshot : ModelSnapshot
    {
        public override void BuildModel(ModelBuilder builder)
        {
            builder
                .Annotation("ProductVersion", "7.0.0-beta6-13815");

            builder.Entity("OoadProjekatBaza.RestoranBaza.Models.Restoran", b =>
                {
                    b.Property<int>("RestoranId")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("GeoDuzina");

                    b.Property<float>("GeoSirina");

                    b.Property<string>("Naziv");

                    b.Property<string>("Opis");

                    b.Property<float>("Rating");

                    b.Property<byte[]>("Slika")
                        .Annotation("Relational:ColumnType", "image");

                    b.Property<string>("Telefon");

                    b.Property<string>("fourSqaureId");

                    b.Key("RestoranId");
                });
        }
    }
}
