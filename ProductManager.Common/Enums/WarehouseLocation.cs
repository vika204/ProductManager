using System.ComponentModel.DataAnnotations;

namespace ProductManager.Common.Enums
{
    public enum WarehouseLocation
    {
        [Display(Name = "Kyiv")]
        Kyiv,

        [Display(Name = "Lviv")]
        Lviv,

        [Display(Name = "Odesa")]
        Odesa,

        [Display(Name = "Kharkiv")]
        Kharkiv,

        [Display(Name = "Dnipro")]
        Dnipro
    }
}
