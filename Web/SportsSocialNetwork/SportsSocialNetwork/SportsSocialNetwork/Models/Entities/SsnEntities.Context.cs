﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportsSocialNetwork.Models.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SSNEntities : DbContext
    {
        public SSNEntities()
            : base("name=SSNEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategorySport> CategorySports { get; set; }
        public virtual DbSet<Challenge> Challenges { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<FieldImage> FieldImages { get; set; }
        public virtual DbSet<FieldPrice> FieldPrices { get; set; }
        public virtual DbSet<FieldSchedule> FieldSchedules { get; set; }
        public virtual DbSet<FieldType> FieldTypes { get; set; }
        public virtual DbSet<FirebaseToken> FirebaseTokens { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupMember> GroupMembers { get; set; }
        public virtual DbSet<Hobby> Hobbies { get; set; }
        public virtual DbSet<Invitation> Invitations { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsComment> NewsComments { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Participation> Participations { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<PlaceImage> PlaceImages { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostComment> PostComments { get; set; }
        public virtual DbSet<PostImage> PostImages { get; set; }
        public virtual DbSet<PostSport> PostSports { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<TimeBlock> TimeBlocks { get; set; }
        public virtual DbSet<UserInvitation> UserInvitations { get; set; }
    }
}
