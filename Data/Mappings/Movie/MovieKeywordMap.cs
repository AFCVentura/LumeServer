﻿using LumeServer.Models.Movie;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LumeServer.Data.Mappings.Movie
{
    public class MovieKeywordMap : IEntityTypeConfiguration<MovieKeyword>
    {
        public void Configure(EntityTypeBuilder<MovieKeyword> builder)
        {
            builder.HasKey(mk => new { mk.MovieId, mk.KeywordId }); // Chave composta

            builder.HasOne(mk => mk.Movie)
                   .WithMany(m => m.MovieKeywords)
                   .HasForeignKey(mk => mk.MovieId);

            builder.HasOne(mk => mk.Keyword)
                   .WithMany(k => k.MovieKeywords)
                   .HasForeignKey(mk => mk.KeywordId);
        }
    }
}
