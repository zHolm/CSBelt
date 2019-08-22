using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belt.Models
{
    public class Bid
    {
        [Key]
        public int BidId {get;set;}
       
        [Required]
        //CUSTOM VALIDATION FOR NEW BID BEEING GREATER THAN OLD
        public int BidPrice {get;set;}
       
        [Required]
        public int UserId {get;set;}
       
        [Required]
        public int ProductId {get;set;}
       
        public Product Product {get;set;}
        public User User {get;set;}
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}
