//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportsSocialNetwork.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class FirebaseTokenViewModel : SkyWeb.DatVM.Mvc.BaseEntityViewModel<SportsSocialNetwork.Models.Entities.FirebaseToken>
    {
    	
    			public virtual int Id { get; set; }
    			public virtual string UserId { get; set; }
    			public virtual string Token { get; set; }
    	
    	public FirebaseTokenViewModel() : base() { }
    	public FirebaseTokenViewModel(SportsSocialNetwork.Models.Entities.FirebaseToken entity) : base(entity) { }
    
    }
}
