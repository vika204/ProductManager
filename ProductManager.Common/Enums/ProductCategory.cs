using System.ComponentModel.DataAnnotations;

namespace ProductManager.Common.Enums
{
    public enum ProductCategory
    {
        [Display(Name = "Electronics")]
        Electronics,

        [Display(Name = "Clothing")]
        Clothing,

        [Display(Name = "Books")]
        Books,

        [Display(Name = "Toys")]
        Toys,

        [Display(Name = "Food")]
        Food
    }
}
