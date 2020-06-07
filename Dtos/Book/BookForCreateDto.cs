﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Dtos.Book
{
    public class BookForCreateDto
    {
        public string NameBook { get; set; }

        public int CategoryID { get; set; }

        public int PublisherID { get; set; }
        public string Author { get; set; }
        public string Dimensions { get; set; }

        public string Format { get; set; }
        public DateTime? Date { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int? NumberOfPage { get; set; }

        public string Infomation { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal? OriginalPrice { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal? Price { get; set; }

        public string ImageLink { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public int? QuantityIn { get; set; }
        public bool? Status { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Value must be a positive number")]
        public float? Weight { get; set; }
    }
}
