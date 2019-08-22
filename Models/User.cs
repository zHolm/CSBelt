using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belt.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        
        [Required]
        [MinLength(2)]
        [Display(Name = "First Name")]
        public string FirstName {get;set;}
        
        [Required]
        [MinLength(2)]
        [Display(Name = "Last Name")]
        public string LastName {get;set;}
        
        [Required]
        [MinLength(4)]
        [MaxLength(19)]
        [Display(Name = "Username")]
        public string Email {get;set;}
        
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}
        public decimal Wallet {get;set;} = 1000;
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        //[PLACE NAVIGATION HERE]
        //public List<Comment> CommentsMade {get;set;}
        //public List<Message> MessagesMade {get;set;}
        public List<Bid> BidsMade {get;set;}
        public List<Product> ProductsListed {get;set;}
        
        // Will not be mapped to your users table!
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string Confirm {get;set;}
    }
    public class LoginUser
    {
        // No other fields!
        [Required]
        [Display(Name = "Username")]
        public string EmailLogin {get; set;}
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordLogin { get; set; }
    }
}