using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belt.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}
       
        [Required]
        [MinLength(4, ErrorMessage="Auction item name must be atleast 4 characters")] 
        [Display(Name="Auction Item")]
        public string Name {get;set;}

        [Required]
        [MinLength(11, ErrorMessage="Description must be atleast 11 characters")]
        public string Description {get;set;}
        
        [Required(ErrorMessage="Starting bid must be between $1 and $100,000")]
        [Range(1, 100000)]
        [Display(Name = "Listing Price (USD)")]
        public decimal  StartingBid {get;set;}
        
        [Required]
        [Future]
        [DataType(DataType.Date)]
        [Display(Name = "Auction End Date")] 
        public DateTime Deadline {get;set;}
        
        [Required]
        public int UserId {get;set;}
        public User User {get;set;}
        public Bid HighestBid {get;set;}
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
    public class FutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value !=null)
            {
                string strD = value.ToString();
                DateTime parsedDate = DateTime.Parse(strD);
                Console.WriteLine(parsedDate>DateTime.Now);
                Console.WriteLine($"The parsed Date is: {parsedDate} and its type is {parsedDate.GetType()}");
                Console.WriteLine($"The variable strd is: {strD} and its type is {strD.GetType()}");
                if(parsedDate < DateTime.Now)
                {
                    return new ValidationResult("Must Select a Future date");
                }
            }
            return ValidationResult.Success;
        }
    }
}