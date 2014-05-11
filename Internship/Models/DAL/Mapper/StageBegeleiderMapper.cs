using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;


namespace Internship.Models.DAL.Mapper
{
    public class StageBegeleiderMapper : EntityTypeConfiguration<Stagebegeleider>
    {
        public StageBegeleiderMapper()
        {
            //HasKey(s => s.Emailadres);
            //derest zullen we wel zien zeker
            HasMany(s => s.Preferences).WithMany(o => o.StageBegeleiderPreference).Map(s =>
            {
                s.ToTable("BegeleiderPreferences");
                s.MapLeftKey("StagebegeleiderId");
                s.MapRightKey("OpdrachtId");
            });

            ToTable("Stagebegeleider");
        }

    }
}